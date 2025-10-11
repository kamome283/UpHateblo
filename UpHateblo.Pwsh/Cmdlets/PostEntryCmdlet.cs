using System.ComponentModel.DataAnnotations;
using System.Management.Automation;
using UpHateblo.Lib.Entry.Post;
using UpHateblo.Lib.Entry.Shared;

namespace UpHateblo.Pwsh.Cmdlets;

[Cmdlet("Post", "Entry"), OutputType(typeof(FetchedEntry))]
public class PostEntryCmdlet : WebRequestingCmdletBase
{
    private AsyncTaskHandler<PostableEntry, FetchedEntry, Exception> _taskHandler;
    [Parameter(ValueFromPipeline = true, Mandatory = true)]
    public MaybeEntry[] Entry { get; set; } = [];
    [Parameter] public int Parallel { get; set; } = 1;

    protected override void BeginProcessing()
    {
        base.BeginProcessing();
        _taskHandler = new AsyncTaskHandler<PostableEntry, FetchedEntry, Exception>
            {
                ParallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = Parallel,
                    CancellationToken = CancellationToken
                },
                Body = async (entry, token) =>
                    await PostEntry.Run(HttpClient, BlogConfig, entry, token)
            }
            .StartProcessing();
    }

    protected override void ProcessRecord()
    {
        foreach (var entry in Entry)
        {
            try
            {
                var postable = (PostableEntry)entry;
                _taskHandler.Write(postable);
            }
            catch (ValidationException ex)
            {
                WriteError(
                    new ErrorRecord(ex, ex.GetType().Name, ErrorCategory.InvalidData, entry));
            }
        }
    }

    protected override void EndProcessing()
    {
        _taskHandler.Complete();
        foreach (var (postable, fetched, ex) in _taskHandler.BlockingOut)
        {
            if (ex is not null)
                WriteError(new ErrorRecord(ex, ex.GetType().Name, ErrorCategory.InvalidOperation,
                    postable));
            else WriteObject(fetched);
        }
    }
}