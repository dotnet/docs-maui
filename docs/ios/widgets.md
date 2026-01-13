---
title: "Bundle Swift widgets with .NET MAUI iOS apps"
description: "Learn how to create iOS widgets using Swift and WidgetKit, and bundle them with your .NET MAUI iOS app."
ms.date: 01/13/2026
---

# Bundle Swift widgets with .NET MAUI iOS apps

iOS widgets are small, focused apps that display timely information on the home screen or lock screen. While .NET MAUI doesn't provide direct APIs for creating widgets, you can create widgets using Swift and Apple's WidgetKit framework in Xcode, then bundle them with your .NET MAUI app. This approach gives you full access to native widget capabilities while maintaining your main app logic in .NET MAUI.

This article demonstrates how to create an iOS widget extension in Xcode, integrate it with your .NET MAUI app, and enable data sharing and communication between them.

> [!NOTE]
> iOS widgets are standalone extensions that are bundled with a host app. This article refers to the .NET MAUI app as the "app" and the widget extension as the "widget".

For more information about iOS widgets, see [Creating a widget extension](https://developer.apple.com/documentation/widgetkit/creating-a-widget-extension) on developer.apple.com. For a complete working example, see the blog post [How to Build iOS Widgets with .NET MAUI](https://devblogs.microsoft.com/dotnet/how-to-build-ios-widgets-with-dotnet-maui/).

## Prerequisites

Before you begin, ensure you have the following:

- A Mac with Xcode installed
- An active Apple Developer Account
- A .NET MAUI app targeting iOS
- Access to the [Apple Developer Console](https://developer.apple.com/account)

You'll also need to configure the following in your Apple Developer Account:

- **App Bundle ID**: The bundle identifier for your .NET MAUI app (for example, `com.contoso.MyApp`)
- **Widget Bundle ID**: A bundle identifier for your widget extension (for example, `com.contoso.MyApp.WidgetExtension`)
- **App Group ID**: A shared group identifier for data sharing between the app and widget (for example, `group.com.contoso.MyApp`)

Both the app and widget bundle IDs must be enabled for the **App Groups** capability with the same group identifier.

## Create the widget project in Xcode

To create a widget, you'll first create an Xcode app project as a container, then add the widget extension to it:

01. Open Xcode and create a new project using the **App** template with Swift.
01. Set the bundle identifier to match your .NET MAUI app's bundle ID. This Xcode project serves only as a development container and won't be shipped with your app.
01. In Xcode, go to **File > New > Target** and choose the **Widget Extension** template.
01. Enter a name for your widget extension and ensure the bundle identifier matches the widget bundle ID you created in your Apple Developer Account.
01. Select the **Include Configuration Intent** option to enable widget configuration and generate sample code.
01. Ensure all targets in the Xcode project use the same minimum iOS version by selecting the project name, navigating to each target's **General** tab, and setting the **Minimum Deployments** iOS version.

Build and run the widget extension on a device or simulator to verify it works correctly.

### Understand the widget structure

A widget extension in Xcode consists of several key components:

- **WidgetBundle**: The entry point of the widget extension that exposes one or more widgets to the user.
- **Widget**: The configuration object that defines the widget's view, provider, configuration intent, and supported sizes.
- **AppIntentTimelineProvider**: Provides data models to the widget according to a timeline. It includes:
  - `placeholder`: Returns a minimal data model displayed while the widget loads.
  - `snapshot`: Returns a data model for the widget gallery preview and when first added to the screen.
  - `timeline`: Returns data models for normal widget operation.
- **TimelineEntry**: The data model structure containing the information the widget displays.
- **View**: The SwiftUI view that defines the widget's visual appearance.
- **WidgetConfigurationIntent**: Allows users to configure the widget through system settings.

The following code shows a basic widget structure:

```swift
import WidgetKit
import SwiftUI

struct Provider: AppIntentTimelineProvider {
    func placeholder(in context: Context) -> SimpleEntry {
        SimpleEntry(date: Date(), message: "Placeholder")
    }

    func snapshot(for configuration: ConfigurationAppIntent, in context: Context) async -> SimpleEntry {
        SimpleEntry(date: Date(), message: "Snapshot")
    }
    
    func timeline(for configuration: ConfigurationAppIntent, in context: Context) async -> Timeline<SimpleEntry> {
        let entry = SimpleEntry(date: Date(), message: "Hello from Widget")
        let timeline = Timeline(entries: [entry], policy: .never)
        return timeline
    }
}

struct SimpleEntry: TimelineEntry {
    let date: Date
    let message: String
}

struct WidgetEntryView: View {
    var entry: Provider.Entry

    var body: some View {
        VStack {
            Text(entry.date, style: .time)
            Text(entry.message)
        }
    }
}

struct MyWidget: Widget {
    let kind: String = "MyWidget"

    var body: some WidgetConfiguration {
        AppIntentConfiguration(kind: kind, intent: ConfigurationAppIntent.self, provider: Provider()) { entry in
            WidgetEntryView(entry: entry)
        }
        .configurationDisplayName("My Widget")
        .description("This is a sample widget.")
        .supportedFamilies([.systemSmall, .systemMedium])
    }
}

@main
struct MyWidgetBundle: WidgetBundle {
    var body: some Widget {
        MyWidget()
    }
}
```

### Configure the widget app icon

To ensure the widget displays the correct app icon:

01. In Xcode, open the widget extension's **Assets.xcassets** folder.
01. Add your app icon images to the **AppIcon** asset. You can use an online iOS icon generator to create all required sizes.
01. Open the widget extension's **Info.plist** file in a text editor (not in Xcode's property list editor).
01. Add the following entries inside the `<dict>` element under `NSExtension`:

    ```xml
    <key>NSExtensionPrincipalClass</key>
    <string>MyWidgetExtension.MyWidgetBundle</string>
    <key>CFBundleIcons</key>
    <dict>
        <key>CFBundlePrimaryIcon</key>
        <dict>
            <key>CFBundleIconFiles</key>
            <array>
                <string>AppIcon</string>
            </array>
            <key>UIPrerenderedIcon</key>
            <false/>
        </dict>
    </dict>
    <key>CFBundleIconName</key>
    <string>AppIcon</string>
    ```

01. Replace `MyWidgetExtension.MyWidgetBundle` with your widget's **module name** and **bundle name**:

    - **Module name**: Found in Xcode under **Build Settings > Product Module Name**
    - **Bundle name**: The name of your `WidgetBundle` struct

If icons appear incorrect after updating, restart your test device as iOS caches widget icons.

## Build the widget for release

To prepare your widget for bundling with your .NET MAUI app, build it for both device and simulator:

01. Open Terminal and navigate to the root directory of your Xcode project.
01. Run the following commands to build for both platforms:

    ```bash
    # Create output directory
    rm -Rf XReleases

    # Build for device
    xcodebuild -project YourProject.xcodeproj \
        -scheme "YourWidgetExtension" \
        -configuration Release \
        -sdk iphoneos \
        BUILD_DIR=$(PWD)/XReleases clean build

    # Build for simulator
    xcodebuild -project YourProject.xcodeproj \
        -scheme "YourWidgetExtension" \
        -configuration Release \
        -sdk iphonesimulator \
        BUILD_DIR=$(PWD)/XReleases clean build
    ```

    Replace `YourProject` and `YourWidgetExtension` with your actual project and extension names.

The build output is an `.appex` file (a macOS bundle folder), which you'll include in your .NET MAUI app.

## Bundle the widget with your .NET MAUI app

To include the widget extension in your .NET MAUI app:

01. Copy the built `.appex` files to your .NET MAUI project under `Platforms/iOS/WidgetExtensions/`:
    - `Release-iphoneos/YourWidgetExtension.appex` for device builds
    - `Release-iphonesimulator/YourWidgetExtension.appex` for simulator builds

01. Add the following to your .NET MAUI project's `.csproj` file to include the widget extension files:

    ```xml
    <ItemGroup Condition="$(TargetFramework.Contains('-ios'))">
        <Content Remove="Platforms\iOS\WidgetExtensions\**" />
        <Content Condition="'$(ComputedPlatform)' == 'iPhone'" 
                 Include="Platforms\iOS\WidgetExtensions\Release-iphoneos\YourWidgetExtension.appex\**" 
                 CopyToOutputDirectory="PreserveNewest" />
        <Content Condition="'$(ComputedPlatform)' == 'iPhoneSimulator'" 
                 Include="Platforms\iOS\WidgetExtensions\Release-iphonesimulator\YourWidgetExtension.appex\**" 
                 CopyToOutputDirectory="PreserveNewest" />
    </ItemGroup>
    ```

01. Add the following to register the widget extension with your app:

    ```xml
    <ItemGroup Condition="$(TargetFramework.Contains('-ios'))">
        <AdditionalAppExtensions Include="$(MSBuildProjectDirectory)/Platforms/iOS/WidgetExtensions">
            <Name>YourWidgetExtension</Name>
            <BuildOutput Condition="'$(ComputedPlatform)' == 'iPhone'">Release-iphoneos</BuildOutput>
            <BuildOutput Condition="'$(ComputedPlatform)' == 'iPhoneSimulator'">Release-iphonesimulator</BuildOutput>
        </AdditionalAppExtensions>
    </ItemGroup>
    ```

    Replace `YourWidgetExtension` with the name of your widget extension (without the `.appex` extension).

> [!NOTE]
> Widget extensions might not be visible when building from Visual Studio to local iOS devices. They should work correctly when deploying to actual devices or simulators.

## Share data between the app and widget

iOS widgets are separate processes and cannot directly access your app's memory. To share data between your .NET MAUI app and the widget, use shared preferences backed by the App Groups capability.

### Configure entitlements

Both your .NET MAUI app and the widget extension need entitlements files configured with the same App Group ID.

#### Configure MAUI app entitlements

01. In your .NET MAUI project, create or edit `Platforms/iOS/Entitlements.plist`:

    ```xml
    <?xml version="1.0" encoding="UTF-8"?>
    <!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
    <plist version="1.0">
        <dict>
            <key>com.apple.security.application-groups</key>
            <array>
                <string>group.com.contoso.MyApp</string>
            </array>
        </dict>
    </plist>
    ```

01. Ensure your project file references this entitlements file. For more information, see [Consume entitlements](entitlements.md#consume-entitlements).

#### Configure widget entitlements

01. In your Xcode project, create or edit the widget extension's `Entitlements.plist` with the same App Group ID:

    ```xml
    <?xml version="1.0" encoding="UTF-8"?>
    <!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
    <plist version="1.0">
        <dict>
            <key>com.apple.security.application-groups</key>
            <array>
                <string>group.com.contoso.MyApp</string>
            </array>
        </dict>
    </plist>
    ```

01. In your .NET MAUI project's `.csproj` file, reference the widget's entitlements file in the `AdditionalAppExtensions` element:

    ```xml
    <ItemGroup Condition="$(TargetFramework.Contains('-ios'))">
        <AdditionalAppExtensions Include="$(MSBuildProjectDirectory)/Platforms/iOS/WidgetExtensions">
            <Name>YourWidgetExtension</Name>
            <BuildOutput Condition="'$(ComputedPlatform)' == 'iPhone'">Release-iphoneos</BuildOutput>
            <BuildOutput Condition="'$(ComputedPlatform)' == 'iPhoneSimulator'">Release-iphonesimulator</BuildOutput>
            <CodesignEntitlements>Platforms/iOS/Entitlements.YourWidgetExtension.plist</CodesignEntitlements>
        </AdditionalAppExtensions>
    </ItemGroup>
    ```

> [!NOTE]
> If you encounter errors about entitlements not being found, ensure the entitlements file is included in your project with **CopyToOutputDirectory** set to **PreserveNewest**. If you encounter errors reading the entitlements file during build, verify the file uses LF line endings (not CRLF).

### Store and retrieve data

With entitlements configured, both the app and widget can read and write to shared storage.

#### Store data from .NET MAUI

Use the `Preferences` API with the shared App Group ID:

```csharp
// Store data in .NET MAUI
Preferences.Set("WidgetMessage", "Hello from MAUI!", "group.com.contoso.MyApp");
Preferences.Set("WidgetCount", 42, "group.com.contoso.MyApp");
```

> [!IMPORTANT]
> Don't use `Preferences.Default` for data shared with widgets. Always specify the App Group ID in the `sharedName` parameter.

#### Retrieve data in Swift

In your widget's Swift code, use `UserDefaults` with the same suite name:

```swift
let groupId = "group.com.contoso.MyApp"

if let userDefaults = UserDefaults(suiteName: groupId) {
    let message = userDefaults.string(forKey: "WidgetMessage") ?? "No message"
    let count = userDefaults.integer(forKey: "WidgetCount")
    
    // Use the data in your widget
}
```

> [!NOTE]
> Storage keys are case-sensitive. Use consistent naming conventions to avoid errors.

## Communicate from app to widget

After your app updates shared data, you need to notify the widget to refresh. iOS provides the WidgetKit API for this, but it's not available directly in .NET MAUI. You can use a NuGet package like `WidgetKit.WidgetCenterProxy` or create your own binding.

### Use WidgetKit.WidgetCenterProxy

01. Install the `WidgetKit.WidgetCenterProxy` NuGet package in your .NET MAUI project.

01. After updating shared data, reload the widget:

    ```csharp
    // Update shared data
    Preferences.Set("WidgetMessage", "Updated message", "group.com.contoso.MyApp");

    // Notify the widget to reload
    var widgetCenter = new WidgetKit.WidgetCenterProxy();
    widgetCenter.ReloadTimeLinesOfKind("MyWidget");
    ```

    Replace `"MyWidget"` with the `kind` value from your widget's `Widget` struct in Swift.

> [!NOTE]
> The reload methods are polite requests to iOS. The system decides when to actually refresh the widget and may ignore frequent requests. Usually, the widget refreshes immediately.

## Communicate from widget to app

Widgets can communicate back to the app in two ways: deep links and app intents.

### Use deep links

By default, tapping a widget opens the app. You can customize this behavior with deep links:

01. In your widget's Swift code, add a `widgetURL` modifier to your view:

    ```swift
    struct WidgetEntryView: View {
        var entry: Provider.Entry

        var body: some View {
            VStack {
                Text(entry.message)
            }
            .widgetURL(URL(string: "myapp://widget?action=refresh&data=\(entry.message)"))
        }
    }
    ```

01. In your .NET MAUI app, handle the deep link. For more information about deep linking, see [Universal links](~/macios/universal-links.md).

### Use app intents for interactive widgets

App intents enable interactive elements like buttons in your widget. When triggered, an intent can update shared data and refresh the widget:

01. In your widget's Swift code, create an app intent:

    ```swift
    import AppIntents
    import WidgetKit

    struct IncrementCounterIntent: AppIntent {
        static var title: LocalizedStringResource { "Increment Counter" }
        static var description: IntentDescription { "Increments the counter by 1" }

        func perform() async throws -> some IntentResult {
            let groupId = "group.com.contoso.MyApp"
            let userDefaults = UserDefaults(suiteName: groupId)
            
            // Get current value
            let currentCount = userDefaults?.integer(forKey: "WidgetCount") ?? 0
            
            // Increment and save
            let newCount = currentCount + 1
            userDefaults?.set(newCount, forKey: "WidgetCount")
            
            // Reload the widget
            WidgetCenter.shared.reloadTimelines(ofKind: "MyWidget")
            
            return .result()
        }
    }
    ```

01. Add a button to your widget view that triggers the intent:

    ```swift
    struct WidgetEntryView: View {
        var entry: Provider.Entry

        var body: some View {
            VStack {
                Text("Count: \(entry.count)")
                Button(intent: IncrementCounterIntent()) {
                    Text("Increment")
                        .padding()
                        .background(Color.blue)
                        .foregroundColor(.white)
                        .cornerRadius(8)
                }
            }
        }
    }
    ```

01. Update your timeline entry to include the count:

    ```swift
    struct SimpleEntry: TimelineEntry {
        let date: Date
        let count: Int
    }
    ```

01. In the timeline provider, read the count from shared storage:

    ```swift
    func timeline(for configuration: ConfigurationAppIntent, in context: Context) async -> Timeline<SimpleEntry> {
        let groupId = "group.com.contoso.MyApp"
        let userDefaults = UserDefaults(suiteName: groupId)
        let count = userDefaults?.integer(forKey: "WidgetCount") ?? 0
        
        let entry = SimpleEntry(date: Date(), count: count)
        let timeline = Timeline(entries: [entry], policy: .never)
        return timeline
    }
    ```

> [!NOTE]
> iOS widgets are short-lived processes. Do not rely on in-memory state. Always use shared storage for data that needs to persist between widget updates.

## Create configurable widgets

To allow users to configure your widget:

01. Ensure you created the widget with the **Include Configuration Intent** option in Xcode.

01. In your configuration intent struct, add parameters for user-configurable options:

    ```swift
    import AppIntents

    struct ConfigurationAppIntent: WidgetConfigurationIntent {
        static var title: LocalizedStringResource = "Configuration"
        static var description = IntentDescription("Configures the widget")

        @Parameter(title: "Display Mode")
        var displayMode: DisplayModeEnum

        @Parameter(title: "Show Date")
        var showDate: Bool
    }

    enum DisplayModeEnum: String, AppEnum {
        case compact
        case detailed
        
        static var typeDisplayRepresentation: TypeDisplayRepresentation = "Display Mode"
        static var caseDisplayRepresentations: [DisplayModeEnum: DisplayRepresentation] = [
            .compact: "Compact",
            .detailed: "Detailed"
        ]
    }
    ```
    iOS automatically generates a configuration UI based on the parameters you define in the WidgetConfigurationIntent.
 
01. Update your timeline entry to include the showDate:

    ```swift
    struct SimpleEntry: TimelineEntry {
        let date: Date
        let displayMode: DisplayModeEnum
        let showDate: Bool
    }
    ```
 
01. Access the configuration in your timeline provider:

    ```swift
    func timeline(for configuration: ConfigurationAppIntent, in context: Context) async -> Timeline<SimpleEntry> {
        let displayMode = configuration.displayMode
        let showDate = configuration.showDate
        
        // Use configuration to customize the widget data
        let entry = SimpleEntry(
            date: Date(),
            displayMode: displayMode,
            showDate: showDate
        )
        
        return Timeline(entries: [entry], policy: .never)
    }
    ```

01. Update your widget view to reflect the configuration:

    ```swift
    struct WidgetEntryView: View {
        var entry: Provider.Entry

        var body: some View {
            VStack {
                if entry.showDate {
                    Text(entry.date, style: .date)
                }
 
                if entry.displayMode == .detailed {
                    Text("Detailed View")
                } else {
                    Text("Compact View")
                } 
 
                Button(intent: IncrementCounterIntent()) {
                    Text("Increment")
                        .padding()
                        .background(Color.blue)
                        .foregroundColor(.white)
                        .cornerRadius(8)
                }
            }
        }
    }
    ```


## See also

- [Creating a widget extension](https://developer.apple.com/documentation/widgetkit/creating-a-widget-extension) on developer.apple.com
- [How to Build iOS Widgets with .NET MAUI](https://devblogs.microsoft.com/dotnet/how-to-build-ios-widgets-with-dotnet-maui/) on the .NET Blog
- [iOS capabilities](capabilities.md)
- [iOS entitlements](entitlements.md)
- [Universal links](~/macios/universal-links.md)
