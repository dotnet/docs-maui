---
title: "Migrate to the UIKit scene lifecycle"
description: "Update an existing .NET MAUI iOS or Mac Catalyst app for the UIKit scene lifecycle required by the Xcode 27 SDKs, and troubleshoot scene-manifest validation."
ms.date: 10/26/2026
monikerRange: ">=net-maui-10.0"
ai-usage: ai-assisted
---

# Migrate to the UIKit scene lifecycle

Apps built against the iOS or Mac Catalyst 27 SDKs must use the UIKit scene lifecycle to launch on iOS or macOS 27 or later. Updating .NET MAUI packages or workloads doesn't add scene configuration to an existing project. This guide shows you how to update the platform files and preserve your app's lifecycle behavior.

A *scene* manages an instance of your app's user interface and its foreground and background transitions. The application lifecycle continues to manage the process as a whole. Adopting scenes doesn't require enabling multiple windows, changing your shared UI, or raising your minimum OS version to 27. An existing binary built with an older SDK doesn't require this migration just because a user updates their OS.

Missing scene configuration can cause an app to close immediately after its splash screen, with the following device log message:

```text
UIScene life cycle is required for apps built with this SDK.
```

Other problems can cause the same startup symptom, so check the device log before attributing a crash to scene adoption. If your build reports a missing `UIApplicationSceneManifest` dictionary, see [Troubleshoot scene-manifest validation](#troubleshoot-scene-manifest-validation).

For a standard app, check your toolchain and minimum OS versions, add the delegate and manifest for each Apple target, then build and verify. If you've customized startup, lifecycle callbacks, authentication, links, or App Actions, also complete [Migrate custom lifecycle behavior](#migrate-custom-lifecycle-behavior).

## Check your project and toolchain

Save your current work before making changes. Identify the app's iOS and Mac Catalyst target frameworks, effective .NET MAUI package versions, and any SDK or workload pins in `global.json` and your build pipeline.

1. Use a .NET for iOS and Mac Catalyst workload that supports your selected Xcode version. Check the [.NET for iOS and Mac Catalyst releases](https://github.com/dotnet/macios/releases) for the required pairing. For example, the .NET 10 Apple workloads `27.0.10722` require Xcode 27.0. Don't disable Xcode version validation to use an incompatible pairing.
1. For a .NET 10 app, update the .NET MAUI packages to **10.0.110 or a later compatible servicing release**. Earlier versions include `MauiUISceneDelegate`, but 10.0.110 also includes scene-based App Actions dispatch and relevant `WebAuthenticator` forwarding. Adding the scene files alone isn't sufficient to preserve those integrations.
1. Check for existing scene delegates, scene manifests, custom `AppDelegate` overrides, and `ConfigureLifecycleEvents` registrations. If your app already uses scenes, reconcile its configuration and review its custom behavior instead of adding duplicate files.

If your package references use `$(MauiVersion)`, you can set the version in a `<PropertyGroup>` in the app's `.csproj` file:

```xml
<PropertyGroup>
  <MauiVersion>10.0.110</MauiVersion>
</PropertyGroup>
```

If you manage versions in `Directory.Packages.props`, another imported file, or explicit `PackageReference` versions, update the existing version definitions instead. Don't downgrade a newer compatible version or add a competing version pin. You don't need to change a .NET 10 app to target .NET 11 for this migration.

Apps created with the .NET MAUI 10.0.110 templates already include the scene files. Existing projects still need the changes in the following sections. Apply each platform-specific step only to platforms your app targets.

### Check minimum OS versions

The .NET for iOS and Mac Catalyst 27 SDKs require minimum deployment targets of iOS **15.0** and Mac Catalyst **17.0** (macOS 14), respectively. These SDK minimums are separate from scene adoption.

If your existing deployment targets are lower, update their definitions inside a `<PropertyGroup>` in your app's `.csproj` file:

```xml
<SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">15.0</SupportedOSPlatformVersion>
<SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'maccatalyst'">17.0</SupportedOSPlatformVersion>
```

Include only the platforms your app targets, replace existing definitions instead of adding duplicates, and retain any higher minimum your app already requires.

> [!IMPORTANT]
> Raising a deployment target drops support for older OS versions. If you must retain support for iOS versions below 15 or macOS 12 and 13, stay on a compatible 26.x Apple SDK while you decide your support policy. Don't raise these values without considering your users.

## Add the scene delegate

Create `Platforms/iOS/SceneDelegate.cs` with the following content. Replace `MyMauiApp` with your app's namespace:

```csharp
using Foundation;
using Microsoft.Maui;

namespace MyMauiApp;

[Register("SceneDelegate")]
public class SceneDelegate : MauiUISceneDelegate
{
}
```

If your app targets Mac Catalyst, add the same class in `Platforms/MacCatalyst/SceneDelegate.cs`. In a standard .NET MAUI single project, only the source files for the target platform are compiled, so these two files can declare the same class.

Keep your existing `AppDelegate : MauiUIApplicationDelegate`. The scene delegate works alongside the application delegate; it doesn't replace it. .NET MAUI creates the window through `MauiUISceneDelegate`, so the default scene delegate needs no additional code.

If you already have a scene delegate, preserve its custom behavior and check its base class and registered name instead of adding a second delegate. When you override methods on `MauiUISceneDelegate`, call the corresponding `base` implementation so framework window creation, lifecycle dispatch, and authentication handling still run.

## Add the scene manifest

Open `Platforms/iOS/Info.plist` in a text editor and search for `UIApplicationSceneManifest`. If it already exists, update that dictionary instead of adding a duplicate key. Otherwise, paste the following entry inside the root `<dict>`, before the closing `</dict>` that precedes `</plist>`:

```xml
<key>UIApplicationSceneManifest</key>
<dict>
  <key>UIApplicationSupportsMultipleScenes</key>
  <false/>
  <key>UISceneConfigurations</key>
  <dict>
    <key>UIWindowSceneSessionRoleApplication</key>
    <array>
      <dict>
        <key>UISceneConfigurationName</key>
        <string>__MAUI_DEFAULT_SCENE_CONFIGURATION__</string>
        <key>UISceneDelegateClassName</key>
        <string>SceneDelegate</string>
      </dict>
    </array>
  </dict>
</dict>
```

If your app targets Mac Catalyst, add the same entry to `Platforms/MacCatalyst/Info.plist`.

Follow these rules when you add or reconcile the manifest:

- Keep `__MAUI_DEFAULT_SCENE_CONFIGURATION__` exactly as shown. The default .NET MAUI scene delegate uses this name to create the window.
- Match `UISceneDelegateClassName` to the Objective-C registration name in `[Register("SceneDelegate")]`, not the namespace-qualified C# name.
- Keep `UIApplicationSupportsMultipleScenes` set to `false` for a single-window app. If your app already supports multiple windows, preserve that behavior. See [Multi-window support](../user-interface/controls/window.md#multi-window-support).
- Update an existing `UIApplicationSceneManifest` dictionary instead of adding a duplicate key. Preserve other scene roles and customizations. If you override `GetConfiguration` or use custom configuration names, review that implementation together with the manifest and delegate; a different name requires custom window-creation handling.
- Don't add `UISceneStoryboardFile`. .NET MAUI creates the window in code. Preserve your existing `MauiSplashScreen` and `UILaunchStoryboardName` configuration.

Some projects select different plist files for Debug, Release, or other build configurations. Check `AppManifest` settings and partial manifests in your project and imported build files. Update every applicable configuration, not just the default platform plist. The manifest in the built app bundle is the final result that matters.

## Migrate custom lifecycle behavior

If your app uses only the standard template startup code, you don't need to add lifecycle handlers. Existing cross-platform `Window` events continue to work; .NET MAUI forwards scene events to them.

If you've added native lifecycle code, review both `ConfigureLifecycleEvents` registrations in `MauiProgram.cs` and overrides in `AppDelegate`. With scenes enabled, move custom UI activation, backgrounding, URL, and user-activity logic to the corresponding scene callbacks:

| Existing lifecycle registration | Scene lifecycle registration |
| --- | --- |
| `OnActivated` | `SceneOnActivated` |
| `OnResignActivation` | `SceneOnResignActivation` |
| `DidEnterBackground` | `SceneDidEnterBackground` |
| `WillEnterForeground` | `SceneWillEnterForeground` |
| `OpenUrl` | `SceneOpenUrl` |
| `ContinueUserActivity` | `SceneContinueUserActivity` |

The callback arguments differ, so adapt the logic rather than only renaming the registration. Remove obsolete registrations once you've moved their behavior. Keep application-level work, such as push registration and `DidReceiveRemoteNotification`, at the application level. Use `SceneDidDisconnect` for scene-specific cleanup; `WillTerminate` remains an application termination callback.

For example, add `using Microsoft.Maui.LifecycleEvents;` to `MauiProgram.cs` and register scene handlers on your existing `builder`, before `return builder.Build();`:

```csharp
builder.ConfigureLifecycleEvents(events =>
{
#if IOS || MACCATALYST
    events.AddiOS(ios => ios
        .SceneOnActivated(scene =>
            System.Diagnostics.Debug.WriteLine("Scene activated"))
        .SceneDidEnterBackground(scene =>
            System.Diagnostics.Debug.WriteLine("Scene entered background")));
#endif
});
```

For all available registrations and their arguments, see [iOS and Mac Catalyst lifecycle events](../fundamentals/app-lifecycle.md#ios-and-mac-catalyst).

### Move startup work that needs a window

`FinishedLaunching` still runs, but the standard .NET MAUI scene path creates the window later. `AppDelegate.Window` is `null` in this path, including after `base.FinishedLaunching`.

Keep process initialization in `FinishedLaunching`. Move UI work that accesses a window to the owning scene's window after it's created, for example through the cross-platform `Window.Created` or `Window.Activated` event. `SceneWillConnect` runs before .NET MAUI creates the window, so don't navigate from that registration.

### Preserve links, authentication, and notification taps

Test and handle both delivery paths for your app's activation data:

| App state | Where to handle activation data |
| --- | --- |
| A scene is already connected (*warm delivery*). | Use `SceneOpenUrl` for URL contexts and `SceneContinueUserActivity` for user activities, including universal links and Handoff. |
| A new scene is connecting, including when the app isn't running (*cold delivery*). | Inspect `connectionOptions.UrlContexts`, `connectionOptions.UserActivities`, and, for notification taps, `connectionOptions.NotificationResponse` in `SceneWillConnect`. Retain the input and process navigation after that scene's window is ready. |

Guard against absent URL-context and user-activity collections before enumerating them. A normal launch might have neither. .NET MAUI doesn't automatically replay cold-connection URLs or user activities through the warm scene callbacks. These delivery paths apply even when multiple windows are disabled.

In .NET MAUI 10.0.110 and later compatible releases, the framework forwards warm scene URL contexts and user activities with a `WebPageUrl` to `WebAuthenticator` callback handling. This bridge isn't general app deep-link navigation and doesn't process cold-connection activation data. The built-in `ASWebAuthenticationSession` flow receives its result through its own completion handler. Preserve your app-specific link handlers and test authentication redirects separately. For domain association setup, see [Universal links](universal-links.md#add-the-associated-domains-entitlement-to-your-app).

### Preserve App Actions

With the default .NET MAUI builder and packages that include the scene fixes, continue to use `OnAppAction` to respond to App Actions. The framework dispatches both warm scene shortcuts and cold-connection `ShortcutItem` data through its App Actions integration. Don't add duplicate `Platform.PerformActionForShortcutItem` forwarding.

If you override `PerformActionForShortcutItem` in `AppDelegate`, move that custom behavior to a `ConfigureLifecycleEvents` registration for `PerformActionForShortcutItem`. Each custom registration must call its completion callback exactly once, with `true` if it handled the action or `false` otherwise. This requirement also applies to logging-only handlers. For details, see [iOS and Mac Catalyst scene dispatch](../platform-integration/appmodel/app-actions.md#ios-and-mac-catalyst-scene-dispatch).

## Troubleshoot scene-manifest validation

If your installed .NET for iOS and Mac Catalyst workload includes the `ValidateSceneManifest` build check, it validates the merged `Info.plist` for apps built against SDK 27.0 or later. In that implementation, the property defaults to `true`. A missing `UIApplicationSceneManifest` dictionary, or a value of the wrong type, produces a build error.

Availability depends on your .NET Apple workload version, not just Xcode or your .NET MAUI package version. Check the workload's [release notes](https://github.com/dotnet/macios/releases); don't assume every SDK 27 workload includes this check.

### MT7198 and MT7199

The [upstream scene-manifest implementation](https://github.com/dotnet/macios/pull/26936) uses **`MT7199`**. Earlier development versions assigned it `MT7198`. Check the message as well as the number: `MT7198` is now assigned to a code-signing error about extended file attributes, which isn't fixed by this migration. The scene-manifest message identifies `UIApplicationSceneManifest` and links to migration guidance.

If the message reports a missing scene manifest:

1. Add or reconcile the scene delegate and manifest as described above.
1. Check the plist selected by the failing target and build configuration, including partial manifests.
1. Rebuild and inspect the final bundled `Info.plist`. Verify that the scene configuration and registered delegate names match.

This check only detects a missing or incorrectly typed manifest dictionary. It doesn't verify delegate registration, window creation, or custom callbacks. An empty dictionary can pass the check for an app that configures scenes dynamically, but isn't sufficient for the default .NET MAUI configuration shown in this guide. SDK versions without this check can still build an app that fails at launch.

### ValidateSceneManifest

In workloads that include this check, leave validation enabled for a normal .NET MAUI app. If it incorrectly rejects an app with a verified custom scene implementation, you can disable only the build-time check in the app's `.csproj` file:

```xml
<PropertyGroup>
  <ValidateSceneManifest>false</ValidateSceneManifest>
</PropertyGroup>
```

> [!WARNING]
> Setting `ValidateSceneManifest` to `false` doesn't enable scenes or bypass Apple's runtime requirement. Don't use it to avoid migrating a legacy app. Verify your custom implementation on OS 27 or later before distributing the app.

Setting this property has no effect in workloads that don't implement the check.

## Build and verify the migration

On a Mac with the matching Xcode and .NET workloads installed, open a terminal in the app project directory. For a .NET 10 app, restore and build the iOS target:

```dotnetcli
dotnet restore -p:TargetFramework=net10.0-ios
dotnet build -f net10.0-ios --no-restore
```

Use your app's actual target framework if it differs. If your app targets Mac Catalyst, also restore and build that target:

```dotnetcli
dotnet restore -p:TargetFramework=net10.0-maccatalyst
dotnet build -f net10.0-maccatalyst --no-restore
```

Repeat for Release with `-p:Configuration=Release` on restore and `-c Release` on build. Use your normal IDE or deployment workflow to install and launch each build. See [Build an iOS app with .NET CLI](../ios/cli.md) and [Build a Mac Catalyst app with .NET CLI](../mac-catalyst/cli.md).

Inspect the `Info.plist` in each built `.app` bundle to confirm the merged scene manifest. On macOS, you can use `plutil -p "<path-to-app-bundle>/Info.plist"` for iOS, or `plutil -p "<path-to-app-bundle>/Contents/Info.plist"` for Mac Catalyst.

A successful build isn't enough. Exercise Debug and Release builds on iOS 27 or later, and on macOS 27 or later if you target Mac Catalyst:

- Launch the app from a fully closed state and verify that the main UI appears after the splash screen.
- Background and foreground the app, and confirm that custom activation, pause, and resume behavior still runs.
- Open your custom URLs and universal links while the app is running and while it's fully closed. Verify that navigation reaches the intended content after the window is ready.
- Complete authentication flows and verify that redirects return to the app.
- Test notification taps and App Actions, if your app uses them, with the app both running and fully closed.
- If your app supports multiple windows, verify activation, navigation, and cleanup for each window.
- Test the oldest OS versions you continue to support.

If you suspect a stale deployment, uninstall the test app and clean the app project's `bin` and `obj` folders before rebuilding. Uninstalling removes local app data, so use a test device or simulator and preserve any data you need.

> [!NOTE]
> To use an AI agent, install the optional [`maui-xcode27-migration` skill](https://github.com/dotnet/maui-labs/tree/main/plugins/dotnet-maui/skills/maui-xcode27-migration) with [.NET MAUI CLI](../developer-tools/cli/index.md) version `0.1.0-preview.13.26512.5` or later. From your app's repository, run `maui ai add skill maui-xcode27-migration --env CopilotCli --yes`, then reload your agent session and ask it to use the skill. For other agent environments, see `maui ai add skill --help`. This tooling is experimental and requires network access. Installing the skill doesn't migrate your app. Review the agent's changes and complete the same build and runtime checks.

## Related content

- [App lifecycle](../fundamentals/app-lifecycle.md).
- [Migrating to the UIKit scene-based life cycle](https://developer.apple.com/documentation/technotes/tn3187-migrating-to-the-uikit-scene-based-life-cycle).
- [Get your .NET MAUI app ready for Xcode 27](https://devblogs.microsoft.com/dotnet/migrate-dotnet-maui-apps-uikit-scene-lifecycle/).
