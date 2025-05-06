---
title: Upgrade your app with MVVM concepts
description: Upgrade the Notes app from the previous tutorial with the MVVM Community Toolkit.
ms.date: 03/21/2024
ms.topic: tutorial
---

# Upgrade your app with MVVM concepts

This tutorial is designed to continue the [Create a .NET MAUI app](./notes-app.md) tutorial, which created a note taking app. In this tutorial, you'll learn how to:

> [!div class="checklist"]
>
> - Implement the model-view-viewmodel (MVVM) pattern.
> - Use an additional style of query string for passing data during navigation.

We suggest that you first follow the [Create a .NET MAUI app](./notes-app.md) tutorial, as the code created in that tutorial is the basis for this tutorial. If you lost the code, or want to start fresh, download [this project](https://github.com/dotnet/maui-samples/raw/main/8.0/Tutorials/ConvertToMvvm/app_before.zip).

## Understand MVVM

The .NET MAUI developer experience typically involves creating a user interface in XAML, and then adding code-behind that operates on the user interface. Complex maintenance issues can arise as apps are modified and grow in size and scope. These issues include the tight coupling between the UI controls and the business logic, which increases the cost of making UI modifications, and the difficulty of unit testing such code.

The model-view-viewmodel (MVVM) pattern helps cleanly separate an app's business and presentation logic from its user interface (UI). Maintaining a clean separation between app logic and the UI helps address numerous development issues and makes an app easier to test, maintain, and evolve. It can also significantly improve code reuse opportunities and allows developers and UI designers to collaborate more easily when developing their respective parts of an app.

### The pattern

There are three core components in the MVVM pattern: the model, the view, and the view model. Each serves a distinct purpose. The following diagram shows the relationships between the three components.

:::image type="content" source="./media/notes-mvvm/mvvm/mvvm-pattern.png" alt-text="A diagram demonstrating the parts of an MVVM-modeled application":::

In addition to understanding the responsibilities of each component, it's also important to understand how they interact. At a high level, the view "knows about" the view model, and the view model "knows about" the model, but the model is unaware of the view model, and the view model is unaware of the view. Therefore, the view model isolates the view from the model, and allows the model to evolve independently of the view.

The key to using MVVM effectively lies in understanding how to factor app code into the correct classes and how the classes interact.

#### View

The view is responsible for defining the structure, layout, and appearance of what the user sees on screen. Ideally, each view is defined in XAML, with a limited code-behind that doesn't contain business logic. However, in some cases, the code-behind might contain UI logic that implements visual behavior that is difficult to express in XAML, such as animations.

#### ViewModel

The view model implements properties and commands to which the view can data bind to, and notifies the view of any state changes through change notification events. The properties and commands that the view model provides define the functionality to be offered by the UI, but the view determines how that functionality is to be displayed.

The view model is also responsible for coordinating the view's interactions with any model classes that are required. There's typically a one-to-many relationship between the view model and the model classes.

Each view model provides data from a model in a form that the view can easily consume. To accomplish this, the view model sometimes performs data conversion. Placing this data conversion in the view model is a good idea because it provides properties that the view can bind to. For example, the view model might combine the values of two properties to make it easier to display by the view.

> [!IMPORTANT]
> .NET MAUI marshals binding updates to the UI thread. When using MVVM this enables you to update data-bound viewmodel properties from any thread, with .NET MAUI's binding engine bringing the updates to the UI thread.

#### Model

Model classes are non-visual classes that encapsulate the app's data. Therefore, the model can be thought of as representing the app's domain model, which usually includes a data model along with business and validation logic.

### Update the model

In this first part of the tutorial, you'll implement the model-view-viewmodel (MVVM) pattern. To start, open the _Notes.sln_ solution in Visual Studio.

### Clean up the model

In the previous tutorial, the model types were acting both as the model (data) and as a view model (data preparation), which was mapped directly to a view. The following table describes the model:

| Code file            | Description                                                                   |
|----------------------|-------------------------------------------------------------------------------|
| _Models/About.cs_    | The `About` model. Contains read-only fields that describe the app itself, such as the app title and version. |
| _Models/Note.cs_     | The `Note` model. Represents a note.                                          |
| _Models/AllNotes.cs_ | The `AllNotes` model. Loads all of the notes on the device into a collection. |

Thinking about the app itself, there is only one piece of data that is used by the app, the `Note`. Notes are loaded from the device, saved to the device, and edited through the app UI. There really isn't a need for the `About` and `AllNotes` models. Remove these models from the project:

01. Find the **Solution Explorer** pane of Visual Studio.
01. Right-click on the **Models\\About.cs** file and select **Delete**. Press **OK** to delete the file.
01. Right-click on the **Models\\AllNotes.cs** file and select **Delete**. Press **OK** to delete the file.

The only model file remaining is the **Models\\Note.cs** file.

### Update the model

The `Note` model contains:

- A unique identifier, which is the file name of the note as stored on the device.
- The text of the note.
- A date to indicate when the note was created or last updated.

Currently, loading and saving the model was done through the views, and in some cases, by the other model types that you just removed. The code you have for the `Note` type should be the following:

```csharp
namespace Notes.Models;

internal class Note
{
    public string Filename { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }
}
```

The `Note` model is going to be expanded to handle loading, saving, and deleting notes.

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Models\\Note.cs**.
01. In the code editor, add the following two methods to the `Note` class. These methods are instance-based and handle saving or deleting the current note to or from the device, respectively:

    :::code language="csharp" source="./snippets/notes-mvvm/bugs/Models/Note.cs" id="save_delete":::

01. The app needs to load notes in two ways, loading an individual note from a file and loading all notes on the device. The code to handle loading can be `static` members, not requiring a class instance to run.

    Add the following code to the class to load a note by file name:

    :::code language="csharp" source="./snippets/notes-mvvm/bugs/Models/Note.cs" id="load_single":::

    This code takes the file name as a parameter, builds the path to where the notes are stored on the device, and attempts to load the file if it exists.

01. The second way to load notes is to enumerate all notes on the device and load them into a collection.

    Add the following code to the class:

    :::code language="csharp" source="./snippets/notes-mvvm/bugs/Models/Note.cs" id="load_all":::

    This code returns an enumerable collection of `Note` model types by retrieving the files on the device that match the notes file pattern: _*.notes.txt_. Each file name is passed to the `Load` method, loading an individual note. Finally, the collection of notes is ordered by the date of each note and returned to the caller.

01. Lastly, add a constructor to the class which sets the default values for the properties, including a random file name:

    :::code language="csharp" source="./snippets/notes-mvvm/bugs/Models/Note.cs" id="ctor":::

The `Note` class code should look like the following:

:::code language="csharp" source="./snippets/notes-mvvm/bugs/Models/Note.cs" id="full":::

Now that the `Note` model is complete, the view models can be created.

## Create the About viewmodel

Before adding view models to the project, add a reference to the MVVM Community Toolkit. This library is available on NuGet, and provides types and systems that help implement the MVVM pattern.

01. In the **Solution Explorer** pane of Visual Studio, right-click on the **Notes** project > **Manage NuGet Packages**.
01. Select the **Browse** tab.
01. Search for **communitytoolkit mvvm** and select the `CommunityToolkit.Mvvm` package, which should be the first result.
01. Make sure that at least version 8 is selected. This tutorial was written using version **8.0.0**.
01. Next, select **Install** and accept any prompts that are displayed.

    :::image type="content" source="./media/notes-mvvm/viewmodel-about/nuget.png" alt-text="Searching for the CommunityToolkit.Mvvm package in NuGet.":::

Now you're ready to start updating the project by adding view models.

### Decouple with view models

The view-to-viewmodel relationship relies heavily on the binding system provided by .NET Multi-platform App UI (.NET MAUI). The app is already using binding in the views to display a list of notes and to present the text and date of a single note. The app logic is currently provided by the view's code-behind and is directly tied to the view. For example, when a user is editing a note and presses the **Save** button, the `Clicked` event for the button is raised. Then, the code-behind for the event handler saves the note text to a file and navigates to the previous screen.

Having app logic in the code-behind of a view can become an issue when the view changes. For example, if the button is replaced with a different input control, or the name of a control is changed, event handlers might become invalid. Regardless of how the view is designed, the purpose of the view is to invoke some sort of app logic and to present information to the user. For this app, the `Save` button is saving the note and then navigating back to the previous screen.

The viewmodel gives the app a specific place to put the app logic regardless of how the UI is designed or how the data is being loaded or saved. The viewmodel is the glue that represents and interacts with the data model on behalf of the view.

The view models are stored in a _ViewModels_ folder.

01. Find the **Solution Explorer** pane of Visual Studio.
01. Right-click on the **Notes** project and select **Add** > **New Folder**. Name the folder **ViewModels**.
01. Right-click on the **ViewModels** folder > **Add** > **Class** and name it **AboutViewModel.cs**.
01. Repeat the previous step and create two more view models:
    - **NoteViewModel.cs**
    - **NotesViewModel.cs**

Your project structure should look like the following image:

:::image type="content" source="./media/notes-mvvm/viewmodel-about/class-structure-1.png" alt-text="Solution explorer showing MVVM folders.":::

### About viewmodel and About view

The **About view** displays some data on the screen and optionally navigates to a website with more information. Since this view doesn't have any data to change, like with a text entry control or selecting items from a list, it's a good candidate to demonstrate adding a viewmodel. For the **About viewmodel**, there isn't a backing model.

Create the **About viewmodel**:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **ViewModels\\AboutViewModel.cs**.
01. Paste in the following code:

    :::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/ViewModels/AboutViewModel.cs":::

The previous code snippet contains some properties that represent information about the app, such as the name and version. This snippet is exactly the same as the **About model** you deleted earlier. However, this viewmodel contains a new concept, the `ShowMoreInfoCommand` command property.

Commands are bindable actions that invoke code, and are a great place to put app logic. In this example, the `ShowMoreInfoCommand` points to the `ShowMoreInfo` method, which opens the web browser to a specific page. You'll learn more about the command system in the next section.

#### About view

The **About view** needs to be changed slightly to hook it up to the viewmodel that was created in the previous section. In the _Views\\AboutPage.xaml_ file, apply the following changes:

- Update the `xmlns:models` XML namespace to `xmlns:viewModels` and target the `Notes.ViewModels` .NET namespace.
- Change the `ContentPage.BindingContext` property to a new instance of the `About` viewmodel.
- Remove the button's `Clicked` event handler and use the `Command` property.

Update the **About view**:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\AboutPage.xaml**.
01. Paste in the following code:

    :::code language="xaml" source="./snippets/notes-mvvm/viewmodel-shared/Views/AboutPage.xaml" highlight="4,6,8,20":::

    The previous code snippet highlights the lines that have changed in this version of the view.

Notice that the button is using the `Command` property. Many controls have a `Command` property that is invoked when the user interacts with the control. When used with a button, the command is invoked when a user presses the button, similar to how the `Clicked` event handler is invoked, except that you can bind `Command` to a property in the viewmodel.

In this view, when the user presses the button, the `Command` is invoked. The `Command` is bound to the `ShowMoreInfoCommand` property in the viewmodel, and when invoked, runs the code in the `ShowMoreInfo` method, which opens the web browser to a specific page.

### Clean up the About code-behind

The `ShowMoreInfo` button isn't using the event handler, so the `LearnMore_Clicked` code should be removed from the _Views\\AboutPage.xaml.cs_ file. Delete that code, the class should only contain the constructor:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\AboutPage.xaml.cs**.

    > [!TIP]
    > You might need to expand **Views\\AboutPage.xaml** to show the file.

01. Replace the code with the following snippet:

    :::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/Views/AboutPage.xaml.cs":::

## Create the Note viewmodel

The goal of updating the **Note view** is to move as much functionality as possible out of the XAML code-behind and put it in the **Note viewmodel**.

### Note viewmodel

Based on what the **Note view** requires, the **Note viewmodel** needs to provide the following items:

- The text of the note.
- The date/time the note was created or last updated.
- A command that saves the note.
- A command that deletes the note.

Create the **Note viewmodel**:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **ViewModels\\NoteViewModel.cs**.
01. Replace the code in this file with the following snippet:

    ```csharp
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.ComponentModel;
    using System.Windows.Input;

    namespace Notes.ViewModels;

    internal class NoteViewModel : ObservableObject, IQueryAttributable
    {
        private Models.Note _note;

    }
    ```

    This code is the blank `Note` viewmodel where you'll add properties and commands to support the `Note` view. Notice that the `CommunityToolkit.Mvvm.ComponentModel` namespace is being imported. This namespace provides the `ObservableObject` used as the base class. You'll learn more about `ObservableObject` in the next step. The `CommunityToolkit.Mvvm.Input` namespace is also imported. This namespace provides some command-types that invoke methods asynchronously.

    The `Models.Note` model is being stored as a private field. The properties and methods of this class will use this field.

01. Add the following properties to the class:

    :::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/ViewModels/NoteViewModel.cs" id="properties":::

    The `Date` and `Identifier` properties are simple properties that just retrieve the corresponding values from the model.

    > [!TIP]
    > For properties, the `=>` syntax creates a get-only property where the statement to the right of `=>` must evaluate to a value to return.

    The `Text` property first checks if the value being set is a different value. If the value is different, that value is passed on to the model's property, and the `OnPropertyChanged` method is called.

    The `OnPropertyChanged` method is provided by the `ObservableObject` base class. This method uses the name of the calling code, in this case, the property name of **Text**, and raises the `ObservableObject.PropertyChanged` event. This event supplies the name of the property to any event subscribers. The binding system provided by .NET MAUI recognizes this event, and updates any related bindings in the UI. For the **Note viewmodel**, when the `Text` property changes, the event is raised, and any UI element that is bound to the `Text` property is notified that the property changed.

01. Add the following command-properties to the class, which are the commands that the view can bind to:

    :::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/ViewModels/NoteViewModel.cs" id="commands":::

01. Add the following constructors to the class:

    :::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/ViewModels/NoteViewModel.cs" id="ctor":::

    These two constructors are used to either create the viewmodel with a new backing model, which is an empty note, or to create a viewmodel that uses the specified model instance.

    The constructors also setup the commands for the viewmodel. Next, add the code for these commands.

01. Add the `Save` and `Delete` methods:

    :::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/ViewModels/NoteViewModel.cs" id="command_methods":::

    These methods are invoked by associated commands. They perform the related actions on the model and make the app navigate to the previous page. A query string parameter is added to the `..` navigation path, indicating which action was taken and the note's unique identifier.

01. Next, add the `ApplyQueryAttributes` method to the class, which satisfies the requirements of the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface:

    :::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/ViewModels/NoteViewModel.cs" id="iquery":::

    When a page, or the binding context of a page, implements this interface, the query string parameters used in navigation are passed to the `ApplyQueryAttributes` method. This viewmodel is used as the binding context for the **Note view**. When the **Note view** is navigated to, the view's binding context (this viewmodel) is passed the query string parameters used during navigation.

    This code checks if the `load` key was provided in the `query` dictionary. If this key is found, the value should be the identifier (the file name) of the note to load. That note is loaded and set as the underlying model object of this viewmodel instance.

01. Finally, add these two helper methods to the class:

    :::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/ViewModels/NoteViewModel.cs" id="helpers":::

    The `Reload` method is a helper method that refreshes the backing model object, reloading it from device storage

    The `RefreshProperties` method is another helper method to ensure that any subscribers bound to this object are notified that the `Text` and `Date` properties have changed. Since the underlying model (the `_note` field) is changed when the note is loaded during navigation, the `Text` and `Date` properties aren't actually set to new values. Since these properties aren't directly set, any bindings attached to those properties wouldn't be notified because `OnPropertyChanged` isn't called for each property. `RefreshProperties` ensures bindings to these properties are refreshed.

The code for the class should look like the following snippet:

:::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/ViewModels/NoteViewModel.cs" id="full":::

### Note view

Now that the viewmodel has been created, update the **Note view**. In the _Views\\NotePage.xaml_ file, apply the following changes:

- Add the `xmlns:viewModels` XML namespace that targets the `Notes.ViewModels` .NET namespace.
- Add a `BindingContext` to the page.
- Remove the delete and save button `Clicked` event handlers and replace them with commands.

Update the **Note view**:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\NotePage.xaml** to open the XAML editor.
01. Paste in the following code:

    :::code language="xaml" source="./snippets/notes-mvvm/viewmodel-shared/Views/NotePage.xaml" highlight="4,7,8-10,19,23":::

Previously, this view didn't declare a binding context, as it was supplied by the code-behind of the page itself. Setting the binding context directly in the XAML provides two things:

- At run time, when the page is navigated to, it displays a blank note. This is because the parameterless constructor for the binding context, the viewmodel, is invoked. If you remember correctly, the parameterless constructor for the **Note viewmodel** creates a blank note.

- The intellisense in the XAML editor shows the available properties as soon as you start typing `{Binding` syntax. The syntax is also validated and alerts you of an invalid value. Try changing the binding syntax for the `SaveCommand` to `Save123Command`. If you hover the mouse cursor over the text, you'll notice that a tooltip is displayed informing you that **Save123Command** isn't found. This notification isn't considered an error because bindings are dynamic, it's really a small warning that might help you notice when you typed the wrong property.

  If you changed the **SaveCommand** to a different value, restore it now.

### Clean up the Note code-behind

Now that the interaction with the view has changed from event handlers to commands, open the _Views\\NotePage.xaml.cs_ file and replace all the code with a class that only contains the constructor:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\NotePage.xaml.cs**.

    > [!TIP]
    > You might need to expand **Views\\NotePage.xaml** to show the file.

01. Replace the code with the following snippet:

    :::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/Views/NotePage.xaml.cs":::

## Create the Notes viewmodel

The final viewmodel-view pair is the **Notes viewmodel** and **AllNotes view**. Currently though, the view is binding directly to the model, which was deleted at the start of this tutorial. The goal in updating the **AllNotes view** is to move as much functionality as possible out of the XAML code-behind and put it in the viewmodel. Again, the benefit being that the view can change its design with little effect to your code.

### Notes viewmodel

Based on what the **AllNotes view** is going to display and what interactions the user will do, the **Notes viewmodel** must provide the following items:

- A collection of notes.
- A command to handle navigating to a note.
- A command to create a new note.
- Update the list of notes when one is created, deleted, or changed.

Create the **Notes viewmodel**:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **ViewModels\\NotesViewModel.cs**.
01. Replace the code in this file with the following code:

    ```csharp
    using CommunityToolkit.Mvvm.Input;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    namespace Notes.ViewModels;

    internal class NotesViewModel: IQueryAttributable
    {
    }
    ```

    This code is the blank `NotesViewModel` where you'll add properties and commands to support the `AllNotes` view.

01. In the `NotesViewModel` class code, add the following properties:

    :::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/ViewModels/NotesViewModel.cs" id="properties":::

    The `AllNotes` property is an `ObservableCollection` that stores all of the notes loaded from the device. The two commands will be used by the view to trigger the actions of creating a note or selecting an existing note.

01. Add a parameterless constructor to the class, which initializes the commands and loads the notes from the model:

    :::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/ViewModels/NotesViewModel.cs" id="ctor":::

    Notice that the `AllNotes` collection uses the `Models.Note.LoadAll` method to fill the observable collection with notes. The `LoadAll` method returns the notes as the `Models.Note` type, but the observable collection is a collection of `ViewModels.NoteViewModel` types. The code uses the `Select` Linq extension to create viewmodel instances from the note models returned from `LoadAll`.

01. Create the methods targeted by the commands:

    :::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/ViewModels/NotesViewModel.cs" id="commands":::

    Notice that the `NewNoteAsync` method doesn't take a parameter while the `SelectNoteAsync` does. Commands can optionally have a single parameter that is provided when the command is invoked. For the `SelectNoteAsync` method, the parameter represents the note that's being selected.

01. Finally, implement the `IQueryAttributable.ApplyQueryAttributes` method:

    :::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/ViewModels/NotesViewModel.cs" id="query":::

    The **Note viewmodel** created in the previous tutorial step, used navigation when the note was saved or deleted. The viewmodel navigated back to the **AllNotes view**, which this viewmodel is associated with. This code detects if the query string contains either the `deleted` or `saved` key. The value of the key is the unique identifier of the note.

    If the note was **deleted**, that note is matched in the `AllNotes` collection by the provided identifier, and removed.

    There are two possible reasons a note is **saved**. The note was either just created or an existing note was changed. If the note is already in the `AllNotes` collection, it's a note that was updated. In this case, the note instance in the collection just needs to be refreshed. If the note is missing from the collection, it's a new note and must be added to the collection.

The code for the class should look like the following snippet:

:::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/ViewModels/NotesViewModel.cs" id="full":::

### AllNotes view

Now that the viewmodel has been created, update the **AllNotes view** to point to the viewmodel properties. In the _Views\\AllNotesPage.xaml_ file, apply the following changes:

- Add the `xmlns:viewModels` XML namespace that targets the `Notes.ViewModels` .NET namespace.
- Add a `BindingContext` to the page.
- Remove the toolbar button's `Clicked` event and use the `Command` property.
- Change the `CollectionView` to bind its `ItemSource` to `AllNotes`.
- Change the `CollectionView` to use commanding to react to when the selected item changes.

Update the **AllNotes view**:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\AllNotesPage.xaml**.
01. Paste in the following code:

    :::code language="xaml" source="./snippets/notes-mvvm/viewmodel-shared/Views/AllNotesPage.xaml" id="full" highlight="3,6,8,13,18,21-22":::

The toolbar no longer uses the `Clicked` event and instead uses a command.

The `CollectionView` supports commanding with the `SelectionChangedCommand` and `SelectionChangedCommandParameter` properties. In the updated XAML, the `SelectionChangedCommand` property is bound to the viewmodel's `SelectNoteCommand`, which means the command is invoked when the selected item changes. When the command is invoked, the `SelectionChangedCommandParameter` property value is passed to the command.

Look at the binding used for the `CollectionView`:

:::code language="xaml" source="./snippets/notes-mvvm/viewmodel-shared/Views/AllNotesPage.xaml" id="collection_view" highlight="5-6":::

The `SelectionChangedCommandParameter` property uses `Source={RelativeSource Self}` binding. The `Self` references the current object, which is the `CollectionView`. Therefore, `x:DataType` specifies `CollectionView` as the type for the compiled binding. Notice that the binding path is the `SelectedItem` property. When the command is invoked by changing the selected item, the `SelectNoteCommand` command is invoked and the selected item is passed to the command as a parameter.

For the binding expression defined in the `SelectionChangedCommandParameter` property to compile, it's necessary to instruct the project to enable compiled bindings in expressions that specify the `Source` property. To do this edit the project file for your solution and add `<MauiEnableXamlCBindingWithSourceCompilation>true</MauiEnableXamlCBindingWithSourceCompilation>` within the `<PropertyGroup>` element:

```xml
<PropertyGroup>
  <MauiEnableXamlCBindingWithSourceCompilation>true</MauiEnableXamlCBindingWithSourceCompilation>
</PropertyGroup>
```

### Clean up the AllNotes code-behind

Now that the interaction with the view has changed from event handlers to commands, open the _Views\\AllNotesPage.xaml.cs_ file and replace all the code with a class that only contains the constructor:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\AllNotesPage.xaml.cs**.

    > [!TIP]
    > You might need to expand **Views\\AllNotesPage.xaml** to show the file.

01. Replace the code with the following snippet:

    :::code language="csharp" source="./snippets/notes-mvvm/viewmodel-shared/Views/AllNotesPage.xaml.cs":::

### Run the app

You can now run the app, and everything is working. However, there are two problems with how the app behaves:

- If you select a note, which opens the editor, press **Save**, and then try to select the same note, it doesn't work.
- Whenever a note is changed or added, the list of notes isn't reordered to show the latest notes at the top.

These two problems are fixed in the next tutorial step.

## Fix the app behavior

Now that the app code can compile and run, you'll likely have noticed that there are two flaws with how the app behaves. The app doesn't let you reselect a note that's already selected, and the list of notes isn't reordered after a note is created or changed.

### Get notes to the top of the list

First, fix the reordering problem with the notes list. In the _ViewModels\\NotesViewModel.cs_ file, the `AllNotes` collection contains all of the notes to be presented to the user. Unfortunately, the downside to using an `ObservableCollection` is that it must be manually sorted. To get the new or updated items to the top of the list, perform the following steps:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **ViewModels\\NotesViewModel.cs**.
01. In the `ApplyQueryAttributes` method, look at the logic for the **saved** query string key.
01. When the `matchedNote` isn't `null`, the note is being updated. Use the `AllNotes.Move` method to move the `matchedNote` to index 0, which is the top of the list.

    :::code language="csharp" source="./snippets/notes-mvvm/bugs/ViewModels/NotesViewModel.cs" id="move" highlight="8":::

    The `AllNotes.Move` method takes two parameters to move an object's position in the collection. The first parameter is the index of the object that to move, and the second parameters is the index of where to move the object. The `AllNotes.IndexOf` method retrieves the index of the note.

01. When the `matchedNote` is `null`, the note is new and is being added to the list. Instead of adding it, which appends the note to the end of the list, insert the note at index 0, which is the top of the list. Change the `AllNotes.Add` method to `AllNotes.Insert`.

    :::code language="csharp" source="./snippets/notes-mvvm/bugs/ViewModels/NotesViewModel.cs" id="insert" highlight="12":::

The `ApplyQueryAttributes` method should look like the following code snippet:

:::code language="csharp" source="./snippets/notes-mvvm/bugs/ViewModels/NotesViewModel.cs" id="query":::

### Allow selecting a note twice

In the **AllNotes view**, the `CollectionView` lists all of the notes, but doesn't allow you to select the same note twice. There are two ways the item remains selected: when the user changes an existing note, and when the user forcibly navigates backwards. The case where the user saves a note is fixed with the code change in previous section that uses `AllNotes.Move`, so you don't have to worry about that case.

The problem you have to solve now is related to navigation. No matter how the **Allnotes view** is navigated to, the `NavigatedTo` event is raised for the page. This event is a perfect place to forcibly unselect the selected item in the `CollectionView`.

However, with the MVVM pattern being applied here, the viewmodel can't trigger something directly on the view, such as clearing the selected item after the note is saved. So how do you get that to happen? A good implementation of the MVVM pattern minimizes the code-behind in the view. There are a few different ways to solve this problem to support the MVVM separation pattern. However, it's also OK to put code in the code-behind of the view, especially when it's directly tied to the view. MVVM has many great designs and concepts that help you compartmentalize your app, improving maintainability and making it easier for you to add new features. However, in some cases, you might find that MVVM encourages overengineering.

Don't overengineer a solution for this problem, and just use the `NavigatedTo` event to clear the selected item from the `CollectionView`.

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\AllNotesPage.xaml**.
01. In the XAML for the `<ContentPage>`, add the `NavigatedTo` event:

    :::code language="xaml" source="./snippets/notes-mvvm/bugs/Views/AllNotesPage.xaml" range="1-9" highlight="6":::

01. You can add a default event handler by right-clicking on the event method name, `ContentPage_NavigatedTo`, and selecting **Go To Definition**. This action opens the _Views\\AllNotesPage.xaml.cs_ in the code editor.

01. Replace the event handler code with the following snippet:

    :::code language="csharp" source="./snippets/notes-mvvm/bugs/Views/AllNotesPage.xaml.cs" id="event":::

    In the XAML, the `CollectionView` was given the name of `notesCollection`. This code uses that name to access the `CollectionView`, and set `SelectedItem` to `null`. The selected item is cleared every time the page is navigated to.

Now, run your app. Try to navigate to a note, press the back button, and select the same note a second time. The app behavior is fixed!

[![Explore the code.](~/media/code-sample.png) Explore the code for this tutorial.](https://github.com/dotnet/maui-samples/tree/main/8.0/Tutorials/ConvertToMvvm/code). If you want to download a copy of the completed project to compare your code with, download [this project](https://github.com/dotnet/maui-samples/raw/main/8.0/Tutorials/ConvertToMvvm/app_after.zip).

Your app is now using MVVM patterns!

## Next steps

The following links provide more information related to some of the concepts you learned in this tutorial:

- [Enterprise application patterns Using .NET MAUI - MVVM](/dotnet/architecture/maui/mvvm)
- [Data binding and MVVM](../xaml/fundamentals/mvvm.md)
- [ApplyQueryAttributes - Process navigation data using a single method](../fundamentals/shell/navigation.md#process-navigation-data-using-a-single-method)
- [ObservableObject - How it works](/dotnet/communitytoolkit/mvvm/observableobject)
