---
ms.topic: include
ms.date: 04/11/2023
---

When it builds your app, .NET Multi-platform App UI (.NET MAUI) can use a linker called *ILLink* to reduce the overall size of the app. ILLink reduces the size by analyzing the intermediate code produced by the compiler. It removes unused methods, properties, fields, events, structs, and classes to produce an app that contains only code and assembly dependencies that are necessary to run the app.

## Linker behavior

The linker supports three modes for .NET MAUI apps on iOS and Mac Catalyst:

- *Don't link*. Disabling linking ensures assemblies aren't modified.
- *Link SDK assemblies only*. In this mode, the linker leaves your assemblies untouched and reduces the size of the SDK assemblies by removing types and members that your app doesn't use.
- *Link all assemblies*. When it links all assemblies, the linker performs additional optimizations to make your app as small as possible. It modifies the intermediate code for your source code, which may break your app if you use features using an approach that the linker's static analysis can't detect. In these cases, you may need to make adjustments to your source code to make your app work correctly.

Linker behavior can be configured for each build configuration of your app.

> [!WARNING]
> Enabling the linker for your app's debug configuration may hinder your debugging experience, as it may remove property accessors that enable you to inspect the state of your objects.
