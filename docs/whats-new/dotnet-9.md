---
title: What's new in .NET MAUI for .NET 9
description: Learn about the new features introduced in .NET MAUI for .NET 9.
ms.date: 02/14/2024
---

# What's new in .NET MAUI for .NET 9

The focus of .NET Multi-platform App UI (.NET MAUI) in .NET 9 is to improve product quality. This includes expanding test coverage, end to end scenario testing, and bug fixing. For more information about the product quality improvements in .NET MAUI 9 Preview 1, see the [release notes](https://github.com/dotnet/maui/releases/tag/untagged-e764fa1780e2dd618900).

> [!IMPORTANT]
> Due to working with external dependencies, such as Xcode or Android SDK Tools, the .NET MAUI support policy differs from the [.NET and .NET Core support policy](https://dotnet.microsoft.com/platform/support/policy/maui). For more information, see [.NET MAUI support policy](https://dotnet.microsoft.com/platform/support/policy/maui).

In .NET 8 and .NET 9, .NET MAUI ships as a .NET workload and multiple NuGet packages. The advantage of this approach is that it enables you to easily pin your projects to specific versions, while also enabling you to easily preview unreleased or experimental builds. When you create a new .NET MAUI project the required NuGet packages are automatically added to the project.

> [!NOTE]
> Building `net8.0-*` target frameworks from .NET 9 doesn't work in Preview 1.

For information about what's new in .NET 9, see [What's new in .NET 9](/dotnet/core/whats-new/dotnet-9).

## Android

.NET Android 9 Preview 1, which uses API 34 and JDK 17, includes foundational work to reduce build times, and to improve the trimability of apps to reduce size and improve performance. For more information about .NET Android 9 Preview 1, see the [release notes](https://github.com/xamarin/xamarin-android/releases/tag/untagged-63f17517f7acc87fb628).

## iOS

.NET 9 Preview 1 on iOS, tvOS, Mac Catalyst, and macOS uses Xcode 15.2 for the following platform versions:

- iOS: 17.2
- tvOS: 17.2
- Mac Catalyst: 17.2
- macOS: 14.2

For more information about .NET 9 Preview 1 on iOS, tvOS, Mac Catalyst, and macOS, see the [release notes](https://github.com/xamarin/xamarin-macios/releases/tag/untagged-971754610a7cf7b0e8a6).

## See also

- [.NET MAUI updates in .NET 9 Preview 1](https://github.com/dotnet/core/tree/main/release-notes/9.0/preview1/dotnetmaui.md).
- [Announcing .NET 9 Preview 1](https://devblogs.microsoft.com/dotnet/announcing-dotnet-9-preview-1)
