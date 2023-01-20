---
title: "Xamarin.iOS csproj conversion"
description: "Describing the changes to the csproj file between Xamarin.iOS and .NET for iOS."
ms.date: 1/20/2023
---

# Xamarin.iOS csproj conversion

This document details how to map items and properties in legacy Xamarin project to .NET projects (if they map to anything at all).

See also:

* [Project-file-properties](https://github.com/xamarin/xamarin-macios/wiki/Project-file-properties)
* [#11863](https://github.com/xamarin/xamarin-macios/issues/11863)

## Properties

| Property | Description | in .NET 6 | Project conversion |
| --------- | ----------- | --------- | -------------------|
| MtouchExtraArgs | Additional arguments to mtouch | Some arguments are still applicable, some arguments are not. | Copy |
| MtouchArch | App architecture(s) | N/A | Convert to RuntimeIdentifier (see below) |
| XamMacArch | App architecture(s) | N/A | Convert to RuntimeIdentifier (see below) |
| [Mtouch]HttpClientHandler | The default HttpClientHandler | UseNativeHttpHandler | Convert to UseNativeHttpHandler  (see below) |
| EnableCodeSigning | If code signing is enabled | - | Copy as-is |
| CodeSigningKey | Specifies the codesigning key | - | Rename to CodesignKey |
| CodesignKey | Specifies the code signing key | - | Copy as-is |
| CodesignProvision | Specifies the provisioning profile | - | Copy as-is |
| CodesignEntitlements | The path to the entitlements | - | Copy as-is |
| CodesignExtraArgs | Extra code signing arguments | - | Copy as-is |
| PackageSigningKey | Specifies the code signing key to sign the package | - | Copy as-is |
| PackagingExtraArgs | Specifies the extra arguments to the packaging tool | - | Copy as-is |
| ProductDefinition | The path to the product definition file to use when packaging | - | Copy as-is |
| MtouchEnableSGenConc | - | - | Rename to EnableSGenConc |
| EnableSGenConc | - | - | Copy as-is |
| ... | - | - | - |

## Items

| Item | Description | in .NET 6 | Project conversion |
| --------- | ----------- | --------- | -------------------|
| LinkDescription | Additional xml files to the managed linker | Same | Copy |
| ... | - | - | - |

## How to convert MtouchArch / XamMacArch to RuntimeIdentifier

### Xamarin.iOS -> iOS

| Value              | RuntimeIdentifier  | RuntimeIdentifiers                |
| ------------------ | ------------------ | --------------------------------- |
| ARMv7              | ios-arm            |   -                               |
| ARMv7s             | ios-arm            |   -                               |
| ARMv7+ARMv7s       | ios-arm            |   -                               |
| ARM64              | ios-arm64          |   -                               |
| ARMv7+ARM64        | -                  | ios-arm,ios-arm64                 |
| ARMv7+ARMv7s+ARM64 | -                  | ios-arm,ios-arm64                 |
| x86_64             | iossimulator-x64   |  -                                |
| i386               | iossimulator-x86   |  -                                |
| x86_64+i386        | -                  | iossimulator-x86,iossimulator-x64 |

### Xamarin.TVOS -> tvOS

| Property | RuntimeIdentifier |
| -------- | ----------------- |
| ARM64    | tvos-arm64        |
| x86_64   | tvossimulator-x64 |

### Xamarin.Mac -> macOS

| Property | RuntimeIdentifier |
| -------- | ----------------- |
| x86_64   | osx-x64           |

## How to convert HttpClientHandler / MtouchHttpClientHandler

Convert to the `UseNativeHttpHandler` property.

| Value              | UseNativeHttpHandler  |
| ------------------ | ------------------ |
| HttpClientHandler |  false |
| NSUrlSessionHandler | [don't set] |
| CFNetworkHandler | [don't set] |

## Info.plist

Some values have moved from the Info.plist to the project file.

### MinimumOSVersion

Equivalent project file property: `SupportedOSPlatformVersion`.

Ref: [#12336](https://github.com/xamarin/xamarin-macios/issues/12336).

### LSMinimumSystemVersion

Equivalent project file property: `SupportedOSPlatformVersion`.

Ref: [#12336](https://github.com/xamarin/xamarin-macios/issues/12336).
