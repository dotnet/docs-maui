---
title: "Upgrading Xamarin.Forms"
description: ""
ms.date: 10/01/2022
---

# Upgrading Xamarin.Forms to .NET MAUI

In order to upgrade your Xamarin.Android and Xamarin.iOS projects to .NET you will first update to the SDK Style project system (i.e. update your csproj) and then update your dependencies to .NET 6 compatible versions. For most applications, that’s the beginning and the end of the journey – compile and test your apps. You don’t need to change namespaces or rewrite these apps.

**Checklist:**

* Update your csproj to SDK Style
* Upgrade or replace incompatible dependencies with .NET 6 versions
* Compile and test your app

To simplify the upgrade, we recommend creating a new .NET project of the same type and name as your Xamarin project, and then copying in your code. This is the approach we will describe below.
