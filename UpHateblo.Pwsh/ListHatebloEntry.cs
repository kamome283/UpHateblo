using System.Management.Automation;
using UpHateblo.Lib.Entry.List;
using UpHateblo.Lib.Entry.Shared;

namespace UpHateblo.Pwsh;

[Cmdlet("List", "HatebloEntry"), OutputType(typeof(FetchedEntry))]
public class ListHatebloEntry : WebRequestingCmdletBase
{
    protected override void EndProcessing()
    {
        try
        {
            var entries = ListEntry.Run(HttpClient, BlogConfig);
            foreach (var entry in entries.ToBlockingEnumerable())
            {
                WriteObject(entry);
            }
        }
        catch (Exception ex)
        {
            WriteError(new ErrorRecord(ex, ex.GetType().Name, ErrorCategory.NotSpecified, null));
        }
    }
}