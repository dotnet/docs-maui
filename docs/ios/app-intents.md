---
title: "Apple App Intents for Siri and Shortcuts"
description: "Learn how to integrate Apple App Intents into your .NET MAUI iOS app to enable Siri voice commands, Shortcuts actions, and Spotlight suggestions."
ms.date: 03/06/2026
---

# Apple App Intents for Siri and Shortcuts

Apple App Intents let your .NET Multi-platform App UI (.NET MAUI) app expose actions to Siri, the Shortcuts app, and Spotlight. Users can invoke these actions through voice commands ("Hey Siri, create a task in my app"), by building custom automations in the Shortcuts app, or through proactive suggestions that iOS surfaces based on usage patterns.

Because iOS extracts intent metadata at compile time from Swift binaries, App Intents can't be defined purely in C#. The metadata extraction happens during the build process, before the app ever runs, so iOS needs to find intent definitions in compiled Swift code. This guide uses a three-project architecture that keeps Swift as a thin declaration layer while all business logic stays in C#.

## Architecture

The integration uses three projects that work together:

| Project | Language | Purpose |
|---|---|---|
| Xcode framework | Swift | Declares `AppIntent`, `AppEntity`, and `AppEnum` types. Defines an `@objc` bridge protocol that the C# side implements. |
| Binding library | C# | Maps `@objc` types to C# via `ApiDefinition.cs`. Auto-builds the Swift framework into an xcframework. |
| MAUI app | C# | Implements the bridge protocol with all business logic in .NET. Wires up the bridge at startup. |

The Swift framework is deliberately lightweight. It defines *what* actions are available (intent names, parameters, Siri phrases) but delegates all actual work to C# through the bridge protocol. When Siri or Shortcuts invokes an intent, the Swift code calls through the bridge to your .NET implementation, which has full access to your app's services, data, and dependency injection container.

> [!NOTE]
> This pattern is necessary because Apple's App Intents framework relies on compile-time metadata extraction from Swift binaries. The metadata tells iOS what actions are available, their parameters, and how to present them in Siri and Shortcuts — all without running the app.

## Prerequisites

Before you begin, make sure you have:

- macOS with Xcode 16 or later installed.
- .NET 10 SDK or later with the .NET MAUI workload.
- An iOS 17+ device or simulator.
- An Apple Developer account (required for on-device Siri voice testing).

## Create the Swift framework

Create an Xcode framework project that contains your intent definitions and the bridge protocol. A typical structure looks like this:

```text
YourApp/
├── YourApp.csproj
├── MauiProgram.cs
├── Platforms/
│   └── iOS/
│       └── AppDelegate.cs
├── YourBindingLibrary/
│   ├── YourBindingLibrary.csproj
│   ├── ApiDefinition.cs
│   └── StructsAndEnums.cs
└── YourSwiftFramework/
    ├── YourSwiftFramework.xcodeproj
    └── YourSwiftFramework/
        ├── BridgeProtocol.swift
        ├── BridgeTypes.swift
        ├── TaskIntents.swift
        ├── TaskEntity.swift
        ├── TaskEnums.swift
        ├── IntentDonation.swift
        └── AppShortcutsProvider.swift
```

### Bridge protocol

The bridge protocol is the most important piece of the architecture. It defines the contract between Swift and C#. All methods and properties must be marked `@objc` so they're visible to the Objective-C runtime, which is how .NET bindings communicate with Swift.

Define the protocol and a singleton manager that holds a reference to the C# implementation:

```swift
import Foundation

@objc(TaskDataProvider) public protocol TaskDataProvider: AnyObject {
    func getAllTasks() -> [BridgeTaskItem]
    func getTask(withId id: String) -> BridgeTaskItem?
    func createTask(title: String, priorityRawValue: Int,
                    categoryRawValue: Int, dueDate: Date?,
                    estimatedMinutes: Int, notes: String) -> BridgeTaskItem?
    func completeTask(withId id: String) -> Bool
    func searchTasks(query: String) -> [BridgeTaskItem]
}

@objc(TaskBridgeManager) public class TaskBridgeManager: NSObject {
    @objc public static let shared = TaskBridgeManager()
    @objc public weak var provider: TaskDataProvider?
    private override init() { super.init() }
}
```

The `provider` property is `weak` to avoid retain cycles. Your C# code sets this property at app startup, and all Swift intent implementations call through it to reach your .NET business logic.

### Bridge data types

Data that crosses the Swift-to-C# boundary must use `@objc`-compatible types. Define a data transfer object (DTO) class for each entity your intents work with:

```swift
import Foundation

@objc(BridgeTaskItem) public class BridgeTaskItem: NSObject {
    @objc public var id: String
    @objc public var title: String
    @objc public var isCompleted: Bool
    @objc public var priorityRawValue: Int
    @objc public var categoryRawValue: Int
    @objc public var dueDate: Date?
    @objc public var estimatedMinutes: Int
    @objc public var notes: String

    @objc public init(id: String, title: String, isCompleted: Bool,
                      priorityRawValue: Int, categoryRawValue: Int,
                      dueDate: Date?, estimatedMinutes: Int,
                      notes: String) {
        self.id = id
        self.title = title
        self.isCompleted = isCompleted
        self.priorityRawValue = priorityRawValue
        self.categoryRawValue = categoryRawValue
        self.dueDate = dueDate
        self.estimatedMinutes = estimatedMinutes
        self.notes = notes
    }
}
```

> [!IMPORTANT]
> When passing optional integers across the bridge, use sentinel values (such as `-1` for "no value") instead of Swift optionals. The `@objc` bridge doesn't support optional value types like `Int?`. Reserve Swift optionals for reference types like `Date?` and `String?`, which map to nullable Objective-C types.

### Define App Enums

Define `AppEnum` types for any enumerated values your intents use. These appear as selectable options in the Shortcuts app and Siri dialogs:

```swift
import AppIntents

enum TaskPriority: Int, AppEnum {
    case low = 0
    case medium = 1
    case high = 2
    case urgent = 3

    static var typeDisplayRepresentation: TypeDisplayRepresentation {
        "Task Priority"
    }

    static var caseDisplayRepresentations: [TaskPriority: DisplayRepresentation] {
        [
            .low: "Low",
            .medium: "Medium",
            .high: "High",
            .urgent: "Urgent"
        ]
    }
}

enum TaskCategory: Int, AppEnum {
    case personal = 0
    case work = 1
    case shopping = 2
    case health = 3

    static var typeDisplayRepresentation: TypeDisplayRepresentation {
        "Task Category"
    }

    static var caseDisplayRepresentations: [TaskCategory: DisplayRepresentation] {
        [
            .personal: "Personal",
            .work: "Work",
            .shopping: "Shopping",
            .health: "Health"
        ]
    }
}
```

### Define App Entities

An `AppEntity` represents a domain object that intents can operate on. Define an entity with an `EntityStringQuery` so users can search for items by name in Siri and Shortcuts:

```swift
import AppIntents

struct TaskEntity: AppEntity {
    static var defaultQuery = TaskEntityQuery()
    static var typeDisplayRepresentation: TypeDisplayRepresentation {
        "Task"
    }

    var id: String
    var title: String
    var isCompleted: Bool

    var displayRepresentation: DisplayRepresentation {
        DisplayRepresentation(title: "\(title)")
    }
}

struct TaskEntityQuery: EntityStringQuery {
    func entities(for identifiers: [String]) async throws -> [TaskEntity] {
        guard let provider = TaskBridgeManager.shared.provider else {
            return []
        }

        return identifiers.compactMap { id in
            guard let item = provider.getTask(withId: id) else {
                return nil
            }
            return TaskEntity(id: item.id, title: item.title,
                              isCompleted: item.isCompleted)
        }
    }

    func entities(matching query: String) async throws -> [TaskEntity] {
        guard let provider = TaskBridgeManager.shared.provider else {
            return []
        }

        return provider.searchTasks(query: query).map { item in
            TaskEntity(id: item.id, title: item.title,
                       isCompleted: item.isCompleted)
        }
    }

    func suggestedEntities() async throws -> [TaskEntity] {
        guard let provider = TaskBridgeManager.shared.provider else {
            return []
        }

        return provider.getAllTasks().map { item in
            TaskEntity(id: item.id, title: item.title,
                       isCompleted: item.isCompleted)
        }
    }
}
```

### Define App Intents

Each `AppIntent` represents an action users can perform through Siri or Shortcuts. The intent declares its parameters, provides a summary string for the Siri dialog, and implements `perform()` to execute the action through the bridge:

```swift
import AppIntents

enum IntentError: Error {
    case notReady
    case taskCreationFailed
    case taskNotFound
}

struct CreateTaskIntent: AppIntent {
    static var title: LocalizedStringResource { "Create Task" }
    static var description: IntentDescription {
        "Creates a new task in the app."
    }
    static var openAppWhenRun: Bool = false

    @Parameter(title: "Title")
    var taskTitle: String

    @Parameter(title: "Priority")
    var priority: TaskPriority

    @Parameter(title: "Category")
    var category: TaskCategory

    @Parameter(title: "Due Date", optionsProvider: nil)
    var dueDate: Date?

    @Parameter(title: "Estimated Minutes", default: -1)
    var estimatedMinutes: Int

    @Parameter(title: "Notes", default: "")
    var notes: String

    static var parameterSummary: some ParameterSummary {
        Summary("Create task \(\.$taskTitle)") {
            \.$priority
            \.$category
            \.$dueDate
            \.$estimatedMinutes
            \.$notes
        }
    }

    func perform() async throws -> some IntentResult & ReturnsValue<TaskEntity> {
        guard let provider = TaskBridgeManager.shared.provider else {
            throw IntentError.notReady
        }

        guard let item = provider.createTask(
            title: taskTitle,
            priorityRawValue: priority.rawValue,
            categoryRawValue: category.rawValue,
            dueDate: dueDate,
            estimatedMinutes: estimatedMinutes,
            notes: notes
        ) else {
            throw IntentError.taskCreationFailed
        }

        return .result(value: TaskEntity(
            id: item.id, title: item.title,
            isCompleted: item.isCompleted
        ))
    }
}
```

You can define additional intents following the same pattern. For example, a `CompleteTaskIntent` that marks a task as done:

```swift
struct CompleteTaskIntent: AppIntent {
    static var title: LocalizedStringResource { "Complete Task" }
    static var description: IntentDescription {
        "Marks a task as completed."
    }
    static var openAppWhenRun: Bool = false

    @Parameter(title: "Task")
    var task: TaskEntity

    static var parameterSummary: some ParameterSummary {
        Summary("Complete \(\.$task)")
    }

    func perform() async throws -> some IntentResult {
        guard let provider = TaskBridgeManager.shared.provider else {
            throw IntentError.notReady
        }

        let success = provider.completeTask(withId: task.id)
        if !success {
            throw IntentError.taskNotFound
        }

        return .result()
    }
}
```

### Register Siri phrases

Define an `AppShortcutsProvider` to register phrases that users can speak to invoke your intents directly through Siri. Each phrase must include an application name token:

```swift
import AppIntents

struct TaskAppShortcuts: AppShortcutsProvider {
    static var appShortcuts: [AppShortcut] {
        AppShortcut(
            intent: CreateTaskIntent(),
            phrases: [
                "Create a task in \(.applicationName)",
                "Add a new task in \(.applicationName)"
            ],
            shortTitle: "Create Task",
            systemImageName: "plus.circle"
        )
        AppShortcut(
            intent: CompleteTaskIntent(),
            phrases: [
                "Complete a task in \(.applicationName)",
                "Mark task done in \(.applicationName)"
            ],
            shortTitle: "Complete Task",
            systemImageName: "checkmark.circle"
        )
    }
}
```

> [!TIP]
> Keep Siri phrases natural and concise. Include variations for how users might phrase the same request. The `\(.applicationName)` token is required and is automatically replaced with your app's display name.

## Create the binding library

The binding library is a .NET project that wraps the Swift framework into a form that C# can consume. It uses the `XcodeProject` MSBuild item to automatically build the Swift framework and includes a target that extracts App Intents metadata.

### Project configuration

Create a .NET iOS binding library project with the following `.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0-ios</TargetFramework>
    <IsBindingProject>true</IsBindingProject>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
  </PropertyGroup>

  <ItemGroup>
    <ObjcBindingApiDefinition Include="ApiDefinition.cs" />
    <ObjcBindingCoreSource Include="StructsAndEnums.cs" />
  </ItemGroup>

  <ItemGroup>
    <XcodeProject Include="..\YourSwiftFramework\YourSwiftFramework.xcodeproj">
      <SchemeName>YourSwiftFramework</SchemeName>
      <ForceLoad>true</ForceLoad>
      <SmartLink>false</SmartLink>
    </XcodeProject>
  </ItemGroup>

  <Target Name="ExtractAppIntentsMetadata" AfterTargets="_BuildXcodeProjects">
    <PropertyGroup>
      <_XcArchivePath>$([System.IO.Path]::Combine(
        $(IntermediateOutputPath), 'xcode', 'xcarchive',
        'YourSwiftFramework.xcarchive'))</_XcArchivePath>
      <_MetadataSrc>$(_XcArchivePath)/Metadata.appintents</_MetadataSrc>
      <_MetadataDst>$(IntermediateOutputPath)Metadata.appintents</_MetadataDst>
    </PropertyGroup>
    <Copy
      SourceFiles="@(_MetadataFiles)"
      DestinationFolder="$(_MetadataDst)/%(RecursiveDir)"
      Condition="Exists('$(_MetadataSrc)')" />
    <ItemGroup Condition="Exists('$(_MetadataSrc)')">
      <_MetadataFiles Include="$(_MetadataSrc)/**/*" />
    </ItemGroup>
    <Copy
      SourceFiles="@(_MetadataFiles)"
      DestinationFolder="$(_MetadataDst)/%(RecursiveDir)"
      Condition="Exists('$(_MetadataSrc)')" />
  </Target>
</Project>
```

The `XcodeProject` item tells the .NET build system to compile the Swift project into an xcframework automatically. The `ForceLoad` and `SmartLink` settings ensure that all symbols from the Swift framework are available at runtime, which is required because the App Intents framework uses runtime discovery.

The `ExtractAppIntentsMetadata` target copies the `Metadata.appintents` bundle from the xcarchive output into the intermediate output path, where the MAUI app can pick it up.

### ApiDefinition.cs

The API definition file maps Swift `@objc` types to C# interfaces and classes. Each `[Export]` attribute specifies the Objective-C selector name that matches the Swift declaration:

```csharp
using Foundation;
using ObjCRuntime;

namespace YourBindingLibrary;

[BaseType(typeof(NSObject))]
interface BridgeTaskItem
{
    [Export("id")]
    string Id { get; set; }

    [Export("title")]
    string Title { get; set; }

    [Export("isCompleted")]
    bool IsCompleted { get; set; }

    [Export("priorityRawValue")]
    nint PriorityRawValue { get; set; }

    [Export("categoryRawValue")]
    nint CategoryRawValue { get; set; }

    [Export("dueDate")]
    [NullAllowed]
    NSDate DueDate { get; set; }

    [Export("estimatedMinutes")]
    nint EstimatedMinutes { get; set; }

    [Export("notes")]
    string Notes { get; set; }

    [Export("initWithId:title:isCompleted:priorityRawValue:" +
           "categoryRawValue:dueDate:estimatedMinutes:notes:")]
    NativeHandle Constructor(string id, string title, bool isCompleted,
        nint priorityRawValue, nint categoryRawValue,
        [NullAllowed] NSDate dueDate, nint estimatedMinutes,
        string notes);
}

[Protocol]
[BaseType(typeof(NSObject), Name = "TaskDataProvider")]
[Model]
interface TaskDataProvider
{
    [Abstract]
    [Export("getAllTasks")]
    BridgeTaskItem[] GetAllTasks();

    [Abstract]
    [Export("getTaskWithId:")]
    [return: NullAllowed]
    BridgeTaskItem GetTask(string id);

    [Abstract]
    [Export("createTaskWithTitle:priorityRawValue:categoryRawValue:" +
           "dueDate:estimatedMinutes:notes:")]
    [return: NullAllowed]
    BridgeTaskItem CreateTask(string title, nint priorityRawValue,
        nint categoryRawValue, [NullAllowed] NSDate dueDate,
        nint estimatedMinutes, string notes);

    [Abstract]
    [Export("completeTaskWithId:")]
    bool CompleteTask(string id);

    [Abstract]
    [Export("searchTasksWithQuery:")]
    BridgeTaskItem[] SearchTasks(string query);
}

[BaseType(typeof(NSObject))]
[DisableDefaultCtor]
interface TaskBridgeManager
{
    [Static]
    [Export("shared")]
    TaskBridgeManager Shared { get; }

    [Export("provider", ArgumentSemantic.Weak)]
    [NullAllowed]
    ITaskDataProvider Provider { get; set; }
}
```

> [!NOTE]
> Key mapping rules between Swift and C#:
>
> - Swift `Int` maps to `nint` in C# (platform-native integer size).
> - Swift `Date?` maps to `[NullAllowed] NSDate` in C#.
> - Swift optional reference types use `[NullAllowed]` on the property or return value.
> - The `[Export]` selector must match the Objective-C selector that Swift generates. For a method `func getTask(withId id: String)`, the selector is `getTaskWithId:`.
> - Use `[DisableDefaultCtor]` for singleton classes that should only be accessed through a static `Shared` property.

### StructsAndEnums.cs

If your bridge uses enums or structs that need explicit C# definitions, define them in the `StructsAndEnums.cs` file. For simple bridge patterns where enums are passed as raw integer values, this file can remain empty:

```csharp
using ObjCRuntime;

namespace YourBindingLibrary;

// Enums are passed as raw Int values across the bridge,
// so no enum definitions are needed here.
```

## Set up intent donation

Intent donation tells iOS when users perform actions in your app's UI, allowing the system to learn usage patterns and proactively suggest shortcuts. Define a donation bridge in Swift that C# can call:

```swift
import AppIntents
import Foundation

@objc(IntentDonationBridge) public class IntentDonationBridge: NSObject {
    @objc public static let shared = IntentDonationBridge()
    private override init() { super.init() }

    @objc public func donateCreateTask(title: String,
                                       priorityRawValue: Int,
                                       categoryRawValue: Int) {
        let intent = CreateTaskIntent()
        intent.taskTitle = title
        intent.priority = TaskPriority(rawValue: priorityRawValue) ?? .medium
        intent.category = TaskCategory(rawValue: categoryRawValue) ?? .personal

        Task {
            try? await intent.donate()
        }
    }
}
```

Add the corresponding binding in `ApiDefinition.cs`:

```csharp
[BaseType(typeof(NSObject))]
[DisableDefaultCtor]
interface IntentDonationBridge
{
    [Static]
    [Export("shared")]
    IntentDonationBridge Shared { get; }

    [Export("donateCreateTaskWithTitle:priorityRawValue:categoryRawValue:")]
    void DonateCreateTask(string title, nint priorityRawValue,
                          nint categoryRawValue);
}
```

## Implement the bridge in C\#

### Bridge provider

Create a class in your MAUI app that inherits from the generated `TaskDataProvider` base class and delegates to your app's service layer:

```csharp
using Foundation;
using YourBindingLibrary;

namespace YourApp.Platforms.iOS;

public class AppIntentsBridgeProvider : TaskDataProvider
{
    private readonly ITaskService _taskService;

    public AppIntentsBridgeProvider(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public override BridgeTaskItem[] GetAllTasks()
    {
        return _taskService.GetAll()
            .Select(t => t.ToBridge())
            .ToArray();
    }

    public override BridgeTaskItem? GetTask(string id)
    {
        var task = _taskService.GetById(id);
        return task?.ToBridge();
    }

    public override BridgeTaskItem? CreateTask(
        string title, nint priorityRawValue, nint categoryRawValue,
        NSDate? dueDate, nint estimatedMinutes, string notes)
    {
        var newTask = _taskService.Create(
            title,
            (TaskPriorityEnum)(int)priorityRawValue,
            (TaskCategoryEnum)(int)categoryRawValue,
            dueDate is not null
                ? (DateTime)dueDate
                : null,
            (int)estimatedMinutes >= 0
                ? (int)estimatedMinutes
                : null,
            notes);

        return newTask?.ToBridge();
    }

    public override bool CompleteTask(string id)
    {
        return _taskService.Complete(id);
    }

    public override BridgeTaskItem[] SearchTasks(string query)
    {
        return _taskService.Search(query)
            .Select(t => t.ToBridge())
            .ToArray();
    }
}
```

Define a `ToBridge()` extension method to handle the type conversions between your domain model and the bridge DTO:

```csharp
using Foundation;
using YourBindingLibrary;

namespace YourApp.Platforms.iOS;

public static class BridgeExtensions
{
    public static BridgeTaskItem ToBridge(this TaskItem task)
    {
        return new BridgeTaskItem(
            id: task.Id,
            title: task.Title,
            isCompleted: task.IsCompleted,
            priorityRawValue: (nint)(int)task.Priority,
            categoryRawValue: (nint)(int)task.Category,
            dueDate: task.DueDate.HasValue
                ? (NSDate)task.DueDate.Value
                : null,
            estimatedMinutes: task.EstimatedMinutes.HasValue
                ? (nint)task.EstimatedMinutes.Value
                : -1,
            notes: task.Notes ?? string.Empty
        );
    }
}
```

> [!IMPORTANT]
> Pay attention to type marshaling across the bridge:
>
> - Cast C# enums to `int`, then to `nint` for the bridge.
> - Convert `DateTime` to `NSDate` using the explicit cast operator: `(NSDate)dateTime`.
> - Use sentinel values like `-1` for optional integers, since the Objective-C bridge doesn't support nullable value types.

### Wire up in AppDelegate

Set the bridge provider in `AppDelegate.cs` so that intents can reach your C# logic as soon as the app launches:

```csharp
using Foundation;
using UIKit;
using YourBindingLibrary;

namespace YourApp.Platforms.iOS;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(
        UIApplication application, NSDictionary launchOptions)
    {
        var result = base.FinishedLaunching(application, launchOptions);
        WireUpAppIntentsBridge();
        return result;
    }

    private void WireUpAppIntentsBridge()
    {
        var taskService = IPlatformApplication.Current?.Services
            .GetRequiredService<ITaskService>();

        if (taskService is not null)
        {
            TaskBridgeManager.Shared.Provider =
                new AppIntentsBridgeProvider(taskService);
        }
    }
}
```

### Donate intents from your UI

When users perform actions in your app's UI, donate the corresponding intent so iOS can learn patterns and suggest shortcuts:

```csharp
private void OnTaskCreated(string title, int priority, int category)
{
#if IOS
    IntentDonationBridge.Shared.DonateCreateTask(
        title, (nint)priority, (nint)category);
#endif
}
```

### Copy metadata to app bundle

The MAUI app's `.csproj` must include a target that copies the `Metadata.appintents` bundle from the binding library into the final app bundle. Without this metadata, iOS doesn't know your intents exist.

Add this target to your MAUI app's `.csproj`:

```xml
<Target Name="CopyAppIntentsMetadata" AfterTargets="_CopyResourcesToBundle"
        Condition="$(TargetFramework.Contains('-ios'))">
  <PropertyGroup>
    <_MetadataSrc>$(MSBuildProjectDirectory)\..\YourBindingLibrary\
$(IntermediateOutputPath)Metadata.appintents</_MetadataSrc>
    <_MetadataDst>$(_AppBundlePath)/Metadata.appintents/</_MetadataDst>
  </PropertyGroup>
  <ItemGroup>
    <_MetadataFiles
      Include="$(_MetadataSrc)/**/*"
      Condition="Exists('$(_MetadataSrc)')" />
  </ItemGroup>
  <Copy
    SourceFiles="@(_MetadataFiles)"
    DestinationFolder="$(_MetadataDst)%(RecursiveDir)"
    Condition="@(_MetadataFiles->Count()) > 0" />
</Target>
```

> [!WARNING]
> The destination path **must** end with `/` (note the trailing slash in `Metadata.appintents/`). Without it, MSBuild treats the path as a file name instead of a directory, and the metadata ends up outside the app bundle. When this happens, intents silently fail to register — they won't appear in Shortcuts or respond to Siri.

## Build and test

Build the app from the command line:

```bash
dotnet build YourApp.csproj -f net10.0-ios -r iossimulator-arm64
```

For device builds, use `ios-arm64` as the runtime identifier:

```bash
dotnet build YourApp.csproj -f net10.0-ios -r ios-arm64
```

### Verify metadata

After building, confirm that the `Metadata.appintents` bundle exists inside your app bundle:

```bash
find bin/Debug/net10.0-ios/iossimulator-arm64/YourApp.app \
    -name "Metadata.appintents" -type d
```

If the command returns no results, the metadata wasn't copied correctly. Review the `CopyAppIntentsMetadata` target in your `.csproj`.

### Simulator testing

On the iOS simulator, you can:

- Open the **Shortcuts** app to see all registered intents and test them interactively.
- Run intents to verify they execute correctly through the bridge.
- Check the Xcode console for `[AppIntents]` log messages that confirm metadata was loaded.

### Device testing

On a physical device with an Apple Developer account:

- Siri voice invocation works with the phrases registered in `AppShortcutsProvider`.
- Spotlight suggestions appear based on donated intents.
- The Shortcuts app shows all registered intents with their full parameter UI.

> [!TIP]
> During development, if intents stop appearing after code changes, delete the app from the simulator or device and reinstall. iOS caches intent metadata aggressively, and a clean install forces re-registration.

## Troubleshooting

| Problem | Solution |
|---|---|
| Intents don't appear in the Shortcuts app. | Verify that `Metadata.appintents` exists inside the app bundle at the correct path. Run the `find` command in the [Verify metadata](#verify-metadata) section. |
| "App not ready" errors when invoking an intent. | Ensure the bridge is wired up in `AppDelegate.FinishedLaunching` before any intents can fire. Check that `TaskBridgeManager.Shared.Provider` is set. |
| Type conversion errors at runtime. | Check that sentinel values (like `-1` for optional integers) are handled correctly on both sides of the bridge. Verify `nint` casts for enum raw values. |
| Build fails with "scheme not found." | Verify that the `SchemeName` in the `XcodeProject` MSBuild item matches the scheme name in your Xcode project exactly. |
| Intents appear but return errors. | Check the Xcode console or device logs for `[AppIntents]` messages. Verify that `ITaskService` is registered in the DI container before the bridge is wired up. |
| Metadata exists but intents still don't register. | Confirm the `CopyAppIntentsMetadata` target destination path ends with `/`. Verify the metadata was copied into the `.app` bundle, not alongside it. |

## See also

- [Maui.Apple.PlatformFeature.Samples on GitHub](https://github.com/Redth/Maui.Apple.PlatformFeature.Samples) — the sample project this guide is based on.
- [Apple App Intents documentation](https://developer.apple.com/documentation/appintents)
- [Apple Shortcuts documentation](https://developer.apple.com/documentation/appintents/app-shortcuts)
- [Apple SiriKit documentation](https://developer.apple.com/documentation/sirikit)
