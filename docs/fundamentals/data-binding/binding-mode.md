---
title: "Binding Mode"
description: "This article explains how to control the flow of information between source and target using a binding mode, which is specified with a member of the BindingMode enumeration. Every bindable property has a default binding mode, which indicates the mode in effect when that property is a data-binding target."
ms.date: 01/19/2022
---

# Binding Mode

A .NET Multi-platform App UI (.NET MAUI) 
In the [previous article](basic-bindings.md), the **Alternative Code Binding** and **Alternative XAML Binding** pages featured a `Label` with its `Scale` property bound to the `Value` property of a `Slider`. Because the `Slider` initial value is 0, this caused the `Scale` property of the `Label` to be set to 0 rather than 1, and the `Label` disappeared.
[!INCLUDE [docs under construction](~/includes/preview-note.md)]

In the [**DataBindingDemos**](/samples/xamarin/xamarin-forms-samples/databindingdemos) sample, the **Reverse Binding** page is similar to the programs in the previous article, except that the data binding is defined on the `Slider` rather than on the `Label`:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="DataBindingDemos.ReverseBindingPage"
             Title="Reverse Binding">
    <StackLayout Padding="10, 0">

        <Label x:Name="label"
               Text="TEXT"
               FontSize="80"
               HorizontalOptions="Center"
               VerticalOptions="CenterAndExpand" />

        <Slider x:Name="slider"
                VerticalOptions="CenterAndExpand"
                Value="{Binding Source={x:Reference label},
                                Path=Opacity}" />
    </StackLayout>
</ContentPage>
```

At first, this might seem backwards: Now the `Label` is the data-binding source, and the `Slider` is the target. The binding references the `Opacity` property of the `Label`, which has a default value of 1.

As you might expect, the `Slider` is initialized to the value 1 from the initial `Opacity` value of `Label`. This is shown in the following screenshot:

![Reverse Binding.](binding-mode-images/reversebinding.png)

But you might be surprised that the `Slider` continues to work. This seems to suggest that the data binding works better when the `Slider` is the binding target rather than the `Label` because the initialization works like we might expect.

The difference between the **Reverse Binding** sample and the earlier samples involves the *binding mode*.

## The Default Binding Mode

The binding mode is specified with a member of the `BindingMode` enumeration:

- `Default`
- `TwoWay` &ndash; data goes both ways between source and target
- `OneWay` &ndash; data goes from source to target
- `OneWayToSource` &ndash; data goes from target to source
- `OneTime` &ndash; data goes from source to target, but only when the `BindingContext` changes (new with .NET MAUI 3.0)

Every bindable property has a default binding mode that is set when the bindable property is created, and which is available from the `DefaultBindingMode` property of the `BindableProperty` object. This default binding mode indicates the mode in effect when that property is a data-binding target.

The default binding mode for most properties such as `Rotation`, `Scale`, and `Opacity` is `OneWay`. When these properties are data-binding targets, then the target property is set from the source.

However, the default binding mode for the `Value` property of `Slider` is `TwoWay`. This means that when the `Value` property is a data-binding target, then the target is set from the source (as usual) but the source is also set from the target. This is what allows the `Slider` to be set from the initial `Opacity` value.

This two-way binding might seem to create an infinite loop, but that doesn't happen. Bindable properties do not signal a property change unless the property actually changes. This prevents an infinite loop.

### Two-Way Bindings

Most bindable properties have a default binding mode of `OneWay` but the following properties have a default binding mode of `TwoWay`:

- `Date` property of `DatePicker`
- `Text` property of `Editor`, `Entry`, `SearchBar`, and `EntryCell`
- `IsRefreshing` property of `ListView`
- `SelectedItem` property of `MultiPage`
- `SelectedIndex` and `SelectedItem` properties of `Picker`
- `Value` property of `Slider` and `Stepper`
- `IsToggled` property of `Switch`
- `On` property of `SwitchCell`
- `Time` property of `TimePicker`

These particular properties are defined as `TwoWay` for a very good reason:

When data bindings are used with the Model-View-ViewModel (MVVM) application architecture, the ViewModel class is the data-binding source, and the View, which consists of views such as `Slider`, are data-binding targets. MVVM bindings resemble the **Reverse Binding** sample more than the bindings in the previous samples. It is very likely that you want each view on the page to be initialized with the value of the corresponding property in the ViewModel, but changes in the view should also affect the ViewModel property.

The properties with default binding modes of `TwoWay` are those properties most likely to be used in MVVM scenarios.

### One-Way-to-Source Bindings

Read-only bindable properties have a default binding mode of `OneWayToSource`. There is only one read/write bindable property that has a default binding mode of `OneWayToSource`:

- `SelectedItem` property of `ListView`

The rationale is that a binding on the `SelectedItem` property should result in setting the binding source. An example later in this article overrides that behavior.

### One-Time Bindings

Several properties have a default binding mode of `OneTime`, including the `IsTextPredictionEnabled` property of `Entry`.

Target properties with a binding mode of `OneTime` are updated only when the binding context changes. For bindings on these target properties, this simplifies the binding infrastructure because it is not necessary to monitor changes in the source properties.

## ViewModels and Property-Change Notifications

The **Simple Color Selector** page demonstrates the use of a simple ViewModel. Data bindings allow the user to select a color using three `Slider` elements for the hue, saturation, and luminosity.

The ViewModel is the data-binding source. The ViewModel does *not* define bindable properties, but it does implement a notification mechanism that allows the binding infrastructure to be notified when the value of a property changes. This notification mechanism is the [`INotifyPropertyChanged`](xref:System.ComponentModel.INotifyPropertyChanged) interface, which defines a single event named [`PropertyChanged`](xref:System.ComponentModel.INotifyPropertyChanged.PropertyChanged). A class that implements this interface generally fires the event when one of its public properties changes value. The event does not need to be fired if the property never changes. (The `INotifyPropertyChanged` interface is also implemented by `BindableObject` and a `PropertyChanged` event is fired whenever a bindable property changes value.)

The `HslColorViewModel` class defines five properties: The `Hue`, `Saturation`, `Luminosity`, and `Color` properties are interrelated. When any one of the three color components changes value, the `Color` property is recalculated, and `PropertyChanged` events are fired for all four properties:

```csharp
public class HslColorViewModel : INotifyPropertyChanged
{
    Color color;
    string name;

    public event PropertyChangedEventHandler PropertyChanged;

    public double Hue
    {
        set
        {
            if (color.Hue != value)
            {
                Color = Color.FromHsla(value, color.Saturation, color.Luminosity);
            }
        }
        get
        {
            return color.Hue;
        }
    }

    public double Saturation
    {
        set
        {
            if (color.Saturation != value)
            {
                Color = Color.FromHsla(color.Hue, value, color.Luminosity);
            }
        }
        get
        {
            return color.Saturation;
        }
    }

    public double Luminosity
    {
        set
        {
            if (color.Luminosity != value)
            {
                Color = Color.FromHsla(color.Hue, color.Saturation, value);
            }
        }
        get
        {
            return color.Luminosity;
        }
    }

    public Color Color
    {
        set
        {
            if (color != value)
            {
                color = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hue"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Saturation"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Luminosity"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color"));

                Name = NamedColor.GetNearestColorName(color);
            }
        }
        get
        {
            return color;
        }
    }

    public string Name
    {
        private set
        {
            if (name != value)
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }
        get
        {
            return name;
        }
    }
}
```

When the `Color` property changes, the static `GetNearestColorName` method in the `NamedColor` class (also included in the **DataBindingDemos** solution) obtains the closest named color and sets the `Name` property. This `Name` property has a private `set` accessor, so it cannot be set from outside the class.

When a ViewModel is set as a binding source, the binding infrastructure attaches a handler to the `PropertyChanged` event. In this way, the binding can be notified of changes to the properties, and can then set the target properties from the changed values.

However, when a target property (or the `Binding` definition on a target property) has a `BindingMode` of `OneTime`, it is not necessary for the binding infrastructure to attach a handler on the `PropertyChanged` event. The target property is updated only when the `BindingContext` changes and not when the source property itself changes.

The **Simple Color Selector** XAML file instantiates the `HslColorViewModel` in the page's resource dictionary and initializes the `Color` property. The `BindingContext` property of the `Grid` is set to a `StaticResource` binding extension to reference that resource:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:DataBindingDemos"
             x:Class="DataBindingDemos.SimpleColorSelectorPage">

    <ContentPage.Resources>
        <ResourceDictionary>
            <local:HslColorViewModel x:Key="viewModel"
                                     Color="MediumTurquoise" />

            <Style TargetType="Slider">
                <Setter Property="VerticalOptions" Value="CenterAndExpand" />
            </Style>
        </ResourceDictionary>
    </ContentPage.Resources>

    <Grid BindingContext="{StaticResource viewModel}">
        <Grid.RowDefinitions>
            <RowDefinition Height="*" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>

        <BoxView Color="{Binding Color}"
                 Grid.Row="0" />

        <StackLayout Grid.Row="1"
                     Margin="10, 0">

            <Label Text="{Binding Name}"
                   HorizontalTextAlignment="Center" />

            <Slider Value="{Binding Hue}" />

            <Slider Value="{Binding Saturation}" />

            <Slider Value="{Binding Luminosity}" />
        </StackLayout>
    </Grid>
</ContentPage>
```

The `BoxView`, `Label`, and three `Slider` views inherit the binding context from the `Grid`. These views are all binding targets that reference source properties in the ViewModel. For the `Color` property of the `BoxView`, and the `Text` property of the `Label`, the data bindings are `OneWay`: The properties in the view are set from the properties in the ViewModel.

The `Value` property of the `Slider`, however, is `TwoWay`. This allows each `Slider` to be set from the ViewModel, and also for the ViewModel to be set from each `Slider`.

When the program is first run, the `BoxView`, `Label`, and three `Slider` elements are all set from the ViewModel based on the initial `Color` property set when the ViewModel was instantiated. This is shown in the following screenshot:

![Simple Color Selector.](binding-mode-images/simplecolorselector.png)

As you manipulate the sliders, the `BoxView` and `Label` are updated accordingly.

Instantiating the ViewModel in the resource dictionary is one common approach. It's also possible to instantiate the ViewModel within property element tags for the `BindingContext` property. In the **Simple Color Selector** XAML file, try removing the `HslColorViewModel` from the resource dictionary and set it to the `BindingContext` property of the `Grid` like this:

```xaml
<Grid>
    <Grid.BindingContext>
        <local:HslColorViewModel Color="MediumTurquoise" />
    </Grid.BindingContext>

    ···

</Grid>
```

The binding context can be set in a variety of ways. Sometimes, the code-behind file instantiates the ViewModel and sets it to the `BindingContext` property of the page. These are all valid approaches.

## Overriding the Binding Mode

If the default binding mode on the target property is not suitable for a particular data binding, it's possible to override it by setting the `Mode` property of `Binding` (or the `Mode` property of the `Binding` markup extension) to one of the members of the `BindingMode` enumeration.

However, setting the `Mode` property to `TwoWay` doesn't always work as you might expect. For example, try modifying the **Alternative XAML Binding** XAML file to include `TwoWay` in the binding definition:

```xaml
<Label Text="TEXT"
       FontSize="40"
       HorizontalOptions="Center"
       VerticalOptions="CenterAndExpand"
       Scale="{Binding Source={x:Reference slider},
                       Path=Value,
                       Mode=TwoWay}" />
```

It might be expected that the `Slider` would be initialized to the initial value of the `Scale` property, which is 1, but that doesn't happen. When a `TwoWay` binding is initialized, the target is set from the source first, which means that the `Scale` property is set to the `Slider` default value of 0. When the `TwoWay` binding is set on the `Slider`, then the `Slider` is initially set from the source.

You can set the binding mode to `OneWayToSource` in the **Alternative XAML Binding** sample:

```xaml
<Label Text="TEXT"
       FontSize="40"
       HorizontalOptions="Center"
       VerticalOptions="CenterAndExpand"
       Scale="{Binding Source={x:Reference slider},
                       Path=Value,
                       Mode=OneWayToSource}" />
```

Now the `Slider` is initialized to 1 (the default value of `Scale` but manipulating the `Slider` doesn't affect the `Scale` property, so this is not very useful.

> [!NOTE]
> The `VisualElement` class also defines `ScaleX` and `ScaleY` properties, which can scale the `VisualElement` differently in the horizontal and vertical directions.

A very useful application of overriding the default binding mode with `TwoWay` involves the `SelectedItem` property of `ListView`. The default binding mode is `OneWayToSource`. When a data binding is set on the `SelectedItem` property to reference a source property in a ViewModel, then that source property is set from the `ListView` selection. However, in some circumstances, you might also want the `ListView` to be initialized from the ViewModel.

The **Sample Settings** page demonstrates this technique. This page represents a simple implementation of application settings, which are very often defined in a ViewModel, such as this `SampleSettingsViewModel` file:

```csharp
public class SampleSettingsViewModel : INotifyPropertyChanged
{
    string name;
    DateTime birthDate;
    bool codesInCSharp;
    double numberOfCopies;
    NamedColor backgroundNamedColor;

    public event PropertyChangedEventHandler PropertyChanged;

    public SampleSettingsViewModel(IDictionary<string, object> dictionary)
    {
        Name = GetDictionaryEntry<string>(dictionary, "Name");
        BirthDate = GetDictionaryEntry(dictionary, "BirthDate", new DateTime(1980, 1, 1));
        CodesInCSharp = GetDictionaryEntry<bool>(dictionary, "CodesInCSharp");
        NumberOfCopies = GetDictionaryEntry(dictionary, "NumberOfCopies", 1.0);
        BackgroundNamedColor = NamedColor.Find(GetDictionaryEntry(dictionary, "BackgroundNamedColor", "White"));
    }

    public string Name
    {
        set { SetProperty(ref name, value); }
        get { return name; }
    }

    public DateTime BirthDate
    {
        set { SetProperty(ref birthDate, value); }
        get { return birthDate; }
    }

    public bool CodesInCSharp
    {
        set { SetProperty(ref codesInCSharp, value); }
        get { return codesInCSharp; }
    }

    public double NumberOfCopies
    {
        set { SetProperty(ref numberOfCopies, value); }
        get { return numberOfCopies; }
    }

    public NamedColor BackgroundNamedColor
    {
        set
        {
            if (SetProperty(ref backgroundNamedColor, value))
            {
                OnPropertyChanged("BackgroundColor");
            }
        }
        get { return backgroundNamedColor; }
    }

    public Color BackgroundColor
    {
        get { return BackgroundNamedColor?.Color ?? Color.White; }
    }

    public void SaveState(IDictionary<string, object> dictionary)
    {
        dictionary["Name"] = Name;
        dictionary["BirthDate"] = BirthDate;
        dictionary["CodesInCSharp"] = CodesInCSharp;
        dictionary["NumberOfCopies"] = NumberOfCopies;
        dictionary["BackgroundNamedColor"] = BackgroundNamedColor.Name;
    }

    T GetDictionaryEntry<T>(IDictionary<string, object> dictionary, string key, T defaultValue = default(T))
    {
        return dictionary.ContainsKey(key) ? (T)dictionary[key] : defaultValue;
    }

    bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (object.Equals(storage, value))
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

Each application setting is a property that is saved to the .NET MAUI properties dictionary in a method named `SaveState` and loaded from that dictionary in the constructor. Towards the bottom of the class are two methods that help streamline ViewModels and make them less prone to errors. The `OnPropertyChanged` method at the bottom has an optional parameter that is set to the calling property. This avoids spelling errors when specifying the name of the property as a string.

The `SetProperty` method in the class does even more: It compares the value that is being set to the property with the value stored as a field, and only calls `OnPropertyChanged` when the two values are not equal.

The `SampleSettingsViewModel` class defines two properties for the background color: The `BackgroundNamedColor` property is of type `NamedColor`, which is a class also included in the **DataBindingDemos** solution. The `BackgroundColor` property is of type `Color`, and is obtained from the `Color` property of the `NamedColor` object.

The `NamedColor` class uses .NET reflection to enumerate all the static public fields in the .NET MAUI `Color` structure, and to store them with their names in a collection accessible from the static `All` property:

```csharp
public class NamedColor : IEquatable<NamedColor>, IComparable<NamedColor>
{
    // Instance members
    private NamedColor()
    {
    }

    public string Name { private set; get; }

    public string FriendlyName { private set; get; }

    public Color Color { private set; get; }

    public string RgbDisplay { private set; get; }

    public bool Equals(NamedColor other)
    {
        return Name.Equals(other.Name);
    }

    public int CompareTo(NamedColor other)
    {
        return Name.CompareTo(other.Name);
    }

    // Static members
    static NamedColor()
    {
        List<NamedColor> all = new List<NamedColor>();
        StringBuilder stringBuilder = new StringBuilder();

        // Loop through the public static fields of the Color structure.
        foreach (FieldInfo fieldInfo in typeof(Color).GetRuntimeFields())
        {
            if (fieldInfo.IsPublic &&
                fieldInfo.IsStatic &&
                fieldInfo.FieldType == typeof(Color))
            {
                // Convert the name to a friendly name.
                string name = fieldInfo.Name;
                stringBuilder.Clear();
                int index = 0;

                foreach (char ch in name)
                {
                    if (index != 0 && Char.IsUpper(ch))
                    {
                        stringBuilder.Append(' ');
                    }
                    stringBuilder.Append(ch);
                    index++;
                }

                // Instantiate a NamedColor object.
                Color color = (Color)fieldInfo.GetValue(null);

                NamedColor namedColor = new NamedColor
                {
                    Name = name,
                    FriendlyName = stringBuilder.ToString(),
                    Color = color,
                    RgbDisplay = String.Format("{0:X2}-{1:X2}-{2:X2}",
                                                (int)(255 * color.R),
                                                (int)(255 * color.G),
                                                (int)(255 * color.B))
                };

                // Add it to the collection.
                all.Add(namedColor);
            }
        }
        all.TrimExcess();
        all.Sort();
        All = all;
    }

    public static IList<NamedColor> All { private set; get; }

    public static NamedColor Find(string name)
    {
        return ((List<NamedColor>)All).Find(nc => nc.Name == name);
    }

    public static string GetNearestColorName(Color color)
    {
        double shortestDistance = 1000;
        NamedColor closestColor = null;

        foreach (NamedColor namedColor in NamedColor.All)
        {
            double distance = Math.Sqrt(Math.Pow(color.R - namedColor.Color.R, 2) +
                                        Math.Pow(color.G - namedColor.Color.G, 2) +
                                        Math.Pow(color.B - namedColor.Color.B, 2));

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestColor = namedColor;
            }
        }
        return closestColor.Name;
    }
}
```

The `App` class in the **DataBindingDemos** project defines a property named `Settings` of type `SampleSettingsViewModel`. This property is initialized when the `App` class is instantiated, and the `SaveState` method is called when the `OnSleep` method is called:

```csharp
public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        Settings = new SampleSettingsViewModel(Current.Properties);

        MainPage = new NavigationPage(new MainPage());
    }

    public SampleSettingsViewModel Settings { private set; get; }

    protected override void OnStart()
    {
        // Handle when your app starts
    }

    protected override void OnSleep()
    {
        // Handle when your app sleeps
        Settings.SaveState(Current.Properties);
    }

    protected override void OnResume()
    {
        // Handle when your app resumes
    }
}
```

Almost everything else is handled in the **SampleSettingsPage.xaml** file. The `BindingContext` of the page is set using a `Binding` markup extension: The binding source is the static `Application.Current` property, which is the instance of the `App` class in the project, and the `Path` is set to the `Settings` property, which is the `SampleSettingsViewModel` object:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:DataBindingDemos"
             x:Class="DataBindingDemos.SampleSettingsPage"
             Title="Sample Settings"
             BindingContext="{Binding Source={x:Static Application.Current},
                                      Path=Settings}">

    <StackLayout BackgroundColor="{Binding BackgroundColor}"
                 Padding="10"
                 Spacing="10">

        <StackLayout Orientation="Horizontal">
            <Label Text="Name: "
                   VerticalOptions="Center" />

            <Entry Text="{Binding Name}"
                   Placeholder="your name"
                   HorizontalOptions="FillAndExpand"
                   VerticalOptions="Center" />
        </StackLayout>

        <StackLayout Orientation="Horizontal">
            <Label Text="Birth Date: "
                   VerticalOptions="Center" />

            <DatePicker Date="{Binding BirthDate}"
                        HorizontalOptions="FillAndExpand"
                        VerticalOptions="Center" />
        </StackLayout>

        <StackLayout Orientation="Horizontal">
            <Label Text="Do you code in C#? "
                   VerticalOptions="Center" />

            <Switch IsToggled="{Binding CodesInCSharp}"
                    VerticalOptions="Center" />
        </StackLayout>

        <StackLayout Orientation="Horizontal">
            <Label Text="Number of Copies: "
                   VerticalOptions="Center" />

            <Stepper Value="{Binding NumberOfCopies}"
                     VerticalOptions="Center" />

            <Label Text="{Binding NumberOfCopies}"
                   VerticalOptions="Center" />
        </StackLayout>

        <Label Text="Background Color:" />

        <ListView x:Name="colorListView"
                  ItemsSource="{x:Static local:NamedColor.All}"
                  SelectedItem="{Binding BackgroundNamedColor, Mode=TwoWay}"
                  VerticalOptions="FillAndExpand"
                  RowHeight="40">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <ViewCell>
                        <StackLayout Orientation="Horizontal">
                            <BoxView Color="{Binding Color}"
                                     HeightRequest="32"
                                     WidthRequest="32"
                                     VerticalOptions="Center" />

                            <Label Text="{Binding FriendlyName}"
                                   FontSize="24"
                                   VerticalOptions="Center" />
                        </StackLayout>                        
                    </ViewCell>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </StackLayout>
</ContentPage>
```

All the children of the page inherit the binding context. Most of the other bindings on this page are to properties in `SampleSettingsViewModel`. The `BackgroundColor` property is used to set the `BackgroundColor` property of the `StackLayout`, and the `Entry`, `DatePicker`, `Switch`, and `Stepper` properties are all bound to other properties in the ViewModel.

The `ItemsSource` property of the `ListView` is set to the static `NamedColor.All` property. This fills the `ListView` with all the `NamedColor` instances. For each item in the `ListView`, the binding context for the item is set to a `NamedColor` object. The `BoxView` and `Label` in the `ViewCell` are bound to properties in `NamedColor`.

The `SelectedItem` property of the `ListView` is of type `NamedColor`, and is bound to the `BackgroundNamedColor` property of `SampleSettingsViewModel`:

```xaml
SelectedItem="{Binding BackgroundNamedColor, Mode=TwoWay}"
```

The default binding mode for `SelectedItem` is `OneWayToSource`, which sets the ViewModel property from the selected item. The `TwoWay` mode allows the `SelectedItem` to be initialized from the ViewModel.

However, when the `SelectedItem` is set in this way, the `ListView` does not automatically scroll to show the selected item. A little code in the code-behind file is necessary:

```csharp
public partial class SampleSettingsPage : ContentPage
{
    public SampleSettingsPage()
    {
        InitializeComponent();

        if (colorListView.SelectedItem != null)
        {
            colorListView.ScrollTo(colorListView.SelectedItem,
                                   ScrollToPosition.MakeVisible,
                                   false);
        }
    }
}
```

The following screenshot shows the program when it's first run. The constructor in `SampleSettingsViewModel` initializes the background color to white, and that's what's selected in the `ListView`:

![Sample Settings.](binding-mode-images/samplesettings.png)

When experimenting with this page, remember to put the program to sleep or to terminate it on the device or emulator that it's running. Terminating the program from the Visual Studio debugger will not cause the `OnSleep` override in the `App` class to be called.

In the next article you'll see how to specify [**String Formatting**](string-formatting.md) of data bindings that are set on the `Text` property of `Label`.
