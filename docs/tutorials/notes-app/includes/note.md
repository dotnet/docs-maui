---
author: adegeo
ms.author: adegeo
ms.date: 03/21/2024
ms.topic: include
no-loc: ["NotePage.xaml", "NotePage.xaml.cs", "AppShell.xaml"]
---

Now that the app contains the `MainPage` and `AboutPage`, you can start creating the rest of the app. First, you'll create a page that allows a user to create and display note, and then you'll write the code to load and save the note.

The note page will display the note and allow you to either save or delete it. First, add the new page to the project:

01. In the **Solution Explorer** pane of Visual Studio, right-click on the **Notes** project > **Add** > **New Item...**.

01. In the **Add New Item** dialog, select **.NET MAUI** in the template list on the left-side of the window. Next, select the **.NET MAUI ContentPage (XAML)** template. Name the file _NotePage.xaml_, and then select **Add**.

01. The _NotePage.xaml_ file will open in a new tab, displaying all of the XAML markup that represents the UI of the page. Replace the XAML code markup the following markup:

    :::code language="xaml" source="../snippets/note/NotePage.xaml":::

01. Save the file by pressing <kbd>CTRL + S</kbd> or by selecting the menu **File** > **Save NotePage.xaml**.

Let's break down the key parts of the XAML controls placed on the page:

- `<VerticalStackLayout>` arranges its children controls vertically, one below the other.
- `<Editor>` is a multi-line text editor control, and is the first control inside of <xref:Microsoft.Maui.Controls.VerticalStackLayout>.
- `<Grid>` is a layout control, and is the second control inside of <xref:Microsoft.Maui.Controls.VerticalStackLayout>.

  This control defines columns and rows to create cells. Child controls are placed within those cells.

  By default, the <xref:Microsoft.Maui.Controls.Grid> control contains a single row and column, creating a single cell. Columns are defined with a width, and the `*` value for width tells the column to fill up as much space as possible. The previous snippet defined two columns, both using as much space as possible, which evenly distributes the columns in the allotted space: `ColumnDefinitions="*,*"`. The column sizes are separated by a `,` character.

  Columns and rows defined by a <xref:Microsoft.Maui.Controls.Grid> are indexed starting at 0. So the first column would be index 0, the second column is index 1, and so on.

- Two `<Button>` controls are inside the `<Grid>` and assigned a column. If a child control doesn't define a column assignment, it's automatically assigned to the first column. In this markup, the first button is the "Save" button and automatically assigned to the first column, column 0. The second button is the "Delete" button and assigned to the second column, column 1.

  Notice the two buttons have the `Clicked` event handled. You'll add the code for those handlers in the next section.

## Load and save a note

Open the _NotePage.xaml.cs_ code-behind file. You can open the code-behind for the _NotePage.xaml_ file in three ways:

- If the _NotePage.xaml_ is open and is the active document being edited, press <kbd>F7</kbd>.
- If the _NotePage.xaml_ is open and is the active document being edited, right-click in the text editor and select **View Code**.
- Use the **Solution Explorer** to expand the _NotePage.xaml_ entry, revealing the _NotePage.xaml.cs_ file. Double-click the file to open it.

When you add a new XAML file, the code-behind contains a single line in the constructor, a call to the `InitializeComponent` method:

```csharp
namespace Notes;

public partial class NotePage : ContentPage
{
    public NotePage()
    {
        InitializeComponent();
    }
}
```

The `InitializeComponent` method reads the XAML markup and initializes all of the objects defined by the markup. The objects are connected in their parent-child relationships, and the event handlers defined in code are attached to events set in the XAML.

Now that you understand a little more about code-behind files, you're going to add code to the _NotePage.xaml.cs_ code-behind file to handle loading and saving notes.

01. When a note is created, it's saved to the device as a text file. The name of the file is represented by the `_fileName` variable. Add the following `string` variable declaration to the `NotePage` class:

    :::code language="csharp" source="../snippets/note/NotePage.xaml.cs" id="filename_variable" highlight="3":::

    The code above constructs a path to the file, storing it in the app's local data directory. The file name is _notes.txt_.

01. In the constructor of the class, after the `InitializeComponent` method is called, read the file from the device and store its contents in the `TextEditor` control's `Text` property:

    :::code language="csharp" source="../snippets/note/NotePage.xaml.cs" id="load_note" highlight="5,6":::

01. Next, add the code to handle the `Clicked` events defined in the XAML:

    :::code language="csharp" source="../snippets/note/NotePage.xaml.cs" id="buttons":::

    The `SaveButton_Clicked` method writes the text in the <xref:Microsoft.Maui.Controls.Editor> control, to the file represented by the `_fileName` variable.

    The `DeleteButton_Clicked` method first checks if the file represented by the `_fileName` variable, and if it exists, deletes it. Next, the <xref:Microsoft.Maui.Controls.Editor> control's text is cleared.

01. Save the file by pressing <kbd>CTRL + S</kbd> or by selecting the menu **File** > **Save NotePage.xaml.cs**.

The final code for the code-behind file should look like the following:

:::code language="csharp" source="../snippets/note/NotePage.xaml.cs" id="full":::

## Test the note

Now that **note page** is finished, you need a way to present it to the user. Open the _AppShell.xaml_ file, and change the first <xref:Microsoft.Maui.Controls.ShellContent> entry to point to the `NotePage` instead of `MainPage`:

:::code language="xaml" source="../snippets/note/AppShell.xaml" highlight="12":::

Save the file and run the app. Try typing into the entry box and press the **Save** button. Close the app, and reopen it. The note you entered should be loaded from the device's storage.

:::image type="content" source="../media/note/final.png" alt-text="Note entry page in .NET MAUI app.":::
