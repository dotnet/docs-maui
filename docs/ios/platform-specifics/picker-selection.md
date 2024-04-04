---
title: "Picker item selection on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that controls when item selection occurs in a Picker."
ms.date: 04/05/2022
---

# Picker item selection on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific controls when item selection occurs in a <xref:Microsoft.Maui.Controls.Picker>, allowing the user to specify that item selection occurs when browsing items in the control, or only once the **Done** button is pressed. It's consumed in XAML by setting the `Picker.UpdateMode` attached property to a value of the `UpdateMode` enumeration:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout Margin="20">
        <Picker ... Title="Select a monkey" ios:Picker.UpdateMode="WhenFinished">
          ...
        </Picker>
        ...
    </StackLayout>
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

picker.On<iOS>().SetUpdateMode(UpdateMode.WhenFinished);
```

The `Picker.On<iOS>` method specifies that this platform-specific will only run on iOS. The `Picker.SetUpdateMode` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to control when item selection occurs, with the `UpdateMode` enumeration providing two possible values:

- `Immediately` – item selection occurs as the user browses items in the <xref:Microsoft.Maui.Controls.Picker>. This is the default behavior in .NET MAUI.
- `WhenFinished` – item selection only occurs once the user has pressed the **Done** button in the <xref:Microsoft.Maui.Controls.Picker>.

In addition, the `SetUpdateMode` method can be used to toggle the enumeration values by calling the `UpdateMode` method, which returns the current `UpdateMode`:

```csharp
switch (picker.On<iOS>().UpdateMode())
{
    case UpdateMode.Immediately:
        picker.On<iOS>().SetUpdateMode(UpdateMode.WhenFinished);
        break;
    case UpdateMode.WhenFinished:
        picker.On<iOS>().SetUpdateMode(UpdateMode.Immediately);
        break;
}
```

The result is that a specified `UpdateMode` is applied to the <xref:Microsoft.Maui.Controls.Picker>, which controls when item selection occurs:

:::image type="content" source="media/picker-selection/picker-updatemode.png" alt-text="Picker UpdateMode Platform-Specific.":::
