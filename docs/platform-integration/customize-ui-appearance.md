---
title: "Customize UI appearance based on the platform and device idiom"
description: "Learn how the OnPlatform and OnIdiom XAML markup extensions enable you to customize UI appearance on a per-platform and per-device basis."
ms.date: 12/03/2024
---

# Customize UI appearance based on the platform and device idiom

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/xaml-markupextensions)

.NET Multi-platform App UI (.NET MAUI) apps can have their UI customized for specific platforms and devices. This enables your app to:

- Make the most effective use of space. If you design an app to look good on a mobile device, the app will still be usable on a desktop device but there will most likely be some wasted space. You can customize your app to display more content when the screen is above a certain size. For example, a shopping app might display one item at a time on a mobile device, but might show multiple items on a desktop device. In addition, by placing more content on screen you can reduce the amount of navigation that users need to perform.
- Take advantage of device capabilities. Certain devices are more likely to have certain capabilities. For example, mobile devices are more likely to have a location sensor and a camera, while desktop devices might not have either. Your app can detect which capabilities are available and enable controls that use them.
- Optimize for input. You can rearrange your UI elements to optimize for specific input types. For example, if you place navigation elements at the bottom of the app, they'll be easier for mobile users to access. But desktop users often expect to see navigation elements towards the top of the app.

When you optimize your app's UI for specific platforms and device idioms, you're creating a responsive UI. The primary approach to creating a responsive UI in .NET MAUI involves using the <xref:Microsoft.Maui.Controls.OnPlatform`1> and <xref:Microsoft.Maui.Controls.OnIdiom`1> classes. An alternative approach is to use the [`OnPlatform`](xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension) and [`OnIdiom`](xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension) XAML markup extensions. However, these markup extensions aren't trim safe. For more information about the markup extensions, see [Customize UI appearance with markup extensions](#customize-ui-appearance-with-markup-extensions).

> [!NOTE]
> There is a category of triggers, known as state triggers, that can be used to customize UI appearance in specific scenarios such as when the orientation of a device changes. For more information, see [State trigger](~/fundamentals/triggers.md#state-trigger).

## Customize UI appearance based on the platform

The <xref:Microsoft.Maui.Controls.OnPlatform`1> and <xref:Microsoft.Maui.Controls.On> classes enable you to customize UI appearance on a per-platform basis:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage">
    <ContentPage.Padding>
        <OnPlatform x:TypeArguments="Thickness">
            <On Platform="iOS" Value="0,20,0,0" />
            <On Platform="Android" Value="10,20,20,10" />
        </OnPlatform>
    </ContentPage.Padding>
    ...
</ContentPage>
```

<xref:Microsoft.Maui.Controls.OnPlatform`1> is a generic class and so you need to specify the generic type argument, in this case, <xref:Microsoft.Maui.Thickness>, which is the type of `Padding` property. This is achieved with the `x:TypeArguments` XAML attribute. The <xref:Microsoft.Maui.Controls.OnPlatform`1> class defines a <xref:Microsoft.Maui.Controls.OnPlatform`1.Default> property that can be set to a value that will be applied to all platforms:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage">
    <ContentPage.Padding>
        <OnPlatform x:TypeArguments="Thickness" Default="20">
            <On Platform="iOS" Value="0,20,0,0" />
            <On Platform="Android" Value="10,20,20,10" />
        </OnPlatform>
    </ContentPage.Padding>
    ...
</ContentPage>
```

In this example, the `Padding` property is set to different values on iOS and Android, with the other platforms being set to the default value.

The <xref:Microsoft.Maui.Controls.OnPlatform`1> class also defines a <xref:Microsoft.Maui.Controls.OnPlatform`1.Platforms> property, which is an `IList` of <xref:Microsoft.Maui.Controls.On> objects. Each <xref:Microsoft.Maui.Controls.On> object can set the <xref:Microsoft.Maui.Controls.On.Platform> and <xref:Microsoft.Maui.Controls.On.Value> property to define the <xref:Microsoft.Maui.Thickness> value for a specific platform. In addition, the <xref:Microsoft.Maui.Controls.On.Platform?displayProperty=nameWithType> property is of type `IList<string>`, so you can include multiple comma-delimited platforms if the values are the same:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage">
    <ContentPage.Padding>
        <OnPlatform x:TypeArguments="Thickness" Default="20">
            <On Platform="iOS, Android" Value="10,20,20,10" />
        </OnPlatform>
    </ContentPage.Padding>
    ...
</ContentPage>
```

Providing an incorrect <xref:Microsoft.Maui.Controls.On.Platform> value won't result in an error. Instead, your XAML will execute without the platform-specific value being applied.

> [!NOTE]
> If the `Value` property of an `On` object can't be represented by a single string, you can define property elements for it.

## Customize UI appearance based on the device idiom

The <xref:Microsoft.Maui.Controls.OnIdiom`1> class enables you to customize UI appearance based on the idiom of the device the app is running on:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage">
    <ContentPage.Margin>
        <OnIdiom x:TypeArguments="Thickness">
            <OnIdiom.Phone>0,20,0,0</OnIdiom.Phone>
            <OnIdiom.Tablet>0,40,0,0</OnIdiom.Tablet>
            <OnIdiom.Desktop>0,60,0,0</OnIdiom.Desktop>
        </OnPlatform>
    </ContentPage.Margin>
    ...
</ContentPage>
```

<xref:Microsoft.Maui.Controls.OnIdiom`1> is a generic class and so you need to specify the generic type argument, in this case, <xref:Microsoft.Maui.Thickness>, which is the type of `Margin` property. This is achieved with the `x:TypeArguments` XAML attribute. The <xref:Microsoft.Maui.Controls.OnIdiom`1> class defines a <xref:Microsoft.Maui.Controls.OnIdiom`1.Default> property that can be set to a value that will be applied to all platforms:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage">
    <ContentPage.Margin>
        <OnIdiom x:TypeArguments="Thickness" Default="20">
            <OnIdiom.Desktop>0,60,0,0</OnIdiom.Desktop>
        </OnPlatform>
    </ContentPage.Margin>
    ...
</ContentPage>
```

In this example, the `Margin` property is set to a specific value on desktop idioms, with the other idioms being set to the default value.

## Customize UI appearance with markup extensions

UI appearance can also be customized with the [`OnPlatform`](xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension) and [`OnIdiom`](xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension) markup extensions. However, these markup extensions aren't trim safe.

### Customize UI appearance with a markup extension based on the platform

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

The following XAML example shows how to use the [`OnPlatform`](xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension) markup extension:

```xaml
<BoxView Color="{OnPlatform Yellow, iOS=Red, Android=Green}"
         WidthRequest="{OnPlatform 250, iOS=200, Android=300}"
         HeightRequest="{OnPlatform 250, iOS=200, Android=300}"
         HorizontalOptions="Center" />
```

In this example, all three [`OnPlatform`](xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension) expressions use the abbreviated version of the <xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension> class name. The three [`OnPlatform`](xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension) markup extensions set the <xref:Microsoft.Maui.Graphics.Color>, <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest>, and <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> properties of the <xref:Microsoft.Maui.Controls.BoxView> to different values on iOS and Android. The markup extensions also provide default values for these properties on the platforms that aren't specified, while eliminating the `Default=` part of the expression.

::: moniker range=">=net-maui-9.0"

> [!WARNING]
> The [`OnPlatform`](xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension) markup extension isn't trim safe and shouldn't be used with full trimming or NativeAOT. Instead, you should use the <xref:Microsoft.Maui.Controls.OnPlatform`1> class to customize UI appearance on a per-platform basis. For more information, see [Customize UI appearance based on the platform](#customize-ui-appearance-based-on-the-platform), [Trim a .NET MAUI app](~/deployment/trimming.md) and [Native AOT deployment](~/deployment/nativeaot.md).

::: moniker-end

### Customize UI appearance with a markup extension based on the device idiom

The [`OnIdiom`](xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension) markup extension enables you to customize UI appearance based on the idiom of the device the app is running on. It provides the same functionality as the <xref:Microsoft.Maui.Controls.OnIdiom`1> class, but with a more concise representation.

The [`OnIdiom`](xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension) markup extension is supported by the <xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension> class, which defines the following properties:

- `Default`, of type `object`, that you set to a default value to be applied to the properties that represent device idioms.
- `Phone`, of type `object`, that you set to a value to be applied on phones.
- `Tablet`, of type `object`, that you set to a value to be applied on tablets. This property isn't exclusive to Android and iOS platforms.
- `Desktop`, of type `object`, that you set to a value to be applied on desktop platforms. Note that some laptops may be classified using the `Tablet` property.
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

In this example, all three [`OnIdiom`](xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension) expressions use the abbreviated version of the <xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension> class name. The three [`OnIdiom`](xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension) markup extensions set the <xref:Microsoft.Maui.Graphics.Color>, <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest>, and <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> properties of the <xref:Microsoft.Maui.Controls.BoxView> to different values on the phone, tablet, and desktop idioms. The markup extensions also provide default values for these properties on the idioms that aren't specified, while eliminating the `Default=` part of the expression.

::: moniker range=">=net-maui-9.0"

> [!WARNING]
> The [`OnIdiom`](xref:Microsoft.Maui.Controls.Xaml.OnIdiomExtension) markup extension isn't trim safe and shouldn't be used with full trimming or NativeAOT. Instead, you should use the <xref:Microsoft.Maui.Controls.OnIdiom`1> class to customize UI appearance based on the idiom of the device the app is running on. For more information, see [Customize UI appearance based on the device idiom](#customize-ui-appearance-based-on-the-device-idiom), [Trim a .NET MAUI app](~/deployment/trimming.md) and [Native AOT deployment](~/deployment/nativeaot.md).

::: moniker-end
