---
title: UI testing with Appium
description: Learn how to automate UI testing for .NET MAUI apps using Appium across Android, iOS, Windows, and Mac Catalyst.
ms.date: 04/14/2026
---

# UI testing with Appium

UI testing verifies that your app's user interface behaves correctly by automating interactions such as tapping buttons, entering text, and navigating between pages. While [unit tests](unit-testing.md) validate individual methods and classes in isolation, UI tests exercise the full app on a real device or emulator to catch regressions that only surface through actual user interaction.

[Appium](https://appium.io) is an open-source UI testing framework that works with native, hybrid, and web apps across multiple platforms. Because Appium operates at the platform level, it tests your app the same way regardless of whether it was built with .NET MAUI or platform-native tooling. Appium uses platform-specific drivers to send interactions to your app:

| Platform | Appium driver | Host OS |
|---|---|---|
| Android | [UIAutomator2](https://github.com/appium/appium-uiautomator2-driver) | Windows, macOS, Linux |
| iOS | [XCUITest](https://github.com/appium/appium-xcuitest-driver) | macOS only |
| Mac Catalyst | [Mac2](https://github.com/appium/appium-mac2-driver) | macOS only |
| Windows | [Windows](https://github.com/appium/appium-windows-driver) | Windows only |

> [!NOTE]
> What you can test depends on your development machine. On **Windows**, you can test Android and Windows apps. On **macOS**, you can test Android, iOS, and Mac Catalyst apps.

> [!TIP]
> For a complete working sample, see the [Basic Appium NUnit Sample](https://github.com/dotnet/maui-samples/tree/main/10.0/UITesting/BasicAppiumNunitSample) on GitHub.

## Prerequisites

Before you can write and run UI tests, install the following prerequisites:

### Node.js and Appium

Appium is built on Node.js. Install [Node.js](https://nodejs.org/) (LTS version recommended), then install Appium and the drivers for the platforms you want to test:

```bash
# Install Appium
npm install -g appium

# Install platform drivers (install only the ones you need)
appium driver install uiautomator2     # Android
appium driver install xcuitest         # iOS (macOS only)
appium driver install mac2             # Mac Catalyst (macOS only)
appium driver install windows          # Windows
```

Verify the installation by running `appium` in a terminal. The server should start and display a message indicating it's listening on port 4723.

### Windows Application Driver (Windows only)

The Appium Windows driver uses [Windows Application Driver (WinAppDriver)](https://github.com/microsoft/WinAppDriver) to automate Windows apps. Download and install [WinAppDriver version 1.2.1](https://github.com/microsoft/WinAppDriver/releases/tag/v1.2.1).

> [!IMPORTANT]
> Use WinAppDriver version 1.2.1 specifically. Other versions may not work correctly with the Appium Windows driver.

### Android SDK (Android only)

Make sure the Android SDK is installed and that the `ANDROID_HOME` environment variable points to its location. If you've already set up your machine for .NET MAUI Android development, this should be in place.

## Prepare the .NET MAUI app for testing

All UI elements that you want to interact with from your tests need to have the `AutomationId` property set to a unique value. This property maps to platform-specific accessibility identifiers that Appium uses to locate elements.

```xaml
<Button
    x:Name="CounterBtn"
    AutomationId="CounterBtn"
    Text="Click me"
    SemanticProperties.Hint="Counts the number of times you click"
    Clicked="OnCounterClicked"
    HorizontalOptions="Fill" />
```

### Android activity registration

For Android, Appium needs to know the fully qualified activity name to launch your app. Add a `[Register]` attribute to your `MainActivity` class with a value that matches your app's package name:

```csharp
[Register("com.companyname.myapp.MainActivity")]
public class MainActivity : MauiAppCompatActivity
{
}
```

Make sure the value in the `Register` attribute matches the `ApplicationId` in your project file and the `AppActivity` capability in your test setup.

## Create the test projects

A proven project structure uses separate test projects per platform, with shared test code in a [NoTargets project](https://github.com/microsoft/MSBuildSdks/blob/main/src/NoTargets/). This is the same pattern used in the [official sample](https://github.com/dotnet/maui-samples/tree/main/10.0/UITesting/BasicAppiumNunitSample) and the .NET MAUI codebase itself:

```
MySolution/
├── MauiApp/                  # Your .NET MAUI app
├── UITests.Shared/           # Shared test code (NoTargets project)
├── UITests.Android/          # Android-specific setup
├── UITests.iOS/              # iOS-specific setup
├── UITests.Windows/          # Windows-specific setup
└── UITests.macOS/            # macOS-specific setup
```

Each platform project compiles the shared code by linking to the files in the Shared project. This means all tests in the Shared project run on every platform you test. The Shared project itself can't be run directly. Always run one of the platform-specific test projects.

> [!IMPORTANT]
> Keep the namespace the same across all test projects (for example, `UITests`). NUnit's `[SetUpFixture]` attribute runs setup methods for all test fixtures in the same namespace. If the namespaces don't match, the Appium driver won't be initialized when your tests run.

### Platform test project NuGet packages

Each platform-specific test project needs the following NuGet packages:

```xml
<ItemGroup>
    <PackageReference Include="Appium.WebDriver" Version="8.0.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
    <PackageReference Include="NUnit" Version="3.14.0" />
    <PackageReference Include="NUnit3TestAdapter" Version="4.5.0" />
</ItemGroup>
```

> [!TIP]
> Instead of creating test projects from scratch, you can install community-maintained templates that scaffold the full project structure for you. For more information, see the [Template.Maui.UITesting](https://github.com/jfversluis/Template.Maui.UITesting) GitHub repository.

## Write a base test class

Create a `BaseTest.cs` file in the Shared project. This base class provides access to the Appium driver and a helper method that handles a difference in how elements are located on Windows versus other platforms:

```csharp
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace UITests;

public abstract class BaseTest
{
    protected AppiumDriver App => AppiumSetup.App;

    protected AppiumElement FindUIElement(string id)
    {
        if (App is WindowsDriver)
        {
            return App.FindElement(MobileBy.AccessibilityId(id));
        }

        return App.FindElement(MobileBy.Id(id));
    }
}
```

On Windows, `AutomationId` is accessed through `MobileBy.AccessibilityId`. On Android, iOS, and macOS, use `MobileBy.Id`.

## Start the Appium server

The Appium server must be running before you execute tests. The simplest approach is to start it manually from a terminal:

```bash
appium
```

The server starts on `http://127.0.0.1:4723` by default. Leave it running while you run your tests.

### Start the server programmatically (optional)

To make your test suite self-contained, you can start and stop the Appium server as part of the test run. Create an `AppiumServerHelper.cs` file in the Shared project:

```csharp
using OpenQA.Selenium.Appium.Service;

namespace UITests;

public static class AppiumServerHelper
{
    private static AppiumLocalService? _appiumLocalService;

    public const string DefaultHostAddress = "127.0.0.1";
    public const int DefaultHostPort = 4723;

    public static void StartAppiumLocalServer(
        string host = DefaultHostAddress,
        int port = DefaultHostPort)
    {
        if (_appiumLocalService is not null)
            return;

        var builder = new AppiumServiceBuilder()
            .WithIPAddress(host)
            .UsingPort(port);

        _appiumLocalService = builder.Build();
        _appiumLocalService.Start();
    }

    public static void DisposeAppiumLocalServer()
    {
        _appiumLocalService?.Dispose();
    }
}
```

## Configure Appium for each platform

Each platform project contains an `AppiumSetup.cs` file that configures the Appium driver with platform-specific options. This class uses NUnit's `[SetUpFixture]` attribute to initialize the driver once before all tests run.

### Android

```csharp
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace UITests;

[SetUpFixture]
public class AppiumSetup
{
    private static AppiumDriver? driver;

    public static AppiumDriver App => driver
        ?? throw new NullReferenceException("AppiumDriver is null");

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        AppiumServerHelper.StartAppiumLocalServer();

        var androidOptions = new AppiumOptions
        {
            AutomationName = "UIAutomator2",
            PlatformName = "Android",
        };

        // For debug builds, use NoReset to preserve Fast Deployment libraries
        androidOptions.AddAdditionalAppiumOption(
            MobileCapabilityType.NoReset, "true");
        androidOptions.AddAdditionalAppiumOption(
            AndroidMobileCapabilityType.AppPackage,
            "com.companyname.myapp");
        androidOptions.AddAdditionalAppiumOption(
            AndroidMobileCapabilityType.AppActivity,
            "com.companyname.myapp.MainActivity");

        driver = new AndroidDriver(androidOptions);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        driver?.Quit();
        AppiumServerHelper.DisposeAppiumLocalServer();
    }
}
```

> [!NOTE]
> For debug builds on Android, set `NoReset` to `true`. Debug builds use Fast Deployment, and Appium's default reset behavior deletes the libraries that Fast Deployment requires. For release builds, you can instead set the `App` property to the full path of the signed `.apk` file.

### iOS

```csharp
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;

namespace UITests;

[SetUpFixture]
public class AppiumSetup
{
    private static AppiumDriver? driver;

    public static AppiumDriver App => driver
        ?? throw new NullReferenceException("AppiumDriver is null");

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        AppiumServerHelper.StartAppiumLocalServer();

        var iOSOptions = new AppiumOptions
        {
            AutomationName = "XCUITest",
            PlatformName = "iOS",
            PlatformVersion = "17.0",
            DeviceName = "iPhone 15 Pro",
            App = "com.companyname.myapp",
        };

        driver = new IOSDriver(iOSOptions);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        driver?.Quit();
        AppiumServerHelper.DisposeAppiumLocalServer();
    }
}
```

### Windows

```csharp
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace UITests;

[SetUpFixture]
public class AppiumSetup
{
    private static AppiumDriver? driver;

    public static AppiumDriver App => driver
        ?? throw new NullReferenceException("AppiumDriver is null");

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        AppiumServerHelper.StartAppiumLocalServer();

        var windowsOptions = new AppiumOptions
        {
            AutomationName = "windows",
            PlatformName = "Windows",
            App = "com.companyname.myapp_9zz4h110yvjzm!App",
        };

        driver = new WindowsDriver(windowsOptions);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        driver?.Quit();
        AppiumServerHelper.DisposeAppiumLocalServer();
    }
}
```

### Mac Catalyst

```csharp
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Mac;

namespace UITests;

[SetUpFixture]
public class AppiumSetup
{
    private static AppiumDriver? driver;

    public static AppiumDriver App => driver
        ?? throw new NullReferenceException("AppiumDriver is null");

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        AppiumServerHelper.StartAppiumLocalServer();

        var macOptions = new AppiumOptions
        {
            AutomationName = "mac2",
            PlatformName = "Mac",
            App = "/path/to/MyApp/bin/Debug/net10.0-maccatalyst/maccatalyst-x64/MyApp.app",
        };

        macOptions.AddAdditionalAppiumOption(
            IOSMobileCapabilityType.BundleId,
            "com.companyname.myapp");

        driver = new MacDriver(macOptions);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        driver?.Quit();
        AppiumServerHelper.DisposeAppiumLocalServer();
    }
}
```

> [!IMPORTANT]
> For Mac Catalyst, you must set the `BundleId` option. Without it, Appium will automate Finder instead of your app.

## Write UI tests

Add test classes to the Shared project so they run on all platforms. Each test class should inherit from `BaseTest`. The following example tests the counter button in the default .NET MAUI template:

```csharp
using NUnit.Framework;

namespace UITests;

public class MainPageTests : BaseTest
{
    [Test]
    public void AppLaunches()
    {
        App.GetScreenshot().SaveAsFile($"{nameof(AppLaunches)}.png");
    }

    [Test]
    public void ClickCounterTest()
    {
        // Arrange
        var element = FindUIElement("CounterBtn");

        // Act
        element.Click();
        Task.Delay(500).Wait();

        // Assert
        App.GetScreenshot().SaveAsFile($"{nameof(ClickCounterTest)}.png");
        Assert.That(element.Text, Is.EqualTo("Clicked 1 time"));
    }
}
```

The `FindUIElement` method locates controls by their `AutomationId` value. You can also use other selectors:

| Selector | Method | Notes |
|---|---|---|
| AutomationId | `MobileBy.Id()` or `MobileBy.AccessibilityId()` | Preferred. Use the `FindUIElement` helper to abstract the platform difference. |
| XPath | `MobileBy.XPath()` | Slower, but useful for complex queries. |
| Class name | `MobileBy.ClassName()` | Finds elements by their native class name. |

> [!TIP]
> Set `AutomationId` on every element you want to test. It's the most reliable and performant way to locate elements across all platforms.

## Run UI tests

Before running tests, make sure:

- The .NET MAUI app is deployed to the target device or emulator.
- For Android, an emulator is booted or a physical device is connected.
- For iOS, a Simulator is running or a device is provisioned.
- For Windows, the app is installed (for example, by running it from Visual Studio first).
- For macOS, the `.app` bundle is built and available at the path specified in `AppiumSetup.cs`.

Run tests from Visual Studio using **Test Explorer**, or from the command line:

```bash
dotnet test UITests.Android/UITests.Android.csproj
```

Replace the project path with the platform you want to test.

## See also

- [Unit testing](unit-testing.md)
- [Getting started with UI testing .NET MAUI apps using Appium (blog post)](https://devblogs.microsoft.com/dotnet/dotnet-maui-ui-testing-appium/)
- [Use BrowserStack App Automate with Appium UI Tests for .NET MAUI Apps (blog post)](https://devblogs.microsoft.com/dotnet/browserstack-appium-dotnet-maui/)
- [Appium documentation](https://appium.io/docs/en/latest/)
- [Template.Maui.UITesting (project templates)](https://github.com/jfversluis/Template.Maui.UITesting)
