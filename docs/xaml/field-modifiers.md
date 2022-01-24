---
title: "Field modifiers"
description: "The .NET MAUI x:FieldModifier attribute specifies the access level for generated fields for named XAML elements."
ms.date: 01/24/2022
---

# Field modifiers

The .NET Multi-platform App UI (.NET MAUI) `x:FieldModifier` attribute specifies the access level for generated fields for named XAML elements. Valid values of the attribute are:

- `private` – specifies that the generated field for the XAML element is accessible only within the body of the class in which it is declared.
- `public`  – specifies that the generated field for the XAML element has no access restrictions.
- `protected` – specifies that the generated field for the XAML element is accessible within its class and by derived class instances.
- `internal` – specifies that the generated field for the XAML element is accessible only within types in the same assembly.
- `notpublic` – specifies that the generated field for the XAML element is accessible only within types in the same assembly.

By default, if the value of the attribute isn't set, the generated field for the element will be `private`.

> [!NOTE]
> The value of the attribute can use any casing, as it will be converted to lowercase by .NET MAUI.

The following conditions must be met for an `x:FieldModifier` attribute to be processed:

- The top-level XAML element must be a valid `x:Class`.
- The current XAML element has an `x:Name` specified.

The following XAML shows examples of setting the attribute:

```xaml
<Label x:Name="privateLabel" />
<Label x:Name="internalLabel" x:FieldModifier="internal" />
<Label x:Name="publicLabel" x:FieldModifier="public" />
```

> [!IMPORTANT]
> The `x:FieldModifier` attribute cannot be used to specify the access level of a XAML class.
