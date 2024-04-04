---
title: "Fonts in .NET MAUI"
description: "This article explains how to specify font information on controls that display text in .NET MAUI apps."
ms.date: 12/23/2021
---

# Fonts in .NET MAUI

By default, .NET Multi-platform App UI (.NET MAUI) apps use the Open Sans font on each platform. However, this default can be changed, and additional fonts can be registered for use in an app.

All controls that display text define properties that can be set to change font appearance:

- `FontFamily`, of type `string`.
- `FontAttributes`, of type `FontAttributes`, which is an enumeration with three members: `None`, `Bold`, and `Italic`. The default value of this property is `None`.
- `FontSize`, of type `double`.
- `FontAutoScalingEnabled`, of type `bool`, which defines whether an app's UI reflects text scaling preferences set in the operating system. The default value of this property is `true`.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

All controls that display text automatically use font scaling, which means that an app's UI reflects text scaling preferences set in the operating system.

## Register fonts

True type format (TTF) and open type font (OTF) fonts can be added to your app and referenced by filename or alias, with registration being performed in the `CreateMauiApp` method in the `MauiProgram` class. This is accomplished by invoking the `ConfigureFonts` method on the `MauiAppBuilder` object. Then, on the `IFontCollection` object, call the `AddFont` method to add the required font to your app:

```csharp
namespace MyMauiApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Lobster-Regular.ttf", "Lobster");
                });

            return builder.Build();
        }
    }  
}
```

In the example above, the first argument to the `AddFont` method is the font filename, while the second argument represents an optional alias by which the font can be referenced when consuming it.

A font can be added to your app project by dragging it into the *Resources\Fonts* folder of the project, where its build action will automatically be set to **MauiFont**. This creates a corresponding entry in your project file. Alternatively, all fonts in the app can be registered by using a wildcard in your project file:

```xml
<ItemGroup>
   <MauiFont Include="Resources\Fonts\*" />
</ItemGroup>
```

Fonts can also be added to other folders of your app project. However, in this scenario their build action must be manually set to **MauiFont** in the **Properties** window.

At build time, fonts are copied to your app package. For information about disabling font packaging, see [Disable font packaging](~/troubleshooting.md#disable-font-packaging).

> [!NOTE]
> The `*` wildcard character indicates that all the files within the folder will be treated as being font files. In addition, if you want to include files from sub-folders too, then configure it using additional wildcard characters, for example, `Resources\Fonts\**\*`.

## Consume fonts

Registered fonts can be consumed by setting the `FontFamily` property of a control that displays text to the font name, without the file extension:

```xaml
<!-- Use font name -->
<Label Text="Hello .NET MAUI"
       FontFamily="Lobster-Regular" />
```

Alternatively, it can be consumed by referencing its alias:

```xaml
<!-- Use font alias -->
<Label Text="Hello .NET MAUI"
       FontFamily="Lobster" />
```

The equivalent C# code is:

```csharp
// Use font name
Label label1 = new Label
{
    Text = "Hello .NET MAUI!",
    FontFamily = "Lobster-Regular"
};

// Use font alias
Label label2 = new Label
{
    Text = "Hello .NET MAUI!",
    FontFamily = "Lobster"
};
```

::: moniker range=">=net-maui-8.0"

On Android, the following system fonts can be consumed by setting them as the value of the `FontFamily` property:

- monospace
- serif
- sans-serif (or sansserif)
- sans-serif-black (or sansserif-black)
- sans-serif-condensed (or sansserif-condensed)
- sans-serif-condensed-light (or sansserif-condensed-light)
- sans-serif-light (or sansserif-light)
- sans-serif-medium (or sansserif-medium)

For example, the monospace system font can be consumed with the following XAML:

```xaml
<Label Text="Hello .NET MAUI"
       FontFamily="monospace" />
```

The equivalent C# code is:

```csharp
// Use font name
Label label1 = new Label
{
    Text = "Hello .NET MAUI!",
    FontFamily = "monospace"
};
```

::: moniker-end

## Set font attributes

Controls that display text can set the `FontAttributes` property to specify font attributes:

```xaml
<Label Text="Italics"
       FontAttributes="Italic" />
<Label Text="Bold and italics"
       FontAttributes="Bold, Italic" />
```

The equivalent C# code is:

```csharp
Label label1 = new Label
{
    Text = "Italics",
    FontAttributes = FontAttributes.Italic
};

Label label2 = new Label
{
    Text = "Bold and italics",
    FontAttributes = FontAttributes.Bold | FontAttributes.Italic
};    
```

## Set the font size

Controls that display text can set the `FontSize` property to specify the font size. The `FontSize` property can be set to a `double` value:

```xaml
<Label Text="Font size 24"
       FontSize="24" />
```

The equivalent C# code is:

```csharp
Label label = new Label
{
    Text = "Font size 24",
    FontSize = 24
};
```

> [!NOTE]
> The `FontSize` value is measured in device-independent units. <!--For more information, see [Units of Measurement](~/xamarin-forms/user-interface/controls/common-properties.md#units-of-measurement).-->

## Disable font auto scaling

All controls that display text have font scaling enabled by default, which means that an app's UI reflects text scaling preferences set in the operating system. However, this behavior can be disabled by setting the `FontAutoScalingEnabled` property on text-based control's to `false`:

```xaml
<Label Text="Scaling disabled"
       FontSize="18"
       FontAutoScalingEnabled="False" />
```

This approach is useful when you want to guarantee that text is displayed at a specific size.

> [!NOTE]
> Font auto scaling also works with font icons. For more information, see [Display font icons](#display-font-icons).

<!-- Todo: Breaking change on iOS because all numbered fonts will scale. Disable by implementing IFontManager that inherits from MAUI, and explicitly implement GetFont to influence the font that's returned. -->

## Set font properties per platform

The `OnPlatform` and `On` classes can be used in XAML to set font properties per platform. The example below sets different font families and sizes:

```xaml
<Label Text="Different font properties on different platforms"
       FontSize="{OnPlatform iOS=20, Android=22, WinUI=24}">
    <Label.FontFamily>
        <OnPlatform x:TypeArguments="x:String">
            <On Platform="iOS" Value="MarkerFelt-Thin" />
            <On Platform="Android" Value="Lobster-Regular" />
            <On Platform="WinUI" Value="ArimaMadurai-Black" />
        </OnPlatform>
    </Label.FontFamily>
</Label>
```

The `DeviceInfo.Platform` property can be used in code to set font properties per platform:

```csharp
Label label = new Label
{
    Text = "Different font properties on different platforms"
};

label.FontSize = DeviceInfo.Platform == DevicePlatform.iOS ? 20 :
    DeviceInfo.Platform == DevicePlatform.Android ? 22 : 24;  
label.FontFamily = DeviceInfo.Platform == DevicePlatform.iOS ? "MarkerFelt-Thin" :
    DeviceInfo.Platform == DevicePlatform.Android ? "Lobster-Regular" : "ArimaMadurai-Black";
```

For more information about providing platform-specific values, see [Device information](~/platform-integration/device/information.md). For information about the `OnPlatform` markup extension, see [Customize UI appearance based on the platform](~/platform-integration/customize-ui-appearance.md#customize-ui-appearance-based-on-the-platform).

## Display font icons

Font icons can be displayed by .NET MAUI apps by specifying the font icon data in a <xref:Microsoft.Maui.Controls.FontImageSource> object. This class, which derives from the <xref:Microsoft.Maui.Controls.ImageSource> class, has the following properties:

- `Glyph` – the unicode character value of the font icon, specified as a `string`.
- `Size` – a `double` value that indicates the size, in device-independent units, of the rendered font icon. The default value is 30. In addition, this property can be set to a named font size.
- `FontFamily` – a `string` representing the font family to which the font icon belongs.
- `Color` – an optional <xref:Microsoft.Maui.Graphics.Color> value to be used when displaying the font icon.

This data is used to create a PNG, which can be displayed by any view that can display an <xref:Microsoft.Maui.Controls.ImageSource>. This approach permits font icons, such as emojis, to be displayed by multiple views, as opposed to limiting font icon display to a single text presenting view, such as a <xref:Microsoft.Maui.Controls.Label>.

> [!IMPORTANT]
> Font icons can only currently be specified by their unicode character representation.

The following XAML example has a single font icon being displayed by an <xref:Microsoft.Maui.Controls.Image> view:

```xaml
<Image BackgroundColor="#D1D1D1">
    <Image.Source>
        <FontImageSource Glyph="&#xf30c;"
                         FontFamily="{OnPlatform iOS=Ionicons, Android=ionicons.ttf#}"
                         Size="44" />
    </Image.Source>
</Image>
```

This code displays an XBox icon, from the Ionicons font family, in an <xref:Microsoft.Maui.Controls.Image> view. Note that while the unicode character for this icon is `\uf30c`, it has to be escaped in XAML and so becomes `&#xf30c;`. The equivalent C# code is:

```csharp
Image image = new Image { BackgroundColor = Color.FromArgb("#D1D1D1") };
image.Source = new FontImageSource
{
    Glyph = "\uf30c",
    FontFamily = DeviceInfo.Platform == DevicePlatform.iOS ? "Ionicons" : "ionicons.ttf#",
    Size = 44
};
```

The following screenshot shows several font icons being displayed:

:::image type="content" source="media/fonts/font-image-source.png" alt-text="Screenshot of three font icons.":::

Alternatively, you can display a font icon with the [`FontImage`](xref:Microsoft.Maui.Controls.Xaml.FontImageExtension) markup extension. For more information, see [Load a font icon](~/user-interface/controls/image.md#load-a-font-icon).
