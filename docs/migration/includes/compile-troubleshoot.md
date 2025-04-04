---
ms.topic: include
ms.date: 10/06/2023
---

## Compile and troubleshoot

Once your dependencies are resolved, you should build your project. Any errors will guide you towards next steps.

<!-- markdownlint-disable MD032 -->
> [!TIP]
> - Delete all *bin* and *obj* folders from all projects before opening and building projects in Visual Studio, particularly when changing .NET versions.
> - Delete the *Resource.designer.cs* generated file from the Android project.
<!-- markdownlint-enable MD032 -->

The following table provides guidance for overcoming common build or runtime issues:

| Issue | Tip |
| ----- | --- |
| `Xamarin.*` namespace doesn't exist. | Update the namespace to its .NET MAUI equivalent. For more information, see [Namespace changes](#namespace-changes). |
| API doesn't exist. | Update the API usage to its .NET MAUI equivalent. For more information, see [API changes](#api-changes). |
| App won't deploy. | Ensure that the required platform project is set to deploy in Visual Studio's Configuration Manager. |
| App won't launch. | Update each platform project's entry point class, and the app entry point. For more information, see [Bootstrap your migrated app](../multi-project-to-multi-project.md#bootstrap-your-migrated-app). |
| <xref:Microsoft.Maui.Controls.CollectionView> doesn't scroll. | Check the container layout and the measured size of the <xref:Microsoft.Maui.Controls.CollectionView>. By default the control will take up as much space as the container allows. A <xref:Microsoft.Maui.Controls.Grid> constrains children at its own size. However a <xref:Microsoft.Maui.Controls.StackLayout> enables children to take up space beyond its bounds. |
| Pop-up is displayed under the page on iOS. | In Xamarin.Forms, all pop-ups on iOS are `UIWindow` instances but in .NET MAUI pop-ups are displayed by locating the current presenting `ViewController` and displaying the pop-up with `PresentViewControllerAsync`. In plugins such as Mopups, to ensure that your pop-ups are correctly displayed you should call <xref:Microsoft.Maui.Controls.Page.DisplayAlert%2A> (or <xref:Microsoft.Maui.Controls.Page.DisplayAlertAsync%2A> in .NET 10+), <xref:Microsoft.Maui.Controls.Page.DisplayActionSheet%2A> (or <xref:Microsoft.Maui.Controls.Page.DisplayActionSheetAsync%2A> in .NET 10+), or <xref:Microsoft.Maui.Controls.Page.DisplayPromptAsync%2A> from the <xref:Microsoft.Maui.Controls.ContentPage> that's used inside the `Mopup` popup. |
| <xref:Microsoft.Maui.Controls.BoxView> not appearing. | The default size of a <xref:Microsoft.Maui.Controls.BoxView> in Xamarin.Forms is 40x40. The default size of a <xref:Microsoft.Maui.Controls.BoxView> in .NET MAUI is 0x0. Set `WidthRequest` and `HeightRequest` to 40. |
| Layout is missing padding, margin, or spacing. | Add default values to your project based on the .NET MAUI style resource. For more information, see [Default value changes from Xamarin.Forms](../layouts.md#default-layout-value-changes-from-xamarinforms). |
| Custom layout doesn't work. | Custom layout code needs updating to work in .NET MAUI. For more information, see [Custom layout changes](#custom-layout-changes). |
| Custom renderer doesn't work. | Renderer code needs updating to work in .NET MAUI. For more information, see [Use custom renderers in .NET MAUI](../custom-renderers.md). |
| Effect doesn't work. | Effect code needs updating to work in .NET MAUI. For more information, see [Use effects in .NET MAUI](../effects.md). |
| SkiaSharp code doesn't work. | SkiaSharp code needs minor updates to work in .NET MAUI. For more information, see [Reuse SkiaSharp code in .NET MAUI](../skiasharp.md). |
| Can't access previously created app properties data. | Migrate the app properties data to .NET MAUI preferences. For more information, see [Migrate data from the Xamarin.Forms app properties dictionary to .NET MAUI preferences](../app-properties.md). |
| Can't access previously created secure storage data. | Migrate the secure storage data to .NET MAUI. For more information, see [Migrate from Xamarin.Essentials secure storage to .NET MAUI secure storage](../secure-storage.md). |
| Can't access previously created version tracking data. | Migrate the version tracking data to .NET MAUI. For more information, see [Migrate version tracking data from a Xamarin.Forms app to a .NET MAUI app](../version-tracking.md). |
