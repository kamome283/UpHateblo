using System.Diagnostics.CodeAnalysis;
using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public record BlogConfigSecrets(BlogConfig Blog);