---
ms.topic: include
ms.date: 04/11/2023
---

When building your app, .NET Multi-platform App UI (.NET MAUI) can use a linker called *ILLink* to reduce the overall size of the app. ILLink does this by analysing the intermediate code produced by the compiler, removing unused methods, properties, fields, events, structs, and classes to produce an app that contains only code and assembly dependencies that are necessary to run the app.

## Linker behavior

The linker supports three modes for .NET MAUI iOS apps:

- *Don't link*. Disabling linking will ensure assemblies aren't modified.
- *Link SDK assemblies only*. In this mode, the linker will leave your assemblies untouched, and will reduce the size of the SDK assemblies by removing types and members that your app doesn't use.
- *Link all assemblies*. When linking all assemblies, the linker will perform additional optimizations to make your app as small as possible. It will modify the intermediate code for your source code, which may break your app if you use features using an approach that the linker's static analysis can't detect. In these cases, adjustments to your source code may be required to make your app work correctly.

Linker behavior can be configured for each build configuration of your app.

> [!WARNING]
> Enabling the linker for debug configuration may hinder your debugging experience, as it may remove property accessors that enable you to inspect the state of your objects.
