---
title: "Xamarin.Android project migration"
description: "Learn how to map properties and items in legacy Xamarin Android projects to .NET projects"
ms.date: 1/31/2023
---

# Xamarin.Android project migration

A .NET 7 project for a Xamarin.Android application looks something like:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net7.0-android</TargetFramework>
    <OutputType>Exe</OutputType>
  </PropertyGroup>
</Project>
```

For a "library" project, omit the `$(OutputType)` property completely or specify `Library`.

Additional resources:

* [Binding projects](https://github.com/xamarin/xamarin-android/blob/main/Documentation/guides/OneDotNetBindingProjects.md)

## .NET Configuration Files

No support for [configuration files](/dotnet/framework/configure-apps/) such as `Foo.dll.config` or `Foo.exe.config` is available in Xamarin.Android projects targeting .NET 7. [`<dllmap>`](https://github.com/dotnet/runtime/blob/main/docs/design/features/dllmap.md) configuration elements are not supported in .NET Core at all, and other element types for compatibility packages like [System.Configuration.ConfigurationManager](https://www.nuget.org/packages/System.Configuration.ConfigurationManager/) have never been supported in Xamarin.Android projects.

## Changes to MSBuild properties

`$(AndroidSupportedAbis)` should not be used. Instead of:

```xml
<PropertyGroup>
  <!-- Used in legacy Xamarin.Android projects -->
  <AndroidSupportedAbis>armeabi-v7a;arm64-v8a;x86;x86_64</AndroidSupportedAbis>
</PropertyGroup>
```

Use .NET's concept of runtime identifiers. For more information about runtime identifiers, see [.NET RID Catalog](/dotnet/core/rid-catalog).

```xml
<PropertyGroup>
  <!-- Used going forward in .NET -->
  <RuntimeIdentifiers>android-arm;android-arm64;android-x86;android-x64</RuntimeIdentifiers>
</PropertyGroup>
```

`$(AndroidUseIntermediateDesignerFile)` will be `True` by default.

`$(AndroidBoundExceptionType)` will be `System` by default. This will alter the types of exceptions thrown from various methods to better align with existing .NET 6 semantics, at the cost of compatibility with previous Xamarin.Android releases. For more information, see [Some of the new wrapped Java exceptions use BCL exceptions that differ from the related BCL types](https://github.com/xamarin/xamarin-android/issues/4127).

`$(AndroidClassParser)` will be `class-parse` by default. `jar2xml` will not be supported.

`$(AndroidDexTool)` will be `d8` by default. `dx` will not be supported.

`$(AndroidCodegenTarget)` will be `XAJavaInterop1` by default. `XamarinAndroid` will not be supported.

`$(AndroidManifest)` will default to `AndroidManifest.xml` in the root of projects as `Properties\AssemblyInfo.cs` is no longer used in short-form MSBuild projects. `Properties\AndroidManifest.xml` will also be detected and used if it exists to ease migration.

`$(DebugType)` will be `portable` by default. `full` and `pdbonly` will not be supported.

`$(MonoSymbolArchive)` will be `False`, since `mono-symbolicate` is not yet supported.

If Java binding is enabled with `@(InputJar)`, `@(EmbeddedJar)`, `@(LibraryProjectZip)`, etc. then `$(AllowUnsafeBlocks)` will default to `True`.

Referencing an Android Wear project from an Android application will not be supported:

```xml
<ProjectReference Include="..\Foo.Wear\Foo.Wear.csproj">
  <IsAppExtension>True</IsAppExtension>
</ProjectReference>
```

## Default file inclusion

Default Android related file globbing behavior is defined in `AutoImport.props`. This behavior can be disabled for Android items by setting `$(EnableDefaultAndroidItems)` to `false`, or all default item inclusion behavior can be disabled by setting `$(EnableDefaultItems)` to `false`. For more information, see [Workload props files](https://github.com/dotnet/designs/blob/4703666296f5e59964961464c25807c727282cae/accepted/2020/workloads/workload-resolvers.md#workload-props-files).

## Runtime behavior

There is some behavioral changes to the `String.IndexOf()` method in .NET 5 and higher on different platforms, see details [here](/dotnet/standard/globalization-localization/globalization-icu).

## Linker (ILLink)

.NET 5 and higher has new [settings for the linker](/dotnet/core/deploying/trimming-options):

- `<PublishTrimmed>true</PublishTrimmed>`
- `<TrimMode>link</TrimMode>` - Enable member-level trimming.

In Android application projects by default, `Debug` builds will not use the linker and `Release` builds will set `PublishTrimmed=true` and `TrimMode=link`. `TrimMode=copyused` is the default the dotnet/sdk, but it doesn't not seem to be appropriate for mobile applications. Developers can still opt into `TrimMode=copyused` if needed.

If the legacy `AndroidLinkMode` setting is used, both `SdkOnly` and `Full` will default to equivalent linker settings:

- `<PublishTrimmed>true</PublishTrimmed>`
- `<TrimMode>link</TrimMode>`

With `AndroidLinkMode=SdkOnly` only BCL and SDK assemblies marked with `%(Trimmable)` will be linked at the member level. `AndroidLinkMode=Full` will set `%(TrimMode)=link` on all .NET assemblies similar to the example in the [trimming documentation](/dotnet/core/deploying/trimming-options#trimmed-assemblies).

It is recommended to migrate to the new linker settings, as `AndroidLinkMode` will eventually be deprecated.

## AOT

`$(RunAOTCompilation)` will be the new MSBuild property for enabling AOT. This is the same property used for [Blazor WASM](/aspnet/core/blazor/host-and-deploy/webassembly/#ahead-of-time-aot-compilation). `$(AotAssemblies)` will also enable AOT, in order to help with migration from "legacy" Xamarin.Android to .NET 6.

It is recommended to migrate to the new `$(RunAOTCompilation)` property, as `$(AotAssemblies)` is deprecated in .NET 7.

We want to choose the optimal settings for startup time and app size. By default `Release` builds will default to:

```xml
<PropertyGroup Condition="'$(Configuration)' == 'Release'">
  <RunAOTCompilation>true</RunAOTCompilation>
  <AndroidEnableProfiledAot>true</AndroidEnableProfiledAot>
</PropertyGroup>
```

This is the behavior when `$(RunAOTCompilation)` and `$(AndroidEnableProfiledAot)` are blank.

So if you would like to *disable* AOT, you would need to explicitly turn these settings off:

```xml
<PropertyGroup Condition="'$(Configuration)' == 'Release'">
  <RunAOTCompilation>false</RunAOTCompilation>
  <AndroidEnableProfiledAot>false</AndroidEnableProfiledAot>
</PropertyGroup>
```

## dotnet cli

There are currently a few "verbs" we are aiming to get working in Xamarin.Android:

```
dotnet new
dotnet build
dotnet publish
dotnet run
```

### dotnet new

To support `dotnet new`, we created a few basic project and item templates for Android that are named following the patterns and naming of existing .NET templates:

Templates                                     Short Name           Language    Tags
--------------------------------------------  -------------------  ----------  ----------------------
Android Activity template                     android-activity     [C#]        Android
Android Java Library Binding                  android-bindinglib   [C#]        Android
Android Layout template                       android-layout       [C#]        Android
Android Class library                         androidlib           [C#]        Android
Android Application                           android              [C#]        Android
Console Application                           console              [C#],F#,VB  Common/Console
Class library                                 classlib             [C#],F#,VB  Common/Library
WPF Application                               wpf                  [C#],VB     Common/WPF
WPF Class library                             wpflib               [C#],VB     Common/WPF
NUnit 3 Test Project                          nunit                [C#],F#,VB  Test/NUnit
NUnit 3 Test Item                             nunit-test           [C#],F#,VB  Test/NUnit

To create different types of Android projects:

```console
dotnet new android            --output MyAndroidApp     --packageName com.mycompany.myandroidapp
dotnet new androidlib         --output MyAndroidLibrary
dotnet new android-bindinglib --output MyJavaBinding
```

Once the projects are created, some basic item templates can also be used such as:

```console
dotnet new android-activity --name LoginActivity --namespace MyAndroidApp
dotnet new android-layout   --name MyLayout      --output Resources/layout
```

### dotnet build & publish

Currently in .NET console apps, `dotnet publish` is where all the work to produce a self-contained "app" happens:

- The linker via the `<IlLink/>` MSBuild task
- .NET Core's version of AOT, named "ReadyToRun"

[Documentation](/dotnet/core/whats-new/dotnet-core-3-0#readytorun-images)

However, for Xamarin.Android, `dotnet build` should produce something that is runnable. This basically means we need to create an `.apk` or `.aab` file during `build`. We will need to reorder any MSBuild tasks from the dotnet SDK, so that they will run during `build`.

This means Xamarin.Android would run:

- Run `aapt` to generate `Resource.designer.cs` and potentially emit build errors for issues in `@(AndroidResource)` files.
- Compile C# code
- Run the new [ILLink](https://github.com/mono/linker/blob/master/src/linker/README.md) MSBuild target for linking.
- Generate java stubs, `AndroidManifest.xml`, etc. This must happen after the linker.
- Compile java code via `javac`
- Convert java code to `.dex` via d8/r8
- Create an `.apk` or `.aab` and sign it

`dotnet publish` will be reserved for publishing an app for Google Play, ad-hoc distribution, etc. It could be able to sign the `.apk` or `.aab` with different keys. As a starting point, this will currently copy the output to a `publish` directory on disk.

> [!NOTE]
> Behavior inside IDEs will differ. The `Build` target will not produce an `.apk` file if `$(BuildingInsideVisualStudio)` is `true`. IDEs will call the `Install` target for deployment, which will produce the `.apk` file. This behavior matches "legacy" Xamarin.Android._

### dotnet run

`dotnet run` can be used to launch applications on a device or emulator via the `--project` switch:

```console
dotnet run --project HelloAndroid.csproj
```

Alternatively, you could use the `Run` MSBuild target such as:

```console
dotnet build HelloAndroid.csproj -t:Run
```
