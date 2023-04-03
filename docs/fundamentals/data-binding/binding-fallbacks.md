---
title: "Binding fallbacks"
description: ".NET MAUI data bindings can be made more robust by defining fallback values that will be used if binding fails."
ms.date: 01/19/2022
---

# Binding fallbacks

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-databinding)

Sometimes data bindings fail, because the binding source can't be resolved, or because the binding succeeds but returns a `null` value. While these scenarios can be handled with value converters, or other additional code, data bindings can be made more robust by defining fallback values to use if the binding process fails. In a .NET Multi-platform App UI (.NET MAUI) app this can be accomplished by defining the `FallbackValue` and `TargetNullValue` properties in a binding expression. Because these properties reside in the `BindingBase` class, they can be used with bindings, multi-bindings, compiled bindings, and with the `Binding` markup extension.

> [!NOTE]
> Use of the `FallbackValue` and `TargetNullValue` properties in a binding expression is optional.

## Define a fallback value

The `FallbackValue` property allows a fallback value to be defined that will be used when the binding *source* can't be resolved. A common scenario for setting this property is when binding to source properties that might not exist on all objects in a bound collection of heterogeneous types.

The following example demonstrates setting the `FallbackValue` property:

```xaml
<Label Text="{Binding Population, FallbackValue='Population size unknown'}"
       ... />   
```

The binding on the <xref:Microsoft.Maui.Controls.Label> defines a `FallbackValue` value (delimited by single-quote characters) that will be set on the target if the binding source can't be resolved. Therefore, the value defined by the `FallbackValue` property will be displayed if the `Population` property doesn't exist on the bound object.

Rather than defining `FallbackValue` property values inline, it's recommended to define them as resources in a <xref:Microsoft.Maui.Controls.ResourceDictionary>. The advantage of this approach is that such values are defined once in a single location, and are more easily localizable. The resources can then be retrieved using the [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) markup extension:

```xaml
<Label Text="{Binding Population, FallbackValue={StaticResource populationUnknown}}"
       ... />  
```

> [!NOTE]
> It's not possible to set the `FallbackValue` property with a binding expression.

When the `FallbackValue` property isn't set in a binding expression and the binding path or part of the path isn't resolved, `BindableProperty.DefaultValue` is set on the target. However, when the `FallbackValue` property is set and the binding path or part of the path isn't resolved, the value of the `FallbackValue` value property is set on the target:

:::image type="content" source="media/binding-fallbacks/bindingunavailable-detail.png" alt-text="FallbackValue Binding.":::

Therefore, in this example the <xref:Microsoft.Maui.Controls.Label> displays "Population size unknown" because the bound object lacks a `Population` property.

> [!IMPORTANT]
> A defined value converter is not executed in a binding expression when the `FallbackValue` property is set.

## Define a null replacement value

The `TargetNullValue` property allows a replacement value to be defined that will be used when the binding *source* is resolved, but the value is `null`. A common scenario for setting this property is when binding to source properties that might be `null` in a bound collection.

The following example demonstrates setting the `TargetNullValue` property:

```xaml
<ListView ItemsSource="{Binding Monkeys}"
          ...>
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                <Grid>
                    ...
                    <Image Source="{Binding ImageUrl, TargetNullValue='https://upload.wikimedia.org/wikipedia/commons/2/20/Point_d_interrogation.jpg'}"
                           ... />
                    ...
                    <Label Text="{Binding Location, TargetNullValue='Location unknown'}"
                           ... />
                </Grid>
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

The bindings on the <xref:Microsoft.Maui.Controls.Image> and <xref:Microsoft.Maui.Controls.Label> both define `TargetNullValue` values (delimited by single-quote characters) that will be applied if the binding path returns `null`. Therefore, the values defined by the `TargetNullValue` properties will be displayed for any objects in the collection where the `ImageUrl` and `Location` properties are not defined.

Rather than defining `TargetNullValue` property values inline, it's recommended to define them as resources in a <xref:Microsoft.Maui.Controls.ResourceDictionary>. The advantage of this approach is that such values are defined once in a single location, and are more easily localizable. The resources can then be retrieved using the [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) markup extension:

```xaml
<Image Source="{Binding ImageUrl, TargetNullValue={StaticResource fallbackImageUrl}}"
       ... />
<Label Text="{Binding Location, TargetNullValue={StaticResource locationUnknown}}"
       ... />
```

> [!NOTE]
> It's not possible to set the `TargetNullValue` property with a binding expression.

When the `TargetNullValue` property isn't set in a binding expression, a source value of `null` will be converted if a value converter is defined, formatted if a `StringFormat` is defined, and the result is then set on the target. However, when the `TargetNullValue` property is set, a source value of `null` will be converted if a value converter is defined, and if it's still `null` after the conversion, the value of the `TargetNullValue` property is set on the target:

:::image type="content" source="media/binding-fallbacks/bindingunavailable.png" alt-text="TargetNullValue Binding.":::

Therefore, in this example the <xref:Microsoft.Maui.Controls.Image> and <xref:Microsoft.Maui.Controls.Label> objects display their `TargetNullValue` when their source objects are `null`.

> [!IMPORTANT]
> String formatting is not applied in a binding expression when the `TargetNullValue` property is set.
