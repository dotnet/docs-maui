---
title: "Objective Sharpie Tools &amp; Commands"
description: "This document provides an overview of the tools included with Objective Sharpie and the command-line arguments to use with them."
ms.date: 02/11/2026
---

# Objective Sharpie Tools & Commands

_Overview of the tools included with Objective Sharpie, and the command line arguments to use them._

Once Objective Sharpie is successfully [installed](~/cross-platform/macios/binding/objective-sharpie/get-started.md),
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
|**bind**|Parses a header file (`*.h`) for an Objective-C library and generates the initial [ApiDefinition.cs and StructsAndEnums.cs](~/cross-platform/macios/binding/objective-sharpie/platform/apidefinitions-structsandenums.md) files.|
|**sdk-db**|Generates bindings for an entire Apple platform SDK (iOS, macOS, tvOS, etc.). This is used internally for .NET for iOS/macOS bindings and can also be used to bind a complete SDK.|

To get help on a specific Objective Sharpie tool, enter the name of the tool and the `-help` option.

## The `bind` tool

The `bind` tool is the primary tool for creating bindings for 3rd party Objective-C frameworks. Run `sharpie bind -help` for the complete list of options:

```
$ sharpie bind -help

Options:
  -h, --help                 Show detailed
  -v, --verbose              Be verbose with output
  -q, --quiet                Suppress fluffy status messages
  @file                      Read response file for more options

Parse options:
  -s, --sdk=VALUE            Target SDK
  -f, --header=VALUE         The input header file to bind
      --scope=VALUE          Restrict following #include and #import
                               directives declared in header files to
                               within the specified DIR directory
  -c, --clang                All arguments after this argument are not
                               processed by Objective Sharpie and are
                               proxied directly to Clang

Bind options:
  -o, --output=VALUE         Output directory for generated binding files
  -n, --namespace=VALUE      Namespace under which to generate the binding
  -m, --massage=VALUE        Register (+ prefix) or exclude (- prefix) a
                               massager by name
      --nosplit              Do not split the generated binding into
                               multiple files
```

### Basic usage

To bind a framework, specify the umbrella header file and the target SDK. For example, to bind the [Sparkle](https://sparkle-project.org/) framework:

```bash
$ sharpie bind \
    -f ./Sparkle.framework/Headers/Sparkle.h \
    --scope ./Sparkle.framework/Headers \
    -o Binding \
    -sdk macosx \
    -c -F . -arch arm64

Bindings generated successfully.
```

The `-c` argument tells Objective Sharpie to stop processing its own options and forward all subsequent arguments directly to the Clang compiler. In the example above, `-F .` is a Clang argument that adds the current directory as a framework search path, and `-arch arm64` specifies the target architecture.

The `--scope` argument restricts binding to only APIs defined in headers within the specified directory, preventing Objective Sharpie from generating bindings for system headers that are `#import`ed by the framework.

For a complete framework binding walkthrough, see [Binding Native Frameworks](~/cross-platform/macios/binding/objective-sharpie/platform/native-frameworks.md).

## The `sdk-db` tool

The `sdk-db` tool generates bindings for an entire platform SDK. This is primarily used internally, but can also be used to generate a complete set of bindings for a given SDK.

```
$ sharpie sdk-db -help

Options:
  -h, --help                 Show detailed
  -v, --verbose              Be verbose with output
  -q, --quiet                Suppress fluffy status messages
  @file                      Read response file for more options

Options:
  -a, --arch=VALUE           Specify which architecture(s) to build for
  -o, --output=VALUE         Output directory for generated binding files
  -s, --sdk=VALUE            Target SDK
  -x, --exclude=VALUE        Exclude a framework by name from the SDK
  -i, --extra-hash-import=VALUE
                               Framework-relative header for which to
                               generate an #import
      --modules=VALUE        Enable/use modules (-fmodules). Defaults to
                               'true'
  -c, --clang                All arguments after this argument are not
                               processed by Objective Sharpie and are
                               proxied directly to Clang
      --nosplit              Do not split the generated binding into
                               multiple files
```

### Basic usage

To generate bindings for the iOS SDK:

```bash
$ sharpie sdk-db -s iphoneos18.4 -o output/
```

To exclude specific frameworks:

```bash
$ sharpie sdk-db -s iphoneos18.4 -o output/ -x CloudKit -x GameKit
```
