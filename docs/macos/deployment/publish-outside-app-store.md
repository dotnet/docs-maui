---
title: "Publish a .NET MAUI Mac Catalyst app for distribution outside the App Store"
description: "Learn how to provision and publish a .NET MAUI Mac Catalyst app for distribution outside the App Store."
ms.date: 03/16/2023
---

# Publish a Mac Catalyst app for distribution outside the App Store

An alternative to distributing Mac Catalyst apps through the App Store is to distribute them outside the App Store. With this approach, your Mac Catalyst app can be hosted on a server for download.

Distributing a Mac Catalyst app requires that the app is provisioned using a *provisioning profile*. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism.

To distribute a .NET Multi-platform App UI (.NET MAUI) Mac Catalyst app outside the App Store, you'll need to build a *distribution provisioning profile* specific to it. This profile enables the app to be digitally signed for release so that it can be installed on Macs. A distribution provisioning profile contains an App ID and a developer ID certificate. You'll need to create a developer ID certificate to identify yourself or your organization, if you don't already have one. In addition, you'll need to create developer ID installer certificate to sign your app's installer package.

The process for provisioning a Mac Catalyst app for distribution outside the App Store is as follows:

1. Create a certificate signing request. For more information, see [Create a certificate signing request](#create-a-certificate-signing-request).
1. Create a developer ID certificate. For more information, see [Create a developer ID certificate](#create-a-developer-id-certificate).
1. Create a developer ID installer certificate. For more information, see [Create a developer ID installer certificate](#create-a-developer-id-installer-certificate).
1. Create an App ID. For more information, see [Create an App ID](#create-an-app-id).
1. Configure the App ID. For more information, see [Configure the App ID](#configure-the-app-id). IS THIS REALLY REQUIRED???
1. Create a provisioning profile. For more information, see [Create a provisioning profile](#create-a-provisioning-profile).

[!INCLUDE [Create a certificate signing request](../includes/certificate-signing-request.md)]
