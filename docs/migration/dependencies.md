---
title: "Update app dependencies"
description: "Learn how to update app dependencies to be compatible with .NET 6+."
ms.date: 1/31/2023
---

# Update app dependencies

NuGet packages and other library dependencies that depend upon target frameworks prior to .NET 6 are generally not compatible with .NET 6+. When migrating your Xamarin projects, you should identify which packages have `net6-` or later compatibility.

> [!NOTE]
> .NET Standard libraries that have no dependencies on the incompatible frameworks listed below are still compatible with .NET 6+.

| Compatible frameworks | Incompatible frameworks |
|:--|:--|
| net6.0-android | monoandroid, monoandroid10.0 |
| net6.0-ios | monotouch, xamarinios, xamarinios10 |
| net6.0-macos | monomac, xamarinmac, xamarinmac20 |
| net6.0-maccatalyst |  |
| net6.0-tvos | xamarintvos |
| | xamarinwatchos |
| net6.0-windows | uap10.0.16299 |

If a package on [NuGet](https://nuget.org) indicates compatibility with any of the net6 or newer frameworks above, regardless of also including incompatible frameworks, then the package is compatible.

You should replace any incompatible packages with compatible alternatives.

## See also

- [Porting from .NET Framework to .NET](/dotnet/core/porting/)
- [.NET Upgrade Assistant](/dotnet/core/porting/upgrade-assistant-overview)
