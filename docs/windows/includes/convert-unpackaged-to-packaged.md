---
ms.topic: include
ms.date: 11/07/2024
---

## Convert an unpackaged .NET MAUI Windows app to packaged

If your app needs to use APIs that are only available with Windows packaged apps and you plan on distributing your app through the Microsoft Store, you'll need to convert your unpackaged app to a packaged app. This can be achieved in Visual Studio:

1. In **Solution Explorer**, right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **Application > Windows Targets** tab and ensure that **Create a Windows MSIX package** is checked:

:::image type="content" source="../media/setup/packaged-app-checked.png" alt-text="Screenshot of Create a Windows MSIX package checked in Visual Studio.":::

Visual Studio will modify your app's project file (*.csproj*) to remove the `<WindowsPackageType>None</WindowsPackageType>` line. In addition, your app's *Properties/launchSettings.json* file will have the `commandName` value changed from `Project` to `MsixPackage`:

```json
{
  "profiles": {
    "Windows Machine": {
      "commandName": "MsixPackage",
      "nativeDebugging": false
    }
  }
}
```

> [!IMPORTANT]
> If your app defines multiple launch setting profiles you'll have to manually update the `commandName` value from `Project` to `MsixPackage` for each profile.

When deploying a packaged .NET MAUI Windows app, you'll need to enable Developer Mode in Windows. For more information, see [Configure Windows for packaged app deployment](#configure-windows-for-packaged-app-deployment).
