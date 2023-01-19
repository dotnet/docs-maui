---
title: "Publish a .NET MAUI app for iOS"
description: "Learn about distribution approaches for .NET MAUI iOS apps."
ms.date: 01/17/2023
---

# Publish a .NET MAUI app for iOS

> [!div class="op_single_selector"]
>
> - [Publish for Android](../../android/deployment/overview.md)
> - [Publish for iOS](index.md)
> - [Publish for macOS](../../macos/deployment/overview.md)
> - [Publish for Windows](../../windows/deployment/overview.md)

Once a .NET Multi-platform App UI (.NET MAUI) iOS app has been developed and tested, it can be distributed to users. The following diagram shows the steps required to produce the app package for distribution:

:::image type="content" source="media/ios-distribution.png" alt-text="Steps required to prepare an iOS app for distribution." border="false":::

You must have an active subscription to Apple's Developer Program before attempting to package an app for distribution. Apple offers two program options:

- **Apple Developer Program**. Regardless of whether you are an individual or represent an organization, the [Apple Developer Program](https://developer.apple.com/programs/) enables you to develop, test, and distribute apps.
- **Apple Developer Enterprise Program**, which is most suited to organizations that want to develop and distribute apps in-house only. Members of the [Apple Developer Enterprise Program](https://developer.apple.com/programs/enterprise/) do not have access to iTunes Connect, and apps created cannot be published to the App Store.

To register for either of these programs, you must first have an [Apple ID](https://appleid.apple.com/). Then you can visit the [Apple Developer Program](https://developer.apple.com/programs/enroll/) to register for a program.

Apple provides multiple approaches for distributing an iOS app:

- App Store. This is the main approach for distributing iOS apps to users. Apps are submitted to the App Store through a portal called *iTunes Connect*. Only developers who belong to the Apple Developer Program have access to this portal. Members of the Apple Developer Enterprise Program do not have access. All apps submitted to the App Store require approval from Apple. For more information, see [Provision an iOS app for App Store distribution](app-store.md).
- In-house. This distribution mechanism is also known as enterprise distribution. It enables members of the Apple Developer Enterprise Program to distribute apps internally to other members of the same organization. This has the advantage of not requiring an App Store review, and having no limit on the number of devices on which an app can be installed. However, members of the Apple Developer Enterprise Program don't have access to iTunes Connect, and therefore the licenses is responsible for distributing the app. For more information, see [Provision an iOS app for in-house distribution]().
- Ad-hoc. iOS apps can be user-tested via ad-hoc distribution, which is available for the Apple Developer Program and the Apple Developer Enterprise Program. It allows an app to be deployed on up to 100 devices, for testing. A use case for ad-hoc distribution is distribution within a company when iTunes Connect is not an option. For more information, see [Provision an iOS app for ad-hoc distribution](ad-hoc.md).
- Custom apps for business. Apple allows custom distribution of apps to businesses and education. For more information, see [Distributing Custom Apps](https://developer.apple.com/custom-apps/) on developer.apple.com and [Apple Business Manager User Guide](https://support.apple.com/guide/apple-business-manager/welcome/web) on support.apple.com.

All approaches require that apps are provisioned using an appropriate *provisioning profile*. Provisioning profiles contain code signing and app identity information, as well as the intended distribution mechanism. For non-App Store distribution, they also contain information about the devices the app can be deployed to.
