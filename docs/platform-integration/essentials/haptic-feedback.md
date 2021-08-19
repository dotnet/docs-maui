---
title: "Haptic Feedback"
description: "Learn how to use the .NET MAUI HapticFeedback class in the Microsoft.Maui.Essentials namespace. This class lets you control haptic feedback on a device."
ms.date: 08/19/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Haptic Feedback

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `HapticFeedback` class to control haptic feedback on a device.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

To access the haptic feedback functionality, the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

The `Vibrate` permission is required and must be configured in the Android project. This can be added in the following ways:

- Add the assembly-based permission:

  Open the _AssemblyInfo.cs_ file under the **Properties** folder and add:

  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.Vibrate)]
  ```

  \- or -

- Update the Android Manifest:

  Open the _AndroidManifest.xml_ file under the **Properties** folder and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.VIBRATE" />
  ```

  \- or -

- Use the Android project properties:

  Right-click on the Android project and open the project's properties. Under _Android Manifest_ find the **Required permissions:** area and check the **VIBRATE** permission. This will automatically update the _AndroidManifest.xml_ file.

# [iOS](#tab/ios)

No setup is required.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Use haptic feedback

The haptic feedback functionality can be performed with a `Click` or `LongPress` feedback type.

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

- [HapticFeedback source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/HapticFeedback)
<!-- - [HapticFeedback API documentation](xref:Microsoft.Maui.Essentials.HapticFeedback)-->
