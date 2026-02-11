---
title: "Creating Bindings with Objective Sharpie"
description: "This section provides an introduction to Objective Sharpie, a command line tool used to automate the process of creating a .NET binding to an Objective-C Library"
ms.date: 02/11/2026
---

# Creating Bindings with Objective Sharpie

_This section provides an introduction to Objective Sharpie, a command line tool used to automate the process of creating a .NET binding to an Objective-C Library_

- [Overview](#overview)
- [Getting Started](get-started.md)
- [Tools & Commands](tools.md)
- [Features](platform/index.md)
- [Examples](examples/index.md)
- [Complete Walkthrough](platform/native-frameworks.md)

## Overview

Objective Sharpie is a command line tool to help bootstrap the first pass of a binding.
It works by parsing the header files of a native framework to map the public API
into the [binding definition](platform/api-definition-structs-and-enums.md).

Objective Sharpie uses Clang to parse header files, so the binding is as exact and thorough as possible. This can greatly reduce the time and effort it takes to produce a quality binding.

Objective Sharpie is distributed as a [.NET tool](/dotnet/core/tools/global-tools) and is [open source](https://github.com/dotnet/macios/tree/main/tools/sharpie).

> [!IMPORTANT]
> Objective Sharpie is a tool for experienced .NET developers with
> advanced knowledge of Objective-C (and by extension, C). Before
> attempting to bind an Objective-C library you should have solid
> knowledge of how to use the native framework in a native (Xcode) project (and a
> good understanding of how the native framework works).

## Related Links

- [Walkthrough: Binding a Native Framework](platform/native-frameworks.md)
- [iOS Binding Projects](~/migration/ios-binding-projects.md)
