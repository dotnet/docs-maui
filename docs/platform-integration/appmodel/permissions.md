---
title: "Permissions"
description: "Learn how to use the .NET MAUI Permissions class, to check and request permissions. This class is in the Microsoft.Maui.ApplicationModel namespace."
ms.date: 05/09/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel"]
---

# Permissions

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `Permissions` class. This class allows you to check and request permissions at run-time. The `Permissions` type is available in the `Microsoft.Maui.ApplicationModel` namespace.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

## Get started

The following information applies to Android platforms.

[!INCLUDE [android-permissions](../includes/android-permissions.md)]

## Available permissions

.NET MAUI attempts to abstract as many permissions as possible. However, each operating system has a different set of permissions. Even though the API allows access to a common permission, there may be differences between operating systems related to that permission. The following table describes the available permissions:

The following table uses ✔️ to indicate that the permission is supported and ❌ to indicate the permission isn't supported or isn't required:

| Permission        | Android | iOS | Windows | tvOS |
|-------------------|:-------:|:---:|:-------:|:----:|
| CalendarRead      | ✔️     | ✔️  | ❌      | ❌   |
| CalendarWrite     | ✔️     | ✔️  | ❌      | ❌   |
| Camera            | ✔️     | ✔️  | ❌      | ❌   |
| ContactsRead      | ✔️     | ✔️  | ✔️      | ❌   |
| ContactsWrite     | ✔️     | ✔️  | ✔️      | ❌   |
| Flashlight        | ✔️     | ❌  | ❌      | ❌   |
| LocationWhenInUse | ✔️     | ✔️  | ✔️      | ✔️   |
| LocationAlways    | ✔️     | ✔️  | ✔️      | ❌   |
| Media             | ❌     | ✔️  | ❌      | ❌   |
| Microphone        | ✔️     | ✔️  | ✔️      | ❌   |
| Phone             | ✔️     | ✔️  | ❌      | ❌   |
| Photos            | ❌     | ✔️  | ❌      | ✔️   |
| Reminders         | ❌     | ✔️  | ❌      | ❌   |
| Sensors           | ✔️     | ✔️  | ✔️      | ❌   |
| Sms               | ✔️     | ✔️  | ❌      | ❌   |
| Speech            | ✔️     | ✔️  | ❌      | ❌   |
| StorageRead       | ✔️     | ❌  | ❌      | ❌   |
| StorageWrite      | ✔️     | ❌  | ❌      | ❌   |

If a permission is marked as ❌, it will always return `Granted` when checked or requested.

## Checking permissions

To check the current status of a permission, use the `Permissions.CheckStatusAsync` method along with the specific permission to get the status for. The following example checks the status of the `LocationWhenInUse` permission:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="permission_check":::

A `PermissionException` is thrown if the required permission isn't declared.

It's best to check the status of the permission before requesting it. Each operating system returns a different default state, if the user has never been prompted. iOS returns `Unknown`, while others return `Denied`. If the status is `Granted` then there's no need to make other calls. On iOS if the status is `Denied` you should prompt the user to change the permission in the settings. On Android, you can call `ShouldShowRationale` to detect if the user has already denied the permission in the past.

### Permission status

When using `CheckStatusAsync` or `RequestAsync`, a `PermissionStatus` is returned that can be used to determine the next steps:

- `Unknown`\
The permission is in an unknown state, or on iOS, the user has never been prompted.

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

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="permission_request":::

A `PermissionException` is thrown if the required permission isn't declared.

> [!IMPORTANT]
> On some platforms, a permission request can only be activated a single time. Further prompts must be handled by the developer to check if a permission is in the `Denied` state, and then ask the user to manually turn it on.

## Explain why permission is needed

It's best practice to explain to your user why your application needs a specific permission. On iOS, you must specify a string that is displayed to the user. Android doesn't have this ability, and also defaults permission status to `Disabled`. This limits the ability to know if the user denied the permission or if it's the first time the permission is being requested. The `ShouldShowRationale` method can be used to determine if an informative UI should be displayed. If the method returns `true`, this is because the user has denied or disabled the permission in the past. Other platforms always return `false` when calling this method.

## Example

The following code presents the general usage pattern for determining whether a permission has been granted, and then requesting it if it hasn't.

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="permission_check_and_request":::

## Extending permissions

The Permissions API was created to be flexible and extensible for applications that require more validation or permissions that aren't included in .NET MAUI. Create a class that inherits from `Permissions.BasePermission`, and implement the required abstract methods. The following example code demonstrates the basic abstract members, but without implementation:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="permission_class":::

When implementing a permission in a specific platform, the `BasePlatformPermission` class can be inherited from. This class provides extra platform helper methods to automatically check the permission declarations. This helps when creating custom permissions that do groupings, for example requesting both **Read** and **Write** access to storage on Android. The following code example demonstrates requesting **Read** and **Write** storage access:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="permission_readwrite":::

You then check the permission in the same way as any other permission type provided by .NET MAUI:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="permission_readwrite_request":::

<!-- Cutting this
     It should be updated to demonstrate creating a class in each of the platform project folders, or by simply using #if OS checks
     However, at this time VS isn't giving me the correct APIs when I swap from Android to Windows, so I can't even try to write
     any code outside of Android

     Also, this talks about the "shared" project concept which isn't required in .NET MAUI. I would also assume that
     using DependencyService.Register isn't required. So this code will have to be updated too.
>

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
public class ReadWriteStoragePermission : Permissions.BasePlatformPermission, IReadWritePermission
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
-->

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
