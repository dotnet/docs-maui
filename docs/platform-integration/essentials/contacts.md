---
title: "Contacts"
description: "Learn how to use the .MET MAUI Contacts class in Microsoft.Maui.Essentials namespace, which lets a pick a contact and retrieve information about it."
ms.date: 08/16/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Contacts

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `Contacts` class to select a contact and read information about it.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

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

  \- or -

- Use the Android project properties:

  Right-click on the Android project and open the project's properties. Under **Android Manifest** find the **Required permissions:** area and check the **Contacts** permission. This will automatically update the **AndroidManifest.xml** file.

# [iOS](#tab/ios)

In your _Info.plist_ file, add the following keys:

```xml
<key>NSContactsUsageDescription</key>
<string>This app needs access to contacts to pick a contact and get info.</string>
```

The `<string>` element is the description specific to your app and is shown to the user.

# [Windows](#tab/windows)

In the _Package.appxmanifest_, under **Capabilities**, ensure that the **Contact** capability is checked.

-----
<!-- markdownlint-enable MD025 -->

## Pick a contact

You can request the user to pick a contact by calling the `Contacts.PickContactAsync` method. A contact dialog will appear on the device allowing the user to select a contact. If the user doesn't select a contact, `null` is returned.

```csharp
try
{
    var contact = await Contacts.PickContactAsync();

    if(contact == null)
        return;

    var id = contact.Id;
    var namePrefix = contact.NamePrefix;
    var givenName = contact.GivenName;
    var middleName = contact.MiddleName;
    var familyName = contact.FamilyName;
    var nameSuffix = contact.NameSuffix;
    var displayName = contact.DisplayName;
    var phones = contact.Phones; // List of phone numbers
    var emails = contact.Emails; // List of email addresses
}
catch (Exception ex)
{
    // Handle exception here.
}
```

## Get all contacts

The `Contacts.GetAllAsync` method returns a collection of contacts.

```csharp
ObservableCollection<Contact> contactsCollect = new ObservableCollection<Contact>();

try
{
    // cancellationToken parameter is optional
    var cancellationToken = default(CancellationToken);
    var contacts = await Contacts.GetAllAsync(cancellationToken);

    if (contacts == null)
        return;

    foreach (var contact in contacts)
        contactsCollect.Add(contact);
}
catch (Exception ex)
{
    // Handle exception here.
}
```

## Platform differences

This section describes the platform-specific differences with the contacts API.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
# [Android](#tab/android)

- The `cancellationToken` parameter in the `GetAllAsync` method is only used on Windows.

# [iOS](#tab/ios)

- The `cancellationToken` parameter in the `GetAllAsync` method is only used on Windows.
- The iOS platform doesn't support the `DisplayName` property natively, thus, the `DisplayName` value is constructed as "GivenName FamilyName".

# [Windows](#tab/windows)

No platform differences.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->

## API

- [Contacts source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/Contacts)
<!-- - [Contacts API documentation](xref:Microsoft.Maui.Essentials.Contacts)-->
