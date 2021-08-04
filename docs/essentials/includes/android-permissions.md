---
ms.topic: include
ms.date: 08/04/2021
---

This API uses runtime permissions on Android. Please ensure that .NET MAUI Essentials is fully initialized and permission handling is setup in your app. For more information, see [Get Started with .NET MAUI Essentials](../get-started.md).

In the Android project's `MainLauncher` or any `Activity` that is launched, .NET MAUI Essentials must be initialized in the `OnCreate` method:

```csharp
protected override void OnCreate(Bundle savedInstanceState) 
{
    //...
    base.OnCreate(savedInstanceState);
    Microsoft.Maui.Essentials.Platform.Init(this, savedInstanceState); // add this line to your code, it may also be called: bundle
    //...
}    
```

To handle runtime permissions on Android, .NET MAUI Essentials must receive any `OnRequestPermissionsResult`. Add the following code to all `Activity` classes:

```csharp
public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
{
    Microsoft.Maui.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
}
```
