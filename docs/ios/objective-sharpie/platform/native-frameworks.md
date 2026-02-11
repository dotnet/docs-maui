---
title: "Binding Native Frameworks"
description: "This document describes how to use Objective Sharpie to create a binding to a library distributed as a framework."
ms.date: 02/11/2026
---

# Binding Native Frameworks

Sometimes a native library is distributed as a [framework](https://developer.apple.com/library/mac/documentation/MacOSX/Conceptual/BPFrameworks/Concepts/WhatAreFrameworks.html). Objective Sharpie can bind these frameworks directly — pass the `.framework` directory to `-f/--framework` and Objective Sharpie will automatically find the umbrella header (or module map), set the scope, set the namespace, and detect the target SDK from the framework's `Info.plist` if available.

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

```bash
$ sharpie bind \
    -f ./Sparkle.framework \
    -o Binding \
    -c -F . -arch arm64

Bindings generated successfully.
```

The arguments break down as follows:

|Argument|Purpose|
|---|---|
|`-f ./Sparkle.framework`|The framework to bind. Objective Sharpie automatically finds the umbrella header or module map, sets `--scope` to the framework directory, sets `--namespace` to `Sparkle`, and detects the SDK from the framework's `Info.plist`.|
|`-o Binding`|The output directory for the generated files.|
|`-c`|All arguments after `-c` are passed directly to the Clang compiler.|
|`-F .`|A Clang argument that adds the current directory as a framework search path, so Clang can resolve `#import <Sparkle/Sparkle.h>`.|
|`-arch arm64`|A Clang argument specifying the target architecture.|

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

Add the generated `ApiDefinition.cs` and `StructsAndEnums.cs` files to a
[binding project](~/cross-platform/macios/binding/objective-c-libraries.md)
to produce the final binding assembly.

## Framework binding tips

- **Use `-f/--framework`**: Pass the `.framework` directory directly instead of specifying the umbrella header and scope separately. Objective Sharpie handles the details automatically.
- **SDK auto-detection**: When using `-f`, Objective Sharpie reads the framework's `Info.plist` for the `DTSDKName` key to determine the target SDK. If the framework doesn't include this, add `-sdk` explicitly.
- **Framework search paths**: Pass `-c -F <directory>` where `<directory>` is the parent directory containing the `.framework` bundle, so that Clang can resolve framework imports.
- **Manual header binding**: If you need to bind individual header files instead of a framework, use `--header` (without the `-f` shorthand) and `--scope` to control which headers are included.
