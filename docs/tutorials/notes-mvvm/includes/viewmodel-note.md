---
author: adegeo
ms.author: adegeo
ms.date: 12/12/2022
ms.topic: include
no-loc: ["communitytoolkit", "CommunityToolkit.Mvvm", "AllNotes", "Notes", "About"]
---

The final viewmodel-view pair is the **Note viewmodel** and **Note view**. Again, the goal in updating the **Note view** is to move as much functionality as possible out of the XAML code-behind and put it in the viewmodel.

## Note viewmodel

Based on what the **Note view** requires, the **Note viewmodel** needs to provide the following items:

- The text of the note.
- The date/time the note was created or last updated.
- A command to that saves the note.
- A command to that deletes the note.

Create the **Note viewmodel**:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **ViewModels\\Note.cs**.
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

    This code is the blank `Note` viewmodel where you'll add properties and commands to support the `Note` view. Notice that the `CommunityToolkit.Mvvm.ComponentModel` namespace is being imported. This namespace provides the `ObservableObject` used as the base class. You'll learn more about `ObservableObject` in the next step.

    The `Models.Note` model is being stored as a private field, the next step is to map public properties to the model.

01. Add the following properties to the class:

    :::code language="csharp" source="../snippets/viewmodel-shared/csharp/Notes/ViewModels/NoteViewModel.cs" id="properties":::

    The `Date` and `Identifier` properties are simple properties that just retrieve the corresponding values from the model.

    > [!TIP]
    > For properties, the `=>` syntax creates a `get`-only property where the statement to the right of `=>` must evaluate to a value to return.

    The `Text` property first checks if the value being set is a different value. If the value is different, it's set to the property and the `OnPropertyChanged` method is called.

    The `OnPropertyChanged` method is provided by the `ObservableObject` base class. This method uses the name of the calling code, in this case the property name of **Text**, and raises the `ObservableObject.PropertyChanged` event. This event supplies the name of the property to any event subscribers. The binding system provided by .NET MAUI recognizes this event, and updates any related bindings in the UI. In the example of this code, when the `Text` property changes, the event is raised, and any UI element that is bound to the `Text` property is notified that the property changed.

01. Add the following command-properties to the class, which are the commands that the view can bind to:

    :::code language="csharp" source="../snippets/viewmodel-shared/csharp/Notes/ViewModels/NoteViewModel.cs" id="commands":::

01. Add the following constructors to the class:

    :::code language="csharp" source="../snippets/viewmodel-shared/csharp/Notes/ViewModels/NoteViewModel.cs" id="ctor":::

    These two constructors are used to either create the viewmodel as an empty note, or to load the viewmodel from the supplied model. The constructors also use the `SetupCommands` method, which you'll add next.

01. Create the `SetupCommands` method, along with the methods that the commands will reference:

    :::code language="csharp" source="../snippets/viewmodel-shared/csharp/Notes/ViewModels/NoteViewModel.cs" id="command_methods":::

    These commands invoke the specified action on the underlying model, and navigates to the previous page. A query string parameter is added to the `..` path, indicating which action was taken and the note's unique identifier.

01. Finally, add the following methods to the class:

    :::code language="csharp" source="../snippets/viewmodel-shared/csharp/Notes/ViewModels/NoteViewModel.cs" id="methods":::

    The `ApplyQueryAttributes` method satisfies the `IQueryAttributable` interface that is declared on the class. When a page or the binding context of a page implements this interface, the query parameters used in navigation are passed to the `ApplyQueryAttributes` method. This viewmodel is used as the binding context for the **Note view**. When the **Note view** is navigated to, that binding context (this viewmodel) is passed the query string parameters used during navigation.

    This code checks if the `load` key was provided in the `query` dictionary. If this key is found, the value should be the identifier (the file name) of the note to load. That note is loaded and set as the underlying model object of this viewmodel instance.

    The `Reload` method is a helper method that refreshes the backing model object.

    The `RefreshProperties` method is another helper method to ensure that any subscribers bound to this object are notified that the `Text` and `Date` properties have changed. Since the underlying model (the `_note` field) is changed when the note is loaded during navigation, the `Text` and `Date` properties aren't actually set to new values. Since these properties aren't directly set, any bindings attached to those properties wouldn't be notified because `OnPropertyChanged` isn't called for each property. `RefreshProperties` ensures bindings to these properties are refreshed.

The code for the class should look like the following snippet:

:::code language="csharp" source="../snippets/viewmodel-shared/csharp/Notes/ViewModels/NoteViewModel.cs" id="full":::

## Note view

Now that the viewmodel has been created, update the **Note view**. In the _Views\\NotePage.xaml_ file, apply the following changes:

- Add the `xmlns:viewModels` XML namespace that targets the `Notes.ViewModels` .NET namespace.
- Add a `BindingContext` to the page.
- Remove the delete and save button `Clicked` event handlers and replace them with commands.

Create the **Note view**:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\NotePage.xaml** to open the XAML editor.
01. Paste in the following code:

:::code language="xaml" source="../snippets/viewmodel-shared/csharp/Notes/Views/NotePage.xaml" highlight="4,7-9,18,22":::

Previously, this view didn't declare a binding context, as it was supplied by the code-behind of the page itself. Setting the binding context directly in the XAML provides two things:

- At run-time, when the page is navigated to, it displays a blank note. This is because the default constructor for the note viewmodel is invoked, not the constructor that loads a note viewmodel from a model.

- The intellisense in the XAML editor shows the available properties as soon as you start typing `{Binding` syntax. The syntax is also validated and alerts you of an invalid value. Try changing the binding syntax for the `SaveCommand` to `Save123Command`. If you hover the mouse cursor over the text, you'll notice that a tooltip is displayed informing you that **Save123Command** isn't found. This notification isn't considered an error because bindings are dynamic, it's really a small warning that may help you notice when you typed the wrong property.

## Clean up the Note code-behind

Now that the interaction with the view has changed from event handlers to commands, open the _Views\\NotePage.xaml.cs_ file and replace all the code with the following snippet:

:::code language="csharp" source="../snippets/viewmodel-shared/csharp/Notes/Views/NotePage.xaml.cs":::
