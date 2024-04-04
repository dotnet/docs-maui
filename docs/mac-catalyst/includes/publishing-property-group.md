---
ms.topic: include
ms.date: 03/23/2023
---

This example `<PropertyGroup>` adds a condition check, preventing the settings from being processed unless the condition check passes. The condition check looks for two items:

1. The build configuration is set to `Release`.
1. The target framework is set to something containing the text `net8.0-maccatalyst`.
1. The platform is set to `AnyCPU`.

If any of these conditions fail, the settings aren't processed. More importantly, the `<CodesignKey>`, `<CodesignProvision>`, and `<PackageSigningKey>` settings aren't set, preventing the app from being signed.

After adding the above property group, the app can be published from the command line on a Mac by opening a terminal and navigating to the folder for your .NET MAUI app project. Then, run the following command:

```dotnetcli
dotnet build -f net8.0-maccatalyst -c Release
```

[!INCLUDE [dotnet publish in .NET 8](~/includes/dotnet-publish-net8.md)]

Publishing builds, signs, and packages the app, and then copies the *.pkg* to the *bin/Release/net8.0-maccatalyst/publish/* folder.
