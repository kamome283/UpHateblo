using System.Management.Automation;
using UpHateblo.Lib.Entry.Shared;
using UpHateblo.Lib.Entry.Stringify;

namespace UpHateblo.Pwsh.Cmdlets;

[Cmdlet("ConvertFrom", "HatebloEntry"), OutputType(typeof(string))]
public class ConvertFromHatebloEntry : Cmdlet
{
    [Parameter(ValueFromPipeline = true, Mandatory = true)]
    public MaybeEntry[] Entry { get; set; }

    protected override void ProcessRecord()
    {
        foreach (var entry in Entry)
        {
            try
            {
                var body = StringifyEntry.Run(entry);
                WriteObject(body);
            }
            catch (Exception ex)
            {
                WriteError(
                    new ErrorRecord(ex, ex.GetType().Name, ErrorCategory.NotSpecified, entry));
            }
        }
    }
}