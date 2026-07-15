# Convex .NET Client for Unity (Unofficial)

A Unity 6 UPM packaging of
[zakstam/convex-dotnet-unofficial](https://github.com/zakstam/convex-dotnet-unofficial)'s
`Convex.Client` — the unofficial .NET client for [Convex](https://convex.dev)
(websocket realtime subscriptions via `Observe<T>`, one-shot queries,
mutations, and actions).

Unofficial: not affiliated with or endorsed by Convex or the upstream author.

## What this is

- Source from upstream commit `cc0759f5c5c3261dd898a355dde149096f853c9e`
  (MIT), trimmed of pieces a Unity runtime doesn't need (Clerk auth,
  `Microsoft.Extensions.DependencyInjection` registration, WPF/MAUI helpers,
  batching/gaming/testing extensions).
- Mechanically down-converted from C#13 to C#11 so Unity 6000.0's bundled
  Roslyn (4.3.1, C#11-preview via `csc.rsp`) can compile it. Every
  transformation is recorded in [NOTICE.md](NOTICE.md).
- Bundled managed dependencies (System.Reactive, System.Text.Json, and their
  netstandard2.0 support assemblies) under `Runtime/Plugins/`, plus a
  `link.xml` for IL2CPP.

## Install

Add to your project's `Packages/manifest.json`:

```json
"com.ryanbliss.convex-dotnet-unity": "https://github.com/ryanbliss/convex-dotnet-unity.git#v0.1.1"
```

Reference the `Convex.Client` assembly from your asmdef.

## Platform notes

- Requires .NET Standard 2.1 API compatibility (Unity 6 default).
- No WebGL (the transport is `System.Net.WebSockets.ClientWebSocket`).

## License

MIT — see [LICENSE.md](LICENSE.md) (upstream copyright Zak Stam) and
[NOTICE.md](NOTICE.md) for the full provenance, trim log, and edit log.
