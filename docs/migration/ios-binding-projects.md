---
title: "Xamarin.iOS binding project migration"
description: "Learn how to upgrade a Xamarin.iOS binding project to a .NET for iOS project."
ms.date: 10/29/2026
ai-usage: ai-assisted
no-loc: [ "xcframework" ]
---

# Xamarin.iOS binding project migration

To migrate a Xamarin.iOS binding library to a .NET for iOS binding library:

1. In Visual Studio, create a new iOS Binding Library project with the same name as your Xamarin.iOS binding project:

    :::image type="content" source="media/ios-binding-library.png" alt-text="Screenshot of creating an iOS Binding Library project in Visual Studio.":::

    Open the project file to confirm that you have a .NET SDK-style project:

    ```xml
    <Project Sdk="Microsoft.NET.Sdk">
      <PropertyGroup>
        <TargetFramework>net8.0-ios</TargetFramework>
        <Nullable>enable</Nullable>
        <ImplicitUsings>true</ImplicitUsings>
        <IsBindingProject>true</IsBindingProject>
      </PropertyGroup>

      <ItemGroup>
        <ObjcBindingApiDefinition Include="ApiDefinition.cs" />
        <ObjcBindingCoreSource Include="StructsAndEnums.cs" />
      </ItemGroup>
    </Project>
    ```

    > [!NOTE]
    > The project file for an iOS binding library is similar to the project file for an iOS class library.

1. Add your native library, or native framework, to the project as a native reference. Then update the binding project file with the `<NativeReference>` item from your Xamarin.iOS binding project.

    For a static library (`.a`):

    ```xml
    <ItemGroup>
        <NativeReference Include="libXMBindingLibraryUniversal.a">
            <Kind>Static</Kind>
            <ForceLoad>True</ForceLoad>
        </NativeReference>
    </ItemGroup>
    ```

    For an `.xcframework` bundle (recommended when migrating fat `.framework` or universal `.a` binaries to support both device and Apple silicon simulator architectures), reference the top-level `.xcframework` directory or `.xcframework.zip` archive with `Kind` set to `Framework`:

    ```xml
    <ItemGroup>
        <NativeReference Include="XMBindingLibrary.xcframework">
            <Kind>Framework</Kind>
            <SmartLink>False</SmartLink>
        </NativeReference>
    </ItemGroup>
    ```

    If the native SDK includes multiple `.xcframework` bundles (such as a main framework and its native dependency frameworks), add a separate `<NativeReference>` item for each `.xcframework`:

    ```xml
    <ItemGroup>
        <NativeReference Include="XMBindingLibrary.xcframework">
            <Kind>Framework</Kind>
            <SmartLink>False</SmartLink>
        </NativeReference>
        <NativeReference Include="XMBindingDependency.xcframework">
            <Kind>Framework</Kind>
            <SmartLink>False</SmartLink>
        </NativeReference>
    </ItemGroup>
    ```

1. Copy the API definition from your Xamarin.iOS binding project to your .NET for iOS binding project.
1. Copy any additional required code from your Xamarin.iOS binding project to your .NET for iOS binding project.
1. In your .NET for iOS binding project, replace any unsupported APIs with their .NET for iOS equivalents. For more information, see [Unsupported APIs](#unsupported-apis).

For complete guidance on creating and packaging binding libraries in .NET for iOS, see [Binding Objective-C libraries](/dotnet/ios/binding-libs/) and [Binding an XCFramework](/dotnet/ios/binding-libs/xcframeworks).

## `NoBindingEmbedding` and binding resource packages (`.resources`)

In .NET for iOS, the `NoBindingEmbedding` MSBuild property is `true` by default. Instead of embedding native libraries or frameworks directly inside the managed binding `.dll` as manifest resources, the build places all `<NativeReference>` items into a sidecar binding resource package (`<AssemblyName>.resources.zip`, or an `<AssemblyName>.resources` directory when `CompressBindingResourcePackage` is `false`) next to the compiled `.dll`.

When you distribute a binding library:

- **Using `dotnet pack` (recommended)**: `dotnet pack` automatically packs both the managed `.dll` and the `<AssemblyName>.resources.zip` sidecar into the `lib/<TargetFramework>/` folder of the NuGet package. No custom `.nuspec` file is required.
- **Copying build output manually**: If you distribute the compiled `.dll` directly instead of using a NuGet package, you must also distribute the `<AssemblyName>.resources.zip` file (or `<AssemblyName>.resources` folder) alongside the `.dll`.

> [!WARNING]
> Setting `<NoBindingEmbedding>false</NoBindingEmbedding>` is deprecated and is no longer supported starting with .NET 12, where setting it to `false` causes a build error. If your migrated binding project explicitly sets `<NoBindingEmbedding>false</NoBindingEmbedding>` (for example, to work around a legacy packaging layout or because of `.xcframework` embedding limitations), remove that property and distribute the sidecar `.resources.zip` file or pack the project with `dotnet pack`. See [Binding an XCFramework](/dotnet/ios/binding-libs/xcframeworks) for details.

## Unsupported APIs

The following Xamarin.iOS APIs have changed.

### System.nint and System.nunit

The `System.nint` and `System.nuint` types aren't available in .NET for iOS. Instead, they're replaced with the `nint` and `nuint` types, which map to `System.IntPtr` and `System.UIntPtr` respectively. Code that uses the `System.nint` and `System.nuint` types won't compile and should be replaced with the `nint` and `nuint` types. In addition, code that overloads on `System.IntPtr` and `nint`, and `System.UIntPtr` and `nuint` won't compile. In this scenario, rename or remove one of the overloads.

### System.nfloat

The `System.nfloat` type isn't available in .NET for iOS. Instead, it's replaced with the `System.Runtime.InteropServices.NFloat` type. Modify any code that refers to `System.nfloat` to use `nfloat` or the fully qualified type name.

### NSObject.Handle type change

In .NET for iOS, the `NSObject.Handle` and `INativeObject.Handle` properties have changed type from `System.IntPtr` to a `ObjCRuntime.NativeHandle` struct. This means that many parameters and return values will need to change type. For example:

- All objects that inherit from `Foundation.NSObject` that have `ClassHandle` or `Handle` properties of type `IntPtr` should change their property type to `ObjCRuntime.NativeHandle`.
- All objects that inherit from `Foundation.NSObject` or `ObjCRuntime.DisposableObject` that have constructors that previously took a `System.IntPtr`, or a `System.IntPtr` and a `bool`, should change the `IntPtr` references to `ObjcRuntime.NativeHandle`.

There are implicit conversions between `System.IntPtr` and `ObjCRuntime.NativeHandler`, so after making this type change most code should compile without further changes.

### Model.AutoGeneratedName

The `Model.AutoGeneratedName` property isn't available in .NET for iOS. In Xamarin.iOS, this property specified whether the Objective-C type name for the model should be automatically generated. In .NET for iOS, autogenerating the Objective-C type name from the model is the default and only behavior that's available.
