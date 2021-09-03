---
title: "App lifecycle"
description: ".NET MAUI raises cross-platform lifecycle events when an app transitions between its different execution states."
ms.date: 09/03/2021
---

# App lifecycle

.NET Multi-platform App UI (.NET MAUI) apps generally have three execution states: *not running*, *running*, and *stopped*. .NET MAUI raises lifecycle cross-platform events on the `Window` class when an app transitions from the not running state to the running state, and the running state to the stopped state.

The following diagram shows an overview of the .NET MAUI app lifecycle:

:::image type="content" source="app-lifecycle-images/app-lifecycle.png" alt-text=".NET MAUI app lifecycle" border="false":::

In the diagram, the gray oval indicates that the app isn't loaded into memory. The light blue ovals indicate that the app is in memory. The arcs indicate events that are fired by .NET MAUI, that provide notifications to the running app.

The execution state of an app depends on the app's history. For example, when an app is installed for the first time, or a device is started, the app can be considered to be *not running*. When the app is started, the `Created` and `Activated` events are raised and the app is *running*. If the user switches to a different app, or returns to the device's Home screen, the `Deactivated` and `Stopped` events are raised and the app is *stopped*. If the user returns to the app, the `Resuming` event is raised and app is *running*. Alternatively, an app might be terminated by a user while it's running. In this situation the app is *stopped*, the `Destroying` event is raised and the app is *not running*. Similarly, a device might terminate an app while it's stopped, due to resource restrictions, and the `Destroying` event is raised and the app is *not running*.

In addition, .NET MAUI enables apps to be notified when native platform lifecycle events are raised. For more information, see [Native lifecycle events](#native-lifecycle-events).

## Cross-platform lifecycle events

The `Window` class defines the following cross-platform lifecycle events:

| Event | Description | Action to take |
| -- | -- | -- |
| `Created` | This event is raised after the native window has been created. At this point the cross-platform window will have a native window handler, but the window might not be visible yet. | Register any services that are needed before a window exists. |
| `Activated` | This event is raised when the window has been activated, and is, or will become, the focused window. |  |
| `Deactivated` | This event is raised when the window is no longer the focused window. However, the window might still be visible. |  |
| `Stopped` | This event is raised when the window is no longer visible. There's no guarantee that an app will resume from this state, because it may be terminated by the operating system. | Disconnect from any long running processes, or cancel any pending requests that might consume device resources. |
| `Resumed` | This event is raised when an app resumes after being stopped. This event won't be raised the first time your app launches, and can only be raised if the `Stopped` event has previously been raised. | Subscribe to any required events, and refresh any content that's on the visible page. |
| `Destroying` | This event is raised when the native window is being destroyed and deallocated. The same cross-platform window might be used against a new native window when the app is re-opened. | Remove any event subscriptions that you've attached to the native window. |

These cross-platform events map to different platform events, and the following table shows this mapping:

| Event | Android | iOS | Windows |
| -- | -- | -- | -- |
| `Created` | `OnPostCreate` | `FinishedLaunching` | `Created` |
| `Activated` | `OnResume` | `OnActivated` | `Activated` (`CodeActivated` and `PointerActivated`) |
| `Deactivated` | `OnPause` | `OnResignActivation` | `Activated` (`Deactivated`) |
| `Stopped` | `OnStop` | `DidEnterBackground` | `VisibilityChanged` |
| `Resumed` | `OnRestart` | `WillEnterForeground` | `Resumed` |
| `Destroying` | `OnDestroy` | `WillTerminate` | `Closed` |

In addition to these events, the `Window` class also has the following overridable methods:

- `OnCreated`, which is invoked when the `Created` event is raised.
- `OnResumed`, which is invoked when the `Resumed` event is raised.
- `OnActivated`, which is invoked when the `Activated` event is raised.
- `OnDeactivated`, which is invoked when the `OnDeactivated` event is raised.
- `OnStopped`, which is invoked when the `Stopped` event is raised.
- `OnDestroying`, which is invoked when the `Destroying` event is raised.

<!-- Todo: add code example -->

## Native lifecycle events

.NET MAUI defines delegates that are invoked in response to native platform lifecycle events being raised. Handlers can be specified for these delegates, using named methods or anonymous functions, which are executed when the delegate is invoked. This mechanism enables apps to be notified when native platform lifecycle events are raised.

> [!IMPORTANT]
> The `ConfigureLifecycleEvents` method is in the `Microsoft.Maui.LifecycleEvents` namespace.

### Android

The following table lists the .NET MAUI delegates that are invoked in response to Android lifecycle events being raised:

| Delegate | Arguments | Description | Comments |
| -- | -- | -- |
| `OnActivityResult` | `Android.App.Activity`, `int`, `Android.App.Result`, `Android.Content.Intent?` | Invoked when an activity you launched exits. |  |
| `OnApplicationConfigurationChanged` | `Android.App.Application`, `Android.Content.Res.Configuration` | Invoked when the device configuration changes while your component is running. |  |
| `OnApplicationCreate` | `Android.App.Application` | Invoked when the app has started, before an activity, service, or receiver objects (excluding content providers) have been created. |  |
| `OnApplicationCreating` | `Android.App.Application` | Invoked when the app is starting, before an activity, service, or receiver objects (excluding content providers) have been created. |  |
| `OnApplicationLowMemory` | `Android.App.Application` | Invoked when the system is running low on memory, and actively running processes should trim their memory usage. |  |
| `OnApplicationTrimMemory` | `Android.App.Application`, `Android.Content.TrimMemory` | Invoked when the operating system has determined that it's a good time for a process to trim unneeded memory from its process. |  |
| `OnBackPressed` | `Android.App.Activity` | Invoked when the activity has detected a press of the back key. |  |
| `OnConfigurationChanged` | `Android.App.Activity`, `Android.Content.Res.Configuration` | Invoked when the device configuration changes while your activity is running. |  |
| `OnCreate` | `Android.App.Activity`, `Android.OS.Bundle?` | Raised when the activity is created. |  |
| `OnDestroy` | `Android.App.Activity` | Invoked when the activity is finishing, or because the system is temporarily destroying the activity instance to save space. | Always call the super class's implementation. |
| `OnNewIntent` | `Android.App.Activity`, `Android.Content.Intent?` | Invoked when the activity is relaunched while at the top of the activity stack instead of a new instance of the activity being started. |  |
| `OnPause` | `Android.App.Activity` | Invoked when an activity is going into the background, but has not yet been killed. | Always call the super class's implementation. |
| `OnPostCreate` | `Android.App.Activity`, `Android.OS.Bundle?` | Invoked when activity startup is complete, after `OnStart` and `OnRestoreInstanceState` have been called. | Always call the super class's implementation. This is a system-only event that generally shouldn't be used by apps. |
| `OnPostResume` | `Android.App.Activity` | Invoked when activity resume is complete, after `OnResume` has been called. | Always call the super class's implementation. This is a system-only event that generally shouldn't be used by apps. |
| `OnPressingBack` | `Android.App.Activity` | Invoked when the activity has detected a press of the back key, but hasn't handled the press. | This is a .NET MAUI custom event.  |
| `OnRequestPermissionsResult` | `Android.App.Activity`, `int`, `string[]`, `Android.Content.PM.Permission[]` | Invoked as a callback for the result from requesting permissions. |  |
| `OnRestart` | `Android.App.Activity` | Invoked after `OnStop` when the current activity is being re-displayed to the user (the user has navigated back to it). | Always call the super class's implementation. |
| `OnRestoreInstanceState` | `Android.App.Activity`, `Android.OS.Bundle` | Invoked after `OnStart` when the activity is being re-initialized from a previously saved state. |  |
| `OnResume` | `Android.App.Activity` | Invoked after `OnRestoreInstanceState`, `OnRestart`, or `OnPause`, to indicate that the activity is active and is ready to receive input. |  |
| `OnSaveInstanceState` | `Android.App.Activity`, `Android.OS.Bundle` | Invoked to retrieve per-instance state from an activity being killed so that the state can be restored in `OnCreate` or `OnRestoreInstanceState`. |  |
| `OnStart` | `Android.App.Activity` | Invoked after `OnCreate` or `OnRestart` when the activity has been stopped, but is now being displayed to the user. | Always call the super class's implementation.  |
| `OnStop` | `Android.App.Activity` | Invoked when the activity is no longer visible to the user. | Always call the super class's implementation.  |

> [!IMPORTANT]
> Each delegate has a corresponding identically named extension method, that can be called to register a handler for the delegate.

To respond to an Android lifecycle delegate being invoked, call the `ConfigureLifecycleEvents` method on the `IAppHostBuilder` object in the `Configure` method of your `Startup` class. Then, on the `ILifecycleBuilder` object, call the `AddAndroid` method and specify the `Action` that registers handlers for the required delegates:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;

namespace NativeLifecycleDemo
{
    public class Startup : IStartup
    {
        public void Configure(IAppHostBuilder appBuilder)
        {
            appBuilder
                .UseMauiApp<App>()
                .ConfigureLifecycleEvents(events =>
                {
#if __ANDROID__
                    events.AddAndroid(android => android
                        .OnActivityResult((activity, requestCode, resultCode, data) => LogEvent("OnActivityResult", requestCode.ToString()))
                        .OnStart((activity) => LogEvent("OnStart"))
                        .OnCreate((activity, bundle) => LogEvent("OnCreate"))
                        .OnBackPressed((activity) => LogEvent("OnBackPressed"))
                        .OnStop((activity) => LogEvent("OnStop")));
#endif
                    static void LogEvent(string eventName, string type = null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lifecycle event: {eventName}{(type == null ? string.Empty : $" ({type})")}");
                    }
                });
        }
    }
}
```

For more information about the Android app lifecycle, see [Understand the Activity Lifecycle](https://developer.android.com/guide/components/activities/activity-lifecycle) on developer.android.com.

### iOS

The following table lists the .NET MAUI delegates that are invoked in response to iOS lifecycle events being raised:

| Delegate | Arguments | Description |
| -- | -- | -- |
| `ContinueUserActivity` | `UIKit.UIApplication`, `Foundation.NSUserActivity`, `UIKit.UIApplicationRestorationHandler` | Invoked when the app receives data associated with a user activity, such as transferring an activity from a different device using Handoff. |
| `DidEnterBackground` | `UIKit.UIApplication` | Invoked when the app has entered the background. |
| `FinishedLaunching` | `UIKit.UIApplication`, `Foundation.NSDictionary` | Invoked when the app has launched. |
| `OnActivated` | `UIKit.UIApplication` | Invoked when the app is launched and every time the app returns to the foreground. |
| `OnResignActivation` | `UIKit.UIApplication` | Invoked when the app is about to enter the background, be suspended, or when the user receives an interruption such as a phone call or text. |
| `OpenUrl` | `UIKit.UIApplication`, `Foundation.NSDictionary` | Invoked when the app should open a specified URL. |
| `PerformActionForShortcutItem` | `UIKit.UIApplication`, `UIKit.UIApplicationShortcutItem`, `UIKit.UIOperationHandler` | Invoked when a Home screen quick action is initiated. |
| `WillEnterForeground` | `UIKit.UIApplication` | Invoked if the app will be returning from a backgrounded state. |
| `WillFinishLaunching` | `UIKit.UIApplication`, `Foundation.NSDictionary` | Invoked when app launching has begun, but state restoration has not yet occurred. |
| `WillTerminate` | `UIKit.UIApplication` | Invoked if the app is being terminated due to memory constraints, or directly by the user. |

> [!IMPORTANT]
> Each delegate has a corresponding identically named extension method, that can be called to register a handler for the delegate.

To respond to an iOS lifecycle delegate being invoked, call the `ConfigureLifecycleEvents` method on the `IAppHostBuilder` object in the `Configure` method of your `Startup` class. Then, on the `ILifecycleBuilder` object, call the `AddiOS` method and specify the `Action` that registers handlers for the required delegates:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;

namespace NativeLifecycleDemo
{
    public class Startup : IStartup
    {
        public void Configure(IAppHostBuilder appBuilder)
        {
            appBuilder
                .UseMauiApp<App>()
                .ConfigureLifecycleEvents(events =>
                {
#if __IOS__
                    events.AddiOS(ios => ios
                        .OnActivated((app) => LogEvent("OnActivated"))
                        .OnResignActivation((app) => LogEvent("OnResignActivation"))
                        .DidEnterBackground((app) => LogEvent("DidEnterBackground"))
                        .WillTerminate((app) => LogEvent("WillTerminate")));
#endif

                    static void LogEvent(string eventName, string type = null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lifecycle event: {eventName}{(type == null ? string.Empty : $" ({type})")}");
                    }
                });
        }
    }
}
```

For more information about the iOS app lifecycle, see [Managing Your App's Life Cycle](https://developer.apple.com/documentation/uikit/app_and_environment/managing_your_app_s_life_cycle?language=objc) on developer.apple.com.

### Windows

The following table lists the .NET MAUI delegates that are invoked in response to Windows lifecycle events being raised:

| Delegate | Arguments | Description |
| -- | -- | -- |
| `OnActivated` | `Microsoft.UI.Xaml.Window`, `Microsoft.UI.Xaml.WindowActivatedEventArgs` | Invoked when the native [`Activated`](xref:Microsoft.UI.Xaml.Window.Activated) event is raised, provided that the app isn't resuming. |
| `OnClosed` | `Microsoft.UI.Xaml.Window`, `Microsoft.UI.Xaml.WindowEventArgs` | Invoked when the native [`Closed`](xref:Microsoft.UI.Xaml.Window.Closed) event is raised. |
| `OnLaunched` | `Microsoft.UI.Xaml.Window`, `Microsoft.UI.Xaml.LaunchActivatedEventArgs` | Invoked by .NET MAUI's [`Application.OnLaunched`](xref:Microsoft.UI.Xaml.Application.OnLaunched*) override once the native window has been created and activated. |
| `OnLaunching` | `Microsoft.UI.Xaml.Window`, `Microsoft.UI.Xaml.LaunchActivatedEventArgs` | Invoked by .NET MAUI's [`Application.OnLaunched`](xref:Microsoft.UI.Xaml.Application.OnLaunched*) override before the native window has been created and activated. |
| `OnNativeMessage` | `Microsoft.UI.Xaml.Window`, `WindowsNativeMessageEventArgs` | Invoked when .NET MAUI's `NativeMessage` event is raised. |
| `OnResumed` | `Microsoft.UI.Xaml.Window` | Invoked when the native [`Activated`](xref:Microsoft.UI.Xaml.Window.Activated) event is raised, provided that the app is resuming. |
| `OnVisibilityChanged` | `Microsoft.UI.Xaml.Window`, `Microsoft.UI.Xaml.WindowVisibilityChangedEventArgs` | Invoked when the native [`VisibilityChanged`](xref:Microsoft.UI.Xaml.Window.VisibilityChanged) event is raised. |
| `OnWindowCreated` | `Microsoft.UI.Xaml.Window` | Invoked when the native window is created for the cross-platform `Window`. |

The `NativeMessage` event is specific to .NET MAUI, and enables native Windows messages to be exposed as a lifecycle event. The `WindowsNativeMessageEventArgs` object that accompanies the `NativeMessage` event includes a `MessageId` property, of type `uint`. The value of this property can be examined to determine which message has been passed to your app window. For more information about windows messages, see [Windows Messages (Get Started with Win32 and C++)](/windows/win32/learnwin32/window-messages). For a list of window message constants, see [Window notifications](/windows/win32/winmsg/window-notifications).

> [!IMPORTANT]
> Each delegate has a corresponding identically named extension method, that can be called to register a handler for the delegate.

To respond to a Windows lifecycle delegate being invoked, call the `ConfigureLifecycleEvents` method on the `IAppHostBuilder` object in the `Configure` method of your `Startup` class. Then, on the `ILifecycleBuilder` object, call the `AddWindows` method and specify the `Action` that call registers handlers for the required delegates:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;

namespace NativeLifecycleDemo
{
    public class Startup : IStartup
    {
        public void Configure(IAppHostBuilder appBuilder)
        {
            appBuilder
                  .UseMauiApp<App>()
                  .ConfigureLifecycleEvents(events =>
                  {
#if WINDOWS
                               events.AddWindows(windows => windows
                                      .OnActivated((window, args) => LogEvent("OnActivated"))
                                      .OnClosed((window, args) => LogEvent("OnClosed"))
                                      .OnLaunched((window, args) => LogEvent("OnLaunched"))
                                      .OnLaunching((window, args) => LogEvent("OnLaunching"))
                                      .OnVisibilityChanged((window, args) => LogEvent("OnVisibilityChanged"))
                                      .OnNativeMessage((window, args) =>
                                      {
                                          if (args.MessageId == Convert.ToUInt32("0x02E0"))
                                          {
                                              // DPI has changed
                                          }
                                      }));
#endif

                      static void LogEvent(string eventName, string type = null)
                      {
                          System.Diagnostics.Debug.WriteLine($"Lifecycle event: {eventName}{(type == null ? string.Empty : $" ({type})")}");
                      }
                  });
        }
    }
}
```

## Custom lifecycle events

<!-- Todo: AddEvent example and explanation
```csharp
events.AddEvent<Action<string>>("CustomEventName", value => LogEvent("CustomEventName"));
```-->
