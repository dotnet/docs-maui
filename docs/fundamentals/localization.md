---
title: "Localization"
description: "Learn how to XXXXXXX."
ms.date: 08/07/2023
---

# Localization

.NET Multi-platform App UI (.NET MAUI)

## Right-to-left localization

Flow direction, or layout direction, is the direction in which the UI elements on the page are scanned by the eye. Some languages, such as Arabic and Hebrew, require that UI elements are laid out in a right-to-left flow direction. .NET MAUI apps automatically respect the device's flow direction based on the selected language and region. For information about how to retrieve the flow direction of the device, based on its locale, see [Get the layout direction](~/platform-integration/appmodel/app-information.md#get-the-layout-direction).

To override the flow direction of an app, set the <xref:Microsoft.Maui.Controls.VisualElement.FlowDirection?displayProperty=nameWithType> property. This property gets or sets the direction in which UI elements flow within any parent element that controls their layout, and should be set to one of the <xref:Microsoft.Maui.FlowDirection> enumeration values:

- `LeftToRight`
- `RightToLeft`
- `MatchParent`

Setting the <xref:Microsoft.Maui.Controls.VisualElement.FlowDirection> property to `RightToLeft` on an element sets the alignment to the right, the reading order to right-to-left, and the layout of the control to flow from right-to-left.

> [!WARNING]
> Changing the <xref:Microsoft.Maui.Controls.VisualElement.FlowDirection> property at runtime causes an expensive layout process that will affect performance.

The default <xref:Microsoft.Maui.Controls.VisualElement.FlowDirection> property value for an element is `MatchParent`. Therefore, an element inherits the `FlowDirection` property value from its parent in the visual tree, and any element can override the value it gets from its parent.

> [!TIP]
> If you do need to change the flow direction, set the <xref:Microsoft.Maui.Controls.VisualElement.FlowDirection> property on a page or root layout. This causes all of the elements contained within the page, or root layout, to respond appropriately to the flow direction.

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
