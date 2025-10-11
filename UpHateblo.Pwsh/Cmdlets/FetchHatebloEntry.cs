using System.Management.Automation;
using UpHateblo.Lib.Entry.Fetch;
using UpHateblo.Lib.Entry.Shared;

namespace UpHateblo.Pwsh.Cmdlets;

[Cmdlet("Fetch", "HatebloEntry"), OutputType(typeof(FetchedEntry))]
public class FetchHatebloEntry : WebRequestingCmdletBase
{
    private AsyncTaskHandler<string, FetchedEntry, Exception> _taskHandler;
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
    public string[] EntryId { get; set; }
    [Parameter] public int Parallel { get; set; } = 1;

    protected override void BeginProcessing()
    {
        base.BeginProcessing();
        _taskHandler = new AsyncTaskHandler<string, FetchedEntry, Exception>()
            {
                ParallelOptions = new ParallelOptions()
                {
                    CancellationToken = CancellationToken,
                    MaxDegreeOfParallelism = Parallel
                },
                Body = async (entryId, token) =>
                    await FetchEntry.Run(HttpClient, BlogConfig, entryId, token)
            }
            .StartProcessing();
    }

    protected override void ProcessRecord()
    {
        foreach (var entryId in EntryId)
        {
            _taskHandler.Write(entryId);
        }
    }

    protected override void EndProcessing()
    {
        _taskHandler.Complete();
        foreach (var (entryId, fetched, ex) in _taskHandler.BlockingOut)
        {
            if (ex is not null)
                WriteError(
                    new ErrorRecord(ex, ex.GetType().Name, ErrorCategory.NotSpecified, entryId));
            else WriteObject(fetched);
        }
    }
}