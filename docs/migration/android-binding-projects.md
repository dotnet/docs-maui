---
title: "Xamarin.Android binding project migration"
description: "Learn how to upgrade a Xamarin.Android binding project to a .NET for Android project."
ms.date: 08/21/2023
---

# Xamarin.Android binding project migration

In .NET, there's no concept of an Android binding project as a separate project type. Any of the MSBuild item groups or build actions that work in Xamarin.Android binding projects are supported through a .NET for Android app or library.

To migrate a Xamarin.Android binding library to a .NET for Android class library:

1. In Visual Studio, create a new Android Java Library Binding project with the same name as your Xamarin.Android binding project:

    :::image type="content" source="media/android-binding-library.png" alt-text="Screenshot of creating an Android Java Library Binding project in Visual Studio.":::

    Opening the project file will confirm that you have a .NET SDK-style project:

    ```xml
    <Project Sdk="Microsoft.NET.Sdk">
      <PropertyGroup>
        <TargetFramework>net8.0-android</TargetFramework>
        <SupportedOSPlatformVersion>21</SupportedOSPlatformVersion>
        <Nullable>enable</Nullable>
        <ImplicitUsings>enable</ImplicitUsings>
      </PropertyGroup>
    </Project>
    ```

    > [!NOTE]
    > The project file for an Android binding library is identical to the project file for an Android class library.

1. Add your Java Archive (JAR) or Android Archive (AAR) to the project and ensure that its build action is set to **AndroidLibrary**.
1. Copy any transforms or additions from your Xamarin.Android bindings library.

## Unsupported legacy options

The following legacy options are no longer supported. The supported alternatives have been available for several years, and the smoothest migration option is to update and test your current projects with these options before migrating them to .NET.

### AndroidClassParser

`jar2xml` is no longer a valid option for the `$(AndroidClassParser)` property. `class-parse` is now the default and only supported option.

`class-parse` takes advantage of many new modern features not available in `jar2xml`, such as:

- Automatic parameter names for class methods (if your Java code is compiled with `javac -parameters`).
- Kotlin support.
- Static/default interface member (DIM) support.
- Java Nullable reference type (NRT) annotations support.

### AndroidCodegenTarget

`XamarinAndroid` is no longer a valid option for the `$(AndroidCodegenTarget)` property. `XAJavaInterop1` is now the default and only supported option.

If you have hand-bound code in your `Additions` files that interacts with the generated binding code, it may need to be updated to be compatible with `XAJavaInterop1`.

## Default file inclusion

Given the following file structure:

```
Transforms/
    Metadata.xml
foo.jar
```

`Transforms\*.xml` files are automatically included as a `@(TransformFile)` item, and `.jar`/`.aar` files are automatically included as a `@(AndroidLibrary)` item. This will bind C# types for the Java types found in `foo.jar` using the metadata fixups from `Transforms\Metadata.xml`.

Default Android related file globbing behavior is defined in [AutoImport.props](https://github.com/xamarin/xamarin-android/blob/main/src/Xamarin.Android.Build.Tasks/Microsoft.Android.Sdk/Sdk/AutoImport.props). This behavior can be disabled for Android items by setting the `$(EnableDefaultAndroidItems)` property to `false`, or all default item inclusion behavior can be disabled by setting the `$(EnableDefaultItems)` property to `false`.

Undesired `.jar` or `.aar` files could be included with the default wildcards. For example, the following C# compiler errors results from a `AndroidStudio\gradle\wrapper\gradle-wrapper.jar` file being unintentionally bound:

```
Org.Gradle.Cli.AbstractCommandLineConverter.cs(11,89): error CS0535: 'Download' does not implement interface member 'IDownload.Download(URI, File)'
Org.Gradle.Wrapper.Download.cs(10,60): error CS0535: 'AbstractCommandLineConverter' does not implement interface member 'ICommandLineConverter.Convert(ParsedCommandLine, Object)'
```

To solve this issue, you can remove the specific file in your project file:

```xml
<ItemGroup>
  <AndroidLibrary Remove="AndroidStudio\gradle\wrapper\gradle-wrapper.jar" />
</ItemGroup>
```

Alternatively, you could exclude all files within a folder:

```xml
<AndroidLibrary Remove="AndroidStudio\**\*" />
```

## New item group names

`<AndroidLibrary>` is now the recommended item group to use for all `.jar` and `.aar` files. In Xamarin.Android, the following items groups were used, which can instead use item metadata to achieve the same result:

| Legacy Item Group      | New Item Group   | Item Metadata  | Legacy Project Type          |
|------------------------|------------------|----------------| ---------------------------- |
| `AndroidAarLibrary`    | `AndroidLibrary` | `Bind="false"` | Application                  |
| `AndroidJavaLibrary`   | `AndroidLibrary` | `Bind="false"` | Application or class library |
| `EmbeddedJar`          | `AndroidLibrary` | n/a            | Binding project              |
| `EmbeddedReferenceJar` | `AndroidLibrary` | `Bind="false"` | Binding project              |
| `InputJar`             | `AndroidLibrary` | `Pack="false"` | Binding project              |
| `LibraryProjectZip`    | `AndroidLibrary` | n/a            | Binding project              |

Consider a `.aar` or `.jar` file, in which you aren't interested in including a C# binding. This is common for cases where you have Java or Kotlin dependencies that you don't need to call from C#. In this case, you can set the `Bind` metadata to `false`. By default, the file is picked up by the default wildcards. You can also use the `Update` attribute to set the `Bind` metadata:

```xml
<ItemGroup>
  <AndroidLibrary Update="foo.jar" Bind="false">
</ItemGroup>
```

In an Android class library project, this would redistribute the `.jar` file inside the resulting NuGet package as is. In an Android application project, this would include the `.jar` file in the resulting `.apk` or `.aab` file. Neither would include a C# binding for this Java library.

## Embedded JAR/AAR files

In Xamarin.Android, the Java `.jar` or `.aar` was often embedded into the binding `.dll` as an Embedded Resource. However, this led to slow builds, as each `.dll` must be opened and scanned for Java code. If found, it must be extracted to disk to be used.

In .NET, Java code is no longer embedded in the `.dll`. The app build process will automatically include any `.jar` or `.aar` files it finds in the same directory as a referenced `.dll`.

If a project references a binding via `<PackageReference>` or `<ProjectReference>` then everything works and no additional considerations are required. However, if a project references a binding via `<Reference>`, the `.jar`/`.aar` must be located next to the `.dll`. That is, for the following reference:

```xml
<Reference Include="MyBinding.dll" />
```

A directory like the one in the following example won't work:

```
lib/
    MyBinding.dll
```

Instead, the directory must also contain the native code:

```
lib/
    MyBinding.dll
    mybinding.jar
```

## Migration considerations

There are several new features set by default to help produce bindings that better match their Java counterparts. However, if you are migrating an existing binding project, these features may create bindings that are not API compatible with your existing bindings. To maintain compatibility, you may wish to disable or modify these new features.

### Interface constants

Traditionally, C# has not allowed constants to be declared in an `interface`, which is a common pattern in Java:

```java
public interface Foo {
     public static int BAR = 1;
}
```

This pattern was previously supported by creating an alternative `class` that contains the constants:

```csharp
public abstract class Foo : Java.Lang.Object
{
   public static int Bar = 1;
}
```

With C# 8, these constants are placed on the `interface`:

```csharp
public interface IFoo
{
    public static int Bar = 1;
}
```

However, this means that the alternative class that existing code may depend on is no longer generated.

Setting the `$(AndroidBoundInterfacesContainConstants)` property to `false` in your project file will revert to the legacy behavior.

### Nested interface types

Traditionally, C# has not allowed nested types to be declared in an `interface`, which is allowed in Java:

```java
public interface Foo {
     public class Bar { }
}
```

This pattern was supported by moving the nested type to a top-level type with a generated name composed of the interface and nested type name:

```csharp
public interface IFoo { }

public class IFooBar : Java.Lang.Object { }
```

With C# 8, nested types can placed in the `interface`:

```csharp
public interface IFoo
{
    public class Bar : Java.Lang.Object { }
}
```

However, this means that the top-level class that existing code may depend on is no longer generated.

Setting the `$(AndroidBoundInterfacesContainTypes)` property to `false` in your project file will revert to the legacy behavior.

If you wish to use a hybrid approach, for example, to keep existing nested types moved to a top-level type, but allow any future nested types to remain nested, you can specify this at the `interface` level using `metadata` to set the `unnest` attribute. Setting it to `true` will result in "un-nesting" any nested types (legacy behavior):

```xml
<attr path="/api/package[@name='my.package']/interface[@name='Foo']" name="unnest">true</attr>
```

Setting it to `false` will result in nested types remaining nested in the `interface` (.NET behavior):

```xml
<attr path="/api/package[@name='my.package']/interface[@name='Foo']" name="unnest">false</attr>
```

Using this approach, you could leave the `$(AndroidBoundInterfacesContainTypes)` property as `true` and set `unnest` to `true` for every `interface` with nested types that you currently have. These will always remain top-level types, while any new nested types introduced later will be nested.

### Static and default interface members (DIM)

Traditionally, C# has not allowed interfaces to contain `static` members and `default` methods:

```java
public interface Foo {
  public static void Bar () { ... }
  public default void Baz () { ... }
}
```

Static members on interfaces has been supported by moving them to a sibling `class`:

```csharp
public interface IFoo { }

public class Foo
{
    public static void Bar () { ... }
}
```

`default` interface methods have traditionally not been bound, since they aren't required and there wasn't a C# construct to support them.

With C# 8, `static` and `default` members are supported on interfaces, mirroring the Java interface:

```csharp
public interface IFoo
{
    public static void Bar () { ... }
    public default void Baz () { ... }
}
```

However, this means the alternative sibling `class` containing `static` members will no longer be generated.

Setting the `$AndroidBoundInterfacesContainStaticAndDefaultInterfaceMethods` property to `false` in your project file will revert to the legacy behavior.

### Nullable reference types

Support for Nullable Reference Types (NRT) was added in Xamarin.Android 11.0. NRT support can be enabled using the standard .NET mechanism:

```xml
<PropertyGroup>
  <Nullable>enable</Nullable>
</PropertyGroup>
```

Because the default for .NET is `disable`, the same applies for Xamarin.Android projects.

### Resource.designer.cs

In Xamarin.Android, Java binding projects didn't support generating a `Resource.designer.cs` file. Since binding projects are just class libraries in .NET, this file will be generated. This could be a breaking change when migrating existing projects.

One example of a failure from this change, is if your binding generates a class named `Resource` in the root namespace:

```
error CS0101: The namespace 'MyBinding' already contains a definition for 'Resource'
```

Or in the case of AndroidX, there are project files with `-` in the name such as `androidx.window/window-extensions.csproj`. This results in the root namespace `window-extensions` and invalid C# in `Resource.designer.cs`:

```
error CS0116: A namespace cannot directly contain members such as fields, methods or statements
error CS1514: { expected
error CS1022: Type or namespace definition, or end-of-file expected
```

To disable `Resource.designer.cs` generation, set the `$(AndroidGenerateResourceDesigner)` property to `false` in your project file:

```xml
<PropertyGroup>
  <AndroidGenerateResourceDesigner>false</AndroidGenerateResourceDesigner>
</PropertyGroup>
```
