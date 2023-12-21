---
ms.topic: include
ms.date: 12/21/2023
---

In your .NET MAUI WinUI 3 project, update *App.xaml* to match the code below:

```xaml
<?xml version="1.0" encoding="utf-8"?>
<maui:MauiWinUIApplication
    x:Class="YOUR_NAMESPACE_HERE.WinUI.App"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:maui="using:Microsoft.Maui"
    xmlns:local="using:YOUR_NAMESPACE_HERE.WinUI">
    <maui:MauiWinUIApplication.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />
                <!-- Other merged dictionaries here -->
            </ResourceDictionary.MergedDictionaries>
            <!-- Other app resources here -->
        </ResourceDictionary>
    </maui:MauiWinUIApplication.Resources>
</maui:MauiWinUIApplication>
```

> [!NOTE]
> If your project included resources in your existing *App.xaml* you should migrate them to the new version of the file.

Also, update *App.xaml.cs* to match the code below:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using YOUR_MAUI_CLASS_LIB_HERE;

namespace YOUR_NAMESPACE_HERE.WinUI;

public partial class App : MauiWinUIApplication
{
    public App()
    {
        InitializeComponent();
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
```

> [!NOTE]
> If your project included business logic in *App.xaml.cs* you should migrate that logic to the new version of the file.

Then add a *launchSettings.json* file to the *Properties* folder of the project, and add the following JSON to the file:

```json
{
  "profiles": {
    "Windows Machine": {
      "commandName": "MsixPackage",
      "nativeDebugging": true
    }
  }
}
```
