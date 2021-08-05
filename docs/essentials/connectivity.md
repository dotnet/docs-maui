---
title: "Xamarin.Essentials: Connectivity"
description: "The Connectivity class in Xamarin.Essentials lets you monitor for changes in the device's network conditions, check the current network access, and how it is currently connected."
author: jamesmontemagno
ms.author: jamont
ms.date: 01/08/2019
ms.custom: video
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Connectivity

The **Connectivity** class lets you monitor for changes in the device's network conditions, check the current network access, and how it is currently connected.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

To access the **Connectivity** functionality the following platform specific setup is required.

# [Android](#tab/android)

The `AccessNetworkState` permission is required and must be configured in the Android project. This can be added in the following ways:

Open the **AssemblyInfo.cs** file under the **Properties** folder and add:

```csharp
[assembly: UsesPermission(Android.Manifest.Permission.AccessNetworkState)]
```

OR Update Android Manifest:

Open the **AndroidManifest.xml** file under the **Properties** folder and add the following inside of the **manifest** node.

```xml
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
```

Or right click on the Android project and open the project's properties. Under **Android Manifest** find the **Required permissions:** area and check the **Access Network State** permission. This will automatically update the **AndroidManifest.xml** file.

# [iOS](#tab/ios)

No additional setup required.

# [UWP](#tab/uwp)

No additional setup required.

-----

## Using Connectivity

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

Check current network access:

```csharp
var current = Connectivity.NetworkAccess;

if (current == NetworkAccess.Internet)
{
    // Connection to internet is available
}
```

[Network access](xref:Xamarin.Essentials.NetworkAccess) falls into the following categories:

* **Internet** – Local and internet access.
* **ConstrainedInternet** – Limited internet access. Indicates captive portal connectivity, where local access to a web portal is provided, but access to the Internet requires that specific credentials are provided via a portal.
* **Local** – Local network access only.
* **None** – No connectivity is available.
* **Unknown** – Unable to determine internet connectivity.

You can check what type of [connection profile](xref:Xamarin.Essentials.ConnectionProfile) the device is actively using:

```csharp
var profiles = Connectivity.ConnectionProfiles;
if (profiles.Contains(ConnectionProfile.WiFi))
{
    // Active Wi-Fi connection.
}
```

Whenever the connection profile or network access changes you can receive an event when triggered:

```csharp
public class ConnectivityTest
{
    public ConnectivityTest()
    {
        // Register for connectivity changes, be sure to unsubscribe when finished
        Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
    }

    void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        var access = e.NetworkAccess;
        var profiles = e.ConnectionProfiles;
    }
}
```

## Limitations

It is important to note that it is possible that `Internet` is reported by `NetworkAccess` but full access to the web is not available. Due to how connectivity works on each platform it can only guarantee that a connection is available. For instance the device may be connected to a Wi-Fi network, but the router is disconnected from the internet. In this instance Internet may be reported, but an active connection is not available.

## API

* [Connectivity source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Connectivity)
* [Connectivity API documentation](xref:Xamarin.Essentials.Connectivity)
