---
title: "Label"
description: "The .NET MAUI Label displays single-line and multi-line text in an app."
ms.date: 03/02/2022
---

# Label

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-hyperlinks)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.Label> displays single-line and multi-line text. Text displayed by a <xref:Microsoft.Maui.Controls.Label> can be colored, spaced, and can have text decorations.

<xref:Microsoft.Maui.Controls.Label> defines the following properties:

- `CharacterSpacing`, of type `double`, sets the spacing between characters in the displayed text.
- `FontAttributes`, of type `FontAttributes`, determines text style.
- `FontAutoScalingEnabled`, of type `bool`, defines whether the text will reflect scaling preferences set in the operating system. The default value of this property is `true`.
- `FontFamily`, of type `string`, defines the font family.
- `FontSize`, of type `double`, defines the font size.
- `FormattedText`, of type `FormattedString`, specifies the presentation of text with multiple presentation options such as fonts and colors.
- `HorizontalTextAlignment`, of type `TextAlignment`, defines the horizontal alignment of the displayed text.
- `LineBreakMode`, of type `LineBreakMode`, determines how text should be handled when it can't fit on one line.
- `LineHeight`, of type `double`, specifies the multiplier to apply to the default line height when displaying text.
- `MaxLines`, of type `int`, indicates the maximum number of lines allowed in the <xref:Microsoft.Maui.Controls.Label>.
- `Padding`, of type `Thickness`, determines the label's padding.
- `Text`, of type `string`, defines the text displayed as the content of the label.
- `TextColor`, of type <xref:Microsoft.Maui.Graphics.Color>, defines the color of the displayed text.
- `TextDecorations`, of type `TextDecorations`, specifies the text decorations (underline and strikethrough) that can be applied.
- `TextTransform`, of type `TextTransform`, specifies the casing of the displayed text.
- `TextType`, of type `TextType`, determines whether the <xref:Microsoft.Maui.Controls.Label> should display plain text or HTML text.
- `VerticalTextAlignment`, of type `TextAlignment`, defines the vertical alignment of the displayed text.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

For information about specifying fonts on a <xref:Microsoft.Maui.Controls.Label>, see [Fonts](~/user-interface/fonts.md).

## Create a Label

The following example shows how to create a <xref:Microsoft.Maui.Controls.Label>:

```xaml
<Label Text="Hello world" />
```

The equivalent C# code is:

```csharp
Label label = new Label { Text = "Hello world" };
```

## Set colors

Labels can be set to use a specific text color via the `TextColor` property.

The following example sets the text color of a <xref:Microsoft.Maui.Controls.Label>:

```xaml
<Label TextColor="#77d065"
       Text="This is a green label." />
```

For more information about colors, see [Colors](~/user-interface/graphics/colors.md).

## Set character spacing

Character spacing can be applied to <xref:Microsoft.Maui.Controls.Label> objects by setting the `CharacterSpacing` property to a `double` value:

```xaml
<Label Text="Character spaced text"
       CharacterSpacing="10" />
```

The result is that characters in the text displayed by the <xref:Microsoft.Maui.Controls.Label> are spaced `CharacterSpacing` device-independent units apart.

## Add new lines

There are two main techniques for forcing text in a <xref:Microsoft.Maui.Controls.Label> onto a new line, from XAML:

1. Use the unicode line feed character, which is "&amp;#10;".
1. Specify your text using *property element* syntax.

The following code shows an example of both techniques:

```xaml
<!-- Unicode line feed character -->
<Label Text="First line &#10; Second line" />

<!-- Property element syntax -->
<Label>
    <Label.Text>
        First line
        Second line
    </Label.Text>
</Label>
```

In C#, text can be forced onto a new line with the "\n" character:

```csharp
Label label = new Label { Text = "First line\nSecond line" };
```

## Control text truncation and wrapping

Text wrapping and truncation can be controlled by setting the `LineBreakMode` property to a value of the `LineBreakMode` enumeration:

- `NoWrap` — does not wrap text, displaying only as much text as can fit on one line. This is the default value of the `LineBreakMode` property.
- `WordWrap` — wraps text at the word boundary.
- `CharacterWrap` — wraps text onto a new line at a character boundary.
- `HeadTruncation` — truncates the head of the text, showing the end.
- `MiddleTruncation` — displays the beginning and end of the text, with the middle replace by an ellipsis.
- `TailTruncation` — shows the beginning of the text, truncating the end.

## Display a specific number of lines

The number of lines displayed by a <xref:Microsoft.Maui.Controls.Label> can be specified by setting the `MaxLines` property to an `int` value:

- When `MaxLines` is -1, which is its default value, the <xref:Microsoft.Maui.Controls.Label> respects the value of the `LineBreakMode` property to either show just one line, possibly truncated, or all lines with all text.
- When `MaxLines` is 0, the <xref:Microsoft.Maui.Controls.Label> isn't displayed.
- When `MaxLines` is 1, the result is identical to setting the `LineBreakMode` property to `NoWrap`, `HeadTruncation`, `MiddleTruncation`, or `TailTruncation`. However, the <xref:Microsoft.Maui.Controls.Label> will respect the value of the `LineBreakMode` property with regard to placement of an ellipsis, if applicable.
- When `MaxLines` is greater than 1, the <xref:Microsoft.Maui.Controls.Label> will display up to the specified number of lines, while respecting the value of the `LineBreakMode` property with regard to placement of an ellipsis, if applicable. However, setting the `MaxLines` property to a value greater than 1 has no effect if the `LineBreakMode` property is set to `NoWrap`.

The following XAML example demonstrates setting the `MaxLines` property on a <xref:Microsoft.Maui.Controls.Label>:

```xaml
<Label Text="Lorem ipsum dolor sit amet, consectetur adipiscing elit. In facilisis nulla eu felis fringilla vulputate. Nullam porta eleifend lacinia. Donec at iaculis tellus."
       LineBreakMode="WordWrap"
       MaxLines="2" />
```

## Set line height

The vertical height of a <xref:Microsoft.Maui.Controls.Label> can be customized by setting the `Label.LineHeight` property to a `double` value.

> [!NOTE]
>
> - On iOS, the `Label.LineHeight` property changes the line height of text that fits on a single line, and text that wraps onto multiple lines.
> - On Android, the `Label.LineHeight` property only changes the line height of text that wraps onto multiple lines.
> - On Windows, the `Label.LineHeight` property changes the line height of text that wraps onto multiple lines.

The following example demonstrates setting the `LineHeight` property on a <xref:Microsoft.Maui.Controls.Label>:

```xaml
<Label Text="Lorem ipsum dolor sit amet, consectetur adipiscing elit. In facilisis nulla eu felis fringilla vulputate. Nullam porta eleifend lacinia. Donec at iaculis tellus."
       LineBreakMode="WordWrap"
       LineHeight="1.8" />
```

The following screenshot shows the result of setting the `Label.LineHeight` property to 1.8:

:::image type="content" source="media/label/lineheight.png" alt-text="Screenshot of Labels line height example.":::

## Display HTML

The <xref:Microsoft.Maui.Controls.Label> class has a `TextType` property, which determines whether the <xref:Microsoft.Maui.Controls.Label> object should display plain text, or HTML text. This property should be set to one of the members of the `TextType` enumeration:

- `Text` indicates that the <xref:Microsoft.Maui.Controls.Label> will display plain text, and is the default value of the `TextType` property.
- `Html` indicates that the <xref:Microsoft.Maui.Controls.Label> will display HTML text.

Therefore, <xref:Microsoft.Maui.Controls.Label> objects can display HTML by setting the `TextType` property to `Html`, and the `Text` property to a HTML string:

```csharp
Label label = new Label
{
    Text = "This is <span style=\"color:red;\"><strong>HTML</strong></span> text.",
    TextType = TextType.Html
};
```

In the example above, the double quote characters in the HTML have to be escaped using the `\` symbol.

In XAML, HTML strings can become unreadable due to additionally escaping the `<` and `>` symbols:

```xaml
<Label Text="This is &lt;span style=&quot;color:red&quot;&gt;&lt;strong&gt;HTML&lt;/strong&gt;&lt;/span&gt; text."
       TextType="Html"  />
```

Alternatively, for greater readability the HTML can be inlined in a `CDATA` section:

```xaml
<Label TextType="Html">
    <![CDATA[
    <Label Text="This is &lt;span style=&quot;color:red&quot;&gt;&lt;strong&gt;HTML&lt;/strong&gt;&lt;/span&gt; text."
    ]]>
</Label>
```

In this example, the `Text` property is set to the HTML string that's inlined in the `CDATA` section. This works because the `Text` property is the `ContentProperty` for the <xref:Microsoft.Maui.Controls.Label> class.

> [!IMPORTANT]
> Displaying HTML in a <xref:Microsoft.Maui.Controls.Label> is limited to the HTML tags that are supported by the underlying platform.

## Decorate text

Underline and strikethrough text decorations can be applied to <xref:Microsoft.Maui.Controls.Label> objects by setting the `TextDecorations` property to one or more `TextDecorations` enumeration members:

- `None`
- `Underline`
- `Strikethrough`

The following example demonstrates setting the `TextDecorations` property:

```xaml
<Label Text="This is underlined text." TextDecorations="Underline"  />
<Label Text="This is text with strikethrough." TextDecorations="Strikethrough" />
<Label Text="This is underlined text with strikethrough." TextDecorations="Underline, Strikethrough" />
```

The equivalent C# code is:

```csharp
Label underlineLabel = new Label { Text = "This is underlined text.", TextDecorations = TextDecorations.Underline };
Label strikethroughLabel = new Label { Text = "This is text with strikethrough.", TextDecorations = TextDecorations.Strikethrough };
Label bothLabel = new Label { Text = "This is underlined text with strikethrough.", TextDecorations = TextDecorations.Underline | TextDecorations.Strikethrough };
```

The following screenshot shows the `TextDecorations` enumeration members applied to <xref:Microsoft.Maui.Controls.Label> instances:

:::image type="content" source="media/label/textdecorations.png" alt-text="Screenshot of Labels with text decorations.":::

> [!NOTE]
> Text decorations can also be applied to <xref:Microsoft.Maui.Controls.Span> instances. For more information about the <xref:Microsoft.Maui.Controls.Span> class, see [Use formatted text](#use-formatted-text).

## Transform text

A <xref:Microsoft.Maui.Controls.Label> can transform the casing of its text, stored in the `Text` property, by setting the `TextTransform` property to a value of the `TextTransform` enumeration. This enumeration has four values:

- `None` indicates that the text won't be transformed.
- `Default` indicates that the default behavior for the platform will be used. This is the default value of the `TextTransform` property.
- `Lowercase` indicates that the text will be transformed to lowercase.
- `Uppercase` indicates that the text will be transformed to uppercase.

The following example shows transforming text to uppercase:

```xaml
<Label Text="This text will be displayed in uppercase."
       TextTransform="Uppercase" />
```

## Use formatted text

<xref:Microsoft.Maui.Controls.Label> exposes a `FormattedText` property that allows the presentation of text with multiple fonts and colors in the same view. The `FormattedText` property is of type `FormattedString`, which comprises one or more <xref:Microsoft.Maui.Controls.Span> instances, set via the `Spans` property.

> [!NOTE]
> It's not possible to display HTML in a <xref:Microsoft.Maui.Controls.Span>.

<xref:Microsoft.Maui.Controls.Span> defines the following properties:

- `BackgroundColor`, of type <xref:Microsoft.Maui.Graphics.Color>, which represents the color of the span background.
- `CharacterSpacing`, of type `double`, sets the spacing between characters in the displayed text.
- `FontAttributes`, of type `FontAttributes`, determines text style.
- `FontAutoScalingEnabled`, of type `bool`, defines whether the text will reflect scaling preferences set in the operating system. The default value of this property is `true`.
- `FontFamily`, of type `string`, defines the font family.
- `FontSize`, of type `double`, defines the font size.
- `LineHeight`, of type `double`, specifies the multiplier to apply to the default line height when displaying text.
- `Style`, of type <xref:Microsoft.Maui.Controls.Style>, which is the style to apply to the span.
- `Text`, of type `string`, defines the text displayed as the content of the <xref:Microsoft.Maui.Controls.Span>.
- `TextColor`, of type <xref:Microsoft.Maui.Graphics.Color>, defines the color of the displayed text.
- `TextDecorations`, of type `TextDecorations`, specifies the text decorations (underline and strikethrough) that can be applied.
- `TextTransform`, of type `TextTransform`, specifies the casing of the displayed text.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

> [!NOTE]
> The `Span.LineHeight` property has no effect on Windows.

In addition, the `GestureRecognizers` property can be used to define a collection of gesture recognizers that will respond to gestures on the <xref:Microsoft.Maui.Controls.Span>.

The following XAML example demonstrates a `FormattedText` property that consists of three <xref:Microsoft.Maui.Controls.Span> instances:

```xaml
<Label LineBreakMode="WordWrap">
    <Label.FormattedText>
        <FormattedString>
            <Span Text="Red Bold, " TextColor="Red" FontAttributes="Bold" />
            <Span Text="default, " FontSize="14">
                <Span.GestureRecognizers>
                    <TapGestureRecognizer Command="{Binding TapCommand}" />
                </Span.GestureRecognizers>
            </Span>
            <Span Text="italic small." FontAttributes="Italic" FontSize="12" />
        </FormattedString>
    </Label.FormattedText>
</Label>
```

The equivalent C# code is:

```csharp
FormattedString formattedString = new FormattedString ();
formattedString.Spans.Add (new Span { Text = "Red bold, ", TextColor = Colors.Red, FontAttributes = FontAttributes.Bold });

Span span = new Span { Text = "default, " };
span.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await DisplayAlert("Tapped", "This is a tapped Span.", "OK")) });
formattedString.Spans.Add(span);
formattedString.Spans.Add (new Span { Text = "italic small.", FontAttributes = FontAttributes.Italic, FontSize = 14 });

Label label = new Label { FormattedText = formattedString };
```

The following screenshot shows the resulting <xref:Microsoft.Maui.Controls.Label> that contains three <xref:Microsoft.Maui.Controls.Span> objects:

:::image type="content" source="media/label/formattedtext.png" alt-text="Screenshot of Label consisting of three spans.":::

A <xref:Microsoft.Maui.Controls.Span> can also respond to any gestures that are added to the span's `GestureRecognizers` collection. For example, a <xref:Microsoft.Maui.Controls.TapGestureRecognizer> has been added to the second <xref:Microsoft.Maui.Controls.Span> in the above examples. Therefore, when this <xref:Microsoft.Maui.Controls.Span> is tapped the <xref:Microsoft.Maui.Controls.TapGestureRecognizer> will respond by executing the `ICommand` defined by the `Command` property. For more information about tap gesture recognition, see [Recognize a tap gesture](~/fundamentals/gestures/tap.md).

## Create a hyperlink

The text displayed by <xref:Microsoft.Maui.Controls.Label> and <xref:Microsoft.Maui.Controls.Span> instances can be turned into hyperlinks with the following approach:

1. Set the `TextColor` and `TextDecoration` properties of the <xref:Microsoft.Maui.Controls.Label> or <xref:Microsoft.Maui.Controls.Span>.
1. Add a <xref:Microsoft.Maui.Controls.TapGestureRecognizer> to the `GestureRecognizers` collection of the <xref:Microsoft.Maui.Controls.Label> or <xref:Microsoft.Maui.Controls.Span>, whose `Command` property binds to a `ICommand`, and whose `CommandParameter` property contains the URL to open.
1. Define the `ICommand` that will be executed by the <xref:Microsoft.Maui.Controls.TapGestureRecognizer>.
1. Write the code that will be executed by the `ICommand`.

The following example, shows a <xref:Microsoft.Maui.Controls.Label> whose content is set from multiple <xref:Microsoft.Maui.Controls.Span> objects:

```xaml
<Label>
    <Label.FormattedText>
        <FormattedString>
            <Span Text="Alternatively, click " />
            <Span Text="here"
                  TextColor="Blue"
                  TextDecorations="Underline">
                <Span.GestureRecognizers>
                    <TapGestureRecognizer Command="{Binding TapCommand}"
                                          CommandParameter="https://learn.microsoft.com/dotnet/maui/" />
                </Span.GestureRecognizers>
            </Span>
            <Span Text=" to view .NET MAUI documentation." />
        </FormattedString>
    </Label.FormattedText>
</Label>
```

In this example, the first and third <xref:Microsoft.Maui.Controls.Span> instances contain text, while the second <xref:Microsoft.Maui.Controls.Span> represents a tappable hyperlink. It has its text color set to blue, and has an underline text decoration. This creates the appearance of a hyperlink, as shown in the following screenshot:

:::image type="content" source="media/label/hyperlink.png" alt-text="Screenshot of a hyperlink.":::

When the hyperlink is tapped, the <xref:Microsoft.Maui.Controls.TapGestureRecognizer> will respond by executing the `ICommand` defined by its `Command` property. In addition, the URL specified by the `CommandParameter` property will be passed to the `ICommand` as a parameter.

The code-behind for the XAML page contains the `TapCommand` implementation:

```csharp
using System.Windows.Input;

public partial class MainPage : ContentPage
{
    // Launcher.OpenAsync is provided by Essentials.
    public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }
}
```

The `TapCommand` executes the `Launcher.OpenAsync` method, passing the `TapGestureRecognizer.CommandParameter` property value as a parameter. The `Launcher.OpenAsync` method opens the URL in a web browser. Therefore, the overall effect is that when the hyperlink is tapped on the page, a web browser appears and the URL associated with the hyperlink is navigated to.

### Create a reusable hyperlink class

The previous approach to creating a hyperlink requires writing repetitive code every time you require a hyperlink in your app. However, both the <xref:Microsoft.Maui.Controls.Label> and <xref:Microsoft.Maui.Controls.Span> classes can be subclassed to create `HyperlinkLabel` and `HyperlinkSpan` classes, with the gesture recognizer and text formatting code added there.

The following example shows a `HyperlinkSpan` class:

```csharp
public class HyperlinkSpan : Span
{
    public static readonly BindableProperty UrlProperty =
        BindableProperty.Create(nameof(Url), typeof(string), typeof(HyperlinkSpan), null);

    public string Url
    {
        get { return (string)GetValue(UrlProperty); }
        set { SetValue(UrlProperty, value); }
    }

    public HyperlinkSpan()
    {
        TextDecorations = TextDecorations.Underline;
        TextColor = Colors.Blue;
        GestureRecognizers.Add(new TapGestureRecognizer
        {
            // Launcher.OpenAsync is provided by Essentials.
            Command = new Command(async () => await Launcher.OpenAsync(Url))
        });
    }
}
```

The `HyperlinkSpan` class defines a `Url` property, and associated <xref:Microsoft.Maui.Controls.BindableProperty>, and the constructor sets the hyperlink appearance and the <xref:Microsoft.Maui.Controls.TapGestureRecognizer> that will respond when the hyperlink is tapped. When a `HyperlinkSpan` is tapped, the <xref:Microsoft.Maui.Controls.TapGestureRecognizer> will respond by executing the `Launcher.OpenAsync` method to open the URL, specified by the `Url` property, in a web browser.

The `HyperlinkSpan` class can be consumed by adding an instance of the class to the XAML:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:HyperlinkDemo"
             x:Class="HyperlinkDemo.MainPage">
    <StackLayout>
        ...
        <Label>
            <Label.FormattedText>
                <FormattedString>
                    <Span Text="Alternatively, click " />
                    <local:HyperlinkSpan Text="here"
                                         Url="https://learn.microsoft.com/dotnet/" />
                    <Span Text=" to view .NET documentation." />
                </FormattedString>
            </Label.FormattedText>
        </Label>
    </StackLayout>
</ContentPage>
```
