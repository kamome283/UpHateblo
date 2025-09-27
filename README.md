# UpHateblo

A C# tool providing PowerShell and command-line interfaces for managing Hatena Blog posts through
AtomPub.
This project draws inspiration from [x-motemen/blogsync](https://github.com/x-motemen/blogsync). It
focuses on providing a set of simple commands that are fully controlled through command-line
arguments, making them easy to combine and use in shell.

This repository contains the following projects:
- UpHateblo.Lib: Core library for working with Hatena Blog (AtomPub) posts
- UpHateblo.Pwsh: PowerShell module for working with Hatena Blog posts (not yet implemented)
- UpHateblo.Cli: Command-line interface for working with Hatena Blog posts (not yet implemented)

## Overview of capabilities

- Post entries to Hatena Blog using `EntryCommands.Post`
- Parse Markdown entries with `DeserializeEntry.Run`
  - Supported fields: `Title`, `Category` (list), `Date` (date/time), `UrlPath`, `Draft` (
    true/false), `Preview` (true/false)
  - Additional fields are safely ignored
  - Files without front matter are treated as body content only
  - Invalid field values will cause YamlDotNet parsing errors
---

## Roadmap / TODOs

### v0.1

Key focus: Implement core functionality for posting to Hatena Blog

- [x] Add the ability to post blog entries via `UpHateblo.Lib.Post`
- [x] Add the ability to parse markdown entries via `UpHateblo.Lib.Deserialize`
- [ ] Add the ability to modify existing blog entries via `UpHateblo.Lib.Push`
- [ ] Add the ability to list all blog entries via `UpHateblo.Lib.List`
- [ ] Create PowerShell module `UpHateblo.Pwsh` that integrates with `EntryCommands` and markdown
  parsing
- [ ] Release PowerShell module to PowerShell Gallery

### v0.2

Key focus: Expand functionality for complete blog entry management

- [ ] Add ability to fetch individual blog entries via `UpHateblo.Lib.Fetch`
- [ ] Add ability to delete blog entries via `UpHateblo.Lib.Remove`
- [ ] Create entry deserialization functionality in `UpHateblo.Lib.Deserialize`
  - Focus on string-based deserialization for maximum reusability
  - Let file operations be handled by shell commands
    - Utilize shell redirection and similar features
- [ ] Update `UpHateblo.Pwsh` to include new `Fetch`, `Remove`, and `Deserialize` features
- Publish enhanced PowerShell module to PowerShell Gallery

### v0.3

Key focus: Create command-line interface

- [ ] Develop `UpHateblo.Cli` as a command-line tool matching the PowerShell module's functionality
- [ ] Publish to NuGet Gallery for easy installation through `dotnet tool install`

---

## Instructions for development

### Hatena Blog API
- [Hatena Blog AtomPub API](https://developer.hatena.ne.jp/ja/documents/blog/apis/atom)

### Entry points and internal APIs
- Library entry points intended for consumers within this solution:
  - `EntryCommands.Post(HttpClient, BlogConfig, Entry, ...)` posts an entry to Hatena Blog
  - `DeserializeEntry.Run(string content)` parses markdown + YAML front matter into `MaybeEntry`

### Test instructions

Tests are written in xUnit.net and can be run using `dotnet test`. Some tests make changes to live
blog entries in your account, but this requires configuring test secrets first.
To avoid accidentally making live HTTP requests, make sure to use filtered tests by running
`dotnet test --filter "Category!=Commands"`.

### Secrets and configuration for live HTTP tests
Tests marked `[Trait("Category", "Commands")]` make changes to live blog entries in your account and require .NET User 
Secrets.
- Required secret keys under the default test root:
    - `Blog:BlogId`
    - `Blog:Username`
    - `Blog:Password` (Hatena API key)

- Configure secrets for the test project:
    1) Change directory to `UpHateblo.Lib.Tests`
    2) Set secrets with `dotnet user-secrets`:
```
# from UpHateblo.Lib.Tests/
dotnet user-secrets set "Blog:BlogId" "<your-blog-id>"
dotnet user-secrets set "Blog:Username" "<your-hatena-id>"
dotnet user-secrets set "Blog:Password" "<your-api-key>"
```

With secrets configured, you can run the live-posting tests individually. 
Be aware these tests will make changes to your blog account.

---

## License
MIT License
