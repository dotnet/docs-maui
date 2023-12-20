---
title: "Contacts"
description: "Learn how to use the .NET MAUI Contacts interface in the Microsoft.Maui.ApplicationModel.Communication namespace, which lets a pick a contact and retrieve information about it."
ms.date: 02/02/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel.Communication"]
---

# Contacts

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.ApplicationModel.Communication.IContacts> interface to select a contact and read information about it.

The default implementation of the `IContacts` interface is available through the <xref:Microsoft.Maui.ApplicationModel.Communication.Contacts.Default> property. Both the `IContacts` interface and `Contacts` class are contained in the `Microsoft.Maui.ApplicationModel.Communication` namespace.

> [!IMPORTANT]
> Picking a contact is unsupported on Windows.

Because of a namespace conflict, the `Contacts` type must be fully qualified when targeting iOS or macOS: `Microsoft.Maui.ApplicationModel.Communication.Contacts`. New projects automatically target these platforms, along with Android and Windows.

To write code that will compile for iOS and macOS, fully qualify the `Contacts` type. Alternatively, provide a `using` directive to map the `Communication` namespace:

```csharp
using Communication = Microsoft.Maui.ApplicationModel.Communication;

// Code that uses the namespace:
var contact = await Communication.Contacts.Default.PickContactAsync();
```

## Get started

To access the **Contacts** functionality the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

The `ReadContacts` permission is required and must be configured in the Android project. This can be added in the following ways:

- Add the assembly-based permission:

  Open the _Platforms/Android/MainApplication.cs_ file and add the following assembly attribute after `using` directives:

  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.ReadContacts)]
  ```

  \- or -

- Update the Android Manifest:

  Open the _Platforms/Android/AndroidManifest.xml_ file and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.READ_CONTACTS" />
  ```

  \- or -

- Update the Android Manifest in the manifest editor:

  In Visual Studio double-click on the *Platforms/Android/AndroidManifest.xml* file to open the Android manifest editor. Then, under **Required permissions** check the **READ_CONTACTS** permission. This will automatically update the *AndroidManifest.xml* file.

# [iOS/Mac Catalyst](#tab/macios)

In the **Solution Explorer** pane, right-click on the _Platforms/iOS/Info.plist_ or _Platforms/MacCatalyst/Info.plist_ file. Select **Open With** and then select the **XML (Text) Editor** item. Press the **OK** button. In the file, add the following key and value:

```xml
<key>NSContactsUsageDescription</key>
<string>This app needs access to contacts to pick a contact and get info.</string>
```

The `<string>` element is the description specific to your app and is shown to the user.

# [Windows](#tab/windows)

Picking a contact is unsupported on Windows.

-----
<!-- markdownlint-enable MD025 -->

## Pick a contact

You can request the user to pick a contact by calling the <xref:Microsoft.Maui.ApplicationModel.Communication.IContacts.PickContactAsync> method. A contact dialog will appear on the device allowing the user to select a contact. If the user doesn't select a contact, `null` is returned.

:::code language="csharp" source="../snippets/shared_1/CommsPage.xaml.cs" id="contact_select":::

## Get all contacts

The <xref:Microsoft.Maui.ApplicationModel.Communication.IContacts.GetAllAsync%2A> method returns a collection of contacts.

:::code language="csharp" source="../snippets/shared_1/CommsPage.xaml.cs" id="contact_all":::

## Platform differences

This section describes the platform-specific differences with the contacts API.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
# [Android](#tab/android)

- The `cancellationToken` parameter in the <xref:Microsoft.Maui.ApplicationModel.Communication.IContacts.GetAllAsync%2A> method isn't supported.

# [iOS/Mac Catalyst](#tab/macios)

> [!IMPORTANT]
> Because of a namespace conflict, the `Contacts` type must be fully qualified when targeting iOS or macOS: `Microsoft.Maui.ApplicationModel.Communication.Contacts`.

- The `cancellationToken` parameter in the <xref:Microsoft.Maui.ApplicationModel.Communication.IContacts.GetAllAsync%2A> method isn't supported.
- The iOS platform doesn't support the <xref:Microsoft.Maui.ApplicationModel.Communication.Contact.DisplayName> property natively, thus, the `DisplayName` value is constructed as "<xref:Microsoft.Maui.ApplicationModel.Communication.Contact.GivenName> <xref:Microsoft.Maui.ApplicationModel.Communication.Contact.FamilyName>".

# [Windows](#tab/windows)

Picking a contact is unsupported on Windows.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->
