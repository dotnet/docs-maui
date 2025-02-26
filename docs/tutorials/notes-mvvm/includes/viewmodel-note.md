---
author: adegeo
ms.author: adegeo
ms.date: 03/21/2024
ms.topic: include
no-loc: ["Note viewmodel", "Note view", "ViewModels", "NoteViewModel.cs", "NotesViewModel.cs"]
---

The goal of updating the **Note view** is to move as much functionality as possible out of the XAML code-behind and put it in the **Note viewmodel**.

## Note viewmodel

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

    :::code language="csharp" source="../snippets/viewmodel-shared/ViewModels/NoteViewModel.cs" id="properties":::

    The `Date` and `Identifier` properties are simple properties that just retrieve the corresponding values from the model.

    > [!TIP]
    > For properties, the `=>` syntax creates a get-only property where the statement to the right of `=>` must evaluate to a value to return.

    The `Text` property first checks if the value being set is a different value. If the value is different, that value is passed on to the model's property, and the `OnPropertyChanged` method is called.

    The `OnPropertyChanged` method is provided by the `ObservableObject` base class. This method uses the name of the calling code, in this case, the property name of **Text**, and raises the `ObservableObject.PropertyChanged` event. This event supplies the name of the property to any event subscribers. The binding system provided by .NET MAUI recognizes this event, and updates any related bindings in the UI. For the **Note viewmodel**, when the `Text` property changes, the event is raised, and any UI element that is bound to the `Text` property is notified that the property changed.

01. Add the following command-properties to the class, which are the commands that the view can bind to:

    :::code language="csharp" source="../snippets/viewmodel-shared/ViewModels/NoteViewModel.cs" id="commands":::

01. Add the following constructors to the class:

    :::code language="csharp" source="../snippets/viewmodel-shared/ViewModels/NoteViewModel.cs" id="ctor":::

    These two constructors are used to either create the viewmodel with a new backing model, which is an empty note, or to create a viewmodel that uses the specified model instance.

    The constructors also setup the commands for the viewmodel. Next, add the code for these commands.

01. Add the `Save` and `Delete` methods:

    :::code language="csharp" source="../snippets/viewmodel-shared/ViewModels/NoteViewModel.cs" id="command_methods":::

    These methods are invoked by associated commands. They perform the related actions on the model and make the app navigate to the previous page. A query string parameter is added to the `..` navigation path, indicating which action was taken and the note's unique identifier.

01. Next, add the `ApplyQueryAttributes` method to the class, which satisfies the requirements of the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface:

    :::code language="csharp" source="../snippets/viewmodel-shared/ViewModels/NoteViewModel.cs" id="iquery":::

    When a page, or the binding context of a page, implements this interface, the query string parameters used in navigation are passed to the `ApplyQueryAttributes` method. This viewmodel is used as the binding context for the **Note view**. When the **Note view** is navigated to, the view's binding context (this viewmodel) is passed the query string parameters used during navigation.

    This code checks if the `load` key was provided in the `query` dictionary. If this key is found, the value should be the identifier (the file name) of the note to load. That note is loaded and set as the underlying model object of this viewmodel instance.

01. Finally, add these two helper methods to the class:

    :::code language="csharp" source="../snippets/viewmodel-shared/ViewModels/NoteViewModel.cs" id="helpers":::

    The `Reload` method is a helper method that refreshes the backing model object, reloading it from device storage

    The `RefreshProperties` method is another helper method to ensure that any subscribers bound to this object are notified that the `Text` and `Date` properties have changed. Since the underlying model (the `_note` field) is changed when the note is loaded during navigation, the `Text` and `Date` properties aren't actually set to new values. Since these properties aren't directly set, any bindings attached to those properties wouldn't be notified because `OnPropertyChanged` isn't called for each property. `RefreshProperties` ensures bindings to these properties are refreshed.

The code for the class should look like the following snippet:

:::code language="csharp" source="../snippets/viewmodel-shared/ViewModels/NoteViewModel.cs" id="full":::

## Note view

Now that the viewmodel has been created, update the **Note view**. In the _Views\\NotePage.xaml_ file, apply the following changes:

- Add the `xmlns:viewModels` XML namespace that targets the `Notes.ViewModels` .NET namespace.
- Add a `BindingContext` to the page.
- Remove the delete and save button `Clicked` event handlers and replace them with commands.

Update the **Note view**:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\NotePage.xaml** to open the XAML editor.
01. Paste in the following code:

    :::code language="xaml" source="../snippets/viewmodel-shared/Views/NotePage.xaml" highlight="4,7,8-10,19,23":::

Previously, this view didn't declare a binding context, as it was supplied by the code-behind of the page itself. Setting the binding context directly in the XAML provides two things:

- At run-time, when the page is navigated to, it displays a blank note. This is because the parameterless constructor for the binding context, the viewmodel, is invoked. If you remember correctly, the parameterless constructor for the **Note viewmodel** creates a blank note.

- The intellisense in the XAML editor shows the available properties as soon as you start typing `{Binding` syntax. The syntax is also validated and alerts you of an invalid value. Try changing the binding syntax for the `SaveCommand` to `Save123Command`. If you hover the mouse cursor over the text, you'll notice that a tooltip is displayed informing you that **Save123Command** isn't found. This notification isn't considered an error because bindings are dynamic, it's really a small warning that may help you notice when you typed the wrong property.

  If you changed the **SaveCommand** to a different value, restore it now.

## Clean up the Note code-behind

Now that the interaction with the view has changed from event handlers to commands, open the _Views\\NotePage.xaml.cs_ file and replace all the code with a class that only contains the constructor:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\NotePage.xaml.cs**.

    > [!TIP]
    > You may need to expand **Views\\NotePage.xaml** to show the file.

01. Replace the code with the following snippet:

    :::code language="csharp" source="../snippets/viewmodel-shared/Views/NotePage.xaml.cs":::
