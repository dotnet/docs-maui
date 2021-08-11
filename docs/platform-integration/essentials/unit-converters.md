---
title: "Xamarin.Essentials Unit Converters"
description: "The UnitConverters class in Microsoft.Maui.Essentials provides several unit converters to help developers when using Xamarin.Essentials."
ms.date: 01/06/2020
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Unit Converters

The `UnitConverters` class provides several unit converters to help developers when using Xamarin.Essentials.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using Unit Converters

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

All unit converters are available by using the static `UnitConverters` class in Microsoft.Maui.Essentials. For instance you can easily convert Fahrenheit to Celsius.

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

- [Unit Converters source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Types/UnitConverters.shared.cs)
<!-- - [Unit Converters API documentation](xref:Microsoft.Maui.Essentials.UnitConverters)-->
