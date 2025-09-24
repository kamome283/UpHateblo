using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Entities;

public class BlogConfigTests
{
    [Theory]
    [InlineData("kamome283.hatenablog.com", "Kamome283",
        "https://blog.hatena.ne.jp/Kamome283/kamome283.hatenablog.com/atom/entry")]
    public void EntryEndPointIsCorrect(string blogId, string userName, string expected)
    {
        var blog = new BlogConfig(blogId, userName, "");
        Assert.Equal(expected, blog.EntryEndPoint.AbsoluteUri);
    }
}