---
author: adegeo
ms.author: adegeo
ms.date: 12/12/2022
ms.topic: include
---

In this first part of the tutorial, you'll implement the model-view-viewmodel (MVVM) pattern. This pattern decouples the model (data) from the view (presentation) into a layer called the view model. The role of the view model in MVVM is to transform the model into a format that the view can use. This separation is beneficial when creating complex apps, it allows you to change how the data is presented without being concerned with where the data is coming from or what format the data is in.

## Clean up the model

In the previous tutorial, the model types were acting both as the model (data) and as a view model (data preparation), which was mapped directly to a view. The following table describes the model:

| Code file            | Description                                                                                   |
|----------------------|-----------------------------------------------------------------------------------------------|
| _Models/About.cs_    | The `About` model. Contains read-only fields that describe the app itself, such as the app title and version. |
| _Models/Note.cs_     | The `Note` model. Represents a note.           |
| _Models/AllNotes.cs_ | The `AllNotes` model. Loads all of the notes on the device into a collection. |

Thinking about the app itself, there is only one piece of data that is used by the app, the `Note`. Notes are loaded from the device, saved to the device, and edited through the app UI. There really isn't a need for the `About` and `AllNotes` models. Remove these models from the project:

01. Find the **Solution Explorer** pane of Visual Studio.
01. Right-click on the **Models\\About.cs** file and select **Delete**. Press **OK** to delete the file.
01. Right-click on the **Models\\AllNotes.cs** file and select **Delete**. Press **OK** to delete the file.

The only model file remaining is the **Models\\Note.cs** file.

## Update the model

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

    :::code language="csharp" source="../snippets/model/csharp/Notes/Models/Note.cs" id="save_delete":::

01. The app needs to load notes in two ways, loading an individual note from a file and loading all notes on the device. The code to handle loading can be `static` members, not requiring a class instance to run.

    Add the following code to the class to load a note by file name:

    :::code language="csharp" source="../snippets/model/csharp/Notes/Models/Note.cs" id="load_single":::

    This code takes the file name as a parameter, builds the path to where the notes are stored on the device, and attempts to load the file if it exists.

01. The second way to load notes is to enumerate all notes on the device and load them into a collection.

    Add the following code to the class:

    :::code language="csharp" source="../snippets/model/csharp/Notes/Models/Note.cs" id="load_all":::

    This code returns an enumerable collection of `Note` model types by retrieving the files on the device that match the notes file pattern: _*.notes.txt_. Each file name is passed to the `Load` method that loads a single note. Finally, the collection of notes is ordered by the date of each note and returned to the caller.

01. Lastly, add a constructor to the class which sets the default values for the properties, including a random file name:

    :::code language="csharp" source="../snippets/model/csharp/Notes/Models/Note.cs" id="ctor":::

The `Note` class code should look like the following:

:::code language="csharp" source="../snippets/model/csharp/Notes/Models/Note.cs" id="full":::

Now that the `Note` model is complete, the view models can be created.
