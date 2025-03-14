---
title: "Style apps using Cascading Style Sheets"
description: ".NET MAUI supports styling visual elements using Cascading Style Sheets (CSS)."
ms.date: 03/14/2025
---

# Style apps using Cascading Style Sheets

.NET Multi-platform App UI (.NET MAUI) apps can be styled using Cascading Style Sheets (CSS). A style sheet consists of a list of rules, with each rule consisting of one or more selectors and a declaration block. A declaration block consists of a list of declarations in braces, with each declaration consisting of a property, a colon, and a value. When there are multiple declarations in  a block, a semi-colon is inserted as a separator.

The following  example shows some .NET MAUI compliant CSS:

```css
navigationpage {
    -maui-bar-background-color: lightgray;
}

^contentpage {
    background-color: lightgray;
}

#listView {
    background-color: lightgray;
}

stacklayout {
    margin: 20;
    -maui-spacing: 6;
}

grid {
    row-gap: 6;
    column-gap: 6;
}
.mainPageTitle {
    font-style: bold;
    font-size: 14;
}

.mainPageSubtitle {
    margin-top: 15;
}

.detailPageTitle {
    font-style: bold;
    font-size: 14;
    text-align: center;
}

.detailPageSubtitle {
    text-align: center;
    font-style: italic;
}

listview image {
    height: 60;
    width: 60;
}

stacklayout>image {
    height: 200;
    width: 200;
}
```

In .NET MAUI, CSS style sheets are parsed and evaluated at runtime, rather than compile time, and style sheets are re-parsed on use.

> [!IMPORTANT]
> It's not possible to fully style a .NET MAUI app using CSS. However, XAML styles can be used to supplement CSS. For more information about XAML styles, see [Style apps using XAML](xaml.md).

## Consume a style sheet

The process for adding a style sheet to a .NET MAUI app is as follows:

1. Add an empty CSS file to your .NET MAUI app project. The CSS file can be placed in any folder, with the *Resources* folder being the recommended location.
1. Set the build action of the CSS file to **MauiCss**.

### Loading a style sheet

There are a number of approaches that can be used to load a style sheet.

> [!NOTE]
> It's not possible to change a style sheet at runtime and have the new style sheet applied.

### Load a style sheet in XAML

A style sheet can be loaded and parsed with the `StyleSheet` class before being added to a <xref:Microsoft.Maui.Controls.ResourceDictionary>:

```xaml
<Application ...>
    <Application.Resources>
        <StyleSheet Source="/Resources/styles.css" />
    </Application.Resources>
</Application>
```

The `StyleSheet.Source` property specifies the style sheet as a URI relative to the location of the enclosing XAML file, or relative to the project root if the URI starts with a `/`.

> [!WARNING]
> The CSS file will fail to load if its build action is not set to **MauiCss**.

Alternatively, a style sheet can be loaded and parsed with the `StyleSheet` class, before being added to a <xref:Microsoft.Maui.Controls.ResourceDictionary>, by inlining it in a `CDATA` section:

```xaml
<ContentPage ...>
    <ContentPage.Resources>
        <StyleSheet>
            <![CDATA[
            ^contentpage {
                background-color: lightgray;
            }
            ]]>
        </StyleSheet>
    </ContentPage.Resources>
    ...
</ContentPage>
```

For more information about resource dictionaries, see [Resource dictionaries](~/fundamentals/resource-dictionaries.md).

### Load a style sheet in C\#

In C#, a style sheet can be loaded from a `StringReader` and added to a <xref:Microsoft.Maui.Controls.ResourceDictionary>:

```csharp
using Microsoft.Maui.Controls.StyleSheets;

public partial class MyPage : ContentPage
{
    public MyPage()
    {
        InitializeComponent();

        using (var reader = new StringReader("^contentpage { background-color: lightgray; }"))
        {
            this.Resources.Add(StyleSheet.FromReader(reader));
        }
    }
}
```

The argument to the `StyleSheet.FromReader` method is the `TextReader` that has read the style sheet.

## Select elements and apply properties

CSS uses selectors to determine which elements to target. Styles with matching selectors are applied consecutively, in definition order. Styles defined on a specific item are always applied last. For more information about supported selectors, see [Selector reference](#selector-reference).

CSS uses properties to style a selected element. Each property has a set of possible values, and some properties can affect any type of element, while others apply to groups of elements. For more information about supported properties, see [Property reference](#property-reference).

Child stylesheets always override parent stylesheets if they set the same properties. Therefore, the following precedence rules are followed when applying styles that set the same properties:

- A style defined in the app resources will be overwritten by a style defined in the page resources, if they set the same properties.
- A style defined in page resources will be overwritten by a style defined in the control resources, if they set the same properties.
- A style defined in the app resources will be overwritten by a style defined in the control resources, if they set the same properties.

> [!NOTE]
> CSS variables are unsupported.

### Select elements by type

Elements in the visual tree can be selected by type with the case insensitive `element` selector:

```css
stacklayout {
    margin: 20;
}
```

This selector identifies any <xref:Microsoft.Maui.Controls.StackLayout> elements on pages that consume the style sheet, and sets their margins to a uniform thickness of 20.

> [!NOTE]
> The `element` selector does not identify subclasses of the specified type.

### Selecting elements by base class

Elements in the visual tree can be selected by base class with the case insensitive `^base` selector:

```css
^contentpage {
    background-color: lightgray;
}
```

This selector identifies any <xref:Microsoft.Maui.Controls.ContentPage> elements that consume the style sheet, and sets their background color to `lightgray`.

> [!NOTE]
> The `^base` selector is specific to .NET MAUI, and isn't part of the CSS specification.

### Selecting an element by name

Individual elements in the visual tree can be selected with the case sensitive `#id` selector:

```css
#listView {
    background-color: lightgray;
}
```

This selector identifies the element whose `StyleId` property is set to `listView`. However, if the `StyleId` property is not set, the selector will fall back to using the `x:Name` of the element. Therefore, in the following example, the `#listView` selector will identify the <xref:Microsoft.Maui.Controls.ListView> whose `x:Name` attribute is set to `listView`, and will set it's background color to `lightgray`.

```xaml
<ContentPage ...>
    <ContentPage.Resources>
        <StyleSheet Source="/Resources/styles.css" />
    </ContentPage.Resources>
    <StackLayout>
        <ListView x:Name="listView">
            ...
        </ListView>
    </StackLayout>
</ContentPage>
```

### Select elements with a specific class attribute

Elements with a specific class attribute can be selected with the case sensitive `.class` selector:

```css
.detailPageTitle {
    font-style: bold;
    font-size: 14;
    text-align: center;
}

.detailPageSubtitle {
    text-align: center;
    font-style: italic;
}
```

A CSS class can be assigned to a XAML element by setting the `StyleClass` property of the element to the CSS class name. Therefore, in the following example, the styles defined by the `.detailPageTitle` class are assigned to the first <xref:Microsoft.Maui.Controls.Label>, while the styles defined by the `.detailPageSubtitle` class are assigned to the second <xref:Microsoft.Maui.Controls.Label>.

```xaml
<ContentPage ...>
    <ContentPage.Resources>
        <StyleSheet Source="/Resources/styles.css" />
    </ContentPage.Resources>
    <ScrollView>
        <StackLayout>
            <Label ... StyleClass="detailPageTitle" />
            <Label ... StyleClass="detailPageSubtitle"/>
        </StackLayout>
    </ScrollView>
</ContentPage>
```

### Select child elements

Child elements in the visual tree can be selected with the case insensitive `element element` selector:

```css
listview image {
    height: 60;
    width: 60;
}
```

This selector identifies any <xref:Microsoft.Maui.Controls.Image> elements that are children of <xref:Microsoft.Maui.Controls.ListView> elements, and sets their height and width to 60. Therefore, in the following XAML example, the `listview image` selector will identify the <xref:Microsoft.Maui.Controls.Image> that's a child of the <xref:Microsoft.Maui.Controls.ListView>, and sets its height and width to 60.

```xaml
<ContentPage ...>
    <ContentPage.Resources>
        <StyleSheet Source="/Resources/styles.css" />
    </ContentPage.Resources>
    <StackLayout>
        <ListView ...>
            <ListView.ItemTemplate>
                <DataTemplate>
                    <ViewCell>
                        <Grid>
                            ...
                            <Image ... />
                            ...
                        </Grid>
                    </ViewCell>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </StackLayout>
</ContentPage>
```

> [!NOTE]
> The `element element` selector does not require the child element to be a *direct* child of the parent – the child element may have a different parent. Selection occurs provided that an ancestor is the specified first element.

### Select direct child elements

Direct child elements in the visual tree can be selected with the case insensitive `element>element` selector:

```css
stacklayout>image {
    height: 200;
    width: 200;
}
```

This selector identifies any <xref:Microsoft.Maui.Controls.Image> elements that are direct children of <xref:Microsoft.Maui.Controls.StackLayout> elements, and sets their height and width to 200. Therefore, in the following example, the `stacklayout>image` selector will identify the <xref:Microsoft.Maui.Controls.Image> that's a direct child of the <xref:Microsoft.Maui.Controls.StackLayout>, and sets its height and width to 200.

```xaml
<ContentPage ...>
    <ContentPage.Resources>
        <StyleSheet Source="/Resources/styles.css" />
    </ContentPage.Resources>
    <ScrollView>
        <StackLayout>
            ...
            <Image ... />
            ...
        </StackLayout>
    </ScrollView>
</ContentPage>
```

> [!NOTE]
> The `element>element` selector requires that the child element is a *direct* child of the parent.

## Selector reference

The following CSS selectors are supported by .NET MAUI:

|Selector|Example|Description|
|---|---|---|
|`.class`|`.header`|Selects all elements with the `StyleClass` property containing 'header'. This selector is case sensitive.|
|`#id`|`#email`|Selects all elements with `StyleId` set to `email`. If `StyleId` is not set, fallback to `x:Name`. When using XAML, `x:Name` is preferred over `StyleId`. This selector is case sensitive.|
|`*`|`*`|Selects all elements.|
|`element`|`label`|Selects all elements of type <xref:Microsoft.Maui.Controls.Label>, but not subclasses. This selector is case insensitive.|
|`^base`|`^contentpage`|Selects all elements with <xref:Microsoft.Maui.Controls.ContentPage> as the base class, including <xref:Microsoft.Maui.Controls.ContentPage> itself. This selector is case insensitive and isn't part of the CSS specification.|
|`element,element`|`label,button`|Selects all <xref:Microsoft.Maui.Controls.Button> elements and all <xref:Microsoft.Maui.Controls.Label> elements. This selector is case insensitive.|
|`element element`|`stacklayout label`|Selects all <xref:Microsoft.Maui.Controls.Label> elements inside a <xref:Microsoft.Maui.Controls.StackLayout>. This selector is case insensitive.|
|`element>element`|`stacklayout>label`|Selects all <xref:Microsoft.Maui.Controls.Label> elements with <xref:Microsoft.Maui.Controls.StackLayout> as a direct parent. This selector is case insensitive.|
|`element+element`|`label+entry`|Selects all <xref:Microsoft.Maui.Controls.Entry> elements directly after a <xref:Microsoft.Maui.Controls.Label>. This selector is case insensitive.|
|`element~element`|`label~entry`|Selects all <xref:Microsoft.Maui.Controls.Entry> elements preceded by a <xref:Microsoft.Maui.Controls.Label>. This selector is case insensitive.|

Styles with matching selectors are applied consecutively, in definition order. Styles defined on a specific item are always applied last.

> [!TIP]
> Selectors can be combined without limitation, such as `StackLayout>ContentView>label.email`.

The following selectors are unsupported:

- `[attribute]`
- `@media` and `@supports`
- `:` and `::`

::: moniker range="=net-maui-8.0"

> [!NOTE]
> Specificity, and specificity overrides are unsupported.

::: moniker-end

::: moniker range=">=net-maui-9.0"

If two or more CSS rules point to the same element, the selector with the highest specificity will take precedence and its style declaration will be applied to the element. The [specificity algorithm](https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/StyleSheets/Selector.cs#L319) calculates the weight of a CSS selector to determine which rule from competing CSS declarations gets applied to the element.

::: moniker-end

## Property reference

The following CSS properties are supported by .NET MAUI (in the **Values** column, types are *italic*, while string literals are `gray`):

|Property|Applies to|Values|Example|
|---|---|---|---|
|`align-content`|<xref:Microsoft.Maui.Controls.FlexLayout>| `stretch` \| `center` \| `start` \| `end` \| `spacebetween` \| `spacearound` \| `spaceevenly` \| `flex-start` \| `flex-end` \| `space-between` \| `space-around` \| `initial` |`align-content: space-between;`|
|`align-items`|<xref:Microsoft.Maui.Controls.FlexLayout>| `stretch` \| `center` \| `start` \| `end` \| `flex-start` \| `flex-end` \| `initial` |`align-items: flex-start;`|
|`align-self`|<xref:Microsoft.Maui.Controls.VisualElement>| `auto` \| `stretch` \| `center` \| `start` \| `end` \| `flex-start` \| `flex-end` \| `initial`|`align-self: flex-end;`|
|`background-color`|<xref:Microsoft.Maui.Controls.VisualElement>|_color_ \| `initial` |`background-color: springgreen;`|
|`background-image`|<xref:Microsoft.Maui.Controls.Page>|_string_ \| `initial` |`background-image: bg.png;`|
|`border-color`|<xref:Microsoft.Maui.Controls.Button>, <xref:Microsoft.Maui.Controls.Frame>, <xref:Microsoft.Maui.Controls.ImageButton>|_color_ \| `initial`|`border-color: #9acd32;`|
|`border-radius`|<xref:Microsoft.Maui.Controls.BoxView>, <xref:Microsoft.Maui.Controls.Button>, <xref:Microsoft.Maui.Controls.Frame>, <xref:Microsoft.Maui.Controls.ImageButton>|_double_ \| `initial` |`border-radius: 10;`|
|`border-width`|<xref:Microsoft.Maui.Controls.Button>, <xref:Microsoft.Maui.Controls.ImageButton>|_double_ \| `initial` |`border-width: .5;`|
|`color`|<xref:Microsoft.Maui.Controls.ActivityIndicator>, <xref:Microsoft.Maui.Controls.BoxView>, <xref:Microsoft.Maui.Controls.Button>, <xref:Microsoft.Maui.Controls.CheckBox>, <xref:Microsoft.Maui.Controls.DatePicker>, <xref:Microsoft.Maui.Controls.Editor>, <xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.Label>, <xref:Microsoft.Maui.Controls.Picker>, <xref:Microsoft.Maui.Controls.ProgressBar>, <xref:Microsoft.Maui.Controls.SearchBar>, <xref:Microsoft.Maui.Controls.Switch>, <xref:Microsoft.Maui.Controls.TimePicker>|_color_ \| `initial` |`color: rgba(255, 0, 0, 0.3);`|
|`column-gap`|<xref:Microsoft.Maui.Controls.Grid>|_double_ \| `initial`|`column-gap: 9;`|
|`direction`|<xref:Microsoft.Maui.Controls.VisualElement>|`ltr` \| `rtl` \| `inherit` \| `initial` |`direction: rtl;`|
|`flex-direction`|<xref:Microsoft.Maui.Controls.FlexLayout>| `column` \| `columnreverse` \| `row` \| `rowreverse` \| `row-reverse` \| `column-reverse` \| `initial`|`flex-direction: column-reverse;`|
|`flex-basis`|<xref:Microsoft.Maui.Controls.VisualElement>|_float_ \| `auto` \| `initial`. In addition, a percentage in the range 0% to 100% can be specified with the `%` sign.|`flex-basis: 25%;`|
|`flex-grow`|<xref:Microsoft.Maui.Controls.VisualElement>|_float_ \| `initial`|`flex-grow: 1.5;`|
|`flex-shrink`|<xref:Microsoft.Maui.Controls.VisualElement>|_float_ \| `initial`|`flex-shrink: 1;`|
|`flex-wrap`|<xref:Microsoft.Maui.Controls.VisualElement>| `nowrap` \| `wrap` \| `reverse` \| `wrap-reverse` \| `initial`|`flex-wrap: wrap-reverse;`|
|`font-family`|<xref:Microsoft.Maui.Controls.Button>, <xref:Microsoft.Maui.Controls.DatePicker>, <xref:Microsoft.Maui.Controls.Editor>, <xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.Label>, <xref:Microsoft.Maui.Controls.Picker>, <xref:Microsoft.Maui.Controls.SearchBar>, <xref:Microsoft.Maui.Controls.TimePicker>, <xref:Microsoft.Maui.Controls.Span>|_string_ \| `initial` |`font-family: Consolas;`|
|`font-size`|<xref:Microsoft.Maui.Controls.Button>, <xref:Microsoft.Maui.Controls.DatePicker>, <xref:Microsoft.Maui.Controls.Editor>, <xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.Label>, <xref:Microsoft.Maui.Controls.Picker>, <xref:Microsoft.Maui.Controls.SearchBar>, <xref:Microsoft.Maui.Controls.TimePicker>, <xref:Microsoft.Maui.Controls.Span>|_double_ \| `initial` |`font-size: 12;`|
|`font-style`|<xref:Microsoft.Maui.Controls.Button>, <xref:Microsoft.Maui.Controls.DatePicker>, <xref:Microsoft.Maui.Controls.Editor>, <xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.Label>, <xref:Microsoft.Maui.Controls.Picker>, <xref:Microsoft.Maui.Controls.SearchBar>, <xref:Microsoft.Maui.Controls.TimePicker>, <xref:Microsoft.Maui.Controls.Span>|`bold` \| `italic` \| `initial` |`font-style: bold;`|
|`height`|<xref:Microsoft.Maui.Controls.VisualElement>|_double_ \| `initial` |`height: 250;`|
|`justify-content`|<xref:Microsoft.Maui.Controls.FlexLayout>| `start` \| `center` \| `end` \| `spacebetween` \| `spacearound` \| `spaceevenly` \| `flex-start` \| `flex-end` \| `space-between` \| `space-around` \| `initial`|`justify-content: flex-end;`|
|`letter-spacing`|<xref:Microsoft.Maui.Controls.Button>, <xref:Microsoft.Maui.Controls.DatePicker>, <xref:Microsoft.Maui.Controls.Editor>, <xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.Label>, <xref:Microsoft.Maui.Controls.Picker>, <xref:Microsoft.Maui.Controls.SearchBar>, <xref:Microsoft.Maui.Controls.SearchHandler>, <xref:Microsoft.Maui.Controls.Span>, <xref:Microsoft.Maui.Controls.TimePicker>|_double_ \| `initial`|`letter-spacing: 2.5;`|
|`line-height`|<xref:Microsoft.Maui.Controls.Label>, <xref:Microsoft.Maui.Controls.Span>|_double_ \| `initial` |`line-height: 1.8;`|
|`margin`|<xref:Microsoft.Maui.Controls.View>|_thickness_ \| `initial` |`margin: 6 12;`|
|`margin-left`|<xref:Microsoft.Maui.Controls.View>|_thickness_ \| `initial` |`margin-left: 3;`|
|`margin-top`|<xref:Microsoft.Maui.Controls.View>|_thickness_ \| `initial` |`margin-top: 2;`|
|`margin-right`|<xref:Microsoft.Maui.Controls.View>|_thickness_ \| `initial` |`margin-right: 1;`|
|`margin-bottom`|<xref:Microsoft.Maui.Controls.View>|_thickness_ \| `initial` |`margin-bottom: 6;`|
|`max-lines`|<xref:Microsoft.Maui.Controls.Label>|_int_ \| `initial`|`max-lines: 2;`|
|`min-height`|<xref:Microsoft.Maui.Controls.VisualElement>|_double_ \| `initial` |`min-height: 50;`|
|`min-width`|<xref:Microsoft.Maui.Controls.VisualElement>|_double_ \| `initial` |`min-width: 112;`|
|`opacity`|<xref:Microsoft.Maui.Controls.VisualElement>|_double_ \| `initial` |`opacity: .3;`|
|`order`|<xref:Microsoft.Maui.Controls.VisualElement>|_int_ \| `initial`|`order: -1;`|
|`padding`|<xref:Microsoft.Maui.Controls.Button>, <xref:Microsoft.Maui.Controls.ImageButton>, <xref:Microsoft.Maui.Controls.Layout>, <xref:Microsoft.Maui.Controls.Page>|_thickness_ \| `initial` |`padding: 6 12 12;`|
|`padding-left`|<xref:Microsoft.Maui.Controls.Button>, <xref:Microsoft.Maui.Controls.ImageButton>, <xref:Microsoft.Maui.Controls.Layout>, <xref:Microsoft.Maui.Controls.Page>|_double_ \| `initial`|`padding-left: 3;`|
|`padding-top`|<xref:Microsoft.Maui.Controls.Button>, <xref:Microsoft.Maui.Controls.ImageButton>, <xref:Microsoft.Maui.Controls.Layout>, <xref:Microsoft.Maui.Controls.Page>| _double_ \| `initial` |`padding-top: 4;`|
|`padding-right`|<xref:Microsoft.Maui.Controls.Button>, <xref:Microsoft.Maui.Controls.ImageButton>, <xref:Microsoft.Maui.Controls.Layout>, <xref:Microsoft.Maui.Controls.Page>| _double_ \| `initial` |`padding-right: 2;`|
|`padding-bottom`|<xref:Microsoft.Maui.Controls.Button>, <xref:Microsoft.Maui.Controls.ImageButton>, <xref:Microsoft.Maui.Controls.Layout>, <xref:Microsoft.Maui.Controls.Page>| _double_ \| `initial` |`padding-bottom: 6;`|
|`position`|<xref:Microsoft.Maui.Controls.FlexLayout>| `relative` \| `absolute` \| `initial`|`position: absolute;`|
|`row-gap`|<xref:Microsoft.Maui.Controls.Grid>| _double_ \| `initial`|`row-gap: 12;`|
|`text-align`| <xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.EntryCell>, <xref:Microsoft.Maui.Controls.Label>, <xref:Microsoft.Maui.Controls.SearchBar>|`left` \| `top` \| `right` \| `bottom` \| `start` \| `center` \| `middle` \| `end` \| `initial`. `left` and `right` should be avoided in right-to-left environments.| `text-align: right;`|
|`text-decoration`|<xref:Microsoft.Maui.Controls.Label>, <xref:Microsoft.Maui.Controls.Span>|`none` \| `underline` \| `strikethrough` \| `line-through` \| `initial`|`text-decoration: underline, line-through;`|
|`text-transform`|<xref:Microsoft.Maui.Controls.Button>,<xref:Microsoft.Maui.Controls.Editor>, <xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.Label>, <xref:Microsoft.Maui.Controls.SearchBar>, <xref:Microsoft.Maui.Controls.SearchHandler>|`none` \| `default` \| `uppercase` \| `lowercase` \| `initial` |`text-transform: uppercase;`|
|`transform`|<xref:Microsoft.Maui.Controls.VisualElement>| `none`, `rotate`, `rotateX`, `rotateY`, `scale`, `scaleX`, `scaleY`, `translate`, `translateX`, `translateY`, `initial` |`transform: rotate(180), scaleX(2.5);`|
|`transform-origin`|<xref:Microsoft.Maui.Controls.VisualElement>| _double_, _double_ \| `initial` |`transform-origin: 7.5, 12.5;`|
|`vertical-align`|<xref:Microsoft.Maui.Controls.Label>|`left` \| `top` \| `right` \| `bottom` \| `start` \| `center` \| `middle` \| `end` \| `initial`|`vertical-align: bottom;`|
|`visibility`|<xref:Microsoft.Maui.Controls.VisualElement>|`true` \| `visible` \| `false` \| `hidden` \| `collapse` \| `initial`|`visibility: hidden;`|
|`width`|<xref:Microsoft.Maui.Controls.VisualElement>|_double_ \| `initial`|`width: 320;`|

<!-- Todo: Can also set `background`, on a <xref:Microsoft.Maui.Controls.VisualElement>, to a <xref:Microsoft.Maui.Controls.Brush> -->

> [!NOTE]
> `initial` is a valid value for all properties. It clears the value (resets to default) that was set from another style.

The following properties are unsupported:

- `all: initial`.
- Layout properties (box, or grid).
- Shorthand properties, such as `font`, and `border`.

In addition, there's no `inherit` value and so inheritance isn't supported. Therefore you can't, for example, set the `font-size` property on a layout and expect all the <xref:Microsoft.Maui.Controls.Label> instances in the layout to inherit the value. The one exception is the `direction` property, which has a default value of `inherit`.

> [!IMPORTANT]
> <xref:Microsoft.Maui.Controls.Span> elements can't be targeted using CSS.

### .NET MAUI specific properties

The following .NET MAUI specific CSS properties are also supported (in the **Values** column, types are _italic_, while string literals are `gray`):

|Property|Applies to|Values|Example|
|---|---|---|---|
|`-maui-bar-background-color`|<xref:Microsoft.Maui.Controls.NavigationPage>, <xref:Microsoft.Maui.Controls.TabbedPage>|_color_ \| `initial` |`-maui-bar-background-color: teal;`|
|`-maui-bar-text-color`|<xref:Microsoft.Maui.Controls.NavigationPage>, <xref:Microsoft.Maui.Controls.TabbedPage>|_color_ \| `initial` |`-maui-bar-text-color: gray`|
|`-maui-horizontal-scroll-bar-visibility`|<xref:Microsoft.Maui.Controls.ScrollView>| `default` \| `always` \| `never` \| `initial` |`-maui-horizontal-scroll-bar-visibility: never;`|
|`-maui-max-length`|<xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.Editor>, <xref:Microsoft.Maui.Controls.SearchBar>|_int_ \| `initial` |`-maui-max-length: 20;`|
|`-maui-max-track-color`|<xref:Microsoft.Maui.Controls.Slider>|_color_ \| `initial` |`-maui-max-track-color: red;`|
|`-maui-min-track-color`|<xref:Microsoft.Maui.Controls.Slider>|_color_ \| `initial` |`-maui-min-track-color: yellow;`|
|`-maui-orientation`|<xref:Microsoft.Maui.Controls.ScrollView>, <xref:Microsoft.Maui.Controls.StackLayout>| `horizontal` \| `vertical` \| `both` \| `initial`. `both` is only supported on a <xref:Microsoft.Maui.Controls.ScrollView>. |`-maui-orientation: horizontal;`|
|`-maui-placeholder`|<xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.Editor>, <xref:Microsoft.Maui.Controls.SearchBar>|_quoted text_ \| `initial` |`-maui-placeholder: Enter name;`|
|`-maui-placeholder-color`|<xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.Editor>, <xref:Microsoft.Maui.Controls.SearchBar>|_color_ \| `initial` |`-maui-placeholder-color: green;`|
|`-maui-spacing`|`StackLayout`|_double_ \| `initial` |`-maui-spacing: 8;`|
|`-maui-shadow`|<xref:Microsoft.Maui.Controls.VisualElement>| Valid formats are: color, offsetX, offsetY \| offset X, offsetY, radius, color \| offset X, offsetY, radius, color, opactity | `-maui-shadow: #000000 4 4;` |
|`-maui-thumb-color`|<xref:Microsoft.Maui.Controls.Slider>, <xref:Microsoft.Maui.Controls.Switch>|_color_ \| `initial` |`-maui-thumb-color: limegreen;`|
|`-maui-vertical-scroll-bar-visibility`|<xref:Microsoft.Maui.Controls.ScrollView>| `default` \| `always` \| `never` \| `initial` |`-maui-vertical-scroll-bar-visibility: always;`|
|`-maui-vertical-text-alignment`|<xref:Microsoft.Maui.Controls.Label>| `start` \| `center` \| `end` \| `initial`|`-maui-vertical-text-alignment: end;`|
|`-maui-visual`|<xref:Microsoft.Maui.Controls.VisualElement>|_string_ \| `initial` |`-maui-visual: material;`|

### .NET MAUI Shell specific properties

The following .NET MAUI Shell specific CSS properties are also supported (in the **Values** column, types are _italic_, while string literals are `gray`):

|Property|Applies to|Values|Example|
|---|---|---|---|
|`-maui-flyout-background`|<xref:Microsoft.Maui.Controls.Shell>|_color_ \| `initial` |`-maui-flyout-background: red;`|
|`-maui-shell-background`|<xref:Microsoft.Maui.Controls.Element>|_color_ \| `initial` |`-maui-shell-background: green;`|
|`-maui-shell-disabled`|<xref:Microsoft.Maui.Controls.Element>|_color_ \| `initial` |`-maui-shell-disabled: blue;`|
|`-maui-shell-foreground`|<xref:Microsoft.Maui.Controls.Element>|_color_ \| `initial` |`-maui-shell-foreground: yellow;`|
|`-maui-shell-tabbar-background`|<xref:Microsoft.Maui.Controls.Element>|_color_ \| `initial` |`-maui-shell-tabbar-background: white;`|
|`-maui-shell-tabbar-disabled`|<xref:Microsoft.Maui.Controls.Element>|_color_ \| `initial` |`-maui-shell-tabbar-disabled: black;`|
|`-maui-shell-tabbar-foreground`|<xref:Microsoft.Maui.Controls.Element>|_color_ \| `initial` |`-maui-shell-tabbar-foreground: gray;`|
|`-maui-shell-tabbar-title`|<xref:Microsoft.Maui.Controls.Element>|_color_ \| `initial` |`-maui-shell-tabbar-title: lightgray;`|
|`-maui-shell-tabbar-unselected`|<xref:Microsoft.Maui.Controls.Element>|_color_ \| `initial` |`-maui-shell-tabbar-unselected: cyan;`|
|`-maui-shell-title`|<xref:Microsoft.Maui.Controls.Element>|_color_ \| `initial` |`-maui-shell-title: teal;`|
|`-maui-shell-unselected`|<xref:Microsoft.Maui.Controls.Element>|_color_ \| `initial` |`-maui-shell-unselected: limegreen;`|

### Color

The following `color` values are supported:

- `X11` [colors](https://en.wikipedia.org/wiki/X11_color_names), which match CSS colors and .NET MAUI colors. These color values are case insensitive.
- hex colors: `#rgb`, `#argb`, `#rrggbb`, `#aarrggbb`
- rgb colors: `rgb(255,0,0)`, `rgb(100%,0%,0%)`. Values are in the range 0-255, or 0%-100%.
- rgba colors: `rgba(255, 0, 0, 0.8)`, `rgba(100%, 0%, 0%, 0.8)`. The opacity value is in the range 0.0-1.0.
- hsl colors: `hsl(120, 100%, 50%)`. The h value is in the range 0-360, while s and l are in the range 0%-100%.
- hsla colors: `hsla(120, 100%, 50%, .8)`. The opacity value is in the range 0.0-1.0.

### Thickness

One, two, three, or four `thickness` values are supported, each separated by white space:

- A single value indicates uniform thickness.
- Two values indicate vertical then horizontal thickness.
- Three values indicate top, then horizontal (left and right), then bottom thickness.
- Four values indicate top, then right, then bottom, then left thickness.

> [!NOTE]
> CSS `thickness` values differ from XAML `Thickness` values. For example, in XAML a two-value `Thickness` indicates horizontal then vertical thickness, while a four-value `Thickness` indicates left, then top, then right, then bottom thickness. In addition, XAML `Thickness` values are comma delimited.

## Functions

Linear and radial gradients can be specified using the `linear-gradient()` and `radial-gradient()` CSS functions, respectively. The result of these functions should be assigned to the `background` property of a control.
