using System.Management.Automation;
using UpHateblo.Lib.Entry.Delete;

namespace UpHateblo.Pwsh;

[Cmdlet("Delete", "HatebloEntry")]
public class DeleteHatebloEntry : WebRequestingCmdletBase
{
    private AsyncTaskHandler<string, object, Exception> _taskHandler;
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
    public string[] EntryId { get; set; }
    [Parameter] public int Parallel { get; set; } = 1;

    protected override void BeginProcessing()
    {
        base.BeginProcessing();
        _taskHandler = new AsyncTaskHandler<string, object, Exception>()
        {
            ParallelOptions = new ParallelOptions()
            {
                CancellationToken = CancellationToken,
                MaxDegreeOfParallelism = Parallel
            },
            Body = async (entryId, token) =>
            {
                await DeleteEntry.Run(HttpClient, BlogConfig, entryId, token);
                return null;
            }
        };
        _taskHandler.StartProcessing();
    }

    protected override void ProcessRecord()
    {
        foreach (var entryId in EntryId)
        {
            _taskHandler.Add(entryId);
        }
    }

    protected override void EndProcessing()
    {
        _taskHandler.CompleteAdding();
        foreach (var (entryId, _, ex) in _taskHandler.BlockingOutEnumerable)
        {
            if (ex is not null)
                WriteError(
                    new ErrorRecord(ex, ex.GetType().Name, ErrorCategory.NotSpecified, entryId));
        }
    }
}