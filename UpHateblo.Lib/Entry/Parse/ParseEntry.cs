using System.Text;
using System.Text.RegularExpressions;
using UpHateblo.Lib.Entry.Shared;
using VYaml.Annotations;
using VYaml.Serialization;

namespace UpHateblo.Lib.Entry.Parse;

public static class ParseEntry
{
    /// <remarks>StringifyEntryでも同じコードを書いているけど2箇所なら共通化しなくてもいいや</remarks>
    private static readonly YamlSerializerOptions YamlSerializerOptions =
        new() { NamingConvention = NamingConvention.UpperCamelCase };

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
            : new MaybeEntry(null, null, null, null, null, null, null, null, null, null, null);
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
        var bytes = Encoding.UTF8.GetBytes(frontMatter);
        return YamlSerializer.Deserialize<MaybeEntry>(bytes, YamlSerializerOptions);
    }
}