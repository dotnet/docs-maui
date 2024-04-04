---
title: "Field modifiers"
description: "The .NET MAUI x:FieldModifier attribute specifies the access level for generated fields for named XAML elements."
ms.date: 02/28/2022
---

# Field modifiers

The .NET Multi-platform App UI (.NET MAUI) `x:FieldModifier` attribute specifies the access level for generated fields for named XAML elements.

Valid values of the `x:FieldModifier` attribute are:

- `Private` – specifies that the generated field for the XAML element is accessible only within the body of the class in which it is declared.
- `Public`  – specifies that the generated field for the XAML element has no access restrictions.
- `Protected` – specifies that the generated field for the XAML element is accessible within its class and by derived class instances.
- `Internal` – specifies that the generated field for the XAML element is accessible only within types in the same assembly.
- `NotPublic` – identical to `Internal`.

By default, if the value of the attribute isn't set, the generated field for the element will be `private`.

> [!NOTE]
> The value of the attribute can use any casing, as it will be converted to lowercase by .NET MAUI.

The following conditions must be met for an `x:FieldModifier` attribute to be processed:

- The top-level XAML element must be a valid `x:Class`.
- The current XAML element has an `x:Name` specified.

The following XAML shows examples of setting the attribute:

```xaml
<Label x:Name="privateLabel" />
<Label x:Name="internalLabel" x:FieldModifier="NotPublic" />
<Label x:Name="publicLabel" x:FieldModifier="Public" />
```

::: moniker range="=net-maui-7.0"

> [!IMPORTANT]
> The `x:FieldModifier` attribute cannot be used to specify the access level of a .NET MAUI XAML class.

::: moniker-end

::: moniker range=">=net-maui-8.0"

> [!IMPORTANT]
> The `x:FieldModifier` attribute cannot be used to specify the access level of a .NET MAUI XAML class. For information about specifying the access level of a .NET MAUI XAML class, see [Class modifiers](class-modifiers.md).

::: moniker-end
