---
title: "C# expressions in XAML"
description: "Use C# expressions with .NET MAUI XAML source generation to bind values, compute display text, and handle events."
ms.date: 10/08/2026
monikerRange: ">=net-maui-11.0"
ai-usage: ai-generated
---

# C# expressions in XAML

In .NET MAUI 11, you can write short C# expressions in XAML property values and event attributes. Use them for computed display values or a short event action. Keep larger operations in your view model or code-behind.

> [!IMPORTANT]
> XAML C# expressions are a preview feature. They require XAML source generation and `EnablePreviewFeatures`. XamlC and runtime inflation don't support this syntax.

Set these properties in your project file:

```xml
<PropertyGroup>
    <MauiXamlInflator>SourceGen</MauiXamlInflator>
    <EnablePreviewFeatures>true</EnablePreviewFeatures>
</PropertyGroup>
```

These settings apply to Debug and Release builds. Don't override the inflator with `Runtime` or `XamlC` for a file that uses expressions. Without `EnablePreviewFeatures`, the generator reports `MAUIX2012`. For more information about inflators and per-file settings, see [XAML processing](xamlc.md).

## Bind values and handle events

Declare `x:DataType` for expressions that use your <xref:Microsoft.Maui.Controls.BindableObject.BindingContext>. The following example uses a `ProductViewModel` with `ProductName`, `Quantity`, and `UnitPrice` properties. Define `GetStatusText()` and `Save()` on `MainPage`.

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MyApp.ViewModels"
             x:Class="MyApp.MainPage"
             x:DataType="local:ProductViewModel">
    <VerticalStackLayout>
        <Entry Text="{ProductName}" />
        <Label Text="{$'{Quantity} at {UnitPrice:C2}'}" />
        <Label Text="{this.GetStatusText()}" />
        <Button Text="Save" Clicked="{(sender, e) => this.Save()}" />
    </VerticalStackLayout>
</ContentPage>
```

`x:DataType` declares a type; it doesn't set the binding context. Assign a `ProductViewModel` instance to `BindingContext`, and raise property-change notifications when its values change. For more information, see [Compiled bindings](../fundamentals/data-binding/compiled-bindings.md).

The generator handles these values in different ways:

| Value | Behavior |
| --- | --- |
| `{ProductName}` | Creates a typed binding to the binding context. A writable property path supports updates to the source when the target's binding mode is two-way, as with `Entry.Text`. |
| `{$'{Quantity} at {UnitPrice:C2}'}` | Creates a computed binding. Property-change notifications for its binding-context inputs update the result. The expression has no generated setter. |
| `{this.GetStatusText()}` | Evaluates the page method when the XAML is initialized. This isn't a binding that tracks changes on the page. Page values used in a mixed binding expression are also captured at initialization. |
| `{(sender, e) => this.Save()}` | Connects a delegate to the event. The method runs when the event occurs, not when the XAML is initialized. |

Use single quotes for strings inside a double-quoted XAML attribute. The generator converts them to C# string quotes. Escape XML characters, for example `&amp;&amp;` for `&&` and `&lt;` for `<`.

Existing markup extensions, such as `{Binding ProductName}` and `{StaticResource AccentColor}`, retain their meaning. If a member name matches a markup extension, use `{= MemberName}` to force an expression. If a member exists on both the page and the binding context, use `{this.MemberName}` for the page or `{.MemberName}` for the binding context.

## Refer to types with namespace prefixes

In .NET MAUI 11 RC 2, expressions can refer to types through a declared XAML namespace prefix. For example, declare `helpers` and call a static method on a type in that namespace:

```xaml
<Label xmlns:helpers="clr-namespace:MyApp.Helpers"
       Text="{helpers:DisplayHelper.GetCaption()}" />
```

Define a public static `GetCaption()` method that returns a string on `MyApp.Helpers.DisplayHelper`. The generator resolves `helpers:DisplayHelper` to its fully qualified C# type name. This type-reference syntax doesn't change how prefixed markup extensions, such as `{x:Static ...}`, are interpreted.

## Read attached bindable properties

Use `target.(Type.Property)` to read an attached bindable property from a named element:

```xaml
<VerticalStackLayout>
    <Grid RowDefinitions="Auto,Auto,Auto">
        <Button x:Name="myButton" Grid.Row="2" Text="Save" />
    </Grid>
    <Label Text="{$'Row {myButton.(Grid.Row)}'}" />
</VerticalStackLayout>
```

The generator converts `myButton.(Grid.Row)` to `Grid.GetRow(myButton)`. You can also use a namespace prefix for the declaring type. `this` refers to the root page or view, not the element that contains the expression.

This syntax doesn't generate an attached-property setter or subscribe to changes in the attached property. Don't use it as a two-way binding or expect the label to update when `Grid.Row` changes. The target is required; the standalone expression `{(Grid.Row)}` isn't supported.

## Bind to a struct sub-property

In RC 2, a writable property path can include a struct property, such as a view model's <xref:Microsoft.Maui.Thickness> property:

```xaml
<Slider Minimum="0" Maximum="100" Value="{.Margin.Top}" />
```

Here, `Margin` belongs to the binding context declared by `x:DataType`. For a two-way target, the generated setter copies `Margin`, changes `Top`, and assigns the struct back to `Margin`. Other thickness values are preserved. The intermediate struct property and the final property need public, non-init setters. Raise a property-change notification for `Margin` when you replace it.

If the intermediate struct property has no suitable setter, the generator omits the setter. On a target that defaults to two-way binding, it reports `MAUIX2018`, and updates flow only from the source to the target.

## Resolve common errors

Use these diagnostics to correct expressions:

| Diagnostic | Correction |
| --- | --- |
| `MAUIX2008` | A member exists on both the page and binding context. Use `this.` or the leading `.` to select the source. |
| `MAUIX2009` | Check the member name and `x:DataType`, including names inside interpolated strings. |
| `MAUIX2018` | Add a suitable setter to the intermediate struct property, or use a one-way target. |
| `MAUIX2019` | Invoke the method in an event lambda. Write `(sender, e) => this.Save()`, not `(sender, e) => this.Save`. |

Use an event lambda with parameters that match the event, and a single expression body. Async event lambdas aren't supported; use a named method in code-behind for async work. Computed expressions don't support writes to the source. A computed expression on a two-way-by-default target reports `MAUIX2010`.
