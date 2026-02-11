---
title: "Binding Native Frameworks"
description: "This document describes how to use Objective Sharpie to create a binding to a library distributed as a framework."
ms.date: 02/11/2026
---

# Binding Native Frameworks

Sometimes a native library is distributed as a [framework](https://developer.apple.com/library/mac/documentation/MacOSX/Conceptual/BPFrameworks/Concepts/WhatAreFrameworks.html). Objective Sharpie can bind these frameworks by passing the umbrella header file and the framework's headers directory to the `bind` tool.

## Walkthrough: binding the Sparkle framework

This walkthrough shows how to create a C# binding for the [Sparkle](https://sparkle-project.org/) framework, a popular macOS library for software updates. The same process applies to any `.framework` bundle, whether targeting macOS or iOS — only the `-sdk` value differs.

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

The `Sparkle.h` file is the umbrella header — it `#import`s all the other public headers.

### 2. Run Objective Sharpie

Pass the umbrella header, the scope, and the target SDK to `sharpie bind`. Because Sparkle is a macOS framework, we use `-sdk macosx`:

```bash
$ sharpie bind \
    -f ./Sparkle.framework/Headers/Sparkle.h \
    --scope ./Sparkle.framework/Headers \
    -o Binding \
    -sdk macosx \
    -c -F . -arch arm64

Bindings generated successfully.
```

The arguments break down as follows:

|Argument|Purpose|
|---|---|
|`-f ./Sparkle.framework/Headers/Sparkle.h`|The umbrella header file to parse.|
|`--scope ./Sparkle.framework/Headers`|Only generate bindings for APIs declared in this directory. Without this, Objective Sharpie would also bind any system headers the framework imports.|
|`-o Binding`|The output directory for the generated files.|
|`-sdk macosx`|Target SDK. Use `iphoneos` for iOS frameworks.|
|`-c`|All arguments after `-c` are passed directly to the Clang compiler.|
|`-F .`|A Clang argument that adds the current directory as a framework search path, so Clang can resolve `#import <Sparkle/Sparkle.h>`.|
|`-arch arm64`|A Clang argument specifying the target architecture.|

> [!TIP]
> For an iOS framework, replace `-sdk macosx` with `-sdk iphoneos`.

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

Add the generated `ApiDefinition.cs` and `StructsAndEnums.cs` files to a
[binding project](~/cross-platform/macios/binding/objective-c-libraries.md)
to produce the final binding assembly.

## Framework binding tips

- **Umbrella header**: The `-f` option should point to the framework's umbrella header (usually `FrameworkName.h`). If the umbrella header has a different name, check the framework's `Modules/module.modulemap` file for the header declaration.
- **Scope**: Always pass `--scope` pointing to the framework's `Headers` directory. Without it, Objective Sharpie may generate bindings for system SDK headers that the framework imports, resulting in an excessively large output.
- **Framework search paths**: Pass `-c -F <directory>` where `<directory>` is the parent directory containing the `.framework` bundle, so that Clang can resolve framework imports.
- **Info.plist**: If the framework includes an **Info.plist** with SDK information (such as the `DTSDKName` key or a combination of `DTPlatformName` and `DTPlatformVersion` keys), you can use that to determine the correct `-sdk` value to pass.
