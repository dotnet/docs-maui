---
title: "Localization"
description: "Learn how to localize .NET MAUI apps using .NET resource files."
ms.date: 08/07/2023
---

# Localization

Localization is the process of adapting an app to meet the specific language or cultural requirements of a target market. To accomplish localization, the text and images in an app may need to be translated into multiple languages. A localized app automatically displays translated text based on the culture settings of the device.

.NET includes a mechanism for localizing apps using [resource files](/dotnet/core/extensions/create-resource-files). A resource file stores text and other content as name/value pairs that allow the app to retrieve content for a provided key. Resource files allow localized content to be separated from app code.

To localize a .NET Multi-platform App UI (.NET MAUI) app using resource files you should:

1. Create resource files. For more information, see []().
1. Specify the app's default culture. For more information, see []().
1. Perform platform setup. For more information, see []().
1. Localize text. For more information, see []().
1. Localize images. For more information, see []().
1. Localize the app name. For more information, see []().
1. Test localization. For more information, see []().

## Create resource files

Resource files are XML files with a *.resx* extension that are compiled into binary resource (*.resources*) files during the build process. A localized app typically contains a default resource file with all strings used in the app, as well as resource files for each supported language.

Resource files contain the following information for each item:

- **Name** specifies the key used to access the text in code.
- **Value** specifies the translated text.
- **Comment** is an optional field containing additional information.

A resource file can be added with the **Add New Item** dialog in Visual Studio:

:::image type="content" source="media/localization/add-resource-file.png" alt-text="Screenshot of adding a resource file in Visual Studio.":::

Once the file is added, rows can be added for each text resource:

:::image type="content" source="media/localization/default-strings.png" alt-text="Screenshot of the default strings for the app.":::

The **Access Modifier** drop-down determines how Visual Studio generates the class used to access resources. Setting the Access Modifier to **Public** or **Internal** results in a generated class with the specified accessibility level. Setting the Access Modifier to **No code generation** doesn't generate a class file. The default resource file should be configured to generate a class file, which results in a file with the *.Designer.cs* extension being added to the project.

Once the default resource file is created, additional files can be created for each locale the app supports. Each additional resource file should have the same root filename as the default resource file, but should also include the culture in the filename. For example, if you add a resource file named *AppResources.resx*, you might also create resource files named *AppResources.en-US.resx* and *AppResources.fr-FR.resx* to hold localized resources for the English (United States) and French (France) cultures, respectively. In addition, you should set the **Access Modifier** for each additional resource file to **No code generation**.

At runtime, your app attempts to resolve a resource request in order of specificity. For example, if the device culture is **en-US** the application looks for resource files in this order:

1. AppResources.en-US.resx
1. AppResources.en.resx
1. AppResources.resx (default)

The following screenshot shows a Spanish translation file named **AppResources.es.resx**:

:::image type="content" source="media/localization/spanish-strings.png" alt-text="Screenshot of the Spanish strings for the app.":::

The localized resource file uses the same **Name** values specified in the default file but contains Spanish language strings in the **Value** column. Additionally, the **Access Modifier** is set to **No code generation**.

## Specify the app's default culture

For resource files to work correctly, the app must have a default culture specified. This is the culture whose resources are used if no localized resources for a particular culture can be found. To specify the default culture:

1. In Solution Explorer in Visual Studio, right-click your .NET MAUI app project and select **Properties**.
1. Select the **Package** tab.
1. In the **General** area, select the appropriate language/culture from the **Assembly neutral language** control:

    :::image type="content" source="media/localization/neutral-language.png" alt-text="Screenshot of setting the neutral language for the assembly.":::

1. Save your changes.

Alternatively, add the `<NeutralLanguage>` node to the first `<PropertyGroup>` in your .csproj, and specify your locale as its value:

```xml
<NeutralLanguage>en-US</NeutralLanguage>
```

> [!WARNING]
> If you don'y specify a default culture, the <xref:System.Resources.ResourceManager> class returns `null` values for any cultures without a specific resource file. When a default culture is specified, the <xref:System.Resources.ResourceManager> class returns results from the default resource file for unsupported cultures. Therefore, it's recommended that you always specify a default culture so that text is displayed for unsupported cultures.

## Perform platform setup

Some platforms require additional setup so that all .NET MAUI controls are localized.

### iOS and Mac Catalyst

On iOS and Mac Catalyst, you must declare all supported languages in the *Info.plist* file for your .NET MAUI app project. To do this, open the *Info.plist* file in an XML editor and set an array for the `CFBundleLocalizations` key and provide values that correspond to the resource files. In addition, ensure you set an expected language via the `CFBundleDevelopmentRegion` key:

```xml
<key>CFBundleLocalizations</key>
<array>
    <string>de</string>
    <string>es</string>
    <string>fr</string>
    <string>ja</string>
    <string>pt</string> <!-- Brazil -->
    <string>pt-PT</string> <!-- Portugal -->
    <string>ru</string>
    <string>zh-Hans</string>
    <string>zh-Hant</string>
</array>
<key>CFBundleDevelopmentRegion</key>
<string>en</string>
```

Alternatively, in Solution Explorer in Visual Studio, open the *Info.plist* file for your chosen platform in the **Generic PList Editor**. Then, set an array for the `CFBundleLocalizations` key and provide values that correspond to the resource files. In addition, ensure you set an expected language via the `CFBundleDevelopmentRegion` key:

:::image type="content" source="media/localization/info-plist.png" alt-text="Screenshot of the supported locales for the app in the generic Info.plist editor.":::

### Windows

To support multiple languages in a .NET MAUI app on Windows you must declare each supported language in the *Package.appxmanifest* file in the *Resources\Windows* folder of your .NET MAUI app project:

1. Open the *Package.appxmanifest* file in a text editor and locate the following section:

    ```xml
    <Resources>
        <Resource Language="x-generate"/>
    </Resources>
    ```

1. Replace `<Resource Language="x-generate">` with `<Resource />` elements for each of your supported languages:

    ```xml
    <Resources>
        <Resource Language="en-US"/>      
        <Resource Language="de-DE"/>
        <Resource Language="es-ES"/>
        <Resource Language="fr-FR"/>
        <Resource Language="ja-JP"/>
        <Resource Language="pt-BR"/>
        <Resource Language="pt-PT"/>
        <Resource Language="ru-RU"/>
        <Resource Language="zh-CN"/>
        <Resource Language="zh-TW"/>        
    </Resources>
    ```

## Localize text

Text is localized using a class generated from your default resources file. The class is named based on the default resource filename. Given a default resource filename of *AppResources.resx*, Visual Studio generates a matching class named `AppResources` containing static properties for each entry in the resource file.

In XAML, localized text can be retrieved by using the `x:Static` markup extension to access the generated static properties:

```xaml
<ContentPage ...
             xmlns:strings="clr-namespace:LocalizationDemo.Resources.Strings">
    <VerticalStackLayout>
        <Label Text="{x:Static strings:AppResources.NotesLabel}" />
        <Entry Placeholder="{x:Static strings:AppResources.NotesPlaceholder}" />
        <Button Text="{x:Static strings:AppResources.AddButton}" />
    </VerticalStackLayout>
</ContentPage>
```

For more information about the `x:Static` markup extension, see [x:Static markup extension](~/xaml/fundamentals/markup-extensions.md#xstatic-markup-extension).

Localized text can also be retrieved in code:

```csharp
Label notesLabel = new Label();
notesLabel.Text = AppResources.NotesLabel,

Entry notesEntry = new Entry();
notesEntry.Placeholder = AppResources.NotesPlaceholder,

Button addButton = new Button();
addButton.Text = AppResources.AddButton,
```

The properties in the `AppResources` class use the <xref:System.Globalization.CultureInfo.CurrentUICulture> property value to determine which culture resource file to retrieve values from.

## Localize images

In addition to storing text, Resx files are capable of storing more than just text, they can also store images and binary data. However, mobile devices have a range of screen sizes and densities and each mobile platform has functionality for displaying density-dependent images. Therefore, platform image localization functionality should be used instead of storing images in resource files.

### Android

On Android, localized drawables (images) are stored using a naming convention for folders in the **Resources** directory. Folders are named **drawable** with a suffix for the target language. For example, the Spanish-language folder is named **drawable-es**.

When a four-letter locale code is required, Android requires an additional **r** following the dash. For example, the Mexico locale (es-MX) folder should be named **drawable-es-rMX**. The image file names in each locale folder should be identical:

![Localized images in the Android project](text-images/pc-android-images.png)

For more information, see [Android Localization](~/android/app-fundamentals/localization.md).

### iOS and Mac Catalyst

On iOS, localized images are stored using a naming convention for folders in the **Resources** directory. The default folder is named **Base.lproj**. Language-specific folders are named with the language or locale name, followed by **.lproj**. For example, the Spanish-language folder is named **es.lproj**.

Four-letter local codes work just like two-letter language codes. For example, the Mexico locale (es-MX) folder should be named **es-MX.lproj**. The image file names in each locale folder should be identical:

![Localized images in the iOS project](text-images/pc-ios-images.png)

> [!NOTE]
> iOS supports creating a localized Asset Catalog instead of using the .lproj folder structure. However, these must be created and managed in Xcode.

For more information, see [iOS Localization](~/ios/app-fundamentals/localization/index.md).

### Windows

On UWP, localized images are stored using a naming convention for folders in the **Assets/Images** directory. Folders are named with the language or locale. For example, the Spanish-language folder is named **es** and the Mexico locale folder should be named **es-MX**. The image file names in each locale folder should be identical:

![Localized images in the UWP project](text-images/pc-uwp-images.png)

For more information, see [UWP Localization](/windows/uwp/design/globalizing/globalizing-portal/).

### Consume localized images

Since each platform stores images with a unique file structure, the XAML uses the `OnPlatform` class to set the `ImageSource` property based on the current platform:

```xaml
<Image>
    <Image.Source>
        <OnPlatform x:TypeArguments="ImageSource">
            <On Platform="iOS, Android" Value="flag.png" />
            <On Platform="UWP" Value="Assets/Images/flag.png" />
        </OnPlatform>
    </Image.Source>
</Image>
```

> [!NOTE]
> The `OnPlatform` markup extension offers a more concise way of specifying platform-specific values. For more information, see [OnPlatform markup extension](~/xamarin-forms/xaml/markup-extensions/consuming.md#onplatform-markup-extension).

The image source can be set based on the `Device.RuntimePlatform` property in code:

```csharp
string imgSrc = Device.RuntimePlatform == Device.UWP ? "Assets/Images/flag.png" : "flag.png";
Image flag = new Image
{
    Source = ImageSource.FromFile(imgSrc),
    WidthRequest = 100
};
```

## Localize the app name

The application name is specified per-platform and does not use Resx resource files. To localize the application name on Android, see [Localize app name on Android](~/android/app-fundamentals/localization.md#stringsxml-file-format). To localize the application name on iOS, see [Localize app name on iOS](~/ios/app-fundamentals/localization/index.md#app-name). To localize the application name on UWP, see [Localize strings in the UWP package manifest](/windows/uwp/app-resources/localize-strings-ui-manifest).

## Test localization

At runtime, an app loads the appropriate localized resources on a per-thread basis, based on the culture specified by the <xref:System.Globalization.CultureInfo.CurrentUICulture> property.

Testing localization is best accomplished by changing your device language:

- On Android, this can be accomplished in the Settings app. In the Settings app, you can also set the language for each app without changing your device language.
- On iOS, this can be accomplished in the Settings app. In the Settings app, you can also set the language for each app without changing your device language.
- On Mac, this can be accomplished in System Settings. In System Settings, you can also set the language for each app without changing your device language.
- On Windows, this can be accomplished in Settings.

> [!WARNING]
> While it's possible to set the value of <xref:System.Globalization.CultureInfo.CurrentUICulture> in code, the resulting behavior is inconsistent across platforms so this isn't recommended for testing.

## Right-to-left localization

Flow direction, or layout direction, is the direction in which the UI elements on the page are scanned by the eye. Some languages, such as Arabic and Hebrew, require that UI elements are laid out in a right-to-left flow direction. .NET MAUI apps automatically respect the device's flow direction based on the selected language and region. For information about how to retrieve the flow direction of the device, based on its locale, see [Get the layout direction](~/platform-integration/appmodel/app-information.md#get-the-layout-direction).

To override the flow direction of an app, set the <xref:Microsoft.Maui.Controls.VisualElement.FlowDirection?displayProperty=nameWithType> property. This property gets or sets the direction in which UI elements flow within any parent element that controls their layout, and should be set to one of the <xref:Microsoft.Maui.FlowDirection> enumeration values:

- `LeftToRight`
- `RightToLeft`
- `MatchParent`

<!-- Setting the <xref:Microsoft.Maui.Controls.VisualElement.FlowDirection> property to `RightToLeft` on an element sets the alignment to the right, the reading order to right-to-left, and the layout of the control to flow from right-to-left. -->

> [!WARNING]
> Changing the <xref:Microsoft.Maui.Controls.VisualElement.FlowDirection> property at runtime causes an expensive layout process that will affect performance.

The default <xref:Microsoft.Maui.Controls.VisualElement.FlowDirection> property value for an element is `MatchParent`. Therefore, an element inherits the `FlowDirection` property value from its parent in the visual tree, and any element can override the value it gets from its parent.

> [!TIP]
> If you do need to change the flow direction, set the <xref:Microsoft.Maui.Controls.VisualElement.FlowDirection> property on a window, page or root layout. This causes all of the elements contained within the page, or root layout, to respond appropriately to the flow direction.

### Platform setup

Specific platform setup is required to enable right-to-left locales.

#### Android

App's created using the .NET MAUI app project template will automatically include support for right-to-left locales. This support is enabled by the `android:supportsRtl` attribute being set to `true` on the `<application>` node in the app's *AndroidManifest.xml* file:

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android">
    <application ... android:supportsRtl="true" />
    ...
</manifest>
```

Right-to-left localization can then be tested by changing the device or emulator to use the right-to-left language. Alternatively, provided that you've activated developer options in the Settings app, you can enable **Force RTL layout direction** in **Settings > Developer Options**. For information on configuring developer options, see [Configure on-device developer options](https://developer.android.com/studio/debug/dev-options) on developer.android.com.

#### iOS and Mac Catalyst

The required right-to-left locale should be added as a supported language to the array items for the `CFBundleLocalizations` key in *Info.plist*. The following example shows Arabic having been added to the array for the `CFBundleLocalizations` key:

```xml
<key>CFBundleLocalizations</key>
<array>
    <string>en</string>
    <string>ar</string>
</array>
```

Right-to-left localization can then be tested by changing the language and region on the device or simulator to a right-to-left locale that was specified in *Info.plist*.

#### Windows

The required language resources should be specified in the `<Resources>` node of the *Package.appxmanifest* file. Replace `<Resource Language="x-generate">` with `<Resource />` elemnts for each of your supported languages. For example, the following markup specifies that "en" and "ar" localized resources are available:

```xml
<Resources>
    <Resource Language="en" />
    <Resource Language="ar" />
</Resources>
```

Right-to-left localization can then be tested by changing the language and region on the device to the appropriate right-to-left locale.
