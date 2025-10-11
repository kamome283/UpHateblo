using System.Management.Automation;
using UpHateblo.Lib.Entry.Parse;
using UpHateblo.Lib.Entry.Shared;

namespace UpHateblo.Pwsh.Cmdlets;

[Cmdlet("ConvertTo", "HatebloEntry"), OutputType(typeof(MaybeEntry))]
public class ConvertToHatebloEntry : Cmdlet
{
    private MaybeEntry _propBasedMaybeEntry;
    [Parameter(ValueFromPipeline = true)] public string[] Body { get; set; }
    [Parameter] public string EntryId { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public string[] Category { get; set; }
    [Parameter] public string Content { get; set; }
    [Parameter] public string CustomPath { get; set; }
    [Parameter] public DateTime Date { get; set; }
    [Parameter] public bool Draft { get; set; }
    [Parameter] public bool Preview { get; set; }
    [Parameter] public DateTime Published { get; set; }
    [Parameter] public string ContentType { get; set; }
    [Parameter] public string PreviewUrl { get; set; }

    protected override void BeginProcessing()
    {
        _propBasedMaybeEntry = MaybeEntry.Construct(
            entryId: EntryId,
            title: Title,
            category: Category,
            content: Content,
            customPath: CustomPath,
            date: Date,
            draft: Draft,
            preview: Preview,
            published: Published,
            contentType: ContentType,
            previewUrl: PreviewUrl
        );
    }

    protected override void ProcessRecord()
    {
        foreach (var body in Body)
        {
            try
            {
                var parsed = ParseEntry.Run(body);
                var merged = _propBasedMaybeEntry.Merge(parsed);
                WriteObject(merged);
            }
            catch (Exception ex)
            {
                WriteError(
                    new ErrorRecord(ex, ex.GetType().Name, ErrorCategory.NotSpecified,
                        (_propBasedMaybeEntry, body)));
            }
        }
    }

    protected override void EndProcessing()
    {
        if (Body.Length == 0) WriteObject(_propBasedMaybeEntry);
    }
}