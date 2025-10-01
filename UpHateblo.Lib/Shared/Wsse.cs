using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace UpHateblo.Lib.Shared;

/// <summary>
///     WSSE認証の認証用文字列を作成するためのクラス
/// </summary>
/// <param name="username">はてなID</param>
/// <param name="password">はてなのAPIキー</param>
/// <param name="nonce">HTTPリクエスト毎に生成したランダムな文字列</param>
/// <param name="createdDateTime">Nonceが作成された日時をISO-8601拡張表記で記述したもの</param>
/// <remarks>
///     References:
///     https://developer.hatena.ne.jp/ja/documents/auth/apis/wsse/
///     https://kanatech.hatenablog.com/entry/2013/01/06/132031
///     https://ameblo.jp/norixp/entry-10013651868.html
/// </remarks>
internal class Wsse(string username, string password, string nonce, DateTime createdDateTime)
{
    // 空文字列のnonceで作成したWSSEトークンでは認証できなかったので、
    // 空文字列の場合はイニシャライズ直後にValidationExceptionを投げる
    // ReSharper disable once UnusedMember.Local
    private string _ = nonce != ""
        ? nonce
        : throw new ValidationException("nonce cannot be empty string.");

    private static SHA1 Sha1 => SHA1.Create();

    // ISO-8601 extended format without timezone
    private string Created => createdDateTime.ToString("s");

    public string GetToken()
    {
        var digest = GetDigest();
        return $"""
                UsernameToken Username="{username}", PasswordDigest="{digest}, Nonce="{nonce}", Created="{Created}"
                """;
    }

    private string GetDigest()
    {
        var digestText = nonce + Created + password;
        var digestBytes = Encoding.ASCII.GetBytes(digestText);
        var digest = Sha1.ComputeHash(digestBytes);
        return Convert.ToBase64String(digest);
    }
}