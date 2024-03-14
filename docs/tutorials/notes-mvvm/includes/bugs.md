---
author: adegeo
ms.author: adegeo
ms.date: 01/05/2023
ms.topic: include
no-loc: ["Note.cs", "AllNotes", "NotesViewModel.cs", "AllNotesPage.xaml", "AllNotesPage.xaml.cs"]
---

Now that the app code can compile and run, you'll likely have noticed that there are two flaws with how the app behaves. The app doesn't let you reselect a note that's already selected, and the list of notes isn't reordered after a note is created or changed.

## Get notes to the top of the list

First, fix the reordering problem with the notes list. In the _ViewModels\\NotesViewModel.cs_ file, the `AllNotes` collection contains all of the notes to be presented to the user. Unfortunately, the downside to using an `ObservableCollection` is that it must be manually sorted. To get the new or updated items to the top of the list, perform the following steps:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **ViewModels\\NotesViewModel.cs**.
01. In the `ApplyQueryAttributes` method, look at the logic for the **saved** query string key.
01. When the `matchedNote` isn't `null`, the note is being updated. Use the `AllNotes.Move` method to move the `matchedNote` to index 0, which is the top of the list.

    :::code language="csharp" source="../snippets/bugs/csharp/ViewModels/NotesViewModel.cs" id="move" highlight="8":::

    The `AllNotes.Move` method takes two parameters to move an object's position in the collection. The first parameter is the index of the object that to move, and the second parameters is the index of where to move the object. The `AllNotes.IndexOf` method retrieves the index of the note.

01. When the `matchedNote` is `null`, the note is new and is being added to the list. Instead of adding it, which appends the note to the end of the list, insert the note at index 0, which is the top of the list. Change the `AllNotes.Add` method to `AllNotes.Insert`.

    :::code language="csharp" source="../snippets/bugs/csharp/ViewModels/NotesViewModel.cs" id="insert" highlight="12":::

The `ApplyQueryAttributes` method should look like the following code snippet:

:::code language="csharp" source="../snippets/bugs/csharp/ViewModels/NotesViewModel.cs" id="query":::

## Allow selecting a note twice

In the **AllNotes view**, the `CollectionView` lists all of the notes, but doesn't allow you to select the same note twice. There are two ways the item remains selected: when the user changes an existing note, and when the user forcibly navigates backwards. The case where the user saves a note is fixed with the code change in previous section that uses `AllNotes.Move`, so you don't have to worry about that case.

The problem you have to solve now is related to navigation. No matter how the **Allnotes view** is navigated to, the `NavigatedTo` event is raised for the page. This event is a perfect place to forcibly unselect the selected item in the `CollectionView`.

However, with the MVVM pattern being applied here, the viewmodel can't trigger something directly on the view, such as clearing the selected item after the note is saved. So how do you get that to happen? A good implementation of the MVVM pattern minimizes the code-behind in the view. There are a few different ways to solve this problem to support the MVVM separation pattern. However, it's also OK to put code in the code-behind of the view, especially when it's directly tied to the view. MVVM has many great designs and concepts that help you compartmentalize your app, improving maintainability and making it easier for you to add new features. However, in some cases, you may find that MVVM encourages overengineering.

Don't overengineer a solution for this problem, and just use the `NavigatedTo` event to clear the selected item from the `CollectionView`.

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\AllNotesPage.xaml**.
01. In the XAML for the `<ContentPage>`, add the `NavigatedTo` event:

    :::code language="xaml" source="../snippets/bugs/csharp/Views/AllNotesPage.xaml" range="1-9" highlight="6":::

01. You can add a default event handler by right-clicking on the event method name, `ContentPage_NavigatedTo`, and selecting **Go To Definition**. This action opens the _Views\\AllNotesPage.xaml.cs_ in the code editor.

01. Replace the event handler code with the following snippet:

    :::code language="csharp" source="../snippets/bugs/csharp/Views/AllNotesPage.xaml.cs" id="event":::

    In the XAML, the `CollectionView` was given the name of `notesCollection`. This code uses that name to access the `CollectionView`, and set `SelectedItem` to `null`. The selected item is cleared every time the page is navigated to.

Now, run your app. Try to navigate to a note, press the back button, and select the same note a second time. The app behavior is fixed!

[![Explore the code.](~/media/code-sample.png) Explore the code for this step of the tutorial.](https://github.com/dotnet/maui-samples/tree/main/8.0/Tutorials/ConvertToMvvm/step6_bugs). If you want to download a copy of the completed project to compare your code with, download [this project](https://github.com/dotnet/maui-samples/raw/main/8.0/Tutorials/ConvertToMvvm/app_after.zip).
