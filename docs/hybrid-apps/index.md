---
title: "What are hybrid apps?"
description: ".NET Multi-platform App UI (.NET MAUI) hybrid apps enable you to build apps using Blazor components, or HTML and JavaScript, while accessing native platform features and device hardware."
ms.date: 02/12/2025
---

# What are hybrid apps?

Hybrid apps provide native experiences across multiple device platforms but use web technologies for building the UI. The UI is packaged with the app, and you have full access to the native device capabilities. This approach also allows you to reuse UI assets across devices and web browsers.

Hybrid apps are a blend of both native and web solutions. The core UI of the app is written using web technologies such as HTML, CSS, and JavaScript. These apps are then wrapped in a lightweight native app container that allows them to use certain native platform features and device hardware (like a device's camera, calendar, push notifications, and pinch and spread functionality) that a web app can't access easily or even use at all.

This approach allows hybrid apps to run on, for example, both Android and iOS platforms without the need for completely separate development. In addition, they can be downloaded and installed through the app stores. The main advantage of hybrid apps is that they can achieve greater developer productivity via code reuse across devices and web browsers.

## .NET MAUI hybrid apps

With .NET Multi-platform App UI (.NET MAUI) you can either host Blazor components with the [BlazorWebView](~/user-interface/controls/blazorwebview.md) control or any other HTML/JavaScript-based UI with the [HybridWebView](~/user-interface/controls/hybridwebview.md) control. These controls derive from the native platform’s `WebView` control. No internet is required - your UI is packaged with the app. This makes it easier to share UI across native and web hosting models. Hybrid apps rely on .NET MAUI for a native app container and native controls, if you want to use them. This means you can mix web UI with native UI if you want to. .NET MAUI hybrid apps can also be packaged for store distribution whether that’s the Microsoft, Apple or Google App stores.

> [!NOTE]
> .NET MAUI Blazor hybrid apps also have additional controls, templates and tooling in Visual Studio and Visual Studio Code to make development easier.
