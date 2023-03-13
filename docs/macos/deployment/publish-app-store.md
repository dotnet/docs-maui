---
title: "Publish a .NET MAUI Mac Catalyst app for App Store distribution"
description: "Learn how to provision and publish a .NET MAUI Mac Catalyst app for App Store distribution."
ms.date: 03/13/2023
---

# Publish a Mac Catalyst app for App Store distribution

The most common approach to distributing Mac Catalyst apps to users is through the App Store. Apps are submitted to the App Store through a portal called *App Store Connect*. Only developers who belong to the Apple Developer Program have access to this portal. Members of the Apple Developer Enterprise Program do not have access. All apps submitted to the App Store require approval from Apple.

Distributing a Mac Catalyst app requires that the app is provisioned using a *provisioning profile*. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism.

To distribute a .NET Multi-platform App UI (.NET MAUI) Mac Catalyst app, you'll need to build a *distribution provisioning profile* specific to it. This profile enables the app to be digitally signed for release so that it can be installed . A distribution provisioning profile contains an App ID and a distribution certificate. You can use the same App ID that you used when deploying your app to a device for testing. However, you will need to create a distribution certificate to identify yourself or your organization, if you don't already have one.

The process for creating an App Store distribution provisioning profile is as follows:

1. Create a certificate signing request. For more information, see [Create a certificate signing request](#create-a-certificate-signing-request).
1. Create a distribution certificate. For more information, see [Create a distribution certificate](#create-a-distribution-certificate).
1. Create an installer certificate. For more information, see [Create an installer certificate](#create-an-installer-certificate).
1. Create an App ID. For more information, see [Create an App ID](#create-an-app-id).
1. Configure the App ID. For more information, see [Configure the App ID](#configure-the-app-id).
1. Create a provisioning profile. For more information, see [Create a provisioning profile](#create-a-provisioning-profile). -->

[!INCLUDE [Create a certificate signing request](../includes/certificate-signing-request.md)]

[!INCLUDE [Create a distribution certificate](../includes/distribution-certificate.md)]

[!INCLUDE [Create an installer certificate](../includes/installer-certificate.md)]

[!INCLUDE [Create an App ID](../includes/create-app-id.md)]

[!INCLUDE [Configure the App ID](../includes/configure-app-id.md)]
