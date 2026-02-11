---
title: "Real-World Example using an Xcode Project"
description: "This document describes how to use an Xcode project with Objective Sharpie to create C# bindings to Objective-C code."
ms.date: 02/11/2026
---

# Real-World Example using an Xcode Project

**This example uses the [POP library from Facebook](https://github.com/facebook/pop).**

When binding a library that has an Xcode project, first build the project to produce the header files, then pass those headers to Objective Sharpie using the `bind` tool.

```bash
$ git clone https://github.com/facebook/pop.git
Cloning into 'pop'...
   (more git clone output)

$ cd pop
$ xcodebuild -sdk iphoneos18.4 -arch arm64
```

After building, locate the header files (often in the build output or a `Headers` directory) and use `sharpie bind`:

```bash
$ sharpie bind \
    -f build/Headers/POP/POP.h \
    --scope build/Headers \
    -o Binding \
    -sdk iphoneos18.4 \
    -c -Ibuild/Headers -arch arm64
```

For more details on this approach, see the [advanced manual example](advanced.md).
