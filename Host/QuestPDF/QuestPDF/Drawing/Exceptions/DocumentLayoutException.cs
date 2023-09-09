using System;

namespace QuestPDF.Drawing.Exceptions
{
    public class DocumentLayoutException : Exception
    {
        public DocumentLayoutException()
        {
        }

        public DocumentLayoutException(
            string message
        ) : base(message: message)
        {
        }

        public DocumentLayoutException(
            string message
            , Exception inner
        ) : base(message: message, innerException: inner)
        {
        }

        public string ElementTrace
        {
            get;
            set;
        }
    }
}