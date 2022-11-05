---
title: "Upgrading dependencies"
description: ""
ms.date: 11/04/2022
---

# Upgrading library dependencies

NuGets and other library dependencies that depend upon pre-net6 target frameworks will not be compatible in most cases with .NET 6 and newer. When upgrading your Xamarin projects, you should identify which packages have `net6-` or newer compatibility.

| Compatible Frameworks | Incompatible Frameworks |
|:--|:--|
| net6.0-android | monoandroid, monoandroid10.0 |
| net6.0-ios | monotouch, xamarinios, xamarinios10 |
| net6.0-macos | monomac, xamarinmac, xamarinmac20 |
| net6.0-maccatalyst |  |
| net6.0-tvos | xamarintvos |
| | xamarinwatchos |
| net6.0-windows | uap10.0.16299 |

If a package on NuGet.org indicates compatibility with any of the net6 or newer frameworks above, regardless of also including incompatible frameworks, then the package is reported to be compatible.

Replace any incompatible packages with compatible alternatives.

## How to make your packages compatible

If you have the source code for a library then you can add .NET 6 (or newer) support.

### See also

