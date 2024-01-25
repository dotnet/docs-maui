---
title: "Upgrade from Xamarin to .NET"
description: "Learn how to upgrade Xamarin apps to .NET."
ms.date: 08/30/2023
---

# Upgrade from Xamarin to .NET

> [!IMPORTANT]
> [Microsoft support for Xamarin](https://dotnet.microsoft.com/platform/support/policy/xamarin) will end on May 1, 2024 for all Xamarin SDKs including Xamarin.Forms. Help us improve your upgrade experience from Xamarin to .NET MAUI by completing this [short survey](https://www.surveymonkey.com/r/VXH5TM7).

Xamarin projects can run on .NET after completing an upgrade process. The following table lists the Xamarin project types that can be upgraded to .NET:

| Project type            | Upgrade | Guide                                             |
|-------------------------|---------|---------------------------------------------------|
| Xamarin.Android         | ✅       | [Upgrade Xamarin native projects](native-projects.md) |
| Xamarin.iOS             | ✅       | [Upgrade Xamarin native projects](native-projects.md) |
| Xamarin.Mac             | ✅       | [Upgrade Xamarin native projects](native-projects.md) |
| Xamarin.tvOS            | ✅       | [Upgrade Xamarin native projects](native-projects.md) |
| Xamarin.Forms           | ✅       | [Upgrade a Xamarin.Forms app to a multi-project .NET MAUI app](multi-project-to-multi-project.md) <br> [Upgrade a Xamarin.Forms app to a single project .NET MAUI app](multi-project-to-single-project.md) |
| Xamarin.Forms UWP       | ✅       | [Xamarin.Forms UWP project migration](uwp-projects.md) |
| iOS App Extensions      | ✅       | [Upgrade Xamarin native projects](native-projects.md) |
| Android Wear            | ✅       | [Upgrade Xamarin native projects](native-projects.md) |
| Android Binding Library | ✅       | [Xamarin.Android binding project migration](android-binding-projects.md) |
| iOS Binding Library     | ✅       | [Xamarin.iOS binding project migration](ios-binding-projects.md) |
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

To upgrade your Xamarin native projects to .NET, you'll first have to update the projects to be SDK-style projects and then update your dependencies to .NET 8. For more information, see [Upgrade Xamarin.Android, Xamarin.iOS, and Xamarin.Mac projects to .NET](native-projects.md).

The .NET Upgrade Assistant is a command-line tool that can help you upgrade multi-project Xamarin.Forms apps to multi-project .NET Multi-platform App UI (.NET MAUI) apps. After running the tool, in most cases the app will require additional effort to complete the upgrade. For more information, see [Upgrade a Xamarin.Forms app to a .NET MAUI app with the .NET Upgrade Assistant](upgrade-assistant.md).

You can also manually upgrade at Xamarin.Forms app to a multi-project .NET MAUI app with a two-step process:

1. Upgrade your Xamarin native projects, in your Xamarin.Forms solution, to .NET. For more information, see [Upgrade Xamarin.Android, Xamarin.iOS, and Xamarin.Mac apps to .NET](native-projects.md). In addition, you can upgrade your Xamarin.Forms UWP project to a .NET MAUI WinUI 3 project. For more information, see [Xamarin.Forms UWP project migration](uwp-projects.md).
1. Upgrade your Xamarin.Forms library project to .NET Multi-platform App UI (.NET MAUI). For more information, see [Manually upgrade a Xamarin.Forms app to a multi-project .NET MAUI app](multi-project-to-multi-project.md).

Alternatively, you can manually upgrade a Xamarin.Forms app to a single-project .NET MAUI app. For more information, see [Manually upgrade a Xamarin.Forms app to a single project .NET MAUI app](multi-project-to-single-project.md).
