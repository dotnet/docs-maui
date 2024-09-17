---
title: "Project property settings in Visual Studio"
description: "Describes the Visual Studio project properties available to a .NET MAUI app. The properties and settings are related to building the app, configuring debug options, and other settings related to building the application for Windows, Android, and iOS."
ms.date: 09/17/2024
---

# Project configuration for .NET MAUI apps

.NET MAUI uses a [single-project system](../fundamentals/single-project.md) to manage the configuration of your cross-platform app. Project configuration in .NET MAUI is similar to other projects in Visual Studio, right-click on the project in the **Solution Explorer**, and select **Properties**.

## Application

The **Application** section describes some settings related to which platforms your app targets, as well as the output file and default namespace.

- **General**

  Describes some basic settings about your app.

  | Setting | Default value | Description |
  | - | - | - |
  | Assembly name | `$(MSBuildProjectName)` | Specifies the name of the output file that will hold the assembly manifest. |
  | Default namespace | Varies. | Specifies the base namespace for files added to your project. This generally defaults to the name of your project or a value you specified when you created the project. |

- **iOS Targets**

  If you're going to target iOS and macOS (using Mac Catalyst), these settings describe the target iOS version.

  | Setting | Default value | Description |
  | - | - | - |
  | Target the iOS platform | Checked | Specifies that this project will target the iOS platform. |
  | Target iOS Framework | `net8.0-ios` | The [Target Framework Moniker][tfm] used to target iOS. |
  | Minimum Target iOS Framework | `14.2` | The minimum version of iOS your app targets. |

- **Android Targets**

  If you're going to target Android, these settings describe the target Android version.

  | Setting | Default value | Description |
  | - | - | - |
  | Target the Android platform | Checked | When checked, the .NET MAUI project will target and build an Android version of your app. Uncheck to disable the Android target. |
  | Target Android Framework | `net8.0-android` | The [Target Framework Moniker][tfm] used to target Android. |
  | Minimum Target Android Framework | `21.0` | The minimum version of Android your app targets. |

- **Windows Targets**

  If you're going to target Windows, these settings describe the target Windows version.

  | Setting | Default value | Description |
  | - | - | - |
  | Target the Windows platform | Checked | When checked, the .NET MAUI project will target and build a Windows version of your app. Uncheck to disable the Windows target. |
  | Target Windows Framework | `net8.0-windows10.0.19041.0` | The [Target Framework Moniker][tfm] used to target Windows. |
  | Minimum Target Windows Framework | `10.0.17763.0` | The minimum version of Windows your app targets. |

## Build

The **Build** section describes settings related to compiling your app.

### General

Settings related to target platforms.

- **Conditional compilation symbols**

  Specifies symbols on which to perform conditional compilation. Separate symbols with a semicolon `;`. Symbols can be broken up into target platforms. For more information, see [Conditional compilation](/dotnet/csharp/language-reference/preprocessor-directives#conditional-compilation).

- **Platform target**

  Specifies the processor to be targeted by the output file. Choose `Any CPU` to specify that any processor is acceptable, allowing the application to run on the broadest range of hardware.

  Typically this is set to `Any CPU` and the runtime identifier setting is used to target a CPU platform.

  | Option | Description |
  | ------ | ----------- |
  | `Any CPU` | (Default) Compiles your assembly to run on any platform. Your application runs as a 64-bit process whenever possible and falls back to 32-bit when only that mode is available. |
  | `x86` | Compiles your assembly to be run by the 32-bit, x86-compatible runtime. |
  | `x64` | Compiles your assembly to be run by the 64-bit runtime on a computer that supports the AMD64 or EM64T instruction set. |
  | `ARM32` | Compiles your assembly to run on a computer that has an Advanced RISC Machine (ARM) processor. |
  | `ARM64` | Compiles your assembly to run by the 64-bit runtime on a computer that has an Advanced RISC Machine (ARM) processor that supports the A64 instruction set. |

- **Nullable**

  Specifies the project-wide C# nullable context. For more information, see [Nullable References](/dotnet/csharp/nullable-references#nullable-contexts).

  | Option        | Description |
  | ------------- | ----------- |
  | Unset         | (Default) If this setting isn't set, the default is `Disable`. |
  | `Disable`     | Nullable warnings are disabled. All reference type variables are nullable reference types. |
  | `Enable`      | The compiler enables all null reference analysis and all language features. |
  | `Warnings`    | The compiler performs all null analysis and emits warnings when code might dereference null. |
  | `Annotations` | The compiler doesn't perform null analysis or emit warnings when code might dereference null. |

- **Implicit global usings**

  Enables implicit global usings to be declared by the project SDK. This is enabled by default and imports many of the .NET MAUI namespaces automatically to all code files. Code files don't need to add `using` statements for common .NET MAUI namespaces. For more information, see [MSBuild properties - ImplicitUsings](/dotnet/core/project-sdk/msbuild-props#implicitusings).

- **Unsafe code**

  Allow code that uses the `unsafe` keyword to compile. This is disabled by default.

- **Optimize code**

  Enable compiler optimizations for smaller, faster, and more efficient output. There is an option for each target platform, in Debug or Release mode. Generally, this is enabled for Release mode, as the code is optimized for speed at the expense of helpful debugging information.

- **Debug symbols**

  Specifies the kind of debug symbols produced during build.

### Errors and warnings

Settings related to how errors and warnings are treated and reported during compilation.

- **Warning level**

  Specifies the level to display for compiler warnings.

- **Suppress specific warnings**

  Blocks the compiler from generating the specified warnings. Separate multiple warning numbers with a comma `,` or a semicolon `;`.

- **Treat warnings as errors**

  When enabled, instructs the compiler to treat warnings as errors. This is disabled by default.

- **Treat specific warnings as errors**

  Specifies which warnings are treated as errors. Separate multiple warning numbers with a comma `,` or a semicolon `;`.

### Output

Settings related to generating the output file.

- **Base output path**

  Specifies the base location for the project's output during build. Subfolders will be appended to this path to differentiate project configuration.

  Defaults to `.\bin\`.

- **Base intermediate output path**

  Specifies the base location for the project's intermediate output during build. Subfolders will be appended to the path to differentiate project configuration.

  Defaults to `.\obj\`.

- **Reference assembly**

  When enabled, produces a reference assembly containing the public API of the project. This is disabled by default.

- **Documentation file**

  When enabled, generates a file containing API documentation. This is disabled by default.

### Events

In this section you can add commands that run during the build.

- **Pre-build event**

  Specifies commands that run before the build starts. Does not run if the project is up-to-date. A non-zero exit code will fail the build before it runs.

- **Post-build event**

  Specifies commands that run before the build starts. Does not run if the project is up-to-date. A non-zero exit code will fail the build before it runs.

- **When to run the post-build event**

  Specifies under which condition the post-build even will be run.

### Strong naming

Settings related to signing the assembly.

- **Sign the assembly**

  When enabled, signs the output assembly to give it a strong name.

### Advanced

Additional settings related to the build.

- **Language version**

  The version of the language available to the code in the project. Defaults to `10.0`.

- **Check for arithmetic overflow**

  Throw exceptions when integer arithmetic produces out of range values. This setting is available for each platform. The default is disabled for each platform.

- **Deterministic**

  Produce identical compilation output for identical inputs. This setting is available for each platform. The default is enabled for each platform.

- **Internal compiler error reporting**

  Send internal compiler error reports to Microsoft. Defaults to `Prompt before sending`.

- **File alignment**

  Specifies, in bytes, where to align the sections of the output file. This setting is available for each platform. The default is `512` for each platform.

## Package

The **Package** section describes settings related to generating a NuGet package.

### General

Settings related to generating a NuGet package.

- **Generate NuGet package on build**

  When enabled, produces a NuGet package file during build operations. This is disabled by default.

- **Package ID**

  The case-insensitive package identifier, which must be unique across the NuGet package gallery, such as nuget.org. IDs may not contain spaces or characters that aren't valid for a URL, and generally follow .NET namespace rules.

  Defaults to the MSBuild value of `$(AssemblyName)`.

- **Title**

  A human-friendly title of the package, typically used in UI displays as on nuget.org and the Package Manager in Visual Studio.

- **Package Version**

  The version of the package, following the `major.minor.patch` pattern. Version numbers may include a pre-release suffix.

  Defaults to the MSBuild value of `$(ApplicationDisplayVersion)`.

- **Authors**

  A comma-separated list of authors, matching the profile names on nuget.org. These are displayed in the NuGet Gallery on nuget.org and are used to cross-reference packages by the same authors.

  Defaults to the MSBuild value of `$(AssemblyName)`.

- **Company**

  The name of the company associated with the NuGet package.

  Defaults to the MSBuild value of `$(Authors)`.

- **Product**

  The name of the product associated with the NuGet package.

  Defaults to the MSBuild value of `$(AssemblyName)`.

- **Description**

  A description of the package for UI display.

- **Copyright**

  Copyright details for the package.

- **Project URL**

  A URL for the package's home page, often shown in UI displays as well as nuget.org.

- **Icon**

  The icon image for the package. Image file size is limited to 1 MB. Supported file formats include JPEG and PNG. An image resolution of 128x128 is recommended.

- **README**

  The README document for the package. Must be a Markdown (.md) file.

- **Repository URL**

  Specifies the URL for the repository where the source code for the package resides and/or from which it's being built. For linking to the project page, use the 'Project URL' field, instead.

- **Repository type**

  Specifies the type of the repository. Default is 'git'.

- **Tags**

  A semicolon-delimited list of tags and keywords that describe the package and aid discoverability of the packages through search and filtering.

- **Release notes**

  A description of the changes made in the release of the package, often used in UI like the Updates tab of the Visual Studio Package Manager in place of the package description.

- **Pack as a .NET tool**

  When enabled, packs the project as a special package that contains a console application that may be installed via the "dotnet tool" command. This is disabled by default.

- **Package Output Path**

  Determines the output path in which the package will be dropped.

  Defaults to the MSBuild value of `$(OutputPath)`.

- **Assembly neutral language**

  Which language code is considered the neutral language. Defaults to unset.

- **Assembly version**

  The version of the assembly, defaults to `1.0.0.0` if not set.

- **File version**

  The version associated with the file, defaults to `1.0.0.0` if not set.

### License

- **Package License**

  Specify a license fo the project's package. Defaults to `None`.

- **Symbols**

  - **Produce a symbol package**

    When enabled, creates an additional symbol package when the project is packaged. This is disabled by default.

## Code Analysis

Settings related to code analysis.

### All analyzers

Settings related to when analysis runs.

- **Run on build**

  When enabled, runs code analysis on build. Defaults to enabled.

- **Run on live analysis**

  When enabled, runs code analysis live in the editor as you type. Defaults to enabled.

### .NET analysis

Settings related to .NET analyzers.

- **Enforce code style on build (experimental)**

  When enabled, produces diagnostics about code style on build. This is disabled by default.

- **Enable .NET analyzers**

  When enabled, runs .NET analyzers to help with API usage. Defaults to enabled.

- **Analysis level**

  The set of analyzers that should be run in the project. Defaults to `Latest`. For more information, see [MSBuild: AnalysisLevel](/dotnet/core/project-sdk/msbuild-props#analysislevel).

## MAUI Shared

These are project settings for .NET MAUI that are shared across all target platforms.

### General

General settings related to .NET MAUI.

- **Application Title**

  The display name of the application.

- **Application ID**

  The identifier of the application in reverse domain name format, for example: `com.microsoft.maui`.

- **Application ID (GUID)**

  The identifier of the application in GUID format.

- **Application Display Version**

  The display version of the application. This should be at most a three part version number such as 1.0.0.

- **Application Version**

  The version of the application. This should be a single digit integer such as 1.

## Android

These are Android-specific .NET MAUI settings.

### Manifest

Settings related to the Android manifest.

- **Application name**

  The string that's displayed as the name of the application. This is the name that's shown in the app's title bar. If not set, the label of the app's MainActivity is used as the application name. The default setting is `@string/app_name`, which refers to the string resource `app_name` location in `Resources/values/Strings.xaml`.

- **Application package name**

  A string that's used to uniquely identify the application. Typically, the package name is based on a reversed internet domain name convention, such as `com.company.appname`.

- **Application icon**

  Specifies the application icon resource that will be displayed for the app. The setting `@drawable/icon` refers to the image file `icon.png` located in the `Resources/mipmap` folder.

- **Application theme**

  Sets the UI style that's applied to the entire app. Every view in the app applies to the style attributes that are defined in the selected theme.

- **Application version number**

  An integer value greater than zero that defines the version number of the app. Higher numbers indicate more recent versions. This value is evaluated programmatically by Android and by other apps, it isn't shown to users.

- **Application version name**

  A string that specifies the version of the app to users. The version name can be a raw string or a reference to a string resource.

- **Install location**

  Indicates a preference as to where the app should be stored, whether in internal or external storage.

  | Option | Description |
  | -- | -- |
  | `Internal-only` | (Default) Specifies that the app can't be installed or moved to external storage. |
  | `Prefer external` | Specifies that the app should be installed in external storage, if possible. |
  | `Prefer internal` | Specifies that the app should be installed in internal storage, if possible. |

- **Minimum Android version**

  The oldest API level of an Android device that can install and run the app. Also referred to as `minSdkVersion`.

- **Target Android version**

  The target API level of the Android device where the app expects to run. This API level is used at run-time, unlike Target Framework, which is used at build time. Android uses this version as a way to provide forward compatibility. Also referred to as `targetSdkVersion`, this should match Target Framework `compileSdkVersion`.

### Options

Miscellaneous options for building an Android app.

- **Android package format**

  Either `apk` or `bundle`, which packages the Android application as an APK file or Android App Bundle, respectively. This can be set individually for both Debug and Release modes.

  App Bundles are the latest format for Android release builds that are intended for submission on Google Play.

  The default value is `apk`.

  When `bundle` is selected, other MSBuild properties are set:

  - `AndroidUseAapt2` is set to `True`.
  - `AndroidUseApkSigner` is set to `False`.
  - `AndroidCreatePackagePerAbi` is set to `False`.

- **Fast deployment (debug mode only)**

  When enabled, deploys the app faster than normal to the target device. This process speeds up the build/deploy/debug cycle because the package isn't reinstalled when only assemblies are changed. Only the updated assemblies are resynchronized to the target device.

  This is enabled by default.

- **Generate per ABI**

  When enabled, generates one Android package (apk) per selected Application Binary Interface (ABI). This is disabled by default.

- **Use incremental packaging**

  When enabled, uses the incremental Android packaging system (aapt2). This is enabled by default.

- **Multi-dex**

  When enabled, allows the Android build system to use multidex. The default is disabled.

- **Code shrinker**

  Selects the code shrinker to use.

  - `ProGuard` (default) is considered the legacy code shrinker.
  - `r8` is the next-generation tool which converts Java byte code to optimized dex code.

- **Uncompressed resources**

  Leaves the specified resource extensions uncompressed. Separate extensions with a semicolon `;`. For example: `.mp3;.dll;.png`.

- **Developer instrumentation**

  When enabled, developer instrumentation is provided for debugging and profiling. This can be set for individually for both Debug and Release modes.

  The default is enabled for Debug builds.

- **Debugger**

  Selects which debugger to use. The default is `.NET (Xamarin)`, which is used for managed code. The C++ debugger can be selected to debug native libraries used by the app.

- **AOT**

  Enables Ahead-of-Time (AOT) compilation. This can be set for individually for both Debug and Release modes.

  The default is enabled for Release builds.

- **LLVM**

  Enables the LLVM optimizing compiler. The default is disabled.

- **Startup Tracing**

  Enables startup tracing. This can be set for individually for both Debug and Release modes.

  The default is enabled for Release builds.

- **Garbage Collection**

  When enabled, uses the concurrent garbage collector. Defaults to enabled.

- **Enable trimming**

  When enabled, trims the application during publishing. This can be set for individually for both Debug and Release modes. For more information, see [Trim self-contained deployments and executables](/dotnet/core/deploying/trimming/trim-self-contained) and [Trim options](/dotnet/core/deploying/trimming/trimming-options).

  The default is enabled for Release builds.

- **Trimming granularity**

  Controls how aggressively IL is discarded. There are two modes to select from:

  - `Link` enables member-level trimming, which removes unused members from types.
  - `CopyUsed` (default) enableds assembly-level trimming, which keeps an entire assembly if any part of it is used.

- **Java max heap size**

  Set this value to increase the size of memory that an app can use. For example, a value of `2G` increases the heap size to 2 gigabytes. Note that there isn't a guarantee of how large the heap will be, and requesting too much heap memory may force other apps to terminate prematurely.

  The default is `1G`.

- **Additional Java options**

  Specifies additional command-line options to pass to the Java compiler when building a _.dex_ file. From the command line, you can type `java -help` to see the available options.

### Package Signing

When enabled, signs the _.APK_ file using the keystore details. This is disabled by default.

## iOS

These are iOS-specific .NET MAUI settings.

### Build

Settings related to building the iOS app.

- **Linker behavior**

  The linker can strip out unused methods, properties, fields, events, structs, and even classes in order to reduce the overall size of the application. You can add a `Preserve` attribute to any of these in order to prevent the linker from stripping it out if it's needed for serialization or reflection.

  > [!WARNING]
  > Enabling this feature may hinder debugging, as it may strip out property accessors that would allow you to inspect the state of your objects.

  Options are:

  - `Don't link`
  - `Link Framework SDKs only` (default)
  - `Link All`

- **LLVM**

  When enabled, uses the LLVM optimized compiler. This can be set for individually for both Debug and Release modes.

  The default is enabled for Release builds.

- **Float operations**

  Performs all 32-bit float operations as 64-bit float operations.

- **Symbols**

  When enabled, strips native debugging symbols from the output. This is enabled by default.

- **Garbage collector**

  When enabled, uses the concurrent garbage collector. This is disabled by default.

- **Additional arguments**

  Additional command line arguments to be passed to the application bundling code.

- **Optimization**

  When enabled, optimizes _.PNG_ images. This is enabled by default.

### Bundle Signing

These settings are related to generating and signing the app bundle.

- **Scheme**

  Configures the signing scheme for the bundle. It can be set to one of the following values:

  - `Manual provisioning`: With this value, you'll be responsible for setting provisioning profiles and signing certificates yourself.
  - `Automatic provisioning`: (default) With this value, Visual Studio will set provisioning profiles and signing certificates for you, which simplifies app deployment when testing on a device.

- **Signing identity**

  A signing identity is the certificate and private key pair that's used for code-signing app bundle using Apple's codesign utility.

  - `Developer (automatic)` (default)
  - `Distribution (automatic)`

- **Provisioning profile**

  Provisioning profiles are a way of tying together a team of developers with an App ID and, potentially, a list of test devices. The provisioning profiles list is filtered to only show provisioning profiles that match both the chosen identity and the App ID (aka bundle identifier) set in the _Info.plist_. If the provisioning profile that you're looking for isn't in the list, make sure that you've chosen a compatible identity and double-check that the bundle identifier set in your _Info.plist_ is correct.

- **Custom Entitlements**

  The plist file to use for entitlements. For more information, see [Entitlements](../ios/entitlements.md).

- **Custom Resource Rules**

  The plist file containing custom rules used by Apple's codesign utility.

  > [!NOTE]
  > As of Mac OSX 10.10, Apple has deprecated the use of custom resource rules. So, this setting should be avoided unless absolutely necessary.

- **Additional arguments**

  Additional command line arguments to be passed to Apple's codesign utility during the code-signing phase of the build.

### Debug

These are settings related to debugging.

- **Debugging**

  When enabled, turns on debugging. The default is based on the current profile. Debug profiles enable debugging, while Release profiles disable debugging.

- **Profiling**

  When enabled, turns on profiling.

### IPA Options

When enabled, builds an iTunes Package Archive (IPA).

### On Demand Resources

Settings related to on-demand resources. For more information, see [Apple Developer Documentation - On-Demand Resources Essentials](https://developer.apple.com/library/archive/documentation/FileManagement/Conceptual/On_Demand_Resources_Guide/index.html).

- **Initial Tags**

  The tags of the on-demand resources that are downloaded at the same time the app is downloaded from the app store. Separate tags with a semicolon `;`.

- **Pre-fetch Order**

  The tags of the on-demand resources that are downloaded after the app is installed. Separate tags with a semicolon `;`.

- **Embed**

  When enabled, embeds on-demand resources in the app bundle. This is enabled by default. Disable this setting to use the **Web server**.

- **Web server**

  The URI of a web server that hosts on-demand resources.

### Run Options

Options related to running the app on an iOS or macOS device.

- **Execution mode**

  This setting determines how the app is run on the target device.

- **Start arguments**

  Additional command line arguments to be passed to the app when it's started on the device.

- **Extra mlaunch arguments**

  Additional command line arguments to be passed to **mlaunch**.

- **Environment variables**

  Name-value pairs of environment variables to set when the app is run on the device.

[tfm]: /dotnet/standard/frameworks
