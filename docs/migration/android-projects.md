---
title: "Xamarin.Android project migration"
description: "Learn how to map properties and items in legacy Xamarin Android projects to .NET projects"
ms.date: 1/31/2023
---

# Xamarin.Android project migration

A .NET 7 project for a Xamarin.Android app is similar to the following example:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net7.0-android</TargetFramework>
    <OutputType>Exe</OutputType>
  </PropertyGroup>
</Project>
```

For a library project, omit the `$(OutputType)` property completely or specify `Library`.

## .NET configuration files

No support for [configuration files](/dotnet/framework/configure-apps/) such as `Foo.dll.config` or `Foo.exe.config` is available in Xamarin.Android projects targeting .NET 7. [`<dllmap>`](https://github.com/dotnet/runtime/blob/main/docs/design/features/dllmap.md) configuration elements are not supported in .NET Core at all, and other element types for compatibility packages like [System.Configuration.ConfigurationManager](https://www.nuget.org/packages/System.Configuration.ConfigurationManager/) have never been supported in Xamarin.Android projects.

## Changes to MSBuild properties

`$(AndroidSupportedAbis)` should not be used:

```xml
<PropertyGroup>
  <!-- Used in legacy Xamarin.Android projects -->
  <AndroidSupportedAbis>armeabi-v7a;arm64-v8a;x86;x86_64</AndroidSupportedAbis>
</PropertyGroup>
```

Instead, `$(AndroidSupportedAbis)` should be replaced with .NET runtime identifiers:

```xml
<PropertyGroup>
  <!-- Used going forward in .NET -->
  <RuntimeIdentifiers>android-arm;android-arm64;android-x86;android-x64</RuntimeIdentifiers>
</PropertyGroup>
```

For more information about runtime identifiers, see [.NET RID Catalog](/dotnet/core/rid-catalog).

`$(AndroidUseIntermediateDesignerFile)` will be `True` by default.

`$(AndroidBoundExceptionType)` will be `System` by default. This will alter the types of exceptions thrown from various methods to better align with existing .NET 6+ semantics, at the cost of compatibility with previous Xamarin.Android releases. For more information, see [Some of the new wrapped Java exceptions use BCL exceptions that differ from the related BCL types](https://github.com/xamarin/xamarin-android/issues/4127).

`$(AndroidClassParser)` will be `class-parse` by default. `jar2xml` will not be supported.

`$(AndroidDexTool)` will be `d8` by default. `dx` will not be supported.

`$(AndroidCodegenTarget)` will be `XAJavaInterop1` by default. `XamarinAndroid` will not be supported.

`$(AndroidManifest)` will default to `AndroidManifest.xml` in the root of projects as `Properties\AssemblyInfo.cs` is no longer used in short-form MSBuild projects. `Properties\AndroidManifest.xml` will also be detected and used if it exists to ease migration.

`$(DebugType)` will be `portable` by default. `full` and `pdbonly` will not be supported.

`$(MonoSymbolArchive)` will be `False`, since `mono-symbolicate` is not yet supported.

If Java binding is enabled with `@(InputJar)`, `@(EmbeddedJar)`, or `@(LibraryProjectZip)`, then `$(AllowUnsafeBlocks)` will default to `True`.

Referencing an Android Wear project from an Android app isn't supported.

## Default file inclusion

Default Android related file globbing behavior is defined in `AutoImport.props`. This behavior can be disabled for Android items by setting `$(EnableDefaultAndroidItems)` to `false`, or all default item inclusion behavior can be disabled by setting `$(EnableDefaultItems)` to `false`. For more information, see [Workload props files](https://github.com/dotnet/designs/blob/4703666296f5e59964961464c25807c727282cae/accepted/2020/workloads/workload-resolvers.md#workload-props-files).

## Runtime behavior

There are some behavioral changes to the `String.IndexOf()` method in .NET 5+ on different platforms. For more information, see [.NET globalization and ICU](/dotnet/standard/globalization-localization/globalization-icu).

## Linker (ILLink)

.NET 5+ has new settings for the linker:

- `<PublishTrimmed>true</PublishTrimmed>`
- `<TrimMode>link</TrimMode>` - Enable member-level trimming.

For more information, see [Trimming options](/dotnet/core/deploying/trimming-options).

In Android app projects by default, `Debug` builds will not use the linker and `Release` builds will set `PublishTrimmed=true` and `TrimMode=link`. `TrimMode=copyused` is the default for the .NET SDK but doesn't seem to be appropriate for mobile apps. However, you can still opt into `TrimMode=copyused` if required.

If the legacy `AndroidLinkMode` setting is used, both `SdkOnly` and `Full` will default to equivalent linker settings:

- `<PublishTrimmed>true</PublishTrimmed>`
- `<TrimMode>link</TrimMode>`

With `AndroidLinkMode=SdkOnly` only BCL and SDK assemblies marked with `%(Trimmable)` will be linked at the member level. `AndroidLinkMode=Full` will set `%(TrimMode)=link` on all .NET assemblies.

> [!IMPORTANT]
> It's recommended to migrate to the new linker settings, as `AndroidLinkMode` will eventually be deprecated.

## AOT

`$(RunAOTCompilation)` is the new MSBuild property for enabling AOT. This is the same property used for [Blazor WASM](/aspnet/core/blazor/host-and-deploy/webassembly/#ahead-of-time-aot-compilation). `$(AotAssemblies)` also enables AOT, in order to help with migration from legacy Xamarin.Android to .NET 6+.

> [!IMPORTANT]
> Its recommended to migrate to the new `$(RunAOTCompilation)` property, as `$(AotAssemblies)` is deprecated in .NET 7.

Release builds will default to:

```xml
<PropertyGroup Condition="'$(Configuration)' == 'Release'">
  <RunAOTCompilation>true</RunAOTCompilation>
  <AndroidEnableProfiledAot>true</AndroidEnableProfiledAot>
</PropertyGroup>
```

This is the behavior when `$(RunAOTCompilation)` and `$(AndroidEnableProfiledAot)` are blank, and chooses the optimal settings for startup time and app size.

If you would like to disable AOT, you would need to explicitly turn off the following settings:

```xml
<PropertyGroup Condition="'$(Configuration)' == 'Release'">
  <RunAOTCompilation>false</RunAOTCompilation>
  <AndroidEnableProfiledAot>false</AndroidEnableProfiledAot>
</PropertyGroup>
```

## .NET CLI

.NET for Android supports using .NET CLI to create, build, publish, and run Android apps.

### dotnet new

`dotnet new` can be used create new Android projects and items using project templates and item templates for Android that are named following the patterns and naming of existing .NET templates:

| Template | Short Name | Language | Tags |
| -------- | ---------- | -------- | ---- |
| Android Activity template | android-activity | [C#] | Android |
| Android Java Library Binding | android-bindinglib | [C#] | Android |
| Android Layout template | android-layout | [C#] | Android |
| Android Class library | androidlib | [C#] | Android |
| Android Application | android | [C#] | Android |

The following examples show using `dotnet new` to create different types of Android projects:

```console
dotnet new android            --output MyAndroidApp     --packageName com.mycompany.myandroidapp
dotnet new androidlib         --output MyAndroidLibrary
dotnet new android-bindinglib --output MyJavaBinding
```

Once the projects are created, some basic item templates can also be used to add items to the projects:

```console
dotnet new android-activity --name LoginActivity --namespace MyAndroidApp
dotnet new android-layout   --name MyLayout      --output Resources/layout
```

### dotnet build & publish

For .NET for Android, `dotnet build` produces a runnable app. This means creating an `.apk` or `.aab` file during `build`, and reordering MSBuild tasks from the .NET SDK so that they run during `build`. This means .NET for Android does the following:

- Run `aapt` to generate `Resource.designer.cs` and potentially emit build errors for issues in `@(AndroidResource)` files.
- Compile C# code.
- Run the [ILLink](https://github.com/mono/linker/blob/master/src/linker/README.md) MSBuild target for linking.
- Generate java stubs, and `AndroidManifest.xml`. This must happen after the linker.
- Compile java code via `javac`.
- Convert java code to `.dex` via d8/r8.
- Create an `.apk` or `.aab` and sign it.

`dotnet publish` is reserved for publishing an app for Google Play and other distribution mechanisms such as ad-hoc. It also signs the `.apk` or `.aab` with different keys.

> [!NOTE]
> Behavior inside IDEs will differ. The `Build` target will not produce an `.apk` file if `$(BuildingInsideVisualStudio)` is `true`. IDEs will call the `Install` target for deployment, which will produce the `.apk` file. This behavior matches Xamarin.Android.

### dotnet run

`dotnet run` can be used to launch apps on a device or emulator via the `--project` switch:

```console
dotnet run --project HelloAndroid.csproj
```

Alternatively, you could use the `Run` MSBuild target:

```console
dotnet build HelloAndroid.csproj -t:Run
```

## See also

- [Binding projects](https://github.com/xamarin/xamarin-android/blob/main/Documentation/guides/OneDotNetBindingProjects.md)
