---
title: "Get started with .NET MAUI XAML"
description: "In a .NET MAUI app, XAML is mostly used to define the visual contents of a page and works together with a code-behind file."
ms.date: 08/19/2025
---

# Get started with XAML

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/xaml-fundamentals)

In a .NET Multi-platform App UI (.NET MAUI) app, XAML is mostly used to define the visual contents of a page and works together with a C# code-behind file. The code-behind file provides code support for the markup. Together, these two files contribute to a new class definition that includes child views and property initialization. Within the XAML file, classes and properties are referenced with XML elements and attributes, and links between the markup and code are established.

## Anatomy of a XAML file

A new .NET MAUI app contains three XAML files, and their associated code-behind files:

:::image type="content" source="media/get-started/new-solution.png" alt-text="Screenshot of the structure of a new .NET MAUI app.":::

The first file pairing is *App.xaml*, a XAML file, and *App.xaml.cs*, a C# *code-behind* file associated with the XAML file. Both *App.xaml* and *App.xaml.cs* contribute to a class named `App` that derives from `Application`. The second file pairing is *AppShell.xaml* and *AppShell.xaml.cs*, which contribute to a class named `AppShell` that derives from <xref:Microsoft.Maui.Controls.Shell>. Most other classes with XAML files contribute to a class that derives from <xref:Microsoft.Maui.Controls.ContentPage>, and define the UI of a page. This is true of the *MainPage.xaml* and *MainPage.xaml.cs* files.

The *MainPage.xaml* file has the following structure:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage">
    ...
</ContentPage>
```

The two XML namespace (`xmlns`) declarations refer to URIs on microsoft.com. However, there's no content at these URIs, and they basically function as version identifiers.

The first XML namespace declaration means that tags defined within the XAML file with no prefix refer to classes in .NET MAUI, for example <xref:Microsoft.Maui.Controls.ContentPage>. The second namespace declaration defines a prefix of `x`. This is used for several elements and attributes that are intrinsic to XAML itself and which are supported by other implementations of XAML. However, these elements and attributes are slightly different depending on the year embedded in the URI. .NET MAUI supports the [2009 XAML specification](/dotnet/desktop/xaml-services/xaml-2009-language-features).

At the end of the first tag, the `x` prefix is used for an attribute named `Class`. Because the use of this `x` prefix is virtually universal for the XAML namespace, XAML attributes such as `Class` are almost always referred to as `x:Class`. The `x:Class` attribute specifies a fully qualified .NET class name: the `MainPage` class in the `MyMauiApp` namespace. This means that this XAML file defines a new class named `MainPage` in the `MyMauiApp` namespace that derives from <xref:Microsoft.Maui.Controls.ContentPage> (the tag in which the `x:Class` attribute appears).

The `x:Class` attribute can only appear in the root element of a XAML file to define a derived C# class. This is the only new class defined in the XAML file. Everything else that appears in a XAML file is instead simply instantiated from existing classes and initialized.

The *MainPage.xaml.cs* file looks similar to this:

```csharp
namespace MyMauiApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
}
```

The `MainPage` class derives from <xref:Microsoft.Maui.Controls.ContentPage>, and is a [partial class](/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods) definition.

At runtime, code in the `MauiProgram` class bootstraps the app and executes the `App` class constructor, which instantiates `AppShell`. The `AppShell` class instantiates the first page of the app to be displayed, which is `MainPage`. The `MainPage` constructor calls `InitializeComponent`, which initializes all the objects defined in the XAML file, connects them all together in parent-child relationships, attaches event handlers defined in code to events set in the XAML file, and sets the resultant tree of objects as the content of the page.

> [!NOTE]
> The `AppShell` class uses .NET MAUI Shell to set the first page of the app to be displayed. However, Shell is beyond the scope of this introduction to XAML. For more information, see [.NET MAUI Shell](~/fundamentals/shell/index.md).

## Set page content

A <xref:Microsoft.Maui.Controls.ContentPage> should contain a single child, that can be a view or a layout with child views. The child of the <xref:Microsoft.Maui.Controls.ContentPage> is automatically set as the value of the `ContentPage.Content` property.

The following example shows a <xref:Microsoft.Maui.Controls.ContentPage> containing a <xref:Microsoft.Maui.Controls.Label>:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="XamlSamples.HelloXamlPage"
             Title="Hello XAML Page">
    <Label Text="Hello, XAML!"
           VerticalOptions="Center"
           HorizontalTextAlignment="Center"
           Rotation="-15"
           FontSize="18"
           FontAttributes="Bold"
           TextColor="Blue" />
</ContentPage>
```

::: moniker range=">=net-maui-10.0"

## XAML Source Generation

> [!IMPORTANT]
> This feature is opt-in. To use it, set the XAML inflator in your project file as per the instructions below. Please let us know your experience with this (good or bad) in the .NET MAUI repository.

You have the option as of .NET 10 to have XAML generate C# instead of IL (intermediate language) by using Roslyn source generation. This improves your ability to debug XAML by emitting the source files and setting break points in them directly, speeds up the debug performance of page rendering, and improves your app behavior consistency between debug and release build modes.

To enable XAML source generation for the whole project, add this to your project file:

```xml
<PropertyGroup>
   <MauiXamlInflator>SourceGen</MauiXamlInflator>
</PropertyGroup>
```

For more fine-grained control over what XAML files use XAML Source Generation, you can configure individual files or patterns:

```xml
<ItemGroup>
    <MauiXaml Update="**/*.xaml" Inflator="SourceGen" />
</ItemGroup>
```

You can include or exclude certain XAML files by configuring the pattern in the `Inflator` attribute.
If you want to opt-out on certain files or pattern, unset (or set to "") the `Inflator` to ensure the correct defaults (the default is runtime inflation on Debug, XamlC on Release)

Reference the [source generator configuration documentation](/dotnet/core/extensions/configuration-generator) for using options like `EmitCompilerGeneratedFiles`.

## Global XML namespace (preview)

In .NET 10, you can optionally use a global XAML namespace to reduce boilerplate `xmlns:` declarations across your pages.

> [!IMPORTANT]
> This feature is in preview. To try it, enable preview features in your project file. Behavior and defaults may change.

To opt in, add a `GlobalXmlns.cs` with assembly-level `XmlnsDefinition` attributes that map the global namespace to your CLR namespaces, and reference the `http://schemas.microsoft.com/dotnet/maui/global` namespace at the root of your XAML:

```csharp
// GlobalXmlns.cs
using Microsoft.Maui.Controls;

[assembly: XmlnsDefinition("http://schemas.microsoft.com/dotnet/maui/global", "MyApp.Views")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/dotnet/maui/global", "MyApp.Controls")]
```

```xml
<!-- Root element uses the global namespace -->
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/maui/global"
                         xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                         x:Class="MyApp.MainPage">
        <TagView x:DataType="Tag" />
        <!-- No xmlns prefixes for TagView/Tag when globally mapped -->
        <!-- Add explicit xmlns prefixes only when disambiguation is needed -->
    
</ContentPage>
```

Optionally, set default prefixes:

```csharp
using XmlnsPrefixAttribute = Microsoft.Maui.Controls.XmlnsPrefixAttribute;

[assembly: XmlnsPrefix("MyApp.Controls", "controls")]
```

To enable the most streamlined experience (implicit default namespaces), add:

```xml
<PropertyGroup>
    <DefineConstants>$(DefineConstants);MauiAllowImplicitXmlnsDeclaration</DefineConstants>
    <EnablePreviewFeatures>true</EnablePreviewFeatures>
</PropertyGroup>
```

Now your project will implicitly include these 2 namespaces which you’ve been accustomed to seeing in every XAML file since .NET MAUI first shipped.

```xml
xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
```

> [!NOTE]
> Because `x:` is used by the XAML inflator, you still need to use that prefix. With this change alone, your XAML for a view gets much tighter.

**Before**

```xml
<ContentPage 
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="MyApp.Pages.MyContentPage">
</ContentPage>
```

**After**

```xml
<ContentPage x:Class="MyApp.Pages.MyContentPage">
</ContentPage>
```

::: moniker-end

From the example above the relationship between classes, properties, and XML should be evident. A .NET MAUI class (such as <xref:Microsoft.Maui.Controls.ContentPage> or <xref:Microsoft.Maui.Controls.Label>) appears in the XAML file as an XML element. Properties of that class—including `Title` on <xref:Microsoft.Maui.Controls.ContentPage> and seven properties of <xref:Microsoft.Maui.Controls.Label> usually appear as XML attributes.

Many shortcuts exist to set the values of these properties. Some properties are basic data types. For example, the `Title` and `Text` properties are of type `string`, and `Rotation` is of type `double`. The `HorizontalTextAlignment` property is of type <xref:Microsoft.Maui.TextAlignment>, which is an enumeration. For a property of any enumeration type, all you need to supply is a member name.

For properties of more complex types, however, converters are used for parsing the XAML. These are classes in .NET MAUI that derive from `TypeConverter`. For the example above, several .NET MAUI converters are automatically applied to convert string values to their correct type:

- `LayoutOptionsConverter` for the `VerticalOptions` property. This converter converts the names of public static fields of the `LayoutOptions` structure to values of type `LayoutOptions`.
- `ColorTypeConverter` for the `TextColor` property. This converter converts the names of public static fields of the <xref:Microsoft.Maui.Graphics.Colors> class or hexadecimal RGB values, with or without an alpha channel.

## Page navigation

When you run a .NET MAUI app, the `MainPage` is typically displayed. To see a different page you can either set that as the new startup page in the *AppShell.xaml* file, or navigate to the new page from `MainPage`.

To implement navigation, in the *MainPage.xaml.cs* constructor, you can create a simple <xref:Microsoft.Maui.Controls.Button> and use the event handler to navigate to `HelloXamlPage`:

```csharp
public MainPage()
{
    InitializeComponent();

    Button button = new Button
    {
        Text = "Navigate!",
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.Center
    };

    button.Clicked += async (sender, args) =>
    {
        await Navigation.PushAsync(new HelloXamlPage());
    };

    Content = button;
}
```

When you compile and deploy the new version of this app, a button appears on the screen. Pressing it navigates to `HelloXamlPage`:

:::image type="content" source="media/get-started/helloxaml1.png" alt-text="Screenshot of rotated Label text.":::

You can navigate back to `MainPage` using the navigation bar that appears on each platform.

> [!NOTE]
> An alternative to this navigation model is to use .NET MAUI Shell. For more information, see [.NET MAUI Shell overview](~/fundamentals/shell/index.md).

## XAML and code interactions

The child of most <xref:Microsoft.Maui.Controls.ContentPage> derivatives is a layout, such as a <xref:Microsoft.Maui.Controls.StackLayout> or a <xref:Microsoft.Maui.Controls.Grid>, and the layout can contain multiple children. In XAML, these parent-child relationships are established with normal XML hierarchy:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="XamlSamples.XamlPlusCodePage"
             Title="XAML + Code Page">
    <StackLayout>
        <Slider VerticalOptions="Center" />
        <Label Text="A simple Label"
               FontSize="18"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
        <Button Text="Click Me!"
                HorizontalOptions="Center"
                VerticalOptions="Center" />
    </StackLayout>
</ContentPage>
```

This XAML file is syntactically complete, and produces the following UI:

:::image type="content" source="media/get-started/xamlpluscode1.png" alt-text="Screenshot of multiple controls on a page.":::

However, while you can interact with the <xref:Microsoft.Maui.Controls.Slider> and <xref:Microsoft.Maui.Controls.Button>, the UI isn't updated. The <xref:Microsoft.Maui.Controls.Slider> should cause the <xref:Microsoft.Maui.Controls.Label> to display the current value, and the <xref:Microsoft.Maui.Controls.Button> should do something.

Displaying a <xref:Microsoft.Maui.Controls.Slider> value using a <xref:Microsoft.Maui.Controls.Label> can be achieved entirely in XAML with a *data binding*. However, it's useful to see the code solution first. Even so, handling the <xref:Microsoft.Maui.Controls.Button> click definitely requires code. This means that the code-behind file for `XamlPlusCodePage` must contain handlers for the `ValueChanged` event of the <xref:Microsoft.Maui.Controls.Slider> and the `Clicked` event of the <xref:Microsoft.Maui.Controls.Button>:

::: moniker range="<=net-maui-9.0"

```csharp
namespace XamlSamples
{
    public partial class XamlPlusCodePage
    {
        public XamlPlusCodePage()
        {
            InitializeComponent();
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            valueLabel.Text = args.NewValue.ToString("F3");
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await DisplayAlert("Clicked!", "The button labeled '" + button.Text + "' has been clicked", "OK");
        }
    }
}
```

::: moniker-end

::: moniker range=">=net-maui-10.0"

```csharp
namespace XamlSamples
{
    public partial class XamlPlusCodePage
    {
        public XamlPlusCodePage()
        {
            InitializeComponent();
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            valueLabel.Text = args.NewValue.ToString("F3");
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await DisplayAlertAsync("Clicked!", "The button labeled '" + button.Text + "' has been clicked", "OK");
        }
    }
}
```

::: moniker-end

Back in the XAML file, the <xref:Microsoft.Maui.Controls.Slider> and <xref:Microsoft.Maui.Controls.Button> tags need to include attributes for the `ValueChanged` and `Clicked` events that reference these handlers:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="XamlSamples.XamlPlusCodePage"
             Title="XAML + Code Page">
    <StackLayout>
        <Slider VerticalOptions="Center"
                ValueChanged="OnSliderValueChanged" />
        <Label x:Name="valueLabel"
               Text="A simple Label"
               FontSize="18"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
        <Button Text="Click Me!"
                HorizontalOptions="Center"
                VerticalOptions="Center"
                Clicked="OnButtonClicked" />
    </StackLayout>
</ContentPage>
```

Notice that assigning a handler to an event has the same syntax as assigning a value to a property. In addition, for the `ValueChanged` event handler of the <xref:Microsoft.Maui.Controls.Slider> to use the <xref:Microsoft.Maui.Controls.Label> to display the current value, the handler needs to reference that object from code. Therefore, the <xref:Microsoft.Maui.Controls.Label> needs a name, which is specified with the `x:Name` attribute. The `x` prefix of the `x:Name` attribute indicates that this attribute is intrinsic to XAML. The name you assign to the `x:Name` attribute has the same rules as C# variable names. For example, it must begin with a letter or underscore and contain no embedded spaces.

The `ValueChanged` event handler can now set the <xref:Microsoft.Maui.Controls.Label> to display the new <xref:Microsoft.Maui.Controls.Slider> value, which is available from the event arguments:

```csharp
void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
{
    valueLabel.Text = args.NewValue.ToString("F3");
}
```

Alternatively, the handler could obtain the <xref:Microsoft.Maui.Controls.Slider> object that is generating this event from the `sender` argument and obtain the `Value` property from that:

```csharp
void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
{
    valueLabel.Text = ((Slider)sender).Value.ToString("F3");
}
```

The result is that any manipulation of the <xref:Microsoft.Maui.Controls.Slider> causes its value to be displayed in the <xref:Microsoft.Maui.Controls.Label>:

:::image type="content" source="media/get-started/xamlpluscode2.png" alt-text="Screenshot of multiple controls on a page, with Slider value displayed.":::

In the example above the <xref:Microsoft.Maui.Controls.Button> simulates a response to a `Clicked` event by displaying an alert with the `Text` of the button. Therefore, the event handler can cast the `sender` argument to a <xref:Microsoft.Maui.Controls.Button> and then access its properties:

::: moniker range="<=net-maui-9.0"

```csharp
async void OnButtonClicked(object sender, EventArgs args)
{
    Button button = (Button)sender;
    await DisplayAlert("Clicked!", "The button labeled '" + button.Text + "' has been clicked", "OK");
}
```

The `OnButtonClicked` method is defined as `async` because the <xref:Microsoft.Maui.Controls.Page.DisplayAlert%2A> method is asynchronous and should be prefaced with the `await` operator, which returns when the method completes. Because this method obtains the <xref:Microsoft.Maui.Controls.Button> firing the event from the `sender` argument, the same handler could be used for multiple buttons.

::: moniker-end

::: moniker range=">=net-maui-10.0"

```csharp
async void OnButtonClicked(object sender, EventArgs args)
{
    Button button = (Button)sender;
    await DisplayAlertAsync("Clicked!", "The button labeled '" + button.Text + "' has been clicked", "OK");
}
```

The `OnButtonClicked` method is defined as `async` because the <xref:Microsoft.Maui.Controls.Page.DisplayAlertAsync%2A> method is asynchronous and should be prefaced with the `await` operator, which returns when the method completes. Because this method obtains the <xref:Microsoft.Maui.Controls.Button> firing the event from the `sender` argument, the same handler could be used for multiple buttons.

::: moniker-end

## Next steps

XAML is mostly designed for instantiating and initializing objects. But often, properties must be set to complex objects that cannot easily be represented as XML strings, and sometimes properties defined by one class must be set on a child class. These two needs require the essential XAML syntax features of *property elements* and *attached properties*.

> [!div class="nextstepaction"]
> [Essential XAML syntax](essential-syntax.md)
