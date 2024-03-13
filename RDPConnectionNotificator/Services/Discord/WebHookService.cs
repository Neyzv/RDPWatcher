using System.Text;
using RDPConnectionNotificator.Config;

namespace RDPConnectionNotificator.Services.Discord;

public sealed class WebHookService : IDisposable
{
    private const string JsonHeader = "application/json";
    private const string MessageFormat = "{{\"embeds\":[{{\"title\": \"{0}\", \"color\": {1}}}]}}";
    
    private readonly string _webHookUrl;
    private readonly HttpClient _client = new();

    public WebHookService(AppConfig config) =>
        _webHookUrl = config.WebHookUrl;

    public async Task SendAsync(string message, int color)
    {
        var content = new StringContent(string.Format(MessageFormat, message, color), Encoding.UTF8, JsonHeader);

        await _client.PostAsync(_webHookUrl, content)
            .ConfigureAwait(false);
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}