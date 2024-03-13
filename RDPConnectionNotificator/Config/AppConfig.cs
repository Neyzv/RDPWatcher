namespace RDPConnectionNotificator.Config;

public sealed class AppConfig
{
    public const string ConfigPath = "config.json";

    public string WebHookUrl { get; set; } = "YourWebHookUrl";
}