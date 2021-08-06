---
title: "Get Started with Xamarin.Essentials"
description: "Get started with Xamarin.Essentials, which provides a single cross-platform API that works with any iOS, Android, or UWP application."
ms.date: 05/11/2020
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Get Started with Xamarin.Essentials

Xamarin.Essentials provides a single cross-platform API that works with any iOS, Android, or UWP application that can be accessed from shared code no matter how the user interface is created. See the [platform & feature support guide](platform-feature-support.md) for more information on supported operating systems.

## Installation

Xamarin.Essentials is available as a NuGet package and is included in every new project in Visual Studio. It can also be added to any existing projects using Visual Studio with the following steps.

1. Download and install [Visual Studio](https://visualstudio.microsoft.com/) with the [Visual Studio tools for Xamarin](~/xamarin-forms/get-started/installation/index.md).

2. Open an existing project, or create a new project using the Blank App template under **Visual Studio C#** (Android, iPhone & iPad, or Cross-Platform).

    > [!IMPORTANT]
    > If adding to a UWP project ensure Build 16299 or higher is set in the project properties.

3. Add the [**Xamarin.Essentials**](https://www.nuget.org/packages/Xamarin.Essentials/) NuGet package to each project:

    <!--markdownlint-disable MD023 -->
    # [Visual Studio](#tab/windows)

    In the Solution Explorer panel, right click on the solution name and select **Manage NuGet Packages**. Search for **Xamarin.Essentials** and install the package into **ALL** projects including Android, iOS, UWP, and .NET Standard libraries.

    # [Visual Studio for Mac](#tab/macos)

    In the Solution Explorer panel, right click on the project name and select **Add > Add NuGet Packages...**. Search for **Xamarin.Essentials** and install the package into **ALL** projects including Android, iOS, and .NET Standard libraries.

    -----

4. Add a reference to Xamarin.Essentials in any C# class to reference the APIs.

    ```csharp
    using Xamarin.Essentials;
    ```

5. Xamarin.Essentials requires platform-specific setup:

    # [Android](#tab/android)

    Xamarin.Essentials supports a minimum Android version of 4.4, corresponding to API level 19, but the target Android version for compiling must be 9.0 or 10.0, corresponding to API level 28 and level 29. (In Visual Studio, these two versions are set in the Project Properties dialog for the Android project, in the Android Manifest tab. In Visual Studio for Mac, they're set in the Project Options dialog for the Android project, in the Android Application tab.)

    When compiling against Android 9.0, Xamarin.Essentials installs version 28.0.0.3 of the Xamarin.Android.Support libraries that it requires. Any other Xamarin.Android.Support libraries that your application requires should also be updated to version 28.0.0.3 using the NuGet package manager. All Xamarin.Android.Support libraries used by your application should be the same, and should be at least version 28.0.0.3. Refer to the [troubleshooting page](troubleshooting.md) if you have issues adding the Xamarin.Essentials NuGet or updating NuGets in your solution.

    Starting with version 1.5.0 when compiling against Android 10.0, Xamarin.Essentials install AndroidX support libraries that it requires. Read through the [AndroidX documentation](../android/platform/androidx.md) if you have not made the transition yet.

    In the Android project's `MainLauncher` or any `Activity` that is launched, Xamarin.Essentials must be initialized in the `OnCreate` method:

    ```csharp
    protected override void OnCreate(Bundle savedInstanceState) {
        //...
        base.OnCreate(savedInstanceState);
        Xamarin.Essentials.Platform.Init(this, savedInstanceState); // add this line to your code, it may also be called: bundle
        //...
    ```

    To handle runtime permissions on Android, Xamarin.Essentials must receive any `OnRequestPermissionsResult`. Add the following code to all `Activity` classes:

    ```csharp
    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
    {
        Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
    ```

    # [iOS](#tab/ios)

    No additional setup required.

    # [Windows](#tab/windows)

    No additional setup required.

    -----

6. Follow the [Xamarin.Essentials guides](index.md) that enable you to copy and paste code snippets for each feature.

## Xamarin.Essentials - Cross-Platform APIs for Mobile Apps (video)

> [!Video https://channel9.msdn.com/Shows/XamarinShow/Snack-Pack-XamarinEssentials-Cross-Platform-APIs-for-Mobile-Apps/player]

## Other Resources

We recommend developers new to Xamarin visit [getting started with Xamarin development](~/cross-platform/getting-started/index.md).

Visit the [Xamarin.Essentials GitHub Repository](https://github.com/xamarin/Essentials) to see the current source code, what is coming next, run samples, and clone the repository. Community contributions are welcome!

Browse through the [API documentation](xref:Xamarin.Essentials) for every feature of Xamarin.Essentials.
