# Vendored third-party code

## Convex .NET client (unofficial)

- Source: <https://github.com/zakstam/convex-dotnet-unofficial>
- Vendored path in upstream: `src/Convex.Client`
- Pinned commit: `cc0759f5c5c3261dd898a355dde149096f853c9e`
- License: MIT — Copyright (c) Zak Stam 2025
- Packaged into: `Runtime/Convex.Client/`

This is a trimmed source copy compiled by Unity as the
`Convex.Client` assembly (`csc.rsp` raises the language version;
the code targets the netstandard2.1 surface Unity provides). It is not a fork:
upstream updates are re-vendored against the pinned commit, re-applying the
trim and edit log below.

### Trim log (deleted at vendor time, never copied)

- `Auth/` — Clerk token services; consumers bring their own
  `IAuthTokenProvider` implementation.
- `DependencyInjection/` — Microsoft.Extensions.DependencyInjection
  registration; clients are constructed directly.
- `DeveloperTools/` — dev-only builder extensions.
- `Extensions/Gaming/` — game-loop sync helpers, unused by the provider.
- `Extensions/Batching/` — batching pipeline, unused by the provider.
- `Extensions/Testing/` — upstream test helpers.
- `Extensions/ExtensionMethods/ConvexWpfMauiExtensions.cs` — WPF/MAUI.
- `Extensions/ExtensionMethods/ConvexTestingExtensions.cs` — test helpers.
- `Extensions/ExtensionMethods/ConvexPerformanceExtensions.cs` and
  `Infrastructure/Performance/` — object-pooling optimizations; removing them
  drops the `Microsoft.Extensions.ObjectPool` dependency.
- `*.csproj`, `PublicAPI.*.txt`, `obj/`, `bin/`.

### Edit log (source changes relative to the pinned commit)

Unity 6000.0 ships Roslyn 4.3.1 (C#11-preview via this assembly's
`csc.rsp -langversion:preview`); upstream compiles with `LangVersion=latest`
(C#13). The vendored copy is therefore mechanically down-converted to C#11:

- **C#12 collection expressions** (89 sites, Roslyn-scripted rewrite using
  semantic target types): `[]`/`[a, b]`/`[.. x]` became `Array.Empty<T>()`,
  `new T[] { … }`, `new List<T>(x)`, `Enumerable.ToArray(x)`, etc.
- **C#12 non-record primary constructors** (72 types, Roslyn-scripted):
  parameters became private capture fields + an explicit constructor;
  parameter-referencing member initializers moved into the constructor body;
  base-type argument lists moved to `: base(…)`. Two `readonly struct` cases
  (`ConvexTimestamp`, `ConvexNumber`) and `OptimisticCollection<T>` (nested
  collection expression) were finished by hand.
- **C#13 `Span<byte>` local in an async method**
  (`ConvexWebSocketClient.ReceiveLoopAsync`): replaced with an
  `ArraySegment<byte>` `AddRange`, and `Encoding.UTF8.GetString([.. buf])`
  with `GetString(buf.ToArray())`.
- `ArgumentNullException.ThrowIfNull(x);` rewritten to
  `if (x is null) { throw new ArgumentNullException(nameof(x)); }` (32 sites)
  and `ObjectDisposedException.ThrowIf(_disposed, this);` rewritten to
  `if (_disposed) { throw new ObjectDisposedException(GetType().FullName); }`
  (2 sites): both APIs are .NET 6+, absent from Unity's netstandard2.1 BCL.
  Call-site rewrites were chosen over a shadowing exception shim because
  `ConvexWebSocketClient` catches `ObjectDisposedException` thrown by the BCL,
  which a shadow type would silently stop matching.
- `ArgumentOutOfRangeException.ThrowIfNegative/ThrowIfNegativeOrZero/`
  `ThrowIfLessThanOrEqual` (.NET 8+) expanded to explicit `if`/`throw`
  (11 sites).
- `HttpContent.ReadAsStringAsync(ct)` / `ReadAsStreamAsync(ct)` /
  `StreamReader.ReadToEndAsync(ct)` (.NET 5+/7+ overloads) dropped the
  `CancellationToken` argument (5 sites; netstandard2.1 has no such
  overloads — cancellation still applies at surrounding await points).
- `Task.WaitAsync(ct)` (.NET 6+) in `ConvexClient.EnsureConnectedAsync`
  replaced with a `Task.WhenAny` + cancellation-`TaskCompletionSource`
  equivalent.

### Support files (ours, not upstream's)

- `Polyfills/CompilerAttributePolyfills.cs` — internal declarations of
  `IsExternalInit`, `RequiredMemberAttribute`,
  `CompilerFeatureRequiredAttribute`, `SetsRequiredMembersAttribute` so C#
  `init`/`required` in the vendored code compile against Unity's BCL.
- `csc.rsp` — raises the C# language version for this assembly only and
  enables nullable (upstream compiles with `LangVersion=latest`,
  `Nullable=enable`).
- `link.xml` — IL2CPP stripping preserves for the dependency assemblies.

## Bundled dependency assemblies (`Plugins/`)

Managed NuGet binaries the source requires. Versions match the upstream pins at the
vendored commit where available.

| Assembly | NuGet package | Version | lib TFM |
| --- | --- | --- | --- |
| System.Reactive.dll | System.Reactive | 6.0.1 | netstandard2.0 |
| System.Text.Json.dll | System.Text.Json | 8.0.5 | netstandard2.0 |
| System.Text.Encodings.Web.dll | System.Text.Encodings.Web | 8.0.0 | netstandard2.0 |
| Microsoft.Bcl.AsyncInterfaces.dll | Microsoft.Bcl.AsyncInterfaces | 8.0.0 | netstandard2.1 |
| System.Threading.Channels.dll | System.Threading.Channels | 8.0.0 | netstandard2.1 |
| Microsoft.Extensions.Logging.Abstractions.dll | Microsoft.Extensions.Logging.Abstractions | 9.0.0 | netstandard2.0 |
| Microsoft.Extensions.DependencyInjection.Abstractions.dll | Microsoft.Extensions.DependencyInjection.Abstractions | 9.0.0 | netstandard2.1 |
| System.Diagnostics.DiagnosticSource.dll | System.Diagnostics.DiagnosticSource | 9.0.0 | netstandard2.0 |
| System.Runtime.CompilerServices.Unsafe.dll | System.Runtime.CompilerServices.Unsafe | 6.0.0 | netstandard2.0 |
| System.Buffers.dll | System.Buffers | 4.5.1 | netstandard2.0 |
| System.Memory.dll | System.Memory | 4.5.5 | netstandard2.0 |
| System.Threading.Tasks.Extensions.dll | System.Threading.Tasks.Extensions | 4.5.4 | netstandard2.0 |

The last four are netstandard2.0-era compat facades that the
netstandard2.0-targeted assemblies above reference; Unity does not provide
them itself.

All are MIT-licensed (.NET Foundation / Microsoft).
