---
title: Create a .NET MAUI app
description: Learn how to create a .NET MAUI app that lets you store and retrieve notes. This tutorial demonstrates creating an app that doesn't use any platform-specific code.
ms.date: 03/14/2024
ms.topic: tutorial
---

# Create a .NET MAUI app

This tutorial is designed to demonstrate how to create a .NET Multi-platform App UI (.NET MAUI) app that only uses cross-platform code. Meaning, the code you write won't be specific to Windows, Android, iOS, or macOS. The app you'll create will be a note-taking app, where the user can create, save, and load multiple notes.

In this tutorial, you learn how to:

> [!div class="checklist"]
>
> - Create a .NET MAUI Shell app.
> - Run your app on your chosen platform.
> - Define the user interface with eXtensible Application Markup Language (XAML), and interact with XAML elements through code.
> - Create views and bind them to data.
> - Use navigation to move to and from pages.

You'll use Visual Studio 2022 to create an application with which you can enter a note and save it to device storage. The final application is shown here:

:::image type="content" source="./media/notes-app/intro/final-notelist-small.png" alt-text="Final screenshot of the notes app, listing the notes." lightbox="./media/notes-app/intro/final-notelist.png"::: :::image type="content" source="./media/notes-app/intro/final-note-small.png" alt-text="Final screenshot of the notes app, adding a note." lightbox="./media/notes-app/intro/final-note.png":::

## Create a project

Before you can begin this tutorial, you must follow the [Build your first app article](../../../get-started/first-app.md). While creating the project, use the following settings:

- **Project Name**

  This must be set to `Notes`. If the project is named something different, the code you copy and paste from this tutorial may result in build errors.

- **Place solution and project in the same directory**

  Uncheck this setting.

:::image type="content" source="./media/notes-app/project/vs-configure-project.png" alt-text="Set the name of the .NET MAUI project to Notes in Visual Studio.":::

Choose the latest .NET version when creating your project.

### Select the target device

.NET MAUI apps are designed to run on multiple operating systems and devices. You'll need to select which target you want to test and debug your app with.

Set the **Debug Target** in the Visual Studio toolbar to the device you want to debug and test with. The following steps demonstrate setting the **Debug Target** to Android:

:::image type="content" source="./media/notes-app/project/vs-debugtarget.png" alt-text="Selecting the Android debug target for a .NET MAUI app in Visual Studio.":::

01. Select the **Debug Target** drop-down button.
01. Select the **Android Emulators** item.
01. Select the emulator device.

## Customize the app shell

When Visual Studio creates a .NET MAUI project, four important code files are generated. These can be seen in the **Solution Explorer** pane of Visual Studio:

:::image type="content" source="./media/notes-app/shell/vs-solution-explorer.png" alt-text="Solution Explorer showing the files for a .NET MAUI project in Visual Studio.":::

These files help get the .NET MAUI app configured and running. Each file serves a different purpose, described below:

- _MauiProgram.cs_

  This is a code file that bootstraps your app. The code in this file serves as the cross-platform entry point of the app, which configures and starts the app. The template startup code points to the `App` class defined by the _App.xaml_ file.

- _App.xaml_ and _App.xaml.cs_

  Just to keep things simple, both of these files are referred to as a single file. There are generally two files with any XAML file, the _.xaml_ file itself, and a corresponding code file that is a child item of it in the **Solution Explorer**. The _.xaml_ file contains XAML markup and the code file contains code created by the user to interact with the XAML markup.

  The _App.xaml_ file contains app-wide XAML resources, such as colors, styles, or templates. The _App.xaml.cs_ file generally contains code that instantiates the Shell application. In this project, it points to the `AppShell` class.

- _AppShell.xaml_ and _AppShell.xaml.cs_

  This file defines the `AppShell` class, which is used to define visual hierarchy of the app.

- _MainPage.xaml_ and _MainPage.xaml.cs_

  This is the startup page displayed by the app. The _MainPage.xaml_ file defines the UI (user interface) of the page. _MainPage.xaml.cs_ contains the code-behind for the XAML, like code for a button click event.

### Add an "about" page

The first customization you'll do is adding another page to the project. This page is an "about" page, which represents information about this app, such as the author, version, and perhaps a link for more information.

01. In the **Solution Explorer** pane of Visual Studio, right-click on the **Notes** project > **Add** > **New Item...**.

    :::image type="content" source="./media/notes-app/shell/vs-new-item.png" alt-text="Right-clicking on a project in Visual Studio and selecting New Item.":::

01. In the **Add New Item** dialog, select **.NET MAUI** in the template list on the left-side of the window. Next, select the **.NET MAUI ContentPage (XAML)** template. Name the file _AboutPage.xaml_, and then select **Add**.

    :::image type="content" source="./media/notes-app/shell/vs-about-page.png" alt-text="Adding a new ContentPage to the project. The ContentPage is named AboutPage.xaml.":::

01. The _AboutPage.xaml_ file will open a new document tab, displaying all of the XAML markup that represents the UI of the page. Replace the XAML markup with the following markup:

    :::code language="xaml" source="../snippets/notes-app/shell/AboutPage.xaml":::

01. Save the file by pressing <kbd>CTRL+S</kbd> or by selecting the menu **File** > **Save AboutPage.xaml**.

Let's break down the key parts of the XAML controls placed on the page:

- `<ContentPage>` is the root object for the `AboutPage` class.
- `<VerticalStackLayout>` is the only child object of the <xref:Microsoft.Maui.Controls.ContentPage>. <xref:Microsoft.Maui.Controls.ContentPage> can only have one child object. The <xref:Microsoft.Maui.Controls.VerticalStackLayout> type can have multiple children. This layout control arranges its children vertically, one after the other.
- `<HorizontalStackLayout>` operates the same as a `<VerticalStackLayout>`, except its children are arranged horizontally.
- `<Image>` displays an image, in this case it's using the `dotnet_bot.png` image that comes with every .NET MAUI project.

  > [!IMPORTANT]
  > The file added to the project is actually `dotnet_bot.svg`. .NET MAUI converts Scalable Vector Graphics (SVG) files to Portable Network Graphic (PNG) files based on the target device. Therefore, when adding an SVG file to your .NET MAUI app project, it should be referenced from XAML or C# with a `.png` extension. The only reference to the SVG file should be in your project file.

- `<Label>` controls display text.
- `<Button>` controls can be pressed by the user, which raise the `Clicked` event. You can run code in response to the `Clicked` event.
- `Clicked="LearnMore_Clicked"`

  The `Clicked` event of the button is assigned to the `LearnMore_Clicked` event handler, which will be defined in the code-behind file. You'll create this code in the next step.

### Handle the Clicked event

The next step is to add the code for the button's `Clicked` event.

01. In the **Solution Explorer** pane of Visual Studio, expand the _AboutPage.xaml_ file to reveal it's code-behind file _AboutPage.xaml.cs_. Then, double-click on the _AboutPage.xaml.cs_ file to open it in the code editor.

    :::image type="content" source="./media/notes-app/shell/vs-expand.png" alt-text="An image of the Solution Explorer window in Visual Studio, with a red box highlighting the expand icon.":::

01. Add the following `LearnMore_Clicked` event handler code, which opens the system browser to a specific URL:

    :::code language="csharp" source="../snippets/notes-app/shell/AboutPage.xaml.cs" id="learn_more":::

    Notice that the `async` keyword has been added to the method declaration, which allows the use of the `await` keyword when opening the system browser.

01. Save the file by pressing <kbd>CTRL+S</kbd> or by selecting the menu **File** > **Save AboutPage.xaml.cs**.

Now that the XAML and code-behind of the `AboutPage` is complete, you'll need to get it displayed in the app.

### Add image resources

Some controls can use images, which enhances how users interact with your app. In this section you'll download two images you'll use in your app, along with two alternative images for use with iOS.

Download the following images:

- [Icon: About](https://raw.githubusercontent.com/dotnet/docs-maui/main/docs/tutorials/notes-app/snippets/shell/Resources/Images/icon_about.png)\
  This image is used as an icon for the about page you created earlier.

- [Icon: Notes](https://raw.githubusercontent.com/dotnet/docs-maui/main/docs/tutorials/notes-app/snippets/shell/Resources/Images/icon_notes.png)\
  This image is used as an icon for the notes page you'll create in the next part of this tutorial.

- [Icon: About (iOS)](https://raw.githubusercontent.com/dotnet/docs-maui/main/docs/tutorials/notes-app/snippets/shell/Resources/Images/icon_about_ios.png)
- [Icon: Notes (iOS)](https://raw.githubusercontent.com/dotnet/docs-maui/main/docs/tutorials/notes-app/snippets/shell/Resources/Images/icon_notes_ios.png)

After you've downloaded the images, you can move them with File Explorer to the _Resources\Images_ folder of the project. Any file in this folder is automatically included in the project as a **MauiImage** resource. You can also use Visual Studio to add the images to your project. If you move the images by hand, skip the following procedure.

> [!IMPORTANT]
> Don't skip downloading the iOS-specific images, they're required to complete this tutorial.

#### Move the images with Visual Studio

01. In the **Solution Explorer** pane of Visual Studio, expand the **Resources** folder, which reveals the **Images** folder.

    > [!TIP]
    > You can use File Explorer to drag-and-drop the images directly into the **Solution Explorer** pane, on top of the **Images** folder. This automatically moves the files to the folder, and includes them in the project. If you choose to drag-and-drop the files, ignore the rest of this procedure.

01. Right-click on **Images** and select **Add** > **Existing Item...**.
01. Navigate to the folder that contains the downloaded images.
01. Change the filter to file type filter to **Image Files**.
01. Hold down <kbd>CTRL</kbd> and click on each of the images you downloaded, then press **Add**

:::image type="content" source="./media/notes-app/shell/vs-add-image.png" alt-text="Add four icon images to .NET MAUI project.":::

### Modify the app shell

As noted at the start of this article, the `AppShell` class defines an app's visual hierarchy, the XAML markup used in creating the UI of the app. Update the XAML to add a <xref:Microsoft.Maui.Controls.TabBar> control:

01. Double-click the _AppShell.xaml_ file in the **Solution Explorer** pane to open the XAML editor. Replace the XAML markup with the following code:

    :::code language="xaml" source="../snippets/notes-app/shell/AppShell.xaml":::

01. Save the file by pressing <kbd>CTRL+S</kbd> or by selecting the menu **File** > **Save AppShell.xaml**.

Let's break down the key parts of the XAML:

- `<Shell>` is the root object of the XAML markup.
- `<TabBar>` is the content of the <xref:Microsoft.Maui.Controls.Shell>.
- Two `<ShellContent>` objects inside of the `<TabBar>`. Before you replaced the template code, there was a single `<ShellContent>` object, pointing to the `MainPage` page.

The `TabBar` and its children don't represent any user interface elements, but rather the organization of the app's visual hierarchy. Shell takes these objects and produces the user interface for the content, with a bar at the top representing each page. The `ShellContent.Icon` property for each page uses the `OnPlatform` markup extension. This XAML markup extension is used to specify different values for different platforms. In this example, every platform uses the `icon_about.png` icon by default, but iOS and MacCatalyst use `icon_about_ios.png`.

Each `<ShellContent>` object is pointing to a page to display. This is set by the `ContentTemplate` property.

### Run the app

Run the app by pressing <kbd>F5</kbd> or pressing the play button at the top of Visual Studio:

:::image type="content" source="./media/notes-app/shell/vs-debug-button.png" alt-text="Visual Studio's Debug Target button with the text Windows Machine.":::

You'll see that there are two tabs: **Notes** and **About**. Press the **About** tab and the app navigates to the `AboutPage` you created. Press on the **Learn More** button to open the web browser.

:::image type="content" source="./media/notes-app/shell/final.png" alt-text="About page of .NET MAUI app tutorial.":::

Close the app and return to Visual Studio. If you're using the Android emulator, terminate the app in the virtual device or press the stop button at the top of Visual Studio:

:::image type="content" source="./media/notes-app/shell/vs-stop-button.png" alt-text="Visual Studio's stop debugging button.":::

## Create a page for a note

Now that the app contains the `MainPage` and `AboutPage`, you can start creating the rest of the app. First, you'll create a page that allows a user to create and display note, and then you'll write the code to load and save the note.

The note page will display the note and allow you to either save or delete it. First, add the new page to the project:

01. In the **Solution Explorer** pane of Visual Studio, right-click on the **Notes** project > **Add** > **New Item**.

01. In the **Add New Item** dialog, select **.NET MAUI** in the template list on the left-side of the window. Next, select the **.NET MAUI ContentPage (XAML)** template. Name the file _NotePage.xaml_, and then select **Add**.

01. The _NotePage.xaml_ file will open in a new tab, displaying all of the XAML markup that represents the UI of the page. Replace the XAML code markup the following markup:

    :::code language="xaml" source="../snippets/notes-app/note/NotePage.xaml":::

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

### Load and save a note

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

    :::code language="csharp" source="../snippets/notes-app/note/NotePage.xaml.cs" id="filename_variable" highlight="3":::

    The code above constructs a path to the file, storing it in the app's local data directory. The file name is _notes.txt_.

01. In the constructor of the class, after the `InitializeComponent` method is called, read the file from the device and store its contents in the `TextEditor` control's `Text` property:

    :::code language="csharp" source="../snippets/notes-app/note/NotePage.xaml.cs" id="load_note" highlight="5,6":::

01. Next, add the code to handle the `Clicked` events defined in the XAML:

    :::code language="csharp" source="../snippets/notes-app/note/NotePage.xaml.cs" id="buttons":::

    The `SaveButton_Clicked` method writes the text in the <xref:Microsoft.Maui.Controls.Editor> control, to the file represented by the `_fileName` variable.

    The `DeleteButton_Clicked` method first checks if the file represented by the `_fileName` variable, and if it exists, deletes it. Next, the <xref:Microsoft.Maui.Controls.Editor> control's text is cleared.

01. Save the file by pressing <kbd>CTRL + S</kbd> or by selecting the menu **File** > **Save NotePage.xaml.cs**.

The final code for the code-behind file should look like the following:

:::code language="csharp" source="../snippets/notes-app/note/NotePage.xaml.cs" id="full":::

### Test the note

Now that **note page** is finished, you need a way to present it to the user. Open the _AppShell.xaml_ file, and change the first <xref:Microsoft.Maui.Controls.ShellContent> entry to point to the `NotePage` instead of `MainPage`:

:::code language="xaml" source="../snippets/notes-app/note/AppShell.xaml" highlight="12":::

Save the file and run the app. Try typing into the entry box and press the **Save** button. Close the app, and reopen it. The note you entered should be loaded from the device's storage.

:::image type="content" source="./media/notes-app/note/final.png" alt-text="Note entry page in .NET MAUI app.":::

## Bind data to the UI and navigate pages

This portion of the tutorial introduces the concepts of views, models, and in-app navigation.

In the previous steps of the tutorial, you added two pages to the project: `NotePage` and `AboutPage`. The pages represent a view of data. The `NotePage` is a "view" that displays "note data" and the `AboutPage` is a "view" that displays "app information data." Both of these views have a model of that data hardcoded, or embedded in them, and you'll need to separate the data model from the view.

What is the benefit of separating the model from the view? It allows you to design the view to represent and interact with any portion of the model without worrying about the actual code that implements the model. This is accomplished using data binding, something that will be presented later in this tutorial. For now, though, lets restructure the project.

### Separate the view and model

Refactor the existing code to separate the model from the view. The next few steps will organize the code so that views and models are defined separately from each other.

01. Delete _MainPage.xaml_ and _MainPage.xaml.cs_ from your project, they're no longer needed. In the **Solution Explorer** pane, find the entry for **MainPage.xaml**, right-click it and select **Delete**.

    > [!TIP]
    > Deleting the _MainPage.xaml_ item should also delete the _MainPage.xaml.cs_ item too. If _MainPage.xaml.cs_ wasn't deleted, right-click on it and select **Delete**.

01. Right-click on the **:::no-loc text="Notes":::** project and select **Add** > **New Folder**. Name the folder **:::no-loc text="Models":::**.
01. Right-click on the **:::no-loc text="Notes":::** project and select **Add** > **New Folder**. Name the folder **:::no-loc text="Views":::**.
01. Find the **NotePage.xaml** item and drag it to the **:::no-loc text="Views":::** folder. The **NotePage.xaml.cs** should move with it.

    > [!IMPORTANT]
    > When you move a file, Visual Studio usually prompts you with a warning about how the move operation may take a long time. This shouldn't be a problem here, press **OK** if you see this warning.
    >
    > Visual Studio may also ask you if you want to adjust the namespace of the moved file. Select **No** as the next steps will change the namespace.

01. Find the **AboutPage.xaml** item and drag it to the **:::no-loc text="Views":::** folder. The **AboutPage.xaml.cs** should move with it.

#### Update the view namespace

Now that the views have been moved to the **:::no-loc text="Views":::** folder, you'll need to update the namespaces to match. The namespace for the XAML and code-behind files of the pages is set to `Notes`. This needs to be updated to `Notes.Views`.

01. In the **Solution Explorer** pane, expand both **NotePage.xaml** and **AboutPage.xaml** to reveal the code-behind files:

    :::image type="content" source="./media/notes-app/navigation/vs-expand-views.png" alt-text="The Notes project with both the Views folder and the page views expanded.":::

01. Double-click on the **NotePage.xaml.cs** item to open the code editor. Change the namespace to `Notes.Views`:

    ```csharp
    namespace Notes.Views;
    ```

01. Repeat the previous steps for the **AboutPage.xaml.cs** item.
01. Double-click on the **NotePage.xaml** item to open the XAML editor. The old namespace is referenced through the `x:Class` attribute, which defines which class type is the code-behind for the XAML. This entry isn't just the namespace, but the namespace with the type. Change the `x:Class` value to `Notes.Views.NotePage`:

    ```xaml
    x:Class="Notes.Views.NotePage"
    ```

01. Repeat the previous step for the **AboutPage.xaml** item, but set the `x:Class` value to `Notes.Views.AboutPage`.

#### Fix the namespace reference in Shell

The _AppShell.xaml_ defines two tabs, one for the `NotesPage` and another for `AboutPage`. Now that those two pages were moved to a new namespace, the type mapping in the XAML is now invalid. In the **Solution Explorer** pane, double-click on the **AppShell.xaml** entry to open it in the XAML editor. It should look like the following snippet:

```xaml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell
    x:Class="Notes.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:Notes"
    Shell.FlyoutBehavior="Disabled">

    <TabBar>
        <ShellContent
            Title="Notes"
            ContentTemplate="{DataTemplate local:NotePage}"
            Icon="{OnPlatform 'icon_notes.png', iOS='icon_notes_ios.png', MacCatalyst='icon_notes_ios.png'}" />

        <ShellContent
            Title="About"
            ContentTemplate="{DataTemplate local:AboutPage}"
            Icon="{OnPlatform 'icon_about.png', iOS='icon_about_ios.png', MacCatalyst='icon_about_ios.png'}" />
    </TabBar>

</Shell>
```

A .NET namespace is imported into the XAML through an XML namespace declaration. In the previous XAML markup, it's the `xmlns:local="clr-namespace:Notes"` attribute in the root element: `<Shell>`. The format of declaring an XML namespace to import a .NET namespace in the same assembly is:

```xml
xmlns:{XML namespace name}="clr-namespace:{.NET namespace}"
```

So the previous declaration maps the XML namespace of `local` to the .NET namespace of `Notes`. It's common practice to map the name `local` to the root namespace of your project.

Remove the `local` XML namespace and add a new one. This new XML namespace will map to the .NET namespace of `Notes.Views`, so name it `views`. The declaration should look like the following attribute: `xmlns:views="clr-namespace:Notes.Views"`.

The `local` XML namespace was used by the `ShellContent.ContentTemplate` properties, change them to `views`. Your XAML should now look like the following snippet:

```xaml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell
    x:Class="Notes.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:views="clr-namespace:Notes.Views"
    Shell.FlyoutBehavior="Disabled">

    <TabBar>
        <ShellContent
            Title="Notes"
            ContentTemplate="{DataTemplate views:NotePage}"
            Icon="{OnPlatform 'icon_notes.png', iOS='icon_notes_ios.png', MacCatalyst='icon_notes_ios.png'}" />

        <ShellContent
            Title="About"
            ContentTemplate="{DataTemplate views:AboutPage}"
            Icon="{OnPlatform 'icon_about.png', iOS='icon_about_ios.png', MacCatalyst='icon_about_ios.png'}" />
    </TabBar>

</Shell>
```

You should now be able to run the app without any compiler errors, and everything should still work as before.

### Define the model

Currently the model is the data that is embedded in the note and about views. We'll create new classes to represent that data. First, the model to represent a note page's data:

01. In the **Solution Explorer** pane, right-click on the **:::no-loc text="Models":::** folder and select **Add** > **Class...**.
01. Name the class **Note.cs** and press **Add**.
01. Open **Note.cs** and replace the code with the following snippet:

    :::code language="csharp" source="../snippets/notes-app/navigation/Models/Note.cs":::

01. Save the file.

Next, create the about page's model:

01. In the **Solution Explorer** pane, right-click on the **:::no-loc text="Models":::** folder and select **Add** > **Class...**.
01. Name the class **About.cs** and press **Add**.
01. Open **About.cs** and replace the code with the following snippet:

    :::code language="csharp" source="../snippets/notes-app/navigation/Models/About.cs":::

01. Save the file.

### Update About page

The about page will be the quickest page to update and you'll be able to run the app and see how it loads data from the model.

01. In the **Solution Explorer** pane, open the _Views\\AboutPage.xaml_ file.
01. Replace the content with the following snippet:

    :::code language="xaml" source="../snippets/notes-app/navigation/Views/AboutPage.xaml" highlight="4,6,7-9,15,16,19":::

Let's look at the changed lines, which are highlighted in the previous snippet:

- `xmlns:models="clr-namespace:Notes.Models"`

  This line maps the `Notes.Models` .NET namespace to the `models` XML namespace.

- `x:DataType="models:About"`

  This line instructs the XAML compiler to compile all binding expressions for increased runtime performance, and resolves the binding expressions against the `Notes.Models.About` type.

- The `BindingContext` property of the <xref:Microsoft.Maui.Controls.ContentPage> is set to an instance of the `Note.Models.About` class, using the XML namespace and object of `models:About`. This was set using **property element syntax** instead of an XML attribute.

  > [!IMPORTANT]
  > Until now, properties have been set using an XML attribute. This works great for simple values, such as a `Label.FontSize` property. But if the property value is more complex, you must use **property element syntax** to create the object. Consider the following example of a creating a label with its `FontSize` property set:
  >
  > ```xaml
  > <Label FontSize="22" />
  > ```
  >
  > The same `FontSize` property can be set using **property element syntax**:
  >
  > ```xaml
  > <Label>
  >     <Label.FontSize>
  >         22
  >     </Label.FontSize>
  > </Label>
  > ```

- Three `<Label>` controls had their `Text` property value changed from a hardcoded string to binding syntax: `{Binding PATH}`.

  `{Binding}` syntax is processed at run-time, allowing the value returned from the binding to be dynamic. The `PATH` portion of `{Binding PATH}` is the property path to bind to. The property comes from the current control's `BindingContext`. With the `<Label>` control, `BindingContext` is unset. Context is inherited from the parent when it's unset by the control, which in this case, the parent object setting context is the root object: <xref:Microsoft.Maui.Controls.ContentPage>.

  The object in the `BindingContext` is an instance of the `About` model. The binding path of one of the labels binds the `Label.Text` property to the `About.Title` property.

The final change to the about page is updating the button click that opens a web page. The URL was hardcoded in the code-behind, but the URL should come from the model that is in the `BindingContext` property.

01. In the **Solution Explorer** pane, open the _Views\\AboutPage.xaml.cs_ file.
01. Replace the `LearnMore_Clicked` method with the following code:

    :::code language="csharp" source="../snippets/notes-app/navigation/Views/AboutPage.xaml.cs" id="learn_more" highlight="3,6":::

If you look at the highlighted line, the code checks if the `BindingContext` is a `Models.About` type, and if it is, assigns it to the `about` variable. The next line inside of the `if` statement opens the browser to the URL provided by the `about.MoreInfoUrl` property.

Run the app and you should see that it runs exactly the same as before. Try changing the about model's values and see how the UI and URL opened by the browser also change.

### Update Note page

The previous section bound the **:::no-loc text="about":::** page view to the **:::no-loc text="about":::** model, and now you'll do the same, binding the **:::no-loc text="note":::** view to the **:::no-loc text="note":::** model. However, in this case, the model won't be created in XAML but will be provided in the code-behind in the next few steps.

01. In the **Solution Explorer** pane, open the _Views\\NotePage.xaml_ file.
01. Replace the content with the following snippet:

    :::code language="xaml" source="../snippets/notes-app/navigation/Views/NotePage.xaml" highlight="4,7,11":::

Let's look at the changed lines, which are highlighted in the previous snippet:

- `xmlns:models="clr-namespace:Notes.Models"`

  This line maps the `Notes.Models` .NET namespace to the `models` XML namespace.

- `x:DataType="models:Note"`

  This line instructs the XAML compiler to compile all binding expressions for increased runtime performance, and resolves the binding expressions against the `Notes.Models.Note` type.

- `Text="{Binding Text}"`

  This line changes the `<Editor>` control by adding the `Text` property, and by binding the property to the `Text` property.

The modifications for the code-behind are more complicated than the XAML. The current code is loading the file content in the constructor, and then setting it directly to the `TextEditor.Text` property. Here is what the current code looks like:

```csharp
public NotePage()
{
    InitializeComponent();

    if (File.Exists(_fileName))
        TextEditor.Text = File.ReadAllText(_fileName);
}
```

Instead of loading the note in the constructor, create a new `LoadNote` method. This method will do the following:

- Accept a file name parameter.
- Create a new note model and set the file name.
- If the file exists, load its content into the model.
- If the file exists, update the model with the date the file was created.
- Set the `BindingContext` of the page to the model.

01. In the **Solution Explorer** pane, open the _Views\\NotePage.xaml.cs_ file.
01. Add the following method to the class:

    :::code language="csharp" source="../snippets/notes-app/navigation/Views/NotePage.xaml.cs" id="load_note_by_file":::

01. Update the class constructor to call `LoadNote`. The file name for the note should be a randomly generated name to be created in the app's local data directory.

    :::code language="csharp" source="../snippets/notes-app/navigation/Views/NotePage.xaml.cs" id="load_note_ctor" highlight="5-8":::

## Add a view and model that lists all notes

This portion of the tutorial adds the final piece of the app, a view that displays all of the notes previously created.

### Multiple notes and navigation

Currently the **note** view displays a single note. To display multiple notes, create a new view and model: **AllNotes**.

01. In the **Solution Explorer** pane, right-click on the **:::no-loc text="Views":::** folder and select **Add** > **New Item...**
01. In the **Add New Item** dialog, select **.NET MAUI** in the template list on the left-side of the window. Next, select the **.NET MAUI ContentPage (XAML)** template. Name the file _AllNotesPage.xaml_, and then select **Add**.
01. In the **Solution Explorer** pane, right-click on the **:::no-loc text="Models":::** folder and select **Add** > **Class...**
01. Name the class _AllNotes.cs_ and press **Add**.

### Code the AllNotes model

The new model will represent the data required to display multiple notes. This data will be a property that represents a collection of notes. The collection will be an `ObservableCollection` which is a specialized collection. When a control which lists multiple items, such as a <xref:Microsoft.Maui.Controls.ListView>, is bound to an `ObservableCollection`, the two work together to automatically keep the list of items in sync with the collection. If the list adds an item, the collection is updated. If the collection adds an item, the control is automatically updated with a new item.

01. In the **Solution Explorer** pane, open the _Models\\AllNotes.cs_ file.
01. Replace the code with the following snippet:

    :::code language="csharp" source="../snippets/notes-app/allnotes/Models/AllNotes.cs":::

The previous code declares a collection, named `Notes`, and uses the `LoadNotes` method to load notes from the device. This method uses LINQ extensions to load, transform, and sort the data into the `Notes` collection.

### Design the AllNotes page

Next, the view needs to be designed to support the **AllNotes** model.

01. In the **Solution Explorer** pane, open the _Views\\AllNotesPage.xaml_ file.
01. Replace the code with the following markup:

    :::code language="xaml" source="../snippets/notes-app/allnotes/Views/AllNotesPage.xaml":::

The previous XAML introduces a few new concepts:

- The `ContentPage.ToolbarItems` property contains a `ToolbarItem`. The buttons defined here are usually display at the top of the app, along the page title. Depending on the platform, though, it may be in a different position. When one of these buttons is pressed, the `Clicked` event is raised, just like a normal button.

  The `ToolbarItem.IconImageSource` property sets the icon to display on the button. The icon can be any image resource defined by the project, however, in this example, a `FontImage` is used. A `FontImage` can use a single glyph from a font as an image.

- The <xref:Microsoft.Maui.Controls.CollectionView> control displays a collection of items, and in this case, is bound to the model's `Notes` property. The way each item is presented by the collection view is set through the  `CollectionView.ItemsLayout` and `CollectionView.ItemTemplate` properties.

  For each item in the collection, the `CollectionView.ItemTemplate` generates the declared XAML. The `BindingContext` of that XAML becomes the collection item itself, in this case, each individual note. The template for the note uses two labels, which are bound to the note's `Text` and `Date` properties.

- The <xref:Microsoft.Maui.Controls.CollectionView> handles the `SelectionChanged` event, which is raised when an item in the collection view is selected.

The code-behind for the view needs to be written to load the notes and handle the events.

01. In the **Solution Explorer** pane, open the _Views/AllNotesPage.xaml.cs_ file.
01. Replace the code with the following snippet:

    :::code language="csharp" source="../snippets/notes-app/allnotes/Views/AllNotesPage.xaml.cs":::

This code uses the constructor to set the `BindingContext` of the page to the model.

The `OnAppearing` method is overridden from the base class. This method is automatically called whenever the page is shown, such as when the page is navigated to. The code here tells the model to load the notes. Because the <xref:Microsoft.Maui.Controls.CollectionView> in the **AllNotes view** is bound to the **AllNotes model's** `Notes` property, which is an `ObservableCollection`, whenever the notes are loaded, the <xref:Microsoft.Maui.Controls.CollectionView> is automatically updated.

The `Add_Clicked` handler introduces another new concept, navigation. Because the app is using .NET MAUI Shell, you can navigate to pages by calling the `Shell.Current.GoToAsync` method. Notice that the handler is declared with the `async` keyword, which allows the use of the `await` keyword when navigating. This handler navigates to the `NotePage`.

The last piece of code in the previous snippet is the `notesCollection_SelectionChanged` handler. This method takes the currently selected item, a **:::no-loc text="Note":::** model, and uses its information to navigate to the `NotePage`. <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> uses a URI string for navigation. In this case, a string is constructed that uses a query string parameter to set a property on the destination page. The interpolated string representing the URI ends up looking similar to the following string:

```text
NotePage?ItemId=path\on\device\XYZ.notes.txt
```

The `ItemId=` parameter is set to the file name on the device where the note is stored.

Visual Studio may be indicating that the `NotePage.ItemId` property doesn't exist, which it doesn't. The next step is modifying the **:::no-loc text="Note"::: view** to load the model based on the `ItemId` parameter that you'll create.

### Query string parameters

The **:::no-loc text="Note"::: view** needs to support the query string parameter, `ItemId`. Create it now:

01. In the **Solution Explorer** pane, open the _Views/NotePage.xaml.cs_ file.
01. Add the `QueryProperty` attribute to the `class` keyword, providing the name of the query string property, and the class property it maps to, `ItemId` and `ItemId` respectively:

    :::code language="csharp" source="../snippets/notes-app/allnotes/Views/NotePage.xaml.cs" id="query_prop":::

01. Add a new `string` property named `ItemId`. This property calls the `LoadNote` method, passing the value of the property, which in turn, should be the file name of the note:

    :::code language="csharp" source="../snippets/notes-app/allnotes/Views/NotePage.xaml.cs" id="itemid":::

01. Replace the `SaveButton_Clicked` and `DeleteButton_Clicked` handlers with the following code:

    :::code language="csharp" source="../snippets/notes-app/allnotes/Views/NotePage.xaml.cs" id="buttons":::

    The buttons are now `async`. After they're pressed, the page navigates back to the previous page by using a URI of `..`.

01. Delete the `_fileName` variable from top of the code, as it's no longer used by the class.

### Modify the app's visual tree

The `AppShell` is still loading the single note page, instead, it needs to load the **AllPages view**. Open the _AppShell.xaml_ file and change the first <xref:Microsoft.Maui.Controls.ShellContent> entry to point to the `AllNotesPage` instead of `NotePage`:

:::code language="xaml" source="../snippets/notes-app/allnotes/AppShell.xaml" highlight="12":::

If you run the app now, you'll notice it crashes if you press the **Add** button, complaining that it can't navigate to `NotesPage`. Every page that can be navigated to from another page, needs to be registered with the navigation system. The `AllNotesPage` and `AboutPage` pages are automatically registered with the navigation system by being declared in the <xref:Microsoft.Maui.Controls.TabBar>.

Register the `NotesPage` with the navigation system:

01. In the **Solution Explorer** pane, open the _AppShell.xaml.cs_ file.
01. Add a line to the constructor that registers the navigation route:

    :::code language="csharp" source="../snippets/notes-app/allnotes/AppShell.xaml.cs" highlight="9":::

The `Routing.RegisterRoute` method takes two parameters:

- The first parameter is the string name of the URI you want to register, in this case the resolved name is `"NotePage"`.
- The second parameter is the type of page to load when `"NotePage"` is navigated to.

Now you can run your app. Try adding new notes, navigating back and forth between notes, and deleting notes.

[![Explore the code.](~/media/code-sample.png) Explore the code for this tutorial.](https://github.com/dotnet/maui-samples/tree/main/8.0/Tutorials/CreateNetMauiApp/code). If you want to download a copy of the completed project to compare your code with, download [this project](https://github.com/dotnet/maui-samples/raw/main/8.0/Tutorials/CreateNetMauiApp/app_after.zip).

You've completed the Create a .NET MAUI app tutorial!

## Next steps

In the next tutorial, you'll learn how to implement model-view-viewmodel (MVVM) patterns in your project.

- [Upgrade an app with MVVM principles](../notes-mvvm/index.yml)

The following links provide more information related to some of the concepts you learned in this tutorial:

- [.NET MAUI Shell overview](../../fundamentals/shell/index.md)
- [.NET MAUI Shell pages](../../fundamentals/shell/pages.md)
- [Basic bindings](../../fundamentals/data-binding/basic-bindings.md)
- [Editor control](../../user-interface/controls/editor.md)
