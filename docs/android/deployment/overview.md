---
title: "Publish a .NET MAUI app for Android"
description: "Learn how to package and publish an Android .NET MAUI app."
ms.date: 10/07/2022
---

# Publish a .NET MAUI app for Android

> [!div class="op_single_selector"]
>
> - [Publish for Android](overview.md)
> - [Publish for iOS](../../ios/deployment/overview.md)
> - [Publish for macOS](../../macos/deployment/overview.md)
> - [Publish for Windows](../../windows/deployment/overview.md)

The final step in the development of a .NET MAUI app is to publish it. Publishing is the process of creating a package that contains the app and is ready for users to install on their devices. Packaging and deployment involve two essential tasks:

- **Preparing for publication**

  A release version of the app is created that can be deployed to Android devices.

- **Distribution**

  The release version of an app is made available through one or more of the various distribution channels.

The following diagram illustrates the steps involved with publishing a .NET MAUI app:

:::image type="content" source="media/overview/build-and-deploy-steps.svg" alt-text="Build and deploy flowchart":::

As can be seen by the diagram above, the preparation is the same regardless of the distribution method that is used. There are several ways that an Android app may be released to users:

- **Via a website** &ndash; A .NET MAUI app can be made available for download on a website, from which users may then install the app by clicking on a link.
- **Via a file share** &ndash; Similar to a website, as long as the app package is available to the user, they can side-load it on their device.
- **Through a market** &ndash; There are several Android marketplaces that exist for distribution, such as  [Google Play](https://play.google.com/) or [Amazon App Store for Android](https://www.amazon.com/mobile-apps/b?ie=UTF8&node=2350149011).

Using an established marketplace is the most common way to publish an app as it provides the broadest market reach and the greatest control over distribution. However, publishing an app through a marketplace requires extra effort.

Multiple channels can distribute a .NET MAUI app simultaneously. For example, an app could be published on Google Play, the Amazon App Store for Android, and also be downloaded from a web server.

Making your app available for direct download is most useful for a controlled subset of users, such as an enterprise environment or an app that is only meant for a small or well-specified set of users. Server and email distribution are also simpler publishing models, requiring less preparation to publish an app, though apps may be blocked as an email attachment.

The Amazon Mobile App Distribution Program enables mobile app developers to distribute and sell their applications on Amazon. Users can discover and shop for apps on their Android devices by using the Amazon App Store application.

Google Play is arguably the most comprehensive and popular marketplace for Android applications. Google Play allows users to discover, download, rate, and pay for applications by clicking a single icon either on their device or on their computer. Google Play also provides tools to help in the analysis of sales and market trends and to control which devices and users may download an application.

## See also

This section links to articles that may help in publishing an app to an app store such as Google Play.

<!--
- [Build Process](~/android/deploy-test/building-apps/build-process.md)
- [Linking](~/android/deploy-test/linker.md)
- [Obtaining A Google Maps API Key](~/android/platform/maps-and-location/maps/obtaining-a-google-maps-api-key.md)
- [Deploy via Visual Studio App Center](/appcenter/distribution/stores/googleplay)
- [Application Signing](https://source.android.com/security/apksigning/)
-->

### Google Play app store

- [Publishing on Google Play](https://developer.android.com/distribute/googleplay/publish/index.html)
- [Google Application Licensing](https://developer.android.com/guide/google/play/licensing/index.html)

### Amazon app store

- [Mobile App Distribution Portal](https://developer.amazon.com/welcome.html)
- [Amazon Mobile App Distribution FAQ](https://developer.amazon.com/help/faq.html)
