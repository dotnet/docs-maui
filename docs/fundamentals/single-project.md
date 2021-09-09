---
title: ".NET MAUI single project"
description: "The .NET MAUI single project brings all the platform-specific experiences across Android, iOS, macOS, and Windows, into one shared head project."
ms.date: 06/10/2021
---

# Single project

.NET Multi-platform App UI (.NET MAUI) single project is a collection of features that brings all the platform-specific experiences you encounter while developing apps into one shared head project that can target Android, iOS, macOS, and Windows.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Single project is enabled using multi-targeting and the use of SDK-style projects in .NET 6. You can still expect access to all the platform-specific experiences and tools when you need them, while enjoying a simplified, shared development experience across all the platforms you're targeting.

## Simplify development

Single project is built on top of a collection of experiences that are being simplified in .NET 6. The following list shows the experiences that will be shared in .NET MAUI single project:

- Resources
  - Images
  - Fonts
  - App icons
  - Splash screens
  - Raw Assets
- App manifest
- NuGet
- Platform-specific code

All other features are being moved from their own platform-projects into platform folders in the single project.

## Visual Studio changes

In addition to the simplified, shared experiences in single project, there are changes being made to Visual Studio to support single project. These changes will enable the use of a shared resource file within the single project, platform files for platform-specific development, and a simplified debug target selection for running your .NET MAUI apps:

:::image type="content" source="single-project-images/example.png" alt-text="Single project screenshot.":::
