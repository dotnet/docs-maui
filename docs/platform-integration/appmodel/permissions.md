---
title: "Permissions"
description: "Learn how to use the .NET MAUI Permissions class, to check and request permissions. This class is in the Microsoft.Maui.Essentials namespace."
ms.date: 08/27/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

<!-- TODO update article -->
# Permissions

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `Permissions` class. This class allows you to check and request permissions at run-time.

## Get started

[!INCLUDE [get-started](../includes/get-started.md)]

[!INCLUDE [essentials-namespace](../includes/essentials-namespace.md)]

[!INCLUDE [android-permissions](../includes/android-permissions.md)]

## Available permissions

.NET MAUI attempts to abstract as many permissions as possible. However, each operating system has a different set of permissions. Even though the API allows access to a common permission, there may be differences between operating systems related to that permission. The following table describes the available permissions:

Icon Guide:

<!-- TODO: X = not supported or required?! why required? That doesn't make sense. -->
- ✔️ - Supported
- ❌ - Not supported/required

| Permission        | Android | iOS | UWP | watchOS | tvOS | Tizen |
|-------------------|:-------:|:---:|:---:|:-------:|:----:|:-----:|:-:|
| CalendarRead      | ✔️     | ✔️  | ❌  | ✔️      | ❌   | ❌     |
| CalendarWrite     | ✔️     | ✔️  | ❌  | ✔️      | ❌   | ❌     |
| Camera            | ✔️     | ✔️  | ❌  | ❌       | ❌   | ✔️    |
| ContactsRead      | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ❌     |
| ContactsWrite     | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ❌     |
| Flashlight        | ✔️     | ❌   | ❌  | ❌       | ❌   | ✔️    |
| LocationWhenInUse | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    |
| LocationAlways    | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    |
| Media             | ❌      | ✔️  | ❌  | ❌       | ❌   | ❌     |
| Microphone        | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    |
| Phone             | ✔️     | ✔️  | ❌  | ❌       | ❌   | ❌     |
| Photos            | ❌      | ✔️  | ❌  | ❌       | ✔️  | ❌     |
| Reminders         | ❌      | ✔️  | ❌  | ✔️      | ❌   | ❌     |
| Sensors           | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ❌     |
| Sms               | ✔️     | ✔️  | ❌  | ❌       | ❌   | ❌     |
| Speech            | ✔️     | ✔️  | ❌  | ❌       | ❌   | ❌     |
| StorageRead       | ✔️     | ❌   | ❌  | ❌       | ❌   | ❌     |
| StorageWrite      | ✔️     | ❌   | ❌  | ❌       | ❌   | ❌     |

If a permission is marked as ❌, it will always return `Granted` when checked or requested.

## Checking permissions

To check the current status of a permission, use the `Permissions.CheckStatusAsync` method along with the specific permission to get the status for. The following example checks the status of the `LocationWhenInUse` permission:

```csharp
PermissionStatus status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
```

A `PermissionException` is thrown if the required permission isn't declared.

It's best to check the status of the permission before requesting it. Each operating system returns a different default state, if the user has never been prompted. iOS returns `Unknown`, while others return `Denied`. If the status is `Granted` then there's no need to make other calls. On iOS if the status is `Denied` you should prompt the user to change the permission in the settings. On Android, you can call `ShouldShowRationale` to detect if the user has already denied the permission in the past.

### Permission status

When using `CheckStatusAsync` or `RequestAsync`, a `PermissionStatus` is returned that can be used to determine the next steps:

- `Unknown`\
The permission is in an unknown state.

- `Denied`\
The user denied the permission request.

- `Disabled`\
The feature is disabled on the device.

- `Granted`\
The user granted permission or is automatically granted.

- `Restricted`\
In a restricted state.

## Requesting permissions

To request a permission from the users, use the `Permissions.RequestAsync` method along with the specific permission to request. If the user previously granted permission, and hasn't revoked it, then this method will return `Granted` without showing a dialog to the user. The following example requests the `LocationWhenInUse` permission:

```csharp
PermissionStatus status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
```

A `PermissionException` is thrown if the required permission isn't declared.

> [!IMPORTANT]
> On some platforms, a permission request can only be activated a single time. Further prompts must be handled by the developer to check if a permission is in the `Denied` state, and then ask the user to manually turn it on.

## Explain why permission is needed

It's best practice to explain to your user why your application needs a specific permission. On iOS, you must specify a string that is displayed to the user. Android doesn't have this ability, and also defaults permission status to `Disabled`. This limits the ability to know if the user denied the permission or if it's the first time the permission is being requested. The `ShouldShowRationale` method can be used to determine if an informative UI should be displayed. If the method returns `true`, this is because the user has denied or disabled the permission in the past. Other platforms always return `false` when calling this method.

## Example

The following code presents the general usage pattern for determining whether a permission has been granted, and then requesting it if it hasn't.

```csharp
public async Task<PermissionStatus> CheckAndRequestLocationPermission()
{
    PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

    if (status == PermissionStatus.Granted)
        return status;

    if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
    {
        // Prompt the user to turn on in settings
        // On iOS once a permission has been denied it may not be requested again from the application
        return status;
    }

    if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
    {
        // Prompt the user with additional information as to why the permission is needed
    }

    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

    return status;
}
```

Each permission type can have an instance of it created that the methods can be called directly.

```csharp
#nullable enable
public async Task<Location?> GetLocationAsync()
{
    PermissionStatus status = await CheckAndRequestPermissionAsync(new Permissions.LocationWhenInUse());
    if (status != PermissionStatus.Granted)
    {
        // Notify user permission was denied
        return null;
    }

    return await Geolocation.GetLocationAsync();
}

public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
            where T : Permissions.BasePermission
{
    PermissionStatus status = await permission.CheckStatusAsync();
            
    if (status != PermissionStatus.Granted)
        status = await permission.RequestAsync();

    return status;
}
#nullable restore
```

## Extending permissions

The Permissions API was created to be flexible and extensible for applications that require more validation or permissions that aren't included in .NET MAUI. Create a class that inherits from `Permissions.BasePermission`, and implement the required abstract methods. The following example code demonstrates the basic abstract members, but without implementation:

```csharp
public class MyPermission : Permissions.BasePermission
{
    // This method checks if current status of the permission.
    public override Task<PermissionStatus> CheckStatusAsync()
    {
        throw new System.NotImplementedException();
    }

    // This method is optional and a PermissionException is often thrown if a permission is not declared.
    public override void EnsureDeclared()
    {
        throw new System.NotImplementedException();
    }

    // Requests the user to accept or deny a permission.
    public override Task<PermissionStatus> RequestAsync()
    {
        throw new System.NotImplementedException();
    }

    // Indicates that the requestor should prompt the user as to why the app requires the permission, because the
    // user has previously denied this permission.
    public override bool ShouldShowRationale()
    {
        throw new NotImplementedException();
    }
}
```

When implementing a permission in a specific platform, the `BasePlatformPermission` class can be inherited from. This class provides extra platform helper methods to automatically check the permission declarations. This helps when creating custom permissions that do groupings, for example requesting both **Read** and **Write** access to storage on Android. The following code example demonstrates requesting **Read** and **Write** storage access:

```csharp
public class ReadWriteStoragePerms: Microsoft.Maui.Essentials.Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
        new List<(string androidPermission, bool isRuntime)>
        {
            (global::Android.Manifest.Permission.ReadExternalStorage, true),
            (global::Android.Manifest.Permission.WriteExternalStorage, true)
        }.ToArray();
}
```

You then check the permission in the same way as any other permission type provided by .NET MAUI:

```csharp
await Permissions.RequestAsync<ReadWriteStoragePermission>();
```

<!-- TODO: Is this still the same? Does shared code just pull in the platform specific code at compile time and thus you don't need an interface as long as you've declared the type in all platforms you're compiling for? (The rest of this section was not processed for this update and needs to be updated)-->
If you wanted to call this API from your shared code, you could create an interface and use a dependency service to register and get the implementation.

```csharp
public interface IReadWritePermission
{        
    Task<PermissionStatus> CheckStatusAsync();
    Task<PermissionStatus> RequestAsync();
}
```

Then implement the interface in your platform project:

```csharp
public class ReadWriteStoragePermission : Xamarin.Essentials.Permissions.BasePlatformPermission, IReadWritePermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new List<(string androidPermission, bool isRuntime)>
    {
        (Android.Manifest.Permission.ReadExternalStorage, true),
        (Android.Manifest.Permission.WriteExternalStorage, true)
    }.ToArray();
}
```

You can then register the specific implementation:

```csharp
DependencyService.Register<IReadWritePermission, ReadWriteStoragePermission>();
```

Then from your shared project you can resolve and use it:

```csharp
var readWritePermission = DependencyService.Get<IReadWritePermission>();
var status = await readWritePermission.CheckStatusAsync();
if (status != PermissionStatus.Granted)
{
    status = await readWritePermission.RequestAsync();
}
```

## Platform differences

This section describes the platform-specific differences with the permissions API.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

Permissions must have the matching attributes set in the Android Manifest file. Permission status defaults to `Denied`.

<!-- TODO For more information, see [Permissions in .NET MAUI for Android](../../android/app-fundamentals/permissions.md). -->

# [iOS](#tab/ios)

Permissions must have a matching string in the _Info.plist_ file. Once a permission is requested and denied, a pop-up will no longer appear if you request the permission a second time. You must prompt your user to manually adjust the setting in the applications settings screen in iOS. Permission status defaults to `Unknown`.

<!-- TODO For more information, see [iOS Security and Privacy Features](../ios/app-fundamentals/security-privacy.md). -->

# [Windows](#tab/windows)

Permissions must have matching capabilities declared in the package manifest. Permission status defaults to `Unknown` in most instances.

<!-- TODO For more information, see [App Capability Declaration](/windows/uwp/packaging/app-capability-declarations). -->

-----
<!-- markdownlint-enable MD025 -->

## API

- [Permissions source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Permissions)
<!-- - [Permissions API documentation](xref:Microsoft.Maui.Essentials.Permissions)-->
