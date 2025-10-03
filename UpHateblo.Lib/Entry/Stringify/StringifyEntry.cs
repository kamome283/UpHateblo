using System.Diagnostics.CodeAnalysis;
using System.Text;
using UpHateblo.Lib.Entry.Shared;
using VYaml.Annotations;
using VYaml.Serialization;

namespace UpHateblo.Lib.Entry.Stringify;

public static class StringifyEntry
{
    /// <remarks>ParseEntryでも同じコードを書いているけど2箇所なら共通化しなくてもいいや</remarks>
    private static readonly YamlSerializerOptions YamlSerializerOptions =
        new() { NamingConvention = NamingConvention.UpperCamelCase };

    public static string Run(MaybeEntry entry)
    {
        var frontMatter = entry.FrontMatter();
        var content = entry.Content;
        var components = new[] { frontMatter, content }.Where(x => x is not null).Select(x => x!);
        return string.Join(Environment.NewLine, components);
    }

    /// <remarks>
    /// VYamlはnullのプロパティを無視せずに、
    /// ex.) Title: null
    /// のように書き込む。
    /// この挙動を無効化するために、動的に作成した辞書をシリアライズしている。
    /// </remarks>
    private static string? FrontMatter(this MaybeEntry entry)
    {
        var header = entry with { Content = null };

        var props = header.GetProps();
        if (props.Count == 0) return null;

        var yamlBytes = YamlSerializer.Serialize(header.GetProps(), YamlSerializerOptions);
        var yaml = Encoding.UTF8.GetString(yamlBytes.Span);
        return $"""
                ---
                {yaml}
                ---
                """;
    }

    [RequiresDynamicCode("Current implementation uses reflection to get properties.")]
    private static Dictionary<string, object> GetProps(this MaybeEntry entry)
    {
        Dictionary<string, object> result = new();
        var props = typeof(MaybeEntry).GetProperties();
        foreach (var prop in props)
        {
            var value = prop.GetValue(entry);
            if (value is null) continue;
            // VYamlはHashSet<string>をプリミティブにサポートしていないので、string[]に変換
            var maybeSet = value as HashSet<string>;
            result[prop.Name] = maybeSet?.ToArray() ?? value;
        }

        return result;
    }
}