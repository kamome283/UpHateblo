using System.ComponentModel.DataAnnotations;
using UpHateblo.Lib.Shared;

namespace UpHateblo.Lib.Tests.Http;

public class WsseTests
{
    private static string ValidityTestExpected =>
        """
        UsernameToken Username="Kamome283", PasswordDigest="EP4+VS7LZFZpfQJf7U3qvqxjW1Y=, Nonce="some-nonce", Created="2025-09-23T21:29:00"
        """;

    internal static Wsse ValidityTestInstance =>
        new("Kamome283", "password12", "some-nonce", DateTime.Parse("2025-09-23T21:29:00"));

    [Fact]
    public void IsValidToken()
    {
        Assert.Equal(ValidityTestExpected, ValidityTestInstance.GetToken());
    }

    [Fact]
    public void ThrowsConstructionWithEmptyNonce()
    {
        Assert.Throws<ValidationException>(() =>
            {
                _ = new Wsse("Kamome283", "password12", "", DateTime.Parse("2025-09-23T21:29:00"));
            }
        );
    }
}