---
title: "Create custom controls with .NET MAUI handlers"
description: ""
ms.date: 08/02/2022
---

# Create custom controls

.NET Multi-platform App UI (.NET MAUI)

Each handler class exposes the native view that implements the cross-platform view via its `PlatformView` property. This property can be accessed to set native view properties, invoke native view methods, and subscribe to native view events. In addition, the cross-platform view implemented by the handler is exposed via its `VirtualView` property.


https://github.com/dotnet/maui/wiki/Porting-Custom-Renderers-to-Handlers

## Register the handler
