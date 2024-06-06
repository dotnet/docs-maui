---
title: Unit testing
description:  Learn how to unit test a .NET MAUI app using xUnit, to improve your code quality.
ms.date: 06/06/2024
---

# Unit testing

Cross-platform apps should be tested as they would be used in the real world to improve their quality, reliability, and performance. Many types of testing should be performed on an app, including unit testing, integration testing, and user interface testing. Unit testing is the most common form and is essential to building high-quality apps.

A unit test takes a small unit of an app, typically a method, isolates it from the remainder of the code, and verifies that it behaves as expected. Its goal is to check that each unit of functionality performs as expected, so errors don't propagate throughout the app. Detecting a bug where it occurs is more efficient than observing the effect of a bug indirectly at a secondary point of failure.

Unit tests should typically use the arrange-act-assert pattern:

| Step | Description |
|---------|---------|
| Arrange | Initialize objects and set the value of the data that is passed to the method under test. |
| Act | Invoke the method under test with the required arguments. |
| Assert | Verify that the action of the method under test behaves as expected. |

This pattern ensures that unit tests are readable, self-describing, and consistent.

Unit testing has the most significant effect on code quality when it's an integral part of your software development workflow. Unit tests can act as design documentation and functional specifications for your app. As soon as a method has been written, unit tests should be written that verify the method's behavior in response to standard, boundary, and incorrect input data cases and check any explicit or implicit assumptions made by the code. Alternatively, with test-driven development, unit tests are written before the code.

> [!IMPORTANT]
> Unit tests are very effective against regression. That is, functionality that used to work, but has been disturbed by a faulty update.

[xUnit](https://xunit.net/) is the recommended test framework for .NET MAUI apps.

## Add xUnit tests to a .NET MAUI solution

To add xUnit tests to your .NET MAUI solution, either:

- Use Visual Studio to add a new **xUnit Test Project** to your solution.

  OR

- Use .NET CLI to create a new xUnit test project and add it to your solution. For more information, see [Unit testing C# in .NET using dotnet test and xUnit](/dotnet/core/testing/unit-testing-with-dotnet-test).

The project file (*.csproj*) for the xUnit test project will be similar to the following example:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>

    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="coverlet.collector" Version="6.0.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
    <PackageReference Include="xunit" Version="2.5.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.3" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>

</Project>
```

The `$(TargetFramework)` build property specifies the target framework for the test project. This will be the latest version of .NET that's installed on your machine.

The `xunit` package brings in child packages that include the testing framework itself, and Roslyn analyzers that detect common issues with unit tests. The `xunit.runner.visualstudio` and `Microsoft.NET.Test.Sdk` packages are required to run your unit tests in Visual Studio as well as with the `dotnet test` command. The `coverlet.collector` packages allows collecting code coverage. If you don't intend to collect code coverage, you can remove this package reference. For more information about code coverage for unit testing, see [Use code coverage for unit testing](/dotnet/core/testing/unit-testing-code-coverage).

There are two main approaches to structuring your app for unit testing:

1. The code you'll unit test is in a .NET MAUI class library project.
1. The code you'll unit test is in a .NET MAUI app project.

Each approach requires specific configuration.

### Configure a .NET MAUI class library project for unit testing

With this approach, the code you want to unit test is in a .NET MAUI class library project that's consumed by your .NET MAUI app project. To write unit tests against the .NET MAUI class library will require you to update the target frameworks used by the project. This can be achieved by adding the value of the `$(TargetFramework)` build property from the xUnit test project file (*.csproj*) to the `$(TargetFrameworks)` build property in the .NET MAUI class library project file:

```xml
<TargetFrameworks>net8.0;net8.0-android;net8.0-ios;net8.0-maccatalyst</TargetFrameworks>
```

In this example, a value of `net8.0` has been added to the `$(TargetFrameworks)` build property in the .NET MAUI class library project file.

Then, you must add a reference to your .NET MAUI class library project from your xUnit test project.

### Configure a .NET MAUI app project for unit testing

With this approach, the code you want to unit test is in a .NET MAUI app project. To write unit tests against the .NET MAUI app project will require you to update the target frameworks used by the project. This can be achieved by adding the value of the `$(TargetFramework)` build property from the xUnit test project file (*.csproj*) to the `$(TargetFrameworks)` build property in the .NET MAUI app project file:

```xml
<TargetFrameworks>net8.0;net8.0-android;net8.0-ios;net8.0-maccatalyst</TargetFrameworks>
```

In this example, a value of `net8.0` has been added to the `$(TargetFrameworks)` build property in the .NET MAUI app project file.

It's also necessary to modify your .NET MAUI app project so that it doesn't output an executable for the target framework used by the xUnit test project. This can be achieved by adding a condition to the `$(OutputType)` build property in the .NET MAUI app project file:

```xml
<OutputType Condition="'$(TargetFramework)' != 'net8.0'">Exe</OutputType>
```

In this example, the .NET MAUI app project only produces an executable when the target framework isn't `net8.0`.

Then, you must add a reference to your .NET MAUI app project from your xUnit test project.

## Write unit tests

xUnit supports two different types of unit tests:

| Testing type | Attribute | Description                                                  |
|--------------|-----------|--------------------------------------------------------------|
| Facts        | `Fact`    | Tests that are always true, which test invariant conditions. |
| Theories     | `Theory`  | Tests that are only true for a particular set of data.       |

Unit tests should be placed in your xUnit test project and be decorated with the `[Fact]` or `[Theory]` attribute. The following example shows unit tests that use the `[Fact]` attribute:

```csharp
namespace MyUnitTests
{
    public class MyTests
    {
        [Fact]
        public void PassingTest()
        {
            Assert.AreEqual(4, 2+2);
        }

        [Fact]
        public void FailingTest()
        {
            Assert.AreEqual(5, 2+2);
        }
    }
}
```

In this example, the tests represent a deliberately passing and failing test.

The following example shows unit tests that use the `[Theory]` attribute:

```csharp
namespace MyUnitTests
{
    public class MyTests
    {
        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void MyTheoryTest(int value)
        {
            Assert.True(value % 2 == 1);
        }
    }
}
```

In this example, even though there's only one test method, there are actually three tests because the theory will be ran once for each item of data.

> [!TIP]
> Test one operation with each unit test. As the complexity of a test expands, it makes verification of that test more difficult. By limiting a unit test to a single concern, you can ensure that your tests are repeatable, isolated, and have a shorter execution time. For more information, see [Unit testing best practices](/dotnet/core/testing/unit-testing-best-practices).

## Run unit tests

Unit tests can be ran in Test Explorer in Visual Studio, or with the `dotnet test` command. For information about Test Explorer, see [Run unit tests with Test Explorer](/visualstudio/test/run-unit-tests-with-test-explorer). For information about the `dotnet test` command, see [Unit testing C# in .NET using dotnet test and xUnit](/dotnet/core/testing/unit-testing-with-dotnet-test) and [dotnet test](/dotnet/core/tools/dotnet-test).

## Run unit tests using device runners

Unit tests can also be ran on a device with a device runner. A device runner is a test runner app that provides a visual runner shell and some hooks to run from the CLI using [XHarness](https://github.com/dotnet/xharness). For more information, see the documentation at the [Test device runners wiki](https://github.com/mattleibow/DeviceRunners/wiki).

## See also

- [Dependency injection and unit testing](/dotnet/architecture/maui/unit-testing#dependency-injection-and-unit-testing)
- [Testing MVVM applications](/dotnet/architecture/maui/unit-testing#testing-mvvm-applications)
- [Testing asynchronous functionality](/dotnet/architecture/maui/unit-testing#testing-asynchronous-functionality)
- [Testing INotifyPropertyChanged implementations](/dotnet/architecture/maui/unit-testing#testing-inotifypropertychanged-implementations)
- [Testing exception handling](/dotnet/architecture/maui/unit-testing#testing-exception-handling)
