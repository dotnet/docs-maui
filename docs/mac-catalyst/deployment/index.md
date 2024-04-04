---
title: "Publish a .NET MAUI Mac Catalyst app"
description: "Learn about distribution approaches for .NET MAUI Mac Catalyst apps."
ms.date: 03/23/2023
---

# Publish a .NET MAUI Mac Catalyst app

> [!div class="op_single_selector"]
>
> - [Publish an unsigned app](publish-unsigned.md)
> - [Publish for app store distribution](publish-app-store.md)
> - [Publish outside the app store](publish-outside-app-store.md)
> - [Publish for ad-hoc distribution](publish-ad-hoc.md)

Once a .NET Multi-platform App UI (.NET MAUI) Mac Catalyst app has been developed and tested, it can be packaged for distribution as an *.app* or a *.pkg* file. An *.app* file is a self-contained app that can be run without installation, whereas a *.pkg* is an app packaged in an installer. The following diagram shows the steps required to produce an app package for distribution:

:::image type="content" source="media/maccat-distribution.png" alt-text="Steps required to prepare a .NET MAUI Mac Catalyst app for distribution." border="false":::

Publishing a .NET MAUI Mac Catalyst app builds on top of Apple's provisioning process, which requires you to have:

- Created an Apple ID. For more information, see [Create Your Apple ID](https://appleid.apple.com/account).
- Enrolled your Apple ID in the Apple Developer Program, which you have to pay to join. Enrolling in the Apple Developer Program enables you to create a *provisioning profile*, which contains code signing information.
- A Mac on which you can build your app.

Apple offers two developer program options:

- *Apple Developer Program*. Regardless of whether you are an individual or represent an organization, the [Apple Developer Program](https://developer.apple.com/programs/) enables you to develop, test, and distribute apps.
- *Apple Developer Enterprise Program*, which is most suited to organizations that want to develop and distribute apps in-house only. Members of the [Apple Developer Enterprise Program](https://developer.apple.com/programs/enterprise/) do not have access to App Store Connect, and apps can't be published to the Mac App Store.

> [!NOTE]
> To register for either of these programs, you must first have an [Apple ID](https://appleid.apple.com/). Then you can visit the [Apple Developer Program](https://developer.apple.com/programs/enroll/) to register for a program.

Apple provides multiple approaches for distributing a Mac Catalyst app:

- *Mac App Store*. This is the main approach for distributing Mac Catalyst apps to users. Apps are submitted to the Mac App Store through on online tool called *App Store Connect*. Only developers who belong to the Apple Developer Program have access to this tool. Members of the Apple Developer Enterprise Program do not have access. All apps submitted to the Mac App Store require approval from Apple. For more information, see [Publish a Mac Catalyst app for Mac App Store distribution](publish-app-store.md).
- *Outside the Mac App Store*. This distribution mechanism enables Mac Catalyst apps to be distributed outside the Mac App Store. It's available for the Apple Developer Program and the Apple Developer Enterprise Program, and enables your Mac Catalyst app to be downloaded from a location of your choosing. For more information, see [Publish a Mac Catalyst app for distribution outside the Mac App Store](publish-outside-app-store.md).
- *Ad-hoc*. Mac Catalyst apps can be user-tested via ad-hoc distribution, which is available for the Apple Developer Program and the Apple Developer Enterprise Program. It allows an app to be deployed on up to 100 devices, for testing. For more information, see [Publish a Mac Catalyst app for ad-hoc distribution](publish-ad-hoc.md).

All approaches require that apps are provisioned using an appropriate *provisioning profile*. Provisioning profiles contain code signing and app identity information, as well as the intended distribution mechanism. For ad-hoc distribution, they also contain information about the devices the app can be deployed to. In addition, Mac Catalyst apps that are distributed outside the Mac App Store must be notarized by Apple.

> [!IMPORTANT]
> When distributing a Blazor Hybrid app, the host platform must have a WebView. For more information, see [Keep the Web View current in deployed Blazor Hybrid apps](/aspnet/core/blazor/hybrid/security/security-considerations#keep-the-web-view-current-in-deployed-apps).
