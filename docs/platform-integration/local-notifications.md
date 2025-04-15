---
title: "Local notifications"
description: "Learn how to send, schedule, and receive local notifications in .NET MAUI"
ms.date: 06/18/2024
zone_pivot_groups: devices-platforms
---

# Local notifications

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-local-notifications)

Local notifications are alerts sent by apps installed on a device. Local notifications are often used for features such as:

- Calendar events
- Reminders
- Location-based triggers

Each platform requires its own native code implementation to send and receive local notifications. However, each platform implementation can be abstracted at the cross-platform layer so that there's a consistent API to send and receive local notifications in a .NET Multi-platform App UI (.NET MAUI) app.

## Create a cross-platform abstraction

A .NET MAUI app should send and receive local notifications without concern for the underlying platform implementations. The following `INotificationManagerService` interface is used to define a cross-platform API that can be used to interact with local notifications:

```csharp
public interface INotificationManagerService
{
    event EventHandler NotificationReceived;
    void SendNotification(string title, string message, DateTime? notifyTime = null);
    void ReceiveNotification(string title, string message);
}
```

This interface defines the following operations:

- The `NotificationReceived` event allows an app to handle incoming notifications.
- The `SendNotification` method sends a notification at an optional `DateTime`.
- The `ReceiveNotification` method processes a notification when received by the underlying platform.

The interface should be implemented on each platform that you want to support local notifications.

:::zone pivot="devices-android"

## Implement local notifications on Android

On Android, a local notification is a message that's displayed outside your app's UI to provide reminders or other information from your app. Users can tap the notification to open your app, or can optionally take an action directly from the notification. For information about local notifications on Android, see [Notifications overview](https://developer.android.com/develop/ui/views/notifications) on developer.android.com.

For a .NET MAUI app to send and receive notifications on Android, the app must provide an implementation of the `INotificationManagerService` interface.

### Send and receive local notifications

On Android, the `NotificationManagerService` class implements the `INotificationManagerService` interface, and contains the logic to send and receive local notifications:

```csharp
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;

namespace LocalNotificationsDemo.Platforms.Android;

public class NotificationManagerService : INotificationManagerService
{
    const string channelId = "default";
    const string channelName = "Default";
    const string channelDescription = "The default channel for notifications.";

    public const string TitleKey = "title";
    public const string MessageKey = "message";

    bool channelInitialized = false;
    int messageId = 0;
    int pendingIntentId = 0;

    NotificationManagerCompat compatManager;

    public event EventHandler NotificationReceived;

    public static NotificationManagerService Instance { get; private set; }

    public NotificationManagerService()
    {
        if (Instance == null)
        {
            CreateNotificationChannel();
            compatManager = NotificationManagerCompat.From(Platform.AppContext);
            Instance = this;
        }
    }

    public void SendNotification(string title, string message, DateTime? notifyTime = null)
    {
        if (!channelInitialized)
        {
            CreateNotificationChannel();
        }

        if (notifyTime != null)
        {
            Intent intent = new Intent(Platform.AppContext, typeof(AlarmHandler));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);
            intent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTop);

            var pendingIntentFlags = (Build.VERSION.SdkInt >= BuildVersionCodes.S)
                ? PendingIntentFlags.CancelCurrent | PendingIntentFlags.Immutable
                : PendingIntentFlags.CancelCurrent;

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Platform.AppContext, pendingIntentId++, intent, pendingIntentFlags);
            long triggerTime = GetNotifyTime(notifyTime.Value);
            AlarmManager alarmManager = Platform.AppContext.GetSystemService(Context.AlarmService) as AlarmManager;
            alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
        }
        else
        {
            Show(title, message);
        }
    }

    public void ReceiveNotification(string title, string message)
    {
        var args = new NotificationEventArgs()
        {
            Title = title,
            Message = message,
        };
        NotificationReceived?.Invoke(null, args);
    }

    public void Show(string title, string message)
    {
        Intent intent = new Intent(Platform.AppContext, typeof(MainActivity));
        intent.PutExtra(TitleKey, title);
        intent.PutExtra(MessageKey, message);
        intent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTop);

        var pendingIntentFlags = (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            ? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
            : PendingIntentFlags.UpdateCurrent;

        PendingIntent pendingIntent = PendingIntent.GetActivity(Platform.AppContext, pendingIntentId++, intent, pendingIntentFlags);
        NotificationCompat.Builder builder = new NotificationCompat.Builder(Platform.AppContext, channelId)
            .SetContentIntent(pendingIntent)
            .SetContentTitle(title)
            .SetContentText(message)
            .SetLargeIcon(BitmapFactory.DecodeResource(Platform.AppContext.Resources, Resource.Drawable.dotnet_logo))
            .SetSmallIcon(Resource.Drawable.message_small);

        Notification notification = builder.Build();
        compatManager.Notify(messageId++, notification);  
    }

    void CreateNotificationChannel()
    {
        // Create the notification channel, but only on API 26+.
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channelNameJava = new Java.Lang.String(channelName);
            var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
            {
                Description = channelDescription
            };
            // Register the channel
            NotificationManager manager = (NotificationManager)Platform.AppContext.GetSystemService(Context.NotificationService);
            manager.CreateNotificationChannel(channel);
            channelInitialized = true;
        }
    }

    long GetNotifyTime(DateTime notifyTime)
    {
        DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
        double epochDiff = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
        long utcAlarmTime = utcTime.AddSeconds(-epochDiff).Ticks / 10000;
        return utcAlarmTime; // milliseconds
    }
}
```

The `NotificationManagerService` class should be placed in your app's *Platforms > Android* folder. Alternatively, multi-targeting can be performed based on your own filename and folder criteria, rather than using the *Platforms > Android* folder. For more information, see [Configure multi-targeting](configure-multi-targeting.md).

Android allows apps to define multiple channels for notifications. The `NotificationManagerService` constructor creates a basic channel that's used to send notifications. The `SendNotification` method defines the platform-specific logic required to create and send a notification. The `ReceiveNotification` method is called by Android OS when a message is received, and invokes the `NotificationReceived` event handler. For more information, see [Create a notification](https://developer.android.com/develop/ui/views/notifications/build-notification) on developer.android.com.

The `SendNotification` method creates a local notification immediately, or at an exact `DateTime`. A notification can be scheduled for an exact `DateTime` using the `AlarmManager` class, and the notification will be received by an object that derives from the `BroadcastReceiver` class:

```csharp
[BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
public class AlarmHandler : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        if (intent?.Extras != null)
        {
            string title = intent.GetStringExtra(NotificationManagerService.TitleKey);
            string message = intent.GetStringExtra(NotificationManagerService.MessageKey);

            NotificationManagerService manager = NotificationManagerService.Instance ?? new NotificationManagerService();
            manager.Show(title, message);
        }
    }
}
```

> [!IMPORTANT]
> By default, notifications scheduled using the `AlarmManager` class won't survive device restart. However, you can design your app to automatically reschedule notifications if the device is restarted. For more information, see [Start an alarm when the device restarts](https://developer.android.com/training/scheduling/alarms#boot) in [Schedule repeating alarms](https://developer.android.com/training/scheduling/alarms) on developer.android.com. For information about background processing on Android, see [Guide to background processing](https://developer.android.com/guide/background) on developer.android.com.

### Handle incoming notifications

The Android app must detect incoming notifications and notify the `NotificationManagerService` instance. One way of achieving this is by modifying the `MainActivity` class.

The `Activity` attribute on the `MainActivity` class should specify a `LaunchMode` value of `LaunchMode.SingleTop`:

```csharp
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, //... )]
public class MainActivity : MauiAppCompatActivity
{
}
```

The `SingleTop` mode prevents multiple instances of an `Activity` from being started while the app is in the foreground. This `LaunchMode` may not be appropriate for apps that launch multiple activities in more complex notification scenarios. For more information about `LaunchMode` enumeration values, see [Android Activity LaunchMode](https://developer.android.com/guide/topics/manifest/activity-element#lmode) on developer.android.com.

The `MainActivity` class also needs to be modified to receive incoming notifications:

```csharp
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        CreateNotificationFromIntent(Intent);
    }

    protected override void OnNewIntent(Intent? intent)
    {
        base.OnNewIntent(intent);

        CreateNotificationFromIntent(intent);
    }

    static void CreateNotificationFromIntent(Intent intent)
    {
        if (intent?.Extras != null)
        {
            string title = intent.GetStringExtra(LocalNotificationsDemo.Platforms.Android.NotificationManagerService.TitleKey);
            string message = intent.GetStringExtra(LocalNotificationsDemo.Platforms.Android.NotificationManagerService.MessageKey);

            var service = IPlatformApplication.Current.Services.GetService<INotificationManagerService>();
            service.ReceiveNotification(title, message);
        }
    }
}
```

The `CreateNotificationFromIntent` method extracts notification data from the `intent` argument and passes it to the `ReceiveNotification` method in the `NotificationManagerService` class. The `CreateNotificationFromIntent` method is called from both the `OnCreate` method and the `OnNewIntent` method:

- When the app is started by notification data, the `Intent` data will be passed to the `OnCreate` method.
- If the app is already in the foreground, the `Intent` data will be passed to the `OnNewIntent` method.

### Check for permission

Android 13 (API 33) and higher requires the `POST_NOTIFICATIONS` permission for sending notifications from an app. This permission should be declared in your *AndroidManifest.xml* file:

```xml
<uses-permission android:name="android.permission.POST_NOTIFICATIONS" />
```

For more information about the `POST_NOTIFICATIONS` permission, see [Notification runtime permission](https://developer.android.com/develop/ui/views/notifications/notification-permission) on developer.android.com.

You should implement a permission class that checks for the permission declaration at runtime:

```csharp
using Android;

namespace LocalNotificationsDemo.Platforms.Android;

public class NotificationPermission : Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions
    {
        get
        {
            var result = new List<(string androidPermission, bool isRuntime)>();
            if (OperatingSystem.IsAndroidVersionAtLeast(33))
                result.Add((Manifest.Permission.PostNotifications, true));
            return result.ToArray();
        }
    }
}
```

After the app has launched you should request the user to grant this permission before attempting to send a local notification:

```csharp
PermissionStatus status = await Permissions.RequestAsync<NotificationPermission>();
```

For more information about .NET MAUI permissions, see [Permissions](~/platform-integration/appmodel/permissions.md).

:::zone-end

:::zone pivot="devices-ios,devices-maccatalyst"

## Implement local notifications on iOS and Mac Catalyst

On Apple platforms, a local notification is a message that conveys important information to users. The system handles delivery of notifications based on a specified time or location. For information about local notifications on Apple platforms, see [Scheduling a notification locally from your app](https://developer.apple.com/documentation/usernotifications/scheduling-a-notification-locally-from-your-app) on developer.apple.com.

For a .NET MAUI app to send and receive notifications on Apple platforms, the app must provide an implementation of the `INotificationManagerService` interface.

### Send and receive local notifications

On Apple platforms, the `NotificationManagerService` class implements the `INotificationManagerService` interface, and contains the logic to send and receive local notifications:

```csharp
using Foundation;
using UserNotifications;

namespace LocalNotificationsDemo.Platforms.iOS;

public class NotificationManagerService : INotificationManagerService
{
    int messageId = 0;
    bool hasNotificationsPermission;

    public event EventHandler? NotificationReceived;

    public NotificationManagerService()
    {
        // Create a UNUserNotificationCenterDelegate to handle incoming messages.
        UNUserNotificationCenter.Current.Delegate = new NotificationReceiver();

        // Request permission to use local notifications.
        UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
        {
            hasNotificationsPermission = approved;
        });
    }

    public void SendNotification(string title, string message, DateTime? notifyTime = null)
    {
        // App doesn't have permissions.
        if (!hasNotificationsPermission)
            return;

        messageId++;
        var content = new UNMutableNotificationContent()
        {
            Title = title,
            Subtitle = "",
            Body = message,
            Badge = 1
        };

        UNNotificationTrigger trigger;
        if (notifyTime != null)
            // Create a calendar-based trigger.
            trigger = UNCalendarNotificationTrigger.CreateTrigger(GetNSDateComponents(notifyTime.Value), false);
        else
            // Create a time-based trigger, interval is in seconds and must be greater than 0.
            trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.25, false);

        var request = UNNotificationRequest.FromIdentifier(messageId.ToString(), content, trigger);
        UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
        {
            if (err != null)
                throw new Exception($"Failed to schedule notification: {err}");
        });
    }

    public void ReceiveNotification(string title, string message)
    {
        var args = new NotificationEventArgs()
        {
            Title = title,
            Message = message
        };
        NotificationReceived?.Invoke(null, args);
    }

    NSDateComponents GetNSDateComponents(DateTime dateTime)
    {
        return new NSDateComponents
        {
            Month = dateTime.Month,
            Day = dateTime.Day,
            Year = dateTime.Year,
            Hour = dateTime.Hour,
            Minute = dateTime.Minute,
            Second = dateTime.Second
        };
    }
}
```

The `NotificationManagerService` class should be placed in your app's *Platforms > iOS*  or *Platforms > Mac Catalyst* folder. Alternatively, multi-targeting can be performed based on your own filename and folder criteria, rather than using the *Platforms* folders. For more information, see [Configure multi-targeting](configure-multi-targeting.md).

On Apple platforms, you must request permission to use notifications before attempting to schedule a notification. This occurs in the `NotificationManagerService` constructor. For more information about local notification permission,, see [Asking permission to use notifications](https://developer.apple.com/documentation/usernotifications/asking-permission-to-use-notifications) on developer.apple.com.

The `SendNotification` method defines the logic required to create and send a notification, and creates an immediate local notification using a `UNTimeIntervalNotificationTrigger` object, or at an exact `DateTime` using a `UNCalendarNotificationTrigger` object. The `ReceiveNotification` method is called by iOS when a message is received, and invokes the `NotificationReceived` event handler.

### Handle incoming notifications

On Apple platforms, to handle incoming messages you must create a delegate that subclasses `UNUserNotificationCenterDelegate`:

```csharp
using UserNotifications;

namespace LocalNotificationsDemo.Platforms.iOS;

public class NotificationReceiver : UNUserNotificationCenterDelegate
{
    // Called if app is in the foreground.
    public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
    {
        ProcessNotification(notification);

        var presentationOptions = (OperatingSystem.IsIOSVersionAtLeast(14))
            ? UNNotificationPresentationOptions.Banner
            : UNNotificationPresentationOptions.Alert;

        completionHandler(presentationOptions);
    }

    // Called if app is in the background, or killed state.
    public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
    {
        if (response.IsDefaultAction)
            ProcessNotification(response.Notification);

        completionHandler();
    }

    void ProcessNotification(UNNotification notification)
    {
        string title = notification.Request.Content.Title;
        string message = notification.Request.Content.Body;

        var service = IPlatformApplication.Current?.Services.GetService<INotificationManagerService>();
        service?.ReceiveNotification(title, message);
    }
}
```

The `NotificationReceiver` class is registered as the `UNUserNotificationCenter` delegate in the `NotificationManagerService` constructor, and provides incoming notification data to the `ReceiveNotification` method in the `NotificationManagerService` class.

:::zone-end

:::zone pivot="devices-windows"

## Implement local notifications on Windows

Local notifications in the Windows App SDK are messages that your app can send to your user while they are not currently inside your app. The notification content is displayed in a transient window in the bottom right corner of the screen and in the Notification Center. Local notifications can be used to inform the user of app status, or to prompt the user to take an action.

For information about local notifications on Windows, including implementation details for packaged and unpackaged apps, see [App notifications overview](/windows/apps/windows-app-sdk/notifications/app-notifications/).

> [!WARNING]
> Currently, scheduled notifications aren't supported in the Windows App SDK. For more information, see [Feature Request: Schedule toast notifications](https://github.com/microsoft/WindowsAppSDK/issues/5050).

:::zone-end

## Register platform implementations

Each `NotificationManagerService` implementation should be registered against the `INotificationManagerService` interface, so that the operations the interface exposes can be called from cross-platform code. This can be achieved by registering the types with the <xref:Microsoft.Maui.Hosting.MauiAppBuilder.Services> property of the <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object in the `CreateMauiApp` method in the `MauiProgram` class:

```csharp
#if ANDROID
            builder.Services.AddTransient<INotificationManagerService, LocalNotificationsDemo.Platforms.Android.NotificationManagerService>();
#elif IOS
            builder.Services.AddTransient<INotificationManagerService, LocalNotificationsDemo.Platforms.iOS.NotificationManagerService>();
#elif MACCATALYST
            builder.Services.AddTransient<INotificationManagerService, LocalNotificationsDemo.Platforms.MacCatalyst.NotificationManagerService>();
#elif WINDOWS
            builder.Services.AddTransient<INotificationManagerService, LocalNotificationsDemo.Platforms.Windows.NotificationManagerService>();          
#endif
```

For more information about dependency injection in .NET MAUI, see [Dependency injection](~/fundamentals/dependency-injection.md).

## Send and receive local notifications

A `INotificationManagerService` implementation can be resolved through automatic dependency resolution, or through explicit dependency resolution. The following example shows using explicit dependency resolution to resolve a `INotificationManagerService` implementation:

```csharp
// Assume the app uses a single window.
INotificationManagerService notificationManager =
    Application.Current?.Windows[0].Page?.Handler?.MauiContext?.Services.GetService<INotificationManagerService>();
```

For more information about resolving registered types, see [Resolution](~/fundamentals/dependency-injection.md#resolution).

Once the `INotificationManagerService` implementation is resolved, its operations can be invoked:

```csharp
// Send
notificationManager.SendNotification();

// Scheduled send
notificationManager.SendNotification("Notification title goes here", "Notification messages goes here.", DateTime.Now.AddSeconds(10));

// Receive
notificationManager.NotificationReceived += (sender, eventArgs) =>
{
    var eventData = (NotificationEventArgs)eventArgs;

    MainThread.BeginInvokeOnMainThread(() =>
    {
        // Take required action in the app once the notification has been received.
    });
};
}
```

The `NotificationReceived` event handler casts its event arguments to `NotificationEventArgs`, which defines `Title` and `Message` properties:

```csharp
public class NotificationEventArgs : EventArgs
{
    public string Title { get; set; }
    public string Message { get; set; }
}
```

:::zone pivot="devices-android"

On Android, when a notification is sent it appears as an icon in the status bar:

:::image type="content" source="media/local-notifications/android-notification-area.png" alt-text="A notification icon in the status bar on Android.":::

When you swipe down on the status bar the notification drawer opens:

:::image type="content" source="media/local-notifications/android-notification-drawer.png" alt-text="A local notification in the notification drawer on Android.":::

Tapping on the notification launches the app. A notification remains visible in the notification drawer until it's dismissed by the app or user.

:::zone-end

:::zone pivot="devices-ios"

On iOS, incoming notifications are automatically received by an app without requiring user input:

:::image type="content" source="media/local-notifications/ios-notification.png" alt-text="A notification on iOS.":::

:::zone-end

:::zone pivot="devices-maccatalyst"

On Mac Catalyst, incoming notifications are automatically received by Notification Centre:

:::image type="content" source="media/local-notifications/mac-notification.png" alt-text="A notification in Notification Centre on macOS.":::

For more information about Notification Centre, see [Use Notification Centre on Mac](https://support.apple.com/en-gb/guide/mac-help/mchl2fb1258f/mac) on support.apple.com.

:::zone-end
