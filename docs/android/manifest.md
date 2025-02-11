---
title: "Android app manifest"
description: Learn about the Android app manifest, AndroidManifest.xml, that describes essential information about your app to build tools, the Android OS, and Google Play.
ms.date: 08/30/2024
---

# Android app manifest

Every .NET Multi-platform App UI (.NET MAUI) app on Android has an *AndroidManifest.xml* file, located in the *Platforms\\Android* folder, that describes essential information about your app to build tools, the Android operating system, and Google Play.

The manifest file for your .NET MAUI Android app is generated as part of the .NET MAUI build process on Android. This build process takes the XML in the *Platforms\\Android\\AndroidManifest.xml* file, and merges it with any XML that's generated from specific attributes on your classes. The resulting manifest file can be found in the *obj* folder. For example, it can be found at *obj\\Debug\\net8.0-android\\AndroidManifest.xml* for debug builds on .NET 8.

> [!NOTE]
> Visual Studio 17.6+ includes an editor that simplifies the process of specifying app details, the target Android version, and required permissions in an Android manifest file.

## Generating the manifest

All .NET MAUI apps have a `MainActivity` class that derives from <xref:Android.App.Activity>, via the `MauiAppCompatActivity` class, and that has the <xref:Android.App.ActivityAttribute> applied to it. Some apps may include additional classes that derive from <xref:Android.App.Activity> and that have the <xref:Android.App.ActivityAttribute> applied.

At build time, assemblies are scanned for non-`abstract` classes that derive from <xref:Android.App.Activity> and that have the <xref:Android.App.ActivityAttribute> applied. These classes and attributes are used to generate the app's manifest. For example, consider the following code:

```csharp
using Android.App;

namespace MyMauiApp;

public class MyActivity : Activity
{
}
```

This example results in nothing being generated in the manifest file. For an `<activity/>` element to be generated, you'd need to add the <xref:Android.App.ActivityAttribute>:

```csharp
using Android.App;

namespace MyMauiApp;

[Activity]
public class MyActivity : Activity
{
}
```

This example causes the following XML fragment to be added to the manifest file:

```xml
<activity android:name="crc64bdb9c38958c20c7c.MyActivity" />
```

> [!NOTE]
> <xref:Android.App.ActivityAttribute> has no effect on `abstract` types.

## Activity name

The type name of an activity is based on the 64-bit cyclic redundancy check of the assembly-qualified name of the type being exported. This enables the same fully-qualified name to be provided from two different assemblies without receiving a packaging error.

To override this default and explicitly specify the name of your activity, use the <xref:Android.App.ActivityAttribute.Name> property:

```csharp
using Android.App;

namespace MyMauiApp;

[Activity (Name="companyname.mymauiapp.activity")]
public class MyActivity : Activity
{
}
```

This example produces the following XML fragment:

```xml
<activity android:name="companyname.mymauiapp.activity" />
```

> [!NOTE]
> You should only use the `Name` property for backward-compatibility reasons, as such renaming can slow down type lookup at runtime.

A typical scenario for setting the <xref:Android.App.ActivityAttribute.Name> property is when you need to obtain a readable Java name for your activity. This can be useful if another Android app needs to be able to open your app, or if you have a script for launching your app and testing startup time.

## Launch from the app chooser

If your .NET MAUI Android app contains multiple activities, and you need to specify which activity should be launchable from the app launcher, use the <xref:Android.App.ActivityAttribute.MainLauncher> property:

```csharp
using Android.App;

namespace MyMauiApp;

[Activity (Label="My Maui App", MainLauncher = true)]
public class MyActivity : Activity
{
}
```

This example produces the following XML fragment:

```xml
<activity android:label="My Maui App"
          android:name="crc64bdb9c38958c20c7c.MainActivity">
  <intent-filter>
    <action android:name="android.intent.action.MAIN" />
    <category android:name="android.intent.category.LAUNCHER" />
  </intent-filter>
</activity>
```

## Permissions

When you add permissions to an Android app, they're recorded in the manifest file. For example, if you set the `ACCESS_NETWORK_STATE` permission, the following element is added to the manifest file:

```xml
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
```

The .NET MAUI app project template sets the `INTERNET` and `ACCESS_NETWORK_STATE` permissions in *Platforms\\Android\\AndroidManifest.xml*, because most apps require internet access. If you remove the `INTERNET` permission from your manifest, debug builds will still include the permission in the generated manifest file.

> [!TIP]
> If you find that switching to a release build causes your app to lose a permission that was available in the debug build, verify that you've explicitly set the required permission in your manifest file.

## Intent actions and features

The Android manifest file provides a way for you to describe the capabilities of your app. This is achieved via [Intents](https://developer.android.com/guide/topics/manifest/intent-filter-element.html) and the <xref:Android.App.IntentFilterAttribute>. You can specify which actions are appropriate for your activity with the <xref:Android.App.IntentFilterAttribute> constructor, and which categories are appropriate with the <xref:Android.App.IntentFilterAttribute.Categories> property. At least one activity must be provided, which is why activities are provided in the constructor. An `[IntentFilter]` can be provided multiple times, and each use results in a separate `<intent-filter/>` element within the `<activity/>`:

```csharp
using Android.App;
using Android.Content;

namespace MyMauiApp;

[Activity(Label = "My Maui App", MainLauncher = true)]
[IntentFilter(new[] {Intent.ActionView},
    Categories = new[] {Intent.CategorySampleCode, "my.custom.category"})]
public class MyActivity : Activity
{
}
```

This example produces the following XML fragment:

```xml
<activity android:label="My Maui App"
          android:name="crc64bdb9c38958c20c7c.MainActivity">
  <intent-filter>
    <action android:name="android.intent.action.MAIN" />
    <category android:name="android.intent.category.LAUNCHER" />
  </intent-filter>
  <intent-filter>
    <action android:name="android.intent.action.VIEW" />
    <category android:name="android.intent.category.SAMPLE_CODE" />
    <category android:name="my.custom.category" />
  </intent-filter>
</activity>
```

### Application element

The Android manifest file also provides a way for you to declare properties for your entire app. This is achieved via the `<application>` element and its counterpart, the <xref:Android.App.ApplicationAttribute>. Typically, you declare `<application>` properties for your entire app and then override these properties as required on an activity basis.

For example, the following `Application` attribute could be added to *MainApplication.cs* to indicate that the app's user-readable name is "My Maui App", and that it uses the `Maui.SplashTheme` style as the default theme for all activities:

```csharp
using Android.App;
using Android.Runtime;

namespace MyMauiApp;

[Application(Label = "My Maui App", Theme = "@style/Maui.SplashTheme")]
public class MainApplication : MauiApplication
{
      public MainApplication(IntPtr handle, JniHandleOwnership ownership)
             : base(handle, ownership)
      {
      }

      protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

```

This declaration causes the following XML fragment to be generated in *obj\\Debug\\net8.0-android\\AndroidManifest.xml*:

```xml
<application android:label="MyMauiApp" android:theme="@style/Maui.SplashTheme" android:debuggable="true" ...>
```

> [!NOTE]
> Debug builds automatically set `android:debuggable="true"` so that debuggers and other tooling can attach to your app. However, it isn't set for release builds.

In this example, all activities in the app will default to the `Maui.SplashTheme` style. If you set an activity's theme to `Maui.MyTheme`, only that activity will use the `Maui.MyTheme` style while any other activities in your app will default to the `Maui.SplashTheme` style that's set in the `<application>` element.

The <xref:Android.App.ApplicationAttribute> isn't the only way to configure `<application>` attributes. You can also insert properties directly into the `<application>` element of the manifest file. These properties are then merged into the generated manifest file. For more information, see the properties section of <xref:Android.App.ApplicationAttribute>.

> [!IMPORTANT]
> The content of *Platforms\\Android\\AndroidManifest.xml* always overrides data provided by attributes.

## App title bar

Android apps have a title bar that displays a label. The value of the `$(ApplicationTitle)` build property, in your .NET MAUI app project file, is displayed on the title bar. .NET MAUI includes it in the generated manifest as the value of [`android.label`](https://developer.android.com/guide/topics/manifest/application-element.html#label):

```xml
<application android:label="My Maui App" ... />
```

To specify an activities label on the title bar, use the <xref:Android.App.ActivityAttribute.Label> property:

```csharp
using Android.App;

namespace MyMauiApp;

[Activity (Label="My Maui App")]
public class MyActivity : Activity
{
}
```

This example produces the following XML fragment:

```xml
<activity android:label="My Maui App"
          android:name="crc64bdb9c38958c20c7c.MyActivity" />
```

## App icon

By default, your app will be given a .NET icon. For information about specifying a custom icon, see [Change a .NET MAUI app icon](~/user-interface/images/app-icons.md?&tabs=android#platform-specific-configuration).

## Attributes

The following table shows the .NET for Android attributes that generate Android manifest XML fragments:

| Attribute | Description |
| --------- | ----------- |
| <xref:Android.App.ActivityAttribute?displayProperty=fullName> | Generates an [activity](https://developer.android.com/guide/topics/manifest/activity-element.html) XML fragment. |
| <xref:Android.App.ApplicationAttribute?displayProperty=fullName> | Generates an [application](https://developer.android.com/guide/topics/manifest/application-element.html) XML fragment. |
| <xref:Android.App.InstrumentationAttribute?displayProperty=fullName> | Generates an [instrumentation](https://developer.android.com/guide/topics/manifest/instrumentation-element.html) XML fragment. |
| <xref:Android.App.IntentFilterAttribute?displayProperty=fullName> | Generates an [intent-filter](https://developer.android.com/guide/topics/manifest/intent-filter-element.html) XML fragment. |
| <xref:Android.App.MetaDataAttribute?displayProperty=fullName> | Generates a [meta-data](https://developer.android.com/guide/topics/manifest/meta-data-element.html) XML fragment. |
| <xref:Android.App.PermissionAttribute?displayProperty=fullName> | Generates a [permission](https://developer.android.com/guide/topics/manifest/permission-element.html) XML fragment. |
| <xref:Android.App.PermissionGroupAttribute?displayProperty=fullName> | Generates a [permission-group](https://developer.android.com/guide/topics/manifest/permission-group-element.html) XML fragment. |
| <xref:Android.App.PermissionTreeAttribute?displayProperty=fullName> | Generates a [permission-tree](https://developer.android.com/guide/topics/manifest/permission-tree-element.html) XML fragment. |
| <xref:Android.App.ServiceAttribute?displayProperty=fullName> | Generates a [service](https://developer.android.com/guide/topics/manifest/service-element.html) XML fragment. |
| <xref:Android.App.UsesLibraryAttribute?displayProperty=fullName> | Generates a [uses-library](https://developer.android.com/guide/topics/manifest/uses-library-element.html) XML fragment. |
| <xref:Android.App.UsesPermissionAttribute?displayProperty=fullName> | Generates a [uses-permission](https://developer.android.com/guide/topics/manifest/uses-permission-element.html) XML fragment. |
| <xref:Android.Content.BroadcastReceiverAttribute?displayProperty=fullName> | Generates a [receiver](https://developer.android.com/guide/topics/manifest/receiver-element.html) XML fragment. |
| <xref:Android.Content.ContentProviderAttribute?displayProperty=fullName> | Generates a [provider](https://developer.android.com/guide/topics/manifest/provider-element.html) XML fragment. |
| <xref:Android.Content.GrantUriPermissionAttribute?displayProperty=fullName> | Generates a [grant-uri-permission](https://developer.android.com/guide/topics/manifest/grant-uri-permission-element.html) XML fragment. |

## See also

- [App Manifest Overview](https://developer.android.com/guide/topics/manifest/manifest-intro) on developer.android.com
