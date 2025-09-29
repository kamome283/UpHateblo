using System.Xml.Linq;
using UpHateblo.Lib.Schema;

namespace UpHateblo.Lib.Tests.Schema;

public class FetchedEntrySchemaTest
{
    private static readonly string ResponseBody =
        """
        <?xml version="1.0" encoding="utf-8" standalone="no"?>
        <feed xmlns="http://www.w3.org/2005/Atom" xmlns:app="http://www.w3.org/2007/app">

            <link href="https://blog.hatena.ne.jp/Kamome283/kamome283.hatenablog.com/atom/entry"
                  rel="first"/>


            <title>Kamome283</title>


            <link href="https://kamome283.hatenablog.com/" rel="alternate"/>

            <updated>2025-09-23T21:29:00+09:00</updated>

            <author>

                <name>Kamome283</name>

            </author>

            <generator uri="https://blog.hatena.ne.jp/" version="e106732cc5d5b9efb3b4458b2dbb2a">
                Hatena::Blog
            </generator>

            <id>hatenablog://blog/8454420450069751439</id>


            <entry>

                <id>tag:blog.hatena.ne.jp,2013:blog-Kamome283-8454420450069751439-6802888565258956140</id>

                <link href="https://blog.hatena.ne.jp/Kamome283/kamome283.hatenablog.com/atom/entry/6802888565258956140"
                      rel="edit"/>

                <link href="https://kamome283.hatenablog.com/entry/01998dd0-29f8-7628-9c70-f35e901318b7"
                      rel="alternate" type="text/html"/>

                <author>
                    <name>Kamome283</name>
                </author>

                <title>テスト</title>

                <updated>2025-09-23T21:29:00+09:00</updated>

                <published>2025-09-28T09:54:14+09:00</published>

                <app:edited>2025-09-28T09:54:14+09:00</app:edited>

                <summary type="text">うまくいっているといいな 複数行</summary>

                <content type="text/x-markdown">うまくいっているといいな
                    複数行
                </content>

                <hatena:formatted-content xmlns:hatena="http://www.hatena.ne.jp/info/xmlns#"
                                          type="text/html">&lt;p&gt;うまくいっているといいな
                    複数行&lt;/p&gt;
                </hatena:formatted-content>

                <category term="Test"/>

                <app:control>

                    <app:draft>yes</app:draft>

                    <app:preview>no</app:preview>

                </app:control>

            </entry>


            <entry>

                <id>tag:blog.hatena.ne.jp,2013:blog-Kamome283-8454420450069751439-6802888565257240236</id>

                <link href="https://blog.hatena.ne.jp/Kamome283/kamome283.hatenablog.com/atom/entry/6802888565257240236"
                      rel="edit"/>

                <link href="https://kamome283.hatenablog.com/entry/01998a00-4a65-7eb1-9d79-ddb822fc17b5"
                      rel="alternate" type="text/html"/>

                <link href="https://kamome283.hatenablog.com/draft/entry/qESFitds1djAmaB3ZhwO8qCWhT4"
                      rel="preview" type="text/html"/>

                <author>
                    <name>Kamome283</name>
                </author>

                <title>PushCommandTest</title>

                <updated>2023-01-01T00:00:00+09:00</updated>

                <published>2025-09-27T15:55:17+09:00</published>

                <app:edited>2025-09-27T16:08:19+09:00</app:edited>

                <summary type="text">This is test content of Push command. This is the second line of the
                    test content.
                </summary>

                <content type="text/x-markdown">This is test content of Push command.
                    This is the second line of the test content.
                </content>

                <hatena:formatted-content xmlns:hatena="http://www.hatena.ne.jp/info/xmlns#"
                                          type="text/html">&lt;p&gt;This is test content of Push command.
                    This is the second line of the test content.&lt;/p&gt;
                </hatena:formatted-content>

                <category term="Push"/>

                <category term="Test"/>

                <app:control>

                    <app:draft>yes</app:draft>

                    <app:preview>yes</app:preview>

                </app:control>

            </entry>


            <entry>

                <id>tag:blog.hatena.ne.jp,2013:blog-Kamome283-8454420450069751439-6802888565247726606</id>

                <link href="https://blog.hatena.ne.jp/Kamome283/kamome283.hatenablog.com/atom/entry/6802888565247726606"
                      rel="edit"/>

                <link href="https://kamome283.hatenablog.com/entry/created-read-file-lines-generator"
                      rel="alternate" type="text/html"/>

                <author>
                    <name>Kamome283</name>
                </author>

                <title>テキストファイルから列挙型を生成する C# ソースジェネレーターを作ってみた</title>

                <updated>2025-09-22T10:07:59+09:00</updated>

                <published>2025-09-22T10:07:59+09:00</published>

                <app:edited>2025-09-22T16:31:26+09:00</app:edited>

                <summary type="text">概要 テキストファイルから列挙型を生成する自作の C# 向けソースジェレレーターのご紹介
                    ソースジェネレーターを自作した感想 自作ソースジェレレーターのご紹介
                    https://github.com/kamome283/ReadFileLinesEnumGenerator 名前は …
                </summary>

                <content type="text/x-markdown"># 概要
                    * テキストファイルから列挙型を生成する自作の C# 向けソースジェレレーターのご紹介
                    * ソースジェネレーターを自作した感想

                    # 自作ソースジェレレーターのご紹介
                    https://github.com/kamome283/ReadFileLinesEnumGenerator

                    名前は `ReadFileLinesEnumGenerator` です。もう少し英語力があれば、もっとマシな名前をつけれたのではないかと思うところはあります。
                    機能としては、 `.csproj` 内で `AdditionalFiles` プロパティに指定された末尾が `.enum.txt`
                    で終わるファイルを読み込み、その各行を列挙型の識別子として定義する列挙型を自動生成するものです。
                    以下は GitHub リポジトリの README から引用したサンプルコードです。使い方のイメージとして参考になればと思います。
                    ```xml
                    &lt;Project&gt;

                    &lt;!-- Add a reference to the source generator project --&gt;
                    &lt;ItemGroup&gt;
                    &lt;PackageReference Include="ReadFileLinesEnumGenerator" Version="1.0.0"&gt;
                    &lt;!-- Enable this project as an analyzer to run the source generator --&gt;
                    &lt;OutputItemType&gt;Analyzer&lt;/OutputItemType&gt;
                    &lt;!-- Include the analyzer output assembly --&gt;
                    &lt;ReferenceOutputAssembly&gt;true&lt;/ReferenceOutputAssembly&gt;
                    &lt;/PackageReference&gt;
                    &lt;/ItemGroup&gt;

                    &lt;!-- Specify additional files for the generator --&gt;
                    &lt;ItemGroup&gt;
                    &lt;AdditionalFiles Include="myEnum.enum.txt" /&gt;
                    &lt;/ItemGroup&gt;

                    &lt;/Project&gt;
                    ```

                    ```cs
                    using ReadFileLinesEnumGenerator.Generated;

                    // The 'myEnum' type is automatically defined using the content of the "myEnum.enum.txt"
                    file
                    // under the ReadFileLinesEnumGenerator.Generated namespace.
                    // Each line in the file is converted into a valid identifier through minimal
                    normalization.
                    myEnum selectedEnumValue = myEnum.A_0;

                    // An extension method is also provided to retrieve the original line.
                    string originalEnumValue = myEnum.A_0.ToStringValue();
                    ```

                    更に気になる方がおられましたら, ぜひリポジトリの方も見ていただければとても嬉しいです！

                    # ソースジェネレーターを自作した感想
                    ## Github Copilot が役に立たない
                    最近の LLM はとても便利で、 C# でも ASP.NET Core
                    を用いたプロジェクト内での提案やリファクタリングはごもっともと思わされることばかりで、 AI
                    は賢いなぁと驚かされます。
                    一方で、ソースジェネレーターを作るというのは比較的マイナーな行為なのでしょうか。自分にとって初めての試みなので、
                    Github Copilot
                    に素案を作ってもらってそれを修正していこうと当初は考えていましたが、古いバージョンのフレームワークに準拠したコードを出力したりとなかなかうまくいきませんでした。

                    ## 役に立った資料
                    横着せずに公式の資料などを読めば、簡単なパッケージを作るには十分な知識を得られました。
                    以下は Roslyn リポジトリ内のドキュメントです。公式の資料がしっかりしていて、それさえ読めば十分なあたりはさすがマイクロソフトだと感じました。

                    https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md
                    https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md

                    日本語の資料でおすすめなのは neuecc さんのブログ記事です。簡単なイントロダクションだけでなくガッツリ開発されている方ならではの
                    tips も参考になります。初出は2022年のようですが、2024年の今もしっかりと記事がアップデートされていて熱量がすごいなとなります。

                    https://neue.cc/2022/12/16_IncrementalSourceGenerator.html
                    # 付記
                    この記事は、私の Zenn のアカウントに 2025/01/09 に投稿した記事を、はてなブログへの移行に合わせて
                    2025/09/21 に加筆したものです。
                </content>

                <hatena:formatted-content xmlns:hatena="http://www.hatena.ne.jp/info/xmlns#"
                                          type="text/html">&lt;h1 id="概要"&gt;概要&lt;/h1&gt;

                    &lt;ul&gt;
                    &lt;li&gt;テキストファイルから列挙型を生成する自作の &lt;a class="keyword"
                    href="https://d.hatena.ne.jp/keyword/C%23"&gt;C#&lt;/a&gt; 向けソースジェレレーターのご紹介&lt;/li&gt;
                    &lt;li&gt;ソースジェネレーターを自作した感想&lt;/li&gt;
                    &lt;/ul&gt;


                    &lt;h1 id="自作ソースジェレレーターのご紹介"&gt;自作ソースジェレレーターのご紹介&lt;/h1&gt;

                    &lt;p&gt;&lt;a href="https://github.com/kamome283/ReadFileLinesEnumGenerator"&gt;https://github.com/kamome283/ReadFileLinesEnumGenerator&lt;/a&gt;&lt;/p&gt;

                    &lt;p&gt;名前は &lt;code&gt;ReadFileLinesEnumGenerator&lt;/code&gt;
                    です。もう少し英語力があれば、もっとマシな名前をつけれたのではないかと思うところはあります。
                    機能としては、 &lt;code&gt;.csproj&lt;/code&gt; 内で &lt;code&gt;AdditionalFiles&lt;/code&gt;
                    プロパティに指定された末尾が &lt;code&gt;.enum.txt&lt;/code&gt;
                    で終わるファイルを読み込み、その各行を列挙型の識別子として定義する列挙型を自動生成するものです。
                    以下は &lt;a class="keyword" href="https://d.hatena.ne.jp/keyword/GitHub"&gt;GitHub&lt;/a&gt;
                    &lt;a class="keyword"
                    href="https://d.hatena.ne.jp/keyword/%A5%EA%A5%DD%A5%B8%A5%C8%A5%EA"&gt;リポジトリ&lt;/a&gt;の
                    README から引用したサンプルコードです。使い方のイメージとして参考になればと思います。&lt;/p&gt;

                    &lt;pre class="code lang-xml" data-lang="xml" data-unlink&gt;&lt;span
                    class="synIdentifier"&gt;&amp;lt;Project&amp;gt;&lt;/span&gt;

                    &lt;span class="synComment"&gt;&amp;lt;!-- Add a reference to the source generator
                    project --&amp;gt;&lt;/span&gt;
                    &lt;span class="synIdentifier"&gt;&amp;lt;ItemGroup&amp;gt;&lt;/span&gt;
                    &lt;span class="synIdentifier"&gt;&amp;lt;PackageReference &lt;/span&gt;&lt;span
                    class="synType"&gt;Include&lt;/span&gt;=&lt;span class="synConstant"&gt;&amp;quot;ReadFileLinesEnumGenerator&amp;quot;&lt;/span&gt;&lt;span
                    class="synIdentifier"&gt; &lt;/span&gt;&lt;span class="synType"&gt;Version&lt;/span&gt;=&lt;span
                    class="synConstant"&gt;&amp;quot;1.0.0&amp;quot;&lt;/span&gt;&lt;span
                    class="synIdentifier"&gt;&amp;gt;&lt;/span&gt;
                    &lt;span class="synComment"&gt;&amp;lt;!-- Enable this project as an analyzer to run the
                    source generator --&amp;gt;&lt;/span&gt;
                    &lt;span class="synIdentifier"&gt;&amp;lt;OutputItemType&amp;gt;&lt;/span&gt;Analyzer&lt;span
                    class="synIdentifier"&gt;&amp;lt;/OutputItemType&amp;gt;&lt;/span&gt;
                    &lt;span class="synComment"&gt;&amp;lt;!-- Include the analyzer output assembly --&amp;gt;&lt;/span&gt;
                    &lt;span class="synIdentifier"&gt;&amp;lt;ReferenceOutputAssembly&amp;gt;&lt;/span&gt;true&lt;span
                    class="synIdentifier"&gt;&amp;lt;/ReferenceOutputAssembly&amp;gt;&lt;/span&gt;
                    &lt;span class="synIdentifier"&gt;&amp;lt;/PackageReference&amp;gt;&lt;/span&gt;
                    &lt;span class="synIdentifier"&gt;&amp;lt;/ItemGroup&amp;gt;&lt;/span&gt;

                    &lt;span class="synComment"&gt;&amp;lt;!-- Specify additional files for the generator --&amp;gt;&lt;/span&gt;
                    &lt;span class="synIdentifier"&gt;&amp;lt;ItemGroup&amp;gt;&lt;/span&gt;
                    &lt;span class="synIdentifier"&gt;&amp;lt;AdditionalFiles &lt;/span&gt;&lt;span
                    class="synType"&gt;Include&lt;/span&gt;=&lt;span class="synConstant"&gt;&amp;quot;myEnum.enum.txt&amp;quot;&lt;/span&gt;&lt;span
                    class="synIdentifier"&gt; /&amp;gt;&lt;/span&gt;
                    &lt;span class="synIdentifier"&gt;&amp;lt;/ItemGroup&amp;gt;&lt;/span&gt;

                    &lt;span class="synIdentifier"&gt;&amp;lt;/Project&amp;gt;&lt;/span&gt;
                    &lt;/pre&gt;


                    &lt;pre class="code lang-cs" data-lang="cs" data-unlink&gt;&lt;span class="synStatement"&gt;using&lt;/span&gt;
                    ReadFileLinesEnumGenerator.Generated;

                    &lt;span class="synComment"&gt;// The 'myEnum' type is automatically defined using the
                    content of the &amp;quot;myEnum.enum.txt&amp;quot; file&lt;/span&gt;
                    &lt;span class="synComment"&gt;// under the ReadFileLinesEnumGenerator.Generated
                    namespace. &lt;/span&gt;
                    &lt;span class="synComment"&gt;// Each line in the file is converted into a valid
                    identifier through minimal normalization.&lt;/span&gt;
                    myEnum selectedEnumValue &lt;span class="synStatement"&gt;=&lt;/span&gt; myEnum.A_0;

                    &lt;span class="synComment"&gt;// An extension method is also provided to retrieve the
                    original line.&lt;/span&gt;
                    &lt;span class="synType"&gt;string&lt;/span&gt; originalEnumValue &lt;span
                    class="synStatement"&gt;=&lt;/span&gt; myEnum.A_0.ToStringValue();
                    &lt;/pre&gt;


                    &lt;p&gt;更に気になる方がおられましたら, ぜひ&lt;a class="keyword"
                    href="https://d.hatena.ne.jp/keyword/%A5%EA%A5%DD%A5%B8%A5%C8%A5%EA"&gt;リポジトリ&lt;/a&gt;の方も見ていただければとても嬉しいです！&lt;/p&gt;

                    &lt;h1 id="ソースジェネレーターを自作した感想"&gt;ソースジェネレーターを自作した感想&lt;/h1&gt;

                    &lt;h2 id="Github-Copilot-が役に立たない"&gt;&lt;a class="keyword"
                    href="https://d.hatena.ne.jp/keyword/Github"&gt;Github&lt;/a&gt; Copilot が役に立たない&lt;/h2&gt;

                    &lt;p&gt;最近の LLM はとても便利で、 &lt;a class="keyword"
                    href="https://d.hatena.ne.jp/keyword/C%23"&gt;C#&lt;/a&gt; でも &lt;a class="keyword"
                    href="https://d.hatena.ne.jp/keyword/ASP.NET"&gt;ASP.NET&lt;/a&gt; Core を用いたプロジェクト内での提案や&lt;a
                    class="keyword"
                    href="https://d.hatena.ne.jp/keyword/%A5%EA%A5%D5%A5%A1%A5%AF%A5%BF%A5%EA%A5%F3%A5%B0"&gt;リファクタリング&lt;/a&gt;はごもっともと思わされることばかりで、
                    AI は賢いなぁと驚かされます。
                    一方で、ソースジェネレーターを作るというのは比較的マイナーな行為なのでしょうか。自分にとって初めての試みなので、
                    &lt;a class="keyword" href="https://d.hatena.ne.jp/keyword/Github"&gt;Github&lt;/a&gt;
                    Copilot に素案を作ってもらってそれを修正していこうと当初は考えていましたが、古いバージョンの&lt;a
                    class="keyword"
                    href="https://d.hatena.ne.jp/keyword/%A5%D5%A5%EC%A1%BC%A5%E0%A5%EF%A1%BC%A5%AF"&gt;フレームワーク&lt;/a&gt;に準拠したコードを出力したりとなかなかうまくいきませんでした。&lt;/p&gt;

                    &lt;h2 id="役に立った資料"&gt;役に立った資料&lt;/h2&gt;

                    &lt;p&gt;横着せずに公式の資料などを読めば、簡単なパッケージを作るには十分な知識を得られました。
                    以下は Roslyn &lt;a class="keyword"
                    href="https://d.hatena.ne.jp/keyword/%A5%EA%A5%DD%A5%B8%A5%C8%A5%EA"&gt;リポジトリ&lt;/a&gt;内のドキュメントです。公式の資料がしっかりしていて、それさえ読めば十分なあたりはさすが&lt;a
                    class="keyword"
                    href="https://d.hatena.ne.jp/keyword/%A5%DE%A5%A4%A5%AF%A5%ED%A5%BD%A5%D5%A5%C8"&gt;マイクロソフト&lt;/a&gt;だと感じました。&lt;/p&gt;

                    &lt;p&gt;&lt;a
                    href="https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md"&gt;https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md&lt;/a&gt;
                    &lt;a
                    href="https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md"&gt;https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md&lt;/a&gt;&lt;/p&gt;

                    &lt;p&gt;日本語の資料でおすすめなのは neuecc
                    さんのブログ記事です。簡単なイントロダクションだけでなくガッツリ開発されている方ならではの
                    tips も参考になります。初出は2022年のようですが、2024年の今もしっかりと記事がアップデートされていて熱量がすごいなとなります。&lt;/p&gt;

                    &lt;p&gt;&lt;a href="https://neue.cc/2022/12/16_IncrementalSourceGenerator.html"&gt;https://neue.cc/2022/12/16_IncrementalSourceGenerator.html&lt;/a&gt;&lt;/p&gt;

                    &lt;h1 id="付記"&gt;付記&lt;/h1&gt;

                    &lt;p&gt;この記事は、私の Zenn のアカウントに 2025/01/09 に投稿した記事を、&lt;a
                    class="keyword"
                    href="https://d.hatena.ne.jp/keyword/%A4%CF%A4%C6%A4%CA%A5%D6%A5%ED%A5%B0"&gt;はてなブログ&lt;/a&gt;への移行に合わせて
                    2025/09/21 に加筆したものです。&lt;/p&gt;
                </hatena:formatted-content>

                <category term="技術"/>

                <app:control>

                    <app:draft>no</app:draft>

                    <app:preview>no</app:preview>

                </app:control>

            </entry>


        </feed>
        """;

    [Fact]
    public void ItCanDeserializeValidResponseBody()
    {
        var xml = XDocument.Parse(ResponseBody);
        var entryElements = xml.Descendants().Where(x => x.Name.LocalName == "entry");
        foreach (var entryElement in entryElements)
        {
            var entry = FetchedEntrySchema.Deserialize(entryElement);
        }
    }
}