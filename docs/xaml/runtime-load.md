---
title: "Load XAML at runtime"
description: "In .NET MAUI, XAML can be loaded and parsed at runtime with the LoadFromXaml extension methods."
ms.date: 10/08/2024
---

# Load XAML at runtime

When a .NET Multi-platform App UI (.NET MAUI) XAML class is constructed, a <xref:Microsoft.Maui.Controls.Xaml.Extensions.LoadFromXaml%2A> method is indirectly called. This occurs because the code-behind file for a XAML class calls the `InitializeComponent` method from its constructor:

```csharp
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
}
```

When a project containing a XAML file is built, a source generator generates new C# source that contains the definition of the `InitializeComponent` method and adds it to the compilation object. The following example shows the generated `InitializeComponent` method for the `MainPage` class:

```csharp
private void InitializeComponent()
{
    global::Microsoft.Maui.Controls.Xaml.Extensions.LoadFromXaml(this, typeof(MainPage));
    ...
}
```

The `InitializeComponent` method calls the <xref:Microsoft.Maui.Controls.Xaml.Extensions.LoadFromXaml%2A> method to extract the XAML compiled binary (or its file) from the app package. After extraction, it initializes all of the objects defined in the XAML, connects them all together in parent-child relationships, attaches event handlers defined in code to events set in the XAML file, and sets the resultant tree of objects as the content of the page.

## Load XAML at runtime

The `Extensions` class, in the `Microsoft.Maui.Controls.Xaml` namespace, includes <xref:Microsoft.Maui.Controls.Xaml.Extensions.LoadFromXaml%2A> extension methods that can be used to load and parse XAML at runtime. The <xref:Microsoft.Maui.Controls.Xaml.Extensions.LoadFromXaml%2A> methods are `public`, and therefore can be called from .NET MAUI applications to load, and parse XAML at runtime. This enables scenarios such as an app downloading XAML from a web service, creating the required view from the XAML, and displaying it in the app.

::: moniker range="=net-maui-8.0"

> [!WARNING]
> Loading XAML at runtime has a significant performance cost, and generally should be avoided.

::: moniker-end

::: moniker range=">=net-maui-9.0"

> [!WARNING]
> Loading XAML at runtime isn't trim safe and shouldn't be used with full trimming or NativeAOT. It can be made trim safe by annotating all types that could be loaded with the [`DynamicallyAccessedMembers`](xref:System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute) attribute or the [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute) attribute. However, this is very error prone and isn't recommended. In addition, loading XAML at runtime has a significant performance cost.

::: moniker-end

The following code example shows a simple usage:

```csharp
string navigationButtonXAML = "<Button Text=\"Navigate\" />";
Button navigationButton = new Button().LoadFromXaml(navigationButtonXAML);
...
stackLayout.Add(navigationButton);
```

In this example, a <xref:Microsoft.Maui.Controls.Button> instance is created, with its `Text` property value being set from the XAML defined in the `string`. The <xref:Microsoft.Maui.Controls.Button> is then added to a <xref:Microsoft.Maui.Controls.StackLayout> that has been defined in the XAML for the page.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.Xaml.Extensions.LoadFromXaml%2A> extension methods allow a generic type argument to be specified. However, it's rarely necessary to specify the type argument, as it will be inferred from the type of the instance it's operating on.

The <xref:Microsoft.Maui.Controls.Xaml.Extensions.LoadFromXaml%2A> method can be used to inflate any XAML, with the following example inflating a <xref:Microsoft.Maui.Controls.ContentPage> and then navigating to it:

```csharp
// See the sample for the full XAML string
string pageXAML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<ContentPage xmlns=\"http://schemas.microsoft.com/dotnet/2021/maui\"\nxmlns:x=\"http://schemas.microsoft.com/winfx/2009/xaml\"\nx:Class=\"LoadRuntimeXAML.CatalogItemsPage\"\nTitle=\"Catalog Items\">\n</ContentPage>";

ContentPage page = new ContentPage().LoadFromXaml(pageXAML);
await Navigation.PushAsync(page);
```

## Access elements

Loading XAML at runtime with the <xref:Microsoft.Maui.Controls.Xaml.Extensions.LoadFromXaml%2A> method does not allow strongly-typed access to the XAML elements that have specified runtime object names (using `x:Name`). However, these XAML elements can be retrieved using the `FindByName` method, and then accessed as required:

```csharp
// See the sample for the full XAML string
string pageXAML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<ContentPage xmlns=\"http://schemas.microsoft.com/dotnet/2021/maui\"\nxmlns:x=\"http://schemas.microsoft.com/winfx/2009/xaml\"\nx:Class=\"LoadRuntimeXAML.CatalogItemsPage\"\nTitle=\"Catalog Items\">\n<StackLayout>\n<Label x:Name=\"monkeyName\"\n />\n</StackLayout>\n</ContentPage>";
ContentPage page = new ContentPage().LoadFromXaml(pageXAML);

Label monkeyLabel = page.FindByName<Label>("monkeyName");
monkeyLabel.Text = "Seated Monkey";
```

In this example, the XAML for a <xref:Microsoft.Maui.Controls.ContentPage> is inflated. This XAML includes a <xref:Microsoft.Maui.Controls.Label> named `monkeyName`, which is retrieved using the `FindByName` method, before its `Text` property is set.
