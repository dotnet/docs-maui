---
title: "Real-world example using CocoaPods"
description: "This document demonstrates how to use Objective Sharpie to automatically generate the C# binding definitions from a CocoaPod."
ms.date: 01/05/2026
---

# Real-world example using CocoaPods

> [!IMPORTANT]
> `sharpie pod` is now deprecated. For more information, see [this GitHub issue](https://github.com/xamarin/xamarin-macios/issues/8238#issuecomment-606666460).

New in version 3.0, Objective Sharpie supports binding CocoaPods, and even
includes a command (`sharpie pod`) to make downloading, configuring, and
building CocoaPods very easy. You should [familiarize yourself with
CocoaPods](https://cocoapods.org) in general before using this feature.

## Creating a binding for a CocoaPod

The `sharpie pod` command has one global option and two subcommands:

```bash
$ sharpie pod -help
usage: sharpie pod [OPTIONS] COMMAND [COMMAND_OPTIONS]

Pod Options:
  -d, -dir DIR     Use DIR as the CocoaPods binding directory,
                   defaulting to the current directory

Available Commands:
  init         Initialize a new Xamarin C# CocoaPods binding project
  bind         Bind an existing Xamarin C# CocoaPods project
```

The `init` subcommand also has some useful help:

```bash
$ sharpie pod init -help
usage: sharpie pod init [INIT_OPTIONS] TARGET_SDK POD_SPEC_NAMES

Init Options:
  -f, -force       Initialize a new Podfile and run actions against
                   it even if one already exists
```

Multiple CocoaPod names and subspec names can be provided to `init`.

```bash
$ sharpie pod init ios AFNetworking
** Setting up CocoaPods master repo ...
   (this may take a while the first time)
** Searching for requested CocoaPods ...
** Working directory:
**   - Writing Podfile ...
**   - Installing CocoaPods ...
**     (running `pod install --no-integrate --no-repo-update`)
Analyzing dependencies
Downloading dependencies
Installing AFNetworking (2.6.0)
Generating Pods project
Sending stats
** 🍻 Success! You can now use other `sharpie podn`  commands.
```

Once your CocoaPod has been set up, you can now create the binding:

```bash
$ sharpie pod bind
```

This will result in the CocoaPod Xcode project being built and then
evaluated and parsed by Objective Sharpie. A lot of console output will be
generated, but should result in the binding definition at the end:

```bash
(... lots of build output ...)

Parsing 19 header files...

Binding...
  [write] ApiDefinitions.cs
  [write] StructsAndEnums.cs

Done.
```

## Next steps

After generating the **ApiDefinitions.cs** and **StructsAndEnums.cs** files,
take a look at the following documentation to generate an assembly to use
in your apps:

- [Binding Objective-C overview](~/cross-platform/macios/binding/overview.md)
- [Binding Objective-C libraries](~/cross-platform/macios/binding/objective-c-libraries.md)
- [Walkthrough: Binding an iOS Objective-C library](~/ios/platform/binding-objective-c/walkthrough.md)
