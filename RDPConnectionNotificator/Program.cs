using System.ServiceProcess;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using RDPConnectionNotificator.Config;
using RDPConnectionNotificator.Services.Discord;
using RDPConnectionNotificator.Services.RDP;

namespace RDPConnectionNotificator;

public sealed class Program(RdpService rdpService) : ServiceBase
{
    public static async Task Main()
    {
        if (!File.Exists(AppConfig.ConfigPath))
        {
            File.Create(AppConfig.ConfigPath).Close();
            
            var content = JsonSerializer.Serialize(new AppConfig(),
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            
            await File.WriteAllTextAsync(AppConfig.ConfigPath, content)
                .ConfigureAwait(false);
        }
        
        var config = JsonSerializer.Deserialize<AppConfig>(
            await File.ReadAllTextAsync(AppConfig.ConfigPath)
                .ConfigureAwait(false)
        )!;
        
        var provider = new ServiceCollection()
            .AddSingleton<Program>()
            .AddSingleton(config)
            .AddSingleton<WebHookService>()
            .AddSingleton<RdpService>()
            .BuildServiceProvider();
        
        Run(provider.GetRequiredService<Program>());

        await Task.Delay(Timeout.Infinite)
            .ConfigureAwait(false);
    }

    protected override void OnStart(string[] args) =>
        rdpService.Start();

    protected override void OnStop() =>
        rdpService.Stop();
}