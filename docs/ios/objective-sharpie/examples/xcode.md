---
title: "Real-World Example using an Xcode Project"
description: "This document describes how to use an Xcode project as a direct input to Objective Sharpie, simplifying the process of creating C# bindings to Objective-C code."
ms.date: 01/05/2026
---

# Real-World Example using an Xcode Project

**This example uses the [POP library from Facebook](https://github.com/facebook/pop).**

New in version 3.0, Objective Sharpie supports Xcode projects as input. These projects specify the correct header files and compiler flags necessary to compile the native library, and thus necessary to bind it too. Objective Sharpie will select the first _target_ and its default configuration of a project if not instructed to do otherwise.

Before Objective Sharpie attempts to parse the project and header files, it must build it. Projects often have build phases that will correctly structure header files for external consumption and integration, so it is best to always build the full project before attempting to bind it.

```
$ git clone https://github.com/facebook/pop.git
Cloning into 'pop'...
   (more git clone output)

$ cd pop
$ sharpie bind pop.xcodeproj -sdk iphoneos9.0
```
