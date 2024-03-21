---
author: adegeo
ms.author: adegeo
ms.date: 03/21/2024
ms.topic: include
no-loc: ["communitytoolkit mvvm", "CommunityToolkit.Mvvm", "AllNotes", "Notes", "About", "ViewModels", "AboutViewModel.cs", "NoteViewModel.cs", "NotesViewModel.cs", "AboutPage.xaml", "AboutPage.xaml.cs"]
---

Before adding view models to the project, add a reference to the MVVM Community Toolkit. This library is available on NuGet, and provides types and systems that help implement the MVVM pattern.

01. In the **Solution Explorer** pane of Visual Studio, right-click on the **Notes** project > **Manage NuGet Packages**.
01. Select the **Browse** tab.
01. Search for **communitytoolkit mvvm** and select the `CommunityToolkit.Mvvm` package, which should be the first result.
01. Make sure that at least version 8 is selected. This tutorial was written using version **8.0.0**.
01. Next, select **Install** and accept any prompts that are displayed.

    :::image type="content" source="../media/viewmodel-about/nuget.png" alt-text="Searching for the CommunityToolkit.Mvvm package in NuGet.":::

Now you're ready to start updating the project by adding view models.

## Decouple with view models

The view-to-viewmodel relationship relies heavily on the binding system provided by .NET Multi-platform App UI (.NET MAUI). The app is already using binding in the views to display a list of notes and to present the text and date of a single note. The app logic is currently provided by the view's code-behind and is directly tied to the view. For example, when a user is editing a note and presses the **Save** button, the `Clicked` event for the button is raised. Then, the code-behind for the event handler saves the note text to a file and navigates to the previous screen.

Having app logic in the code-behind of a view can become an issue when the view changes. For example if the button is replaced with a different input control, or the name of a control is changed, event handlers may become invalid. Regardless of how the view is designed, the purpose of the view is to invoke some sort of app logic and to present information to the user. For this app, the `Save` button is saving the note and then navigating back to the previous screen.

The viewmodel gives the app a specific place to put the app logic regardless of how the UI is designed or how the data is being loaded or saved. The viewmodel is the glue that represents and interacts with the data model on behalf of the view.

The view models are stored in a _ViewModels_ folder.

01. Find the **Solution Explorer** pane of Visual Studio.
01. Right-click on the **Notes** project and select **Add** > **New Folder**. Name the folder **ViewModels**.
01. Right-click on the **ViewModels** folder > **Add** > **Class** and name it **AboutViewModel.cs**.
01. Repeat the previous step and create two more view models:
    - **NoteViewModel.cs**
    - **NotesViewModel.cs**

Your project structure should look like the following image:

:::image type="content" source="../media/viewmodel-about/class-structure-1.png" alt-text="Solution explorer showing MVVM folders.":::

## About viewmodel and About view

The **About view** displays some data on the screen and optionally navigates to a website with more information. Since this view doesn't have any data to change, like with a text entry control or selecting items from a list, it's a good candidate to demonstrate adding a viewmodel. For the **About viewmodel**, there isn't a backing model.

Create the **About viewmodel**:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **ViewModels\\AboutViewModel.cs**.
01. Paste in the following code:

    :::code language="csharp" source="../snippets/viewmodel-shared/csharp/Notes/ViewModels/AboutViewModel.cs":::

The previous code snippet contains some properties that represent information about the app, such as the name and version. This snippet is exactly the same as the **About model** you deleted earlier. However, this viewmodel contains a new concept, the `ShowMoreInfoCommand` command property.

Commands are bindable actions that invoke code, and are a great place to put app logic. In this example, the `ShowMoreInfoCommand` points to the `ShowMoreInfo` method, which opens the web browser to a specific page. You'll learn more about the command system in the next section.

### About view

The **About view** needs to be changed slightly to hook it up to the viewmodel that was created in the previous section. In the _Views\\AboutPage.xaml_ file, apply the following changes:

- Update the `xmlns:models` XML namespace to `xmlns:viewModels` and target the `Notes.ViewModels` .NET namespace.
- Change the `ContentPage.BindingContext` property to a new instance of the `About` viewmodel.
- Remove the button's `Clicked` event handler and use the `Command` property.

Update the **About view**:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\AboutPage.xaml**.
01. Paste in the following code:

    :::code language="xaml" source="../snippets/viewmodel-shared/csharp/Notes/Views/AboutPage.xaml" highlight="4,7,19":::

    The previous code snippet highlights the lines that have changed in this version of the view.

Notice that the button is using the `Command` property. Many controls have a `Command` property that is invoked when the user interacts with the control. When used with a button, the command is invoked when a user presses the button, similar to how the `Clicked` event handler is invoked, except that you can bind `Command` to a property in the viewmodel.

In this view, when the user presses the button, the `Command` is invoked. The `Command` is bound to the `ShowMoreInfoCommand` property in the viewmodel, and when invoked, runs the code in the `ShowMoreInfo` method, which opens the web browser to a specific page.

### Clean up the About code-behind

The `ShowMoreInfo` button isn't using the event handler, so the `LearnMore_Clicked` code should be removed from the _Views\\AboutPage.xaml.cs_ file. Delete that code, the class should only contain the constructor:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\AboutPage.xaml.cs**.

    > [!TIP]
    > You may need to expand **Views\\AboutPage.xaml** to show the file.

01. Replace the code with the following snippet:

    :::code language="csharp" source="../snippets/viewmodel-shared/csharp/Notes/Views/AboutPage.xaml.cs":::
