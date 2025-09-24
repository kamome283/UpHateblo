using System.ComponentModel.DataAnnotations;
using UpHateblo.Lib.Http;

namespace UpHateblo.Lib.Tests.Http;

public class WsseTests
{
    private static string ValidityTestCase1 =>
        """
        UsernameToken Username="Kamome283", PasswordDigest="EP4+VS7LZFZpfQJf7U3qvqxjW1Y=, Nonce="some-nonce", Created="2025-09-23T21:29:00"
        """;

    public static IEnumerable<object[]> ValidityTestSource()
    {
        return
        [
            ["Kamome283", "password12", "some-nonce", "2025-09-23T21:29:00", ValidityTestCase1],
        ];
    }

    [Theory, MemberData(nameof(ValidityTestSource))]
    public void TokenIsValid(string username, string password, string nonce, string datetime,
        string expected)
    {
        var wsse = new Wsse(username, password, nonce, DateTime.Parse(datetime));
        Assert.Equal(expected, wsse.GetToken());
    }

    [Fact]
    public void ConstructWithEmptyNonceThrows()
    {
        Assert.Throws<ValidationException>(() =>
            {
                _ = new Wsse("Kamome283", "password12", "", DateTime.Parse("2025-09-23T21:29:00"));
            }
        );
    }
}