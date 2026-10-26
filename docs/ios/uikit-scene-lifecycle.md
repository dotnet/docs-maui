---
title: "Migrate to the UIKit scene lifecycle"
description: Learn how to migrate a .NET MAUI app on iOS and Mac Catalyst to the UIKit scene-based lifecycle, which is required when building with the iOS 27 / Mac Catalyst 27 SDK (Xcode 27).
ms.date: 10/21/2026
ai-usage: ai-assisted
no-loc: [ "UIScene", "SceneDelegate", "AppDelegate" ]
---

# Migrate to the UIKit scene lifecycle

Apps built against the iOS 27 and Mac Catalyst 27 SDK (Xcode 27) must use the UIKit scene-based lifecycle. An app that still uses the legacy `UIApplicationDelegate`-only lifecycle crashes at launch when it's built with Xcode 27 and run on iOS 27 or Mac Catalyst 27. The device log contains the message `UIScene life cycle is required for apps built with this SDK.` For more information, see [TN3187: Migrating to the UIKit scene-based life cycle](https://developer.apple.com/documentation/technotes/tn3187-migrating-to-the-uikit-scene-based-life-cycle) on developer.apple.com.

Apps created from .NET MAUI project templates before the fix don't contain the required scene configuration, and updating the .NET MAUI NuGet packages doesn't add it to existing `Platforms/iOS` and `Platforms/MacCatalyst` files. You have to make the change in your project.

> [!IMPORTANT]
> This requirement is tied to the **SDK you build with** (Xcode 27 / iOS 27 SDK), not to the **minimum OS version you deploy to**. Adopt scenes before you build with Xcode 27, even if your app still supports older operating systems. Raising `SupportedOSPlatformVersion` doesn't fix a missing scene lifecycle.

## Before you start

Use a .NET for iOS and Mac Catalyst workload that supports your selected Xcode version. Independently of scene adoption, the .NET 27 SDKs require a minimum deployment target of iOS 15.0 and Mac Catalyst 17.0 (macOS 14). Keep a higher deployment target if your app already requires one. If you must support older operating systems, use a compatible older SDK rather than silently dropping those users.

Check the project's `global.json`, .NET MAUI package versions, and any configuration-specific `Info.plist` paths before editing. For a .NET 10 app, update to .NET MAUI 10.0.110 or a later compatible servicing release before enabling scenes; it includes the scene-based App Actions dispatch and relevant `WebAuthenticator` forwarding. Don't downgrade a newer compatible version. Apply the migration to every configuration that can produce a shipping app, including separate iOS and Mac Catalyst app projects.

## Quick fix for most apps

Do the following for **both** `Platforms/iOS` and `Platforms/MacCatalyst` (skip a platform your app doesn't target). Keep your existing `AppDelegate`.

1. Add a `SceneDelegate.cs` file to the platform folder:

    ```csharp
    using Foundation;
    using Microsoft.Maui;

    namespace MyApp;

    [Register("SceneDelegate")]
    public class SceneDelegate : MauiUISceneDelegate
    {
    }
    ```

    Use your app's namespace. The class must be `public` and registered as `SceneDelegate`, which is the name referenced by `Info.plist`. If you already have a custom scene delegate, preserve its registration and behavior instead of adding a competing delegate.

1. Add the `UIApplicationSceneManifest` entry to the root `<dict>` of the platform's `Info.plist`. If the key already exists, reconcile its configuration with the scene delegate instead of adding a duplicate:

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

    Keep `__MAUI_DEFAULT_SCENE_CONFIGURATION__` exactly as shown for the default .NET MAUI window-creation path. Match `UISceneDelegateClassName` to the delegate's `[Register]` name. Supporting multiple windows is optional; keep `UIApplicationSupportsMultipleScenes` set to `false` unless the app supports them.

    Don't add `UISceneStoryboardFile`; .NET MAUI creates the window in code. Preserve existing splash-screen and launch-storyboard settings.

1. Build and run the app, then verify the behavior described in [Test the migration](#test-the-migration).

### How it works

When `UIApplicationSceneManifest` is present, `MauiUIApplicationDelegate` selects scene-based startup and returns the `__MAUI_DEFAULT_SCENE_CONFIGURATION__` scene configuration. `MauiUISceneDelegate` then creates the scene-backed window for your app. For details, see the [.NET MAUI pull request that updated the templates to use scenes](https://github.com/dotnet/maui/pull/38601).

## Preserve existing customizations

If your app already customizes startup, don't overwrite it blindly.

- **Existing `UIApplicationSceneManifest`**: preserve working scene configurations and any custom `SceneDelegate` subclass. Review custom configuration names together with `GetConfiguration` and `WillConnect` overrides: names other than `__MAUI_DEFAULT_SCENE_CONFIGURATION__` need custom window creation.
- **Custom `SceneDelegate` overrides**: always call the `base` implementation.
- **Application-level lifecycle callbacks**: with scenes, lifecycle events and URL/activity handling move to scene-level events. If you register lifecycle events through `ConfigureLifecycleEvents`, move these registrations:

    | Legacy (application) event | Scene event                 |
    |----------------------------|-----------------------------|
    | `OnActivated`              | `SceneOnActivated`          |
    | `OnResignActivation`       | `SceneOnResignActivation`   |
    | `DidEnterBackground`       | `SceneDidEnterBackground`   |
    | `WillEnterForeground`      | `SceneWillEnterForeground`  |
    | `OpenUrl`                  | `SceneOpenUrl`              |
    | `ContinueUserActivity`     | `SceneContinueUserActivity` |

    The callback arguments differ from the application-level versions, so review each handler rather than renaming it.

    ```csharp
    builder.ConfigureLifecycleEvents(events =>
    {
    #if IOS || MACCATALYST
        events.AddiOS(ios => ios
            .SceneOnActivated((scene) => { /* ... */ })
            .SceneOpenUrl((scene, urlContexts) => { /* ... */ return false; }));
    #endif
    });
    ```

- **Cold-start deep links and activities**: URLs and user activities that launch the app arrive in the connection options of `SceneWillConnect`, not through the warm scene URL/activity callbacks. Handle these connection options in addition to the warm callbacks. `SceneWillConnect` runs before .NET MAUI creates the scene's window, so retain activation data and defer navigation until that window is created or activated. In the standard scene startup path, `AppDelegate.Window` remains `null`, including after `base.FinishedLaunching`; move startup code that needs a window to the owning scene's window when it's ready. Review notification-tap and authentication handling separately, preserving the relevant notification delegate and authentication-session completion callbacks.
- **App Actions**: if you override `AppDelegate.PerformActionForShortcutItem`, move that logic to a `ConfigureLifecycleEvents` registration. .NET MAUI 10.0.110 already forwards warm and cold scene shortcuts, so don't add duplicate `Platform.PerformActionForShortcutItem` forwarding. Each custom registration, including one that only logs, must call its completion callback exactly once. An app can launch successfully while shortcuts stop working, so test them. For more information, see [App actions](../platform-integration/appmodel/app-actions.md).
- **Dynamic scene configuration**: Apple also supports providing a configuration by overriding `UIApplicationDelegate.GetConfiguration` (`UIApplicationDelegate.application(_:configurationForConnecting:options:)` in Objective-C and Swift) instead of declaring scene configurations in `Info.plist`. In a standard .NET MAUI app, you still need at least an empty `UIApplicationSceneManifest` dictionary in `Info.plist`. Without the key, `MauiUIApplicationDelegate` hides that callback and creates a legacy window. The build-time check described below accepts an empty dictionary.

## Build-time check

In .NET for iOS and Mac Catalyst workloads that include scene-manifest validation, building against SDK 27 or later checks that the app's final merged `Info.plist` contains a `UIApplicationSceneManifest` dictionary, and reports error **MT7199** if it's missing or isn't a dictionary. Check your workload's release notes for availability; a successful build with an older workload doesn't prove scene adoption. An empty dictionary is accepted, which supports dynamic scene configuration. App extensions, frameworks, and libraries are skipped. Earlier drafts of this check used MT7198, which is now a different error (an extended-attributes signing error).

The error message reads: "The app's Info.plist does not contain a UIApplicationSceneManifest dictionary. Apps built with the iOS or Mac Catalyst 27.0 SDK or later must use the UIKit scene lifecycle or they will crash at launch on iOS or macOS 27.0 or later. Add a scene manifest and scene delegate, and migrate custom lifecycle handlers as described at https://aka.ms/uiscene. To skip this build-time validation, set the MSBuild property ValidateSceneManifest to false; this does not disable the operating system's requirement."

The check is a heuristic on the manifest. It doesn't prove that your scene delegate or window is wired up correctly. Opting out only disables this build-time validation; it isn't a substitute for the manifest in a standard .NET MAUI app, even if you configure scenes dynamically. To skip the check, opt out in the project file:

```xml
<PropertyGroup>
  <ValidateSceneManifest>false</ValidateSceneManifest>
</PropertyGroup>
```

or on the command line: `-p:ValidateSceneManifest=false`. Opting out doesn't prevent the launch crash on iOS 27 / Mac Catalyst 27.

> [!NOTE]
> You can use the experimental [`maui-xcode27-migration` agent skill](https://github.com/dotnet/maui-labs/tree/main/plugins/dotnet-maui/skills/maui-xcode27-migration) to help inspect and migrate your project. Install or update the [.NET MAUI CLI](../developer-tools/cli/index.md) to version `0.1.0-preview.13.26512.5` or later, then run `maui ai add skill maui-xcode27-migration --env CopilotCli --yes` from your app's repository. Reload your agent session and ask it to use the skill. For other agent environments, see `maui ai add skill --help`. This preview tooling isn't covered by the Microsoft Support Policy. Installing the skill doesn't migrate your app; review the agent's changes and complete the same verification steps as for a manual migration.

## Test the migration

Test Debug and Release builds on iOS 27 / Mac Catalyst 27 and on the oldest OS version your app supports:

- Cold launch and warm launch.
- Foreground and background transitions.
- Deep links (cold start and while running), universal links, and authentication redirects such as `WebAuthenticator`.
- App actions (home screen quick actions), push notification taps, and any other launch-from-activity scenarios.
- Any custom lifecycle event handlers you migrated.

## See also

- [TN3187: Migrating to the UIKit scene-based life cycle](https://developer.apple.com/documentation/technotes/tn3187-migrating-to-the-uikit-scene-based-life-cycle)
- [Migrate .NET MAUI apps to the UIKit scene lifecycle](https://devblogs.microsoft.com/dotnet/migrate-dotnet-maui-apps-uikit-scene-lifecycle/)
- [dotnet/maui pull request 38601](https://github.com/dotnet/maui/pull/38601)
