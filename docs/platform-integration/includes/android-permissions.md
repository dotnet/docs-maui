---
ms.topic: include
ms.date: 05/03/2022
---

### Android permissions

This API uses runtime permissions on Android. Please ensure that .NET MAUI is fully initialized and permission handling is setup in your app. In the Android project's `MainLauncher` or any `Activity` that is launched, .NET MAUI must be initialized in the `OnCreate` method:

:::code language="csharp" source="../snippets/shared_1/Platforms/Android/MainActivity.cs" id="OnCreate":::

To handle runtime permissions on Android, .NET MAUI must receive any `OnRequestPermissionsResult`. Add the following code to all `Activity` classes:

:::code language="csharp" source="../snippets/shared_1/Platforms/Android/MainActivity.cs" id="OnRequest":::
