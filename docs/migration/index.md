---
title: "Upgrade from Xamarin to .NET"
description: "Learn how to upgrade Xamarin apps to .NET starting with .NET 6"
ms.date: 02/15/2023
---

# Upgrade from Xamarin to .NET

Xamarin projects can run on .NET, starting with .NET 6, after completing an upgrade process. The following table lists the Xamarin project types that can be upgraded to .NET:

| Project type            | Upgrade | Guide                                             |
|-------------------------|---------|---------------------------------------------------|
| Xamarin.Android         | ✅       | [Upgrade Xamarin native projects](native-projects.md) |
| Xamarin.iOS             | ✅       | [Upgrade Xamarin native projects](native-projects.md) |
| Xamarin.Mac             | ✅       | [Upgrade Xamarin native projects](native-projects.md) |
| Xamarin.tvOS            | ✅       | [Upgrade Xamarin native projects](native-projects.md) |
| Xamarin.Forms           | ✅       | [Upgrade Xamarin.Forms projects](forms-projects.md) |
| iOS App Extensions      | ✅       | [Upgrade Xamarin native projects](native-projects.md) |
| Android Wear            | ✅       | [Upgrade Xamarin native projects](native-projects.md) |
| Android Binding Library | ✅       | Upgrade to SDK Style projects                     |
| iOS Binding Library     | ✅       | Upgrade to SDK Style projects                     |
| SpriteKit               | ✅       | [Upgrade Xamarin native projects](native-projects.md) |
| SceneKit                | ✅       | [Upgrade Xamarin native projects](native-projects.md) |
| Metal                   | ✅       | [Upgrade Xamarin native projects](native-projects.md) |
| OpenGL                  | ❌ (iOS) | Removed from iOS since OpenTK isn't available |
| Xamarin.watchOS         | ❌       | Recommendation: bundle Swift extensions with .NET for iOS apps |

<!-- markdownlint-disable MD032 -->
> [!IMPORTANT]
> To upgrade an app from Xamarin to .NET:
> - All projects **do** need to become SDK-style.
> - Projects **don't** need to be rewritten.
> - Multi-project solutions **don't** need to become a multi-targeted single project.
<!-- markdownlint-enable MD025 -->

To upgrade your Xamarin native projects to .NET, you'll first have to update the projects to be SDK-style projects and then update your dependencies to .NET 6+. For more information, see [Upgrade Xamarin.Android, Xamarin.iOS, and Xamarin.Mac apps to .NET](native-projects.md).

The .NET Upgrade Assistant is a command-line tool that can help you upgrade Xamarin.Forms projects to .NET Multi-platform App UI (.NET MAUI). After running the tool, in most cases the app will require additional effort to complete the upgrade. For more information, see [Upgrade a Xamarin.Forms app to .NET MAUI with the .NET Upgrade Assistant](upgrade-assistant.md).

Alternatively, you can manually upgrade a Xamarin.Forms project to .NET MAUI with a two-step process:

1. Upgrade your Xamarin native projects, in your Xamarin.Forms solution, to .NET. For more information, see [Upgrade Xamarin.Android, Xamarin.iOS, and Xamarin.Mac apps to .NET](native-projects.md).
1. Upgrade your Xamarin.Forms library project to .NET Multi-platform App UI (.NET MAUI). For more information, see [Manually upgrade a Xamarin.Forms app to .NET MAUI](forms-projects.md).
