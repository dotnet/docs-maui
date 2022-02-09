---
title: "Preparing Libraries for .NET MAUI"
description: "Check if your libraries are .NET 6 compatible, and upgrade them as needed."
ms.date: 02/08/2022
---

From the time that Xamarin became part of Microsoft, our technology has been moving from being an exception to being the rule. In .NET 6 the SDKs and libraries we maintain for Apple and Google platforms, the runtime that makes this all work, and our cross-platform UI framework are all unified with the rest of .NET that also enables world class console, web, and desktop applications. This is a huge transition that gives you, the .NET developer the latest C# language enhancements, build and runtime improvements, and the most productive Visual Studio productivity features in 2022. In this post, I'll cover the steps you need to take to bring your .NET dependencies

## Are my libraries .NET 6 ready?

Xamarin is built on .NET Framework 4.8 and the Mono runtime. If the library is .NET Standard and has no references to Xamarin.iOS, Xamarin.Android, or Xamarin.Forms features, then it will work "as-is". The question then to consider is discoverability. If you have used "Xamarin" or "Forms" in the name of your project, then we recommend you consider taking this opportunity to choose a new name that reflects the broader vision of the platform it supports. We'll cover how you can transition from one package to another [later in the guide](#obsolete).

What about the rest? 

Xamarin.Android libraries use a "targetFramework" that we can route to the new "net6.0-android" TFM (Target Framework Moniker), so those may also work "as-is".

For all other libraries there are a few things that need to happen:

1. Update Xamarin.Forms.* code usage to Microsoft.Maui.* (if applicable)
2. Update for .NET 6 TFMs
3. Pack the Nuget

> **Can I ship 1 NuGet that targets both Xamarin and .NET MAUI?** Yes! You'll need to likely do some #ifdef conditional compilation in order to keep all that code in a single solution. For more details, [read on here]().

### Update Code from Xamarin to Microsoft.Maui

detail the changes to code with example of #ifdef

### Update for .NET 6 TFMs

Your existing Xamarin library my still use a *.nuspec file, or may use *.csproj instead. We recommend using *.csproj and the `dotnet pack` CLI command, so this is the approach we'll demonstrate. 

Your existing Xamarin *.csproj may look something like this:

```xml
<Project Sdk="MSBuild.Sdk.Extras/3.0.23">
	<PropertyGroup>
		<TargetFrameworks>netstandard2.0;Xamarin.iOS10;Xamarin.Mac20;MonoAndroid10.0;tizen40</TargetFrameworks>
		<TargetFrameworks Condition=" '$(OS)' == 'Windows_NT' ">$(TargetFrameworks);uap10.0.16299;</TargetFrameworks>
		<AssemblyName>ZXing.Net.Mobile.Forms</AssemblyName>
		<RootNamespace>ZXing.Net.Mobile.Forms</RootNamespace>
		<PackageId>ZXing.Net.Mobile.Forms</PackageId>
		<PackageIcon>icon.png</PackageIcon>
		<Summary>ZXing Barcode Scanning for your Xamarin.iOS, Xamarin.Android and Windows Universal apps!</Summary>
		<PackageTags>barcode, zxing, zxing.net, qr, scan, scanning, scanner</PackageTags>
		<Title>ZXing.Net.Mobile Barcode Scanner for Xamarin.Forms</Title>
		<Description>
			ZXing.Net.Mobile is a C#/.NET library based on the open source Barcode Library: ZXing (Zebra Crossing), using the ZXing.Net Port. It works with Xamarin.iOS, Xamarin.Android, Tizen and Windows Universal (UWP). The goal of ZXing.Net.Mobile is to make scanning barcodes as effortless and painless as possible in your own applications.

			See https://github.com/Redth/ZXing.Net.Mobile/releases for release notes.
		</Description>
		<Product>$(AssemblyName) ($(TargetFramework))</Product>
		<AssemblyVersion>3.0.0.0</AssemblyVersion>
		<AssemblyFileVersion>3.0.0.0</AssemblyFileVersion>
		<Version>3.0.0</Version>
		<PackageVersion>$(Version)$(VersionSuffix)</PackageVersion>
		<Authors>Redth</Authors>
		<Owners>Redth</Owners>
		<NeutralLanguage>en</NeutralLanguage>
		<Copyright>© Redth</Copyright>
		<RepositoryUrl>https://github.com/redth/ZXing.Net.Mobile</RepositoryUrl>
		<PackageReleaseNotes>See: https://github.com/Redth/ZXing.Net.Mobile/releases</PackageReleaseNotes>
		<DefineConstants>$(DefineConstants);</DefineConstants>
		<UseFullSemVerForNuGet>false</UseFullSemVerForNuGet>
		<EnableDefaultCompileItems>false</EnableDefaultCompileItems>
		<PackageLicenseExpression>MIT</PackageLicenseExpression>
		<PackageProjectUrl>http://github.com/Redth/ZXing.Net.Mobile</PackageProjectUrl>
		<DebugType>portable</DebugType>
		<Configurations>Debug;Release</Configurations>
		<IncludeProjectPriFile>false</IncludeProjectPriFile>
	</PropertyGroup>
	<PropertyGroup Condition=" '$(Configuration)'=='Debug' ">
		<DebugSymbols>true</DebugSymbols>
	</PropertyGroup>
	<PropertyGroup Condition=" '$(Configuration)'=='Release' And '$(OS)' == 'Windows_NT' ">
		<!-- sourcelink: Declare that the Repository URL can be published to NuSpec -->
		<PublishRepositoryUrl>true</PublishRepositoryUrl>
		<!-- sourcelink: Embed source files that are not tracked by the source control manager to the PDB -->
		<EmbedUntrackedSources>true</EmbedUntrackedSources>
		<!-- sourcelink: Include PDB in the built .nupkg -->
		<AllowedOutputExtensionsInPackageBuildOutputFolder>$(AllowedOutputExtensionsInPackageBuildOutputFolder);.pdb</AllowedOutputExtensionsInPackageBuildOutputFolder>
	</PropertyGroup>
	<ItemGroup Condition=" '$(Configuration)'=='Release' And '$(OS)' == 'Windows_NT' ">
		<PackageReference Include="Microsoft.SourceLink.GitHub" Version="1.0.0" PrivateAssets="All" />
	</ItemGroup>
	<ItemGroup>
		<None Include="..\Art\ZXing.Net.Mobile-Icon.png" PackagePath="icon.png" Pack="true" />
		<Compile Include="**\*.shared.cs" />
		<Compile Include="**\*.shared.*.cs" />
	</ItemGroup>
	<ItemGroup Condition=" $(TargetFramework.StartsWith('netstandard')) ">
		<Compile Include="**\*.netstandard.cs" />
		<Compile Include="**\*.netstandard.*.cs" />
	</ItemGroup>
	<ItemGroup Condition=" $(TargetFramework.StartsWith('uap10.0')) ">
		<PackageReference Include="Microsoft.NETCore.UniversalWindowsPlatform" Version="6.2.11" />
		<Compile Include="**\*.uwp.cs" />
		<Compile Include="**\*.uwp.*.cs" />
		<SDKReference Include="WindowsMobile, Version=10.0.16299.0">
			<Name>Windows Mobile Extensions for the UWP</Name>
		</SDKReference>
	</ItemGroup>
	<ItemGroup Condition=" $(TargetFramework.StartsWith('MonoAndroid')) ">
		<Compile Include="**\*.android.cs" />
		<Compile Include="**\*.android.*.cs" />
		<AndroidResource Include="Resources\xml\*.xml" />
		<PackageReference Include="Xamarin.AndroidX.AppCompat" Version="1.2.0.6" />
	</ItemGroup>
	<ItemGroup Condition=" $(TargetFramework.StartsWith('Xamarin.iOS')) ">
		<Compile Include="**\*.ios.cs" />
		<Compile Include="**\*.ios.*.cs" />
	</ItemGroup>
	<ItemGroup Condition=" $(TargetFramework.StartsWith('Xamarin.Mac')) ">
		<Reference Include="Xamarin.Mac" />
		<Reference Include="netstandard" />
		<Compile Include="**\*.macos.cs" />
		<Compile Include="**\*.macos.*.cs" />
	</ItemGroup>
	<ItemGroup Condition=" $(TargetFramework.StartsWith('tizen')) ">
		<PackageReference Include="Tizen.NET" Version="8.0.0.15631" />
		<Compile Include="**\*.tizen.cs" />
		<Compile Include="**\*.tizen.*.cs" />
	</ItemGroup>
	<ItemGroup>
		<PackageReference Include="Xamarin.Essentials" Version="1.6.0" />
		<PackageReference Include="Xamarin.Forms" Version="5.0.0.1874" />
		<PackageReference Include="ZXing.Net" Version="0.16.6" />
	</ItemGroup>
	<ItemGroup>
		<ProjectReference Include="..\ZXing.Net.Mobile\ZXing.Net.Mobile.csproj" />
	</ItemGroup>
</Project>
```

You'll first want to convert to an SDK style project and use .NET 6 standards for the project. This greatly slims down your project file.

```xml
<Project Sdk="Microsoft.NET.Sdk">
	<PropertyGroup>
		<TargetFrameworks>net6.0-android;net6.0-maccatalyst;net6.0-ios</TargetFrameworks>
		<PackageId>ZXing.Net.Maui</PackageId>
		<Title>ZXing.Net.MAUI Barcode Scanner for .NET MAUI</Title>
		<Authors>Redth</Authors>
		<UseMaui>True</UseMaui>
		<SingleProject>True</SingleProject>
		<AllowUnsafeBlocks>true</AllowUnsafeBlocks>
		<UseSystemResourceKeys>false</UseSystemResourceKeys>
		<Copyright>Copyright © Redth</Copyright>
		<PackageProjectUrl>https://github.com/redth/BigIslandBarcoding</PackageProjectUrl>
		<PackageLicenseExpression>MIT</PackageLicenseExpression>
		<RepositoryUrl>https://github.com/redth/BigIslandBarcoding</RepositoryUrl>
		<AssemblyFileVersion>$(PackageVersion)</AssemblyFileVersion>
		<PublishRepositoryUrl>true</PublishRepositoryUrl>
		<DebugType>portable</DebugType>
	</PropertyGroup>
	<ItemGroup Condition="'$(TargetFramework)' == 'net6.0-android'">
		<PackageReference Include="Xamarin.AndroidX.Camera.Camera2" Version="1.0.1.1" />
		<PackageReference Include="Xamarin.AndroidX.Camera.Lifecycle" Version="1.0.1.1" />
		<PackageReference Include="Xamarin.AndroidX.Camera.View" Version="1.0.0.5-alpha20" />

		<AndroidManifest Include="Platforms/Android/AndroidManifest.xml" />
	</ItemGroup>
	<ItemGroup>
		<PackageReference Include="ZXing.Net" Version="0.16.6" />
	</ItemGroup>
</Project>
```

The info needed to generate your NuGet package is now all included in the *.csproj instead of a *.nuspec.

https://github.com/Redth/BigIslandBarcoding/blob/main/ZXing.Net.MAUI/ZXing.Net.MAUI.csproj

### Compile

any troubleshooting tips?

https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package-dotnet-cli

### Publish


https://docs.microsoft.com/en-us/nuget/guides/create-packages-for-xamarin
https://docs.microsoft.com/en-us/xamarin/cross-platform/app-fundamentals/nuget-manual

### Obsolete

mark your legacy one as 'obsolete' on nuget.org and associate the new package id with it

![NuGet.org obsolete feature](images/nuget-obsolete.png)

<TargetFrameworks>net6.0-android;net6.0-maccatalyst;net6.0-ios;netstandard2.0;Xamarin.iOS10;Xamarin.Mac20;MonoAndroid10.0;tizen40</TargetFrameworks>
<TargetFrameworks Condition=" '$(OS)' == 'Windows_NT' ">$(TargetFrameworks);uap10.0.16299;</TargetFrameworks>
<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows')) and '$(MSBuildRuntimeType)' == 'Full'">$(TargetFrameworks);net6.0-windows10.0.19041</TargetFrameworks>
	