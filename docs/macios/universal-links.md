---
title: "Universal links"
description: "Learn how to"
ms.date: 02/19/2024
---

Apple deep links are known as universal links. These links can be based on a custom scheme, such as `myappname://` or can use the `http` or `https` scheme. This sample shows how to support `http`/`https` URL's, which requires hosting a well-known association JSON file on the domain that describes the relationship with your app. This enables Apple to verify that the app trying to handle a URL has ownership of the URL's domain to prevent malicious apps from intercepting your universal links.

iOS - [Supporting Universal Links in your app](https://developer.apple.com/documentation/xcode/supporting-universal-links-in-your-app)
https://learn.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/deep-linking
https://github.com/redth/maui.applinks.sample

# Universal links

Apple also supports registering your app to handle both custom URI schemes (eg: `myappname://`) and `http`/`https` schemes, however this sample focuses on `http`/`https` (custom schemes require additional setup in your `Info.plist` which is not covered here).

Apple refers to handling `http`/`https` urls as [Supporting Universal Links in your app](https://developer.apple.com/documentation/xcode/supporting-universal-links-in-your-app).  Similar to Google, Apple requires that you host a well-known `apple-app-site-association` file at the domain you want to handle links for, containing JSON information describing the relationship to your app.  This is a way for Apple to verify ownership for handling urls so not just anyone can intercept your site's links in their own apps.

## Host a well-known association file

Create a `apple-app-site-association` json file hosted on your domain's server under the `/.well-known/` folder.  Your URL should look like `https://redth.dev/.well-known/apple-app-site-association`.  The file contents will need to include:

```json
{
    "activitycontinuation": {
        "apps": [ "85HMA3YHJX.dev.redth.applinkssample" ]
    },
    "applinks": {
        "apps": [],
        "details": [
            {
                "appID": "85HMA3YHJX.dev.redth.applinkssample",
                "paths": [ "*", "/*" ]
            }
        ]
    }
}
```

Be sure to replace the app identifiers with the correct values for your own app.  This step required some trial and error to get working.  There are a number of posts online stating that the `activitycontinuation` was required to get things working.  I had a similar experience.

## Add domain association entitlements to your app

You will need to add custom entitlements to your app to declare the associated domain(s).  You can do this either by adding an Entitlements.plist file to your app, or you can simply add the following to your .csproj file in your MAUI app:

```xml
<ItemGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios' Or $([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'maccatalyst'">

    <!-- For debugging, use '?mode=developer' for debug to bypass apple's CDN cache -->
    <CustomEntitlements
        Condition="$(Configuration) == 'Debug'"
        Include="com.apple.developer.associated-domains"
        Type="StringArray"
        Value="applinks:redth.dev?mode=developer" />

    <!-- Non debugging, use normal applinks:url value -->
    <CustomEntitlements
        Condition="$(Configuration) != 'Debug'"
        Include="com.apple.developer.associated-domains"
        Type="StringArray"
        Value="applinks:redth.dev" />

</ItemGroup>
```

Be sure to replace the `applinks:redth.dev` with the correct value for your own domain.  Also notice the `ItemGroup`'s `Condition` which only includes the entitlement when the app is built for iOS or MacCatalyst.

## Add lifecycle handlers

In your `MauiProgram.cs` file, setup your lifecycle events with the `builder` (if you're not using 'Scenes' for multi window support in your app, you can omit the lifecycle handlers for Scene methods):

```csharp
builder.ConfigureLifecycleEvents(lifecycle =>
{
    #if IOS || MACCATALYST
    lifecycle.AddiOS(ios =>
    {
        ios.FinishedLaunching((app, data)
            => HandleAppLink(app.UserActivity));

        ios.ContinueUserActivity((app, userActivity, handler)
            => HandleAppLink(userActivity));

        if (OperatingSystem.IsIOSVersionAtLeast(13) || OperatingSystem.IsMacCatalystVersionAtLeast(13))
        {
            ios.SceneWillConnect((scene, sceneSession, sceneConnectionOptions)
                => HandleAppLink(sceneConnectionOptions.UserActivities.ToArray()
                    .FirstOrDefault(a => a.ActivityType == NSUserActivityType.BrowsingWeb)));

            ios.SceneContinueUserActivity((scene, userActivity)
                => HandleAppLink(userActivity));
        }
    });
    #elif ANDROID
        // ...
    #endif
});
```

## Test a URL

Testing on iOS can be a bit more tedious than Android.  It seems many have mixed results with iOS Simulators working (I was not able to get the Simulator working), so a device may be required, but is at least recommended.

Once you've deployed your app to your device, you can test that everything is setup correctly by going to `Settings -> Developer` and under `Universal Links`, toggle on `Associated Domains Development` and then go into `Diagnostics`.  Here you can enter your URL (in this case `https://redth.dev`) and if everything is setup correctly you should see a green checkmark with `Opens Installed Application` and the App ID of your app.

It's also worth noting again that from step 2, if you add the applink entitlement with `?mode=developer` to your app, it will bypass Apple's CDN cache when testing/debugging, which is helpful for iterating on your `apple-app-site-association` json file.
