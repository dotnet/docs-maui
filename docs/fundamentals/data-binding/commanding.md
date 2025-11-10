---
title: "Commanding"
description: "Learn how to implement the Command property with .NET MAUI data bindings. The commanding interface provides an alternative approach to implementing commands that is suited to the MVVM architecture."
ms.date: 02/27/2025
---

# Commanding

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-databinding)

In a .NET Multi-platform App UI (.NET MAUI) app that uses the Model-View-ViewModel (MVVM) pattern, data bindings are defined between properties in the viewmodel, which is typically a class that derives from `INotifyPropertyChanged`, and properties in the view, which is typically the XAML file. Sometimes an app has needs that go beyond these property bindings by requiring the user to initiate commands that affect something in the viewmodel. These commands are generally signaled by button clicks or finger taps, and traditionally they are processed in the code-behind file in a handler for the `Clicked` event of the <xref:Microsoft.Maui.Controls.Button> or the `Tapped` event of a <xref:Microsoft.Maui.Controls.TapGestureRecognizer>.

The commanding interface provides an alternative approach to implementing commands that is much better suited to the MVVM architecture. The viewmodel can contain commands, which are methods that are executed in reaction to a specific activity in the view such as a <xref:Microsoft.Maui.Controls.Button> click. Data bindings are defined between these commands and the <xref:Microsoft.Maui.Controls.Button>.

To allow a data binding between a <xref:Microsoft.Maui.Controls.Button> and a viewmodel, the <xref:Microsoft.Maui.Controls.Button> defines two properties:

- `Command` of type [`System.Windows.Input.ICommand`](xref:System.Windows.Input.ICommand)
- `CommandParameter` of type `Object`

To use the command interface, you define a data binding that targets the `Command` property of the <xref:Microsoft.Maui.Controls.Button> where the source is a property in the viewmodel of type <xref:System.Windows.Input.ICommand>. The viewmodel contains code associated with that <xref:System.Windows.Input.ICommand> property that is executed when the button is clicked. You can set the `CommandParameter` property to arbitrary data to distinguish between multiple buttons if they are all bound to the same <xref:System.Windows.Input.ICommand> property in the viewmodel.

Many other views also define `Command` and `CommandParameter` properties. All these commands can be handled within a viewmodel using an approach that doesn't depend on the user-interface object in the view.

## ICommands

The [<xref:System.Windows.Input.ICommand>](xref:System.Windows.Input.ICommand) interface is defined in the [System.Windows.Input](xref:System.Windows.Input) namespace, and consists of two methods and one event:

```csharp
public interface ICommand
{
    public void Execute (Object parameter);
    public bool CanExecute (Object parameter);
    public event EventHandler CanExecuteChanged;
}
```

To use the command interface, your viewmodel should contain properties of type <xref:System.Windows.Input.ICommand>:

```csharp
public ICommand MyCommand { private set; get; }
```

The viewmodel must also reference a class that implements the <xref:System.Windows.Input.ICommand> interface. In the view, the `Command` property of a <xref:Microsoft.Maui.Controls.Button> is bound to that property:

```xaml
<Button Text="Execute command"
        Command="{Binding MyCommand}" />
```

When the user presses the <xref:Microsoft.Maui.Controls.Button>, the <xref:Microsoft.Maui.Controls.Button> calls the `Execute` method in the <xref:System.Windows.Input.ICommand> object bound to its `Command` property.

When the binding is first defined on the `Command` property of the <xref:Microsoft.Maui.Controls.Button>, and when the data binding changes in some way, the <xref:Microsoft.Maui.Controls.Button> calls the `CanExecute` method in the <xref:System.Windows.Input.ICommand> object. If `CanExecute` returns `false`, then the <xref:Microsoft.Maui.Controls.Button> disables itself. This indicates that the particular command is currently unavailable or invalid.

The <xref:Microsoft.Maui.Controls.Button> also attaches a handler on the `CanExecuteChanged` event of <xref:System.Windows.Input.ICommand>. The event is raised from within the viewmodel. When that event is raised, the <xref:Microsoft.Maui.Controls.Button> calls `CanExecute` again. The <xref:Microsoft.Maui.Controls.Button> enables itself if `CanExecute` returns `true` and disables itself if `CanExecute` returns `false`.

> [!NOTE]
> You can also use the `IsEnabled` property on buttons instead of the `CanExecute` method, or in conjunction with it. In .NET MAUI 7 and earlier, it was not possible to use the `IsEnabled` property of <xref:Microsoft.Maui.Controls.Button> while using the command interface, as the `CanExecute` method's return value always overwrote the `IsEnabled` property. This is fixed from .NET MAUI 8 forward; the `IsEnabled` property is now usable on command-based buttons. However, be aware that the `IsEnabled` property and the `CanExecute` method now must *both* return true in order for the button to be enabled (and the parent control must be enabled as well).

When your viewmodel defines a property of type <xref:System.Windows.Input.ICommand>, the viewmodel must also contain or reference a class that implements the <xref:System.Windows.Input.ICommand> interface. This class must contain or reference the `Execute` and `CanExecute` methods, and fire the `CanExecuteChanged` event whenever the `CanExecute` method might return a different value. You can use the `Command` or `Command<T>` class included in .NET MAUI to implement the <xref:System.Windows.Input.ICommand> interface. These classes allow you to specify the bodies of the `Execute` and `CanExecute` methods in class constructors.

> [!TIP]
> Use `Command<T>` when you use the `CommandParameter` property to distinguish between multiple views bound to the same <xref:System.Windows.Input.ICommand> property, and the `Command` class when that isn't a requirement.

## Basic commanding

The following examples demonstrate basic commands implemented in a viewmodel.

The `PersonViewModel` class defines three properties named `Name`, `Age`, and `Skills` that define a person:

```csharp
public class PersonViewModel : INotifyPropertyChanged
{
    string name;
    double age;
    string skills;

    public event PropertyChangedEventHandler PropertyChanged;

    public string Name
    {
        set { SetProperty(ref name, value); }
        get { return name; }
    }

    public double Age
    {
        set { SetProperty(ref age, value); }
        get { return age; }
    }

    public string Skills
    {
        set { SetProperty(ref skills, value); }
        get { return skills; }
    }

    public override string ToString()
    {
        return Name + ", age " + Age;
    }

    bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (Object.Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

The `PersonCollectionViewModel` class shown below creates new objects of type `PersonViewModel` and allows the user to fill in the data. For that purpose, the class defines `IsEditing`, of type `bool`, and `PersonEdit`, of type `PersonViewModel`, properties. In addition, the class defines three properties of type <xref:System.Windows.Input.ICommand> and a property named `Persons` of type `IList<PersonViewModel>`:

```csharp
public class PersonCollectionViewModel : INotifyPropertyChanged
{
    PersonViewModel personEdit;
    bool isEditing;

    public event PropertyChangedEventHandler PropertyChanged;
    ···

    public bool IsEditing
    {
        private set { SetProperty(ref isEditing, value); }
        get { return isEditing; }
    }

    public PersonViewModel PersonEdit
    {
        set { SetProperty(ref personEdit, value); }
        get { return personEdit; }
    }

    public ICommand NewCommand { private set; get; }
    public ICommand SubmitCommand { private set; get; }
    public ICommand CancelCommand { private set; get; }

    public IList<PersonViewModel> Persons { get; } = new ObservableCollection<PersonViewModel>();

    bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (Object.Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

In this example, changes to the three <xref:System.Windows.Input.ICommand> properties and the `Persons` property do not result in `PropertyChanged` events being raised. These properties are all set when the class is first created and do not change.

The following example shows the XAML that consumes the `PersonCollectionViewModel`:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:DataBindingDemos"
             x:Class="DataBindingDemos.PersonEntryPage"
             Title="Person Entry"
             x:DataType="local:PersonCollectionViewModel">             
    <ContentPage.BindingContext>
        <local:PersonCollectionViewModel />
    </ContentPage.BindingContext>
    <Grid Margin="10">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>

        <!-- New Button -->
        <Button Text="New"
                Grid.Row="0"
                Command="{Binding NewCommand}"
                HorizontalOptions="Start" />

        <!-- Entry Form -->
        <Grid Grid.Row="1"
              IsEnabled="{Binding IsEditing}">
            <Grid x:DataType="local:PersonViewModel"
                  BindingContext="{Binding PersonEdit}">
                <Grid.RowDefinitions>
                    <RowDefinition Height="Auto" />
                    <RowDefinition Height="Auto" />
                    <RowDefinition Height="Auto" />
                </Grid.RowDefinitions>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="Auto" />
                    <ColumnDefinition Width="*" />
                </Grid.ColumnDefinitions>

                <Label Text="Name: " Grid.Row="0" Grid.Column="0" />
                <Entry Text="{Binding Name}"
                       Grid.Row="0" Grid.Column="1" />
                <Label Text="Age: " Grid.Row="1" Grid.Column="0" />
                <StackLayout Orientation="Horizontal"
                             Grid.Row="1" Grid.Column="1">
                    <Stepper Value="{Binding Age}"
                             Maximum="100" />
                    <Label Text="{Binding Age, StringFormat='{0} years old'}"
                           VerticalOptions="Center" />
                </StackLayout>
                <Label Text="Skills: " Grid.Row="2" Grid.Column="0" />
                <Entry Text="{Binding Skills}"
                       Grid.Row="2" Grid.Column="1" />
            </Grid>
        </Grid>

        <!-- Submit and Cancel Buttons -->
        <Grid Grid.Row="2">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="*" />
                <ColumnDefinition Width="*" />
            </Grid.ColumnDefinitions>

            <Button Text="Submit"
                    Grid.Column="0"
                    Command="{Binding SubmitCommand}"
                    VerticalOptions="Center" />
            <Button Text="Cancel"
                    Grid.Column="1"
                    Command="{Binding CancelCommand}"
                    VerticalOptions="Center" />
        </Grid>

        <!-- List of Persons -->
        <ListView Grid.Row="3"
                  ItemsSource="{Binding Persons}" />
    </Grid>
</ContentPage>
```

In this example, the page's `BindingContext` property is set to the `PersonCollectionViewModel`. The <xref:Microsoft.Maui.Controls.Grid> contains a <xref:Microsoft.Maui.Controls.Button> with the text **New** with its `Command` property bound to the `NewCommand` property in the viewmodel, an entry form with properties bound to the `IsEditing` property, as well as properties of `PersonViewModel`, and two more buttons bound to the `SubmitCommand` and `CancelCommand` properties of the viewmodel. The <xref:Microsoft.Maui.Controls.ListView> displays the collection of persons already entered:

The following screenshot shows the **Submit** button enabled after an age has been set:

:::image type="content" source="media/commanding/personentry.png" alt-text="Person Entry.":::

When the user first presses the **New** button, this enables the entry form but disables the **New** button. The user then enters a name, age, and skills. At any time during the editing, the user can press the **Cancel** button to start over. Only when a name and a valid age have been entered is the **Submit** button enabled. Pressing this **Submit** button transfers the person to the collection displayed by the <xref:Microsoft.Maui.Controls.ListView>. After either the **Cancel** or **Submit** button is pressed, the entry form is cleared and the **New** button is enabled again.

All the logic for the **New**, **Submit**, and **Cancel** buttons is handled in `PersonCollectionViewModel` through definitions of the `NewCommand`, `SubmitCommand`, and `CancelCommand` properties. The constructor of the `PersonCollectionViewModel` sets these three properties to objects of type `Command`.  

A constructor of the `Command` class allows you to pass arguments of type `Action` and `Func<bool>` corresponding to the `Execute` and `CanExecute` methods. This action and function can be defined as lambda functions in the `Command` constructor:

```csharp
public class PersonCollectionViewModel : INotifyPropertyChanged
{
    ···
    public PersonCollectionViewModel()
    {
        NewCommand = new Command(
            execute: () =>
            {
                PersonEdit = new PersonViewModel();
                PersonEdit.PropertyChanged += OnPersonEditPropertyChanged;
                IsEditing = true;
                RefreshCanExecutes();
            },
            canExecute: () =>
            {
                return !IsEditing;
            });
        ···
    }

    void OnPersonEditPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        (SubmitCommand as Command).ChangeCanExecute();
    }

    void RefreshCanExecutes()
    {
        (NewCommand as Command).ChangeCanExecute();
        (SubmitCommand as Command).ChangeCanExecute();
        (CancelCommand as Command).ChangeCanExecute();
    }
    ···
}
```

When the user clicks the **New** button, the `execute` function passed to the `Command` constructor is executed. This creates a new `PersonViewModel` object, sets a handler on that object's `PropertyChanged` event, sets `IsEditing` to `true`, and calls the `RefreshCanExecutes` method defined after the constructor.

Besides implementing the <xref:System.Windows.Input.ICommand> interface, the `Command` class also defines a method named `ChangeCanExecute`. A viewmodel should call `ChangeCanExecute` for an <xref:System.Windows.Input.ICommand> property whenever anything happens that might change the return value of the `CanExecute` method. A call to `ChangeCanExecute` causes the `Command` class to fire the `CanExecuteChanged` method. The <xref:Microsoft.Maui.Controls.Button> has attached a handler for that event and responds by calling `CanExecute` again, and then enabling itself based on the return value of that method.

When the `execute` method of `NewCommand` calls `RefreshCanExecutes`, the `NewCommand` property gets a call to `ChangeCanExecute`, and the <xref:Microsoft.Maui.Controls.Button> calls the `canExecute` method, which now returns `false` because the `IsEditing` property is now `true`.

The `PropertyChanged` handler for the new `PersonViewModel` object calls the `ChangeCanExecute` method of `SubmitCommand`:

```csharp
public class PersonCollectionViewModel : INotifyPropertyChanged
{
    ···
    public PersonCollectionViewModel()
    {
        ···
        SubmitCommand = new Command(
            execute: () =>
            {
                Persons.Add(PersonEdit);
                PersonEdit.PropertyChanged -= OnPersonEditPropertyChanged;
                PersonEdit = null;
                IsEditing = false;
                RefreshCanExecutes();
            },
            canExecute: () =>
            {
                return PersonEdit != null &&
                       PersonEdit.Name != null &&
                       PersonEdit.Name.Length > 1 &&
                       PersonEdit.Age > 0;
            });
        ···
    }
    ···
}
```

The `canExecute` function for `SubmitCommand` is called every time there's a property changed in the `PersonViewModel` object being edited. It returns `true` only when the `Name` property is at least one character long, and `Age` is greater than 0. At that time, the **Submit** button becomes enabled.

The `execute` function for **Submit** removes the property-changed handler from the `PersonViewModel`, adds the object to the `Persons` collection, and returns everything to its initial state.

The `execute` function for the **Cancel** button does everything that the **Submit** button does except add the object to the collection:

```csharp
public class PersonCollectionViewModel : INotifyPropertyChanged
{
    ···
    public PersonCollectionViewModel()
    {
        ···
        CancelCommand = new Command(
            execute: () =>
            {
                PersonEdit.PropertyChanged -= OnPersonEditPropertyChanged;
                PersonEdit = null;
                IsEditing = false;
                RefreshCanExecutes();
            },
            canExecute: () =>
            {
                return IsEditing;
            });
    }
    ···
}
```

The `canExecute` method returns `true` at any time a `PersonViewModel` is being edited.

> [!NOTE]
> It isn't necessary to define the `execute` and `canExecute` methods as lambda functions. You can write them as private methods in the viewmodel and reference them in the `Command` constructors. However, this approach can result in a lot of methods that are referenced only once in the viewmodel.

## Using Command parameters

It's' sometimes convenient for one or more buttons, or other user-interface objects, to share the same <xref:System.Windows.Input.ICommand> property in the viewmodel. In this case, you can use the `CommandParameter` property to distinguish between the buttons.

You can continue to use the `Command` class for these shared <xref:System.Windows.Input.ICommand> properties. The class defines an alternative constructor that accepts `execute` and `canExecute` methods with parameters of type `Object`. This is how the `CommandParameter` is passed to these methods. However, when specifying a `CommandParameter`, it's easiest to use the generic `Command<T>` class to specify the type of the object set to `CommandParameter`. The `execute` and `canExecute` methods that you specify have parameters of that type.

The following example demonstrates a keyboard for entering decimal numbers:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:DataBindingDemos"
             x:Class="DataBindingDemos.DecimalKeypadPage"
             Title="Decimal Keyboard"
             x:DataType="local:DecimalKeypadViewModel">
    <ContentPage.BindingContext>
        <local:DecimalKeypadViewModel />
    </ContentPage.BindingContext>
    <ContentPage.Resources>
        <Style TargetType="Button">
            <Setter Property="FontSize" Value="32" />
            <Setter Property="BorderWidth" Value="1" />
            <Setter Property="BorderColor" Value="Black" />
        </Style>
    </ContentPage.Resources>

    <Grid WidthRequest="240"
          HeightRequest="480"
          ColumnDefinitions="80, 80, 80"
          RowDefinitions="Auto, Auto, Auto, Auto, Auto, Auto"
          ColumnSpacing="2"
          RowSpacing="2"
          HorizontalOptions="Center"
          VerticalOptions="Center">
        <Label Text="{Binding Entry}"
               Grid.Row="0" Grid.Column="0" Grid.ColumnSpan="3"
               Margin="0,0,10,0"
               FontSize="32"
               LineBreakMode="HeadTruncation"
               VerticalTextAlignment="Center"
               HorizontalTextAlignment="End" />
        <Button Text="CLEAR"
                Grid.Row="1" Grid.Column="0" Grid.ColumnSpan="2"
                Command="{Binding ClearCommand}" />
        <Button Text="&#x21E6;"
                Grid.Row="1" Grid.Column="2"
                Command="{Binding BackspaceCommand}" />
        <Button Text="7"
                Grid.Row="2" Grid.Column="0"
                Command="{Binding DigitCommand}"
                CommandParameter="7" />
        <Button Text="8"
                Grid.Row="2" Grid.Column="1"
                Command="{Binding DigitCommand}"
                CommandParameter="8" />        
        <Button Text="9"
                Grid.Row="2" Grid.Column="2"
                Command="{Binding DigitCommand}"
                CommandParameter="9" />
        <Button Text="4"
                Grid.Row="3" Grid.Column="0"
                Command="{Binding DigitCommand}"
                CommandParameter="4" />
        <Button Text="5"
                Grid.Row="3" Grid.Column="1"
                Command="{Binding DigitCommand}"
                CommandParameter="5" />
        <Button Text="6"
                Grid.Row="3" Grid.Column="2"
                Command="{Binding DigitCommand}"
                CommandParameter="6" />
        <Button Text="1"
                Grid.Row="4" Grid.Column="0"
                Command="{Binding DigitCommand}"
                CommandParameter="1" />
        <Button Text="2"
                Grid.Row="4" Grid.Column="1"
                Command="{Binding DigitCommand}"
                CommandParameter="2" />
        <Button Text="3"
                Grid.Row="4" Grid.Column="2"
                Command="{Binding DigitCommand}"
                CommandParameter="3" />
        <Button Text="0"
                Grid.Row="5" Grid.Column="0" Grid.ColumnSpan="2"
                Command="{Binding DigitCommand}"
                CommandParameter="0" />
        <Button Text="&#x00B7;"
                Grid.Row="5" Grid.Column="2"
                Command="{Binding DigitCommand}"
                CommandParameter="." />
    </Grid>
</ContentPage>
```

In this example, the page's `BindingContext` is a `DecimalKeypadViewModel`. The <xref:Microsoft.Maui.Controls.Entry> property of this viewmodel is bound to the `Text` property of a <xref:Microsoft.Maui.Controls.Label>. All the <xref:Microsoft.Maui.Controls.Button> objects are bound to commands in the viewmodel: `ClearCommand`, `BackspaceCommand`, and `DigitCommand`. The 11 buttons for the 10 digits and the decimal point share a binding to `DigitCommand`. The `CommandParameter` distinguishes between these buttons. The value set to `CommandParameter` is generally the same as the text displayed by the button except for the decimal point, which for purposes of clarity is displayed with a middle dot character:

:::image type="content" source="media/commanding/decimalkeyboard.png" alt-text="Decimal keyboard.":::

The `DecimalKeypadViewModel` defines an <xref:Microsoft.Maui.Controls.Entry> property of type `string` and three properties of type <xref:System.Windows.Input.ICommand>:

```csharp
public class DecimalKeypadViewModel : INotifyPropertyChanged
{
    string entry = "0";

    public event PropertyChangedEventHandler PropertyChanged;
    ···

    public string Entry
    {
        private set
        {
            if (entry != value)
            {
                entry = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Entry"));
            }
        }
        get
        {
            return entry;
        }
    }

    public ICommand ClearCommand { private set; get; }
    public ICommand BackspaceCommand { private set; get; }
    public ICommand DigitCommand { private set; get; }
}
```

The button corresponding to the `ClearCommand` is always enabled and sets the entry back to "0":

```csharp
public class DecimalKeypadViewModel : INotifyPropertyChanged
{
    ···
    public DecimalKeypadViewModel()
    {
        ClearCommand = new Command(
            execute: () =>
            {
                Entry = "0";
                RefreshCanExecutes();
            });
        ···
    }

    void RefreshCanExecutes()
    {
        ((Command)BackspaceCommand).ChangeCanExecute();
        ((Command)DigitCommand).ChangeCanExecute();
    }
    ···
}
```

Because the button is always enabled, it is not necessary to specify a `canExecute` argument in the `Command` constructor.

The **Backspace** button is enabled only when the length of the entry is greater than 1, or if <xref:Microsoft.Maui.Controls.Entry> is not equal to the string "0":

```csharp
public class DecimalKeypadViewModel : INotifyPropertyChanged
{
    ···
    public DecimalKeypadViewModel()
    {
        ···
        BackspaceCommand = new Command(
            execute: () =>
            {
                Entry = Entry.Substring(0, Entry.Length - 1);
                if (Entry == "")
                {
                    Entry = "0";
                }
                RefreshCanExecutes();
            },
            canExecute: () =>
            {
                return Entry.Length > 1 || Entry != "0";
            });
        ···
    }
    ···
}
```

The logic for the `execute` function for the **Backspace** button ensures that the <xref:Microsoft.Maui.Controls.Entry> is at least a string of "0".

The `DigitCommand` property is bound to 11 buttons, each of which identifies itself with the `CommandParameter` property. The `DigitCommand` is set to an instance of the `Command<T>` class. When using the commanding interface with XAML, the `CommandParameter` properties are usually strings, which is type of the generic argument. The `execute` and `canExecute` functions then have arguments of type `string`:

```csharp
public class DecimalKeypadViewModel : INotifyPropertyChanged
{
    ···
    public DecimalKeypadViewModel()
    {
        ···
        DigitCommand = new Command<string>(
            execute: (string arg) =>
            {
                Entry += arg;
                if (Entry.StartsWith("0") && !Entry.StartsWith("0."))
                {
                    Entry = Entry.Substring(1);
                }
                RefreshCanExecutes();
            },
            canExecute: (string arg) =>
            {
                return !(arg == "." && Entry.Contains("."));
            });
    }
    ···
}
```

The `execute` method appends the string argument to the <xref:Microsoft.Maui.Controls.Entry> property. However, if the result begins with a zero (but not a zero and a decimal point) then that initial zero must be removed using the `Substring` function. The `canExecute` method returns `false` only if the argument is the decimal point (indicating that the decimal point is being pressed) and <xref:Microsoft.Maui.Controls.Entry> already contains a decimal point. All the `execute` methods call `RefreshCanExecutes`, which then calls `ChangeCanExecute` for both `DigitCommand` and `ClearCommand`. This ensures that the decimal point and backspace buttons are enabled or disabled based on the current sequence of entered digits.
