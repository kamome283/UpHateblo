using System.Management.Automation;

namespace UpHateblo.Pwsh.Cmdlets;

[Cmdlet("Hello", "World")]
public class HelloWorld : Cmdlet
{
    protected override void BeginProcessing()
    {
        WriteObject("Hello World from BeginProcessing!");
    }
}