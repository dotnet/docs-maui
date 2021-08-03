---
ms.topic: include
author: jamesmontemagno
ms.author: jamont
ms.date: 05/11/2020
---
This API uses runtime permissions on Android. Please ensure that Xamarin.Essentials is fully initialized and permission handling is setup in your app.

In the Android project's `MainLauncher` or any `Activity` that is launched Xamarin.Essentials must be initialized in the `OnCreate` method:

```csharp
protected override void OnCreate(Bundle savedInstanceState) 
{
    //...
    base.OnCreate(savedInstanceState);
    Xamarin.Essentials.Platform.Init(this, savedInstanceState); // add this line to your code, it may also be called: bundle
    //...
}    
```

To handle runtime permissions on Android, Xamarin.Essentials must receive any `OnRequestPermissionsResult`. Add the following code to all `Activity` classes:

```csharp
public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
{
    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
}
```
