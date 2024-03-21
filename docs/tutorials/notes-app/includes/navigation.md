---
author: adegeo
ms.author: adegeo
ms.date: 03/21/2024
ms.topic: include
no-loc: ["MainPage.xaml", "MainPage.xaml.cs", "NotePage.xaml", "NotePage.xaml.cs", "AboutPage.xaml", "AboutPage.xaml.cs", "AppShell.xaml", "AppShell.xaml.cs", "Note.cs", "AllNotes"]
---

This portion of the tutorial introduces the concepts of views, models, and in-app navigation.

In the previous steps of the tutorial, you added two pages to the project: `NotePage` and `AboutPage`. The pages represent a view of data. The `NotePage` is a "view" that displays "note data" and the `AboutPage` is a "view" that displays "app information data." Both of these views have a model of that data hardcoded, or embedded in them, and you'll need to separate the data model from the view.

What is the benefit of separating the model from the view? It allows you to design the view to represent and interact with any portion of the model without worrying about the actual code that implements the model. This is accomplished using data binding, something that will be presented later in this tutorial. For now, though, lets restructure the project.

## Separate the view and model

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

### Update the view namespace

Now that the views have been moved to the **:::no-loc text="Views":::** folder, you'll need to update the namespaces to match. The namespace for the XAML and code-behind files of the pages is set to `Notes`. This needs to be updated to `Notes.Views`.

01. In the **Solution Explorer** pane, expand both **NotePage.xaml** and **AboutPage.xaml** to reveal the code-behind files:

    :::image type="content" source="../media/navigation/vs-expand-views.png" alt-text="The Notes project with both the Views folder and the page views expanded.":::

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

### Fix the namespace reference in Shell

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

## Define the model

Currently the model is the data that is embedded in the note and about views. We'll create new classes to represent that data. First, the model to represent a note page's data:

01. In the **Solution Explorer** pane, right-click on the **:::no-loc text="Models":::** folder and select **Add** > **Class...**.
01. Name the class **Note.cs** and press **Add**.
01. Open **Note.cs** and replace the code with the following snippet:

    :::code language="csharp" source="../snippets/navigation/Models/Note.cs":::

01. Save the file.

Next, create the about page's model:

01. In the **Solution Explorer** pane, right-click on the **:::no-loc text="Models":::** folder and select **Add** > **Class...**.
01. Name the class **About.cs** and press **Add**.
01. Open **About.cs** and replace the code with the following snippet:

    :::code language="csharp" source="../snippets/navigation/Models/About.cs":::

01. Save the file.

## Update About page

The about page will be the quickest page to update and you'll be able to run the app and see how it loads data from the model.

01. In the **Solution Explorer** pane, open the _Views\\AboutPage.xaml_ file.
01. Replace the content with the following snippet:

    :::code language="xaml" source="../snippets/navigation/Views/AboutPage.xaml" highlight="4,6-8,14,15,18":::

Let's look at the changed lines, which are highlighted in the previous snippet:

- `xmlns:models="clr-namespace:Notes.Models"`

  This line maps the `Notes.Models` .NET namespace to the `models` XML namespace.

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

    :::code language="csharp" source="../snippets/navigation/Views/AboutPage.xaml.cs" id="learn_more" highlight="3,6":::

If you look at the highlighted line, the code checks if the `BindingContext` is a `Models.About` type, and if it is, assigns it's assigned to the `about` variable. The next line inside of the `if` statement opens the browser to the URL provided by the `about.MoreInfoUrl` property.

Run the app and you should see that it runs exactly the same as before. Try changing the about model's values and see how the UI and URL opened by the browser also change.

## Update Note page

The previous section bound the **:::no-loc text="about":::** page view to the **:::no-loc text="about":::** model, and now you'll do the same, binding the **:::no-loc text="note":::** view to the **:::no-loc text="note":::** model. However, in this case, the model won't be created in XAML but will be provided in the code-behind in the next few steps.

01. In the **Solution Explorer** pane, open the _Views\\NotePage.xaml_ file.
01. Change the `<Editor>` control adding the `Text` property. Bind the property to the `Text` property: `<Editor ... Text="{Binding Text}"`:

    :::code language="xaml" source="../snippets/navigation/Views/NotePage.xaml" highlight="9":::

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

    :::code language="csharp" source="../snippets/navigation/Views/NotePage.xaml.cs" id="load_note_by_file":::

01. Update the class constructor to call `LoadNote`. The file name for the note should be a randomly generated name to be created in the app's local data directory.

    :::code language="csharp" source="../snippets/navigation/Views/NotePage.xaml.cs" id="load_note_ctor" highlight="5-8":::
