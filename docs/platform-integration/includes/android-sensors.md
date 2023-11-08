---
ms.topic: include
ms.date: 11/08/2023
---

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

If your app targets Android 12+ (API 31+), the system places a 200 Hz limit on the refresh rate of data from this sensor. If your app needs to gather sensor data at a higher rate, you must declare the `HIGH_SAMPLING_RATE_SENSORS` permission. You can configure the permission in the following ways:

- Add the assembly-based permission:

  Open the _Platforms/Android/MainApplication.cs_ file and add the following assembly attribute after `using` directives:

  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.HighSamplingRateSensors)]
  ```

  \- or -

- Update the Android Manifest:

    Open the _Platforms/Android/AndroidManifest.xml_ file and add the following line in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.HIGH_SAMPLING_RATE_SENSORS" />
  ```

<!-- TODO not yet supported>
  \- or -

- Use the Android project properties:

  Right-click on the Android project and open the project's properties. Under _Android Manifest_ find the **Required permissions:** area and check the **High Sampling Rate Sensors** permission. This will automatically update the _AndroidManifest.xml_ file.
-->

> [!NOTE]
> If a user turns off microphone access using the device toggles, motion and position sensors are always rate-limited, regardless of whether you declare the `HIGH_SAMPLING_RATE_SENSORS` permission.

# [iOS/Mac Catalyst](#tab/macios)

No setup is required.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->
