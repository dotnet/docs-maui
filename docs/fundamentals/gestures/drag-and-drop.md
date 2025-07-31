---
title: "Recognize a drag and drop gesture"
description: "This article explains how to recognize drag and drop gestures with .NET MAUI."
ms.date: 11/06/2024
ms.custom: sfi-ropc-nochange
---

# Recognize a drag and drop gesture

A .NET Multi-platform App UI (.NET MAUI) drag and drop gesture recognizer enables items, and their associated data packages, to be dragged from one onscreen location to another location using a continuous gesture. Drag and drop can take place in a single application, or it can start in one application and end in another.

The *drag source*, which is the element on which the drag gesture is initiated, can provide data to be transferred by populating a data package object. When the drag source is released, drop occurs. The *drop target*, which is the element under the drag source, then processes the data package.

The process for enabling drag and drop in an app is as follows:

1. Enable drag on an element by adding a <xref:Microsoft.Maui.Controls.DragGestureRecognizer> object to its <xref:Microsoft.Maui.Controls.View.GestureRecognizers> collection. For more information, see [Enable drag](#enable-drag).
1. [optional] Build a data package. .NET MAUI automatically populates the data package for image and text controls, but for other content you'll need to construct your own data package. For more information, see [Build a data package](#build-a-data-package).
1. Enable drop on an element by adding a <xref:Microsoft.Maui.Controls.DropGestureRecognizer> object to its <xref:Microsoft.Maui.Controls.View.GestureRecognizers> collection. For more information, see [Enable drop](#enable-drop).
1. [optional] Handle the `DropGestureRecognizer.DragOver` event to indicate the type of operation allowed by the drop target. For more information, see [Handle the DragOver event](#handle-the-dragover-event).
1. [optional] Process the data package to receive the dropped content. .NET MAUI will automatically retrieve image and text data from the data package, but for other content you'll need to process the data package. For more information, see [Process the data package](#process-the-data-package).

<!-- > [!NOTE]
> Dragging items to and from a <xref:Microsoft.Maui.Controls.CollectionView> is currently unsupported. -->

## Enable drag

In .NET MAUI, drag gesture recognition is provided by the <xref:Microsoft.Maui.Controls.DragGestureRecognizer> class. This class defines the following properties:

- <xref:Microsoft.Maui.Controls.DragGestureRecognizer.CanDrag>, of type `bool`, which indicates whether the element the gesture recognizer is attached to can be a drag source. The default value of this property is `true`.
- <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DragStartingCommand>, of type <xref:System.Windows.Input.ICommand>, which is executed when a drag gesture is first recognized.
- <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DragStartingCommandParameter>, of type `object`, which is the parameter that's passed to the  <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DragStartingCommand>.
- <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DropCompletedCommand>, of type <xref:System.Windows.Input.ICommand>, which is executed when the drag source is dropped.
- <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DropCompletedCommandParameter>, of type `object`, which is the parameter that's passed to the <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DropCompletedCommand>.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The <xref:Microsoft.Maui.Controls.DragGestureRecognizer> class also defines <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DragStarting> and <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DropCompleted> events that fire if the <xref:Microsoft.Maui.Controls.DragGestureRecognizer.CanDrag> property is `true`. When a <xref:Microsoft.Maui.Controls.DragGestureRecognizer> object detects a drag gesture, it executes the <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DragStartingCommand> and invokes the <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DragStarting> event. Then, when the <xref:Microsoft.Maui.Controls.DragGestureRecognizer> object detects the completion of a drop gesture, it executes the <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DropCompletedCommand> and invokes the <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DropCompleted> event.

The <xref:Microsoft.Maui.Controls.DragStartingEventArgs> object that accompanies the <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DragStarting> event defines the following properties:

- <xref:Microsoft.Maui.Controls.DragStartingEventArgs.Cancel>, of type `bool`, indicates whether the event should be canceled.
- <xref:Microsoft.Maui.Controls.DragStartingEventArgs.Data>, of type <xref:Microsoft.Maui.Controls.DataPackage>, indicates the data package that accompanies the drag source. This is a read-only property.
- <xref:Microsoft.Maui.Controls.DragStartingEventArgs.PlatformArgs>, of type `PlatformDragStartingEventArgs?`, represents the platform-specific arguments associated with the event.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

On Android, the <xref:Microsoft.Maui.Controls.PlatformDragStartingEventArgs> class defines the following properties:

- `Sender`, of type <xref:Android.Views.View>, represents the native view attached to the event.
- `MotionEvent`, of type <xref:Android.Views.MotionEvent>, represents the event containing information for drag and drop status.

In addition, on Android the <xref:Microsoft.Maui.Controls.PlatformDragStartingEventArgs> class defines the following methods:

- `SetDragShadowBuilder`, which sets the <xref:Android.Views.View.DragShadowBuilder> to use when dragging begins.
- `SetClipData`, which sets the the <xref:Android.Content.ClipData> to use when dragging begins.
- `SetLocalData`, which sets the local data to use when dragging begins.
- `SetDragFlags`, which sets the <xref:Android.Views.DragFlags> to use when dragging begins.

For example, use the `SetClipData` method to associate <xref:Android.Content.ClipData> with the dragged item:

```csharp
void OnDragStarting(object sender, DragStartingEventArgs e)
{
#if ANDROID
    string content = "insert your content here";
    e.PlatformArgs.SetClipData(Android.Content.ClipData.NewPlainText("Drag data", content));
#endif
}
```

# [iOS/Mac Catalyst](#tab/macios)

On iOS and Mac Catalyst, the <xref:Microsoft.Maui.Controls.PlatformDragStartingEventArgs> class defines the following properties:

- `Sender`, of type <xref:UIKit.UIView>, represents the native view attached to the event.
- `GestureRecognizer`, of type <xref:UIKit.UIGestureRecognizer>, indicates the native event or handler attached to the view.

In addition, on iOS and Mac Catalyst the <xref:Microsoft.Maui.Controls.PlatformDragStartingEventArgs> class defines the following methods:

- `SetItemProvider`, which sets the <xref:Foundation.NSItemProvider> to use when dragging begins.
- `SetPreviewProvider`, which sets the <xref:UIKit.UIDragPreview> to use when dragging begins.
- `SetDragItems`, which sets the array of <xref:UIKit.UIDragItem> to use when dragging begins.
- `SetPrefersFullSizePreview`, which sets the func that requests that drag previews are full-sized when dragging begins.

For example, use the `SetPreviewProvider` method to set the object to use as a preview of the item being dragged:

```csharp
void OnDragStarting(object sender, DragStartingEventArgs e)
{
#if IOS || MACCATALYST
    Func<UIKit.UIDragPreview> action = () =>
    {
        var image = UIKit.UIImage.FromFile("dotnet_bot.png");
        UIKit.UIImageView imageView = new UIKit.UIImageView(image);
        imageView.ContentMode = UIKit.UIViewContentMode.Center;
        imageView.Frame = new CoreGraphics.CGRect(0, 0, 250, 250);
        return new UIKit.UIDragPreview(imageView);
    };

    e.PlatformArgs.SetPreviewProvider(action);
#endif
}
```

In this example, the preview of the dragged item is replaced with an image.

To set the drag preview to full size, use the `SetPrefersFullSizePreview` method:

```csharp
void OnDragStarting(object sender, DragStartingEventArgs e)
{
#if IOS || MACCATALYST
    e.PlatformArgs.SetPrefersFullSizePreviews((interaction, session) => { return true; });
#endif
}
```

# [Windows](#tab/windows)

On Windows, the <xref:Microsoft.Maui.Controls.PlatformDragStartingEventArgs> class defines the following properties:

- `Sender`, of type <xref:Microsoft.UI.Xaml.FrameworkElement>, represents the native view attached to the event.
- `DragStartingEventArgs`, of type <xref:Microsoft.UI.Xaml.DragStartingEventArgs>, provides event data for the native event.
- `Handled`, of type `bool`, determines if the event arguments have changed. This property should be set to `true` when changing the `DragStartingEventArgs` so that the changes aren't overridden.

---

<!-- markdownlint-enable MD025 -->

The <xref:Microsoft.Maui.Controls.DropCompletedEventArgs> object that accompanies the <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DropCompleted> event defines a <xref:Microsoft.Maui.Controls.DropCompletedEventArgs.PlatformArgs> property, of type `PlatformDropCompletedEventArgs?`, which represents the platform-specific arguments associated with the event.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

On Android, the <xref:Microsoft.Maui.Controls.PlatformDropCompletedEventArgs> class defines the following properties:

- `Sender`, of type <xref:Android.Views.View>, represents the native view attached to the event.
- `DragEvent`, of type <xref:Android.Views.DragEvent>, represents the event that's sent at various times during a drag and drop operation.

# [iOS/Mac Catalyst](#tab/macios)

On iOS and Mac Catalyst, the <xref:Microsoft.Maui.Controls.PlatformDropCompletedEventArgs> class defines the following properties:

- `Sender`, of type <xref:UIKit.UIView>, represents the native view attached to the event.
- `DragInteraction`, of type <xref:UIKit.UIDragInteraction>, indicates the interaction used for dragging items. This property is used when <xref:Microsoft.Maui.Controls.PlatformDropCompletedEventArgs> is called from `SessionWillEnd`.
- `DragSession`, of type <xref:UIKit.IUIDragSession>, retrieves the associated information from the drag session. This property is used when <xref:Microsoft.Maui.Controls.PlatformDropCompletedEventArgs> is called from `SessionWillEnd`.
- `DropOperation`, of type <xref:UIKit.UIDropOperation>, represents the response to a drop. This property is used when <xref:Microsoft.Maui.Controls.PlatformDropCompletedEventArgs> is called from `SessionWillEnd`.
- `DropInteraction`, of type <xref:UIKit.UIDropInteraction>, indicates the interaction used for dropping items. This property is used when <xref:Microsoft.Maui.Controls.PlatformDropCompletedEventArgs> is called from `PerformDrop`.
- `DropSession`, of type <xref:UIKit.IUIDropSession>, retrieves the associated information from the drop session. This property is used when <xref:Microsoft.Maui.Controls.PlatformDropCompletedEventArgs> is called from `PerformDrop`.

# [Windows](#tab/windows)

On Windows, the <xref:Microsoft.Maui.Controls.PlatformDropCompletedEventArgs> class defines the following properties:

- `Sender`, of type <xref:Microsoft.UI.Xaml.FrameworkElement>, represents the native view attached to the event.
- `DropCompletedEventArgs`, of type <xref:Microsoft.UI.Xaml.DropCompletedEventArgs>, provides event data for the native event.

---

<!-- markdownlint-enable MD025 -->

The following XAML example shows a <xref:Microsoft.Maui.Controls.DragGestureRecognizer> attached to an <xref:Microsoft.Maui.Controls.Image>:

```xaml
<Image Source="monkeyface.png">
    <Image.GestureRecognizers>
        <DragGestureRecognizer />
    </Image.GestureRecognizers>
</Image>
```

In this example, a drag gesture can be initiated on the <xref:Microsoft.Maui.Controls.Image>.

> [!TIP]
> A drag gesture is initiated with a long-press followed by a drag.

## Build a data package

.NET MAUI will automatically build a data package for you, when a drag is initiated, for the following controls:

- Text controls. Text values can be dragged from <xref:Microsoft.Maui.Controls.CheckBox>, <xref:Microsoft.Maui.Controls.DatePicker>, <xref:Microsoft.Maui.Controls.Editor>, <xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.Label>, <xref:Microsoft.Maui.Controls.RadioButton>, <xref:Microsoft.Maui.Controls.Switch>, and <xref:Microsoft.Maui.Controls.TimePicker> objects.
- Image controls. Images can be dragged from <xref:Microsoft.Maui.Controls.Button>, <xref:Microsoft.Maui.Controls.Image>, and <xref:Microsoft.Maui.Controls.ImageButton> controls.

The following table shows the properties that are read, and any conversion that's attempted, when a drag is initiated on a text control:

| Control | Property | Conversion |
| --- | --- | --- |
| <xref:Microsoft.Maui.Controls.CheckBox> | `IsChecked` | `bool` converted to a `string`. |
| <xref:Microsoft.Maui.Controls.DatePicker> | `Date` | `DateTime` converted to a `string`. |
| <xref:Microsoft.Maui.Controls.Editor> | `Text` ||
| <xref:Microsoft.Maui.Controls.Entry> | `Text` ||
| <xref:Microsoft.Maui.Controls.Label> | `Text` ||
| <xref:Microsoft.Maui.Controls.RadioButton> | `IsChecked` | `bool` converted to a `string`. |
| <xref:Microsoft.Maui.Controls.Switch> | `IsToggled` | `bool` converted to a `string`. |
| <xref:Microsoft.Maui.Controls.TimePicker> | `Time` | `TimeSpan` converted to a `string`. |

For content other than text and images, you'll need to build a data package yourself.

Data packages are represented by the <xref:Microsoft.Maui.Controls.DataPackage> class, which defines the following properties:

- <xref:Microsoft.Maui.Controls.DataPackage.Properties>, of type <xref:Microsoft.Maui.Controls.DataPackagePropertySet>, which is a collection of properties that comprise the data contained in the <xref:Microsoft.Maui.Controls.DataPackage>. This property is a read-only property.
- <xref:Microsoft.Maui.Controls.Image>, of type <xref:Microsoft.Maui.Controls.ImageSource>, which is the image contained in the <xref:Microsoft.Maui.Controls.DataPackage>.
- <xref:Microsoft.Maui.Controls.DataPackage.Text>, of type `string`, which is the text contained in the <xref:Microsoft.Maui.Controls.DataPackage>.
- <xref:Microsoft.Maui.Controls.View>, of type <xref:Microsoft.Maui.Controls.DataPackageView>, which is a read-only version of the <xref:Microsoft.Maui.Controls.DataPackage>.

The <xref:Microsoft.Maui.Controls.DataPackagePropertySet> class represents a property bag stored as a `Dictionary<string,object>`. For information about the <xref:Microsoft.Maui.Controls.DataPackageView> class, see [Process the data package](#process-the-data-package).

### Store image or text data

Image or text data can be associated with a drag source by storing the data in the `DataPackage.Image` or `DataPackage.Text` property. You can add the data in the handler for the <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DragStarting> event.

The following XAML example shows a <xref:Microsoft.Maui.Controls.DragGestureRecognizer> that registers a handler for the <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DragStarting> event:

```xaml
<Path Stroke="Black"
      StrokeThickness="4">
    <Path.GestureRecognizers>
        <DragGestureRecognizer DragStarting="OnDragStarting" />
    </Path.GestureRecognizers>
    <Path.Data>
        <!-- PathGeometry goes here -->
    </Path.Data>
</Path>
```

In this example, the <xref:Microsoft.Maui.Controls.DragGestureRecognizer> is attached to a <xref:Microsoft.Maui.Controls.Shapes.Path> object. The <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DragStarting> event is raised when a drag gesture is detected on the <xref:Microsoft.Maui.Controls.Shapes.Path>, which executes the `OnDragStarting` event handler:

```csharp
void OnDragStarting(object sender, DragStartingEventArgs e)
{
    e.Data.Text = "My text data goes here";
}
```

The <xref:Microsoft.Maui.Controls.DragStartingEventArgs> object that accompanies the <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DragStarting> event has a `Data` property, of type <xref:Microsoft.Maui.Controls.DataPackage>. In this example, the `Text` property of the <xref:Microsoft.Maui.Controls.DataPackage> object is set to a `string`. The <xref:Microsoft.Maui.Controls.DataPackage> can then be accessed on drop, to retrieve the `string`.

### Store data in the property bag

Any data, including images and text, can be associated with a drag source by storing the data in the `DataPackage.Properties` collection. You can add the data in the handler for the <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DragStarting> event.

The following XAML example shows a <xref:Microsoft.Maui.Controls.DragGestureRecognizer> that registers a handler for the <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DragStarting> event:

```xaml
<Rectangle Stroke="Red"
           Fill="DarkBlue"
           StrokeThickness="4"
           HeightRequest="200"
           WidthRequest="200">
    <Rectangle.GestureRecognizers>
        <DragGestureRecognizer DragStarting="OnDragStarting" />
    </Rectangle.GestureRecognizers>
</Rectangle>
```

In this example, the <xref:Microsoft.Maui.Controls.DragGestureRecognizer> is attached to a <xref:Microsoft.Maui.Controls.Shapes.Rectangle> object. The <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DragStarting> event is raised when a drag gesture is detected on the <xref:Microsoft.Maui.Controls.Shapes.Rectangle>, which executes the `OnDragStarting` event handler:

```csharp
void OnDragStarting(object sender, DragStartingEventArgs e)
{
    Shape shape = (sender as Element).Parent as Shape;
    e.Data.Properties.Add("Square", new Square(shape.Width, shape.Height));
}
```

The <xref:Microsoft.Maui.Controls.DragStartingEventArgs> object that accompanies the <xref:Microsoft.Maui.Controls.DragGestureRecognizer.DragStarting> event has a `Data` property, of type <xref:Microsoft.Maui.Controls.DataPackage>. The `Properties` collection of the <xref:Microsoft.Maui.Controls.DataPackage> object, which is a `Dictionary<string, object>` collection, can be modified to store any required data. In this example, the `Properties` dictionary is modified to store a `Square` object that represents the size of the <xref:Microsoft.Maui.Controls.Shapes.Rectangle> against a "Square" key.

## Enable drop

In .NET MAUI, drop gesture recognition is provided by the <xref:Microsoft.Maui.Controls.DropGestureRecognizer> class. This class defines the following properties:

- <xref:Microsoft.Maui.Controls.DropGestureRecognizer.AllowDrop>, of type `bool`, which indicates whether the element the gesture recognizer is attached to can be a drop target. The default value of this property is `true`.
- <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DragOverCommand>, of type <xref:System.Windows.Input.ICommand>, which is executed when the drag source is dragged over the drop target.
- <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DragOverCommandParameter>, of type `object`, which is the parameter that's passed to the `DragOverCommand`.
- <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DragLeaveCommand>, of type <xref:System.Windows.Input.ICommand>, which is executed when the drag source is dragged off the drop target.
- <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DragLeaveCommandParameter>, of type `object`, which is the parameter that's passed to the `DragLeaveCommand`.
- <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DropCommand>, of type <xref:System.Windows.Input.ICommand>, which is executed when the drag source is dropped over the drop target.
- <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DropCommandParameter>, of type `object`, which is the parameter that's passed to the `DropCommand`.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The <xref:Microsoft.Maui.Controls.DropGestureRecognizer> class also defines <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DragOver>, <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DragLeave>, and <xref:Microsoft.Maui.Controls.DropGestureRecognizer.Drop> events that fire if the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.AllowDrop> property is `true`. When a <xref:Microsoft.Maui.Controls.DropGestureRecognizer> recognizes a drag source over the drop target, it executes the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DragOverCommand> and invokes the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DragOver> event. Then, if the drag source is dragged off the drop target, the <xref:Microsoft.Maui.Controls.DropGestureRecognizer> executes the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DragLeaveCommand> and invokes the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DragLeave> event. Finally, when the <xref:Microsoft.Maui.Controls.DropGestureRecognizer> recognizes a drop gesture over the drop target, it executes the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DropCommand> and invokes the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.Drop> event.

The <xref:Microsoft.Maui.Controls.DragEventArgs> class, which accompanies the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DragOver> and <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DragLeave> events, defines the following properties:

- <xref:Microsoft.Maui.Controls.DragEventArgs.Data>, of type <xref:Microsoft.Maui.Controls.DataPackage>, which contains the data associated with the drag source. This property is read-only.
- <xref:Microsoft.Maui.Controls.DragEventArgs.AcceptedOperation>, of type <xref:Microsoft.Maui.Controls.DataPackageOperation>, which specifies which operations are allowed by the drop target.
- <xref:Microsoft.Maui.Controls.DragEventArgs.PlatformArgs>, of type `PlatformDragEventArgs?`, represents the platform-specific arguments associated with the event.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

On Android, the <xref:Microsoft.Maui.Controls.PlatformDragEventArgs> class defines the following properties:

- `Sender`, of type <xref:Android.Views.View>, represents the native view attached to the event.
- `DragEvent`, of type <xref:Android.Views.DragEvent>, represents the event that's sent at various times during a drag and drop operation.

# [iOS/Mac Catalyst](#tab/macios)

On iOS and Mac Catalyst, the <xref:Microsoft.Maui.Controls.PlatformDragEventArgs> class defines the following properties:

- `Sender`, of type <xref:UIKit.UIView>, represents the native view attached to the event.
- `DropInteraction`, of type <xref:UIKit.UIDropInteraction>, indicates the interaction used for dropping items.
- `DropSession`, of type <xref:UIKit.IUIDropSession>, retrieves the associated information from the drop session.

In addition, on iOS and Mac Catalyst the <xref:Microsoft.Maui.Controls.PlatformDragEventArgs> class defines the `SetDropProposal` method. This method sets the <xref:UIKit.UIDropProposal> to use when dragging an item over a view:

```csharp
void OnDragOver(object sender, DragEventArgs e)
{
#if IOS || MACCATALYST
    e.PlatformArgs.SetDropProposal(new UIKit.UIDropProposal(UIKit.UIDropOperation.Move));
#endif
}
```

In this example, the <xref:UIKit.UIDropOperation> specifies that the data represented by the drag item should be moved rather than copied.

# [Windows](#tab/windows)

On Windows, the <xref:Microsoft.Maui.Controls.PlatformDragEventArgs> class defines the following properties:

- `Sender`, of type <xref:Microsoft.UI.Xaml.FrameworkElement>, represents the native view attached to the event.
- `DragEventArgs`, of type <xref:Microsoft.UI.Xaml.DragEventArgs>, provides event data for the native event.
- `Handled`, of type `bool`, determines if the event arguments have changed. This property should be set to `true` when changing the `DragEventArgs` so that the changes aren't overridden.

For example, the <xref:Microsoft.Maui.Controls.DragEventArgs> property can be used to access native properties:

```csharp
void OnDragOver(object sender, DragEventArgs e)
{
#if WINDOWS
    var dragUI = e.PlatformArgs.DragEventArgs.DragUIOverride;
    dragUI.IsCaptionVisible = false;
    dragUI.IsGlyphVisible = false;
#endif
}
```

In this example, the drag glyph is disabled and caption text that overlays the drag visual is also disabled.

---

<!-- markdownlint-enable MD025 -->

For information about the <xref:Microsoft.Maui.Controls.DataPackageOperation> enumeration, see [Handle the DragOver event](#handle-the-dragover-event).

The <xref:Microsoft.Maui.Controls.DropEventArgs> class that accompanies the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.Drop> event defines the following properties:

- <xref:Microsoft.Maui.Controls.DropEventArgs.Data>, of type <xref:Microsoft.Maui.Controls.DataPackageView>, which is a read-only version of the data package.
- <xref:Microsoft.Maui.Controls.DropEventArgs.Handled>, of type `bool`, indicates whether the event handler has handled the event or whether .NET MAUI should continue its own processing.
- <xref:Microsoft.Maui.Controls.DropEventArgs.PlatformArgs>, of type `PlatformDropEventArgs?`, represents the platform-specific arguments associated with the event.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

On Android, the <xref:Microsoft.Maui.Controls.PlatformDropEventArgs> class defines the following properties:

- `Sender`, of type <xref:Android.Views.View>, represents the native view attached to the event.
- `DragEvent`, of type <xref:Android.Views.DragEvent>, represents the event that's sent at various times during a drag and drop operation.

# [iOS/Mac Catalyst](#tab/macios)

On iOS and Mac Catalyst, the <xref:Microsoft.Maui.Controls.PlatformDropEventArgs> class defines the following properties:

- `Sender`, of type <xref:UIKit.UIView>, represents the native view attached to the event.
- `DropInteraction`, of type <xref:UIKit.UIDropInteraction>, indicates the interaction used for dropping items.
- `DropSession`, of type <xref:UIKit.IUIDropSession>, retrieves the associated information from the drop session.

# [Windows](#tab/windows)

On Windows, the <xref:Microsoft.Maui.Controls.PlatformDropEventArgs> class defines the following properties:

- `Sender`, of type <xref:Microsoft.UI.Xaml.FrameworkElement>, represents the native view attached to the event.
- `DragEventArgs`, of type <xref:Microsoft.UI.Xaml.DragEventArgs>, provides event data for the native event. -->

---

The following XAML example shows a <xref:Microsoft.Maui.Controls.DropGestureRecognizer> attached to an <xref:Microsoft.Maui.Controls.Image>:

```xaml
<Image BackgroundColor="Silver"
       HeightRequest="300"
       WidthRequest="250">
    <Image.GestureRecognizers>
        <DropGestureRecognizer />
    </Image.GestureRecognizers>
</Image>
```

In this example, when a drag source is dropped on the <xref:Microsoft.Maui.Controls.Image> drop target, the drag source will be copied to the drop target if the drag source is an <xref:Microsoft.Maui.Controls.ImageSource>. .NET MAUI automatically copies dragged images, and text, to compatible drop targets.

## Handle the DragOver event

The `DropGestureRecognizer.DragOver` event can be optionally handled to indicate which type of operations are allowed by the drop target. You can indicate the allowable operations by setting the `AcceptedOperation` property, of type <xref:Microsoft.Maui.Controls.DataPackageOperation>, on the <xref:Microsoft.Maui.Controls.DragEventArgs> object that accompanies the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DragOver> event.

The <xref:Microsoft.Maui.Controls.DataPackageOperation> enumeration defines the following members:

- `None`, indicates that no action will be performed.
- `Copy`, indicates that the drag source content will be copied to the drop target.

> [!IMPORTANT]
> When a <xref:Microsoft.Maui.Controls.DragEventArgs> object is created, the `AcceptedOperation` property defaults to `DataPackageOperation.Copy`.

The following XAML example shows a <xref:Microsoft.Maui.Controls.DropGestureRecognizer> that registers a handler for the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DragOver> event:

```xaml
<Image BackgroundColor="Silver"
       HeightRequest="300"
       WidthRequest="250">
    <Image.GestureRecognizers>
        <DropGestureRecognizer DragOver="OnDragOver" />
    </Image.GestureRecognizers>
</Image>
```

In this example, the <xref:Microsoft.Maui.Controls.DropGestureRecognizer> is attached to an <xref:Microsoft.Maui.Controls.Image> object. The <xref:Microsoft.Maui.Controls.DropGestureRecognizer.DragOver> event is raised when a drag source is dragged over the drop target, but hasn't been dropped, which executes the `OnDragOver` event handler:

```csharp
void OnDragOver(object sender, DragEventArgs e)
{
    e.AcceptedOperation = DataPackageOperation.None;
}
```

In this example, the `AcceptedOperation` property of the <xref:Microsoft.Maui.Controls.DragEventArgs> object is set to `DataPackageOperation.None`. This value ensures that no action is taken when a drag source is dropped over the drop target.

## Process the data package

The <xref:Microsoft.Maui.Controls.DropGestureRecognizer.Drop> event is raised when a drag source is released over a drop target. .NET MAUI automatically attempts to retrieve data from the data package when a drag source is dropped onto the following controls:

- Text controls. Text values can be dropped onto <xref:Microsoft.Maui.Controls.CheckBox>, <xref:Microsoft.Maui.Controls.DatePicker>, <xref:Microsoft.Maui.Controls.Editor>, <xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.Label>, <xref:Microsoft.Maui.Controls.RadioButton>, <xref:Microsoft.Maui.Controls.Switch>, and <xref:Microsoft.Maui.Controls.TimePicker> objects.
- Image controls. Images can be dropped onto <xref:Microsoft.Maui.Controls.Button>, <xref:Microsoft.Maui.Controls.Image>, and <xref:Microsoft.Maui.Controls.ImageButton> controls.

The following table shows the properties that are set and any conversion that's attempted when a text-based drag source is dropped on a text control:

| Control | Property | Conversion |
| --- | --- | --- |
| <xref:Microsoft.Maui.Controls.CheckBox> | `IsChecked` | `string` is converted to a `bool`. |
| <xref:Microsoft.Maui.Controls.DatePicker> | `Date` | `string` is converted to a `DateTime`. |
| <xref:Microsoft.Maui.Controls.Editor> | `Text` ||
| <xref:Microsoft.Maui.Controls.Entry> | `Text` ||
| <xref:Microsoft.Maui.Controls.Label> | `Text` ||
| <xref:Microsoft.Maui.Controls.RadioButton> | `IsChecked` | `string` is converted to a `bool`. |
| <xref:Microsoft.Maui.Controls.Switch> | `IsToggled` | `string` is converted to a `bool`. |
| <xref:Microsoft.Maui.Controls.TimePicker> | `Time` | `string` is converted to a `TimeSpan`. |

For content other than text and images, you'll need to process the data package yourself.

The <xref:Microsoft.Maui.Controls.DropEventArgs> class that accompanies the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.Drop> event defines a `Data` property, of type <xref:Microsoft.Maui.Controls.DataPackageView>. This property represents a read-only version of the data package.

### Retrieve image or text data

Image or text data can be retrieved from a data package in the handler for the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.Drop> event, using methods defined in the <xref:Microsoft.Maui.Controls.DataPackageView> class.

The <xref:Microsoft.Maui.Controls.DataPackageView> class includes `GetImageAsync` and `GetTextAsync` methods. The `GetImageAsync` method retrieves an image from the data package that was stored in the `DataPackage.Image` property and returns `Task<ImageSource>`. Similarly, the `GetTextAsync` method retrieves text from the data package that was stored in the `DataPackage.Text` property and returns `Task<string>`.

The following example shows a `Drop` event handler that retrieves text from the data package for a <xref:Microsoft.Maui.Controls.Shapes.Path>:

```csharp
async void OnDrop(object sender, DropEventArgs e)
{
    string text = await e.Data.GetTextAsync();

    // Perform logic to take action based on the text value.
}
```

In this example, text data is retrieved from the data package using the `GetTextAsync` method. An action based on the text value can then be taken.

### Retrieve data from the property bag

Any data can be retrieved from a data package in the handler for the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.Drop> event, by accessing the `Properties` collection of the data package.

The <xref:Microsoft.Maui.Controls.DataPackageView> class defines a `Properties` property, of type `DataPackagePropertySetView`. The `DataPackagePropertySetView` class represents a read-only property bag stored as a `Dictionary<string, object>`.

The following example shows a <xref:Microsoft.Maui.Controls.DropGestureRecognizer.Drop> event handler that retrieves data from the property bag of a data package for a <xref:Microsoft.Maui.Controls.Shapes.Rectangle>:

```csharp
void OnDrop(object sender, DropEventArgs e)
{
    Square square = (Square)e.Data.Properties["Square"];

    // Perform logic to take action based on retrieved value.
}
```

In this example, the `Square` object is retrieved from the property bag of the data package, by specifying the "Square" dictionary key. An action based on the retrieved value can then be taken.

## Drag and drop between applications

On iOS, Mac Catalyst, and Windows, drag can start in one application with the corresponding drop operation ending in a .NET MAUI application. The app from which an item is dragged is the *source* application, and the .NET MAUI app on which an item is dropped is the *destination* application.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

It's not possible to drag from a *source* application to a .NET MAUI *destination* application on Android.

# [iOS/Mac Catalyst](#tab/macios)

On an iPhone you need to drag an item out of a supported application, such as Files or Photos, and then drag the item to the drop target in your application. On an iPad, dragging items between applications can be achieved with apps in Split View.

The following example shows an event handler for the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.Drop?displayProperty=nameWithType> event that processes an item that's been dragged from a *source* application to your .NET MAUI *destination* application:

```csharp
#if IOS || MACCATALYST
using UIKit;
using Foundation;
#endif

async void OnDropGestureRecognizerDrop(object? sender, DropEventArgs e)
{
    var filePaths = new List<string>();

#if IOS || MACCATALYST
    var session = e.PlatformArgs?.DropSession;
    if (session == null)
        return;

    foreach (UIDragItem item in session.Items)
    {
        var result = await LoadItemAsync(item.ItemProvider, item.ItemProvider.RegisteredTypeIdentifiers.ToList());
        if (result is not null)
            filePaths.Add(result.FileUrl?.Path!);
    }
    foreach (var item in filePaths)
    {
        Debug.WriteLine($"Path: {item}");
    }

    static async Task<LoadInPlaceResult?> LoadItemAsync(NSItemProvider itemProvider, List<string> typeIdentifiers)
    {
        if (typeIdentifiers is null || typeIdentifiers.Count == 0)
            return null;

        var typeIdent = typeIdentifiers.First();

        if (itemProvider.HasItemConformingTo(typeIdent))
            return await itemProvider.LoadInPlaceFileRepresentationAsync(typeIdent);

        typeIdentifiers.Remove(typeIdent);
        return await LoadItemAsync(itemProvider, typeIdentifiers);
    }
#endif

    string filePath = filePaths.FirstOrDefault();

    // Process the dropped file
}
```

# [Windows](#tab/windows)

The following example shows an event handler for the <xref:Microsoft.Maui.Controls.DropGestureRecognizer.Drop?displayProperty=nameWithType> event that processes an item that's been dragged from a *source* application to your *destination* application:

```csharp
#if WINDOWS
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
#endif

async void OnDropGestureRecognizerDrop(object? sender, DropEventArgs e)
{
    var filePaths = new List<string>();

#if WINDOWS
    if (e.PlatformArgs is not null && e.PlatformArgs.DragEventArgs.DataView.Contains(StandardDataFormats.StorageItems))
    {
        var items = await e.PlatformArgs.DragEventArgs.DataView.GetStorageItemsAsync();
        if (items.Any())
        {
            foreach (var item in items)
            {
                if (item is StorageFile file)
                    filePaths.Add(item.Path);
            }
        }
    }
#endif

    string filePath = filePaths.FirstOrDefault();

    // Process the dropped file
}
```

---

<!-- markdownlint-enable MD025 -->

## Get the gesture position

The position at which a drag or drop gesture occurred can be obtained by calling the <xref:Microsoft.Maui.Controls.DragEventArgs.GetPosition%2A> method on a <xref:Microsoft.Maui.Controls.DragEventArgs>, <xref:Microsoft.Maui.Controls.DragStartingEventArgs>, or <xref:Microsoft.Maui.Controls.DropEventArgs> object. The <xref:Microsoft.Maui.Controls.DragEventArgs.GetPosition%2A> method accepts an `Element?` argument, and returns a position as a `Point?` object:

```csharp
void OnDragStarting(object sender, DragStartingEventArgs e)
{
    // Position relative to screen
    Point? screenPosition = e.GetPosition(null);

    // Position relative to specified element
    Point? relativeToImagePosition = e.GetPosition(image);
}
```

The `Element?` argument defines the element the position should be obtained relative to. Supplying a `null` value as this argument means that the <xref:Microsoft.Maui.Controls.DragEventArgs.GetPosition%2A> method returns a `Point?` object that defines the position of the drag or drop gesture relative to the screen.
