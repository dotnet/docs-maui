---
ms.topic: include
ms.date: 08/30/2023
---

## Update app dependencies

Xamarin.Forms NuGet packages are not compatible with .NET 7+ unless they have been recompiled using .NET target framework monikers (TFMs). You can confirm a package is .NET 7+ compatible by looking at the **Frameworks** tab on [NuGet](https://nuget.org) for the package you're using, and checking that it lists one of the compatible frameworks shown in the following table:

| Compatible frameworks | Incompatible frameworks |
| --- | --- |
| net7.0-android | monoandroid, monoandroid10.0 |
| net7.0-ios | monotouch, xamarinios, xamarinios10 |
| net7.0-maccatalyst |  |
| net7.0-windows | uap10.0.16299 |

> [!NOTE]
> .NET Standard libraries that have no dependencies on the incompatible frameworks listed above are still compatible with .NET 7+.

If a package on [NuGet](https://nuget.org) indicates compatibility with any of the `net7` or newer frameworks above, regardless of also including incompatible frameworks, then the package is compatible. Compatible NuGet packages can be added to your .NET MAUI library project using the NuGet package manager in Visual Studio.

If you can't find a .NET 7+ compatible version of a NuGet package you should:

- Recompile the package with .NET TFMs, if you own the code.
- Look for a preview release of a .NET 7+ version of the package.
- Replace the dependency with a .NET 7+ compatible alternative.
