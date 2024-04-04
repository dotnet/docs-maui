---
title: "Connectivity"
description: "Learn how to use the .NET MAUI IConnectivity interface in the Microsoft.Maui.Networking namespace. With this interface, you can determine if you can communicate with the internet and which network devices are connected"
ms.date: 02/02/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Networking", "Connectivity"]
---

# Connectivity

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Networking.IConnectivity> interface to inspect the network accessibility of the device. The network connection may have access to the internet. Devices also contain different kinds of network connections, such as Bluetooth, cellular, or WiFi. The `IConnectivity` interface has an event to monitor changes in the devices connection state.

The default implementation of the `IConnectivity` interface is available through the <xref:Microsoft.Maui.Networking.Connectivity.Current?displayProperty=nameWithType> property. Both the `IConnectivity` interface and `Connectivity` class are contained in the `Microsoft.Maui.Networking` namespace.

## Get started

To access the **Connectivity** functionality, the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

The `AccessNetworkState` permission is required and must be configured in the Android project. This can be added in the following ways:

- Add the assembly-based permission:

  Open the _Platforms/Android/MainApplication.cs_ file and add the following assembly attributes after `using` directives:

  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.AccessNetworkState)]
  ```

  \- or -

- Update the Android Manifest:

  Open the _Platforms/Android/AndroidManifest.xml_ file and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
  ```

  \- or -

- Update the Android Manifest in the manifest editor:

  In Visual Studio double-click on the *Platforms/Android/AndroidManifest.xml* file to open the Android manifest editor. Then, under **Required permissions** check the **ACCESS_NETWORK_STATE** permission. This will automatically update the *AndroidManifest.xml* file.

# [iOS/Mac Catalyst](#tab/macios)

No setup is required.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Using Connectivity

You can determine the scope of the current network by checking the <xref:Microsoft.Maui.Networking.IConnectivity.NetworkAccess> property.

:::code language="csharp" source="../snippets/shared_1/NetworkingPage.cs" id="network_test":::

Network access falls into the following categories:

- <xref:Microsoft.Maui.Networking.NetworkAccess.Internet> &mdash; Local and internet access.
- <xref:Microsoft.Maui.Networking.NetworkAccess.ConstrainedInternet> &mdash; Limited internet access. This value means that there's a captive portal, where local access to a web portal is provided. Once the portal is used to provide authentication credentials, internet access is granted.
- <xref:Microsoft.Maui.Networking.NetworkAccess.Local> &mdash; Local network access only.
- <xref:Microsoft.Maui.Networking.NetworkAccess.None> &mdash; No connectivity is available.
- <xref:Microsoft.Maui.Networking.NetworkAccess.Unknown> &mdash; Unable to determine internet connectivity.

You can check what type of connection profile the device is actively using:

:::code language="csharp" source="../snippets/shared_1/NetworkingPage.cs" id="network_profiles":::

Whenever the connection profile or network access changes, the <xref:Microsoft.Maui.Networking.IConnectivity.ConnectivityChanged> event is raised:

:::code language="csharp" source="../snippets/shared_1/NetworkingPage.cs" id="network_implementation":::

## Limitations

It's important to know that it's possible that <xref:Microsoft.Maui.Networking.NetworkAccess.Internet> is reported by <xref:Microsoft.Maui.Networking.IConnectivity.NetworkAccess> but full access to the web isn't available. Because of how connectivity works on each platform, it can only guarantee that a connection is available. For instance, the device may be connected to a Wi-Fi network, but the router is disconnected from the internet. In this instance `Internet` may be reported, but an active connection isn't available.
