---
title: "Xamarin.iOS project migration"
description: "Learn how to migrate a Xamarin.iOS, Xamarin.Mac, or Xamarin.tvOS project to a .NET project."
ms.date: 02/15/2023
---

# Xamarin Apple project migration

A .NET 8 project for a .NET for iOS app is similar to the following example:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0-ios</TargetFramework>
    <OutputType>Exe</OutputType>
    <Nullable>enable</Nullable>
    <ImplicitUsings>true</ImplicitUsings>
    <SupportedOSPlatformVersion>13.0</SupportedOSPlatformVersion>
  </PropertyGroup>
</Project>
```

For a library project, omit the `$(OutputType)` property completely or specify `Library` as the property value.

## Changes to MSBuild properties

The following table shows how to map properties in Xamarin Apple projects to .NET projects:

| Property | Description | .NET | Project conversion |
| -------- | ----------- | ---- | ------------------ |
| `MtouchExtraArgs` | Additional arguments to mtouch. | Some arguments are still applicable, some arguments aren't. | Copy |
| `MtouchArch` | App architecture(s). | N/A | Convert to `RuntimeIdentifier`. For more information, see [Convert to RuntimeIdentifier](#convert-to-runtimeidentifier). |
| `XamMacArch` | App architecture(s). | N/A | Convert to `RuntimeIdentifier`. For more information, see [Convert to RuntimeIdentifier](#convert-to-runtimeidentifier). |
| `HttpClientHandler` | The default `HttpClientHandler`. | `UseNativeHttpHandler` | Convert to `UseNativeHttpHandler`. For more information, see [Convert to UseNativeHttpHandler](#convert-to-usenativehttphandler). |
| `MtouchHttpClientHandler` | The default `MtouchHttpClientHandler.` | `UseNativeHttpHandler` | Convert to `UseNativeHttpHandler`. For more information, see [Convert to UseNativeHttpHandler](#convert-to-usenativehttphandler). |
| `EnableCodeSigning` | Specifies if code signing is enabled. | | Copy |
| `CodeSigningKey` | Specifies the code signing key. | | Rename to CodesignKey |
| `CodesignKey` | Specifies the code signing key. | | Copy |
| `CodesignProvision` | Specifies the provisioning profile. | | Copy |
| `CodesignEntitlements` | The path to the entitlements file. | | Copy |
| `CodesignExtraArgs` | Extra code signing arguments. | | Copy |
| `PackageSigningKey` | Specifies the code signing key to sign the package. | | Copy |
| `PackagingExtraArgs` | Specifies the extra arguments to the packaging tool. | | Copy |
| `ProductDefinition` | The path to the product definition file to use when packaging. | | Copy |
| `MtouchEnableSGenConc` | | | Rename to `EnableSGenConc`. |
| `EnableSGenConc` | | | Copy |

### Convert to RuntimeIdentifier

The following table shows how to convert the `MtouchArch` and `XamMacArch` properties to the `RuntimeIdentifier` property, or the `RuntimeIdentifiers` property, when migrating a Xamarin.iOS project to .NET for iOS:

| Value              | RuntimeIdentifier  | RuntimeIdentifiers                |
| ------------------ | ------------------ | --------------------------------- |
| ARMv7              | ios-arm            |                                   |
| ARMv7s             | ios-arm            |                                   |
| ARMv7+ARMv7s       | ios-arm            |                                   |
| ARM64              | ios-arm64          |                                   |
| ARMv7+ARM64        |                    | ios-arm;ios-arm64                 |
| ARMv7+ARMv7s+ARM64 |                    | ios-arm;ios-arm64                 |
| x86_64             | iossimulator-x64   |                                   |
| i386               | iossimulator-x86   |                                   |
| x86_64+i386        |                    | iossimulator-x86;iossimulator-x64 |

> [!IMPORTANT]
> If you have multiple runtime identifiers, use the `RuntimeIdentifiers` property rather than the `RuntimeIdentifier` property.

The following table shows how to convert the `MtouchArch` and `XamMacArch` properties to the `RuntimeIdentifier` property when migrating a Xamarin.Mac project to .NET for macOS+:

| Property | RuntimeIdentifier |
| -------- | ----------------- |
| x86_64   | osx-x64           |

The following table shows how to convert the `MtouchArch` and `XamMacArch` properties to the `RuntimeIdentifier` property when migrating a Xamarin.tvOS project to .NET for tvOS:

| Property | RuntimeIdentifier |
| -------- | ----------------- |
| ARM64    | tvos-arm64        |
| x86_64   | tvossimulator-x64 |

For more information about the `RuntimeIdentifier` property, see [RuntimeIdentifier](/dotnet/core/project-sdk/msbuild-props#runtimeidentifier). For more information about runtime identifiers, see [.NET RID Catalog](/dotnet/core/rid-catalog).

### Convert to UseNativeHttpHandler

The following table shows how to convert the `HttpClientHandler` and `MtouchHttpClientHandler` properties to the `UseNativeHttpHandler` property when migrating a Xamarin Apple project to .NET 8:

| Value              | UseNativeHttpHandler  |
| ------------------ | ------------------ |
| HttpClientHandler | false |
| NSUrlSessionHandler | *don't set* |
| CFNetworkHandler | *don't set* |

## Changes to other items

The following table shows how to map other items in Xamarin Apple projects to .NET projects:

| Item | Description | .NET | Project conversion |
| -----| ----------- | ---- | ------------------ |
| `LinkDescription` | Additional XML files to the managed linker. | Identical | Copy |

## Changes to Info.plist

Some values have moved from *Info.plist* to the project file.

### MinimumOSVersion and LSMinimumSystemVersion

The `MinimumOSVersion` and `LSMinimumSystemVersion` properties should be converted to the `SupportedOSPlatformVersion` property in .NET 8 projects. For more information, see [Ensure MinimumOSVersion is consistent with SupportedOSPlatformVersion](https://github.com/xamarin/xamarin-macios/issues/12336).

## See also

- [Project file properties](https://github.com/xamarin/xamarin-macios/wiki/Project-file-properties)
