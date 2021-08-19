---
ms.topic: include
ms.date: 08/04/2021
---

When requesting a share or opening launcher on iOS, you can present in a popover. This specifies where the popover will appear and point an arrow directly to. This location is often the control that launched the action. You can specify the location using the `PresentationSourceBounds` property:

```csharp
await Share.RequestAsync(new ShareFileRequest
    {
        Title = Title,
        File = new ShareFile(file),
        PresentationSourceBounds = DeviceInfo.Platform== DevicePlatform.iOS && DeviceInfo.Idiom == DeviceIdiom.Tablet
                                ? new System.Drawing.Rectangle(0, 20, 0, 0)
                                : System.Drawing.Rectangle.Empty
    });
```

```csharp
await Launcher.OpenAsync(new OpenFileRequest
    {
        File = new ReadOnlyFile(file),
        PresentationSourceBounds = DeviceInfo.Platform== DevicePlatform.iOS && DeviceInfo.Idiom == DeviceIdiom.Tablet
                                ? new System.Drawing.Rectangle(0, 20, 0, 0)
                                : System.Drawing.Rectangle.Empty
    });
```

<!-- TODO: Is this stuff Apple specific? It seems generic. I know the previous section is because it references iOS, but that's done in this code -->

Everything described here works equally for `Share` and `Launcher`.

Here are some extension methods that help calculate the bounds of a view:

```csharp
public static class ViewHelpers
{
    public static Rectangle GetAbsoluteBounds(this Microsoft.Maui.Controls.View element)
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

        return new Rectangle(absoluteX, absoluteY, element.Width, element.Height);
    }
}
```

This can then be used when calling `RequstAsync`:

```csharp
public Command<Microsoft.Maui.Controls.View> ShareCommand { get; } = new Command<Microsoft.Maui.Controls.View>(Share);

async void Share(Microsoft.Maui.Controls.View element)
{
    try
    {
        await Microsoft.Maui.Essentials.Share.RequestAsync(new ShareTextRequest
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

For an example of the `ViewHelpers` class, see the [.NET MAUI Essentials Samples hosted on GitHub](https://github.com/dotnet/maui/blob/main/src/Essentials/samples/Samples/Helpers/ViewHelpers.cs).
