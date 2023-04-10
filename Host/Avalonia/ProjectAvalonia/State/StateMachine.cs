using System;
using System.Collections.Generic;

namespace ProjectAvalonia.State;

/// <summary>
///     StateMachine - api based on: https://github.com/dotnet-state-machine/stateless
/// </summary>
public class StateMachine<TState, TTrigger>
    where TTrigger : Enum
    where TState : struct, Enum
{
    public delegate void OnTransitionedDelegate(
        TState from
        , TState to
    );

    private readonly Dictionary<TState, StateContext> _states;
    private StateContext _currentState;
    private OnTransitionedDelegate? _onTransitioned;

    public StateMachine(
        TState initialState
    )
    {
        _states = new Dictionary<TState, StateContext>();

        RegisterStates();

        _currentState = Configure(state: initialState);
    }

    public TState State => _currentState.StateId;

    public bool IsInState(
        TState state
    ) => IsAncestorOf(state: _currentState.StateId, parent: state);

    private void RegisterStates()
    {
        foreach (var state in Enum.GetValues<TState>())
        {
            _states.Add(key: state, value: new StateContext(owner: this, state: state));
        }
    }

    public StateMachine<TState, TTrigger> OnTransitioned(
        OnTransitionedDelegate onTransitioned
    )
    {
        _onTransitioned = onTransitioned;

        return this;
    }

    private bool IsAncestorOf(
        TState state
        , TState parent
    )
    {
        if (_states.TryGetValue(key: state, value: out var value))
        {
            var current = value;

            while (true)
            {
                if (current.StateId.Equals(obj: parent))
                {
                    return true;
                }

                if (current.Parent is not null)
                {
                    current = current.Parent;
                }
                else
                {
                    return false;
                }
            }
        }

        return false;
    }

    public StateContext Configure(
        TState state
    ) => _states[key: state];

    public void Fire(
        TTrigger trigger
    )
    {
        _currentState.Process(trigger: trigger);

        if (_currentState.CanTransit(trigger: trigger))
        {
            var destination = _currentState.GetDestination(trigger: trigger);

            if (_states.TryGetValue(key: destination, value: out var value) && value.Parent is { } parent &&
                !IsInState(state: parent.StateId))
            {
                Goto(state: parent.StateId);
            }

            Goto(state: destination);
        }
        else if (_currentState.Parent is not null && _currentState.Parent.CanTransit(trigger: trigger))
        {
            Goto(state: _currentState.Parent.StateId, exit: true, enter: false);
            Goto(state: _currentState.GetDestination(trigger: trigger));
        }
    }

    public void Start() => Enter();

    private void Enter()
    {
        _currentState.Enter();

        if (_currentState.InitialTransitionTo is { } state)
        {
            Goto(state: state);
        }
    }

    private void Goto(
        TState state
        , bool exit = true
        , bool enter = true
    )
    {
        if (_states.ContainsKey(key: state))
        {
            if (exit && !IsAncestorOf(state: state, parent: _currentState.StateId))
            {
                _currentState.Exit();
            }

            var old = _currentState.StateId;

            _currentState = _states[key: state];

            _onTransitioned?.Invoke(@from: old, to: _currentState.StateId);

            if (enter)
            {
                Enter();
            }
        }
    }

    public class StateContext
    {
        private readonly List<Action> _entryActions;
        private readonly List<Action> _exitActions;
        private readonly StateMachine<TState, TTrigger> _owner;
        private readonly Dictionary<TTrigger, TState> _permittedTransitions;
        private readonly Dictionary<TTrigger, List<Action>> _triggerActions;

        public StateContext(
            StateMachine<TState, TTrigger> owner
            , TState state
        )
        {
            _owner = owner;
            StateId = state;

            _entryActions = new List<Action>();
            _exitActions = new List<Action>();
            _triggerActions = new Dictionary<TTrigger, List<Action>>();
            _permittedTransitions = new Dictionary<TTrigger, TState>();
        }

        public TState StateId
        {
            get;
        }

        public StateContext? Parent
        {
            get;
            private set;
        }

        internal TState? InitialTransitionTo
        {
            get;
            private set;
        }

        public StateContext InitialTransition(
            TState? state
        )
        {
            InitialTransitionTo = state;

            return this;
        }

        public StateContext SubstateOf(
            TState parent
        )
        {
            Parent = _owner._states[key: parent];

            return this;
        }

        public StateContext Permit(
            TTrigger trigger
            , TState state
        )
        {
            if (StateId.Equals(obj: state))
            {
                throw new InvalidOperationException(message: "Configuring state re-entry is not allowed.");
            }

            _permittedTransitions[key: trigger] = state;

            return this;
        }

        public StateContext OnEntry(
            Action action
        )
        {
            _entryActions.Add(item: action);

            return this;
        }

        public StateContext Custom(
            Func<StateContext, StateContext> custom
        ) => custom(arg: this);

        public StateContext OnTrigger(
            TTrigger trigger
            , Action action
        )
        {
            if (_triggerActions.TryGetValue(key: trigger, value: out var t))
            {
                t.Add(item: action);
            }
            else
            {
                _triggerActions.Add(key: trigger, value: new List<Action> { action });
            }

            return this;
        }

        public StateContext OnExit(
            Action action
        )
        {
            _exitActions.Add(item: action);

            return this;
        }

        internal void Enter()
        {
            foreach (var action in _entryActions)
            {
                action();
            }
        }

        internal void Exit()
        {
            foreach (var action in _exitActions)
            {
                action();
            }
        }

        internal void Process(
            TTrigger trigger
        )
        {
            if (_triggerActions.TryGetValue(key: trigger, value: out var value) && value is { } actions)
            {
                foreach (var action in actions)
                {
                    action();
                }
            }

            if (Parent is not null)
            {
                Parent.Process(trigger: trigger);
            }
        }

        internal bool CanTransit(
            TTrigger trigger
        ) => _permittedTransitions.ContainsKey(key: trigger);

        internal TState GetDestination(
            TTrigger trigger
        ) => _permittedTransitions[key: trigger];
    }
}