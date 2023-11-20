---
title: "Publish a .NET MAUI app for Android"
description: "Learn how to package and publish an Android .NET MAUI app."
ms.date: 04/05/2023
---

# Publish a .NET MAUI app for Android

> [!div class="op_single_selector"]
>
> - [Publish for Google Play distribution](publish-google-play.md)
> - [Publish for ad-hoc distribution](publish-ad-hoc.md)
> - [Publish using the command line](publish-cli.md)

The final step in the development of a .NET Multi-platform App UI (.NET MAUI) app is to publish it. Publishing is the process of creating a package that contains the app and is ready for users to install on their devices. Publishing involve two essential tasks:

- **Preparing for deployment**. A release version of the app is created that can be deployed to Android devices.
- **Distribution**. The release version of an app is made available through one or more of the various distribution channels.

The following diagram illustrates the steps involved with publishing a .NET MAUI Android app:

:::image type="content" source="media/build-and-deploy-steps.png" alt-text="Build and deploy flowchart for .NET MAUI Android apps.":::

> [!IMPORTANT]
> When publishing your .NET MAUI app for Android, you generate an Android Package (APK) or an Android App Bundle (AAB) file. The APK is used for installing your app to an Android device, and the AAB is used to publish your app to Google Play.

As can be seen in the diagram above, preparing for deployment is identical regardless of the distribution method that's used. There are several ways that an Android app can be released to users:

- **Through a market** &ndash; There are multiple Android marketplaces that exist for distribution, with the most well known being [Google Play](https://play.google.com/).
- **Via a website** &ndash; A .NET MAUI app can be made available for download on a website, from which users may then install the app by clicking on a link.
- **Via a file share** &ndash; Similar to a website, as long as the app package is available to the user, they can side-load it on their device.

Using an established marketplace is the most common way to publish an app as it provides the broadest market reach and the greatest control over distribution. However, publishing an app through a marketplace requires extra effort.

Multiple channels can distribute a .NET MAUI app simultaneously. For example, an app could be published on Google Play, and also be downloaded from a web server.

Making your app available for direct download is most useful for a controlled subset of users, such as an enterprise environment or an app that is only meant for a small or well-specified set of users. Server and email distribution are also simpler publishing models, requiring less preparation to publish an app, though apps may be blocked as an email attachment.

Google Play is the most comprehensive and popular marketplace for Android apps. Google Play allows users to discover, download, rate, and pay for apps by clicking a single icon either on their device or on their computer. Google Play also provides tools to help in the analysis of sales and market trends and to control which devices and users may download an app.

> [!IMPORTANT]
> When distributing a Blazor Hybrid app, the host platform must have a WebView. For more information, see [Keep the Web View current in deployed Blazor Hybrid apps](/aspnet/core/blazor/hybrid/security/security-considerations#keep-the-web-view-current-in-deployed-apps).

## See also

<!--
- [Build Process](~/android/deploy-test/building-apps/build-process.md)
- [Obtaining A Google Maps API Key](~/android/platform/maps-and-location/maps/obtaining-a-google-maps-api-key.md)
- [Deploy via Visual Studio App Center](/appcenter/distribution/stores/googleplay)
- [Application Signing](https://source.android.com/security/apksigning/)
-->

- [Publishing on Google Play](https://developer.android.com/distribute/googleplay/publish/index.html)
- [Google Application Licensing](https://developer.android.com/guide/google/play/licensing/index.html)
- [Linking](~/android/linking.md)
