---
title: "Xamarin.Essentials Unit Converters"
description: "The UnitConverters class in Xamarin.Essentials provides several unit converters to help developers when using Xamarin.Essentials."
author: jamesmontemagno
ms.custom: video
ms.author: jamont
ms.date: 01/06/2020
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Unit Converters

The **UnitConverters** class provides several unit converters to help developers when using Xamarin.Essentials.

## Get started

[!include[](~/essentials/includes/get-started.md)]

## Using Unit Converters

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

All unit converters are available by using the static `UnitConverters` class in Xamarin.Essentials. For instance you can easily convert Fahrenheit to Celsius.

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
- [Unit Converters API documentation](xref:Xamarin.Essentials.UnitConverters)

## Related Video

> [!Video https://channel9.msdn.com/Shows/XamarinShow/Unit-Conversion-XamarinEssentials-API-of-the-Week/player]

[!include[](~/essentials/includes/xamarin-show-essentials.md)]
