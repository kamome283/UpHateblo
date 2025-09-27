UpHateblo development guidelines (project-specific)

Audience: Advanced .NET contributors working on this repository. This document captures concrete, project-specific practices that have been verified against the current codebase (net9.0, xUnit, YamlDotNet, Equatable.Generator).

1) Build and configuration
- Toolchain
  - .NET SDK: 9.x (TargetFramework is net9.0 across projects).
  - IDEs: JetBrains Rider or Visual Studio 2022+; any editor is fine as long as it uses .NET 9 SDK.
- Projects
  - UpHateblo.Lib: core library. Depends on YamlDotNet and Equatable.Generator.
  - UpHateblo.Cli: Currently not implemented but planned for future implementation as a proper
    command line interface.
  - UpHateblo.Lib.Tests: xUnit test project, references UpHateblo.Lib.
- Build the solution
  - dotnet restore
  - dotnet build UpHateblo.sln -c Debug
- NuGet packages of note
  - YamlDotNet 16.3.0 (front matter parsing). Be aware of scalar type conversion behavior (see Testing/Dev Notes below).
  - Equatable.Generator 2.0.0 (source generator) is added as PrivateAssets=all to UpHateblo.Lib. If you see equality behavior that seems custom (e.g., HashSet value equality), it is produced by this generator from attributes in Entities. Ensure generators are enabled in your environment.
  - Test infra: Microsoft.NET.Test.Sdk, xunit, xunit.runner.visualstudio, coverlet.collector.
- Internals
  - UpHateblo.Lib.csproj declares InternalsVisibleTo=$(AssemblyName).Tests. If you rename the test project, adjust this attribute accordingly or update AssemblyName to maintain internal visibility.

2) Testing: how to run, add, and validate
- Run the entire test suite except requiring a web request
  - dotnet test UpHateblo.sln -c Debug --filter "Category!=Commands"
  - This will build and execute tests in UpHateblo.Lib.Tests. All tests passed locally at the
    time of writing.
  - Tests requiring a web request must be run manually.
- Run with filters
  - By fully qualified name (recommended):
    dotnet test UpHateblo.sln --filter FullyQualifiedName~UpHateblo.Lib.Tests.Markdown.DeserializeEntryTests
  - By trait/category if you add [Trait] attributes in future.
- Code coverage (via coverlet.collector)
  - Example:
    dotnet test UpHateblo.sln -c Debug \
      /p:CollectCoverage=true \
      /p:CoverletOutputFormat=cobertura \
      /p:Exclude="[xunit.*]*,[*]UpHateblo.Lib.Tests.*"
- Adding a new test
  - Place tests under UpHateblo.Lib.Tests/<Area>/ to match the current convention (e.g., Entities/, Markdown/, Http/, Schema/).
  - Namespace should follow the folder structure, e.g., namespace UpHateblo.Lib.Tests.Markdown;
  - Use xUnit [Fact]/[Theory]. Implicit usings are enabled; Xunit is imported via a global Using in the test csproj.
  - Internal types from UpHateblo.Lib are accessible because of InternalsVisibleTo.
- Verification done for this guideline
  - The existing test suite was executed via dotnet test and all tests passed in net9.0.
  - A trivial example test was created and executed during preparation of this document to validate instructions, then removed to keep the repository clean, as required by this task.

3) Project-specific testing/dev notes
- Documentation for using the Hatena Blog API can be found in ./HatenaBlogApiProtocol.md
- YAML front matter deserialization (DeserializeEntry)
  - The Markdown front matter supports keys such as Title, Category (sequence), Date (DateTimeOffset), UrlPath, Draft (bool), Preview (bool).
  - Empty or missing front matter is allowed; the body is the entire content.
  - Unknown fields in front matter are ignored by the deserializer; they do not cause failures.
  - Invalid scalar types: YamlDotNet will throw when a scalar cannot be converted to the target type (e.g., non-ISO date, non-bool for Draft). Tests verify that exceptions are thrown consistently, including under concurrency.
  - Thread-safety: DeserializeEntry has tests asserting consistent results across high-concurrency deserialization. Avoid static mutable state in deserialization paths; prefer constructing YamlDotNet objects in a local scope or ensuring they are thread-safe.
- Equality semantics via Equatable.Generator
  - Entities like MaybeEntry rely on generated equality based on attributes (e.g., [HashSetEquality] for category tags). HashSet order should not affect equality; tests assert this. When modifying entities, ensure attributes remain correct and re-run generator-backed equality tests.
- Date handling
  - Tests expect deterministic UTC/offset behavior (e.g., using fixed DateTimeOffset strings). When parsing dates, use DateTimeOffset and preserve offsets; avoid implicit local time conversions unless intentional.
- URL and endpoint composition
  - BlogConfig tests verify construction of specific Hatena endpoints from BlogId and UserName. When changing config shapes, update tests and keep string composition rules centralized.
- HTTP helpers
  - HatenaContent tests validate content-type handling for XML. Preserve content types exactly as asserted in tests when evolving HTTP code.

4) Debugging tips and workflow specifics
- Reproduce-specific test(s)
  - Use the provided focused tests (e.g., DeserializeEntryTests.ItHandlesInvalidNonStringProperties) to iterate on YAML mapping behavior.
- Concurrency stress
  - DeserializeEntryTests includes high iteration Task.WhenAll based concurrency checks. If you change parsing or shared caches, run that class alone to quickly detect regressions.
- Working with source generators
  - If your IDE doesn’t show generated members from Equatable.Generator, build from the command line to confirm behavior. Generators run at compile time; no runtime dependency is produced.
- Localized resources in bin for tests
  - The test output includes localized xUnit runner resources; no action required, but avoid committing bin/obj.

5) Code style and conventions
- C# language level: ImplicitUsings and Nullable enabled. Favor modern C# (primary constructors, target-typed new, collection expressions) where it doesn’t obscure readability.
- Namespaces follow folder layout (file-scoped namespaces are used). Keep files short and cohesive.
- Tests: prefer clear Arrange/Act/Assert with minimal shared state. Use explicit, deterministic inputs (e.g., fixed dates) to avoid flakiness.
- Follow these instructions:
  - Do not return example code, do not use @author or @version or @since tags.
  - DO NOT generate example usage.
  - DO NOT generate usage example.
  - DO NOT use html tags such as <p>, <lu>, <li>.
  - DO NOT generate documentation for type member properties.
  - DO NOT convert from IEnumerable<T> to T[] if not required.
  - Use raw string literal for multiple line strings.
  - Use raw string literal for string which requires escaping.
  - Use LINQ as far as possible.

6) Typical developer commands
- Build: dotnet build UpHateblo.sln -c Debug
- Run tests (all): dotnet test UpHateblo.sln -c Debug
- Run one class: dotnet test UpHateblo.sln --filter FullyQualifiedName~UpHateblo.Lib.Tests.Entities.MaybeEntryTests
- Coverage: dotnet test UpHateblo.sln /p:CollectCoverage=true

If you introduce new projects or rename existing ones, keep InternalsVisibleTo and test project references in sync, and update this document accordingly.
