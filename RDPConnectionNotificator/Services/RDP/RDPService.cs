using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using RDPConnectionNotificator.Services.Discord;

namespace RDPConnectionNotificator.Services.RDP;

public sealed class RdpService
{
    private const short RdpLogOnEventId = 21;
    private const short RdpReLogOnEventId = 25;
    private const short RdpLogOffEventId = 24;

    private readonly EventLogWatcher _eventLog = new("Microsoft-Windows-TerminalServices-LocalSessionManager/Operational");
    private readonly WebHookService _webHookService;

    public RdpService(WebHookService webHookService)
    {
        _webHookService = webHookService;
        _eventLog.EventRecordWritten += OnEntryWriten;
    }

    public void Start() =>
        _eventLog.Enabled = true;

    public void Stop() =>
        _eventLog.Enabled = false;

    private void OnEntryWriten(object? sender, EventRecordWrittenEventArgs e)
    {
        _ = (e.EventRecord.Id switch
        {
            RdpLogOnEventId or RdpReLogOnEventId => _webHookService.SendAsync("Y a kelkun ki é co nez kter.", 16711680),
            RdpLogOffEventId => _webHookService.SendAsync("A pu paire sonne.", 523002),
            _ => Task.CompletedTask
        }).ConfigureAwait(false);
    }
}