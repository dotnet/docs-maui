---
title: "Information property list"
description: Learn about the Apple information property list file, Info.plist, that contains configuration information for your .NET MAUI app on iOS and Mac Catalyst.
ms.date: 08/27/2024
ms.custom: sfi-image-nochange
---

# Information property list

An information property list file is an XML file encoded using Unicode UTF-8 that contains configuration information for your .NET Multi-platform App UI (.NET MAUI) app on iOS and Mac Catalyst. The root node of the file is a dictionary, which contains a set of keys and values that define your app configuration. The name of the information property list file is *Info.plist*, and is case sensitive. All .NET MAUI iOS and Mac Catalyst apps must contain an *Info.plist* file that describes the app.

.NET MAUI creates *Info.plist* files for iOS and Mac Catalyst when you create a .NET MAUI app from a project template. These files are located in the *Platforms/iOS* and *Platforms/Mac Catalyst* folders, and are populated with an initial list of property list keys.

When you build your app, .NET MAUI copies your *Info.plist* file into the compiled bundle, before code signing the bundle. During the copy operation, .NET MAUI uses build properties to perform some variable substitution. It can also insert additional keys representing configuration that's specified in other ways. Due to this, the information property list file that ships in your app bundle isn't identical to the source file in your project.

## Edit the information property list in the editor

Double-clicking an *Info.plist* file will open it in Visual Studio's Info.plist editor, which contains two views of the data:

- Application, which enables you to set common app properties:

    :::image type="content" source="media/info-plist/vs/application.png" alt-text="Screenshot of application tab in Visual Studio Info.plist editor.":::

    > [!NOTE]
    > Values for the **Application Name**, **Bundle Identifier**, **Version**, and **Build** fields are retrieved from your app's project file. For more information, see [Provide app info](#provide-app-info).

- Advanced, which enables you to specify supported document types, universal type identifiers (UTIs), and URL types:

    :::image type="content" source="media/info-plist/vs/advanced.png" alt-text="Screenshot of advanced tab in Visual Studio Info.plist editor.":::

## Edit the information property list source

The *Info.plist* file can also be opened in an external editor to edit its XML source. Keys and values to configure the app can be added for the following categories:

- Bundle configuration, to configure the basic characteristics of a bundle such as its name, type, and version. For more information, see [Bundle configuration](https://developer.apple.com/documentation/bundleresources/information_property_list/bundle_configuration) on developer.apple.com.
- User interface, to configure an app's scenes, icons, and fonts. For more information, see [User interface](https://developer.apple.com/documentation/bundleresources/information_property_list/user_interface) on developer.apple.com.
- App execution, to configure app launch, execution, and termination. For more information, see [App execution](https://developer.apple.com/documentation/bundleresources/information_property_list/app_execution) on developer.apple.com.
- Protected resources, to control an app's access to protected services and user data. For more information, see [Protected resources](https://developer.apple.com/documentation/bundleresources/information_property_list/protected_resources) on developer.apple.com.
- Data and storage, to configure your app's data management capabilities. For more information, see [Data and storage](https://developer.apple.com/documentation/bundleresources/information_property_list/data_and_storage) on developer.apple.com.
- App services, to configure the services that your app provides. For more information, see [App services](https://developer.apple.com/documentation/bundleresources/information_property_list/app_services) on developer.apple.com.
- Kernel and drivers, to configure device drivers provided by your app. For more information, see [Kernel and drivers](https://developer.apple.com/documentation/bundleresources/information_property_list/kernel_and_drivers) on developer.apple.com.

## Provide app info

The *Info.plist* editor retrieves basic app data from the app's project file, rather than storing it directly in the *Info.plist* file. At build time, .NET MAUI copies this data into the *Info.plist* file that ships in your app bundle.

### Application name

The application name for a .NET MAUI app is stored in the app's project file as the `ApplicationTitle` build property.

In **Solution Explorer**, right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **MAUI Shared > General** tab. The **Application Title** field lists the application name.

When the value of the **Application Title** field is updated, the value of the **Application Name** field in the application view in the *Info.plist* file will be automatically updated.

### Application ID

The bundle identifier for a .NET MAUI app is stored in the app's project file as the `ApplicationId` build property.

In **Solution Explorer**, right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **MAUI Shared > General** tab. The **Application ID** field lists the bundle identifier.

When the value of the **Application ID** field is updated, the value of the **Bundle Identifier** field in the application view in the *Info.plist* file will be automatically updated.

### Application display version

The application display version for a .NET MAUI app is stored in the app's project file as the `ApplicationDisplayVersion` build property.

In **Solution Explorer**, right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **MAUI Shared > General** tab. The **Application Display Version** field lists the application display version.

When the value of the **Application Display Version** field is updated, the value of the **Version** field in the application view in the *Info.plist* file will be automatically updated.

### Application version

The application version for a .NET MAUI app is stored in the app's project file as the `ApplicationVersion` build property.

In **Solution Explorer**, right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **MAUI Shared > General** tab. The **Application Version** field lists the application version.

When the value of the **Application Version** field is updated, the value of the **Build** field in the application view in the *Info.plist* file will be automatically updated.

## See also

- [Information Property List](https://developer.apple.com/documentation/bundleresources/information_property_list) on developer.apple.com
