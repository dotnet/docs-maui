---
author: adegeo
ms.author: adegeo
ms.date: 12/03/2022
ms.topic: include
---

In the previous step of the tutorial, the notes app was updated to use MVVM concepts. However, the app isn't updating the list of notes after a note is created or deleted. To fix this, you'll need to implement a notification system.

The notification system provides a way for your view models to let other parts of your application know what action it took. For example, if the **Note viewmodel** deletes a note, it would send a notification to the **Notes viewmodel** telling it the note was deleted. The **Notes viewmodel** could then remove the note from the collection.

## Message types

The MVVM Community Toolkit added in the last step of the tutorial includes a messaging system that lets you send messages from one object to another. It's a bit similar to how events work in .NET, except that the recipient of the message doesn't need to know anything about the object that sent the message. This lets you decouple the objects from one another.

Messages contain payload data, and for this app, there'll be two message types related to the **Note viewmodel**:

- Saved
- Deleted

Create a class for each message:

01. In the **Solution Explorer** pane of Visual Studio, right-click on the **Notes** project and select **Add** > **New Folder**. Name the folder **Messages**.
01. Right-click on the **Messages** folder > **Add** > **Class** and name it **NoteSaved.cs**.
01. Replace the code with the following snippet:

    :::code language="csharp" source="../snippets/notifications/csharp/Messages/NoteSaved.cs":::

01. Right-click on the **Messages** folder > **Add** > **Class** and name it **NoteDeleted.cs**.
01. Replace the code with the following snippet:

    :::code language="csharp" source="../snippets/notifications/csharp/Messages/NoteDeleted.cs":::

The code for both message payloads is pretty much the same, the only difference is the type name itself. The payload has a single property named `Note` which is the **Note viewmodel** related to the message.

## Senders

The messages need to be sent when the corresponding action happens in the app. The commands in the **Note viewmodel** already perform the appropriate action, such as deleting the note or saving it. This is when message should be sent.

01. In the **Solution Explorer** pane of Visual Studio, double-click on **ViewModels\\Note.cs**.
01. Update the `Save` method with the following code:

    :::code language="csharp" source="../snippets/notifications/csharp/ViewModels/Note.cs" id="save" highlight="3,6":::

    The new code lines are highlighted. The date of the note is updated to the time it's saved, and then the `WeakReferenceMessenger` sends the `NoteSaved` message.

01. Update the `Delete` method with the following code:

    :::code language="csharp" source="../snippets/notifications/csharp/ViewModels/Note.cs" id="delete" highlight="5":::

    The new code line is highlighted, the `NoteDeleted` message is sent.

## Recipients

The next step is to register an object as a recipient of each message. Really, only the **Notes viewmodel** needs to receive the messages, so that the collection of notes can be updated appropriately.

The **Notes viewmodel** isn't specifying a base class like the **Note viewmodel** does, because there wasn't really any benefit to do so. However, the `ObservableRecipient` base class provided by the MVVM Community Toolkit provides a framework for an object to receive messages, so this is a suitable base class for the **Notes viewmodel**.

The toolkit also provides the `IRecipient<Type>` interface to register an object as a listener to specific messages. This interface has to be implemented for each message type. The interface defines a method named `Receive` which takes the message type as a parameter. For example, if the class implements `IRecipient<string>`, then the following method would be added to the class:

```csharp
public void Receive(string message)
{
    // Method called when the message is sent.
}
```

Update the **Notes viewmodel** to receive the messages:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **ViewModels\\Notes.cs**.
01. Update the namespaces imported in the file with the following:

    :::code language="csharp" source="../snippets/notifications/csharp/ViewModels/Notes.cs" id="namespaces":::

01. Update the class definition to inherit from `ObservableRecipient` and to implement the following interfaces:

    - `IRecipient<Messages.NoteSaved>`
    - `IRecipient<Messages.NoteDeleted>`

    :::code language="csharp" source="../snippets/notifications/csharp/ViewModels/Notes.cs" id="class":::

01. Set the `IsActive` property to `true` in the `Notes()` constructor:

    :::code language="csharp" source="../snippets/notifications/csharp/ViewModels/Notes.cs" id="isactive" highlight="6":::

    The `IsActive` property is important because it sets this object as an active recipient of the messages.

01. Implement the `Receive` method for the `Messages.NoteSaved` message.

    :::code language="csharp" source="../snippets/notifications/csharp/ViewModels/Notes.cs" id="notesaved":::

    This method handles both notes that are created and those that are updated. If updated, the existing note needs to be removed from the `AllNotes` collection, and then the new note added. You may think that you should just remove the note instance directly from the `AllNotes` collection, like so:

    ```csharp
    AllNotes.Remove(message.Note);
    ```

    However, that code won't work. The reason being that the `message.Note` instance isn't the same viewmodel instance that's in the `AllNotes` collection. When a note is selected and the note-editing page is navigated to, only the unique identifier of the note is provided, not the viewmodel instance. The unique identifier is used to load the note from storage, into the new viewmodel instance. When saved, the message is sent with the new viewmodel instance.

    If the new note is added to the `AllNotes` collection, both the old note and the new note will be in the collection, being visible in the UI list. The old note instance must be removed from the collection prior to adding in the new instance. Since both the old and new instances of the note share the same identifier, the code uses it to find the old note, if it exists, and removes it.

01. Implement the `Receive` method for the `Messages.NoteDeleted` message.

    :::code language="csharp" source="../snippets/notifications/csharp/ViewModels/Notes.cs" id="notedeleted":::

    This code searches the `AllNotes` collection for a matching note identifier, and removes it.

Run the project. Notice that when you create, edit, or delete a note, the list of notes is updated.
