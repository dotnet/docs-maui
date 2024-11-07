---
ms.topic: include
ms.date: 11/07/2024
---

## Convert a packaged .NET MAUI Windows app to unpackaged

To convert an existing .NET MAUI Windows packaged app to an unpackaged app in Visual Studio:

1. In **Solution Explorer**, right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **Application > Windows Targets** tab and ensure that **Create a Windows MSIX package** is unchecked:

:::image type="content" source="../media/setup/packaged-app-unchecked.png" alt-text="Screenshot of Create a Windows MSIX package unchecked in Visual Studio.":::

Visual Studio will modify your app's project file (*.csproj) to set the `$(WindowsPackageType)` build property to `None`:

```xml
<PropertyGroup>
    <WindowsPackageType>None</WindowsPackageType>
</PropertyGroup>
```

In addition, your app's *Properties/launchSettings.json* file will have the `commandName` value changed from `MsixPackage` to `Project`:

    ```json
    {
      "profiles": {
        "Windows Machine": {
          "commandName": "Project",
          "nativeDebugging": false
        }
      }
    }
    ```

> [!IMPORTANT]
> If your app defines multiple launch setting profiles you'll have to manually update the `commandName` value from `MsixPackage` to `Project` for each profile.
