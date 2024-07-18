---
title: "Migrate Azure Notification Hub code from Xamarin.Forms to .NET MAUI"
description: "Learn how to migrate your Azure Notification Hub code from a Xamarin.Forms app to a .NET MAUI app."
ms.date: 07/17/2024
---

# Migrate Azure Notification Hub code from Xamarin.Forms to .NET MAUI

In Xamarin.Forms apps, the [Xamarin.Azure.NotificationHubs.iOS](https://www.nuget.org/packages/Xamarin.Azure.NotificationHubs.iOS) and [Xamarin.Azure.NotificationHubs.Android](https://www.nuget.org/packages/Xamarin.Azure.NotificationHubs.Android) NuGet packages could be used to register for push notifications with Azure Notification Hubs. However, these NuGet packages are no longer supported and you must take an alternative approach to performing device installation with an Azure Notification Hub from a .NET Multi-platform App UI (.NET MAUI) app.

One possible approach is to use the Azure Notification Hubs REST APIs to manage installations and send notifications. This can be accomplished from a backend service, or directly from a device. For more information, see [How to use the Notification Hubs REST Interface](/rest/api/notificationhubs/use-notification-hubs-rest-interface).

Alternatively, a .NET wrapper around the REST APIs is available via the [Microsoft.Azure.NotificationHubs](https://www.nuget.org/packages/Microsoft.Azure.NotificationHubs) NuGet package. This NuGet package can be used to handle device installation for a .NET MAUI app, and to initiate a push notification. For more information on this approach, see [Registration management from a backend](/azure/notification-hubs/notification-hubs-push-notification-registration-management#registration-management-from-a-backend).

> [!IMPORTANT]
> Specific Azure Notification Hub SDKs aren't provided for .NET for Android, .NET for iOS, and .NET MAUI. Instead, a [.NET SDK](https://www.nuget.org/packages/Microsoft.Azure.NotificationHubs) can be used by apps built with .NET.

For information on using [Azure Notification Hubs](/azure/notification-hubs/notification-hubs-push-notification-overview) to send push notifications to a .NET MAUI app targeting Android and iOS, see [Send push notifications to .NET MAUI apps using Azure Notification Hubs via a backend service](~/data-cloud/push-notifications.md).

## Firebase Cloud Messaging support

Google deprecated the Firebase Cloud Messaging (FCM) legacy API for HTTP and XMPP on June 20, 2023, and apps that use this API should migrate to the HTTP v1 API at the earliest opportunity. For more information, see [Migrate from legacy FCM APIs to HTTP v1](https://firebase.google.com/docs/cloud-messaging/migrate-v1) on firebase.google.com.

If you have users who've already registered for push notifications via the legacy FCM API they'll need migrating to the FCMv1 API. When you send a notification through FCMv1, the notification will only be sent to devices registered through the FCMv1 API. Therefore, it's recommended to migrate to FCMv1 and ensure that all new device registrations occur through FCMv1. For existing registrations, you'll need to re-register all active device tokens through FCMv1.

Azure Notification Hubs provide support for the FCMv1 API. For more information, see [Azure Notification Hubs and Google Firebase Cloud Messaging migration](/azure/notification-hubs/notification-hubs-gcm-to-fcm).
