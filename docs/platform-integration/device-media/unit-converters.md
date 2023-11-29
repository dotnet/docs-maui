---
title: "Unit converters"
description: "Learn how to use the .NET MAUI UnitConverters class, which provides several unit converters to help developers."
ms.date: 02/02/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Media", "UnitConverters"]
---

# Unit converters

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Media.UnitConverters> class. This class provides several unit converters to help developers convert from one unit of measurement to another.

## Using unit converters

All unit converters are available by using the static `Microsoft.Maui.Media.UnitConverters` class. For example, you can convert Fahrenheit to Celsius with the <xref:Microsoft.Maui.Media.UnitConverters.FahrenheitToCelsius%2A> method:

```csharp
var celsius = UnitConverters.FahrenheitToCelsius(32.0);
```

Here is a list of available conversions:

- <xref:Microsoft.Maui.Media.UnitConverters.FahrenheitToCelsius%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.CelsiusToFahrenheit%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.CelsiusToKelvin%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.KelvinToCelsius%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.MilesToMeters%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.MilesToKilometers%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.KilometersToMiles%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.MetersToInternationalFeet%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.InternationalFeetToMeters%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.DegreesToRadians%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.RadiansToDegrees%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.DegreesPerSecondToRadiansPerSecond%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.RadiansPerSecondToDegreesPerSecond%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.DegreesPerSecondToHertz%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.RadiansPerSecondToHertz%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.HertzToDegreesPerSecond%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.HertzToRadiansPerSecond%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.KilopascalsToHectopascals%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.HectopascalsToKilopascals%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.KilopascalsToPascals%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.HectopascalsToPascals%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.AtmospheresToPascals%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.PascalsToAtmospheres%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.CoordinatesToMiles%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.CoordinatesToKilometers%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.KilogramsToPounds%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.PoundsToKilograms%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.StonesToPounds%2A>
- <xref:Microsoft.Maui.Media.UnitConverters.PoundsToStones%2A>
