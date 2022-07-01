---
title: "Contacts"
description: "Learn how to use the .NET MAUI Contacts class in the Microsoft.Maui.ApplicationModel.Communication namespace, which lets a pick a contact and retrieve information about it."
ms.date: 05/25/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel.Communication"]
---

# Contacts

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IContacts` interface to select a contact and read information about it.
The `IContacts` interface is exposed through the `Contacts.Default` property.

The `Contacts` and `IContacts` types are available in the `Microsoft.Maui.ApplicationModel.Communication` namespace.

> [!IMPORTANT]
> Because of a namespace conflict, the `Contacts` type must be fully qualified when targeting iOS or macOS: `Microsoft.Maui.ApplicationModel.Communication.Contacts`. New projects automatically target these platforms, along with Android and Windows.
>
> To write code that will compile for iOS and macOS, fully qualify the `Contacts` type. Alternatively, provide a `using` directive to map the `Communication` namespace:
>
> ```csharp
> using Communication = Microsoft.Maui.ApplicationModel.Communication;
>
> // Code that uses the namespace:
> var contact = await Communication.Contacts.Default.PickContactAsync();
> ```

## Get started

To access the **Contacts** functionality the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

The `ReadContacts` permission is required and must be configured in the Android project. This can be added in the following ways:

- Add the assembly-based permission:

  Open the _AssemblyInfo.cs_ file under the **Properties** folder and add:

  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.ReadContacts)]
  ```

  \- or -

- Update the Android Manifest:

  Open the _AndroidManifest.xml_ file under the **Properties** folder and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.READ_CONTACTS" />
  ```

<!-- TODO not yet supported>

  \- or -

- Use the Android project properties:

  Right-click on the Android project and open the project's properties. Under **Android Manifest** find the **Required permissions:** area and check the **Contacts** permission. This will automatically update the **AndroidManifest.xml** file.

-->

# [iOS\macOS](#tab/ios)

In the **Solution Explorer** pane, right-click on the _Platforms/iOS/Info.plist_ file. Select **Open With** and then select the **XML (Text) Editor** item. Press the **OK** button. In the file, add the following key and value:

```xml
<key>NSContactsUsageDescription</key>
<string>This app needs access to contacts to pick a contact and get info.</string>
```

The `<string>` element is the description specific to your app and is shown to the user.

# [Windows](#tab/windows)

In the **Solution Explorer** pane, right-click on the _Platforms/Windows/Package.appxmanifest_ file, and select **View Code**. Under the `<Capabilities>` node, add `<uap:Capability Name="contacts"/>`.

-----
<!-- markdownlint-enable MD025 -->

## Pick a contact

You can request the user to pick a contact by calling the `PickContactAsync` method. A contact dialog will appear on the device allowing the user to select a contact. If the user doesn't select a contact, `null` is returned.

:::code language="csharp" source="../snippets/shared_1/CommsPage.xaml.cs" id="contact_select":::

## Get all contacts

The `GetAllAsync` method returns a collection of contacts.

:::code language="csharp" source="../snippets/shared_1/CommsPage.xaml.cs" id="contact_all":::

## Platform differences

This section describes the platform-specific differences with the contacts API.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
# [Android](#tab/android)

- The `cancellationToken` parameter in the `GetAllAsync` method isn't supported.

# [iOS\macOS](#tab/ios)

> [!IMPORTANT]
> Because of a namespace conflict, the `Contacts` type must be fully qualified when targeting iOS or macOS: `Microsoft.Maui.ApplicationModel.Communication.Contacts`.

- The `cancellationToken` parameter in the `GetAllAsync` method isn't supported.
- The iOS platform doesn't support the `DisplayName` property natively, thus, the `DisplayName` value is constructed as "GivenName FamilyName".

# [Windows](#tab/windows)

No platform differences.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->
