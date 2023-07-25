---
title: "XAML"
description: ".NET MAUI XAML enables you to define app user interfaces using markup rather than code."
ms.date: 01/24/2022
---

# XAML

The eXtensible Application Markup Language (XAML) is an XML-based language that's an alternative to programming code for instantiating and initializing objects, and organizing those objects in parent-child hierarchies.

XAML allows developers to define user interfaces in .NET Multi-platform App UI (.NET MAUI) apps using markup rather than code. XAML is not required in a .NET MAUI app, but it is the recommended approach to developing your UI because it's often more succinct, more visually coherent, and has tooling support. XAML is also well suited for use with the Model-View-ViewModel (MVVM) pattern, where XAML defines the view that is linked to viewmodel code through XAML-based data bindings.

Within a XAML file, you can define user interfaces using all the .NET MAUI views, layouts, and pages, as well as custom classes. The XAML file can be either compiled or embedded in the app package. Either way, the XAML is parsed at build time to locate named objects, and at runtime the objects represented by the XAML are instantiated and initialized.

XAML has several advantages over equivalent code:

- XAML is often more succinct and readable than equivalent code.
- The parent-child hierarchy inherent in XML allows XAML to mimic with greater visual clarity the parent-child hierarchy of user-interface objects.

There are also disadvantages, mostly related to limitations that are intrinsic to markup languages:

- XAML cannot contain code. All event handlers must be defined in a code file.
- XAML cannot contain loops for repetitive processing. However there are controls that display collections of data, such as <xref:Microsoft.Maui.Controls.ListView> and <xref:Microsoft.Maui.Controls.CollectionView>.
- XAML cannot contain conditional processing. However, a data-binding can reference a code-based binding converter that effectively allows some conditional processing.
- XAML generally cannot instantiate classes that do not define a parameterless constructor, although this restriction can sometimes be overcome.
- XAML generally cannot call methods, although this restriction can sometimes be overcome.

There is no visual designer for producing XAML in .NET MAUI apps. All XAML must be hand-written, but you can use XAML hot reload to view your UI as you edit it.

XAML is basically XML, but XAML has some unique syntax features. The most important are:

- Property elements
- Attached properties
- Markup extensions

These features are *not* XML extensions. XAML is entirely legal XML. But these XAML syntax features use XML in unique ways.
