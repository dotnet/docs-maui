---
title: "Getting Started With Objective Sharpie"
description: "This document provides a high-level overview of Objective Sharpie, the tool used to automate the creation of .NET bindings to Objective-C code."
ms.date: 02/11/2026
---

# Getting Started With Objective Sharpie

> [!IMPORTANT]
> Objective Sharpie is a tool for experienced .NET developers with
> advanced knowledge of Objective-C (and by extension, C). Before
> attempting to bind an Objective-C library you should have solid
> knowledge of how to use the native framework in a native (Xcode) project (and a
> good understanding of how the native framework works).

<a name="installing"></a>

## Installing Objective Sharpie

Objective Sharpie is a .NET command line tool for macOS (arm64 only). Install it as a global .NET tool:

```bash
dotnet tool install -g dotnet-sharpie
```

> [!TIP]
> Use the `dotnet tool update -g dotnet-sharpie` command to update to the latest version.

## Basic Walkthrough

Objective Sharpie is a command line tool that assists in creating the
definitions required to bind a 3rd party Objective-C library to C#. Even when
using Objective Sharpie, the developer *will* need to modify the generated
files after Objective Sharpie finishes to address any issues that could not be
automatically handled by the tool.

Where possible, Objective Sharpie will annotate APIs with which it has some
doubt on how to properly bind (many constructs in the native code are ambiguous).
These annotations will appear as [`[Verify]` attributes](~/ios/objective-sharpie/platform/verify.md).

The output of Objective Sharpie is a pair of files -
[`ApiDefinition.cs` and `StructsAndEnums.cs`](~/ios/objective-sharpie/platform/apidefinitions-structsandenums.md) -
that can be used to create a binding project which compiles into a library
you can use in .NET projects.

> [!IMPORTANT]
> Objective Sharpie comes with one **major** rule for proper usage: you
> must absolutely pass it the correct clang compiler command line arguments
> in order to ensure proper parsing. This is because the Objective Sharpie
> parsing phase is simply a tool [implemented against the clang libtooling
> API](https://clang.llvm.org/docs/LibTooling.html).

This means that Objective Sharpie has the full power of Clang
(the C/Objective-C/C++ compiler that actually compiles the native library
you would bind) and all of its internal knowledge of the header files for binding.
Instead of translating the parsed [AST](https://en.wikipedia.org/wiki/Abstract_syntax_tree)
to object code, Objective Sharpie translates the AST to a C# binding "scaffold"
suitable for input to a binding project.

If Objective Sharpie errors out during parsing, it means that clang errored out
during its parsing phase trying to construct the AST, and you need to figure out why.

It's required to be familiar enough with the native framework to know how it's
used in an Xcode project; and if the framework requires any special compiler
flags in an Xcode project, those flags will have to be passed to Objective
Sharpie.
