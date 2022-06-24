---
title: "Data binding"
description: ".NET MAUI data binding is the technique of linking properties of two objects so that changes in one property are automatically reflected in the other property."
ms.date: 01/19/2022
---

# Data binding

A .NET Multi-platform App UI (.NET MAUI) app consists of one or more pages, each of which typically contains multiple user-interface objects called *views*. One of the primary tasks of the app is to keep these views synchronized, and to keep track of the various values or selections that they represent. Often the views represent values from an underlying data source, and users manipulate these views to change that data. When the view changes, the underlying data must reflect that change, and similarly, when the underlying data changes, that change must be reflected in the view.

To handle this successfully, the app must be notified of changes in these views or the underlying data. The common solution is to define events that signal when a change occurs. An event handler can then be installed that is notified of these changes. It responds by transferring data from one object to another. However, when there are many views, there must also be many event handlers, which results in a lot of boilerplate code.

Data binding automates this task, and renders the event handlers unnecessary. Data bindings can be implemented either in XAML or code, but they are much more common in XAML where they help to reduce the size of the code-behind file. By replacing procedural code in event handlers with declarative code or markup, the app is simplified and clarified.

Data binding is therefore the technique of linking properties of two objects so that changes in one property are automatically reflected in the other property. One of the two objects involved in a data binding is almost always an element that derives from `View` and forms part of the visual interface of a page. The other object is either:

- Another `View` derivative, usually on the same page.
- An object in a code file.

> [!NOTE]
> Data bindings between two `View` derivatives are often shown for purposes of clarity and simplicity. However, the same principles can be applied to data bindings between a `View` and other objects. When an application is built using the Model-View-ViewModel (MVVM) architecture, the class with underlying data is often called a viewmodel.
