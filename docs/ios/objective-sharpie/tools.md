---
title: "Objective Sharpie Tools &amp; Commands"
description: "This document provides an overview of the tools included with Objective Sharpie and the command-line arguments to use with them."
ms.date: 01/05/2026
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
  -h, -help                Show detailed help
  -v, -version             Show version information

Telemetry Options:
  -tlm-about               Show a detailed overview of what usage and binding
                             information will be submitted to Xamarin by
                             default when using Objective Sharpie.
  -tlm-do-not-submit       Do not submit any usage or binding information to
                             Xamarin. Run 'sharpie -tml-about' for more
                             information.
  -tlm-do-not-identify     Do not submit Xamarin account information when
                             submitting usage or binding information to Xamarin
                             for analysis. Binding attempts and usage data will
                             be submitted anonymously if this option is
                             specified.

Available Tools:
  xcode              Get information about Xcode installations and available SDKs.
  pod                Create a Xamarin C# binding to Objective-C CocoaPods
  bind               Create a Xamarin C# binding to Objective-C APIs
  update             Update to the latest release of Objective Sharpie
  verify-docs        Show cross reference documentation for [Verify] attributes
  docs               Open the Objective Sharpie online documentation
```

Objective Sharpie provides the following tools:

|Tool|Description|
|--- |--- |
|**xcode**|Provides information about the current Xcode installation and the versions of iOS and Mac SDKs that are available. We will be using this information later when we generate our bindings.|
|**pod**|Searches for, configures, installs (in a local directory), and binds Objective-C [CocoaPod](https://cocoapods.org/) libraries available from the master Spec repository. This tool evaluates the installed CocoaPod to automatically deduce the correct input to pass to the `bind` tool below. New in 3.0!|
|**bind**|Parses the header files (`*.h`) in the Objective-C library into the initial [ApiDefinition.cs and StructsAndEnums.cs](~/cross-platform/macios/binding/objective-sharpie/platform/apidefinitions-structsandenums.md) files.|
|**update**|Checks for newer versions of Objective Sharpie and downloads and launches the installer if one is available.|
|**verify-docs**|Shows detailed information about `[Verify]` attributes.|
|**docs**|Navigates to this document in your default web browser.|

To get help on a specific Objective Sharpie tool, enter the name of the tool and the `-help` option. For example, `sharpie xcode -help` returns the following output:

```
$ sharpie xcode -help
usage: sharpie xcode [OPTIONS]

Options:
  -h, -help        Show detailed help
  -v, -verbose     Be verbose with output

Xcode Options:
  -sdks            List all available Xcode SDKs. Pass -verbose for more details.
```

Before we can start the binding process, we need to get information about our current installed SDKs by entering the following command into the Terminal `sharpie xcode -sdks`. Your output may differ depending on which version(s) of Xcode you have installed. Objective Sharpie looks for SDKs installed in any `Xcode*.app` under the `/Applications` directory:

```
$ sharpie xcode -sdks
sdk: appletvos9.0    arch: arm64
sdk: iphoneos9.1     arch: arm64   armv7
sdk: iphoneos9.0     arch: arm64   armv7
sdk: iphoneos8.4     arch: arm64   armv7
sdk: macosx10.11     arch: x86_64  i386
sdk: macosx10.10     arch: x86_64  i386
sdk: watchos2.0      arch: armv7
```

From the above, we can see that we have the `iphoneos9.1` SDK installed on our
machine and it has `arm64` architecture support. We will be using this value
for all the samples in this section. With this information in place, we are ready to
parse an Objective-C library header files into the initial `ApiDefinition.cs`
and `StructsAndEnums.cs` for the Binding project.
