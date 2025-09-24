using UpHateblo.Lib.Entities;
using UpHateblo.Lib.Http;
using UpHateblo.Lib.Schema;

namespace UpHateblo.Lib.Commands;

public class PostEntry(HttpClient client, Entry entry)
{
    private BlogConfig Blog => entry.Blog;
    private EntryHeader Header => entry.Header;
    private string Content => entry.Content;

    public async Task Run()
    {
        var xml = new PostEntrySchema().Serialize(entry);
        var wsse =
            new Wsse(Blog.Username, Blog.Password, Guid.NewGuid().ToString(), DateTime.Now);
        var httpContent = new HatenaContent(xml, wsse);
        var res = await client.PostAsync(Blog.EntryEndPoint, httpContent);
        res.EnsureSuccessStatusCode();
    }
}