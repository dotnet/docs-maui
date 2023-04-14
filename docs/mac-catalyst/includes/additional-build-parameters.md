---
ms.topic: include
ms.date: 03/23/2023
---

> [!WARNING]
> Attempting to publish a .NET MAUI solution will result in the `dotnet publish` command attempting to publish each project in the solution individually, which can cause issues when you've added other project types to your solution. Therefore, the `dotnet publish` command should be scoped to your .NET MAUI app project.

Additional build parameters can be specified on the command line, if they aren't provided in a `<PropertyGroup>` in your project file. The following table lists some of the common parameters:

| Parameter                    | Value                                                                                           |
|------------------------------|-------------------------------------------------------------------------------------------------|
| `-p:ApplicationTitle` | The user-visible name for the app. |
| `-p:ApplicationId` | The unique identifier for the app, such as `com.companyname.mymauiapp`. |
| `-p:ApplicationVersion` | The version of the build that identifies an iteration of the app. |
| `-p:ApplicationDisplayVersion` | The version number of the app. |
| `-p:RuntimeIdentifier` | The runtime identifier (RID) for the project. Release builds of .NET MAUI Mac Catalyst apps default to using `maccatalyst-x64` and `maccatalyst-arm64` as runtime identifiers, to support universal apps. To support only a single architecture, specify `maccatalyst-x64` or `maccatalyst-arm64`. |

For a full list of build properties, see [Project file properties](https://github.com/xamarin/xamarin-macios/wiki/Project-file-properties).

> [!IMPORTANT]
> Values for all of these parameters don't have to be provided on the command line. They can also be provided in the project file. When a parameter is provided on the command line and in the project file, the command line parameter takes precedence. For more information about providing build properties in your project file, see [Define build properties in your project file](#define-build-properties-in-your-project-file).
