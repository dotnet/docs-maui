---
ms.topic: include
ms.date: 11/13/2023
---

::: moniker range=">=net-maui-8.0"

### Troubleshoot a remote build

If a `RuntimeIdentifier` isn't specified when building remotely from the command line on Windows, the architecture of the Windows machine will be used. This occurs because the `RuntimeIdentifier` has to be set early in the build process, before the build can connect to the Mac to derive its architecture.

if a `RuntimeIdentifier` isn't specified when building remotely using Visual Studio on Windows, the IDE will detect the architecture of the remote Mac and set it accordingly. Overriding the default can be achieved by setting the `$(ForceSimulatorX64ArchitectureInIDE)` build property:

```xml
<PropertyGroup Condition="'$(Configuration)' == 'Release' And '$(TargetFramework)' == 'net8.0-ios'">
    <ForceSimulatorX64ArchitectureInIDE>true</ForceSimulatorX64ArchitectureInIDE>
</PropertyGroup>
```

::: moniker-end
