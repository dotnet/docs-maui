---
title: "Unit converters"
description: "Learn how to use the .NET MAUI UnitConverters class, which provides several unit converters to help developers."
ms.date: 09/17/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Unit Converters

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `UnitConverters` class. This class provides several unit converters to help developers convert from one unit of measurement to another.

## Get started

[!INCLUDE [get-started](../essentials/includes/get-started.md)]

[!INCLUDE [essentials-namespace](../essentials/includes/essentials-namespace.md)]

## Using unit converters

All unit converters are available by using the static `Microsoft.Maui.Essentials.UnitConverters` class. For example, you can convert Fahrenheit to Celsius with the `FahrenheitToCelsius` method:

```csharp
var celsius = UnitConverters.FahrenheitToCelsius(32.0);
```

Here is a list of available conversions:

- FahrenheitToCelsius
- CelsiusToFahrenheit
- CelsiusToKelvin
- KelvinToCelsius
- MilesToMeters
- MilesToKilometers
- KilometersToMiles
- MetersToInternationalFeet
- InternationalFeetToMeters
- DegreesToRadians
- RadiansToDegrees
- DegreesPerSecondToRadiansPerSecond
- RadiansPerSecondToDegreesPerSecond
- DegreesPerSecondToHertz
- RadiansPerSecondToHertz
- HertzToDegreesPerSecond
- HertzToRadiansPerSecond
- KilopascalsToHectopascals
- HectopascalsToKilopascals
- KilopascalsToPascals
- HectopascalsToPascals
- AtmospheresToPascals
- PascalsToAtmospheres
- CoordinatesToMiles
- CoordinatesToKilometers
- KilogramsToPounds
- PoundsToKilograms
- StonesToPounds
- PoundsToStones

## API

- [Unit Converters source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/UnitConverters.shared.cs)
<!-- - [Unit Converters API documentation](xref:Microsoft.Maui.Essentials.UnitConverters)-->
