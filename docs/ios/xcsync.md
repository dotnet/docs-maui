---
title: Xcsync for .NET MAUI
description: [Xcsync is designed to provide .NET developers with improved support for editing Apple specific files]
author: haritha-mohan, mcumming
ms.date: [08/05/2024]
---
# Xcsync: improved support for Apple development via .NET

Xcsync is a tool designed to provide developers using the .NET MAUI/.NET Apple SDKs have improved support for editing Apple specific files, such as storyboards and assets. The tool generates a temporary Xcode project from a .NET project, monitors both for changes, and synchronizes the changes between them.

#### How to use Xcsync

The tool has 2 commands right now: generate and sync. Use generate for tracking your changes from your .NET to Xcode and sync for tracking your changes from Xcode to .NET.<br />
Xcsync can be invoked via a dotnet build target, like so:
```dotnet build /t:xcsync-generate``` or ```dotnet build /t:xcsync-sync```

You can also add the xcsync tool path to your PATH environment variable to invoke the tool directly as a more seamless dotnet command.

You can customize several things regarding the tool's functionality like where the temporary Xcode project will be generated, if you would like to open the Xcode project upon being generated, how verbose you would like the logging output to be. Run ```xcsync [generate/sync] -h``` to learn more.

- - - -

#### Known issues

* The addition of Xcsync to the workload as it is now has made it quite large. In the next release, we plan to optimize how intermediate dependencies are included and make the workload size more manageable.
* When invoking the tool via the dotnet command with a higher verbosity, the xcsync command might generate some extra output in the logs regarding a compatible .NET SDK not being found. Please disregard this extraneous output, the tool will still execute successfully
* Using an invalid value for the --verbosity, -v results in a runtime exception instead of the proper help message.

- - - -

#### Next steps:

* A watch command will be added so that the generate and sync commands do not have to be called explicitly each time and changes between your .NET project and the corresponding Xcode project will be tracked more seamlessly.
* We plan to integrate this tool as part of the .NET MAUI extension for VS Code for a better user experience
