---
title: "Xamarin.iOS project migration"
description: "Learn how to map properties and items in legacy Xamarin Apple projects to .NET projects."
ms.date: 1/31/2023
---

# Xamarin Apple project migration

This article details how to map properties and items in legacy Xamarin Apple projects to .NET projects.

## Properties

| Property | Description | .NET 6 | Project conversion |
| --------- | ----------- | ------ | -------------------|
| MtouchExtraArgs | Additional arguments to mtouch | Some arguments are still applicable, some arguments are not. | Copy |
| MtouchArch | App architecture(s) | N/A | Convert to RuntimeIdentifier. For more information, see [Convert to RuntimeIdentifier](#convert-to-runtimeidentifier). |
| XamMacArch | App architecture(s) | N/A | Convert to RuntimeIdentifier. For more information, see [Convert to RuntimeIdentifier](#convert-to-runtimeidentifier). |
| HttpClientHandler | The default HttpClientHandler | UseNativeHttpHandler | Convert to UseNativeHttpHandler. For more information, see [Convert to UseNativeHttpHandler](#convert-to-usenativehttphandler). |
| MtouchHttpClientHandler | The default MtouchHttpClientHandler | UseNativeHttpHandler | Convert to UseNativeHttpHandler. For more information, see [Convert to UseNativeHttpHandler](#convert-to-usenativehttphandler). |
| EnableCodeSigning | If code signing is enabled | | Copy |
| CodeSigningKey | Specifies the codesigning key | | Rename to CodesignKey |
| CodesignKey | Specifies the code signing key | | Copy |
| CodesignProvision | Specifies the provisioning profile | | Copy |
| CodesignEntitlements | The path to the entitlements | | Copy |
| CodesignExtraArgs | Extra code signing arguments | | Copy |
| PackageSigningKey | Specifies the code signing key to sign the package | | Copy |
| PackagingExtraArgs | Specifies the extra arguments to the packaging tool | | Copy |
| ProductDefinition | The path to the product definition file to use when packaging | | Copy |
| MtouchEnableSGenConc | | | Rename to EnableSGenConc |
| EnableSGenConc | | | Copy as-is |

## Items

| Item | Description | .NET 6 | Project conversion |
| --------- | ----------- | ------ | -------------------|
| LinkDescription | Additional XML files to the managed linker | Identical | Copy |

## Convert to RuntimeIdentifier

The following table shows how to convert the MtouchArch and XamMacArch properties to the RuntimeIdentifier property when migrating a Xamarin.iOS project to .NET 6+:

| Value              | RuntimeIdentifier  | RuntimeIdentifiers                |
| ------------------ | ------------------ | --------------------------------- |
| ARMv7              | ios-arm            |                                   |
| ARMv7s             | ios-arm            |                                   |
| ARMv7+ARMv7s       | ios-arm            |                                   |
| ARM64              | ios-arm64          |                                   |
| ARMv7+ARM64        |                    | ios-arm,ios-arm64                 |
| ARMv7+ARMv7s+ARM64 |                    | ios-arm,ios-arm64                 |
| x86_64             | iossimulator-x64   |                                   |
| i386               | iossimulator-x86   |                                   |
| x86_64+i386        |                    | iossimulator-x86,iossimulator-x64 |

The following table shows how to convert the MtouchArch and XamMacArch properties to the RuntimeIdentifier property when migrating a Xamarin.tvOS project to .NET 6+:

| Property | RuntimeIdentifier |
| -------- | ----------------- |
| ARM64    | tvos-arm64        |
| x86_64   | tvossimulator-x64 |

The following table shows how to convert the MtouchArch and XamMacArch properties to the RuntimeIdentifier property when migrating a Xamarin.Mac project to .NET 6+:

| Property | RuntimeIdentifier |
| -------- | ----------------- |
| x86_64   | osx-x64           |

For more information about the RuntimeIdentifier property, see [RuntimeIdentifier](/dotnet/core/project-sdk/msbuild-props#runtimeidentifier). For more information about runtime identifiers, see [.NET RID Catalog](/dotnet/core/rid-catalog).

## Convert to UseNativeHttpHandler

The following table shows how to convert the HttpClientHandler and MtouchHttpClientHandler properties to the UseNativeHttpHandler property when migrating a Xamarin Apple project to .NET 6+:

| Value              | UseNativeHttpHandler  |
| ------------------ | ------------------ |
| HttpClientHandler | false |
| NSUrlSessionHandler | *don't set* |
| CFNetworkHandler | *don't set* |

## Info.plist

Some values have moved from **Info.plist** to the project file.

## MinimumOSVersion and LSMinimumSystemVersion

The MinimumOSVersion and LSMinimumSystemVersion properties should be converted to the SupportedOSPlatformVersion project in .NET 6+ projects. For more information, see [Ensure MinimumOSVersion is consistent with SupportedOSPlatformVersion](https://github.com/xamarin/xamarin-macios/issues/12336).

## See also

- [Project file properties](https://github.com/xamarin/xamarin-macios/wiki/Project-file-properties)
