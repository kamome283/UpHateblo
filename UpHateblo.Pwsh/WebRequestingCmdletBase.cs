using System.Management.Automation;
using UpHateblo.Lib.Shared;

namespace UpHateblo.Pwsh;

public abstract class WebRequestingCmdletBase : Cmdlet, IDisposable
{
    [Parameter] public string BlogId { get; set; }
    [Parameter, Credential] public PSCredential Credential { get; set; }
    [Parameter] public HttpClient HttpClient { get; set; }
    protected BlogConfig BlogConfig { get; set; }

    protected CancellationTokenSource CancellationTokenSource { get; } = new();
    protected CancellationToken CancellationToken { get; set; }
    protected bool HasConstructedHttpClient { get; set; }

    public void Dispose()
    {
        CancellationTokenSource?.Cancel();
        CancellationTokenSource?.Dispose();
        if (HasConstructedHttpClient) HttpClient?.Dispose();
        GC.SuppressFinalize(this);
    }

    protected override void BeginProcessing()
    {
        if (HttpClient is null)
        {
            HttpClient = new HttpClient();
            HasConstructedHttpClient = true;
        }

        if (BlogId is null) throw new ArgumentNullException(nameof(BlogId));
        if (Credential is null) throw new ArgumentNullException(nameof(Credential));
        BlogConfig = new BlogConfig(BlogId, Credential.UserName,
            Credential.GetNetworkCredential().Password);

        CancellationToken = CancellationTokenSource.Token;
    }

    protected override void StopProcessing()
    {
        CancellationTokenSource.Cancel();
    }
}