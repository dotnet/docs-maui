---
title: "Xamarin.Android project migration"
description: "Learn how to migrate a Xamarin.Android project to a .NET for Android project."
ms.date: 02/15/2023
---

# Xamarin.Android project migration

A .NET 8 project for a .NET for Android app is similar to the following example:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0-android</TargetFramework>
    <OutputType>Exe</OutputType>
  </PropertyGroup>
</Project>
```

For a library project, omit the `$(OutputType)` property completely or specify `Library` as the property value.

## .NET configuration files

There's no support for [configuration files](/dotnet/framework/configure-apps/) such as `Foo.dll.config` or `Foo.exe.config` in .NET for Android projects. [`<dllmap>`](https://github.com/dotnet/runtime/blob/main/docs/design/features/dllmap.md) configuration elements aren't supported in .NET Core at all, and other element types for compatibility packages like [System.Configuration.ConfigurationManager](https://www.nuget.org/packages/System.Configuration.ConfigurationManager/) have never been supported in Android projects.

## Changes to MSBuild properties

The `$(AndroidSupportedAbis)` property shouldn't be used:

```xml
<PropertyGroup>
  <!-- Used in Xamarin.Android projects -->
  <AndroidSupportedAbis>armeabi-v7a;arm64-v8a;x86;x86_64</AndroidSupportedAbis>
</PropertyGroup>
```

Instead, the `$(AndroidSupportedAbis)` property should be replaced with .NET runtime identifiers:

```xml
<PropertyGroup>
  <!-- Used in .NET for Android projects -->
  <RuntimeIdentifiers>android-arm;android-arm64;android-x86;android-x64</RuntimeIdentifiers>
</PropertyGroup>
```

For more information about runtime identifiers, see [.NET RID Catalog](/dotnet/core/rid-catalog).

The following table shows other MSBuild properties that have changed in .NET for Android:

| Property | Comments |
| --- | --- |
| `$(AndroidUseIntermediateDesignerFile)` | `True` by default. |
| `$(AndroidBoundExceptionType)` | `System` by default. This property alters the types of exceptions thrown from various methods to better align with .NET semantics, at the cost of compatibility with Xamarin.Android. For more information, see [Some of the new wrapped Java exceptions use BCL exceptions that differ from the related BCL types](https://github.com/xamarin/xamarin-android/issues/4127). |
| `$(AndroidClassParser)` | `class-parse` by default. `jar2xml` isn't supported. |
| `$(AndroidDexTool)` | `d8` by default. `dx` isn't supported. |
| `$(AndroidCodegenTarget)` | `XAJavaInterop1` by default. `XamarinAndroid` isn't supported. |
| `$(AndroidManifest)` | Defaults to `AndroidManifest.xml` in the root of projects because `Properties\AssemblyInfo.cs` is no longer used in SDK-style projects. `Properties\AndroidManifest.xml` will also be detected and used if it exists, to ease migration. |
| `$(DebugType)` | `portable` by default. `full` and `pdbonly` aren't supported. |
| `$(MonoSymbolArchive)` | `False`, since `mono-symbolicate` isn't supported. |

In addition, if Java binding is enabled with `@(InputJar)`, `@(EmbeddedJar)`, or `@(LibraryProjectZip)`, then the `$(AllowUnsafeBlocks)` property defaults to `True`.

> [!NOTE]
> Referencing an Android Wear project from an Android app isn't supported.

## Changes to AndroidManifest.xml

In Xamarin.Android, Java, and Kotlin Android projects, the `<uses-sdk/>` element denotes the minimum Android version your app supports, as well as the target Android version your app is compiled against:

```xml
﻿<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android"
    android:versionCode="1"
    android:versionName="1.0"
    package="com.companyname.myapp">
  <uses-sdk android:minSdkVersion="21" android:targetSdkVersion="33" />
  <application android:icon="@mipmap/ic_launcher" android:label="@string/app_name" android:theme="@style/AppTheme" />
</manifest>
```

For more information about the `<uses-sdk/>` element, see the [Android documentation](https://developer.android.com/guide/topics/manifest/uses-sdk-element).

In .NET 8 Android apps, there are MSBuild properties to set these values. Using the MSBuild properties has other benefits. In most cases the `<uses-sdk/>` element should be removed in favor of values in your project's `.csproj` file:

```xml
<Project>
  <PropertyGroup>
    <TargetFramework>net8.0-android</TargetFramework>
    <SupportedOSPlatformVersion>21</SupportedOSPlatformVersion>
  </PropertyGroup>
</Project>
```

In this example, `net8.0-android` is shorthand for `net8.0-android34.0`. Future versions of .NET will track the latest Android version available at the time of the .NET release.

`TargetFramework` maps to `android:targetSdkVersion`. At build time, this value will automatically be included in the `<uses-sdk/>` element for you. The benefit of using `TargetFramework` in this way is that you're given the matching C# binding for Android API 34 for `net8.0-android34.0`. Android releases independently of the .NET release cycle, so we have the flexibility to opt into `net8.0-android35.0` when a binding is available for the next Android release.

Similarly, `SupportedOSPlatformVersion` maps to `android:minSdkVersion`. At build time, this value will automatically be included in the `<uses-sdk/>` element for you. Android APIs are decorated with the <xref:System.Runtime.Versioning.SupportedOSPlatformAttribute> so that you get build warnings for calling APIs that are only available for some of the Android versions your app can run on:

```
error CA1416: This call site is reachable on 'Android' 21.0 and later. `ConnectivityManager.ActiveNetwork` is only supported on: 'Android' 23.0 and later.
```

To safely use this API, you can declare a higher `SupportedOSPlatformVersion` in your project or use the <xref:System.OperatingSystem.IsAndroidVersionAtLeast%2A> API at runtime:

```csharp
if (OperatingSystem.IsAndroidVersionAtLeast(23))
{
    // Use the API here
}
```

## Default file inclusion

Default .NET for Android related file globbing behavior is defined in `AutoImport.props`. This behavior can be disabled for Android items by setting `$(EnableDefaultAndroidItems)` to `false`, or all default item inclusion behavior can be disabled by setting `$(EnableDefaultItems)` to `false`. For more information, see [Workload props files](https://github.com/dotnet/designs/blob/4703666296f5e59964961464c25807c727282cae/accepted/2020/workloads/workload-resolvers.md#workload-props-files).

## Runtime behavior

There are behavioral changes to the `String.IndexOf()` method in .NET 5+ on different platforms. For more information, see [.NET globalization and ICU](/dotnet/standard/globalization-localization/globalization-icu).

## Linker

::: moniker range="=net-maui-8.0"

.NET 8 has new settings for the linker:

- `<PublishTrimmed>true</PublishTrimmed>`
- `<TrimMode>partial</TrimMode>`, which trims assemblies that have opted-in to trimming.

For more information, see [Trimming options](/dotnet/core/deploying/trimming-options).

In .NET for Android projects by default, `Debug` builds don't use the linker, and `Release` builds set `PublishTrimmed=true` and `TrimMode=partial`.

If the legacy `AndroidLinkMode` setting is used, both `SdkOnly` and `Full` default to equivalent linker settings:

- `<PublishTrimmed>true</PublishTrimmed>`
- `<TrimMode>partial</TrimMode>`

With `AndroidLinkMode=SdkOnly`, only BCL and SDK assemblies marked with `%(Trimmable)` are linked at the member level. `AndroidLinkMode=Full` sets `%(TrimMode)=partial` on all .NET assemblies.

> [!TIP]
> You should migrate to the new linker settings, because the `AndroidLinkMode` setting will eventually be deprecated.

::: moniker-end

::: moniker range=">=net-maui-9.0"

.NET 9 has new settings for the linker:

- `<TrimMode>Full</TrimMode>`, which performs full trimming.

For more information, see [Trimming options](/dotnet/core/deploying/trimming-options).

In .NET for Android projects by default, `Debug` builds don't perform any trimming, and `Release` builds set `PublishTrimmed=True` and `TrimMode=Full`.

::: moniker-end

## Ahead-of-Time compilation

`$(RunAOTCompilation)` is the new MSBuild property for enabling Ahead-of-Time (AoT) compilation. This is the same property used for [Blazor WASM](/aspnet/core/blazor/host-and-deploy/webassembly/#ahead-of-time-aot-compilation). The `$(AotAssemblies)` property also enables AOT, in order to help with migration from Xamarin.Android projects to .NET for Android projects. However, this property was deprecated in .NET 7.

Release builds default to the following AOT property values:

```xml
<PropertyGroup Condition="'$(Configuration)' == 'Release'">
  <RunAOTCompilation>true</RunAOTCompilation>
  <AndroidEnableProfiledAot>true</AndroidEnableProfiledAot>
</PropertyGroup>
```

This is the behavior when the `$(RunAOTCompilation)` and `$(AndroidEnableProfiledAot)` properties are unset, and chooses the optimal settings for startup time and app size.

To disable AOT, you need to explicitly set the `$(RunAOTCompilation)` and `$(AndroidEnableProfiledAot)` properties to `false`:

```xml
<PropertyGroup Condition="'$(Configuration)' == 'Release'">
  <RunAOTCompilation>false</RunAOTCompilation>
  <AndroidEnableProfiledAot>false</AndroidEnableProfiledAot>
</PropertyGroup>
```

## Supported encodings

If your Xamarin.Android app uses certain international codesets, they have to be specified explicitly in your project file using the `Mandroidl18n` MSBuild property, so that the linker can include supporting resources. For more information about this build property, see [MAndroidl18n](/xamarin/android/deploy-test/building-apps/build-properties#mandroidi18n).

However, the `Mandroidl18n` MSBuild property isn't supported in .NET for Android apps. Instead, support is provided by the [System.TextEncoding.CodePages](https://www.nuget.org/packages/System.Text.Encoding.CodePages/) NuGet package. For more information, see <xref:System.Text.CodePagesEncodingProvider>.

## .NET CLI

.NET for Android supports using .NET command-line interface (.NET CLI) to create, build, publish, and run Android apps.

### dotnet new

`dotnet new` can be used to create new .NET for Android projects and items using project templates and item templates that are named following the patterns and naming of existing .NET templates:

| Template | Short Name | Language | Tags |
| -------- | ---------- | -------- | ---- |
| Android Activity template | android-activity | C# | Android |
| Android Java Library Binding | android-bindinglib | C# | Android |
| Android Layout template | android-layout | C# | Android |
| Android Class library | androidlib | C# | Android |
| Android Application | android | C# | Android |

The following examples show using `dotnet new` to create different types of .NET for Android projects:

```dotnetcli
dotnet new android            --output MyAndroidApp     --packageName com.mycompany.myandroidapp
dotnet new androidlib         --output MyAndroidLibrary
dotnet new android-bindinglib --output MyJavaBinding
```

Once .NET for Android projects have been created, item templates can be used to add items to the projects:

```dotnetcli
dotnet new android-activity --name LoginActivity --namespace MyAndroidApp
dotnet new android-layout   --name MyLayout      --output Resources/layout
```

### dotnet build & publish

For .NET for Android, `dotnet build` produces a runnable app. This means creating an `.apk` or `.aab` file during the build process, and reordering MSBuild tasks from the .NET SDK so that they run during the build. Therefore, .NET for Android does the following during a build:

- Run `aapt` to generate `Resource.designer.cs` and potentially emit build errors for issues in `@(AndroidResource)` files.
- Compile C# code.
- Run the [ILLink](https://github.com/mono/linker/blob/master/src/linker/README.md) MSBuild target for linking.
- Generate java stubs, and `AndroidManifest.xml`.
- Compile java code via `javac`.
- Convert java code to `.dex` via d8/r8.
- Create an `.apk` or `.aab` and sign it.

`dotnet publish` is reserved for publishing an app for Google Play and other distribution mechanisms such as ad-hoc. It also signs the `.apk` or `.aab` with different keys.

> [!NOTE]
> Behavior inside IDEs will differ. The `Build` target will not produce an `.apk` file if `$(BuildingInsideVisualStudio)` is `true`. IDEs will call the `Install` target for deployment, which will produce the `.apk` file. This behavior matches Xamarin.Android.

### dotnet run

`dotnet run` can be used to launch apps on a device or emulator via the `--project` argument:

```dotnetcli
dotnet run --project HelloAndroid.csproj
```

Alternatively, you could use the `Run` MSBuild target:

```dotnetcli
dotnet build HelloAndroid.csproj -t:Run
```

## See also

- [Binding projects](https://github.com/xamarin/xamarin-android/blob/main/Documentation/guides/OneDotNetBindingProjects.md)
