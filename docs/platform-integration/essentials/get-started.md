---
title: "Get Started with Essentials"
description: "Get started with Essentials in .NET MAUI. Essentials provides a single cross-platform API that your app can use to access device features, operating system functionality, and phone support, among other capabilities."
ms.date: 08/11/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Get started with .NET MAUI Essentials

.NET Multi-platform App UI (.NET MAUI) Essentials provides a single cross-platform API that works with any iOS, Android, or UWP application. Essentials is accessed from cross-platform friendly code that is ignorant of the platform it's run on. Some APIs do require platform-specific configure or setup, but that's the exception rather than the rule. For more information about platform and operating system support, see [Platform Support](platform-feature-support.md).

## Migrating from Xamarin.Forms

Unlike Xamarin.Forms Essentials, .NET MAUI Essentials is included with .NET MAUI. You're no longer required to install a NuGet package or add reference to the Essentials library.

## Setup

.NET MAUI Essentials requires setup on Android. iOS and Windows don't require any setup to enable access to Essentials.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

.NET MAUI Essentials supports a minimum Android version of _4.4_, corresponding to _API level 19_, on the client device. But when compiling your project, the target Android version must be _9.0_ or _10.0_, corresponding to _API level 28_ and _API level 29_.

- Visual Studio for Windows

  These two versions are set in the Android project's properties. Right-click the project node in **Solution Explorer** and choose **Properties**, and navigate to the **Android Manifest** property page.

- Visual Studio for Mac

  These two versions are set in the Project Options dialog for the Android project. Double-click the project node in the **Solution** pane, or right-click the project node and then select **Options**, then navigate to the **Android Application** tab.

<!-- TODO: What is still valid in these two paragraphs? -->
When compiling against Android 9.0, Xamarin.Essentials installs version 28.0.0.3 of the Xamarin.Android.Support libraries that it requires. Any other Xamarin.Android.Support libraries that your application requires should also be updated to version 28.0.0.3 using the NuGet package manager. All Xamarin.Android.Support libraries used by your application should be the same, and should be at least version 28.0.0.3. Refer to the [troubleshooting page](troubleshooting.md) if you have issues adding the Xamarin.Essentials NuGet or updating NuGets in your solution.

Starting with version 1.5.0 when compiling against Android 10.0, Xamarin.Essentials install AndroidX support libraries that it requires. Read through the [AndroidX documentation](../android/platform/androidx.md) if you have not made the transition yet.

<!-- markdownlint-disable MD001 -->
### Configure the MainApplication or Activity
<!-- markdownlint-enable MD001 -->

In the Android project's `MainApplication` or any `Activity` that is launched, Essentials must be initialized in the `OnCreate` method by calling the `Microsoft.Maui.Essentials.Platform.Init`method:

```csharp
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Microsoft.Maui.Essentials.Platform.Init(this, savedInstanceState);
    }

    // ... Other code ...
}
```

### Configure the permissions

To handle runtime permissions on Android, permission requests must be passed on to Essentials by calling the `Microsoft.Maui.Essentials.Platform.OnRequestPermissionsResult` method. Add the following code to all `Activity` classes:

```csharp
public class MainActivity : MauiAppCompatActivity
{
    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        Microsoft.Maui.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    // ... Other code ...
}
```

# [iOS](#tab/ios)

No extra setup required.

# [Windows](#tab/windows)

No extra setup required.

-----
<!-- markdownlint-enable MD025 -->
