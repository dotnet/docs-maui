---
title: "Android app manifest"
description: Learn about the Android app manifest, AndroidManifest.xml, that describes essential information about your app to build tools, the Android OS, and Google Play.
ms.date: 03/07/2023
---

# Android app manifest

Every .NET Multi-platform App UI (.NET MAUI) app on Android has an *AndroidManifest.xml* file, located in the *Platforms\\Android\\* folder, that describes essential information about your app to build tools, the Android operating system, and Google Play.

The Android manifest file for your .NET MAUI Android app is generated as part of the .NET MAUI build process on Android. This build process takes the XML in the *Platforms\\Android\\AndroidManifest.xml* file, and merges it with any XML that is generated from specific attributes that are added to your classes. The resulting manifest file can be find in the *obj* folder. For example, it can be found at *obj\\Debug\\net7.0-android\\AndroidManifest.xml* for Debug builds on .NET 7.

> ![NOTE]
> SOMETHING ABOUT THE NEW EDITOR HERE.

## Generating the manifest

At compile time, assemblies are scanned for non-`abstract` classes that derive from <xref:Android.App.Activity> and have the <xref:Android.App.ActivityAttribute> attribute declared on them. These classes and attributes are used to build the manifest. For example, consider the following code:

```csharp
using Android.App;

namespace MyMauiApp;

public class MyActivity : Activity
{
}
```

This code results in nothing being generated in the Android manifest file. If you want an `<activity/>` element to be generated, you need to use the (xref:Android.App.ActivityAttribute) attribute:

```csharp
using Android.App;

namespace MyMauiApp;

[Activity]
public class MyActivity : Activity
{
}
```

This example causes the following XML fragment to be added to the Android manifest file:

```xml
<activity android:name="crc64bdb9c38958c20c7c.MainActivity" />
```

The `[Activity]` attribute has no effect on `abstract` types; `abstract` types are ignored.

## Activity name

The type name of an activity is based on the CRC64 of the assembly-qualified name of the type being exported. This enables the same fully-qualified name to be provided from two different assemblies and not get a packaging error.

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

## Activity title bar

By default, Android app's have a title bar that displays a label. The value used for this is [`/manifest/application/activity/@android:label`](https://developer.android.com/guide/topics/manifest/activity-element.html#label). In most cases, this value will differ from your class name. To specify your app's label on the title bar, use the <xref:Android.App.ActivityAttribute.Label> property:

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
          android:name="crc64bdb9c38958c20c7c.MainActivity" />
```

## Launch from the app chooser

By default, your activity will not show up in Android's app launcher screen. This is because there will likely be many activities in your app, and you don't want an icon for every one. To specify which activity should be launchable from the app launcher, use the <xref:Android.App.ActivityAttribute.MainLauncher> property:

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

Debug builds automatically set the `INTERNET` permission. However, this permission is set in the generated *Platforms\\Android\\AndroidManifest.xml* file. If you examine the release build version of the manifest at *obj\\Release\\net7.0-android\\AndroidManifest.xml*, the `INTERNET` permission is not present.

> [!TIP]
> If you find that switching to a release build causes your app to lose a permission that was available in the debug build, verify that you've explicitly set the required permission in your manifest file.

## Intent actions and features

The Android manifest file provides a way for you to describe the capabilities of your app. This is achieved done via [Intents](https://developer.android.com/guide/topics/manifest/intent-filter-element.html) and the <xref:Android.App.IntentFilterAttribute> attribute. You can specify which actions are appropriate for your activity with the <xref:Android.App.IntentFilterAttribute> constructor, and which categories are appropriate with the <xref:Android.App.IntentFilterAttribute.Categories> property. At least one activity must be provided, which is why activities are provided in the constructor. An `[IntentFilter]` can be provided multiple times, and each use results in a separate `<intent-filter/>` element within the `<activity/>`:

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

The Android manifest file also provides a way for you to declare properties for your entire app. This is done via the `<application>` element and its counterpart, the <xref:Android.App.ApplicationAttribute> attribute. Typically, you declare `<application>` properties for your entire app and then override these properties as required on a per-Activity basis.

For example, the following `Application` attribute could be added to *MainApplication.cs* to indicate that the app can be debugged, that its user-readable name is "My Maui App", and that it uses the `Maui.SplashTheme` style as the default theme for all activities:

```csharp
using Android.App;
using Android.Runtime;

namespace MyMauiApp;

[Application(Debuggable = true, Label = "My Maui App", Theme = "@style/Maui.SplashTheme")]
public class MainApplication : MauiApplication
{
      public MainApplication(IntPtr handle, JniHandleOwnership ownership)
             : base(handle, ownership)
      {
      }

      protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

```

This declaration causes the following XML fragment to be generated in **obj/Debug/android/AndroidManifest.xml**:

```xml
<application android:label="MyMauiApp" android:debuggable="true" android:theme="@style/Maui.SplashTheme" ...>
```

In this example, all activities in the app will default to the `Maui.SplashTheme` style. If you set an activity's theme to `Maui.MyTheme`, only that activity will use the `Maui.MyTheme` style while any other activities in your app will default to the `Maui.SplashTheme` style that's set in the `<application>` element.

The <xref:Android.App.ApplicationAttribute> attribute is not the only way to configure `<application>` attributes. You can also insert properties directly into the `<application>` element of the Android manifest file. These properties are then merged into the generated Android manifest file. For more information, see the properties section of <xref:Android.App.ApplicationAttribute>.

> [!IMPORTANT]
> The contents of *Platforms\\Android\\AndroidManifest.xml* always override data provided by attributes.

## App icon

By default, your app will be given a .NET icon. For information about specifying a custom icon, see [Change a .NET MAUI app icon](~/user-interface/images/app-icons.md?&tabs=android#platform-specific-configuration).

## Attributes

The following table shows the .NET for Android attributes that generate Android manifest XML fragments:

| Attribute | Description |
| --------- | ----------- |
| <xref:Android.App.ActivityAttribute?displayProperty=fullName> | Generates a [activity](https://developer.android.com/guide/topics/manifest/activity-element.html) XML fragment. |
| <xref:Android.App.ApplicationAttribute?displayProperty=fullName> | Generates a [application](https://developer.android.com/guide/topics/manifest/application-element.html) XML fragment. |
| <xref:Android.App.InstrumentationAttribute?displayProperty=fullName> | Generates a [instrumentation](https://developer.android.com/guide/topics/manifest/instrumentation-element.html) XML fragment. |
| <xref:Android.App.IntentFilterAttribute?displayProperty=fullName> | Generates a [intent-filter](https://developer.android.com/guide/topics/manifest/intent-filter-element.html) XML fragment. |
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
