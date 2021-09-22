---
title: "Troubleshooting"
description: "Describes how to troubleshoot issues encountered when using .NET MAUI Essentials."
ms.date: 01/06/2020
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Troubleshooting

<!-- TODO Will this still apply with essentials being merged into .NET MAUI? -->

This article describes common errors you may encounter while using .NET MAUI Essentials, and how to fix them.

## Error: Version conflict detected for Xamarin.Android.Support.Compat

The following error may occur when updating NuGet packages (or adding a new package) with a .NET MAUI project that uses Essentials:

```
NU1107: Version conflict detected for Xamarin.Android.Support.Compat. Reference the package directly from the project to resolve this issue.
 MyApp -> Xamarin.Essentials 1.3.1 -> Xamarin.Android.Support.CustomTabs 28.0.0.3 -> Xamarin.Android.Support.Compat (= 28.0.0.3)
 MyApp -> Xamarin.Forms 3.1.0.583944 -> Xamarin.Android.Support.v4 25.4.0.2 -> Xamarin.Android.Support.Compat (= 25.4.0.2).
```

The problem is mismatched dependencies for the two NuGets. This can be resolved by manually adding a specific version of the dependency (in this case **Xamarin.Android.Support.Compat**) that can support both.

To do this, add the NuGet that is the source of the conflict manually, and use the **Version** list to select a specific version. Currently version 28.0.0.3 of the Xamarin.Android.Support.Compat & Xamarin.Android.Support.Core.Util NuGet will resolve this error.

For more information about resolving this problem, see [this blog post](https://redth.codes/how-to-fix-the-dreaded-version-conflict-nuget-error-in-your-xamarin-android-projects/).

## Reporting issues

To report problems with .NET MAUI, file an issue on the [.NET MAUI GitHub repository](https://github.com/dotnet/maui/).
