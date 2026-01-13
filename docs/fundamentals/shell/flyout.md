---
title: ".NET MAUI Shell flyout"
description: "Learn how to customize and control a .NET MAUI flyout, which is the optional root menu for a .NET MAUI Shell app."
ms.date: 11/28/2025
---

# .NET MAUI Shell flyout

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-shell)

The navigation experience provided by .NET Multi-platform App UI (.NET MAUI) Shell is based on flyouts and tabs. A flyout is the optional root menu for a Shell app, and is fully customizable. It's accessible through an icon or by swiping from the side of the screen. The flyout consists of an optional header, flyout items, optional menu items, and an optional footer:

:::image type="content" source="media/flyout/flyout-annotated.png" alt-text="Screenshot of a Shell annotated flyout.":::

## Flyout items

One or more flyout items can be added to the flyout, and each flyout item is represented by a <xref:Microsoft.Maui.Controls.FlyoutItem> object. Each <xref:Microsoft.Maui.Controls.FlyoutItem> object should be a child of the subclassed <xref:Microsoft.Maui.Controls.Shell> object. Flyout items appear at the top of the flyout when a flyout header isn't present.

The following example creates a flyout containing two flyout items:

```xaml
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:controls="clr-namespace:Xaminals.Controls"
       xmlns:views="clr-namespace:Xaminals.Views"
       x:Class="Xaminals.AppShell">
    <FlyoutItem Title="Cats"
                Icon="cat.png">
       <Tab>
           <ShellContent ContentTemplate="{DataTemplate views:CatsPage}" />
       </Tab>
    </FlyoutItem>
    <FlyoutItem Title="Dogs"
                Icon="dog.png">
       <Tab>
           <ShellContent ContentTemplate="{DataTemplate views:DogsPage}" />
       </Tab>
    </FlyoutItem>
</Shell>
```

The `FlyoutItem.Title` property, of type `string`, defines the title of the flyout item. The `FlyoutItem.Icon` property, of type <xref:Microsoft.Maui.Controls.ImageSource>, defines the icon of the flyout item:

:::image type="content" source="media/flyout/two-page-app-flyout.png" alt-text="Screenshot of a Shell two page app with flyout items.":::

In this example, each <xref:Microsoft.Maui.Controls.ShellContent> object can only be accessed through flyout items, and not through tabs. This is because by default, tabs will only be displayed if the flyout item contains more than one tab.

> [!IMPORTANT]
> In a Shell app, pages are created on demand in response to navigation. This is accomplished by using the <xref:Microsoft.Maui.Controls.DataTemplate> markup extension to set the `ContentTemplate` property of each <xref:Microsoft.Maui.Controls.ShellContent> object to a <xref:Microsoft.Maui.Controls.ContentPage> object.

Shell has implicit conversion operators that enable the Shell visual hierarchy to be simplified, without introducing additional views into the visual tree. This is possible because a subclassed <xref:Microsoft.Maui.Controls.Shell> object can only ever contain <xref:Microsoft.Maui.Controls.FlyoutItem> objects or a <xref:Microsoft.Maui.Controls.TabBar> object, which can only ever contain <xref:Microsoft.Maui.Controls.Tab> objects, which can only ever contain <xref:Microsoft.Maui.Controls.ShellContent> objects. These implicit conversion operators can be used to remove the <xref:Microsoft.Maui.Controls.FlyoutItem> and <xref:Microsoft.Maui.Controls.Tab> objects from the previous example:

```xaml
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:controls="clr-namespace:Xaminals.Controls"
       xmlns:views="clr-namespace:Xaminals.Views"
       x:Class="Xaminals.AppShell">
   <ShellContent Title="Cats"
                 Icon="cat.png"
                 ContentTemplate="{DataTemplate views:CatsPage}" />
   <ShellContent Title="Dogs"
                 Icon="dog.png"
                 ContentTemplate="{DataTemplate views:DogsPage}" />
</Shell>
```

This implicit conversion automatically wraps each <xref:Microsoft.Maui.Controls.ShellContent> object in <xref:Microsoft.Maui.Controls.Tab> objects, which are wrapped in <xref:Microsoft.Maui.Controls.FlyoutItem> objects.

> [!NOTE]
> All <xref:Microsoft.Maui.Controls.FlyoutItem> objects in a subclassed <xref:Microsoft.Maui.Controls.Shell> object are automatically added to the `Shell.FlyoutItems` collection, which defines the list of items that will be shown in the flyout.

### Flyout display options

The `FlyoutItem.FlyoutDisplayOptions` property configures how a flyout item and its children are displayed in the flyout. This property should be set to a <xref:Microsoft.Maui.Controls.FlyoutDisplayOptions> enumeration member:

- `AsSingleItem`, indicates that the <xref:Microsoft.Maui.Controls.FlyoutItem> will be visible as a single entry in the flyout, regardless of how many child <xref:Microsoft.Maui.Controls.Tab> or <xref:Microsoft.Maui.Controls.ShellContent> objects it contains. When selected, the first child content is displayed, and users can switch between children using tabs (if more than one child exists). This is the default value of the <xref:Microsoft.Maui.Controls.FlyoutDisplayOptions> property.
- `AsMultipleItems`, indicates that the direct children (<xref:Microsoft.Maui.Controls.Tab> and <xref:Microsoft.Maui.Controls.ShellContent> objects) of the <xref:Microsoft.Maui.Controls.FlyoutItem> will each appear as separate entries in the flyout. This enables users to navigate directly to any child content from the flyout, rather than having to use tabs.

Use `AsSingleItem` when you want to group related pages under a single flyout entry with tab navigation. Use `AsMultipleItems` when you want each page to be directly accessible from the flyout menu.

A flyout item for each <xref:Microsoft.Maui.Controls.Tab> object within a <xref:Microsoft.Maui.Controls.FlyoutItem> can be displayed by setting the `FlyoutItem.FlyoutDisplayOptions` property to `AsMultipleItems`:

```xaml
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:controls="clr-namespace:Xaminals.Controls"
       xmlns:views="clr-namespace:Xaminals.Views"
       FlyoutHeaderBehavior="CollapseOnScroll"
       x:Class="Xaminals.AppShell">

    <FlyoutItem FlyoutDisplayOptions="AsMultipleItems">
        <Tab Title="Domestic"
             Icon="paw.png">
            <ShellContent Title="Cats"
                          Icon="cat.png"
                          ContentTemplate="{DataTemplate views:CatsPage}" />
            <ShellContent Title="Dogs"
                          Icon="dog.png"
                          ContentTemplate="{DataTemplate views:DogsPage}" />
        </Tab>
        <ShellContent Title="Monkeys"
                      Icon="monkey.png"
                      ContentTemplate="{DataTemplate views:MonkeysPage}" />
        <ShellContent Title="Elephants"
                      Icon="elephant.png"
                      ContentTemplate="{DataTemplate views:ElephantsPage}" />  
        <ShellContent Title="Bears"
                      Icon="bear.png"
                      ContentTemplate="{DataTemplate views:BearsPage}" />
    </FlyoutItem>

    <ShellContent Title="About"
                  Icon="info.png"
                  ContentTemplate="{DataTemplate views:AboutPage}" />    
</Shell>
```

In this example, flyout items are created for the <xref:Microsoft.Maui.Controls.Tab> object that's a child of the <xref:Microsoft.Maui.Controls.FlyoutItem> object, and the <xref:Microsoft.Maui.Controls.ShellContent> objects that are children of the <xref:Microsoft.Maui.Controls.FlyoutItem> object. This occurs because each <xref:Microsoft.Maui.Controls.ShellContent> object that's a child of the <xref:Microsoft.Maui.Controls.FlyoutItem> object is automatically wrapped in a <xref:Microsoft.Maui.Controls.Tab> object. In addition, a flyout item is created for the final <xref:Microsoft.Maui.Controls.ShellContent> object, which is automatically wrapped in a <xref:Microsoft.Maui.Controls.Tab> object, and then in a <xref:Microsoft.Maui.Controls.FlyoutItem> object.

> [!NOTE]
> Tabs are displayed when a <xref:Microsoft.Maui.Controls.FlyoutItem> contains more than one <xref:Microsoft.Maui.Controls.ShellContent> object.

This results in the following flyout items:

:::image type="content" source="media/flyout/flyout-reduced.png" alt-text="Screenshot of flyout containing FlyoutItem objects.":::

### Define FlyoutItem appearance

The appearance of each <xref:Microsoft.Maui.Controls.FlyoutItem> can be customized by setting the `Shell.ItemTemplate` attached property to a <xref:Microsoft.Maui.Controls.DataTemplate>:

```xaml
<Shell ...
       xmlns:local="clr-namespace:Xaminals"
       x:DataType="local:AppShell">
    ...
    <Shell.ItemTemplate>
        <DataTemplate>
            <Grid ColumnDefinitions="0.2*,0.8*">
                <Image Source="{Binding FlyoutIcon}"
                       Margin="5"
                       HeightRequest="45" />
                <Label Grid.Column="1"
                       Text="{Binding Title}"
                       FontAttributes="Italic"
                       VerticalTextAlignment="Center" />
            </Grid>
        </DataTemplate>
    </Shell.ItemTemplate>
</Shell>
```

This example displays the title of each <xref:Microsoft.Maui.Controls.FlyoutItem> object in italics:

:::image type="content" source="media/flyout/flyoutitem-templated.png" alt-text="Screenshot of templated FlyoutItem objects.":::

Because `Shell.ItemTemplate` is an attached property, different templates can be attached to specific <xref:Microsoft.Maui.Controls.FlyoutItem> objects.

> [!NOTE]
> Shell provides the `Title` and `FlyoutIcon` properties to the `BindingContext` of the `ItemTemplate`.

In addition, Shell includes three style classes, which are automatically applied to <xref:Microsoft.Maui.Controls.FlyoutItem> objects. For more information, see [Style FlyoutItem and MenuItem objects](#style-flyoutitem-and-menuitem-objects).

### Default template for FlyoutItems

The default <xref:Microsoft.Maui.Controls.DataTemplate> used for each <xref:Microsoft.Maui.Controls.FlyoutItem> is shown below:

```xaml
<DataTemplate x:Key="FlyoutTemplate">
    <Grid x:Name="FlyoutItemLayout"
          HeightRequest="{OnPlatform 44, Android=50}"
          ColumnSpacing="{OnPlatform WinUI=0}"
          RowSpacing="{OnPlatform WinUI=0}">
        <VisualStateManager.VisualStateGroups>
            <VisualStateGroupList>
                <VisualStateGroup x:Name="CommonStates">
                    <VisualState x:Name="Normal">
                        <VisualState.Setters>
                            <Setter Property="BackgroundColor"
                                    Value="Transparent" />
                        </VisualState.Setters>
                    </VisualState>          
                    <VisualState x:Name="Selected">
                        <VisualState.Setters>
                            <Setter Property="BackgroundColor"
                                    Value="{AppThemeBinding Light=#1A000000, Dark=#1AFFFFFF}" />
                        </VisualState.Setters>
                    </VisualState>
                </VisualStateGroup>
            </VisualStateGroupList>
        </VisualStateManager.VisualStateGroups>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="{OnPlatform Android=54, iOS=50, WinUI=Auto}" />
            <ColumnDefinition Width="*" />
        </Grid.ColumnDefinitions>
        <Image x:Name="FlyoutItemImage"
               Source="{Binding FlyoutIcon}"
               VerticalOptions="Center"
               HorizontalOptions="{OnPlatform Default=Center, WinUI=Start}"
               HeightRequest="{OnPlatform Android=24, iOS=22, WinUI=16}"
               WidthRequest="{OnPlatform Android=24, iOS=22, WinUI=16}">
            <Image.Margin>
                <OnPlatform x:TypeArguments="Thickness">
                    <OnPlatform.Platforms>
                        <On Platform="WinUI"
                            Value="12,0,12,0" />
                    </OnPlatform.Platforms>
                </OnPlatform>
            </Image.Margin>
        </Image>
        <Label x:Name="FlyoutItemLabel"
               Grid.Column="1"
               Text="{Binding Title}"
               TextColor="{OnPlatform Android={AppThemeBinding Light=#DE000000, Dark=White}}"
               FontSize="{OnPlatform Android=14, iOS=14}"
               FontAttributes="{OnPlatform iOS=Bold}"
               HorizontalOptions="{OnPlatform WinUI=Start}"
               HorizontalTextAlignment="{OnPlatform WinUI=Start}"
               VerticalTextAlignment="Center">
            <Label.Margin>
                <OnPlatform x:TypeArguments="Thickness">
                    <OnPlatform.Platforms>
                        <On Platform="Android"
                            Value="20, 0, 0, 0" />
                    </OnPlatform.Platforms>
                </OnPlatform>
            </Label.Margin>
            <Label.FontFamily>
                <OnPlatform x:TypeArguments="x:String">
                    <OnPlatform.Platforms>
                        <On Platform="Android"
                            Value="sans-serif-medium" />
                    </OnPlatform.Platforms>
                </OnPlatform>
            </Label.FontFamily>
        </Label>
    </Grid>
</DataTemplate>
```

> [!IMPORTANT]
> When combining `OnPlatform` with `AppThemeBinding`, avoid nesting `AppThemeBinding` inside an `<OnPlatform>` element with `x:TypeArguments="Color"`, as this can cause type cast errors at runtime on Android. Instead, use the inline markup extension syntax as shown above, or apply colors directly using style classes.

This template can be used for as a basis for making alterations to the existing flyout layout, and also shows the visual states that are implemented for flyout items.

In addition, the <xref:Microsoft.Maui.Controls.Grid>, <xref:Microsoft.Maui.Controls.Image>, and <xref:Microsoft.Maui.Controls.Label> elements all have `x:Name` values and so can be targeted with the Visual State Manager. For more information, see [Set state on multiple elements](~/user-interface/visual-states.md#set-state-on-multiple-elements).

> [!NOTE]
> The same template can also be used for  <xref:Microsoft.Maui.Controls.MenuItem> objects.

### Replace flyout content

Flyout items, which represent the flyout content, can optionally be replaced with your own content by setting the `Shell.FlyoutContent` bindable property to an `object`:

```xaml
<Shell ...
       xmlns:local="clr-namespace:Xaminals"
       x:Name="shell"
       x:DataType="local:AppShell">
    ...
    <Shell.FlyoutContent>
        <CollectionView BindingContext="{x:Reference shell}"
                        IsGrouped="True"
                        ItemsSource="{Binding FlyoutItems}">
            <CollectionView.ItemTemplate>
                <DataTemplate x:DataType="local:AppShell">
                    <Label Text="{Binding Title}"
                           TextColor="White"
                           FontSize="18" />
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
    </Shell.FlyoutContent>
</Shell>
```

In this example, the flyout content is replaced with a <xref:Microsoft.Maui.Controls.CollectionView> that displays the title of each item in the `FlyoutItems` collection.

> [!NOTE]
> The `FlyoutItems` property, in the <xref:Microsoft.Maui.Controls.Shell> class, is a read-only collection of flyout items.

Alternatively, flyout content can be defined by setting the `Shell.FlyoutContentTemplate` bindable property to a <xref:Microsoft.Maui.Controls.DataTemplate>:

```xaml
<Shell ...
       xmlns:local="clr-namespace:Xaminals"
       x:Name="shell"
       x:DataType="local:AppShell">
    ...
    <Shell.FlyoutContentTemplate>
        <DataTemplate>
            <CollectionView BindingContext="{x:Reference shell}"
                            IsGrouped="True"
                            ItemsSource="{Binding FlyoutItems}">
                <CollectionView.ItemTemplate>
                    <DataTemplate x:DataType="local:AppShell">
                        <Label Text="{Binding Title}"
                               TextColor="White"
                               FontSize="18" />
                    </DataTemplate>
                </CollectionView.ItemTemplate>
            </CollectionView>
        </DataTemplate>
    </Shell.FlyoutContentTemplate>
</Shell>
```

> [!IMPORTANT]
> A flyout header can optionally be displayed above your flyout content, and a flyout footer can optionally be displayed below your flyout content. If your flyout content is scrollable, Shell will attempt to honor the scroll behavior of your flyout header.

## Menu items

Menu items can be optionally added to the flyout, and each menu item is represented by a  <xref:Microsoft.Maui.Controls.MenuItem> object. The position of  <xref:Microsoft.Maui.Controls.MenuItem> objects on the flyout is dependent upon their declaration order in the Shell visual hierarchy. Therefore, any  <xref:Microsoft.Maui.Controls.MenuItem> objects declared before <xref:Microsoft.Maui.Controls.FlyoutItem> objects will appear before the <xref:Microsoft.Maui.Controls.FlyoutItem> objects in the flyout, and any  <xref:Microsoft.Maui.Controls.MenuItem> objects declared after <xref:Microsoft.Maui.Controls.FlyoutItem> objects will appear after the <xref:Microsoft.Maui.Controls.FlyoutItem> objects in the flyout.

The  <xref:Microsoft.Maui.Controls.MenuItem> class has a `Clicked` event, and a `Command` property. Therefore,  <xref:Microsoft.Maui.Controls.MenuItem> objects enable scenarios that execute an action in response to the  <xref:Microsoft.Maui.Controls.MenuItem> being tapped.

 <xref:Microsoft.Maui.Controls.MenuItem> objects can be added to the flyout as shown in the following example:

```xaml
<Shell ...
       xmlns:local="clr-namespace:Xaminals"
       x:DataType="local:AppShell">
    ...            
    <MenuItem Text="Help"
              IconImageSource="help.png"
              Command="{Binding HelpCommand}"
              CommandParameter="https://learn.microsoft.com/dotnet/maui/fundamentals/shell" />    
</Shell>
```

This example adds a  <xref:Microsoft.Maui.Controls.MenuItem> object to the flyout, beneath all the flyout items:

:::image type="content" source="media/flyout/flyout.png" alt-text="Screenshot of flyout containing a MenuItem object.":::

The  <xref:Microsoft.Maui.Controls.MenuItem> object executes an <xref:System.Windows.Input.ICommand> named `HelpCommand`, which opens the URL specified by the `CommandParameter` property in the system web browser.

> [!NOTE]
> The `BindingContext` of each  <xref:Microsoft.Maui.Controls.MenuItem> is inherited from the subclassed <xref:Microsoft.Maui.Controls.Shell> object.

### Define MenuItem appearance

The appearance of each <xref:Microsoft.Maui.Controls.MenuItem> can be customized by setting the `Shell.MenuItemTemplate` attached property to a <xref:Microsoft.Maui.Controls.DataTemplate>:

```xaml
<Shell ...
       xmlns:local="clr-namespace:Xaminals"
       x:DataType="local:AppShell">
    <Shell.MenuItemTemplate>
        <DataTemplate x:DataType="MenuItem">
            <Grid ColumnDefinitions="0.2*,0.8*">
                <Image Source="{Binding IconImageSource}"
                       Margin="5"
                       HeightRequest="45" />
                <Label Grid.Column="1"
                       Text="{Binding Text}"
                       FontAttributes="Italic"
                       VerticalTextAlignment="Center" />
            </Grid>
        </DataTemplate>
    </Shell.MenuItemTemplate>
    ...
    <MenuItem Text="Help"
              IconImageSource="help.png"
              Command="{Binding HelpCommand}"
              CommandParameter="https://learn.microsoft.com/xamarin/xamarin-forms/app-fundamentals/shell" />  
</Shell>
```

This example attaches the <xref:Microsoft.Maui.Controls.DataTemplate> to each <xref:Microsoft.Maui.Controls.MenuItem> object, displaying the title of the  <xref:Microsoft.Maui.Controls.MenuItem> object in italics:

:::image type="content" source="media/flyout/menuitem-templated.png" alt-text="Screenshot of templated MenuItem objects.":::

Because `Shell.MenuItemTemplate` is an attached property, different templates can be attached to specific <xref:Microsoft.Maui.Controls.MenuItem> objects.

> [!NOTE]
> Shell provides the `Text` and `IconImageSource` properties to the `BindingContext` of the `MenuItemTemplate`. You can also use `Title` in place of `Text` and `Icon` in place of `IconImageSource` which will let you reuse the same template for menu items and flyout items.

The default template for <xref:Microsoft.Maui.Controls.FlyoutItem> objects can also be used for <xref:Microsoft.Maui.Controls.MenuItem> objects. For more information, see [Default template for FlyoutItems](#default-template-for-flyoutitems).

## Style FlyoutItem and MenuItem objects

Shell includes three style classes, which are automatically applied to <xref:Microsoft.Maui.Controls.FlyoutItem> and  <xref:Microsoft.Maui.Controls.MenuItem> objects. The style class names are `FlyoutItemLabelStyle`, `FlyoutItemImageStyle`, and `FlyoutItemLayoutStyle`.

The following XAML shows an example of defining styles for these style classes:

```xaml
<Style TargetType="Label"
       Class="FlyoutItemLabelStyle">
    <Setter Property="TextColor"
            Value="Black" />
    <Setter Property="HeightRequest"
            Value="100" />
</Style>

<Style TargetType="Image"
       Class="FlyoutItemImageStyle">
    <Setter Property="Aspect"
            Value="Fill" />
</Style>

<Style TargetType="Layout"
       Class="FlyoutItemLayoutStyle"
       ApplyToDerivedTypes="True">
    <Setter Property="BackgroundColor"
            Value="Teal" />
</Style>
```

These styles will automatically be applied to <xref:Microsoft.Maui.Controls.FlyoutItem> and  <xref:Microsoft.Maui.Controls.MenuItem> objects, without having to set their `StyleClass` properties to the style class names.

In addition, custom style classes can be defined and applied to <xref:Microsoft.Maui.Controls.FlyoutItem> and  <xref:Microsoft.Maui.Controls.MenuItem> objects. For more information about style classes, see [Style classes](~/user-interface/styles/xaml.md#style-classes).

## Flyout header

The flyout header is the content that optionally appears at the top of the flyout, with its appearance being defined by an `object` that can be set with the `Shell.FlyoutHeader` bindable property:

```xaml
<Shell ...>
    <Shell.FlyoutHeader>
        <controls:FlyoutHeader />
    </Shell.FlyoutHeader>
</Shell>
```

The `FlyoutHeader` type is shown in the following example:

```xaml
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="Xaminals.Controls.FlyoutHeader"
             HeightRequest="200">
    <Grid BackgroundColor="Black">
        <Image Aspect="AspectFill"
               Source="store.jpg"
               Opacity="0.6" />
        <Label Text="Animals"
               TextColor="White"
               FontAttributes="Bold"
               HorizontalTextAlignment="Center"
               VerticalTextAlignment="Center" />
    </Grid>
</ContentView>
```

This results in the following flyout header:

:::image type="content" source="media/flyout/flyout-header.png" alt-text="Screenshot of the flyout header.":::

Alternatively, the flyout header appearance can be defined by setting the `Shell.FlyoutHeaderTemplate` bindable property to a <xref:Microsoft.Maui.Controls.DataTemplate>:

```xaml
<Shell ...>
    <Shell.FlyoutHeaderTemplate>
        <DataTemplate>
            <Grid BackgroundColor="Black"
                  HeightRequest="200">
                <Image Aspect="AspectFill"
                       Source="store.jpg"
                       Opacity="0.6" />
                <Label Text="Animals"
                       TextColor="White"
                       FontAttributes="Bold"
                       HorizontalTextAlignment="Center"
                       VerticalTextAlignment="Center" />
            </Grid>            
        </DataTemplate>
    </Shell.FlyoutHeaderTemplate>
</Shell>
```

By default, the flyout header will be fixed in the flyout while the content below will scroll if there are enough items. However, this behavior can be changed by setting the `Shell.FlyoutHeaderBehavior` bindable property to one of the `FlyoutHeaderBehavior` enumeration members:

- `Default` – indicates that the default behavior for the platform will be used. This is the default value of the `FlyoutHeaderBehavior` property.
- `Fixed` – indicates that the flyout header remains visible and unchanged at all times.
- `Scroll` – indicates that the flyout header scrolls out of view as the user scrolls the items.
- `CollapseOnScroll` – indicates that the flyout header collapses to a title only, as the user scrolls the items.

The following example shows how to collapse the flyout header as the user scrolls:

```xaml
<Shell ...
       FlyoutHeaderBehavior="CollapseOnScroll">
    ...
</Shell>
```

## Flyout footer

The flyout footer is the content that optionally appears at the bottom of the flyout, with its appearance being defined by an `object` that can be set with the `Shell.FlyoutFooter` bindable property:

```xaml
<Shell ...>
    <Shell.FlyoutFooter>
        <controls:FlyoutFooter />
    </Shell.FlyoutFooter>
</Shell>
```

The `FlyoutFooter` type is shown in the following example:

```xaml
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:sys="clr-namespace:System;assembly=netstandard"
             x:Class="Xaminals.Controls.FlyoutFooter">
    <StackLayout>
        <Label Text="Xaminals"
               TextColor="GhostWhite"
               FontAttributes="Bold"
               HorizontalOptions="Center" />
        <Label x:DataType="sys:DateTime"
               Text="{Binding Source={x:Static sys:DateTime.Now}, StringFormat='{0:MMMM dd, yyyy}'}"
               TextColor="GhostWhite"
               HorizontalOptions="Center" />
    </StackLayout>
</ContentView>
```

> [!IMPORTANT]
> The previous XAML example defined a new XAML namespace named `sys`: `xmlns:sys="clr-namespace:System;assembly=netstandard"`. This XAML namespace maps `sys` to the .NET `System` namespace. The mapping allows you to use the .NET types defined in that namespace, such as `DateTime`, in the XAML. For more information, see [XAML Namespaces](../../xaml/namespaces/index.md).

This results in the following flyout footer:

:::image type="content" source="media/flyout/flyout-footer.png" alt-text="Screenshot of the flyout footer.":::

Alternatively, the flyout footer appearance can be defined by setting the `Shell.FlyoutFooterTemplate` property to a <xref:Microsoft.Maui.Controls.DataTemplate>:

```xaml
<Shell ...
       xmlns:sys="clr-namespace:System;assembly=netstandard">
    <Shell.FlyoutFooterTemplate>
        <DataTemplate>
            <StackLayout>
                <Label Text="Xaminals"
                       TextColor="GhostWhite"
                       FontAttributes="Bold"
                       HorizontalOptions="Center" />
                <Label x:DataType="sys:DateTime"
                       Text="{Binding Source={x:Static sys:DateTime.Now}, StringFormat='{0:MMMM dd, yyyy}'}"
                       TextColor="GhostWhite"
                       HorizontalOptions="Center" />
            </StackLayout>
        </DataTemplate>
    </Shell.FlyoutFooterTemplate>
</Shell>
```

The flyout footer is fixed to the bottom of the flyout, and can be any height. In addition, the footer never obscures any menu items.

## Flyout width and height

The width and height of the flyout can be customized by setting the `Shell.FlyoutWidth` and `Shell.FlyoutHeight` attached properties to `double` values:

```xaml
<Shell ...
       FlyoutWidth="400"
       FlyoutHeight="200">
    ...
</Shell>
```

This enables scenarios such as expanding the flyout across the entire screen, or reducing the height of the flyout so that it doesn't obscure the tab bar.

## Flyout icon

By default, Shell apps have a hamburger icon which, when pressed, opens the flyout. This icon can be changed by setting the `Shell.FlyoutIcon` bindable property, of type <xref:Microsoft.Maui.Controls.ImageSource>, to an appropriate icon:

```xaml
<Shell ...
       FlyoutIcon="flyouticon.png">
    ...       
</Shell>
```

## Flyout background

The background color of the flyout can be set with the `Shell.FlyoutBackgroundColor` bindable property:

```xaml
<Shell ...
       FlyoutBackgroundColor="AliceBlue">
    ...
</Shell>
```

> [!NOTE]
> The `Shell.FlyoutBackgroundColor` can also be set from a Cascading Style Sheet (CSS). For more information, see [.NET MAUI Shell specific properties](~/user-interface/styles/css.md#net-maui-shell-specific-properties).

Alternatively, the background of the flyout can be specified by setting the `Shell.FlyoutBackground` bindable property to a <xref:Microsoft.Maui.Controls.Brush>:

```xaml
<Shell ...
       FlyoutBackground="LightGray">
    ...
</Shell>
```

In this example, the flyout background is painted with a light gray <xref:Microsoft.Maui.Controls.SolidColorBrush>.

The following example shows setting the flyout background to a <xref:Microsoft.Maui.Controls.LinearGradientBrush>:

```xaml
<Shell ...>
    <Shell.FlyoutBackground>
        <LinearGradientBrush StartPoint="0,0"
                             EndPoint="1,1">
            <GradientStop Color="#8A2387"
                          Offset="0.1" />
            <GradientStop Color="#E94057"
                          Offset="0.6" />
            <GradientStop Color="#F27121"
                          Offset="1.0" />
        </LinearGradientBrush>
    </Shell.FlyoutBackground>
    ...
</Shell>
```

For more information about brushes, see [.NET MAUI Brushes](~/user-interface/brushes/index.md).

## Flyout background image

The flyout can have an optional background image, which appears beneath the flyout header and behind any flyout items, menu items, and the flyout footer. The background image can be specified by setting the `FlyoutBackgroundImage` bindable property, of type <xref:Microsoft.Maui.Controls.ImageSource>, to a file, embedded resource, URI, or stream.

The aspect ratio of the background image can be configured by setting the `FlyoutBackgroundImageAspect` bindable property, of type `Aspect`, to one of the `Aspect` enumeration members:

- `AspectFill` - clips the image so that it fills the display area while preserving the aspect ratio.
- `AspectFit` - letterboxes the image, if required, so that the image fits into the display area, with blank space added to the top/bottom or sides depending on whether the image is wide or tall. This is the default value of the `FlyoutBackgroundImageAspect` property.
- `Fill` - stretches the image to completely and exactly fill the display area. This may result in image distortion.

The following example shows setting these properties:

```xaml
<Shell ...
       FlyoutBackgroundImage="photo.jpg"
       FlyoutBackgroundImageAspect="AspectFill">
    ...
</Shell>
```

This results in a background image appearing in the flyout, below the flyout header:

:::image type="content" source="media/flyout/flyout-backgroundimage.png" alt-text="Screenshot of a flyout background image.":::

## Flyout backdrop

The backdrop of the flyout, which is the appearance of the flyout overlay, can be specified by setting the `Shell.FlyoutBackdrop` attached property to a <xref:Microsoft.Maui.Controls.Brush>:

```xaml
<Shell ...
       FlyoutBackdrop="Silver">
    ...
</Shell>
```

In this example, the flyout backdrop is painted with a silver <xref:Microsoft.Maui.Controls.SolidColorBrush>.

> [!IMPORTANT]
> The `FlyoutBackdrop` attached property can be set on any Shell element, but will only be applied when it's set on <xref:Microsoft.Maui.Controls.Shell>, <xref:Microsoft.Maui.Controls.FlyoutItem>, or <xref:Microsoft.Maui.Controls.TabBar> objects.

The following example shows setting the flyout backdrop to a <xref:Microsoft.Maui.Controls.LinearGradientBrush>:

```xaml
<Shell ...>
    <Shell.FlyoutBackdrop>
        <LinearGradientBrush StartPoint="0,0"
                             EndPoint="1,1">
            <GradientStop Color="#8A2387"
                          Offset="0.1" />
            <GradientStop Color="#E94057"
                          Offset="0.6" />
            <GradientStop Color="#F27121"
                          Offset="1.0" />
        </LinearGradientBrush>
    </Shell.FlyoutBackdrop>
    ...
</Shell>
```

For more information about brushes, see [.NET MAUI Brushes](~/user-interface/brushes/index.md).

## Flyout behavior

The flyout can be accessed through the hamburger icon or by swiping from the side of the screen. However, this behavior can be changed by setting the `Shell.FlyoutBehavior` attached property to one of the `FlyoutBehavior` enumeration members:

- `Disabled` – indicates that the flyout can't be opened by the user.
- `Flyout` – indicates that the flyout can be opened and closed by the user. This is the default value for the `FlyoutBehavior` property.
- `Locked` – indicates that the flyout can't be closed by the user, and that it doesn't overlap content.

The following example shows how to disable the flyout:

```xaml
<Shell ...
       FlyoutBehavior="Disabled">
    ...
</Shell>
```

> [!NOTE]
> The `FlyoutBehavior` attached property can be set on <xref:Microsoft.Maui.Controls.Shell>, <xref:Microsoft.Maui.Controls.FlyoutItem>, <xref:Microsoft.Maui.Controls.ShellContent>, and page objects, to override the default flyout behavior.

## Flyout vertical scroll

By default, a flyout can be scrolled vertically when the flyout items don't fit in the flyout. This behavior can be changed by setting the `Shell.FlyoutVerticalScrollMode` bindable property to one of the `ScrollMode` enumeration members:

- `Disabled` – indicates that vertical scrolling will be disabled.
- `Enabled` – indicates that vertical scrolling will be enabled.
- `Auto` – indicates that vertical scrolling will be enabled if the flyout items don't fit in the flyout. This is the default value of the `FlyoutVerticalScrollMode` property.

The following example shows how to disable vertical scrolling:

```xaml
<Shell ...
       FlyoutVerticalScrollMode="Disabled">
    ...
</Shell>
```

## FlyoutItem selection

When a Shell app that uses a flyout is first run, the `Shell.CurrentItem` property will be set to the first <xref:Microsoft.Maui.Controls.FlyoutItem> object in the subclassed <xref:Microsoft.Maui.Controls.Shell> object. However, the property can be set to another <xref:Microsoft.Maui.Controls.FlyoutItem>, as shown in the following example:

```xaml
<Shell ...
       CurrentItem="{x:Reference aboutItem}">
    <FlyoutItem FlyoutDisplayOptions="AsMultipleItems">
        ...
    </FlyoutItem>
    <ShellContent x:Name="aboutItem"
                  Title="About"
                  Icon="info.png"
                  ContentTemplate="{DataTemplate views:AboutPage}" />
</Shell>
```

This example sets the `CurrentItem` property to the <xref:Microsoft.Maui.Controls.ShellContent> object named `aboutItem`, which results in it being selected and displayed. In this example, an implicit conversion is used to wrap the <xref:Microsoft.Maui.Controls.ShellContent> object in a <xref:Microsoft.Maui.Controls.Tab> object, which is wrapped in a <xref:Microsoft.Maui.Controls.FlyoutItem> object.

The equivalent C# code, given a <xref:Microsoft.Maui.Controls.ShellContent> object named `aboutItem`, is:

```csharp
CurrentItem = aboutItem;
```

In this example, the `CurrentItem` property is set in the subclassed <xref:Microsoft.Maui.Controls.Shell> class. Alternatively, the `CurrentItem` property can be set in any class through the `Shell.Current` static property:

```csharp
Shell.Current.CurrentItem = aboutItem;
```

> [!NOTE]
> An app may enter a state where selecting a flyout item is not a valid operation. In such cases, the <xref:Microsoft.Maui.Controls.FlyoutItem> can be disabled by setting its `IsEnabled` property to `false`. This will prevent users from being able to select the flyout item.

## FlyoutItem visibility

Flyout items are visible in the flyout by default. However, an item can be hidden in the flyout with the `FlyoutItemIsVisible` property, and removed from the flyout with the `IsVisible` property:

- `FlyoutItemIsVisible`, of type `bool`, indicates if the item is hidden in the flyout, but is still reachable with the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> navigation method. The default value of this property is `true`.
- `IsVisible`, of type `bool`, indicates if the item should be removed from the visual tree and therefore not appear in the flyout. Its default value is `true`.

The following example shows hiding an item in the flyout:

```xaml
<Shell ...>
    <FlyoutItem ...
                FlyoutItemIsVisible="False">
        ...
    </FlyoutItem>
</Shell>
```

> [!NOTE]
> There's also a `Shell.FlyoutItemIsVisible` attached property, which can be set on <xref:Microsoft.Maui.Controls.FlyoutItem>,  <xref:Microsoft.Maui.Controls.MenuItem>, <xref:Microsoft.Maui.Controls.Tab>, and <xref:Microsoft.Maui.Controls.ShellContent> objects.

## Open and close the flyout programmatically

The flyout can be programmatically opened and closed by setting the `Shell.FlyoutIsPresented` bindable property to a `boolean` value that indicates whether the flyout is currently open:

```xaml
<Shell ...
       FlyoutIsPresented="{Binding IsFlyoutOpen}">
</Shell>
```

Alternatively, this can be performed in code:

```csharp
Shell.Current.FlyoutIsPresented = false;
```
