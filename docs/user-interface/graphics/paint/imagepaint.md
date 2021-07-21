


```csharp
IImage image;
var assembly = GetType().GetTypeInfo().Assembly;
using (var stream = assembly.GetManifestResourceStream("MyMauiApp.Resources.Images.dotnet_bot.png"))
{
    image = GraphicsPlatform.CurrentService.LoadImageFromStream(stream);
}

if (image != null)
{
    IImage newImage = image.Downsize(100, true);
    canvas.SetFillImage(newImage);
    canvas.FillRectangle(20, 20, 380, 400);
}
```

:::image type="content" source="draw-images/filled-image.png" alt-text="Screenshot of a filled image.":::
