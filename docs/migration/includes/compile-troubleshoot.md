---
ms.topic: include
ms.date: 08/30/2023
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
| App won't launch. | Update each platform project's entry point class, and the app entry point. For more information, see [Boostrap your migrated app](#bootstrap-your-migrated-app). |
| <xref:Microsoft.Maui.Controls.CollectionView> doesn't scroll. | Check the container layout and the measured size of the <xref:Microsoft.Maui.Controls.CollectionView>. By default the control will take up as much space as the container allows. A <xref:Microsoft.Maui.Controls.Grid> will constrain children at its own size. However a <xref:Microsoft.Maui.Controls.StackLayout> will enable children to take up space beyond its bounds. |
| <xref:Microsoft.Maui.Controls.BoxView> not appearing. | The default size of a <xref:Microsoft.Maui.Controls.BoxView> in Xamarin.Forms is 40x40. The default size of a <xref:Microsoft.Maui.Controls.BoxView> in .NET MAUI is 0x0. Set `WidthRequest` and `HeightRequest` to 40. |
| Layout is missing padding, margin, or spacing. | Add default values to your project based on the .NET MAUI style resource. For more information, see [Default value changes from Xamarin.Forms](layouts.md#default-layout-value-changes-from-xamarinforms). |
| Custom layout doesn't work. | Custom layout code needs updating to work in .NET MAUI. For more information, see [Custom layout changes](#custom-layout-changes). |
| Custom renderer doesn't work. | Renderer code needs updating to work in .NET MAUI. For more information, see [Use custom renderers in .NET MAUI](custom-renderers.md). |
| Effect doesn't work. | Effect code needs updating to work in .NET MAUI. For more information, see [Use effects in .NET MAUI](effects.md). |
