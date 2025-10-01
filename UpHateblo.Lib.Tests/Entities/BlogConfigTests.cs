using UpHateblo.Lib.Shared;

namespace UpHateblo.Lib.Tests.Entities;

public class BlogConfigTests
{
    [Fact]
    public void IsCorrectEntryEndPoint()
    {
        var blog = new BlogConfig("kamome283.hatenablog.com", "Kamome283", "");
        var expected =
            new Uri("https://blog.hatena.ne.jp/Kamome283/kamome283.hatenablog.com/atom/entry");
        Assert.Equal(expected, blog.EntryEndPoint);
    }
}