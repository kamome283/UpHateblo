using System.Text.RegularExpressions;
using UpHateblo.Lib.Entities;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace UpHateblo.Lib.Commands;

// TODO: Source GeneratorベースのYAMLパーサーを導入
public static class WriteEntryCommand
{
    private static readonly IDeserializer YamlDeserializer =
        new DeserializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

    private static readonly Regex FrontMatterRegex =
        new("""
            ^---
            (.*)
            ---
            (.*)$
            """, RegexOptions.Singleline);

    public static MaybeEntry Run(string content)
    {
        var (maybeFrontMatter, body) = content.Separate();
        var frontMatter = maybeFrontMatter is not null
            ? maybeFrontMatter.ParseFrontMatter()
            : new MaybeEntry(null, null, null, null, null, null, null, null, null, null);
        return frontMatter with { Content = body };
    }

    private static (string? maybeFrontMatter, string body) Separate(this string content)
    {
        var match = FrontMatterRegex.Match(content);
        return match.Success
            ? (match.Groups[1].Value, match.Groups[2].Value)
            : (null, content);
    }

    private static MaybeEntry ParseFrontMatter(this string frontMatter)
    {
        return YamlDeserializer.Deserialize<MaybeEntry>(frontMatter);
    }
}