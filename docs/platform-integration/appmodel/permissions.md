---
title: "Permissions"
description: "Learn how to use the .NET MAUI Permissions class, to check and request permissions. This class is in the Microsoft.Maui.ApplicationModel namespace."
ms.date: 10/19/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel"]
---

# Permissions

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.ApplicationModel.Permissions> class. This class allows you to check and request permissions at run-time. The `Permissions` type is available in the `Microsoft.Maui.ApplicationModel` namespace.

## Available permissions

.NET MAUI attempts to abstract as many permissions as possible. However, each operating system has a different set of permissions. Even though the API allows access to a common permission, there may be differences between operating systems related to that permission. The following table describes the available permissions:

The following table uses ✔️ to indicate that the permission is supported and ❌ to indicate the permission isn't supported or isn't required:

::: moniker range="=net-maui-7.0"

| Permission                                                                              | Android | iOS | Windows | tvOS |
|------------------------------------------------------------------------------------------|:-------:|:---:|:-------:|:----:|
| [Battery](xref:Microsoft.Maui.ApplicationModel.Permissions.Battery)                     | ✔️     | ❌  | ❌     | ❌    |
| [CalendarRead](xref:Microsoft.Maui.ApplicationModel.Permissions.CalendarRead)           | ✔️     | ✔️  | ❌      | ❌    |
| [CalendarWrite](xref:Microsoft.Maui.ApplicationModel.Permissions.CalendarWrite)         | ✔️     | ✔️  | ❌      | ❌    |
| [Camera](xref:Microsoft.Maui.ApplicationModel.Permissions.Camera)                       | ✔️     | ✔️  | ❌      | ❌    |
| [ContactsRead](xref:Microsoft.Maui.ApplicationModel.Permissions.ContactsRead)           | ✔️     | ✔️  | ❌      | ❌    |
| [ContactsWrite](xref:Microsoft.Maui.ApplicationModel.Permissions.ContactsWrite)         | ✔️     | ✔️  | ❌      | ❌    |
| [Flashlight](xref:Microsoft.Maui.ApplicationModel.Permissions.Flashlight)               | ✔️     | ❌   | ❌      | ❌    |
| [LocationWhenInUse](xref:Microsoft.Maui.ApplicationModel.Permissions.LocationWhenInUse) | ✔️     | ✔️  | ❌      | ✔️   |
| [LocationAlways](xref:Microsoft.Maui.ApplicationModel.Permissions.LocationAlways)       | ✔️     | ✔️  | ❌      | ❌    |
| [Media](xref:Microsoft.Maui.ApplicationModel.Permissions.Media)                         | ❌      | ✔️  | ❌      | ❌    |
| [Microphone](xref:Microsoft.Maui.ApplicationModel.Permissions.Microphone)               | ✔️     | ✔️  | ❌      | ❌    |
| [NetworkState](xref:Microsoft.Maui.ApplicationModel.Permissions.NetworkState)           | ✔️     | ❌  | ❌      | ❌   |
| [Phone](xref:Microsoft.Maui.ApplicationModel.Permissions.Phone)                         | ✔️     | ✔️  | ❌      | ❌    |
| [Photos](xref:Microsoft.Maui.ApplicationModel.Permissions.Photos)                       | ❌     | ✔️  | ❌      | ✔️   |
| [PhotosAddOnly](xref:Microsoft.Maui.ApplicationModel.Permissions.PhotosAddOnly)         | ❌     | ✔️  | ❌       | ✔️   |
| [Reminders](xref:Microsoft.Maui.ApplicationModel.Permissions.Reminders)                 | ❌      | ✔️  | ❌      | ❌    |
| [Sensors](xref:Microsoft.Maui.ApplicationModel.Permissions.Sensors)                     | ✔️     | ✔️  | ❌      | ❌    |
| [Sms](xref:Microsoft.Maui.ApplicationModel.Permissions.Sms)                             | ✔️     | ✔️  | ❌      | ❌    |
| [Speech](xref:Microsoft.Maui.ApplicationModel.Permissions.Speech)                       | ✔️     | ✔️  | ❌      | ❌    |
| [StorageRead](xref:Microsoft.Maui.ApplicationModel.Permissions.StorageRead)             | ✔️     | ❌   | ❌      | ❌    |
| [StorageWrite](xref:Microsoft.Maui.ApplicationModel.Permissions.StorageWrite)           | ✔️     | ❌   | ❌      | ❌    |
| [Vibrate](xref:Microsoft.Maui.ApplicationModel.Permissions.Vibrate)                     | ✔️     | ❌   | ❌      | ❌    |

::: moniker-end

::: moniker range=">=net-maui-8.0"

| Permission                                                                              | Android | iOS | Windows | tvOS |
|------------------------------------------------------------------------------------------|:-------:|:---:|:-------:|:----:|
| [Battery](xref:Microsoft.Maui.ApplicationModel.Permissions.Battery)                     | ✔️     | ❌  | ❌     | ❌    |
| [Bluetooth](xref:Microsoft.Maui.ApplicationModel.Permissions.Bluetooth)                 | ✔️     | ❌  | ❌     | ❌    |
| [CalendarRead](xref:Microsoft.Maui.ApplicationModel.Permissions.CalendarRead)           | ✔️     | ✔️  | ❌      | ❌    |
| [CalendarWrite](xref:Microsoft.Maui.ApplicationModel.Permissions.CalendarWrite)         | ✔️     | ✔️  | ❌      | ❌    |
| [Camera](xref:Microsoft.Maui.ApplicationModel.Permissions.Camera)                       | ✔️     | ✔️  | ❌      | ❌    |
| [ContactsRead](xref:Microsoft.Maui.ApplicationModel.Permissions.ContactsRead)           | ✔️     | ✔️  | ❌      | ❌    |
| [ContactsWrite](xref:Microsoft.Maui.ApplicationModel.Permissions.ContactsWrite)         | ✔️     | ✔️  | ❌      | ❌    |
| [Flashlight](xref:Microsoft.Maui.ApplicationModel.Permissions.Flashlight)               | ✔️     | ❌   | ❌      | ❌    |
| [LocationWhenInUse](xref:Microsoft.Maui.ApplicationModel.Permissions.LocationWhenInUse) | ✔️     | ✔️  | ❌      | ✔️   |
| [LocationAlways](xref:Microsoft.Maui.ApplicationModel.Permissions.LocationAlways)       | ✔️     | ✔️  | ❌      | ❌    |
| [Media](xref:Microsoft.Maui.ApplicationModel.Permissions.Media)                         | ❌      | ✔️  | ❌      | ❌    |
| [Microphone](xref:Microsoft.Maui.ApplicationModel.Permissions.Microphone)               | ✔️     | ✔️  | ❌      | ❌    |
| [NearbyWifiDevices](xref:Microsoft.Maui.ApplicationModel.Permissions.NearbyWifiDevices) | ✔️     | ❌  | ❌     | ❌    |
| [NetworkState](xref:Microsoft.Maui.ApplicationModel.Permissions.NetworkState)           | ✔️     | ❌  | ❌      | ❌   |
| [Phone](xref:Microsoft.Maui.ApplicationModel.Permissions.Phone)                         | ✔️     | ✔️  | ❌      | ❌    |
| [Photos](xref:Microsoft.Maui.ApplicationModel.Permissions.Photos)                       | ❌     | ✔️  | ❌      | ✔️   |
| [PhotosAddOnly](xref:Microsoft.Maui.ApplicationModel.Permissions.PhotosAddOnly)         | ❌     | ✔️  | ❌       | ✔️   |
| [Reminders](xref:Microsoft.Maui.ApplicationModel.Permissions.Reminders)                 | ❌      | ✔️  | ❌      | ❌    |
| [Sensors](xref:Microsoft.Maui.ApplicationModel.Permissions.Sensors)                     | ✔️     | ✔️  | ❌      | ❌    |
| [Sms](xref:Microsoft.Maui.ApplicationModel.Permissions.Sms)                             | ✔️     | ✔️  | ❌      | ❌    |
| [Speech](xref:Microsoft.Maui.ApplicationModel.Permissions.Speech)                       | ✔️     | ✔️  | ❌      | ❌    |
| [StorageRead](xref:Microsoft.Maui.ApplicationModel.Permissions.StorageRead)             | ✔️     | ❌   | ❌      | ❌    |
| [StorageWrite](xref:Microsoft.Maui.ApplicationModel.Permissions.StorageWrite)           | ✔️     | ❌   | ❌      | ❌    |
| [Vibrate](xref:Microsoft.Maui.ApplicationModel.Permissions.Vibrate)                     | ✔️     | ❌   | ❌      | ❌    |

::: moniker-end

If a permission is marked as ❌, it will always return <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Granted> when checked or requested.

## Checking permissions

To check the current status of a permission, use the <xref:Microsoft.Maui.ApplicationModel.Permissions.CheckStatusAsync%2A?displayProperty=nameWithType> method along with the specific permission to get the status for. The following example checks the status of the [`LocationWhenInUse`](xref:Microsoft.Maui.ApplicationModel.Permissions.LocationWhenInUse) permission:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="permission_check":::

A <xref:Microsoft.Maui.ApplicationModel.PermissionException> is thrown if the required permission isn't declared.

It's best to check the status of the permission before requesting it. Each operating system returns a different default state, if the user has never been prompted. iOS returns <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Unknown>, while others return <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Denied>. If the status is <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Granted> then there's no need to make other calls. On iOS if the status is <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Denied> you should prompt the user to change the permission in the settings. On Android, you can call <xref:Microsoft.Maui.ApplicationModel.Permissions.ShouldShowRationale%2A> to detect if the user has already denied the permission in the past.

### Permission status

When using <xref:Microsoft.Maui.ApplicationModel.Permissions.CheckStatusAsync%2A> or <xref:Microsoft.Maui.ApplicationModel.Permissions.RequestAsync%2A>, a <xref:Microsoft.Maui.ApplicationModel.PermissionStatus> is returned that can be used to determine the next steps:

- <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Unknown>\
The permission is in an unknown state, or on iOS, the user has never been prompted.

- <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Denied>\
The user denied the permission request.

- <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Disabled>\
The feature is disabled on the device.

- <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Granted>\
The user granted permission or is automatically granted.

- <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Restricted>\
In a restricted state.

- <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Limited>\
In a limited state. Only iOS returns this status.

## Requesting permissions

To request a permission from the users, use the <xref:Microsoft.Maui.ApplicationModel.Permissions.RequestAsync%2A> method along with the specific permission to request. If the user previously granted permission, and hasn't revoked it, then this method will return <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Granted> without showing a dialog to the user. Permissions shouldn't be requested from your `MauiProgram` or `App` class, and should only be requested once the first page of the app has appeared.

The following example requests the [`LocationWhenInUse`](xref:Microsoft.Maui.ApplicationModel.Permissions.LocationWhenInUse) permission:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="permission_request":::

A <xref:Microsoft.Maui.ApplicationModel.PermissionException> is thrown if the required permission isn't declared.

> [!IMPORTANT]
> On some platforms, a permission request can only be activated a single time. Further prompts must be handled by the developer to check if a permission is in the <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Denied> state, and then ask the user to manually turn it on.

## Explain why permission is needed

It's best practice to explain to your user why your application needs a specific permission. On iOS, you must specify a string that is displayed to the user. Android doesn't have this ability, and also defaults permission status to <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Disabled>. This limits the ability to know if the user denied the permission or if it's the first time the permission is being requested. The <xref:Microsoft.Maui.ApplicationModel.Permissions.ShouldShowRationale%2A> method can be used to determine if an informative UI should be displayed. If the method returns `true`, this is because the user has denied or disabled the permission in the past. Other platforms always return `false` when calling this method.

## Example

The following code presents the general usage pattern for determining whether a permission has been granted, and then requesting it if it hasn't.

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="permission_check_and_request":::

## Extending permissions

The Permissions API was created to be flexible and extensible for applications that require more validation or permissions that aren't included in .NET MAUI. Create a class that inherits from <xref:Microsoft.Maui.ApplicationModel.Permissions.BasePermission>, and implement the required abstract methods. The following example code demonstrates the basic abstract members, but without implementation:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="permission_class":::

When implementing a permission in a specific platform, the <xref:Microsoft.Maui.ApplicationModel.Permissions.BasePlatformPermission> class can be inherited from. This class provides extra platform helper methods to automatically check the permission declarations. This helps when creating custom permissions that do groupings, for example requesting both **Read** and **Write** access to storage on Android. The following code example demonstrates requesting **Read** and **Write** storage access:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="permission_readwrite":::

You then check the permission in the same way as any other permission type provided by .NET MAUI:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="permission_readwrite_request":::

If you wanted to call this API from your cross-platform code, you could create an interface and register the custom permission as a dependency in the app's service container. The following example shows the `IReadWritePermission` interface:

```csharp
public interface IReadWritePermission
{        
    Task<PermissionStatus> CheckStatusAsync();
    Task<PermissionStatus> RequestAsync();
}
```

Then implement the interface in your custom permission:

```csharp
public class ReadWriteStoragePermission : Permissions.BasePlatformPermission, IReadWritePermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new List<(string androidPermission, bool isRuntime)>
    {
        (Android.Manifest.Permission.ReadExternalStorage, true),
        (Android.Manifest.Permission.WriteExternalStorage, true)
    }.ToArray();
}
```

In the `MauiProgram` class you should then register the interface and its concrete type, and the type that will consume the custom permission, in the app's service container:

```csharp
builder.Services.AddTransient<MyViewModel>();
builder.Services.AddSingleton<IReadWritePermission, ReadWriteStoragePermission>();
```

The custom permission implementation can then be resolved and invoked from one of your types, such as a viewmodel:

```csharp
public class MyViewModel
{
    IReadWritePermission _readWritePermission;

    public MyViewModel(IReadWritePermission readWritePermission)
    {
        _readWritePermission = readWritePermission;
    }

    public async Task CheckPermissionAsync()
    {
        var status = await _readWritePermission.CheckStatusAsync();
        if (status != PermissionStatus.Granted)
        {
            status = await _readWritePermission.RequestAsync();
        }
    }
}
```

## Platform differences

This section describes the platform-specific differences with the permissions API.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

Permissions must have the matching attributes set in the Android Manifest file. Permission status defaults to <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Denied>.

<!-- TODO For more information, see [Permissions in .NET MAUI for Android](../../android/app-fundamentals/permissions.md). -->

# [iOS/Mac Catalyst](#tab/macios)

Permissions must have a matching string in the _Info.plist_ file. Once a permission is requested and denied, a pop-up will no longer appear if you request the permission a second time. You must prompt your user to manually adjust the setting in the applications settings screen in iOS. Permission status defaults to <xref:Microsoft.Maui.ApplicationModel.PermissionStatus.Unknown>.

<!-- TODO For more information, see [iOS Security and Privacy Features](../ios/app-fundamentals/security-privacy.md). -->

# [Windows](#tab/windows)

No platform differences.

-----
<!-- markdownlint-enable MD025 -->
