---
title: "Class modifiers"
description: "The .NET MAUI x:ClassModifier attribute specifies the access level for generated fields for named XAML elements."
ms.date: 10/19/2023
---

# Class modifiers

The .NET Multi-platform App UI (.NET MAUI) `x:ClassModifier` attribute specifies the access level for a generated class in an assembly, provided that `x:Class` is specified on the same XAML element. This enables types to be hidden in libraries, so that they aren't accessible to consumers.

Valid values of the `x:ClassModifier` attribute are:

- `Public`  – specifies that the generated class has no access restrictions.
- `Internal` – specifies that the generated class is accessible only within types in the same assembly.
- `NotPublic` – identical to `Internal`.

The value of the `x:ClassModifier` attribute must align with the access level of the type in its code-behind file.

By default, if the value of the attribute isn't set, the generated class will be `public`.

> [!NOTE]
> The value of the attribute can use any casing, as it will be converted to lowercase by .NET MAUI.

The following XAML shows an example of setting the attribute:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage"
             x:ClassModifier="NotPublic">
    ...
</ContentPage>
```

For information about specifying the access level for XAML fields, see [Field modifiers](field-modifiers.md).
