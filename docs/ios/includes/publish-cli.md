---
ms.date: 02/22/2023
ms.topic: include
---


At this time, publishing is only supported through the .NET command line interface.

To publish your app, open a terminal and navigate to the folder for your .NET MAUI app project. Run the `dotnet publish` command, providing the following parameters:

| Parameter                    | Value                                                                                           |
|------------------------------|-------------------------------------------------------------------------------------------------|
| `-f` or `--framework`        | The target framework, which is `net6.0-ios` or `net7.0-ios`.                                    |
| `-c` or `--configuration`    | The build configuration, which is `Release`.                                                    |

> [!WARNING]
> Attempting to publish a .NET MAUI solution will result in the `dotnet publish` command attempting to publish each project in the solution individually, which can cause issues when you've added other project types to your solution. Therefore, the `dotnet publish` command should be scoped to your .NET MAUI app project.

In addition, the following common parameters can be specified on the command line if they aren't provided in a `<PropertyGroup>` in your project file:

| Parameter                    | Value                                                                                           |
|------------------------------|-------------------------------------------------------------------------------------------------|
| `/p:RuntimeIdentifier` | The runtime identifier (RID) for the project. Use `ios-arm64`. |
| `/p:CodesignKey` | The name of the distribution certificate you installed into Keychain Access on your Mac build host. |
| `/p:CodesignProvision` | The provisioning profile name. |
| `/p:CodesignEntitlements` | The name of the entitlements file. Use *Entitlements.plist*. |
| `/p:ArchiveOnBuild` | A boolean value that indicates whether to produce the archive. Use `true` to produce the *.ipa*. |
| `/p:TcpPort` | The TCP port to use to communicate with the Mac build host, which is 58181. |
| `/p:ServerAddress` | The IP address of the Mac build host. |
| `/p:ServerUser` | The username to use when logging into the Mac build host. Use your system username rather than your full name. |
| `/p:ServerPassword` | The password for the username used to log into the Mac build host. |
| `/p:_DotNetRootRemoteDirectory` | The folder on the Mac build host that contains the .NET SDK. Use `/Users/{macOS username}/Library/Caches/Xamarin/XMA/SDKs/dotnet/`. |

<!--
> [!IMPORTANT]
> Values for these parameters don't have to be provided on the command line. They can also be provided in the project file. For more information, see [Add code signing data to your app project](#add-code-signing-data-to-your-app-project).
-->

::: moniker range="=net-maui-6.0"

For example, use the following command to create an *.ipa*:

```console
dotnet publish -f:net6.0-ios -c:Release /p:ServerAddress={macOS build host IP address} /p:ServerUser={macOS username} /p:ServerPassword={macOS password} /p:TcpPort=58181 /p:ArchiveOnBuild=true /p:_DotNetRootRemoteDirectory=/Users/{macOS username}/Library/Caches/Xamarin/XMA/SDKs/dotnet/
```

> [!NOTE]
> If the `ServerPassword` parameter is omitted from a command line build invocation, Pair to Mac attempts to log in to the Mac build host using the saved SSH keys.

Publishing builds the app, and then copies the *.ipa* to the *bin\\Release\\net6.0-ios\\ios-arm64\\publish* folder.

::: moniker-end

::: moniker range="=net-maui-7.0"

For example, use the following command to create an *.ipa*:

```console
dotnet publish -f:net7.0-ios -c:Release /p:ServerAddress={macOS build host IP address} /p:ServerUser={macOS username} /p:ServerPassword={macOS password} /p:TcpPort=58181 /p:ArchiveOnBuild=true /p:_DotNetRootRemoteDirectory=/Users/{macOS username}/Library/Caches/Xamarin/XMA/SDKs/dotnet/
```

> [!NOTE]
> If the `ServerPassword` parameter is omitted from a command line build invocation, Pair to Mac attempts to log in to the Mac build host using the saved SSH keys.

Publishing builds the app, and then copies the *.ipa* to the *bin\\Release\\net7.0-ios\\ios-arm64\\publish* folder.

::: moniker-end

During the publishing process it maybe necessary to allow `codesign` to run on your paired Mac:

:::image type="content" source="../deployment/media/overview/codesign.png" alt-text="Allow codesign to sign your app on your paired Mac.":::

The *.ipa* file can then be uploaded to the App Store using App Store Connect. To learn how to use App Store Connect, see [App Store Connect workflow](https://help.apple.com/app-store-connect/#/dev300c2c5bf).

For more information about the `dotnet publish` command, see [dotnet publish](/dotnet/core/tools/dotnet-publish).
