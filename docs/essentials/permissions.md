---
title: "Permissions"
description: "Describes the Permissions class in Microsoft.Maui.Essentials, which provides the ability to check and request runtime permissions."
ms.date: 01/04/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Permissions

The `Permissions` class provides the ability to check and request runtime permissions.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

[!INCLUDE [android-permissions](includes/android-permissions.md)]

## Using Permissions

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

## Checking Permissions

To check the current status of a permission, use the `CheckStatusAsync` method along with the specific permission to get the status for.

```csharp
var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
```

A `PermissionException` is thrown if the required permission is not declared.

It's best to check the status of the permission before requesting it. Each operating system returns a different default state if the user has never been prompted. iOS returns `Unknown`, while others return `Denied`. If the status is `Granted` then there is no need to make other calls. On iOS if the status is `Denied` you should prompt the user to change the permission in the settings and on Android you can call `ShouldShowRationale` to detect if the user has already denied the permission in the past.

## Requesting Permissions

To request a permission from the users, use the `RequestAsync` method along with the specific permission to request. If the user previously granted permission and hasn't revoked it, then this method will return `Granted` immediately and not display a dialog.

```csharp
var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
```

A `PermissionException` is thrown if the required permission is not declared.

Note, that on some platforms a permission request can only be activated a single time. Further prompts must be handled by the developer to check if a permission is in the `Denied` state and ask the user to manually turn it on.

## Permission Status

When using `CheckStatusAsync` or `RequestAsync` a `PermissionStatus` will be returned that can be used to determine the next steps:

* Unknown - The permission is in an unknown state
* Denied - The user denied the permission request
* Disabled - The feature is disabled on the device
* Granted - The user granted permission or is automatically granted
* Restricted - In a restricted state


## Explain Why Permission Is Needed

It is best practice to explain why your application needs a specific permission. On iOS you must specify a string that is displayed to the user. Android does not have this ability and and also defaults permission status to Disabled. This limits the ability to know if the user denied the permission or if it is the first time prompting the user. The `ShouldShowRationale` method can be used to determine if an educational UI should be displayed. If the method returns `true` this is because the user has denied or disabled the permission in the past. Other platforms will always return `false` when calling this method.

## Available Permissions

Xamarin.Essentials attempts to abstract as many permissions as possible. However, each operating system has a different set of runtime permissions. In addition there are differences when providing a single API for some permissions. Here is a guide to the currently available permissions:

Icon Guide:

* ✔️ - Supported
* ❌ - Not supported/required

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

If a permission is marked as ❌ it will always return `Granted` when checked or requested.

## General Usage

The following code presents the general usage pattern for determining whether a permission has been granted and requesting it if it has not. This code uses features that are available with Xamarin.Essentials version 1.6.0 or later.

```csharp
public async Task<PermissionStatus> CheckAndRequestLocationPermission()
{
    var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

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
public async Task GetLocationAsync()
{
    var status = await CheckAndRequestPermissionAsync(new Permissions.LocationWhenInUse());
    if (status != PermissionStatus.Granted)
    {
        // Notify user permission was denied
        return;
    }

    var location = await Geolocation.GetLocationAsync();
}

public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
            where T : BasePermission
{
    var status = await permission.CheckStatusAsync();
    if (status != PermissionStatus.Granted)
    {
        status = await permission.RequestAsync();
    }

    return status;
}
```

## Extending Permissions

The Permissions API was created to be flexible and extensible for applications that require additional validation or permissions that aren't included in Xamarin.Essentials. Create a new class that inherits from `BasePermission` and implement the required abstract methods.

```csharp
public class MyPermission : BasePermission
{
    // This method checks if current status of the permission
    public override Task<PermissionStatus> CheckStatusAsync()
    {
        throw new System.NotImplementedException();
    }

    // This method is optional and a PermissionException is often thrown if a permission is not declared
    public override void EnsureDeclared()
    {
        throw new System.NotImplementedException();
    }

    // Requests the user to accept or deny a permission
    public override Task<PermissionStatus> RequestAsync()
    {
        throw new System.NotImplementedException();
    }
}
```

When implementing a permission in a specific platform, the `BasePlatformPermission` class can be inherited from. This provides additional platform helper methods to automatically check the declarations. This can help when creating custom permissions that do groupings. For example, you can request both Read and Write access to storage on Android using the following custom permission.

```csharp
public class ReadWriteStoragePermission : Xamarin.Essentials.Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new List<(string androidPermission, bool isRuntime)>
    {
        (Android.Manifest.Permission.ReadExternalStorage, true),
        (Android.Manifest.Permission.WriteExternalStorage, true)
    }.ToArray();
}
```

Then you can call your new permission from Android project.

```csharp
await Permissions.RequestAsync<ReadWriteStoragePermission>();
```

If you wanted to call this API from your shared code you could create an interface and use a dependency service to register and get the implementation.

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

## Platform implementation specifics

# [Android](#tab/android)

Permissions must have the matching attributes set in the Android Manifest file. Permission status defaults to Denied.

Read more on the [Permissions in Xamarin.Android](../android/app-fundamentals/permissions.md) documentation.

# [iOS](#tab/ios)

Permissions must have a matching string in the `Info.plist` file. Once a permission is requested and denied a pop-up will no longer appear if you request the permission a second time. You must prompt your user to manually adjust the setting in the applications settings screen in iOS. Permission status defaults to Unknown.

Read more on the [iOS Security and Privacy Features](../ios/app-fundamentals/security-privacy.md) documentation.

# [Windows](#tab/windows)

Permissions must have matching capabilities declared in the package manifest. Permission status defaults to Unknown in most instances.

Read more on the [App Capability Declaration](/windows/uwp/packaging/app-capability-declarations) documentation.

--------------

## API

- [Permissions source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Permissions)
<!-- - [Permissions API documentation](xref:Microsoft.Maui.Essentials.Permissions)-->
