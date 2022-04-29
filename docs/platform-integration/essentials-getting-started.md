---
title: "Get Started with Essentials"
description: "Get started with Essentials in .NET MAUI. Essentials provides a single cross-platform API that your app can use to access device features, operating system functionality, and phone support, among other capabilities."
ms.date: 08/11/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Get started with .NET MAUI Essentials

----
TODO: This was part of the original /essentials/ folder. I'm not yet sure what config there will be. It seems this article was required for Android config. We'll have to see what is required for "Init" since essentials is now gone and the code base is just directly integrated into .net maui.
----

.NET Multi-platform App UI (.NET MAUI) Essentials provides a single cross-platform API that works with any iOS, Android, or Windows application. Essentials is accessed from cross-platform friendly code that is ignorant of the platform it's run on. Some APIs do require platform-specific configure or setup, but that's the exception rather than the rule. For more information about platform and operating system support, see [.NET MAUI supported platforms](../../supported-platforms.md).

## Migrating from Xamarin.Forms

Unlike Xamarin.Forms Essentials, .NET MAUI Essentials is included with .NET MAUI. You're no longer required to install a NuGet package or add a reference to the Essentials library.

## Setup

<!-- TODO: Final requirements are needed before GA -->
<!-- TODO: What do we mention about AndroidX? -->

.NET MAUI Essentials requires setup on Android. iOS and Windows don't require any setup to enable access to Essentials.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

.NET MAUI Essentials supports a minimum Android version of _4.4_, corresponding to _API level 19_, on the client device. But when compiling your project, the target Android version must be _9.0_ or _10.0_, corresponding to _API level 28_ and _API level 29_.

- Visual Studio for Windows

  These two versions are set in the Android project's properties. Right-click the project node in **Solution Explorer** and choose **Properties**, and navigate to the **Android Manifest** property page.

- Visual Studio for Mac

  These two versions are set in the Project Options dialog for the Android project. Double-click the project node in the **Solution** pane, or right-click the project node and then select **Options**, then navigate to the **Android Application** tab.

<!-- markdownlint-disable MD001 -->
### Configure the MainApplication or Activity
<!-- markdownlint-enable MD001 -->

<!-- TODO: Verify this is no longer required by GA -->

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
