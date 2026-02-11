---
title: "Objective Sharpie Tools &amp; Commands"
description: "This document provides an overview of the tools included with Objective Sharpie and the command-line arguments to use with them."
ms.date: 02/11/2026
---

# Objective Sharpie Tools & Commands

_Overview of the tools included with Objective Sharpie, and the command line arguments to use them._

Once Objective Sharpie is successfully [installed](~/ios/objective-sharpie/get-started.md),
open a terminal and familiarize yourself with the *commands*
Objective Sharpie has to offer:

```
$ sharpie -help
usage: sharpie [OPTIONS] TOOL [TOOL_OPTIONS]

Options:
  -h, -help        Show detailed help
  -v, -version     Show version information

Available Tools:
  bind               Create a C# binding to Objective-C APIs
  sdk-db             Generate bindings for an entire platform's SDK
```

Objective Sharpie provides the following tools:

|Tool|Description|
|--- |--- |
|**bind**|Parses a native framework (`.framework` bundle) or a header file (`*.h`) for an Objective-C library and generates the initial [ApiDefinition.cs and StructsAndEnums.cs](~/ios/objective-sharpie/platform/apidefinitions-structsandenums.md) files.|
|**sdk-db**|Generates bindings for an entire Apple platform SDK (iOS, macOS, tvOS, etc.). This is used internally for .NET for iOS/macOS bindings and can also be used to bind a complete SDK.|

To get help on a specific Objective Sharpie tool, enter the name of the tool and the `-help` option.

## The `bind` tool

The `bind` tool is the primary tool for creating bindings for 3rd party Objective-C frameworks. Run `sharpie bind -help` for the complete list of options:

```
$ sharpie bind -help

Options:
  -h, --help                 Show detailed help information.
  -v, --verbose              Be verbose with output.
  -q, --quiet                Suppress fluffy status messages.

Parse options:
  -s, --sdk=VALUE            Target SDK.
  -f, --framework=VALUE      The input framework to bind. Implies setting
                               the scope (--scope) to the framework,
                               setting the namespace (--namespace) to the
                               name of the framework, and no other
                               sources/headers can be specified. If the
                               framework provides an 'Info.plist' with SDK
                               information (DTSDKName), the '-sdk' option
                               will be implied as well (if not manually
                               specified).
      --header=VALUE         The input header file to bind.
      --scope=VALUE          Restrict following #include and #import
                               directives declared in header files to
                               within the specified DIR directory.
  -c, --clang                All arguments after this argument are not
                               processed by Objective Sharpie and are
                               proxied directly to Clang.

Bind options:
  -o, --output=VALUE         Output directory for generated binding files.
  -n, --namespace=VALUE      Namespace under which to generate the binding.
                               The default is to use the framework's name
                               as the namespace.
  -m, --massage=VALUE        Register (+ prefix) or exclude (- prefix) a
                               massager by name.
      --nosplit              Do not split the generated binding into
                               multiple files.
  @file                      Read response file for more options.
```

### Basic usage

To bind a framework, pass the `.framework` directory to `-f`. Objective Sharpie will automatically find the umbrella header (or module map), set the scope and namespace, detect the SDK from the framework's `Info.plist` if available, and configure the framework search path for Clang. For example, to bind the [Sparkle](https://sparkle-project.org/) framework:

```bash
$ sharpie bind \
    -f ./Sparkle.framework \
    -o Binding

Bindings generated successfully.
```

If the framework doesn't include an `Info.plist` with SDK information, add `-sdk` explicitly (for example, `-sdk macosx` or `-sdk iphoneos`).

For a complete framework binding walkthrough, see [Binding Native Frameworks](~/ios/objective-sharpie/platform/native-frameworks.md).

## The `sdk-db` tool

The `sdk-db` tool generates bindings for an entire platform SDK. This is primarily used internally, but can also be used to generate a complete set of bindings for a given SDK.

```
$ sharpie sdk-db -help

Options:
  -h, --help                 Show detailed help information.
  -v, --verbose              Be verbose with output.
  -q, --quiet                Suppress fluffy status messages.

Options:
  -a, --arch=VALUE           Specify which architecture(s) to build for.
  -o, --output=VALUE         Output directory for generated binding files.
  -s, --sdk=VALUE            Target SDK.
  -x, --exclude=VALUE        Exclude a framework by name from the SDK.
  -i, --extra-hash-import=VALUE
                               Framework-relative header for which to
                               generate an #import.
      --modules=VALUE        Enable/use modules (-fmodules). Defaults to
                               'true'.
  -c, --clang                All arguments after this argument are not
                               processed by Objective Sharpie and are
                               proxied directly to Clang.
      --nosplit              Do not split the generated binding into
                               multiple files.
  @file                      Read response file for more options.
```

### Basic usage

To generate bindings for the iOS SDK:

```bash
sharpie sdk-db -s iphoneos18.4 -o output/
```

To exclude specific frameworks:

```bash
sharpie sdk-db -s iphoneos18.4 -o output/ -x CloudKit -x GameKit
```
