---
title: "Vibration"
description: "Describes the Vibration class in Microsoft.Maui.Essentials, which lets you start and stop the vibrate functionality for a desired amount of time."
ms.date: 11/04/2018
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Vibration

The `Vibration` class lets you start and stop the vibrate functionality for a desired amount of time.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

To access the **Vibration** functionality the following platform specific setup is required.

# [Android](#tab/android)

The Vibrate permission is required and must be configured in the Android project. This can be added in the following ways:

Open the **AssemblyInfo.cs** file under the **Properties** folder and add:

```csharp
[assembly: UsesPermission(Android.Manifest.Permission.Vibrate)]
```

OR Update Android Manifest:

Open the **AndroidManifest.xml** file under the **Properties** folder and add the following inside of the **manifest** node.

```xml
<uses-permission android:name="android.permission.VIBRATE" />
```

Or right click on the Android project and open the project's properties. Under **Android Manifest** find the **Required permissions:** area and check the **VIBRATE** permission. This will automatically update the **AndroidManifest.xml** file.

# [iOS](#tab/ios)

No additional setup required.

# [Windows](#tab/windows)

No additional setup required.

-----

## Using Vibration

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

The Vibration functionality can be requested for a set amount of time or the default of 500 milliseconds.

```csharp
try
{
    // Use default vibration length
    Vibration.Vibrate();

    // Or use specified time
    var duration = TimeSpan.FromSeconds(1);
    Vibration.Vibrate(duration);
}
catch (FeatureNotSupportedException ex)
{
    // Feature not supported on device
}
catch (Exception ex)
{
    // Other error has occurred.
}
```

Cancellation of device vibration can be requested with the `Cancel` method:

```csharp
try
{
    Vibration.Cancel();
}
catch (FeatureNotSupportedException ex)
{
    // Feature not supported on device
}
catch (Exception ex)
{
    // Other error has occurred.
}
```

## Platform differences

# [Android](#tab/android)

No platform differences.

# [iOS](#tab/ios)

- Only vibrates when device is set to "Vibrate on ring".
- Always vibrates for 500 milliseconds.
- Not possible to cancel vibration.

# [Windows](#tab/windows)

No platform differences.

-----

## API

- [Vibration source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Vibration)
<!-- - [Vibration API documentation](xref:Microsoft.Maui.Essentials.Vibration)-->
