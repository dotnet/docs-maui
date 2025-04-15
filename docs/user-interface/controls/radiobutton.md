---
title: "RadioButton"
description: "The .NET MAUI RadioButton is a type of button that allows users to select one option from a set. Each option is represented by one radio button, and you can only select one radio button in a group."
ms.date: 08/30/2024
---

# RadioButton

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-radiobutton)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.RadioButton> is a type of button that allows users to select one option from a set. Each option is represented by one radio button, and you can only select one radio button in a group. By default, each <xref:Microsoft.Maui.Controls.RadioButton> displays text:

:::image type="content" source="media/radiobutton/radiobuttons-default.png" alt-text="Screenshot of RadioButtons.":::

However, on some platforms a <xref:Microsoft.Maui.Controls.RadioButton> can display a <xref:Microsoft.Maui.Controls.View>, and on all platforms the appearance of each <xref:Microsoft.Maui.Controls.RadioButton> can be redefined with a <xref:Microsoft.Maui.Controls.ControlTemplate>:

:::image type="content" source="media/radiobutton/radiobuttons-controltemplate.png" alt-text="Screenshot of re-defined RadioButtons.":::

<xref:Microsoft.Maui.Controls.RadioButton> defines the following properties:

- `Content`, of type `object`, which defines the `string` or <xref:Microsoft.Maui.Controls.View> to be displayed by the <xref:Microsoft.Maui.Controls.RadioButton>.
- `IsChecked`, of type `bool`, which defines whether the <xref:Microsoft.Maui.Controls.RadioButton> is checked. This property uses a `TwoWay` binding, and has a default value of `false`.
- `GroupName`, of type `string`, which defines the name that specifies which <xref:Microsoft.Maui.Controls.RadioButton> controls are mutually exclusive. This property has a default value of `null`.
- `Value`, of type `object`, which defines an optional unique value associated with the <xref:Microsoft.Maui.Controls.RadioButton>.
- `BorderColor`, of type <xref:Microsoft.Maui.Graphics.Color>, which defines the border stroke color.
- `BorderWidth`, of type `double`, which defines the width of the <xref:Microsoft.Maui.Controls.RadioButton> border.
- `CharacterSpacing`, of type `double`, which defines the spacing between characters of any displayed text.
- `CornerRadius`, of type `int`, which defines the corner radius of the <xref:Microsoft.Maui.Controls.RadioButton>.
- `FontAttributes`, of type `FontAttributes`, which determines text style.
- `FontAutoScalingEnabled`, of type `bool`, which defines whether an app's UI reflects text scaling preferences set in the operating system. The default value of this property is `true`.
- `FontFamily`, of type `string`, which defines the font family.
- `FontSize`, of type `double`, which defines the font size.
- `TextColor`, of type <xref:Microsoft.Maui.Graphics.Color>, which defines the color of any displayed text.
- `TextTransform`, of type `TextTransform`, which defines the casing of any displayed text.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

<xref:Microsoft.Maui.Controls.RadioButton> also defines a `CheckedChanged` event that's raised when the `IsChecked` property changes, either through user or programmatic manipulation. The `CheckedChangedEventArgs` object that accompanies the `CheckedChanged` event has a single property named `Value`, of type `bool`. When the event is raised, the value of the `CheckedChangedEventArgs.Value` property is set to the new value of the `IsChecked` property.

<xref:Microsoft.Maui.Controls.RadioButton> grouping can be managed by the `RadioButtonGroup` class, which defines the following attached properties:

- `GroupName`, of type `string`, which defines the group name for <xref:Microsoft.Maui.Controls.RadioButton> objects in an `ILayout`.
- `SelectedValue`, of type `object`, which represents the value of the checked <xref:Microsoft.Maui.Controls.RadioButton> object within an `ILayout` group. This attached property uses a `TwoWay` binding by default.

> [!TIP]
> Although not mandatory, it's strongly recommended that you set the `GroupName` property to ensure that the `SelectedValue` property works correctly across all platforms.

For more information about the `GroupName` attached property, see [Group RadioButtons](#group-radiobuttons). For more information about the `SelectedValue` attached property, see [Respond to RadioButton state changes](#respond-to-radiobutton-state-changes).

## Create RadioButtons

The appearance of a <xref:Microsoft.Maui.Controls.RadioButton> is defined by the type of data assigned to the `RadioButton.Content` property:

- When the `RadioButton.Content` property is assigned a `string`, it will be displayed on each platform, horizontally aligned next to the radio button circle.
- When the `RadioButton.Content` is assigned a <xref:Microsoft.Maui.Controls.View>, it will be displayed on supported platforms (iOS, Windows), while unsupported platforms will fallback to a string representation of the <xref:Microsoft.Maui.Controls.View> object (Android). In both cases, the content is displayed horizontally aligned next to the radio button circle.
- When a <xref:Microsoft.Maui.Controls.ControlTemplate> is applied to a <xref:Microsoft.Maui.Controls.RadioButton>, a <xref:Microsoft.Maui.Controls.View> can be assigned to the `RadioButton.Content` property on all platforms. For more information, see [Redefine RadioButton appearance](#redefine-radiobutton-appearance).

### Display string-based content

A <xref:Microsoft.Maui.Controls.RadioButton> displays text when the `Content` property is assigned a `string`:

```xaml
<StackLayout>
    <Label Text="What's your favorite animal?" />
    <RadioButton Content="Cat" />
    <RadioButton Content="Dog" />
    <RadioButton Content="Elephant" />
    <RadioButton Content="Monkey"
                 IsChecked="true" />
</StackLayout>
```

In this example, <xref:Microsoft.Maui.Controls.RadioButton> objects are implicitly grouped inside the same parent container. This XAML results in the appearance shown in the following screenshot:

:::image type="content" source="media/radiobutton/radiobuttons-text.png" alt-text="Screenshot of text-based RadioButtons.":::

### Display arbitrary content

On iOS and Windows, a <xref:Microsoft.Maui.Controls.RadioButton> can display arbitrary content when the `Content` property is assigned a <xref:Microsoft.Maui.Controls.View>:

```xaml
<StackLayout>
    <Label Text="What's your favorite animal?" />
    <RadioButton>
        <RadioButton.Content>
            <Image Source="cat.png" />
        </RadioButton.Content>
    </RadioButton>
    <RadioButton>
        <RadioButton.Content>
            <Image Source="dog.png" />
        </RadioButton.Content>
    </RadioButton>
    <RadioButton>
        <RadioButton.Content>
            <Image Source="elephant.png" />
        </RadioButton.Content>
    </RadioButton>
    <RadioButton>
        <RadioButton.Content>
            <Image Source="monkey.png" />
        </RadioButton.Content>
    </RadioButton>
</StackLayout>
```

In this example, <xref:Microsoft.Maui.Controls.RadioButton> objects are implicitly grouped inside the same parent container. This XAML results in the appearance shown in the following screenshot:

:::image type="content" source="media/radiobutton/radiobuttons-view.png" alt-text="Screenshot of view-based RadioButtons.":::

On Android, <xref:Microsoft.Maui.Controls.RadioButton> objects will display a string-based representation of the <xref:Microsoft.Maui.Controls.View> object that's been set as content.

> [!NOTE]
> When a <xref:Microsoft.Maui.Controls.ControlTemplate> is applied to a <xref:Microsoft.Maui.Controls.RadioButton>, a <xref:Microsoft.Maui.Controls.View> can be assigned to the `RadioButton.Content` property on all platforms. For more information, see [Redefine RadioButton appearance](#redefine-radiobutton-appearance).

## Associate values with RadioButtons

Each <xref:Microsoft.Maui.Controls.RadioButton> object has a `Value` property, of type `object`, which defines an optional unique value to associate with the radio button. This enables the value of a <xref:Microsoft.Maui.Controls.RadioButton> to be different to its content, and is particularly useful when <xref:Microsoft.Maui.Controls.RadioButton> objects are displaying <xref:Microsoft.Maui.Controls.View> objects.

The following XAML shows setting the `Content` and `Value` properties on each <xref:Microsoft.Maui.Controls.RadioButton> object:

```xaml
<StackLayout>
    <Label Text="What's your favorite animal?" />
    <RadioButton Value="Cat">
        <RadioButton.Content>
            <Image Source="cat.png" />
        </RadioButton.Content>
    </RadioButton>
    <RadioButton Value="Dog">
        <RadioButton.Content>
            <Image Source="dog.png" />
        </RadioButton.Content>
    </RadioButton>
    <RadioButton Value="Elephant">
        <RadioButton.Content>
            <Image Source="elephant.png" />
        </RadioButton.Content>
    </RadioButton>
    <RadioButton Value="Monkey">
        <RadioButton.Content>
            <Image Source="monkey.png" />
        </RadioButton.Content>
    </RadioButton>
</StackLayout>
```

In this example, each <xref:Microsoft.Maui.Controls.RadioButton> has an <xref:Microsoft.Maui.Controls.Image> as its content, while also defining a string-based value. This enables the value of the checked radio button to be easily identified.

## Group RadioButtons

Radio buttons work in groups, and there are three approaches to grouping radio buttons:

- Place them inside the same parent container. This is known as *implicit* grouping.
- Set the `GroupName` property on each radio button in the group to the same value. This is known as *explicit* grouping.
- Set the `RadioButtonGroup.GroupName` attached property on a parent container, which in turn sets the `GroupName` property of any <xref:Microsoft.Maui.Controls.RadioButton> objects in the container. This is also known as *explicit* grouping.

> [!IMPORTANT]
> <xref:Microsoft.Maui.Controls.RadioButton> objects don't have to belong to the same parent to be grouped. They are mutually exclusive provided that they share a group name.

### Explicit grouping with the GroupName property

The following XAML example shows explicitly grouping <xref:Microsoft.Maui.Controls.RadioButton> objects by setting their `GroupName` properties:

```xaml
<Label Text="What's your favorite color?" />
<RadioButton Content="Red"
             GroupName="colors" />
<RadioButton Content="Green"
             GroupName="colors" />
<RadioButton Content="Blue"
             GroupName="colors" />
<RadioButton Content="Other"
             GroupName="colors" />
```

In this example, each <xref:Microsoft.Maui.Controls.RadioButton> is mutually exclusive because it shares the same `GroupName` value.

### Explicit grouping with the RadioButtonGroup.GroupName attached property

The `RadioButtonGroup` class defines a `GroupName` attached property, of type `string`, which can be set on a `Layout<View>` object. This enables any layout to be turned into a radio button group:

```xaml
<StackLayout RadioButtonGroup.GroupName="colors">
    <Label Text="What's your favorite color?" />
    <RadioButton Content="Red" />
    <RadioButton Content="Green" />
    <RadioButton Content="Blue" />
    <RadioButton Content="Other" />
</StackLayout>
```

In this example, each <xref:Microsoft.Maui.Controls.RadioButton> in the <xref:Microsoft.Maui.Controls.StackLayout> will have its `GroupName` property set to `colors`, and will be mutually exclusive.

> [!NOTE]
> When an `ILayout` object that sets the `RadioButtonGroup.GroupName` attached property contains a <xref:Microsoft.Maui.Controls.RadioButton> that sets its `GroupName` property, the value of the `RadioButton.GroupName` property will take precedence.

## Respond to RadioButton state changes

A radio button has two states: checked or unchecked. When a radio button is checked, its `IsChecked` property is `true`. When a radio button is unchecked, its `IsChecked` property is `false`. A radio button can be cleared by tapping another radio button in the same group, but it cannot be cleared by tapping it again. However, you can clear a radio button programmatically by setting its `IsChecked` property to `false`.

### Respond to an event firing

When the `IsChecked` property changes, either through user or programmatic manipulation, the `CheckedChanged` event fires. An event handler for this event can be registered to respond to the change:

```xaml
<RadioButton Content="Red"
             GroupName="colors"
             CheckedChanged="OnColorsRadioButtonCheckedChanged" />
```

The code-behind contains the handler for the `CheckedChanged` event:

```csharp
void OnColorsRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
{
    // Perform required operation
}
```

The `sender` argument is the <xref:Microsoft.Maui.Controls.RadioButton> responsible for this event. You can use this to access the <xref:Microsoft.Maui.Controls.RadioButton> object, or to distinguish between multiple <xref:Microsoft.Maui.Controls.RadioButton> objects sharing the same `CheckedChanged` event handler.

### Respond to a property change

The `RadioButtonGroup` class defines a `SelectedValue` attached property, of type `object`, which can be set on an `ILayout` object. This attached property represents the value of the checked <xref:Microsoft.Maui.Controls.RadioButton> within a group defined on a layout.

When the `IsChecked` property changes, either through user or programmatic manipulation, the `RadioButtonGroup.SelectedValue` attached property also changes. Therefore, the `RadioButtonGroup.SelectedValue` attached property can be data bound to a property that stores the user's selection:

```xaml
<ContentPage ...
             xmlns:local="clr-namespace:RadioButtonDemos"
             x:DataType="local:AnimalViewModel">
    <StackLayout Margin="10"
                 RadioButtonGroup.GroupName="{Binding GroupName}"
                 RadioButtonGroup.SelectedValue="{Binding Selection}">
        <Label Text="What's your favorite animal?" />
        <RadioButton Content="Cat"
                     Value="Cat" />
        <RadioButton Content="Dog"
                     Value="Dog" />
        <RadioButton Content="Elephant"
                     Value="Elephant" />
        <RadioButton Content="Monkey"
                     Value="Monkey"/>
        <Label x:Name="animalLabel">
            <Label.FormattedText>
                <FormattedString>
                    <Span Text="You have chosen:" />
                    <Span Text="{Binding Selection}" />
                </FormattedString>
            </Label.FormattedText>
        </Label>
    </StackLayout>
</ContentPage>
```

In this example, the value of the `RadioButtonGroup.GroupName` attached property is set by the `GroupName` property on the binding context. Similarly, the value of the `RadioButtonGroup.SelectedValue` attached property is set by the `Selection` property on the binding context. In addition, the `Selection` property is updated to the `Value` property of the checked <xref:Microsoft.Maui.Controls.RadioButton>.

## RadioButton visual states

<xref:Microsoft.Maui.Controls.RadioButton> objects have `Checked` and `Unchecked` visual states that can be used to initiate a visual change when a <xref:Microsoft.Maui.Controls.RadioButton> is checked or unchecked.

The following XAML example shows how to define a visual state for the `Checked` and `Unchecked` states:

```xaml
<ContentPage ...>
    <ContentPage.Resources>
        <Style TargetType="RadioButton">
            <Setter Property="VisualStateManager.VisualStateGroups">
                <VisualStateGroupList>
                    <VisualStateGroup x:Name="CheckedStates">
                        <VisualState x:Name="Checked">
                            <VisualState.Setters>
                                <Setter Property="TextColor"
                                        Value="Green" />
                                <Setter Property="Opacity"
                                        Value="1" />
                            </VisualState.Setters>
                        </VisualState>
                        <VisualState x:Name="Unchecked">
                            <VisualState.Setters>
                                <Setter Property="TextColor"
                                        Value="Red" />
                                <Setter Property="Opacity"
                                        Value="0.5" />
                            </VisualState.Setters>
                        </VisualState>
                    </VisualStateGroup>
                </VisualStateGroupList>
            </Setter>
        </Style>
    </ContentPage.Resources>
    <StackLayout>
        <Label Text="What's your favorite mode of transport?" />
        <RadioButton Content="Car" />
        <RadioButton Content="Bike" />
        <RadioButton Content="Train" />
        <RadioButton Content="Walking" />
    </StackLayout>
</ContentPage>
```

In this example, the implicit <xref:Microsoft.Maui.Controls.Style> targets <xref:Microsoft.Maui.Controls.RadioButton> objects. The `Checked` <xref:Microsoft.Maui.Controls.VisualState> specifies that when a <xref:Microsoft.Maui.Controls.RadioButton> is checked, its `TextColor` property will be set to green with an `Opacity` value of 1. The `Unchecked` <xref:Microsoft.Maui.Controls.VisualState> specifies that when a <xref:Microsoft.Maui.Controls.RadioButton> is in a unchecked state, its `TextColor` property will be set to red with an `Opacity` value of 0.5. Therefore, the overall effect is that when a <xref:Microsoft.Maui.Controls.RadioButton> is unchecked it's red and partially transparent, and is green without transparency when it's checked:

:::image type="content" source="media/radiobutton/radiobuttons-visualstates.png" alt-text="Screenshot of RadioButton visual states.":::

For more information about visual states, see [Visual states](~/user-interface/visual-states.md).

## Redefine RadioButton appearance

By default, <xref:Microsoft.Maui.Controls.RadioButton> objects use handlers to utilize native controls on supported platforms. However, <xref:Microsoft.Maui.Controls.RadioButton> visual structure can be redefined with a <xref:Microsoft.Maui.Controls.ControlTemplate>, so that <xref:Microsoft.Maui.Controls.RadioButton> objects have an identical appearance on all platforms. This is possible because the <xref:Microsoft.Maui.Controls.RadioButton> class inherits from the `TemplatedView` class.

The following XAML shows a <xref:Microsoft.Maui.Controls.ControlTemplate> that can be used to redefine the visual structure of <xref:Microsoft.Maui.Controls.RadioButton> objects:

```xaml
<ContentPage ...>
    <ContentPage.Resources>
        <ControlTemplate x:Key="RadioButtonTemplate">
            <Border Stroke="#F3F2F1"
                    StrokeThickness="2"
                    StrokeShape="RoundRectangle 10"
                    BackgroundColor="#F3F2F1"
                    HeightRequest="90"
                    WidthRequest="90"
                    HorizontalOptions="Start"
                    VerticalOptions="Start">
                <VisualStateManager.VisualStateGroups>
                    <VisualStateGroupList>
                        <VisualStateGroup x:Name="CheckedStates">
                            <VisualState x:Name="Checked">
                                <VisualState.Setters>
                                    <Setter Property="Stroke"
                                            Value="#FF3300" />
                                    <Setter TargetName="check"
                                            Property="Opacity"
                                            Value="1" />
                                </VisualState.Setters>
                            </VisualState>
                            <VisualState x:Name="Unchecked">
                                <VisualState.Setters>
                                    <Setter Property="BackgroundColor"
                                            Value="#F3F2F1" />
                                    <Setter Property="Stroke"
                                            Value="#F3F2F1" />
                                    <Setter TargetName="check"
                                            Property="Opacity"
                                            Value="0" />
                                </VisualState.Setters>
                            </VisualState>
                        </VisualStateGroup>
                    </VisualStateGroupList>
                </VisualStateManager.VisualStateGroups>
                <Grid Margin="4"
                      WidthRequest="90">
                    <Grid Margin="0,0,4,0"
                          WidthRequest="18"
                          HeightRequest="18"
                          HorizontalOptions="End"
                          VerticalOptions="Start">
                        <Ellipse Stroke="Blue"
                                 Fill="White"
                                 WidthRequest="16"
                                 HeightRequest="16"
                                 HorizontalOptions="Center"
                                 VerticalOptions="Center" />
                        <Ellipse x:Name="check"
                                 Fill="Blue"
                                 WidthRequest="8"
                                 HeightRequest="8"
                                 HorizontalOptions="Center"
                                 VerticalOptions="Center" />
                    </Grid>
                    <ContentPresenter />
                </Grid>
            </Border>
        </ControlTemplate>

        <Style TargetType="RadioButton">
            <Setter Property="ControlTemplate"
                    Value="{StaticResource RadioButtonTemplate}" />
        </Style>
    </ContentPage.Resources>
    <!-- Page content -->
</ContentPage>
```

In this example, the root element of the <xref:Microsoft.Maui.Controls.ControlTemplate> is a <xref:Microsoft.Maui.Controls.Border> object that defines `Checked` and `Unchecked` visual states. The <xref:Microsoft.Maui.Controls.Border> object uses a combination of <xref:Microsoft.Maui.Controls.Grid>, <xref:Microsoft.Maui.Controls.Shapes.Ellipse>, and <xref:Microsoft.Maui.Controls.ContentPresenter> objects to define the visual structure of a <xref:Microsoft.Maui.Controls.RadioButton>. The example also includes an *implicit* style that will assign the `RadioButtonTemplate` to the <xref:Microsoft.Maui.Controls.ControlTemplate> property of any <xref:Microsoft.Maui.Controls.RadioButton> objects on the page.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.ContentPresenter> object marks the location in the visual structure where <xref:Microsoft.Maui.Controls.RadioButton> content will be displayed.

The following XAML shows <xref:Microsoft.Maui.Controls.RadioButton> objects that consume the <xref:Microsoft.Maui.Controls.ControlTemplate> via the *implicit* style:

```xaml
<StackLayout>
    <Label Text="What's your favorite animal?" />
    <StackLayout RadioButtonGroup.GroupName="animals"
                 Orientation="Horizontal">
        <RadioButton Value="Cat">
            <RadioButton.Content>
                <StackLayout>
                    <Image Source="cat.png"
                           HorizontalOptions="Center"
                           VerticalOptions="Center" />
                    <Label Text="Cat"
                           HorizontalOptions="Center"
                           VerticalOptions="End" />
                </StackLayout>
            </RadioButton.Content>
        </RadioButton>
        <RadioButton Value="Dog">
            <RadioButton.Content>
                <StackLayout>
                    <Image Source="dog.png"
                           HorizontalOptions="Center"
                           VerticalOptions="Center" />
                    <Label Text="Dog"
                           HorizontalOptions="Center"
                           VerticalOptions="End" />
                </StackLayout>
            </RadioButton.Content>
        </RadioButton>
        <RadioButton Value="Elephant">
            <RadioButton.Content>
                <StackLayout>
                    <Image Source="elephant.png"
                           HorizontalOptions="Center"
                           VerticalOptions="Center" />
                    <Label Text="Elephant"
                           HorizontalOptions="Center"
                           VerticalOptions="End" />
                </StackLayout>
            </RadioButton.Content>
        </RadioButton>
        <RadioButton Value="Monkey">
            <RadioButton.Content>
                <StackLayout>
                    <Image Source="monkey.png"
                           HorizontalOptions="Center"
                           VerticalOptions="Center" />
                    <Label Text="Monkey"
                           HorizontalOptions="Center"
                           VerticalOptions="End" />
                </StackLayout>
            </RadioButton.Content>
        </RadioButton>
    </StackLayout>
</StackLayout>
```

In this example, the visual structure defined for each <xref:Microsoft.Maui.Controls.RadioButton> is replaced with the visual structure defined in the <xref:Microsoft.Maui.Controls.ControlTemplate>, and so at runtime the objects in the <xref:Microsoft.Maui.Controls.ControlTemplate> become part of the visual tree for each <xref:Microsoft.Maui.Controls.RadioButton>. In addition, the content for each <xref:Microsoft.Maui.Controls.RadioButton> is substituted into the <xref:Microsoft.Maui.Controls.ContentPresenter> defined in the control template. This results in the following <xref:Microsoft.Maui.Controls.RadioButton> appearance:

:::image type="content" source="media/radiobutton/radiobuttons-templated.png" alt-text="Screenshot of templated RadioButtons.":::

For more information about control templates, see [Control templates](~/fundamentals/controltemplate.md).

## Disable a RadioButton

Sometimes an app enters a state where a <xref:Microsoft.Maui.Controls.RadioButton> being checked is not a valid operation. In such cases, the <xref:Microsoft.Maui.Controls.RadioButton> can be disabled by setting its `IsEnabled` property to `false`.
