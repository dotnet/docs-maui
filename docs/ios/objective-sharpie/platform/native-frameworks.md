---
title: "Binding Native Frameworks"
description: "This document describes how to use Objective Sharpie to create a binding to a library distributed as a framework or XCFramework."
ms.date: 10/29/2026
ai-usage: ai-assisted
no-loc: [ "xcframework" ]
---

# Binding Native Frameworks

Typically a native library is distributed as a [framework](https://developer.apple.com/library/mac/documentation/MacOSX/Conceptual/BPFrameworks/Concepts/WhatAreFrameworks.html). Objective Sharpie can bind these frameworks directly — pass the `.framework` directory to `-f/--framework` and Objective Sharpie will automatically find the umbrella header (or module map), set the scope, set the namespace, and detect the target SDK from the framework's `Info.plist` if available.

## Walkthrough: binding the Sparkle framework

This walkthrough shows how to create a C# binding for the [Sparkle](https://sparkle-project.org/) framework, a popular macOS library for software updates. The same process applies to any `.framework` bundle, whether targeting macOS or iOS.

### 1. Obtain the framework

Download the latest Sparkle release from [GitHub](https://github.com/sparkle-project/Sparkle/releases) and extract `Sparkle.framework` into a working directory:

```bash
$ ls Sparkle.framework/
Headers    Modules    Resources    Sparkle    Versions
$ ls Sparkle.framework/Headers/
Sparkle.h              SPUDownloadData.h      SUAppcast.h
SUAppcastItem.h        SUErrors.h             SUExport.h
SUStandardVersionComparator.h                 SUUpdater.h
SUUpdaterDelegate.h    SUVersionComparisonProtocol.h
SUVersionDisplayProtocol.h
```

The `Sparkle.h` file is the umbrella header — it `#import`s all the other public headers. When you use `-f/--framework`, Objective Sharpie finds this automatically.

### 2. Run Objective Sharpie

Pass the `.framework` directory to `sharpie bind` with `-f`:

> [!IMPORTANT]
> Install the .NET workload for the platform that the framework targets before running `sharpie bind`. For this macOS example, run `dotnet workload install macos`. For an iOS framework, install the `ios` workload instead.

```bash
$ sharpie bind \
    -f ./Sparkle.framework \
    -o Binding

Bindings generated successfully.
```

That's it — Objective Sharpie automatically:

- Finds the umbrella header or module map inside the framework.
- Sets `--scope` to the framework directory (so only the framework's own APIs are bound).
- Sets `--namespace` to `Sparkle` (the framework's name).
- Reads the framework's `Info.plist` and detects the target SDK (`macosx`).
- Configures the Clang framework search path.

The only required argument besides `-f` is `-o` to specify the output directory.

> [!TIP]
> If the framework doesn't include an `Info.plist` with SDK information (the `DTSDKName` key), add `-sdk` explicitly — for example, `-sdk macosx` for macOS or `-sdk iphoneos` for iOS.

### 3. Review the output

Objective Sharpie generates two files in the output directory:

```bash
$ ls Binding/
ApiDefinition.cs    StructsAndEnums.cs
```

**StructsAndEnums.cs** contains enum definitions:

```csharp
public enum SUError {
    AppcastParseError = 1000,
    NoUpdateError = 1001,
    AppcastError = 1002,
    // ...
}
```

**ApiDefinition.cs** contains the interface declarations for the binding:

```csharp
// @interface SUAppcast : NSObject
[BaseType (typeof (NSObject))]
interface SUAppcast {
    // @property (copy) NSString * _Nullable userAgentString;
    [NullAllowed, Export ("userAgentString")]
    string UserAgentString { get; set; }

    // @property (readonly, copy) NSArray * _Nullable items;
    [NullAllowed, Export ("items", ArgumentSemantic.Copy)]
    [Verify (StronglyTypedNSArray)]
    NSObject [] Items { get; }
}
```

Notice the `[Verify (StronglyTypedNSArray)]` attribute on the `Items` property. This signals that Objective Sharpie wasn't sure about the array element type — in this case, you know the items are `SUAppcastItem` objects, so you should change `NSObject[]` to `SUAppcastItem[]` and remove the `[Verify]` attribute. For more about `[Verify]` attributes, see [Verify attributes](verify.md).

### 4. Create a binding project

Add the generated `ApiDefinition.cs` and `StructsAndEnums.cs` files, along with a `<NativeReference>` to the framework or `.xcframework`, to an SDK-style binding project (`.csproj`) to produce the final binding assembly:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0-macos</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>true</ImplicitUsings>
    <IsBindingProject>true</IsBindingProject>
  </PropertyGroup>

  <ItemGroup>
    <ObjcBindingApiDefinition Include="ApiDefinition.cs" />
    <ObjcBindingCoreSource Include="StructsAndEnums.cs" />
  </ItemGroup>

  <ItemGroup>
    <NativeReference Include="Sparkle.framework">
      <Kind>Framework</Kind>
      <SmartLink>False</SmartLink>
    </NativeReference>
  </ItemGroup>
</Project>
```

For complete guidance on configuring binding projects, packaging bindings with `dotnet pack`, and integrating `.xcframework` bundles, see [Binding Objective-C libraries](/dotnet/ios/binding-libs/) and [Binding an XCFramework](/dotnet/ios/binding-libs/xcframeworks). If you're upgrading an existing Xamarin.iOS binding project, see [Xamarin.iOS binding project migration](~/migration/ios-binding-projects.md).

<a name="binding-an-xcframework"></a>

## Bind an XCFramework

Apple distributes multi-platform and multi-architecture native libraries as `.xcframework` bundles. An `.xcframework` directory contains an `Info.plist` manifest at the root and separate subdirectories (slices) for each target platform and architecture, such as `ios-arm64` for devices and `ios-arm64_x86_64-simulator` for the simulator:

```text
MyLibrary.xcframework/
├── Info.plist
├── ios-arm64/
│   └── MyLibrary.framework/
│       ├── Headers/
│       │   └── MyLibrary.h
│       ├── Info.plist
│       └── MyLibrary
└── ios-arm64_x86_64-simulator/
    └── MyLibrary.framework/
        ├── Headers/
        │   └── MyLibrary.h
        ├── Info.plist
        └── MyLibrary
```

### Generate C# definitions from a framework slice

Objective Sharpie's `-f/--framework` option expects a `.framework` bundle rather than the top-level `.xcframework` directory. To generate `ApiDefinition.cs` and `StructsAndEnums.cs`, pass the `.framework` directory from one of the platform slices inside the `.xcframework` (typically the device slice, such as `ios-arm64`):

```bash
$ sharpie bind \
    -f ./MyLibrary.xcframework/ios-arm64/MyLibrary.framework \
    -o Binding

Bindings generated successfully.
```

If the slice's `Info.plist` doesn't include the `DTSDKName` key, specify the target SDK explicitly with `-sdk iphoneos` (or `-sdk macosx` for macOS).

### Reference the top-level XCFramework in the binding project

Although you point `sharpie bind` at a single `.framework` slice to parse the headers, your binding project (`.csproj`) must reference the top-level `.xcframework` directory (or a compressed `.xcframework.zip` archive) in `<NativeReference>`—**not** the inner `.framework` slice subfolder:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0-ios</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>true</ImplicitUsings>
    <IsBindingProject>true</IsBindingProject>
  </PropertyGroup>

  <ItemGroup>
    <ObjcBindingApiDefinition Include="ApiDefinition.cs" />
    <ObjcBindingCoreSource Include="StructsAndEnums.cs" />
  </ItemGroup>

  <ItemGroup>
    <NativeReference Include="MyLibrary.xcframework">
      <Kind>Framework</Kind>
      <SmartLink>False</SmartLink>
    </NativeReference>
  </ItemGroup>
</Project>
```

Referencing the top-level `.xcframework` allows the .NET SDK to automatically resolve the appropriate platform and architecture slice when building for a physical device, simulator, or Mac Catalyst.

### Bind multiple XCFrameworks and native dependencies

If a native SDK distributes multiple `.xcframework` bundles (for example, a primary framework and one or more native dependency `.xcframework` bundles), add a `<NativeReference>` item for each `.xcframework` in the binding project:

```xml
<ItemGroup>
  <NativeReference Include="MyLibrary.xcframework">
    <Kind>Framework</Kind>
    <SmartLink>False</SmartLink>
  </NativeReference>
  <NativeReference Include="MyLibraryCore.xcframework">
    <Kind>Framework</Kind>
    <SmartLink>False</SmartLink>
  </NativeReference>
</ItemGroup>
```

When you build or pack the binding project (`dotnet pack`), the .NET SDK bundles all `<NativeReference>` items into a sidecar `<AssemblyName>.resources.zip` file (or `<AssemblyName>.resources` directory) alongside the managed binding assembly because `NoBindingEmbedding` is `true` by default. Setting `<NoBindingEmbedding>false</NoBindingEmbedding>` is deprecated and is no longer supported starting with .NET 12. For details on multi-framework bindings, `dotnet pack`, and binding resource packages, see [Binding an XCFramework](/dotnet/ios/binding-libs/xcframeworks).

## Framework binding tips

- **Use `-f/--framework`**: Pass the `.framework` directory directly instead of specifying the umbrella header and scope separately. Objective Sharpie handles the details automatically.
- **SDK auto-detection**: When using `-f`, Objective Sharpie reads the framework's `Info.plist` for the `DTSDKName` key to determine the target SDK. If the framework doesn't include this, add `-sdk` explicitly.
- **Manual header binding**: If you need to bind individual header files instead of a framework, use `--header` and `--scope` to control which headers are included.
