---
title: "Binding Native Frameworks"
description: "This document describes how to use Objective Sharpie to create a binding to a library distributed as a framework."
ms.date: 02/11/2026
---

# Binding Native Frameworks

Sometimes a native library is distributed as a [framework](https://developer.apple.com/library/mac/documentation/MacOSX/Conceptual/BPFrameworks/Concepts/WhatAreFrameworks.html). Objective Sharpie can bind these frameworks by passing the umbrella header file and the framework's headers directory.

For example, binding the Adobe Creative SDK Framework for iOS:

```bash
$ sharpie bind \
    -f ./AdobeCreativeSDKFoundation.framework/Headers/AdobeCreativeSDKFoundation.h \
    --scope ./AdobeCreativeSDKFoundation.framework/Headers \
    -sdk iphoneos18.4 \
    -o Binding \
    -c -F .
```

The `-f` option specifies the umbrella header file to parse. The `--scope` option restricts binding to APIs defined within the framework's `Headers` directory.

The `-c -F .` passes the `-F .` argument directly to the Clang compiler, which adds the current directory as a framework search path. This is necessary so that Clang can resolve `#import` directives that reference the framework.

If the framework includes an **Info.plist** with SDK information (such as the `DTSDKName` key or a combination of `DTPlatformName` and `DTPlatformVersion` keys), you can use that to determine the correct `-sdk` value to pass.

> [!TIP]
> If an umbrella header cannot be found by convention (matching the framework name), 
> you must manually identify the correct header file to pass to Objective Sharpie.
