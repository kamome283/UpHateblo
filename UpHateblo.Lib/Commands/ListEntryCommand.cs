using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Commands;

public static class ListEntryCommand
{
    public static async Task Run(
        HttpClient httpClient,
        BlogConfig blog,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        var httpContent =
            CommandHelper.GenHatenaContent(null, blog, null, wsseNonce, wsseDateTime);
        var request = new HttpRequestMessage(HttpMethod.Get, blog.EntryEndPoint);
        foreach (var header in httpContent.Headers)
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        // Debug
        var res = await httpClient.SendAsync(request);
        var content = await res.Content.ReadAsStringAsync();
        res.EnsureSuccessStatusCode();
    }
}