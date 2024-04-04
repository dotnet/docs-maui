---
ms.topic: include
ms.date: 05/23/2022
---

1. To create .NET MAUI apps, you'll need to download and run the [installer](https://github.com/dotnet/installer/blob/main/README.md#installers-and-binaries) for the latest .NET runtime. You'll also need to download and install the latest version of Xcode, which is also available from the App Store app on your Mac.

1. On your Mac, open **Terminal** and check that you have the latest .NET runtime installed:

    ```zsh
    dotnet --version
    ```

1. In **Terminal**, install the latest public build of .NET MAUI:

    ```zsh
    sudo dotnet workload install maui --source https://api.nuget.org/v3/index.json
    ```

    This command will install the latest released version of .NET MAUI, including the required platform SDKs.

1. In **Terminal**, create a new .NET MAUI app using .NET CLI:

    ```zsh
    dotnet new maui -n "MyMauiApp"
    ```
