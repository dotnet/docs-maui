---
title: "DatePicker item selection on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that controls when item selection occurs in a DatePicker."
ms.date: 04/05/2022
---

# DatePicker item selection on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific controls when item selection occurs in a <xref:Microsoft.Maui.Controls.DatePicker>, allowing you to specify that item selection occurs when browsing items in the control, or only once the **Done** button is pressed. It's consumed in XAML by setting the `DatePicker.UpdateMode` attached property to a value of the `UpdateMode` enumeration:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout>
       <DatePicker MinimumDate="01/01/2020"
                   MaximumDate="12/31/2020"
                   ios:DatePicker.UpdateMode="WhenFinished" />
       ...
    </StackLayout>
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

datePicker.On<iOS>().SetUpdateMode(UpdateMode.WhenFinished);
```

The `DatePicker.On<iOS>` method specifies that this platform-specific will only run on iOS. The `DatePicker.SetUpdateMode` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to control when item selection occurs, with the `UpdateMode` enumeration providing two possible values:

- `Immediately` – item selection occurs as the user browses items in the <xref:Microsoft.Maui.Controls.DatePicker>. This is the default behavior.
- `WhenFinished` – item selection only occurs once the user has pressed the **Done** button in the <xref:Microsoft.Maui.Controls.DatePicker>.

In addition, the `SetUpdateMode` method can be used to toggle the enumeration values by calling the `UpdateMode` method, which returns the current `UpdateMode`:

```csharp
switch (datePicker.On<iOS>().UpdateMode())
{
    case UpdateMode.Immediately:
        datePicker.On<iOS>().SetUpdateMode(UpdateMode.WhenFinished);
        break;
    case UpdateMode.WhenFinished:
        datePicker.On<iOS>().SetUpdateMode(UpdateMode.Immediately);
        break;
}
```

The result is that a specified `UpdateMode` is applied to the <xref:Microsoft.Maui.Controls.DatePicker>, which controls when item selection occurs:

:::image type="content" source="media/datepicker-selection/datepicker-updatemode.png" alt-text="Screenshot of DatePicker update modes.":::
