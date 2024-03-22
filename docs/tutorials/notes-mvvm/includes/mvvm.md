---
author: adegeo
ms.author: adegeo
ms.date: 03/21/2024
ms.topic: include
---

The .NET MAUI developer experience typically involves creating a user interface in XAML, and then adding code-behind that operates on the user interface. Complex maintenance issues can arise as apps are modified and grow in size and scope. These issues include the tight coupling between the UI controls and the business logic, which increases the cost of making UI modifications, and the difficulty of unit testing such code.

The model-view-viewmodel (MVVM) pattern helps cleanly separate an app's business and presentation logic from its user interface (UI). Maintaining a clean separation between app logic and the UI helps address numerous development issues and makes an app easier to test, maintain, and evolve. It can also significantly improve code reuse opportunities and allows developers and UI designers to collaborate more easily when developing their respective parts of an app.

## The pattern

There are three core components in the MVVM pattern: the model, the view, and the view model. Each serves a distinct purpose. The following diagram shows the relationships between the three components.

:::image type="content" source="../media/mvvm/mvvm-pattern.png" alt-text="A diagram demonstrating the parts of an MVVM-modeled application":::

In addition to understanding the responsibilities of each component, it's also important to understand how they interact. At a high level, the view "knows about" the view model, and the view model "knows about" the model, but the model is unaware of the view model, and the view model is unaware of the view. Therefore, the view model isolates the view from the model, and allows the model to evolve independently of the view.

The key to using MVVM effectively lies in understanding how to factor app code into the correct classes and how the classes interact.

## View

The view is responsible for defining the structure, layout, and appearance of what the user sees on screen. Ideally, each view is defined in XAML, with a limited code-behind that doesn't contain business logic. However, in some cases, the code-behind might contain UI logic that implements visual behavior that is difficult to express in XAML, such as animations.

## ViewModel

The view model implements properties and commands to which the view can data bind to, and notifies the view of any state changes through change notification events. The properties and commands that the view model provides define the functionality to be offered by the UI, but the view determines how that functionality is to be displayed.

The view model is also responsible for coordinating the view's interactions with any model classes that are required. There's typically a one-to-many relationship between the view model and the model classes.

Each view model provides data from a model in a form that the view can easily consume. To accomplish this, the view model sometimes performs data conversion. Placing this data conversion in the view model is a good idea because it provides properties that the view can bind to. For example, the view model might combine the values of two properties to make it easier to display by the view.

> [!IMPORTANT]
> .NET MAUI marshals binding updates to the UI thread. When using MVVM this enables you to update data-bound viewmodel properties from any thread, with .NET MAUI's binding engine bringing the updates to the UI thread.

## Model

Model classes are non-visual classes that encapsulate the app's data. Therefore, the model can be thought of as representing the app's domain model, which usually includes a data model along with business and validation logic.
