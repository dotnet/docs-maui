---
title: "Image"
description: "The .NET MAUI Image displays an image that can be loaded from a local file, a URI, an embedded resource, or a stream."
ms.date: 03/04/2022
---

# Image

The .NET Multi-platform App UI (.NET MAUI) `Image` displays an image that can be loaded from a local file, a URI, an embedded resource, or a stream. The standard platform image formats are supported, including animated GIFs, and local Scalable Vector Graphics (SVG) files are also supported.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

`Image` defines the following properties:

- `Aspect`, of type `Aspect`, defines the scaling mode of the image.
- `IsAnimationPlaying`, of type `bool`, determines whether an animated GIF is playing or stopped. The default value of this property is `false`.
- `IsLoading`, of type `bool`, indicates the loading status of the image. The default value of this property is `false`.
- `IsOpaque`, of type `bool`, indicates whether the rendering engine may treat the image as opaque while rendering it. The default value of this property is `false`.
- `Source`, of type `ImageSource`, specifies the source of the image.

These properties are backed by `BindableProperty` objects, which means that they can be styled, and be the target of data bindings.

> [!NOTE]
> Font icons can be displayed by an `Image` by specifying the font icon data as a `FontImageSource` object. <!-- For more information, see [Display font icons](~/user-interface/fonts.md#display-font-icons).-->

The `ImageSource` class defines the following methods that can be used to load an image from different sources:

- `FromFile` returns a `FileImageSource` that reads an image from a local file.
- `FromUri` returns an `UriImageSource` that downloads and reads an image from a specified URI.
- `FromResource` returns a `StreamImageSource` that reads an image file embedded in an assembly.
- `FromStream` returns a `StreamImageSource` that reads an image from a stream that supplies image data.

In XAML, images can be loaded from files and URIs by specifying the filename or URI as a string value for the `Source` property. Images can also be loaded from resources or streams in XAML through custom markup extensions.

> [!IMPORTANT]
> Images will be displayed at their full resolution unless the size of the `Image` is constrained by its layout, or the `HeightRequest` or `WidthRequest` property of the `Image` is specified.

For information about adding app icons and a splash screen to your app, see [App icons](~/user-interface/images/app-icons.md) and [Splash screen](~/user-interface/images/splashscreen.md).

## Load a local image

Images can be added to your app project by dragging them to the _Resources\Images_ folder of your project, where its build action will automatically be set to **MauiImage**. At build time, vector images are resized to the correct resolutions for the target platform and device, and added to your app package. This is necessary because different platforms support different image resolutions, and the operating system chooses the appropriate image resolution at runtime based on the device's capabilities.

To comply with Android resource naming rules, all local image filenames must be lowercase, start and end with a letter character, and contain only alphanumeric characters or underscores. For more information, see [App resources overview](https://developer.android.com/guide/topics/resources/providing-resources) on developer.android.com.

> [!IMPORTANT]
> .NET MAUI converts SVG files to PNG files. Therefore, when adding an SVG file to your .NET MAUI app project, it should be referenced from XAML or C# with a .png extension.

Adhering to these rules for file naming and placement enables the following XAML to load and display an image:

```xaml
<Image Source="dotnet_bot.png" />
```

The equivalent C# code is:

```csharp
Image image = new Image
{
    Source = ImageSource.FromFile("dotnet_bot.png")
};
```

The `ImageSource.FromFile` method requires a `string` argument, and returns a new `FileImageSource` object that reads the image from the file. There's also an implicit conversion operator that enables the filename to be specified as a `string` argument to the `Image.Source` property:

```csharp
Image image = new Image { Source = "dotnet_bot.png" };
```

## Load a remote image

Remote images can be downloaded and displayed by specifying a URI as the value of the `Source` property:

```xaml
<Image Source="https://aka.ms/campus.jpg" />
```

The equivalent C# code is:

```csharp
Image image = new Image
{
    Source = ImageSource.FromUri(new Uri("https://aka.ms/campus.jpg"))
};
```

The `ImageSource.FromUri` method requires a `Uri` argument, and returns a new `UriImageSource` object that reads the image from the `Uri`. There's also an implicit conversion for string-based URIs:

```csharp
Image image = new Image { Source = "https://aka.ms/campus.jpg" };
```

### Image caching

Caching of downloaded images is enabled by default, with cached images being stored for 1 day. This behavior can be changed by setting properties of the `UriImageSource` class.

The `UriImageSource` class defines the following properties:

- `Uri`, of type `Uri`, represents the URI of the image to be downloaded for display.
- `CacheValidity`, of type `TimeSpan`, specifies how long the image will be stored locally for. The default value of this property is 1 day.
- `CachingEnabled`, of type `bool`, defines whether image caching is enabled. The default value of this property is `true`.

These properties are backed by `BindableProperty` objects, which means that they can be styled, and be the target of data bindings.

To set a specific cache period, set the `Source` property to an `UriImageSource` object that sets its `CacheValidity` property:

```xaml
<Image>
    <Image.Source>
        <UriImageSource Uri="https://aka.ms/campus.jpg"
                        CacheValidity="10:00:00.0" />
    </Image.Source>
</Image>
```

The equivalent C# code is:

```csharp
Image image = new Image();
image.Source = new UriImageSource
{
    Uri = new Uri("https://aka.ms/campus.jpg"),
    CacheValidity = new TimeSpan(10,0,0,0)
};
```

In this example, the caching period is set to 10 days.

## Load an embedded image

Embedded images can be added to an assembly as a resource by dragging them into your project, and ensuring their build action is set to **Embedded resource** in the **Properties window**.

Embedded images are loaded based on their resource ID, which is compromised of the name of the project and its location in the project. For example, placing *dotnet_bot.png* in the root folder of a project named *MyProject* will result in a resource ID of *MyProject.dotnet_bot.png*. Similarly, placing *dotnet_bot.png* in the *Assets* folder of a project named *MyProject* will result in a resource ID of *MyProject.Assets.dotnet_bot.png*.

The `ImageSource.FromResource` method can be used to load an image that's embedded into an assembly as a resource:

```csharp
Image image = new Image
{
    Source = ImageSource.FromResource("MyProject.Assets.dotnet_bot.png")
};
```

By default, the `ImageSource.FromResource` method only looks for images in the same assembly as the calling code. However, the assembly containing the embedded image can be specified as the second argument to the `ImageSource.FromResource` method:

```csharp
Image image = new Image
{
    Source = ImageSource.FromResource("MyLibrary.MyFolder.myimage.png", typeof(MyLibrary.MyClass).GetTypeInfo().Assembly)
};
```

<!--
> [!NOTE]
> To support displaying embedded images in release mode on Windows, it's necessary to use the overload of `ImageSource.FromResource` that specifies the source assembly in which to search for the image.
-->

### Load an embedded image in XAML

Embedded images can be loaded in XAML with a custom XAML markup extension:

```csharp
using System.Reflection;
using System.Xml;

namespace ImageDemos
{
    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension<ImageSource>
    {
        public string Source { set; get; }

        public ImageSource ProvideValue(IServiceProvider serviceProvider)
        {
            if (String.IsNullOrEmpty(Source))
            {
                IXmlLineInfoProvider lineInfoProvider = serviceProvider.GetService(typeof(IXmlLineInfoProvider)) as IXmlLineInfoProvider;
                IXmlLineInfo lineInfo = (lineInfoProvider != null) ? lineInfoProvider.XmlLineInfo : new XmlLineInfo();
                throw new XamlParseException("ImageResourceExtension requires Source property to be set", lineInfo);
            }

            string assemblyName = GetType().GetTypeInfo().Assembly.GetName().Name;
            return ImageSource.FromResource(assemblyName + "." + Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<ImageSource>).ProvideValue(serviceProvider);
        }
    }
}
```

The `ImageResourceExtension` class is used to access an embedded image in XAML, and uses the `Source` property to call the `ImageSource.FromResource` method. The argument to the `ProvideValue` method is an `IServiceProvider` object that can be used to obtain an `IXmlLineInfoProvider` object that can provide line and character information indicating where an error has been detected. This object is used to raise an exception when the `Image.Source` property hasn't been set.

The markup extension can be consumed in XAML to load an embedded image:

```xaml
<ContentPage ...
             xmlns:local="clr-namespace:ImageDemos">
    <StackLayout>
        <Image Source="{local:ImageResource monkey.png}"
               HeightRequest="100" />
    </StackLayout>
</ContentPage>
```

For more information about XAML markup extensions, see [Create XAML markup extensions](~/xaml/markup-extensions/create.md).

## Load an image from a stream

Images can be loaded from streams with the `ImageSource.FromStream` method:

```csharp
Image image = new Image
{
    Source = ImageSource.FromStream(() => stream)
};
```

## Load animated GIFs

.NET MAUI includes support for displaying small, animated GIFs. This is accomplished by setting the `Source` property to an animated GIF file:

```xaml
<Image Source="demo.gif" />
```

> [!IMPORTANT]
> While the animated GIF support in .NET MAUI includes the ability to download files, it does not support caching or streaming animated GIFs.

By default, when an animated GIF is loaded it will not be played. This is because the `IsAnimationPlaying` property, that controls whether an animated GIF is playing or stopped, has a default value of `false`. Therefore, when an animated GIF is loaded it will not be played until the `IsAnimationPlaying` property is set to `true`. Playback can be stopped by reseting the `IsAnimationPlaying` property to `false`. Note that this property has no effect when displaying a non-GIF image source.

## Control image scaling

The `Aspect` property determines how the image will be scaled to fit the display area, and should be set to one of the members of the `Aspect` enumeration:

- `AspectFit` - letterboxes the image (if required) so that the entire image fits into the display area, with blank space added to the top/bottom or sides depending on whether the image is wide or tall.
- `AspectFill` - clips the image so that it fills the display area while preserving the aspect ratio.
- `Fill` - stretches the image to completely and exactly fill the display area. This may result in the image being distorted.
- `Center` - centers the image in the display area while preserving the aspect ratio.
