---
title: "Behaviors"
description: ".NET MAUI behaviors let you add functionality to user interface controls without having to subclass them. Instead, the functionality is implemented in a behavior class and attached to the control as if it was part of the control itself."
ms.date: 08/24/2023
---

# Behaviors

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-behaviors)

.NET Multi-platform App UI (.NET MAUI) behaviors let you add functionality to user interface controls without having to subclass them. Instead, the functionality is implemented in a behavior class and attached to the control as if it was part of the control itself.

Behaviors enable you to implement code that you would normally have to write as code-behind, because it directly interacts with the API of the control in such a way that it can be concisely attached to the control and packaged for reuse across more than one application. They can be used to provide a full range of functionality to controls, such as:

- Adding an email validator to an <xref:Microsoft.Maui.Controls.Entry>.
- Creating a rating control using a tap gesture recognizer.
- Controlling an animation.

.NET MAUI supports three different types of behaviors:

- Attached behaviors are `static` classes with one or more attached properties. For more information about attached behaviors, see [Attached behaviors](#attached-behaviors).
- .NET MAUI behaviors are classes that derive from the <xref:Microsoft.Maui.Controls.Behavior> or <xref:Microsoft.Maui.Controls.Behavior`1> class, where `T` is the type of the control to which the behavior should apply. For more information, see [.NET MAUI Behaviors](#net-maui-behaviors).
- Platform behaviors are classes that derive from the <xref:Microsoft.Maui.Controls.PlatformBehavior`1> or <xref:Microsoft.Maui.Controls.PlatformBehavior`2> class. These behaviors can respond to arbitrary conditions and events on a native control. For more information, see [Platform behaviors](#platform-behaviors).

## Attached behaviors

Attached behaviors are static classes with one or more attached properties. An attached property is a special type of bindable property. They are defined in one class but attached to other objects, and they are recognizable in XAML as attributes that contain a class and a property name separated by a period. For more information about attached properties, see [Attached properties](~/fundamentals/attached-properties.md).

An attached property can define a `propertyChanged` delegate that will be executed when the value of the property changes, such as when the property is set on a control. When the `propertyChanged` delegate executes, it's passed a reference to the control on which it is being attached, and parameters that contain the old and new values for the property. This delegate can be used to add new functionality to the control that the property is attached to by manipulating the reference that is passed in, as follows:

1. The `propertyChanged` delegate casts the control reference, which is received as a <xref:Microsoft.Maui.Controls.BindableObject>, to the control type that the behavior is designed to enhance.
1. The `propertyChanged` delegate modifies properties of the control, calls methods of the control, or registers event handlers for events exposed by the control, to implement the core behavior functionality.

> [!WARNING]
> Attached behaviors are defined in a `static` class, with `static` properties and methods. This makes it difficult to create attached behaviors that have state.

### Create an attached behavior

An attached behavior can be implemented by creating a static class that contains an attached property that specifies a `propertyChanged` delegate.

The following example shows the `AttachedNumericValidationBehavior` class, which highlights the value entered by the user into an <xref:Microsoft.Maui.Controls.Entry> control in red if it's not a `double`:

```csharp
public static class AttachedNumericValidationBehavior
{
    public static readonly BindableProperty AttachBehaviorProperty =
        BindableProperty.CreateAttached("AttachBehavior", typeof(bool), typeof(AttachedNumericValidationBehavior), false, propertyChanged: OnAttachBehaviorChanged);

    public static bool GetAttachBehavior(BindableObject view)
    {
        return (bool)view.GetValue(AttachBehaviorProperty);
    }

    public static void SetAttachBehavior(BindableObject view, bool value)
    {
        view.SetValue(AttachBehaviorProperty, value);
    }

    static void OnAttachBehaviorChanged(BindableObject view, object oldValue, object newValue)
    {
        Entry entry = view as Entry;
        if (entry == null)
        {
            return;
        }

        bool attachBehavior = (bool)newValue;
        if (attachBehavior)
        {
            entry.TextChanged += OnEntryTextChanged;
        }
        else
        {
            entry.TextChanged -= OnEntryTextChanged;
        }
    }

    static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
    {
        double result;
        bool isValid = double.TryParse(args.NewTextValue, out result);
        ((Entry)sender).TextColor = isValid ? Colors.Black : Colors.Red;
    }
}
```

In this example, the `AttachedNumericValidationBehavior` class contains an attached property named `AttachBehavior` with a `static` getter and setter, which controls the addition or removal of the behavior to the control to which it will be attached. This attached property registers the `OnAttachBehaviorChanged` method that will be executed when the value of the property changes. This method registers or de-registers an event handler for the `TextChanged` event, based on the value of the `AttachBehavior` attached property. The core functionality of the behavior is provided by the `OnEntryTextChanged` method, which parses the value entered in the <xref:Microsoft.Maui.Controls.Entry> and sets the `TextColor` property to red if the value isn't a `double`.

### Consume an attached behavior

An attached behavior can be consumed by setting its attached property on the target control.

The following example shows consuming the `AttachedNumericValidationBehavior` class on an <xref:Microsoft.Maui.Controls.Entry> by adding the `AttachBehavior` attached property to the <xref:Microsoft.Maui.Controls.Entry>:

```xaml

<ContentPage ...
             xmlns:local="clr-namespace:BehaviorsDemos">
    <Entry Placeholder="Enter a System.Double" local:AttachedNumericValidationBehavior.AttachBehavior="true" />
</ContentPage>
```

The equivalent <xref:Microsoft.Maui.Controls.Entry> in C# is shown in the following code example:

```csharp
Entry entry = new Entry { Placeholder = "Enter a System.Double" };
AttachedNumericValidationBehavior.SetAttachBehavior(entry, true);
```

The following screenshot shows the attached behavior responding to invalid input:

:::image type="content" source="media/behaviors/behavior.png" alt-text="Screenshot of attached behavior responding to invalid input":::

> [!NOTE]
> Attached behaviors are written for a specific control type (or a superclass that can apply to many controls), and they should only be added to a compatible control.

### Remove an attached behavior

The `AttachedNumericValidationBehavior` class can be removed from a control by setting the `AttachBehavior` attached property to `false`:

```xaml
<Entry Placeholder="Enter a System.Double" local:AttachedNumericValidationBehavior.AttachBehavior="false" />
```

At runtime, the `OnAttachBehaviorChanged` method will be executed when the value of the `AttachBehavior` attached property is set to `false`. The `OnAttachBehaviorChanged` method will then de-register the event handler for the `TextChanged` event, ensuring that the behavior isn't executed as you interact with the control.

## .NET MAUI behaviors

.NET MAUI behaviors are created by deriving from the <xref:Microsoft.Maui.Controls.Behavior> or <xref:Microsoft.Maui.Controls.Behavior`1> class.

The process for creating a .NET MAUI behavior is as follows:

1. Create a class that inherits from the <xref:Microsoft.Maui.Controls.Behavior> or <xref:Microsoft.Maui.Controls.Behavior`1> class, where `T` is the type of the control to which the behavior should apply.
1. Override the <xref:Microsoft.Maui.Controls.Behavior`1.OnAttachedTo%2A> method to perform any required setup.
1. Override the <xref:Microsoft.Maui.Controls.Behavior`1.OnDetachingFrom%2A> method to perform any required cleanup.
1. Implement the core functionality of the behavior.

This results in the structure shown in the following example:

```csharp
public class MyBehavior : Behavior<View>
{
    protected override void OnAttachedTo(View bindable)
    {
        base.OnAttachedTo(bindable);
        // Perform setup
    }

    protected override void OnDetachingFrom(View bindable)
    {
        base.OnDetachingFrom(bindable);
        // Perform clean up
    }

    // Behavior implementation
}
```

The <xref:Microsoft.Maui.Controls.Behavior`1.OnAttachedTo%2A> method is called immediately after the behavior is attached to a control. This method receives a reference to the control to which it is attached, and can be used to register event handlers or perform other setup that's required to support the behavior functionality. For example, you could subscribe to an event on a control. The behavior functionality would then be implemented in the event handler for the event.

The <xref:Microsoft.Maui.Controls.Behavior`1.OnDetachingFrom%2A> method is called when the behavior is removed from the control. This method receives a reference to the control to which it is attached, and is used to perform any required cleanup. For example, you could unsubscribe from an event on a control to prevent memory leaks.

The behavior can then be consumed by attaching it to the <xref:Microsoft.Maui.Controls.VisualElement.Behaviors> collection of the control.

### Create a .NET MAUI Behavior

A .NET MAUI behavior can be implemented by creating a class that derives from the <xref:Microsoft.Maui.Controls.Behavior> or <xref:Microsoft.Maui.Controls.Behavior`1> class, and overriding the <xref:Microsoft.Maui.Controls.Behavior`1.OnAttachedTo%2A> and <xref:Microsoft.Maui.Controls.Behavior`1.OnDetachingFrom%2A> methods.

The following example shows the `NumericValidationBehavior` class, which highlights the value entered by the user into an <xref:Microsoft.Maui.Controls.Entry> control in red if it's not a `double`:

```csharp
public class NumericValidationBehavior : Behavior<Entry>
{
    protected override void OnAttachedTo(Entry entry)
    {
        entry.TextChanged += OnEntryTextChanged;
        base.OnAttachedTo(entry);
    }

    protected override void OnDetachingFrom(Entry entry)
    {
        entry.TextChanged -= OnEntryTextChanged;
        base.OnDetachingFrom(entry);
    }

    void OnEntryTextChanged(object sender, TextChangedEventArgs args)
    {
        double result;
        bool isValid = double.TryParse(args.NewTextValue, out result);
        ((Entry)sender).TextColor = isValid ? Colors.Black : Colors.Red;
    }
}
```

In this example, the `NumericValidationBehavior` class derives from the <xref:Microsoft.Maui.Controls.Behavior`1> class, where `T` is an <xref:Microsoft.Maui.Controls.Entry>. The <xref:Microsoft.Maui.Controls.Behavior`1.OnAttachedTo%2A> method registers an event handler for the `TextChanged` event, with the <xref:Microsoft.Maui.Controls.Behavior`1.OnDetachingFrom%2A> method de-registering the `TextChanged` event to prevent memory leaks. The core functionality of the behavior is provided by the `OnEntryTextChanged` method, which parses the value entered in the <xref:Microsoft.Maui.Controls.Entry> and sets the `TextColor` property to red if the value isn't a `double`.

> [!IMPORTANT]
> .NET MAUI does not set the `BindingContext` of a behavior, because behaviors can be shared and applied to multiple controls through styles.

### Consume a .NET MAUI behavior

Every .NET MAUI control has a <xref:Microsoft.Maui.Controls.VisualElement.Behaviors> collection, to which one or more behaviors can be added:

```xaml
<Entry Placeholder="Enter a System.Double">
    <Entry.Behaviors>
        <local:NumericValidationBehavior />
    </Entry.Behaviors>
</Entry>
```

The equivalent <xref:Microsoft.Maui.Controls.Entry> in C# is shown in the following code example:

```csharp
Entry entry = new Entry { Placeholder = "Enter a System.Double" };
entry.Behaviors.Add(new NumericValidationBehavior());
```

The following screenshot shows the .NET MAUI behavior responding to invalid input:

:::image type="content" source="media/behaviors/behavior.png" alt-text="Screenshot of .NET MAUI behavior responding to invalid input":::

> [!WARNING]
> .NET MAUI behaviors are written for a specific control type (or a superclass that can apply to many controls), and they should only be added to a compatible control. Attempting to attach a .NET MAUI behavior to an incompatible control will result in an exception being thrown.

### Consume a .NET MAUI behavior with a style

.NET MAUI behaviors can be consumed by an explicit or implicit style. However, creating a style that sets the <xref:Microsoft.Maui.Controls.VisualElement.Behaviors> property of a control is not possible because the property is read-only. The solution is to add an attached property to the behavior class that controls adding and removing the behavior. The process is as follows:

1. Add an attached property to the behavior class that will be used to control the addition or removal of the behavior to the control to which the behavior will be attached. Ensure that the attached property registers a `propertyChanged` delegate that will be executed when the value of the property changes.
1. Create a `static` getter and setter for the attached property.
1. Implement logic in the `propertyChanged` delegate to add and remove the behavior.

The following example shows the `NumericValidationStyleBehavior` class, which has an attached property that controls adding and removing the behavior:

```csharp
public class NumericValidationStyleBehavior : Behavior<Entry>
{
    public static readonly BindableProperty AttachBehaviorProperty =
        BindableProperty.CreateAttached("AttachBehavior", typeof(bool), typeof(NumericValidationStyleBehavior), false, propertyChanged: OnAttachBehaviorChanged);

    public static bool GetAttachBehavior(BindableObject view)
    {
        return (bool)view.GetValue(AttachBehaviorProperty);
    }

    public static void SetAttachBehavior(BindableObject view, bool value)
    {
        view.SetValue(AttachBehaviorProperty, value);
    }

    static void OnAttachBehaviorChanged(BindableObject view, object oldValue, object newValue)
    {
        Entry entry = view as Entry;
        if (entry == null)
        {
            return;
        }

        bool attachBehavior = (bool)newValue;
        if (attachBehavior)
        {
            entry.Behaviors.Add(new NumericValidationStyleBehavior());
        }
        else
        {
            Behavior toRemove = entry.Behaviors.FirstOrDefault(b => b is NumericValidationStyleBehavior);
            if (toRemove != null)
            {
                entry.Behaviors.Remove(toRemove);
            }
        }
    }

    protected override void OnAttachedTo(Entry entry)
    {
        entry.TextChanged += OnEntryTextChanged;
        base.OnAttachedTo(entry);
    }

    protected override void OnDetachingFrom(Entry entry)
    {
        entry.TextChanged -= OnEntryTextChanged;
        base.OnDetachingFrom(entry);
    }

    void OnEntryTextChanged(object sender, TextChangedEventArgs args)
    {
        double result;
        bool isValid = double.TryParse(args.NewTextValue, out result);
        ((Entry)sender).TextColor = isValid ? Colors.Black : Colors.Red;
    }
}
```

In this example, the `NumericValidationStyleBehavior` class contains an attached property named `AttachBehavior` with a `static` getter and setter, which controls the addition or removal of the behavior to the control to which it will be attached. This attached property registers the `OnAttachBehaviorChanged` method that will be executed when the value of the property changes. This method adds or removes the behavior to the control, based on the value of the `AttachBehavior` attached property.

The following code example shows an *explicit* style for the `NumericValidationStyleBehavior` that uses the `AttachBehavior` attached property, and which can be applied to <xref:Microsoft.Maui.Controls.Entry> controls:

```xaml
<Style x:Key="NumericValidationStyle" TargetType="Entry">
    <Style.Setters>
        <Setter Property="local:NumericValidationStyleBehavior.AttachBehavior" Value="true" />
    </Style.Setters>
</Style>
```

The <xref:Microsoft.Maui.Controls.Style> can be applied to an <xref:Microsoft.Maui.Controls.Entry> by setting its <xref:Microsoft.Maui.Controls.NavigableElement.Style> property to the style using the [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) markup extension:

```xaml
<Entry Placeholder="Enter a System.Double" Style="{StaticResource NumericValidationStyle}">
```

For more information about styles, see [Styles](~/user-interface/styles/xaml.md).

> [!NOTE]
> While you can add bindable properties to a behavior that is set or queried in XAML, if you do create behaviors that have state they should not be shared between controls in a <xref:Microsoft.Maui.Controls.Style> in a <xref:Microsoft.Maui.Controls.ResourceDictionary>.

### Remove a .NET MAUI behavior

The <xref:Microsoft.Maui.Controls.Behavior`1.OnDetachingFrom%2A> method is called when a behavior is removed from a control, and is used to perform any required cleanup such as unsubscribing from an event to prevent a memory leak. However, behaviors are not implicitly removed from controls unless the control's <xref:Microsoft.Maui.Controls.VisualElement.Behaviors> collection is modified by the `Remove` or `Clear` method:

```csharp
Behavior toRemove = entry.Behaviors.FirstOrDefault(b => b is NumericValidationStyleBehavior);
if (toRemove != null)
{
    entry.Behaviors.Remove(toRemove);
}
```

Alternatively, the control's <xref:Microsoft.Maui.Controls.VisualElement.Behaviors> collection can be cleared:

```csharp
entry.Behaviors.Clear();
```

> [!NOTE]
> .NET MAUI behaviors aren't implicitly removed from controls when pages are popped from the navigation stack. Instead, they must be explicitly removed prior to pages going out of scope.

## Platform behaviors

Platform behaviors are created by deriving from the <xref:Microsoft.Maui.Controls.PlatformBehavior`1> or <xref:Microsoft.Maui.Controls.PlatformBehavior`2> class. They respond to arbitrary conditions and events on a native control.

A platform behavior can be implemented through conditional compilation, or partial classes. The approach adopted here is to use partial classes, where a platform behavior typically consists of a cross-platform partial class that defines the behaviors API, and a native partial class that implements the behavior on each platform. At build time, multi-targeting combines the partial classes to build the platform behavior on each platform.

The process for creating a platform behavior is as follows:

1. Create a cross-platform partial class that defines the API for the platform behavior.
1. Create a native partial class on each platform your app is built for, that has the same name as the cross-platform partial class. This native partial class should inherit from the <xref:Microsoft.Maui.Controls.PlatformBehavior`1> or <xref:Microsoft.Maui.Controls.PlatformBehavior`2> class, where `TView` is the cross-platform control to which the behavior should apply, and `TPlatformView` is the native view that implements the cross-platform control on a particular platform.

    > [!NOTE]
    > While it's required to create a native partial class on each platform your app is built for, it's not required to implement the platform behavior functionality on every platform. For example, you might create a platform behavior that modifies the border thickness of a native control on some, but not all, platforms.

1. In each native partial class that you require to implement the platform behavior you should:
    1. Override the <xref:Microsoft.Maui.Controls.PlatformBehavior`2.OnAttachedTo%2A> method to perform any setup.
    1. Override the <xref:Microsoft.Maui.Controls.PlatformBehavior`2.OnDetachedFrom%2A> method to perform any cleanup.
    1. Implement the core functionality of the platform behavior.

The behavior can then be consumed by attaching it to the <xref:Microsoft.Maui.Controls.VisualElement.Behaviors> collection of the control.

### Create a platform behavior

To create a platform behavior you must first create a cross-platform partial class that defines the API for the platform behavior:

```csharp
namespace BehaviorsDemos
{
    public partial class TintColorBehavior
    {
        public static readonly BindableProperty TintColorProperty =
            BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(TintColorBehavior));

        public Color TintColor
        {
            get => (Color)GetValue(TintColorProperty);
            set => SetValue(TintColorProperty, value);
        }
    }
}
```

The platform behavior is a partial class whose implementation will be completed on each required platform with an additional partial class that uses the same name. In this example, the `TintColorBehavior` class defines a single bindable property, `TintColor`, that will tint an image with the specified color.

After creating the cross-platform partial class you should create a native partial class on each platform you build the app for. This can be accomplished by adding partial classes to the required child folders of the *Platforms* folder:

:::image type="content" source="media/behaviors/platform-behavior-native-partial-classes.png" alt-text="Screenshot of the native partial classes for a platform behavior.":::

Alternatively you could configure your project to support filename-based multi-targeting, or folder-based multi-targeting, or both. For more information about multi-targeting, see [Configure multi-targeting](~/platform-integration/configure-multi-targeting.md).

The native partial classes should inherit from the <xref:Microsoft.Maui.Controls.PlatformBehavior`1> class or the <xref:Microsoft.Maui.Controls.PlatformBehavior`2> class, where `TView` is the cross-platform control to which the behavior should apply, and `TPlatformView` is the native view that implements the cross-platform control on a particular platform. In each native partial class that you require to implement the platform behavior, you should override the <xref:Microsoft.Maui.Controls.PlatformBehavior`2.OnAttachedTo%2A> method and the <xref:Microsoft.Maui.Controls.PlatformBehavior`2.OnDetachedFrom%2A> method, and implement the core functionality of the platform behavior.

The <xref:Microsoft.Maui.Controls.PlatformBehavior`2.OnAttachedTo%2A> method is called immediately after the platform behavior is attached to a cross-platform control. The method receives a reference to the cross-platform control to which it is attached, and optionally a reference to the native control that implements the cross-platform control. The method can be used to register event handlers or perform other setup that's required to support the platform behavior functionality. For example, you could subscribe to an event on a control. The behavior functionality would then be implemented in the event handler for the event.

The <xref:Microsoft.Maui.Controls.PlatformBehavior`2.OnDetachedFrom%2A> method is called when the behavior is removed from the cross-platform control. The method receives a reference to the control to which it is attached, and optionally a reference to the native control that implements the cross-platform control. The method should be used to perform any required cleanup. For example, you could unsubscribe from an event on a control to prevent memory leaks.

> [!IMPORTANT]
> The partial classes must reside in the same namespace and use identical names.

The following example shows the `TintColorBehavior` partial class for Android, which tints an image with a specified color:

```csharp
using Android.Graphics;
using Android.Widget;
using Microsoft.Maui.Platform;
using Color = Microsoft.Maui.Graphics.Color;

namespace BehaviorsDemos
{
    public partial class TintColorBehavior : PlatformBehavior<Image, ImageView>
    {
        protected override void OnAttachedTo(Image bindable, ImageView platformView)
        {
            base.OnAttachedTo(bindable, platformView);

            if (bindable is null)
                return;
            if (TintColor is null)
                ClearColor(platformView);
            else
                ApplyColor(platformView, TintColor);
        }

        protected override void OnDetachedFrom(Image bindable, ImageView platformView)
        {
            base.OnDetachedFrom(bindable, platformView);

            if (bindable is null)
                return;
            ClearColor(platformView);
        }

        void ApplyColor(ImageView imageView, Color color)
        {
            imageView.SetColorFilter(new PorterDuffColorFilter(color.ToPlatform(), PorterDuff.Mode.SrcIn ?? throw new NullReferenceException()));
        }

        void ClearColor(ImageView imageView)
        {
            imageView.ClearColorFilter();
        }
    }
}
```

In this example, the `TintColorBehavior` class derives from the <xref:Microsoft.Maui.Controls.PlatformBehavior`2> class, where `TView` is an <xref:Microsoft.Maui.Controls.Image> and `TPlatformView` is an <xref:Android.Widget.ImageView>. The <xref:Microsoft.Maui.Controls.PlatformBehavior`2.OnAttachedTo%2A> applies the tint color to the image, provided that the `TintColor` property has a value. The <xref:Microsoft.Maui.Controls.PlatformBehavior`2.OnDetachedFrom%2A> method removes the tint color from the image.

A native partial class must be added on each platform you build your app for. However, you can make the native partial class NO-OP, if the platform behavior isn't required on a specific platform. This can be achieved by providing an empty class:

```csharp
using Microsoft.UI.Xaml;

namespace BehaviorsDemos
{
    public partial class TintColorBehavior : PlatformBehavior<Image, FrameworkElement>
    {
        // NO-OP on Windows
    }
}
```

> [!IMPORTANT]
> .NET MAUI doesn't set the `BindingContext` of a platform behavior.

### Consume a platform behavior

Every .NET MAUI control has a <xref:Microsoft.Maui.Controls.VisualElement.Behaviors> collection, to which one or more platform behaviors can be added:

```xaml
<Image Source="dotnet_bot.png"
       HeightRequest="200"
       HorizontalOptions="Center">
    <Image.Behaviors>
        <local:TintColorBehavior TintColor="Red" />
    </Image.Behaviors>
</Image>
```

The equivalent <xref:Microsoft.Maui.Controls.Image> in C# is shown in the following example:

```csharp
Image image = new Image { Source = "dotnet_bot.png", HeightRequest = 200, HorizontalOptions = LayoutOptions.Center };
image.Behaviors.Add(new TintColorBehavior());
```

The following screenshot shows the platform behavior tinting an image:

:::image type="content" source="media/behaviors/platform-behavior.png" alt-text="Screenshot of .NET MAUI platform behavior tinting an image.":::

> [!WARNING]
> Platform behaviors are written for a specific control type (or a superclass that can apply to many controls), and they should only be added to a compatible control. Attempting to attach a platform behavior to an incompatible control will result in an exception being thrown.
