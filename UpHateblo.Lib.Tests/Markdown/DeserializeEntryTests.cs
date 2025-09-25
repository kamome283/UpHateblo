using UpHateblo.Lib.Entities;
using UpHateblo.Lib.Markdown;
using YamlDotNet.Core;

namespace UpHateblo.Lib.Tests.Markdown;

/// <summary>
/// フロントマターの中のYAMLのパースについては
/// [YamlDotNet](https://github.com/aaubry/YamlDotNet)
/// の挙動に従う。
/// フロントマターと本文を区切る部分が自身の処理なので
/// この部分を重点的にテストする。
/// </summary>
public class DeserializeEntryTests
{
    public static object?[][] TestCases =>
    [
        [
            new MaybeEntry("テスト", ["Test"], DateTime.Parse("2025-01-09T19:07:00"), "これはテストです", null,
                null, null),
            """
            ---
            Title: テスト
            Category: 
              - Test
            Date: 2025-01-09T19:07:00+09:00
            ---
            これはテストです
            """
        ]
    ];

    /// <param name="expected">nullの場合はフロントマターのパースに失敗したケースを想定</param>
    /// <param name="content">パースする文字列</param>
    [Theory, MemberData(nameof(TestCases))]
    public void ItCanDeserializeAsSupposed(MaybeEntry? expected, string content)
    {
        if (expected is null)
        {
            Assert.Throws<YamlException>(() => { DeserializeEntry.Run(content); });
        }
        else
        {
            var actual = DeserializeEntry.Run(content);
            Assert.Equal(expected, actual);
        }
    }

    /// <summary>
    /// フロントマターのデシリアライズを同時に行っても問題ないかを簡易的にテスト
    /// </summary>
    [Fact]
    public async Task ItWorksInMultiThreadEnv()
    {
        var firstTestCase = TestCases[0];
        var expected = firstTestCase[0] as MaybeEntry;
        var content = firstTestCase[1] as string ?? throw new NullReferenceException();
        await Assert.AllAsync(Enumerable.Range(1, 100),
            async _ => { await Task.Run(() => ItCanDeserializeAsSupposed(expected, content)); });
    }
}