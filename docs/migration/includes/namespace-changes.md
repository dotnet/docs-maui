---
ms.topic: include
ms.date: 08/30/2023
---

## Namespace changes

Namespaces have changed in the move from Xamarin.Forms to .NET MAUI, and Xamarin.Essentials features are now part of .NET MAUI. To make namespace updates, perform a find and replace for the following namespaces:

> [!div class="mx-tdBreakAll"]
> | Xamarin.Forms namespace | .NET MAUI namespace(s) |
> | --- | --- |
> | <xref:Xamarin.Forms> | <xref:Microsoft.Maui> and <xref:Microsoft.Maui.Controls> |
> | <xref:Xamarin.Forms.DualScreen> | <xref:Microsoft.Maui.Controls.Foldable> |
> | <xref:Xamarin.Forms.Maps> | <xref:Microsoft.Maui.Controls.Maps> and <xref:Microsoft.Maui.Maps> |
> | <xref:Xamarin.Forms.PlatformConfiguration> | <xref:Microsoft.Maui.Controls.PlatformConfiguration> |
> | <xref:Xamarin.Forms.PlatformConfiguration.AndroidSpecific> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific> |
> | <xref:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.AppCompat> |
> | <xref:Xamarin.Forms.PlatformConfiguration.TizenSpecific> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.TizenSpecific> |
> | <xref:Xamarin.Forms.PlatformConfiguration.WindowsSpecific> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific> |
> | <xref:Xamarin.Forms.PlatformConfiguration.iOSSpecific> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific> |
> | <xref:Xamarin.Forms.Shapes> | <xref:Microsoft.Maui.Controls.Shapes> |
> | <xref:Xamarin.Forms.StyleSheets> | <xref:Microsoft.Maui.Controls.StyleSheets> |
> | <xref:Xamarin.Forms.Xaml> | <xref:Microsoft.Maui.Controls.Xaml> |

.NET MAUI projects make use of implicit `global using` directives. This feature enables you to remove `using` directives for the `Xamarin.Essentials` namespace, without having to replace them with the equivalent .NET MAUI namespaces.

In addition, the default XAML namespace has changed from `http://xamarin.com/schemas/2014/forms` in Xamarin.Forms to `http://schemas.microsoft.com/dotnet/2021/maui` in .NET MAUI. Therefore, you should replace all occurrences of `xmlns="http://xamarin.com/schemas/2014/forms"` with `xmlns="http://schemas.microsoft.com/dotnet/2021/maui"`.

> [!NOTE]
> You can quickly update your `Xamarin.Forms` namespaces to `Microsoft.Maui` by using [Quick actions in Visual Studio](upgrade-assistant.md#quick-actions-in-visual-studio), provided that you have [Upgrade Assistant](../upgrade-assistant.md) installed.
