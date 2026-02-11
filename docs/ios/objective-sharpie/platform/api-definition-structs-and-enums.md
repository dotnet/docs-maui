---
title: "ApiDefinitions &amp; StructsAndEnums Files"
description: "This document describes the ApiDefinition.cs and StructsAndEnums.cs files that Objective Sharpie generates. These files are then used to access the Objective-C code from C#."
ms.date: 02/11/2026
---

# ApiDefinitions & StructsAndEnums Files

When Objective Sharpie has run successfully, it generates
`Binding/ApiDefinition.cs` and `Binding/StructsAndEnums.cs` files.
These two files are added to a binding project, which will produce the final binding assembly.

In *some* cases these generated files might be all you need, however more often
the developer will need to manually modify these generated files
to fix any issues that could not be automatically handled by the tool
(such as those flagged with a [`Verify` attribute](~/ios/objective-sharpie/platform/verify.md)).

Some of the next steps include:

- **Adjusting Names**: Sometimes you will want to adjust the names of methods and classes to match the .NET Framework Design Guidelines.
- **Methods or Properties**: The heuristics used by Objective Sharpie sometimes will pick a method to be turned into a property. At this point, you could decide whether this is the intended behavior or not.
- **Hook up Events**: You could link your classes with your delegate classes and automatically generate events for those.
- **Hook up Notifications**: It is not possible to extract the API contract of notifications from the pure header files, this will require a trip to the API documentation. If you want strongly typed notifications, you will need to update the result.
- **API Curation**: At this point, you might choose to provide extra constructors, add methods (to allow for C# initialize-on-construction syntax), operator overloading and implement your own interfaces on the extra definitions file.

See [iOS binding projects](~/migration/ios-binding-projects.md)
to see how these files fit into the binding process, as shown in the diagram below:

![The binding process is shown in this diagram](api-definition-structs-and-enums-images/binding-flowchart.png)

Refer to the <xref:ObjCRuntime> namespace API reference
for more information on binding attributes used in these files.
