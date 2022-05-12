---
title: "Unit converters"
description: "Learn how to use the .NET MAUI UnitConverters class, which provides several unit converters to help developers."
ms.date: 05/12/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Media", "UnitConverters"]
---

# Unit Converters

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `UnitConverters` class. This class provides several unit converters to help developers convert from one unit of measurement to another.

## Using unit converters

All unit converters are available by using the static `Microsoft.Maui.Media.UnitConverters` class. For example, you can convert Fahrenheit to Celsius with the `FahrenheitToCelsius` method:

```csharp
var celsius = UnitConverters.FahrenheitToCelsius(32.0);
```

Here is a list of available conversions:

- `FahrenheitToCelsius`
- `CelsiusToFahrenheit`
- `CelsiusToKelvin`
- `KelvinToCelsius`
- `MilesToMeters`
- `MilesToKilometers`
- `KilometersToMiles`
- `MetersToInternationalFeet`
- `InternationalFeetToMeters`
- `DegreesToRadians`
- `RadiansToDegrees`
- `DegreesPerSecondToRadiansPerSecond`
- `RadiansPerSecondToDegreesPerSecond`
- `DegreesPerSecondToHertz`
- `RadiansPerSecondToHertz`
- `HertzToDegreesPerSecond`
- `HertzToRadiansPerSecond`
- `KilopascalsToHectopascals`
- `HectopascalsToKilopascals`
- `KilopascalsToPascals`
- `HectopascalsToPascals`
- `AtmospheresToPascals`
- `PascalsToAtmospheres`
- `CoordinatesToMiles`
- `CoordinatesToKilometers`
- `KilogramsToPounds`
- `PoundsToKilograms`
- `StonesToPounds`
- `PoundsToStones`
