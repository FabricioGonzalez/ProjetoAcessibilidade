using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace QuestPDF.Previewer;

internal class CommunicationService
{
    private readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private CommunicationService()
    {
    }

    public static CommunicationService Instance
    {
        get;
    } = new();

    private WebApplication? Application
    {
        get;
        set;
    }

    public event Action<ICollection<PreviewPage>>? OnDocumentRefreshed;

    public Task Start(
        int port
    )
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddLogging(configure: x => x.ClearProviders());
        builder.WebHost.UseKestrel(options: options => options.Limits.MaxRequestBodySize = null);
        Application = builder.Build();

        Application.MapGet(pattern: "ping", handler: HandlePing);
        Application.MapGet(pattern: "version", handler: HandleVersion);
        Application.MapPost(pattern: "update/preview", handler: HandleUpdatePreview);

        return Application.RunAsync(url: $"http://localhost:{port}/");
    }

    public async Task Stop()
    {
        await Application.StopAsync();
        await Application.DisposeAsync();
    }

    private async Task<IResult> HandlePing() =>
        OnDocumentRefreshed == null
            ? Results.StatusCode(statusCode: StatusCodes.Status503ServiceUnavailable)
            : Results.Ok();

    private async Task<IResult> HandleVersion() => Results.Json(data: GetType().Assembly.GetName().Version);

    private async Task<IResult> HandleUpdatePreview(
        HttpRequest request
    )
    {
        var command =
            JsonSerializer.Deserialize<DocumentSnapshot>(json: request.Form[key: "command"]
                , options: JsonSerializerOptions);

        var pages = command
            .Pages
            .Select(selector: page =>
            {
                using var stream = request.Form.Files[name: page.Id].OpenReadStream();
                var picture = SKPicture.Deserialize(stream: stream);

                return new PreviewPage(Picture: picture, Width: page.Width, Height: page.Height);
            })
            .ToList();

        Task.Run(action: () => OnDocumentRefreshed(obj: pages));
        return Results.Ok();
    }
}