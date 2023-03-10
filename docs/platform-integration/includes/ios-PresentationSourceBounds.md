---
ms.topic: include
ms.date: 02/02/2023
---

> [!IMPORTANT]
> This section only applies to iPadOS.

When requesting a share or opening launcher on iPadOS, you can present it in a popover. This specifies where the popover will appear and point an arrow directly to. This location is often the control that launched the action. You can specify the location using the <xref:Microsoft.Maui.ApplicationModel.OpenFileRequest.PresentationSourceBounds> property:

```csharp
await Share.RequestAsync(new ShareFileRequest
    {
        Title = Title,
        File = new ShareFile(file),
        PresentationSourceBounds = DeviceInfo.Platform == DevicePlatform.iOS && DeviceInfo.Idiom == DeviceIdiom.Tablet
                                ? new Rect(0, 20, 0, 0)
                                : Rect.Zero
    });
```

```csharp
await Launcher.OpenAsync(new OpenFileRequest
    {
        File = new ReadOnlyFile(file),
        PresentationSourceBounds = DeviceInfo.Platform == DevicePlatform.iOS && DeviceInfo.Idiom == DeviceIdiom.Tablet
                                ? new Rect(0, 20, 0, 0)
                                : Rect.Zero
    });
```

Everything described here works equally for <xref:Microsoft.Maui.ApplicationModel.DataTransfer.Share> and <xref:Microsoft.Maui.ApplicationModel.Launcher>.

Here is an extension method that helps calculate the bounds of a view:

```csharp
public static class ViewHelpers
{
    public static Rect GetAbsoluteBounds(this Microsoft.Maui.Controls.View element)
    {
        Element looper = element;

        var absoluteX = element.X + element.Margin.Top;
        var absoluteY = element.Y + element.Margin.Left;

        // Add logic to handle titles, headers, or other non-view bars

        while (looper.Parent != null)
        {
            looper = looper.Parent;
            if (looper is Microsoft.Maui.Controls.View v)
            {
                absoluteX += v.X + v.Margin.Top;
                absoluteY += v.Y + v.Margin.Left;
            }
        }

        return new Rect(absoluteX, absoluteY, element.Width, element.Height);
    }
}
```

This can then be used when calling <xref:Microsoft.Maui.ApplicationModel.DataTransfer.IShare.RequestAsync%2A>:

```csharp
public Command<Microsoft.Maui.Controls.View> ShareCommand { get; } = new Command<Microsoft.Maui.Controls.View>(Share);

async void Share(Microsoft.Maui.Controls.View element)
{
    try
    {
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            PresentationSourceBounds = element.GetAbsoluteBounds(),
            Title = "Title",
            Text = "Text"
        });
    }
    catch (Exception)
    {
        // Handle exception that share failed
    }
}
```

You can pass in the calling element when the `Command` is triggered:

```xml
<Button Text="Share"
        Command="{Binding ShareWithFriendsCommand}"
        CommandParameter="{Binding Source={RelativeSource Self}}"/>
```

For an example of the `ViewHelpers` class, see the [.NET MAUI Sample hosted on GitHub](https://github.com/dotnet/maui/blob/main/src/Essentials/samples/Samples/Helpers/ViewHelpers.cs).
