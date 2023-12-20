---
ms.topic: include
ms.date: 08/30/2023
---

## Update app dependencies

Generally, Xamarin.Forms NuGet packages are not compatible with .NET 8 unless they have been recompiled using .NET target framework monikers (TFMs). However, Android apps can use NuGet packages targeting the `monoandroid` and `monoandroidXX.X` frameworks.

You can confirm a package is .NET 8 compatible by looking at the **Frameworks** tab on [NuGet](https://nuget.org) for the package you're using, and checking that it lists one of the compatible frameworks shown in the following table:

| Compatible frameworks | Incompatible frameworks |
| --- | --- |
| net8.0-android, monoandroid, monoandroidXX.X | |
| net8.0-ios | monotouch, xamarinios, xamarinios10 |
| net8.0-macos | monomac, xamarinmac, xamarinmac20 |
| net8.0-tvos | xamarintvos |
| | xamarinwatchos |

> [!NOTE]
> .NET Standard libraries that have no dependencies on the incompatible frameworks listed above are still compatible with .NET 8.

If a package on [NuGet](https://nuget.org) indicates compatibility with any of the compatible frameworks above, regardless of also including incompatible frameworks, then the package is compatible. Compatible NuGet packages can be added to your .NET MAUI library project using the NuGet package manager in Visual Studio.

If you can't find a .NET 8 compatible version of a NuGet package you should:

- Recompile the package with .NET TFMs, if you own the code.
- Look for a preview release of a .NET 8 version of the package.
- Replace the dependency with a .NET 8 compatible alternative.
