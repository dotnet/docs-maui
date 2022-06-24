---
title: "Respond to system theme changes"
description: ".NET MAUI app can respond to operating system theme changes by using the AppThemeBinding markup extension, and the SetAppThemeColor and SetAppTheme extension methods."
ms.date: 02/25/2022
---

# Respond to system theme changes

Devices typically include light and dark themes, which each refer to a broad set of appearance preferences that can be set at the operating system level. Apps should respect these system themes, and respond immediately when the system theme changes.

The system theme may change for a variety of reasons, depending on the device configuration. This includes the system theme being explicitly changed by the user, it changing due to the time of day, and it changing due to environmental factors such as low light.

.NET Multi-platform App UI (.NET MAUI) apps can respond to system theme changes by consuming resources with the `AppThemeBinding` markup extension, and the `SetAppThemeColor` and `SetAppTheme<T>`  extension methods.

> [!NOTE]
> .NET MAUI apps can respond to system theme changes on iOS 13 or greater, Android 10 (API 29) or greater, macOS 10.14 or greater, and Windows 10 or greater.

The following screenshot shows themed pages, for the light system theme on iOS and the dark system theme on Android:

:::image type="content" source="media/system-theme-changes/main-page-both-themes.png" alt-text="Screenshot of the main page of a themed app.":::

## Define and consume theme resources

Resources for light and dark themes can be consumed with the `AppThemeBinding` markup extension, and the `SetAppThemeColor` and `SetAppTheme<T>` extension methods. With these approaches, resources are automatically applied based on the value of the current system theme. In addition, objects that consume these resources are automatically updated if the system theme changes while an app is running.

### AppThemeBinding markup extension

The `AppThemeBinding` markup extension enables you to consume a resource, such as an image or color, based on the current system theme:

```xaml
<StackLayout>
    <Label Text="This text is green in light mode, and red in dark mode."
           TextColor="{AppThemeBinding Light=Green, Dark=Red}" />
    <Image Source="{AppThemeBinding Light=lightlogo.png, Dark=darklogo.png}" />
</StackLayout>
```

In this example, the text color of the first `Label` is set to green when the device is using its light theme, and is set to red when the device is using its dark theme. Similarly, the `Image` displays a different image file based upon the current system theme.

In addition, resources defined in a `ResourceDictionary` can be consumed with the `StaticResource` markup extension:

```xaml
<ContentPage ...>
    <ContentPage.Resources>

        <!-- Light colors -->
        <Color x:Key="LightPrimaryColor">WhiteSmoke</Color>
        <Color x:Key="LightSecondaryColor">Black</Color>

        <!-- Dark colors -->
        <Color x:Key="DarkPrimaryColor">Teal</Color>
        <Color x:Key="DarkSecondaryColor">White</Color>

        <Style x:Key="ButtonStyle"
               TargetType="Button">
            <Setter Property="BackgroundColor"
                    Value="{AppThemeBinding Light={StaticResource LightPrimaryColor}, Dark={StaticResource DarkPrimaryColor}}" />
            <Setter Property="TextColor"
                    Value="{AppThemeBinding Light={StaticResource LightSecondaryColor}, Dark={StaticResource DarkSecondaryColor}}" />
        </Style>

    </ContentPage.Resources>

    <Grid BackgroundColor="{AppThemeBinding Light={StaticResource LightPrimaryColor}, Dark={StaticResource DarkPrimaryColor}}">
      <Button Text="MORE INFO"
              Style="{StaticResource ButtonStyle}" />
    </Grid>    
</ContentPage>    
```

In this example, the background color of the `Grid` and the `Button` style changes based on whether the device is using its light theme or dark theme.

For more information about the `AppThemeBinding` markup extension, see [AppThemeBinding markup extension](~/xaml/markup-extensions/consume.md#appthemebinding-markup-extension).

### Extension methods

.NET MAUI includes `SetAppThemeColor` and `SetAppTheme<T>` extension methods that enable `VisualElement` objects to respond to system theme changes.

The `SetAppThemeColor` method enables `Color` objects to be specified that will be set on a target property based on the current system theme:

```csharp
Label label = new Label();
label.SetAppThemeColor(Label.TextColorProperty, Colors.Green, Colors.Red);
```

In this example, the text color of the `Label` is set to green when the device is using its light theme, and is set to red when the device is using its dark theme.

The `SetAppTheme<T>` method enables objects of type `T` to be specified that will be set on a target property based on the current system theme:

```csharp
Image image = new Image();
image.SetAppTheme<FileImageSource>(Image.SourceProperty, "lightlogo.png", "darklogo.png");
```

In this example, the `Image` displays `lightlogo.png` when the device is using its light theme, and `darklogo.png` when the device is using its dark theme.

## Detect the current system theme

The current system theme can be detected by getting the value of the `Application.RequestedTheme` property:

```csharp
AppTheme currentTheme = Application.Current.RequestedTheme;
```

The `RequestedTheme` property returns an `AppTheme` enumeration member. The `AppTheme` enumeration defines the following members:

- `Unspecified`, which indicates that the device is using an unspecified theme.
- `Light`, which indicates that the device is using its light theme.
- `Dark`, which indicates that the device is using its dark theme.

## Set the current user theme

The theme used by the app can be set with the `Application.UserAppTheme` property, which is of type `AppTheme`, regardless of which system theme is currently operational:

```csharp
Application.Current.UserAppTheme = AppTheme.Dark;
```

In this example, the app is set to use the theme defined for the system dark mode, regardless of which system theme is currently operational.

> [!NOTE]
> Set the `UserAppTheme` property to `AppTheme.Unspecified` to default to the operational system theme.

## React to theme changes

The system theme on a device may change for a variety of reasons, depending on how the device is configured. .NET MAUI apps can be notified when the system theme changes by handling the `Application.RequestedThemeChanged` event:

```csharp
Application.Current.RequestedThemeChanged += (s, a) =>
{
    // Respond to the theme change
};
```

The `AppThemeChangedEventArgs` object, which accompanies the `RequestedThemeChanged` event, has a single property named `RequestedTheme`, of type `AppTheme`. This property can be examined to detect the requested system theme.

> [!IMPORTANT]
> To respond to theme changes on Android your `MainActivity` class must include the `ConfigChanges.UiMode` flag in the `Activity` attribute. .NET MAUI apps created with the Visual Studio project templates automatically include this flag.
