---
title: "Customize UI appearance based on the platform and device idiom"
description: "Learn how the OnPlatform and OnIdiom XAML markup extensions enable you to customize UI appearance on a per-platform and per-device basis."
ms.date: 04/18/2023
---

# Customize UI appearance based on the platform and device idiom

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/xaml-markupextensions)

.NET Multi-platform App UI (.NET MAUI) apps can have their UI customized for specific platforms and devices. This enables your app to:

- Make the most effective use of space. If you design an app to look good on a mobile device, the app will still be usable on a desktop device but there will most likely be some wasted space. You can customize your app to display more content when the screen is above a certain size. For example, a shopping app might display one item at a time on a mobile device, but might show multiple items on a desktop device. In addition, by placing more content on screen you can reduce the amount of navigation that users need to perform.
- Take advantage of device capabilities. Certain devices are more likely to have certain capabilities. For example, mobile devices are more likely to have a location sensor and a camera, while desktop devices might not have either. Your app can detect which capabilities are available and enable controls that use them.
- Optimize for input. You can rearrange your UI elements to optimize for specific input types. For example, if you place navigation elements at the bottom of the app, they'll be easier for mobile users to access. But desktop users often expect to see navigation elements towards the top of the app.

When you optimize your app's UI for specific platforms and device idioms, you're creating a responsive UI. The primary approaches to creating a responsive UI in .NET MAUI involve using the [`OnPlatform`](xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension) markup extension and the [`OnIdiom`](xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension) markup extension.

> [!NOTE]
> There is a category of triggers, known as state triggers, that can be used to customize UI appearance in specific scenarios such as when the orientation of a device changes. For more information, see [State trigger](~/fundamentals/triggers.md#state-trigger).

## Customize UI appearance based on the platform

The [`OnPlatform`](xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension) markup extension enables you to customize UI appearance on a per-platform basis. It provides the same functionality as the <xref:Microsoft.Maui.Controls.OnPlatform`1> and <xref:Microsoft.Maui.Controls.On> classes, but with a more concise representation.

The [`OnPlatform`](xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension) markup extension is supported by the <xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension> class, which defines the following properties:

- `Default`, of type `object`, that you set to a default value to be applied to the properties that represent platforms.
- `Android`, of type `object`, that you set to a value to be applied on Android.
- `iOS`, of type `object`, that you set to a value to be applied on iOS.
- `MacCatalyst`, of type `object`, that you set to a value to be applied on Mac Catalyst.
- `Tizen`, of type `object`, that you set to a value to be applied on the Tizen platform.
- `WinUI`, of type `object`, that you set to a value to be applied on WinUI.
- `Converter`, of type <xref:Microsoft.Maui.Controls.IValueConverter>, that can be set to an <xref:Microsoft.Maui.Controls.IValueConverter> implementation.
- `ConverterParameter`, of type `object`, that can be set to a value to pass to the <xref:Microsoft.Maui.Controls.IValueConverter> implementation.

> [!NOTE]
> The XAML parser allows the <xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension> class to be abbreviated as `OnPlatform`.

The `Default` property is the content property of <xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension>. Therefore, for XAML markup expressions expressed with curly braces, you can eliminate the `Default=` part of the expression if it's the first argument. If the `Default` property isn't set, it defaults to the `BindableProperty.DefaultValue` property value, provided that the markup extension is targeting a <xref:Microsoft.Maui.Controls.BindableProperty>.

> [!IMPORTANT]
> The XAML parser expects that values of the correct type will be provided to properties consuming the [`OnPlatform`](xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension) markup extension. If type conversion is necessary, the [`OnPlatform`](xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension) markup extension will attempt to perform it using the default converters provided by .NET MAUI. However, there are some type conversions that can't be performed by the default converters and in these cases the `Converter` property should be set to an <xref:Microsoft.Maui.Controls.IValueConverter> implementation.

The **OnPlatform Demo** page shows how to use the [`OnPlatform`](xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension) markup extension:

```xaml
<BoxView Color="{OnPlatform Yellow, iOS=Red, Android=Green}"
         WidthRequest="{OnPlatform 250, iOS=200, Android=300}"
         HeightRequest="{OnPlatform 250, iOS=200, Android=300}"
         HorizontalOptions="Center" />
```

In this example, all three [`OnPlatform`](xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension) expressions use the abbreviated version of the <xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension> class name. The three [`OnPlatform`](xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension) markup extensions set the `xref:Microsoft.Maui.Graphics.Color`, <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest>, and <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> properties of the <xref:Microsoft.Maui.Controls.BoxView> to different values on iOS and Android. The markup extensions also provide default values for these properties on the platforms that aren't specified, while eliminating the `Default=` part of the expression.

## Customize UI appearance based on the device idiom

The [`OnIdiom`](xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension) markup extension enables you to customize UI appearance based on the idiom of the device the app is running on. It's supported by the <xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension> class, which defines the following properties:

- `Default`, of type `object`, that you set to a default value to be applied to the properties that represent device idioms.
- `Phone`, of type `object`, that you set to a value to be applied on phones.
- `Tablet`, of type `object`, that you set to a value to be applied on tablets.
- `Desktop`, of type `object`, that you set to a value to be applied on desktop platforms.
- `TV`, of type `object`, that you set to a value to be applied on TV platforms.
- `Watch`, of type `object`, that you set to a value to be applied on Watch platforms.
- `Converter`, of type <xref:Microsoft.Maui.Controls.IValueConverter>, that can be set to an <xref:Microsoft.Maui.Controls.IValueConverter> implementation.
- `ConverterParameter`, of type `object`, that can be set to a value to pass to the <xref:Microsoft.Maui.Controls.IValueConverter> implementation.

> [!NOTE]
> The XAML parser allows the <xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension> class to be abbreviated as `OnIdiom`.

The `Default` property is the content property of <xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension>. Therefore, for XAML markup expressions expressed with curly braces, you can eliminate the `Default=` part of the expression if it's the first argument.

> [!IMPORTANT]
> The XAML parser expects that values of the correct type will be provided to properties consuming the [`OnIdiom`](xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension) markup extension. If type conversion is necessary, the [`OnIdiom`](xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension) markup extension will attempt to perform it using the default converters provided by .NET MAUI. However, there are some type conversions that can't be performed by the default converters and in these cases the `Converter` property should be set to an <xref:Microsoft.Maui.Controls.IValueConverter> implementation.

The following XAML example shows how to use the [`OnIdiom`](xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension) markup extension:

```xaml
<BoxView Color="{OnIdiom Yellow, Phone=Red, Tablet=Green, Desktop=Blue}"
         WidthRequest="{OnIdiom 100, Phone=200, Tablet=300, Desktop=400}"
         HeightRequest="{OnIdiom 100, Phone=200, Tablet=300, Desktop=400}"
         HorizontalOptions="Center" />
```

In this example, all three [`OnIdiom`](xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension) expressions use the abbreviated version of the <xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension> class name. The three [`OnIdiom`](xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension) markup extensions set the `Color`, <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest>, and <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> properties of the <xref:Microsoft.Maui.Controls.BoxView> to different values on the phone, tablet, and desktop idioms. The markup extensions also provide default values for these properties on the idioms that aren't specified, while eliminating the `Default=` part of the expression.
