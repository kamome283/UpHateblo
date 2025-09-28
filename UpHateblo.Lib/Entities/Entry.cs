using Equatable.Attributes;

namespace UpHateblo.Lib.Entities;

// APIレスポンスにはAuthorプロパティが含まれていますが、このクラスでは意図的に除外しています。
// これは、想定ユーザーが個人ブロガーであり、投稿者情報はBlogConfigから取得できるためです。
// 将来的にマルチユーザー対応が必要になった場合は、この設計を見直す可能性があります。
[Equatable]
public partial record Entry(
    string Title,
    [property: HashSetEquality] HashSet<string> Category,
    string Content,
    string? ContentType,
    string? EntryId,
    string? CustomPath,
    DateTime? Updated,
    DateTime? Published,
    bool? Draft,
    bool? Preview
);