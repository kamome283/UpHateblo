using System.Management.Automation;
using UpHateblo.Lib.Entry.Edit;
using UpHateblo.Lib.Entry.Shared;

namespace UpHateblo.Pwsh.Cmdlets;

[Cmdlet("Edit", "Entry"), OutputType(typeof(FetchedEntry))]
public class EditEntryCmdlet : WebRequestingCmdletBase
{
    private AsyncTaskHandler<EditableEntry, FetchedEntry, Exception> _taskHandler;
    [Parameter(ValueFromPipeline = true, Mandatory = true)]
    public MaybeEntry[] Entry { get; set; } = [];
    [Parameter] public int Parallel { get; set; } = 1;

    protected override void BeginProcessing()
    {
        base.BeginProcessing();
        _taskHandler = new AsyncTaskHandler<EditableEntry, FetchedEntry, Exception>
            {
                ParallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = Parallel,
                    CancellationToken = CancellationToken
                },
                Body = async (entry, token) =>
                    await EditEntry.Run(HttpClient, BlogConfig, entry, token)
            }
            .StartProcessing();
    }

    protected override void ProcessRecord()
    {
        foreach (var entry in Entry)
        {
            try
            {
                var editable = (EditableEntry)entry;
                _taskHandler.Write(editable);
            }
            catch (Exception ex)
            {
                WriteError(
                    new ErrorRecord(ex, ex.GetType().Name, ErrorCategory.NotSpecified, entry));
            }
        }
    }

    protected override void EndProcessing()
    {
        _taskHandler.Complete();
        foreach (var (editable, fetched, ex) in _taskHandler.BlockingOut)
        {
            if (ex is not null)
                WriteError(
                    new ErrorRecord(ex, ex.GetType().Name, ErrorCategory.NotSpecified, editable));
            else WriteObject(fetched);
        }
    }
}