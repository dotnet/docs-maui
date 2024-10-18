---
ms.topic: include
ms.date: 17/10/2024
---

> [!TIP]
> Creating a <xref:Microsoft.Maui.Hosting.MauiApp> object each time a .NET MAUI view is embedded as a native view isn't recommended. This can be problematic if embedded views access the `Application.Current` property. Instead, the <xref:Microsoft.Maui.Hosting.MauiApp> object can be created as a shared, static instance:
>
> ```csharp
> public static class MyEmbeddedMauiApp
> {
>     static MauiApp? _shared;
>     public static MauiApp Shared => _shared ??= MauiProgram.CreateMauiApp();
> }
> ```
>
> With this approach, you can instantiate the <xref:Microsoft.Maui.Hosting.MauiApp> object early in your app lifecycle to avoid having a small delay the first time you embed a .NET MAUI view in your app.
