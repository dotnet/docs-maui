---
title: "iOS home screen widgets"
description: "Learn how to add iOS home screen widgets to your .NET MAUI app using a Swift widget extension with App Group shared containers."
ms.date: 03/06/2026
---

# iOS home screen widgets

iOS home screen widgets let your .NET Multi-platform App UI (.NET MAUI) app display glanceable content and interactive controls directly on the user's home screen. Because Apple's WidgetKit framework requires SwiftUI, widgets are built as a separate Xcode project that's embedded into your .NET MAUI app at build time. The .NET MAUI app and the widget extension communicate through JSON files stored in an App Group shared container.

This guide is based on the [Maui.Apple.PlatformFeature.Samples](https://github.com/Redth/Maui.Apple.PlatformFeature.Samples) project on GitHub.

This guide walks you through creating a widget extension, wiring up shared data, handling deep links, and configuring your project to build and embed the extension automatically.

## Architecture

A widget extension runs in its own process and can't directly access your app's memory or state. All communication flows through an App Group shared container, which is a directory on disk that both the app and the extension can read and write to.

The following table summarizes the three communication channels between your app and the widget:

| Direction | Mechanism | How it works |
|---|---|---|
| **App → Widget** | JSON file + WidgetKit reload | The app writes a JSON file to the shared container and calls `WidgetCenter.ReloadTimelines` to tell the widget to refresh. |
| **Widget → App (tap)** | Deep link URL scheme | The widget uses a SwiftUI `Link` or `widgetURL` to open a URL with your app's custom scheme. The app receives it in `OpenUrl`. |
| **Widget → App (interactive)** | AppIntent buttons | A `Button(intent:)` in the widget runs an `AppIntent` that writes a JSON file to the shared container. The app reads it on next launch or resume. |

> [!NOTE]
> Interactive widget buttons (AppIntents) are available on iOS 17 and later. On earlier versions, only tap-to-open deep links are supported.

## Prerequisites

Before you begin, make sure you have:

- macOS with Xcode 16 or later installed.
- .NET 10 SDK or later with the .NET MAUI workload.
- An iOS 14+ device or simulator. Widgets require iOS 14 or later; interactive widget buttons (AppIntents) require iOS 17 or later.
- [xcodegen](https://github.com/yonaskolb/XcodeGen) (optional). Generates an Xcode project from a YAML specification, which avoids checking in `.xcodeproj` files.

## Create the widget extension

The widget is an Xcode project that lives inside your .NET MAUI solution. You can create it in Xcode or generate it with xcodegen.

### Create the Xcode project

1. Open Xcode and create a new project using the **App** template with Swift.
1. Set the bundle identifier to match your .NET MAUI app's bundle ID. This Xcode project serves only as a development container for the widget and won't ship as a standalone app.
1. Go to **File > New > Target** and choose the **Widget Extension** template.
1. Enter a name for your widget extension (for example, `SimpleWidgetExtension`). The bundle identifier must be a child of your app's bundle ID (for example, `com.contoso.myapp.SimpleWidgetExtension`).
1. Ensure all targets use the same minimum iOS deployment version by selecting the project name, navigating to each target's **General** tab, and setting **Minimum Deployments**.

Build and run the widget extension on a simulator to verify the template works before customizing it.

> [!TIP]
> Alternatively, use [xcodegen](https://github.com/yonaskolb/XcodeGen) to generate the `.xcodeproj` from a `project.yml` file. This avoids checking in Xcode project files and simplifies merge conflicts.

### Project structure

A typical directory structure looks like this:

```text
YourApp/
├── YourApp.csproj
├── MauiProgram.cs
├── Platforms/
│   └── iOS/
│       ├── AppDelegate.cs
│       └── Entitlements.plist
└── widget/
    ├── project.yml           # xcodegen spec (optional)
    ├── SimpleWidget/
    │   ├── Settings.swift
    │   ├── WidgetData.swift
    │   ├── SharedStorage.swift
    │   ├── Provider.swift
    │   ├── SimpleWidgetView.swift
    │   ├── IncrementCounterIntent.swift
    │   └── SimpleWidgetBundle.swift
    └── SimpleWidgetExtension.entitlements
```

### Widget components

A widget extension consists of several key components:

- **WidgetBundle**: The `@main` entry point that exposes one or more widgets.
- **Widget**: The configuration object that defines the widget's view, provider, and supported sizes.
- **AppIntentTimelineProvider**: Supplies data to the widget. It includes `placeholder` (loading state), `snapshot` (gallery preview), and `timeline` (live data) methods.
- **TimelineEntry**: The data model structure the widget displays.
- **View**: The SwiftUI view that renders the widget.

The following sections describe each Swift file and its role.

### Settings.swift – constants that must match C# <!-- markdownlint-disable-line MD020 -->

Define constants for the App Group identifier, file names, widget kind, and URL scheme. These values must exactly match the corresponding C# constants in your .NET MAUI project:

```swift
import Foundation

struct Settings {
    static let groupId = "group.com.yourapp"
    static let fromAppFile = "widget_data_fromapp.json"
    static let fromWidgetFile = "widget_data_fromwidget.json"
    static let widgetKind = "SimpleWidget"
    static let urlScheme = "yourapp"
    static let urlHost = "widget"
}
```

### WidgetData.swift – shared JSON contract

Define the data structure that both Swift and C# serialize and deserialize. Keep this structure flat and simple so that both sides can work with it reliably:

```swift
import Foundation

struct WidgetData: Codable {
    var version: Int = 1
    var title: String = ""
    var message: String = ""
    var counter: Int = 0
    var updatedAt: String = ""
    var extras: [String: String] = [:]
}
```

### SharedStorage.swift – file I/O via App Group

The shared storage class reads and writes JSON files in the App Group container. This is the core mechanism for passing data between the app and the widget.

```swift
import Foundation

struct SharedStorage {
    private func containerURL() -> URL? {
        FileManager.default.containerURL(
            forSecurityApplicationGroupIdentifier: Settings.groupId
        )
    }

    func readAppData() -> WidgetData? {
        guard let url = containerURL()?.appendingPathComponent(
            Settings.fromAppFile) else { return nil }
        guard let data = try? Data(contentsOf: url) else { return nil }
        return try? JSONDecoder().decode(WidgetData.self, from: data)
    }

    func readWidgetData() -> WidgetData? {
        guard let url = containerURL()?.appendingPathComponent(
            Settings.fromWidgetFile) else { return nil }
        guard let data = try? Data(contentsOf: url) else { return nil }
        return try? JSONDecoder().decode(WidgetData.self, from: data)
    }

    func writeWidgetData(_ widgetData: WidgetData) {
        guard let url = containerURL()?.appendingPathComponent(
            Settings.fromWidgetFile) else { return }
        guard let data = try? JSONEncoder().encode(widgetData)
            else { return }
        try? data.write(to: url)
    }

    func getBestCounter() -> Int {
        let appData = readAppData()
        let widgetData = readWidgetData()

        let appDate = parseDate(appData?.updatedAt)
        let widgetDate = parseDate(widgetData?.updatedAt)

        if let ad = appDate, let wd = widgetDate {
            return ad > wd
                ? (appData?.counter ?? 0) : (widgetData?.counter ?? 0)
        }
        return widgetData?.counter ?? appData?.counter ?? 0
    }

    private func parseDate(_ str: String?) -> Date? {
        guard let str = str else { return nil }
        return ISO8601DateFormatter().date(from: str)
    }
}
```

> [!IMPORTANT]
> Use file-based I/O instead of `UserDefaults(suiteName:)`. Although `UserDefaults` with a suite name is commonly recommended, the suite name can resolve to different `.plist` files for the app process and the extension process on some configurations. In certain iOS configurations, `UserDefaults(suiteName:)` can resolve to different .plist paths for the app process vs. the extension process, causing data to silently not synchronize. File-based I/O through the App Group container directory is more reliable.

The `getBestCounter` method compares timestamps from both files to determine which counter value is most recent. This avoids race conditions when the app and widget both update the counter.

### Provider.swift – timeline provider

The timeline provider tells WidgetKit when and how to render the widget. Use `AppIntentTimelineProvider` to support interactive widgets:

```swift
import WidgetKit
import SwiftUI

struct Provider: AppIntentTimelineProvider {
    typealias Entry = SimpleEntry
    typealias Intent = ConfigurationAppIntent

    func placeholder(in context: Context) -> SimpleEntry {
        SimpleEntry(date: Date(), data: WidgetData())
    }

    func snapshot(for intent: ConfigurationAppIntent,
                  in context: Context) async -> SimpleEntry {
        let storage = SharedStorage()
        let data = storage.readAppData() ?? WidgetData()
        return SimpleEntry(date: Date(), data: data)
    }

    func timeline(for intent: ConfigurationAppIntent,
                  in context: Context) async -> Timeline<SimpleEntry> {
        let storage = SharedStorage()
        let data = storage.readAppData() ?? WidgetData()
        let entry = SimpleEntry(date: Date(), data: data)
        return Timeline(entries: [entry], policy: .never)
    }
}

struct SimpleEntry: TimelineEntry {
    let date: Date
    let data: WidgetData
}

struct ConfigurationAppIntent: WidgetConfigurationIntent {
    static var title: LocalizedStringResource { "Configuration" }
    static var description: IntentDescription { "Simple widget." }
}
```

> [!TIP]
> The `.never` refresh policy means the widget only refreshes when your app explicitly calls `WidgetCenter.ReloadTimelines`. This avoids unnecessary background work and gives you full control over when the widget updates.

### SimpleWidgetView.swift – the SwiftUI view

The widget view displays data from the shared storage and provides interaction through deep links and buttons:

```swift
import SwiftUI
import WidgetKit

struct SimpleWidgetView: View {
    var entry: Provider.Entry

    var body: some View {
        VStack(alignment: .leading, spacing: 6) {
            // Tapping this text opens the app via deep link
            Link(destination: URL(
                string: "\(Settings.urlScheme)://\(Settings.urlHost)/action"
            )!) {
                Text(entry.data.title.isEmpty
                    ? "My Widget" : entry.data.title)
                    .font(.headline)
            }

            Text("Counter: \(entry.data.counter)")
                .font(.body)

            // Interactive button – runs an AppIntent
            Button(intent: IncrementCounterIntent()) {
                Label("Increment", systemImage: "plus.circle.fill")
            }
            .buttonStyle(.borderedProminent)
            .tint(.blue)
        }
        .padding()
    }
}
```

> [!WARNING]
> Do not wrap a `Button(intent:)` inside a `widgetURL()` modifier or a `Link`. Both `widgetURL` and `Link` intercept all taps in their view hierarchy, which prevents the button's intent from firing. If your layout uses `widgetURL` to make the entire widget tappable, place `Button(intent:)` controls outside that container. Similarly, do not nest a `Button(intent:)` inside a `Link` view.

### Interactive buttons with AppIntents

AppIntents let the widget perform actions without opening the app. The following intent increments the counter, writes the result to the shared container, and tells WidgetKit to reload:

```swift
import AppIntents
import WidgetKit

struct IncrementCounterIntent: AppIntent {
    static var title: LocalizedStringResource { "Increment Counter" }
    static var description: IntentDescription {
        "Increments the counter by 1"
    }

    func perform() async throws -> some IntentResult {
        let storage = SharedStorage()
        let currentCount = storage.getBestCounter()
        let newCount = currentCount + 1

        let data = WidgetData(
            version: 1,
            title: "",
            message: "incremented via widget",
            counter: newCount,
            updatedAt: ISO8601DateFormatter().string(from: Date()),
            extras: [:]
        )

        storage.writeWidgetData(data)
        WidgetCenter.shared.reloadTimelines(ofKind: Settings.widgetKind)
        return .result()
    }
}
```

> [!TIP]
> For a read-only widget without interactive buttons, you can omit the AppIntents, `Button(intent:)`, and the widget-to-app file. Only implement `SharedStorage.readAppData()`, the timeline `Provider`, a `SimpleWidgetView` with `Link` for taps, and `SendDataToWidget` + `RefreshWidget` in C#.

### Widget bundle entry point

Register the widget in a `WidgetBundle`:

```swift
import WidgetKit
import SwiftUI

@main
struct SimpleWidgetBundle: WidgetBundle {
    var body: some Widget {
        SimpleWidget()
    }
}

struct SimpleWidget: Widget {
    let kind: String = Settings.widgetKind

    var body: some WidgetConfiguration {
        AppIntentConfiguration(
            kind: kind,
            intent: ConfigurationAppIntent.self,
            provider: Provider()
        ) { entry in
            SimpleWidgetView(entry: entry)
                .containerBackground(.fill.tertiary, for: .widget)
        }
        .configurationDisplayName("Simple Widget")
        .description("A sample .NET MAUI widget.")
        .supportedFamilies([.systemSmall, .systemMedium])
    }
}
```

## Set up the .NET MAUI project

### Shared data contract (C#) <!-- markdownlint-disable-line MD020 -->

Create a C# record that mirrors the Swift `WidgetData` struct. Use `JsonPropertyName` attributes to match the Swift property names exactly:

```csharp
using System.Text.Json.Serialization;

namespace YourApp.Models;

public record WidgetData
{
    [JsonPropertyName("version")]
    public int Version { get; set; } = 1;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("counter")]
    public int Counter { get; set; }

    [JsonPropertyName("updatedAt")]
    public string UpdatedAt { get; set; } = string.Empty;

    [JsonPropertyName("extras")]
    public Dictionary<string, string> Extras { get; set; } = [];
}
```

Define a constants class that mirrors `Settings.swift`:

```csharp
namespace YourApp;

public static class WidgetConstants
{
    public const string GroupId = "group.com.yourapp";
    public const string FromAppFile = "widget_data_fromapp.json";
    public const string FromWidgetFile = "widget_data_fromwidget.json";
    public const string WidgetKind = "SimpleWidget";
    public const string UrlScheme = "yourapp";
    public const string UrlHost = "widget";
}
```

> [!IMPORTANT]
> If any of these constants differ between the Swift and C# code, data sharing silently fails. Double-check that `GroupId`, file names, `WidgetKind`, and the URL scheme match exactly.

### Widget data service

Define an interface for reading and writing widget data:

```csharp
using YourApp.Models;

namespace YourApp.Services;

public interface IWidgetDataService
{
    WidgetData? ReadAppData();
    WidgetData? ReadWidgetData();
    void WriteAppData(WidgetData data);
    void ClearWidgetData();
    void RefreshWidget();
}
```

On iOS, implement the service using `NSFileManager` to access the App Group container:

```csharp
#if IOS
using Foundation;
using System.Text.Json;
using YourApp.Models;
using WidgetKit;

namespace YourApp.Services;

public class AppleWidgetDataService : IWidgetDataService
{
    private string? GetContainerPath()
    {
        var url = NSFileManager.DefaultManager
            .GetContainerUrl(WidgetConstants.GroupId);
        return url?.Path;
    }

    public WidgetData? ReadAppData()
    {
        return ReadFile(WidgetConstants.FromAppFile);
    }

    public WidgetData? ReadWidgetData()
    {
        return ReadFile(WidgetConstants.FromWidgetFile);
    }

    public void WriteAppData(WidgetData data)
    {
        data.UpdatedAt = DateTime.UtcNow.ToString("o");
        var path = GetFilePath(WidgetConstants.FromAppFile);
        if (path is null) return;

        var json = JsonSerializer.Serialize(data);
        File.WriteAllText(path, json);
    }

    public void ClearWidgetData()
    {
        var path = GetFilePath(WidgetConstants.FromWidgetFile);
        if (path is not null && File.Exists(path))
            File.Delete(path);
    }

    public void RefreshWidget()
    {
        // From the WidgetKit.WidgetCenterProxy NuGet package
        WidgetCenterProxy.ReloadTimelines(WidgetConstants.WidgetKind);
    }

    private string? GetFilePath(string fileName)
    {
        var container = GetContainerPath();
        return container is null ? null : Path.Combine(container, fileName);
    }

    private WidgetData? ReadFile(string fileName)
    {
        var path = GetFilePath(fileName);
        if (path is null || !File.Exists(path)) return null;

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<WidgetData>(json);
    }
}
#endif
```

For non-iOS platforms, provide a stub implementation so that your shared code compiles:

```csharp
using YourApp.Models;

namespace YourApp.Services;

public class StubWidgetDataService : IWidgetDataService
{
    public WidgetData? ReadAppData() => null;
    public WidgetData? ReadWidgetData() => null;
    public void WriteAppData(WidgetData data) { }
    public void ClearWidgetData() { }
    public void RefreshWidget() { }
}
```

Register the service in `MauiProgram.cs`:

```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder.UseMauiApp<App>();

#if IOS
    builder.Services.AddSingleton<IWidgetDataService, AppleWidgetDataService>();
#else
    builder.Services.AddSingleton<IWidgetDataService, StubWidgetDataService>();
#endif

    return builder.Build();
}
```

### Handle deep links

When a user taps the widget (not an interactive button), the widget opens your app through a deep link URL. Handle this in `AppDelegate.cs`:

```csharp
using Foundation;
using UIKit;

namespace YourApp.Platforms.iOS;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    [Export("application:openURL:options:")]
    public override bool OpenUrl(
        UIApplication application,
        NSUrl url,
        NSDictionary options)
    {
        if (url.Scheme == WidgetConstants.UrlScheme
            && url.Host == WidgetConstants.UrlHost)
        {
            if (App.Current is App app)
            {
                app.HandleWidgetUrl(url.AbsoluteString ?? string.Empty);
            }
        }

        return base.OpenUrl(application, url, options);
    }
}
```

> [!NOTE]
> The URL scheme must be registered in your app's `Info.plist` for iOS to route widget taps to your app:
>
> ```xml
> <key>CFBundleURLTypes</key>
> <array>
>     <dict>
>         <key>CFBundleURLSchemes</key>
>         <array>
>             <string>yourapp</string>
>         </array>
>     </dict>
> </array>
> ```

In your `App.xaml.cs`, add the following method:

```csharp
public void HandleWidgetUrl(string url)
{
    // Parse the URL path and query to determine which action to take.
    // For example, navigate to a specific page or update state.
    MainThread.BeginInvokeOnMainThread(() =>
    {
        // Handle the deep link action
    });
}
```

### Send data to the widget

After a user action (for example, tapping a button on `MainPage`), write data to the shared container and tell the widget to refresh:

```csharp
private int _currentCounter = 0;

private void OnSendToWidgetClicked(object sender, EventArgs e)
{
    var service = App.Current?.Handler?.MauiContext?
        .Services.GetRequiredService<IWidgetDataService>();
    if (service is null) return;

    var data = new WidgetData
    {
        Title = "Hello from MAUI",
        Message = "Updated at " + DateTime.Now.ToString("t"),
        Counter = _currentCounter,
    };

    service.WriteAppData(data);
    service.RefreshWidget();
}
```

### Read data from the widget

Check for incoming widget data when the page appears and when the app resumes:

```csharp
protected override void OnAppearing()
{
    base.OnAppearing();
    ReadWidgetData();
}

private void ReadWidgetData()
{
    var service = App.Current?.Handler?.MauiContext?
        .Services.GetRequiredService<IWidgetDataService>();
    if (service is null) return;

    var data = service.ReadWidgetData();
    if (data is null) return;

    _currentCounter = data.Counter;
    CounterLabel.Text = $"Counter: {data.Counter}";
    StatusLabel.Text = $"Widget said: {data.Message}";

    // Clear after reading to avoid processing stale data
    service.ClearWidgetData();
}
```

> [!NOTE]
> The preceding code assumes `Label` controls named `CounterLabel` and `StatusLabel` are defined in your XAML.

## Configure entitlements

Both the .NET MAUI app and the widget extension must declare the same App Group in their entitlements.

Create `Platforms/iOS/Entitlements.plist` for the .NET MAUI app:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN"
  "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>com.apple.security.application-groups</key>
    <array>
        <string>group.com.yourapp</string>
    </array>
</dict>
</plist>
```

Create `widget/SimpleWidgetExtension.entitlements` for the widget extension:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN"
  "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>com.apple.security.application-groups</key>
    <array>
        <string>group.com.yourapp</string>
    </array>
</dict>
</plist>
```

> [!IMPORTANT]
> The App Group identifier (for example, `group.com.yourapp`) must be identical in both entitlements files. The widget extension's bundle identifier must also be a child of the app's bundle identifier. For example, if the app is `com.yourcompany.yourapp`, the widget must be something like `com.yourcompany.yourapp.widget`.

> [!NOTE]
> You must also create the App Group identifier in the [Apple Developer portal](https://developer.apple.com/account/resources/identifiers/list/applicationGroup) and add it to the provisioning profiles for both the app and the widget extension.

For more information about iOS entitlements, see [iOS entitlements](entitlements.md). For information about capabilities, see [iOS capabilities](capabilities.md).

## Configure the project file

Your `.csproj` file needs several additions to build the Swift widget extension and embed it into the app bundle. Add the following elements:

### Entitlements and NuGet package

```xml
<PropertyGroup Condition="$(TargetFramework.Contains('-ios'))">
    <CodesignEntitlements>Platforms/iOS/Entitlements.plist</CodesignEntitlements>
</PropertyGroup>

<ItemGroup Condition="$(TargetFramework.Contains('-ios'))">
    <PackageReference Include="WidgetKit.WidgetCenterProxy"
                      Version="1.0.0-preview.2" />
</ItemGroup>
```

The `WidgetKit.WidgetCenterProxy` package provides a C# API to call `WidgetCenter.reloadTimelines` from your .NET MAUI code.

### Embed the widget extension

Tell the build system to include the compiled `.appex` in the app bundle:

```xml
<ItemGroup Condition="$(TargetFramework.Contains('-ios'))">
    <AdditionalAppExtensions
        Include="$(MSBuildProjectDirectory)/WidgetExtensions/SimpleWidgetExtension.appex"
        Condition="Exists('$(MSBuildProjectDirectory)/WidgetExtensions/SimpleWidgetExtension.appex')">
        <ApplicationIdentifier>com.yourcompany.yourapp.SimpleWidgetExtension</ApplicationIdentifier>
    </AdditionalAppExtensions>
</ItemGroup>
```

### Build the widget extension automatically

Add an MSBuild target that compiles the Xcode project before the .NET MAUI build. This target detects whether you're building for the simulator or a device, optionally runs `xcodegen`, and invokes `xcodebuild`:

```xml
<Target Name="BuildWidgetExtension"
        BeforeTargets="_CompileAppManifest"
        Condition="$(TargetFramework.Contains('-ios'))">

    <!-- Determine SDK and architecture -->
    <PropertyGroup>
        <_WidgetSdk
            Condition="$(RuntimeIdentifier.Contains('simulator'))">iphonesimulator</_WidgetSdk>
        <_WidgetSdk
            Condition="!$(RuntimeIdentifier.Contains('simulator'))">iphoneos</_WidgetSdk>
        <_WidgetArch
            Condition="$(RuntimeIdentifier.Contains('arm64'))">arm64</_WidgetArch>
        <_WidgetArch
            Condition="!$(RuntimeIdentifier.Contains('arm64'))">x86_64</_WidgetArch>
        <_WidgetConfig
            Condition="'$(Configuration)' == 'Debug'">Debug</_WidgetConfig>
        <_WidgetConfig
            Condition="'$(Configuration)' != 'Debug'">Release</_WidgetConfig>
        <_WidgetProjectDir>$(MSBuildProjectDirectory)/widget</_WidgetProjectDir>
        <_WidgetOutputDir>$(MSBuildProjectDirectory)/WidgetExtensions</_WidgetOutputDir>
    </PropertyGroup>

    <!-- Run xcodegen if project.yml exists but .xcodeproj does not -->
    <Exec
        Condition="Exists('$(_WidgetProjectDir)/project.yml') And
                   !Exists('$(_WidgetProjectDir)/widget.xcodeproj')"
        Command="cd $(_WidgetProjectDir) &amp;&amp; xcodegen generate"
        ConsoleToMSBuild="true" />

    <!-- Build the widget extension -->
    <Exec Command="xcodebuild
        -project $(_WidgetProjectDir)/widget.xcodeproj
        -scheme SimpleWidgetExtension
        -sdk $(_WidgetSdk)
        -arch $(_WidgetArch)
        -configuration $(_WidgetConfig)
        CONFIGURATION_BUILD_DIR=$(_WidgetOutputDir)
        CODE_SIGNING_ALLOWED=NO
        ENABLE_USER_SCRIPT_SANDBOXING=NO"
        ConsoleToMSBuild="true" />

</Target>
```

> [!NOTE]
> Setting `CODE_SIGNING_ALLOWED=NO` in `xcodebuild` lets the .NET build system handle all code signing. The widget extension is re-signed as part of the .NET MAUI app's signing step.

> [!NOTE]
> On CI agents (GitHub Actions, Azure DevOps), ensure `xcodebuild` is available by installing Xcode. Optionally set `DEVELOPER_DIR` to pin the Xcode version. The `BuildWidgetExtension` target runs automatically during `dotnet build` on any macOS agent with Xcode installed.

## Build and test

Build the app from the command line:

```bash
dotnet build YourApp.csproj -f net10.0-ios -r iossimulator-arm64 \
    -p:CodesignRequireProvisioningProfile=false
```

For device builds, use `ios-arm64` as the runtime identifier and supply your provisioning profile. Pass signing properties: `-p:CodesignKey="Apple Distribution: ..." -p:CodesignProvision="YourProfileName"`. For more information, see [iOS device provisioning](device-provisioning/index.md).

### Simulator testing

When testing on the simulator, you may need to re-sign the app after building to apply entitlements correctly:

```bash
APP_PATH="bin/Debug/net10.0-ios/iossimulator-arm64/YourApp.app"
codesign -v --force --sign - \
    --entitlements Platforms/iOS/Entitlements.plist \
    "$APP_PATH"
```

> [!NOTE]
> The ad-hoc signing identity (`-`) is for simulator builds only. Device, TestFlight, and App Store builds require a valid signing identity configured through your provisioning profile.

After deploying, long-press the home screen and tap **+** to add a widget. Your widget appears in the gallery under the app name.

> [!TIP]
> If you make changes to the widget's Swift code, delete the `WidgetExtensions` directory and rebuild to ensure the latest `.appex` is generated.

## Troubleshooting

| Problem | Solution |
|---|---|
| Widget doesn't appear in the widget gallery. | Verify the widget extension's bundle identifier is a child of the app's bundle identifier (for example, `com.yourcompany.yourapp.SimpleWidgetExtension`). |
| Data isn't syncing between app and widget. | Confirm the App Group identifier matches exactly in both the app's and the widget extension's entitlements files. |
| `UserDefaults(suiteName:)` returns stale or nil data. | Switch to file-based I/O through the App Group container directory. See the [SharedStorage.swift](#sharedstorageswift--file-io-via-app-group) section. |
| Widget buttons don't respond to taps. | Don't wrap `Button(intent:)` inside a `widgetURL()` modifier or `Link`. These intercept all taps and prevent the intent from running. |
| App Group doesn't work on simulator after build. | Re-sign the app with entitlements after building. See [Simulator testing](#simulator-testing). |
| Build fails with entitlements parsing error. | Ensure your `Entitlements.plist` files use LF line endings, not CRLF. Some editors and source control systems convert line endings automatically. |
| `xcodebuild` can't find the scheme. | If you use `xcodegen`, make sure `project.yml` defines the `SimpleWidgetExtension` scheme, or run `xcodegen generate` manually in the `widget` directory. |

## See also

- [How to Build iOS Widgets with .NET MAUI](https://devblogs.microsoft.com/dotnet/how-to-build-ios-widgets-with-dotnet-maui/) — blog post with a complete working example.
- [Maui.Apple.PlatformFeature.Samples on GitHub](https://github.com/Redth/Maui.Apple.PlatformFeature.Samples) — the sample project this guide is based on.
- [Apple WidgetKit documentation](https://developer.apple.com/documentation/widgetkit)
- [Apple App Groups documentation](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_security_application-groups)
