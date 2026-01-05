---
title: "Binding Native Frameworks"
description: "This document describes how to use Objective Sharpie's -framework option to create a binding to a library distributed as a framework."
ms.date: 01/05/2026
---

# Binding Native Frameworks

Sometimes a native library is distributed as a [framework](https://developer.apple.com/library/mac/documentation/MacOSX/Conceptual/BPFrameworks/Concepts/WhatAreFrameworks.html). Objective Sharpie provides a convenience feature for binding properly defined frameworks through the `-framework` option.

For example, binding the [Adobe Creative SDK Framework](https://creativesdk.adobe.com/downloads.html) for iOS is straightforward:

```
$ sharpie bind \
    -framework ./AdobeCreativeSDKFoundation.framework \
    -sdk iphoneos8.1
```

In some cases, a framework will specify an **Info.plist** which indicates against which SDK the framework should be compiled. If this information exists and no explicit `-sdk` option is passed, Objective Sharpie will infer it from the framework's **Info.plist** (either the `DTSDKName` key or a combination of the `DTPlatformName` and `DTPlatformVersion` keys).

The `-framework` option does not allow explicit header files to be passed. The umbrella header file is chosen by convention based on the framework name. If an umbrella header cannot be found, Objective Sharpie will not attempt to bind the framework and you must manually perform the binding by providing the correct umbrella header file(s) to parse, along with any framework arguments for clang (such as the `-F` framework search path option).

Under the hood, specifying `-framework` is just a shortcut. The following
bind arguments are identical to the `-framework` shorthand above.
Of special importance is the `-F .` framework search path provided to clang
(note the space and period, which are required as part of the command).

```
$ sharpie bind \
    -sdk iphoneos8.1 \
    ./AdobeCreativeSDKFoundation.framework/Headers/AdobeCreativeSDKFoundation.h \
    -scope AdobeCreativeSDKFoundation.framework/Headers \
    -c -F .
```
