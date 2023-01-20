---
title: "Upgrading dependencies"
description: "Updating libraries to be compatible with .NET 6 and newer."
ms.date: 1/20/2023
---

# Upgrading library dependencies

NuGets and other library dependencies that depend upon pre-net6 target frameworks are not compatible (in most cases) with .NET 6 and newer. When upgrading your Xamarin projects, you should identify which packages have `net6-` or newer compatibility.

> .NET Standard libraries that have no dependencies on the incompatible frameworks below are still compatible with .NET 6 and newer.

| Compatible Frameworks | Incompatible Frameworks |
|:--|:--|
| net6.0-android | monoandroid, monoandroid10.0 |
| net6.0-ios | monotouch, xamarinios, xamarinios10 |
| net6.0-macos | monomac, xamarinmac, xamarinmac20 |
| net6.0-maccatalyst |  |
| net6.0-tvos | xamarintvos |
| | xamarinwatchos |
| net6.0-windows | uap10.0.16299 |

If a package on [NuGet.org](https://nuget.org) indicates compatibility with any of the net6 or newer frameworks above, regardless of also including incompatible frameworks, then the package is reported to be compatible.

Replace any incompatible packages with compatible alternatives.

### See also:

* [Porting from .NET Framework to .NET](https://learn.microsoft.com/en-us/dotnet/core/porting/)
* [.NET Upgrade Assistant](https://learn.microsoft.com/en-us/dotnet/core/porting/upgrade-assistant-overview)
