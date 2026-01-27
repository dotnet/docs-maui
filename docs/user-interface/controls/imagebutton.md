---
title: "ImageButton"
description: "The ImageButton displays an image and responds to a tap or click that directs an app to carry out a task."
ms.date: 08/30/2024
---

# ImageButton

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.ImageButton> view combines the <xref:Microsoft.Maui.Controls.Button> view and <xref:Microsoft.Maui.Controls.Image> view to create a button whose content is an image. When you press the <xref:Microsoft.Maui.Controls.ImageButton> with a finger or click it with a mouse, it directs the app to carry out a task. However, unlike the <xref:Microsoft.Maui.Controls.Button> the <xref:Microsoft.Maui.Controls.ImageButton> view has no concept of text and text appearance.

<xref:Microsoft.Maui.Controls.ImageButton> defines the following properties:

- `Aspect`, of type `Aspect`, determines how the image is scaled to fit the display area.
- `BorderColor`, of type <xref:Microsoft.Maui.Graphics.Color>, describes the border color of the button.
- `BorderWidth`, of type `double`, defines the width of the button's border.
- `Command`, of type <xref:System.Windows.Input.ICommand>, defines the command that's executed when the button is tapped.
- `CommandParameter`, of type `object`, is the parameter that's passed to `Command`.
- `CornerRadius`, of type `int`, describes the corner radius of the button's border.
- `IsLoading`, of type `bool`, represents the loading status of the image. The default value of this property is `false`.
- `IsOpaque`, of type `bool`, determines whether .NET MAUI should treat the image as opaque when rendering it. The default value of this property is `false`.
- `IsPressed`, of type `bool`, represents whether the button is being pressed. The default value of this property is `false`.
- `Padding`, of type `Thickness`, determines the button's padding.
- `Source`, of type <xref:Microsoft.Maui.Controls.ImageSource>, specifies an image to display as the content of the button.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The `Aspect` property can be set to one of the members of the `Aspect` enumeration:

- `Fill` - stretches the image to completely and exactly fill the <xref:Microsoft.Maui.Controls.ImageButton>. This may result in the image being distorted.
- `AspectFill` - clips the image so that it fills the <xref:Microsoft.Maui.Controls.ImageButton> while preserving the aspect ratio.
- `AspectFit` - letterboxes the image (if necessary) so that the entire image fits into the <xref:Microsoft.Maui.Controls.ImageButton>, with blank space added to the top/bottom or sides depending on whether the image is wide or tall. This is the default value of the `Aspect` enumeration.
- `Center` - centers the image in the <xref:Microsoft.Maui.Controls.ImageButton> while preserving the aspect ratio.

In addition, <xref:Microsoft.Maui.Controls.ImageButton> defines `Clicked`, `Pressed`, and `Released` events. The `Clicked` event is raised when an <xref:Microsoft.Maui.Controls.ImageButton> tap with a finger or mouse pointer is released from the button's surface. The `Pressed` event is raised when a finger presses on an <xref:Microsoft.Maui.Controls.ImageButton>, or a mouse button is pressed with the pointer positioned over the <xref:Microsoft.Maui.Controls.ImageButton>. The `Released` event is raised when the finger or mouse button is released. Generally, a `Clicked` event is also raised at the same time as the `Released` event, but if the finger or mouse pointer slides away from the surface of the <xref:Microsoft.Maui.Controls.ImageButton> before being released, the `Clicked` event might not occur.

> [!IMPORTANT]
> An <xref:Microsoft.Maui.Controls.ImageButton> must have its `IsEnabled` property set to `true` for it to respond to taps.

## Create an ImageButton

To create an image button, create an <xref:Microsoft.Maui.Controls.ImageButton> object, set its `Source` property and handle its `Clicked` event.

The following XAML example shows how to create an <xref:Microsoft.Maui.Controls.ImageButton>:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="ControlGallery.Views.XAML.ImageButtonDemoPage"
             Title="ImageButton Demo">
    <StackLayout>
       <ImageButton Source="image.png"
                    Clicked="OnImageButtonClicked"
                    HorizontalOptions="Center"
                    VerticalOptions="Center" />
    </StackLayout>
</ContentPage>
```

The `Source` property specifies the image that appears in the <xref:Microsoft.Maui.Controls.ImageButton>. The `Clicked` event is set to an event handler named `OnImageButtonClicked`. This handler is located in the code-behind file:

```csharp
public partial class ImageButtonDemoPage : ContentPage
{
    int clickTotal;

    public ImageButtonDemoPage()
    {
        InitializeComponent();
    }

    void OnImageButtonClicked(object sender, EventArgs e)
    {
        clickTotal += 1;
        label.Text = $"{clickTotal} ImageButton click{(clickTotal == 1 ? "" : "s")}";
    }
}
```

In this example, when the <xref:Microsoft.Maui.Controls.ImageButton> is tapped, the `OnImageButtonClicked` method executes. The `sender` argument is the <xref:Microsoft.Maui.Controls.ImageButton> responsible for this event. You can use this to access the <xref:Microsoft.Maui.Controls.ImageButton> object, or to distinguish between multiple <xref:Microsoft.Maui.Controls.ImageButton> objects sharing the same `Clicked` event. The `Clicked` handler increments a counter and displays the counter value in a <xref:Microsoft.Maui.Controls.Label>:

:::image type="content" source="media/imagebutton/imagebutton.png" alt-text="Screenshot of an ImageButton.":::

The equivalent C# code to create an <xref:Microsoft.Maui.Controls.ImageButton> is:

```csharp
Label label;
int clickTotal = 0;
...

ImageButton imageButton = new ImageButton
{
    Source = "XamarinLogo.png",
    HorizontalOptions = LayoutOptions.Center,
    VerticalOptions = LayoutOptions.Center
};
imageButton.Clicked += (s, e) =>
{
  clickTotal += 1;
  label.Text = $"{clickTotal} ImageButton click{(clickTotal == 1 ? "" : "s")}";
};
```

<!-- > [!NOTE]
> While an <xref:Microsoft.Maui.Controls.ImageButton> can load an animated GIF, it will only display the first frame of the GIF. -->

## Use the command interface

An app can respond to <xref:Microsoft.Maui.Controls.ImageButton> taps without handling the `Clicked` event. The <xref:Microsoft.Maui.Controls.ImageButton> implements an alternative notification mechanism called the _command_ or _commanding_ interface. This consists of two properties:

- `Command` of type [<xref:System.Windows.Input.ICommand>](xref:System.Windows.Input.ICommand), an interface defined in the [`System.Windows.Input`](xref:System.Windows.Input) namespace.
- `CommandParameter` property of type [`Object`](xref:System.Object).

This approach is suitable in connection with data-binding, and particularly when implementing the Model-View-ViewModel (MVVM) pattern. For more information about commanding, see [Use the command interface](button.md#use-the-command-interface) in the [Button](button.md) article.

## Press and release an ImageButton

The `Pressed` event is raised when a finger presses on a <xref:Microsoft.Maui.Controls.ImageButton>, or a mouse button is pressed with the pointer positioned over the <xref:Microsoft.Maui.Controls.ImageButton>. The `Released` event is raised when the finger or mouse button is released. Generally, the `Clicked` event is also raised at the same time as the `Released` event, but if the finger or mouse pointer slides away from the surface of the <xref:Microsoft.Maui.Controls.ImageButton> before being released, the `Clicked` event might not occur.

For more information about these events, see [Press and release the button](button.md#press-and-release-the-button) in the [Button](button.md) article.

## ImageButton visual states

<xref:Microsoft.Maui.Controls.ImageButton> has a `Pressed` <xref:Microsoft.Maui.Controls.VisualState> that can be used to initiate a visual change to the <xref:Microsoft.Maui.Controls.ImageButton> when pressed, if it's enabled.

The following XAML example shows how to define a visual state for the `Pressed` state:

```xaml
 <ImageButton Source="image.png"
              ...>
     <VisualStateManager.VisualStateGroups>
         <VisualStateGroupList>
             <VisualStateGroup x:Name="CommonStates">
                 <VisualState x:Name="Normal">
                     <VisualState.Setters>
                         <Setter Property="Scale"
                                 Value="1" />
                     </VisualState.Setters>
                 </VisualState>
                 <VisualState x:Name="Pressed">
                     <VisualState.Setters>
                         <Setter Property="Scale"
                                 Value="0.8" />
                     </VisualState.Setters>
                 </VisualState>
                 <VisualState x:Name="PointerOver" />
             </VisualStateGroup>
         </VisualStateGroupList>
     </VisualStateManager.VisualStateGroups>
 </ImageButton>
```

In this example, the `Pressed` <xref:Microsoft.Maui.Controls.VisualState> specifies that when the <xref:Microsoft.Maui.Controls.ImageButton> is pressed, its `Scale` property will be changed from its default value of 1 to 0.8. The `Normal` <xref:Microsoft.Maui.Controls.VisualState> specifies that when the <xref:Microsoft.Maui.Controls.ImageButton> is in a normal state, its `Scale` property will be set to 1. Therefore, the overall effect is that when the <xref:Microsoft.Maui.Controls.ImageButton> is pressed, it's rescaled to be slightly smaller, and when the <xref:Microsoft.Maui.Controls.ImageButton> is released, it's rescaled to its default size.

> [!IMPORTANT]
> For an <xref:Microsoft.Maui.Controls.ImageButton> to return to its `Normal` state the `VisualStateGroup` must also define a `PointerOver` state. If you use the styles `ResourceDictionary` created by the .NET MAUI app project template, you'll already have an implicit `ImageButton` style that defines the `PointerOver` state.

For more information about visual states, see [Visual states](~/user-interface/visual-states.md).

## Disable an ImageButton

Sometimes an app enters a state where an <xref:Microsoft.Maui.Controls.ImageButton> click is not a valid operation. In those cases, the <xref:Microsoft.Maui.Controls.ImageButton> should be disabled by setting its `IsEnabled` property to `false`.
