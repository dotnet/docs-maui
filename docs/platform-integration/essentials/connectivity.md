---
title: "Connectivity"
description: "Learn how to use the .MET MAUI Connectivity class in the Microsoft.Maui.Essentials namespace. With this class, you can determine if you can communicate with the internet and which network devices are connected"
ms.date: 08/16/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Connectivity

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `Connectivity` class to inspect the network accessibility of the device. The network connection may have access to the internet. Devices also contain different kinds of network connections, such as Bluetooth, cellular, or WiFi. The `Connectivity` class has an event to monitor changes in the devices connection state.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

To access the **Connectivity** functionality, the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

The `AccessNetworkState` permission is required and must be configured in the Android project. This can be added in the following ways:

- Open the **AssemblyInfo.cs** file under the **Properties** folder and add:
  
  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.AccessNetworkState)]
  ```

  \- or -

- Update Android Manifest:

  Open the _AndroidManifest.xml_ file under the **Properties** folder and add the following in the **manifest** node.
  
  ```xml
  <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
  ```

  \- or -

- Right-click on the Android project and open the project's properties. Under _Android Manifest_ find the **Required permissions:** area and check the **Access Network State** permission. This will automatically update the _AndroidManifest.xml_ file.

# [iOS](#tab/ios)

No setup is required.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Using Connectivity

You can determine the scope of the current network by checking the `Connectivity.NetworkAccess` property.

```csharp
var current = Connectivity.NetworkAccess;

if (current == NetworkAccess.Internet)
{
    // Connection to internet is available
}
```

Network access falls into the following categories:

- **Internet** – Local and internet access.
- **ConstrainedInternet** – Limited internet access. This value means that there's a captive portal, where local access to a web portal is provided. Once the portal is used to provide authentication credentials, internet access is granted.
- **Local** – Local network access only.
- **None** – No connectivity is available.
- **Unknown** – Unable to determine internet connectivity.

You can check what type of connection profile the device is actively using:

```csharp
var profiles = Connectivity.ConnectionProfiles;
if (profiles.Contains(ConnectionProfile.WiFi))
{
    // Active Wi-Fi connection.
}
```

Whenever the connection profile or network access changes, the `Connectivity.ConnectivityChanged` event is raised:

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
        if (e.NetworkAccess == NetworkAccess.ConstrainedInternet)
            Console.WriteLine("Internet access is available but is limited.");

        else if (e.NetworkAccess != NetworkAccess.Internet)
            Console.WriteLine("Internet access has been lost.");

        // Log each active connection
        Console.Write("Connections active: ");

        foreach (var item in e.ConnectionProfiles)
        {
            switch (item)
            {
                case ConnectionProfile.Bluetooth:
                    Console.Write("Bluetooth");
                    break;
                case ConnectionProfile.Cellular:
                    Console.Write("Cell");
                    break;
                case ConnectionProfile.Ethernet:
                    Console.Write("Ethernet");
                    break;
                case ConnectionProfile.WiFi:
                    Console.Write("WiFi");
                    break;
                default:
                    break;
            }
        }

        Console.WriteLine();
    }
}
```

## Limitations

It's important to know that it's possible that `Internet` is reported by `NetworkAccess` but full access to the web isn't available. Because of how connectivity works on each platform, it can only guarantee that a connection is available. For instance, the device may be connected to a Wi-Fi network, but the router is disconnected from the internet. In this instance `Internet` may be reported, but an active connection isn't available.

## API

- [Connectivity source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/Connectivity)
<!-- - [Connectivity API documentation](xref:Microsoft.Maui.Essentials.Connectivity)-->
