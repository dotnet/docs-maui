---
title: "Haptic Feedback"
description: "Describes the HapticFeedback class in Microsoft.Maui.Essentials, which lets you control haptic feedback on device."
author: dimonovdd
ms.date: 01/04/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Haptic Feedback

The `HapticFeedback` class lets you control haptic feedback on device.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

To access the **HapticFeedback** functionality the following platform specific setup is required.

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

No platform differences.

-----

## Using Haptic Feedback

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

The Haptic Feedback functionality can be performed with a `Click` or `LongPress` feedback type.

```csharp
try
{
    // Perform click feedback
    HapticFeedback.Perform(HapticFeedbackType.Click);

    // Or use long press    
    HapticFeedback.Perform(HapticFeedbackType.LongPress);
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

## API

- [HapticFeedback source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/HapticFeedback)
<!-- - [HapticFeedback API documentation](xref:Microsoft.Maui.Essentials.HapticFeedback)-->
