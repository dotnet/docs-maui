---
title: "Real-world example using CocoaPods"
description: "This document describes how to bind an Objective-C CocoaPod using Objective Sharpie."
ms.date: 02/11/2026
---

# Real-world example using CocoaPods

To bind a CocoaPod with Objective Sharpie, first install and build the pod using CocoaPods, then pass the resulting header files to the `sharpie bind` tool.

## Building a CocoaPod

You should [familiarize yourself with CocoaPods](https://cocoapods.org) before proceeding. Create a `Podfile` for the pod you want to bind, install it, and build the resulting Xcode workspace:

```bash
$ pod init
$ # Edit Podfile to add the desired pod, e.g. pod 'AFNetworking'
$ pod install
$ xcodebuild -workspace MyProject.xcworkspace -scheme MyProject -sdk iphoneos18.4 -arch arm64
```

## Creating the binding

Once the pod is built, locate the header files for the pod (typically under the `Pods/Headers/Public` directory) and pass them to Objective Sharpie:

```bash
$ sharpie bind \
    -f Pods/Headers/Public/AFNetworking/AFNetworking.h \
    --scope Pods/Headers/Public/AFNetworking \
    -o Binding \
    -sdk iphoneos18.4 \
    -c -IPods/Headers/Public -arch arm64
```

This will generate the **ApiDefinitions.cs** and **StructsAndEnums.cs** files.

## Next steps

After generating the **ApiDefinitions.cs** and **StructsAndEnums.cs** files,
take a look at the following documentation to generate an assembly to use
in your apps:

- [Binding Objective-C overview](~/cross-platform/macios/binding/overview.md)
- [Binding Objective-C libraries](~/cross-platform/macios/binding/objective-c-libraries.md)
- [Walkthrough: Binding an iOS Objective-C library](~/ios/platform/binding-objective-c/walkthrough.md)
