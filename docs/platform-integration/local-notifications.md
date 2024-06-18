---
title: "Local notifications"
description: "Learn how to send, schedule, and receive local notifications in .NET MAUI"
ms.date: 06/18/2024
zone_pivot_groups: devices-three-platforms
---

# Local notifications

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-local-notifications)

Local notifications are alerts sent by apps installed on a device. Local notifications are often used for features such as:

- Calendar events
- Reminders
- Location-based triggers

Each platform requires its own native code implementation to create, display, and consume local notifications. However, each platform implementation can be abstracted at the cross-platform layer so that there's a consistent API to send, schedule, and receive local notifications in a .NET Multi-platform App UI (.NET MAUI) app.

For information about local notifications on Windows, see [App notifications overview](/windows/apps/windows-app-sdk/notifications/app-notifications/).

## Create a cross-platform interface

A .NET MAUI app should create and consume notifications without concern for the underlying platform implementations. The following `INotificationManagerService` interface is used to define a cross-platform API that an app can use to interact with local notifications:

```csharp
public interface INotificationManagerService
{
    event EventHandler NotificationReceived;
    void SendNotification(string title, string message, DateTime? notifyTime = null);
    void ReceiveNotification(string title, string message);
}
```

This interface should be implemented on each platform that you want to support local notifications. The `NotificationReceived` event allows an app to handle incoming notifications. The `SendNotification` sends a notification at an optional `DateTime`. The `ReceiveNotification` method processes a notification when received by the underlying platform.

:::zone pivot="devices-android"

## Local notifications on Android

For a .NET MAUI app to send and receive notifications on Android, the app must provide an implementation of the `INotificationManagerService` interface.

For information about local notifications on Android, see [Notifications overview](https://developer.android.com/develop/ui/views/notifications) on developer.android.com.

### Send, receive, and show local notifications

On Android, the `NotificationManagerService` class implements the `INotificationManagerService` interface, and contains the logic to send, receive, and show local notifications:

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
            intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

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
        intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

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

The `NotificationManagerService` class should be placed in the *Platforms > Android* folder. Alternatively, multi-targeting can be performed based on your own filename and folder criteria, rather than using the *Platforms* folders. For more information, see [Configure multi-targeting](configure-multi-targeting.md).

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
> By default, notifications scheduled using the `AlarmManager` class will not survive device restart. However, you can design your app to automatically reschedule notifications if the device is restarted. For more information, see [Start an alarm when the device restarts](https://developer.android.com/training/scheduling/alarms#boot) in [Schedule repeating alarms](https://developer.android.com/training/scheduling/alarms) on developer.android.com. For information about background processing on Android, see [Guide to background processing](https://developer.android.com/guide/background) on developer.android.com.

### Handle incoming notifications

The Android app must detect incoming notifications and notify the `NotificationManagerService` instance. One way of achieving this is by modifying the `MainActivity` class.

The `Activity` attribute on the `MainActivity` class should specify a `LaunchMode` value of `LaunchMode.SingleTop`:

```csharp
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, //... )]
public class MainActivity : MauiAppCompatActivity
{
}
```

The `SingleTop` mode prevents multiple instances of an `Activity` from being started while the application is in the foreground. This `LaunchMode` may not be appropriate for apps that launch multiple activities in more complex notification scenarios. For more information about `LaunchMode` enumeration values, see [Android Activity LaunchMode](https://developer.android.com/guide/topics/manifest/activity-element#lmode).

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

The `CreateNotificationFromIntent` method extracts notification data from the `intent` argument and provides it to the `AndroidNotificationManager` using the `ReceiveNotification` method. The `CreateNotificationFromIntent` method is called from both the `OnCreate` method and the `OnNewIntent` method:

- When the application is started by notification data, the `Intent` data will be passed to the `OnCreate` method.
- If the application is already in the foreground, the `Intent` data will be passed to the `OnNewIntent` method.

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

After the app has launched you should request the user to grant this permission, before attempting to send a local notification:

```csharp
PermissionStatus status = await Permissions.RequestAsync<NotificationPermission>();
```

For more information about .NET MAUI permissions, see [Permissions](~/platform-integration/appmodel/permissions.md).

:::zone-end

:::zone pivot="devices-ios,devices-maccatalyst"

## Local notifications on iOS and Mac Catalyst

For a .NET MAUI app to send and receive notifications on Apple platforms the app must provide an implementation of the `INotificationManagerService` interface.

For information about local notifications on Apple platforms, see [Scheduling a notification locally from your app](https://developer.apple.com/documentation/usernotifications/scheduling-a-notification-locally-from-your-app) on developer.apple.com.

### Send, receive, and show local notifications

On iOS and Mac Catalyst, the `NotificationManagerService` class implements the `INotificationManagerService` interface, and contains the logic to send and receive local notifications:

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

The `NotificationManagerService` class should be placed in the *Platforms > iOS*  or *Platforms > Mac Catalyst* folder. Alternatively, multi-targeting can be performed based on your own filename and folder criteria, rather than using the *Platforms* folders. For more information, see [Configure multi-targeting](configure-multi-targeting.md).

On iOS, you must request permission to use notifications before attempting to schedule a notification. This occurs in the `NotificationManagerService` constructor. The `SendNotification` method defines the logic required to create and send a notification, and creates an immediate local notification using a `UNTimeIntervalNotificationTrigger` object, or at an exact `DateTime` using a `UNCalendarNotificationTrigger` object. The `ReceiveNotification` method is called by iOS when a message is received, and invokes the `NotificationReceived` event handler. For more information about local notification permission,, see [Asking permission to use notifications](https://developer.apple.com/documentation/usernotifications/asking-permission-to-use-notifications) on developer.apple.com.

### Handle incoming notifications

On iOS, you must create a delegate that subclasses `UNUserNotificationCenterDelegate` to handle incoming messages:

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

The `NotificationReceiver` class provides incoming notification data to the `ReceiveNotification` method in the `NavigationManagerService` class, and is registered as the `UNUserNotificationCenter` delegate in the `NavigationManagerService` constructor.

:::zone-end

## Register platform implementations

Each platforms `NotificationManagerService` implementation should be registered against the `INotificationManagerService` interface, so that the operations the interface exposes can be called from cross-platform code. This can be achieved by registering the types with the <xref:Microsoft.Maui.Hosting.MauiAppBuilder.Services> property of the <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object in the `CreateMauiApp` method in the `MauiProgram` class:

```csharp
#if ANDROID
            builder.Services.AddTransient<INotificationManagerService, LocalNotificationsDemo.Platforms.Android.NotificationManagerService>();
#elif IOS
            builder.Services.AddTransient<INotificationManagerService, LocalNotificationsDemo.Platforms.iOS.NotificationManagerService>();
#elif MACCATALYST
            builder.Services.AddTransient<INotificationManagerService, LocalNotificationsDemo.Platforms.MacCatalyst.NotificationManagerService>();
#endif
```

For more information about dependency injection in .NET MAUI, see [Dependency injection](~/fundamentals/dependency-injection.md).

## Consume the interface in Xamarin.Forms

The `INotificationManagerService` implementation can be resolved through automatic dependency resolution, or through explicit dependency resolution. The following example shows using explicit dependency resolution to resolve the `INotificationManagerService` implementation:

```csharp
INotificationManagerService notificationManager = Application.Current?.MainPage?.Handler?.MauiContext?.Services.GetService<INotificationManagerService>();
```

For more information about resolving registered types, see [Resolution](~/fundamentals/dependency-injection.md).

Once the `INotificationManagerService` implementation is resolved, its operations can be invoked:

```csharp
notificationManager.NotificationReceived += (sender, eventArgs) =>
{
    var eventData = (NotificationEventArgs)eventArgs;

    MainThread.BeginInvokeOnMainThread(() =>
    {
        // Take any action in the app once the notification has been received.
    });
};

// Send
notificationManager.SendNotification();

// Scheduled send
notificationManager.SendNotification("Notification title goes here", "Notification messages goes here.", DateTime.Now.AddSeconds(10));
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

On Android, notifications will appear in the notification area. When the notification is tapped, the application receives the notification and displays a message:

IMAGE GOES HERE

:::zone-end

:::zone pivot="devices-ios,devices-maccatalyst"

On iOS, incoming notifications are automatically received by the application without requiring user input. The application receives the notification and displays a message:

IMAGE GOES HERE

:::zone-end