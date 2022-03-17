---
title: "Triggers"
description: "Triggers allow you to express actions declaratively in XAML that change the appearance of controls based on events or property changes."
ms.date: 02/18/2022
---

# Triggers

.NET Multi-platform App UI (.NET MAUI) triggers allow you to express actions declaratively in XAML that change the appearance of controls based on events or data changes. In addition, state triggers, which are a specialized group of triggers, define when a `VisualState` should be applied.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

You can assign a trigger directly to a control's `Triggers` collection, or add it to a page-level or app-level resource dictionary to be applied to multiple controls.

## Property triggers

A `Trigger` represents a trigger that applies property values, or performs actions, when the specified property meets a specified condition.

The following example shows a `Trigger` that changes an `Entry` background color when it receives focus:

```xaml
<Entry Placeholder="Enter name">
    <Entry.Triggers>
        <Trigger TargetType="Entry"
                 Property="IsFocused"
                 Value="True">
            <Setter Property="BackgroundColor"
                    Value="Yellow" />
            <!-- Multiple Setter elements are allowed -->
        </Trigger>
    </Entry.Triggers>
</Entry>
```

The trigger's declaration specifies the following:

- `TargetType` - the control type that the trigger applies to.
- `Property` - the property on the control that is monitored.
- `Value` - the value, when it occurs for the monitored property, that causes the trigger to activate.
- `Setter` - a collection of `Setter` elements that are applied when the trigger condition is met.

In addition, optional `EnterActions` and `ExitActions` collections can be specified. For more information, see [EnterActions and ExitActions](#enteractions-and-exitactions).

### Apply a trigger using a style

Triggers can also be added to a `Style` declaration on a control, in a page, or an application `ResourceDictionary`. The following example declares an *implicit* style for all `Entry` controls on the page:

```xaml
<ContentPage.Resources>
    <Style TargetType="Entry">
        <Style.Triggers>
            <Trigger TargetType="Entry"
                     Property="IsFocused"
                     Value="True">
                <Setter Property="BackgroundColor"
                        Value="Yellow" />
                <!-- Multiple Setter elements are allowed -->
            </Trigger>
        </Style.Triggers>
    </Style>
</ContentPage.Resources>
```

## Data triggers

A `DataTrigger` represents a trigger that applies property values, or performs actions, when the bound data meets a specified condition. The `Binding` markup extension is used to monitor for the specified condition.

The following example shows a `DataTrigger` that disables a `Button` when the `Entry` is empty:

```xaml
<Entry x:Name="entry"
       Text=""
       Placeholder="Enter text" />
<Button Text="Save">
    <Button.Triggers>
        <DataTrigger TargetType="Button"
                     Binding="{Binding Source={x:Reference entry},
                                       Path=Text.Length}"
                     Value="0">
            <Setter Property="IsEnabled"
                    Value="False" />
            <!-- Multiple Setter elements are allowed -->
        </DataTrigger>
    </Button.Triggers>
</Button>
```

In this example, when the length of the `Entry` is zero, the trigger is activated.

> [!TIP]
> When evaluating `Path=Text.Length` always provide a default value for the target property (eg. `Text=""`> because otherwise it will be `null` and the trigger won't work like you expect.

In addition, optional `EnterActions` and `ExitActions` collections can be specified. For more information, see [EnterActions and ExitActions](#enteractions-and-exitactions).

## Event triggers

An `EventTrigger` represents a trigger that applies a set of actions in response to an event. Unlike `Trigger`, `EventTrigger` has no concept of termination of state, so the actions will not be undone once the condition that raised the event is no longer true.

An `EventTrigger` only requires an `Event` property to be set:

```xaml
<EventTrigger Event="TextChanged">
    <local:NumericValidationTriggerAction />
</EventTrigger>
```

In this example, there are no `Setter` elements. Instead, there's a `NumericalValidationTriggerAction` object.

> [!NOTE]
> Event triggers don't support `EnterActions` and `ExitActions`.

A trigger action implementation must:

- Implement the generic `TriggerAction<T>` class, with the generic parameter corresponding with the type of control the trigger will be applied to. You can use classes such as `VisualElement` to write trigger actions that work with a variety of controls, or specify a control type like `Entry`.
- Override the `Invoke` method. This method is called whenever the trigger event occurs.
- Optionally expose properties that can be set in XAML when the trigger is declared.

The following example shows the `NumericValidationTriggerAction` class:

```csharp
public class NumericValidationTriggerAction : TriggerAction<Entry>
{
    protected override void Invoke(Entry entry)
    {
        double result;
        bool isValid = Double.TryParse(entry.Text, out result);
        entry.TextColor = isValid ? Colors.Black : Colors.Red;
    }
}
```

> [!WARNING]
> Be careful when sharing triggers in a `ResourceDictionary`. One instance will be shared among controls so any state that is configured once will apply to them all.

## Multi-triggers

A `MultiTrigger` represents a trigger that applies property values, or performs actions, when a set of conditions are satisfied. All the conditions must be true before the `Setter` objects are applied.

The following example shows a `MultiTrigger` that binds to two `Entry` objects:

```xaml
<Entry x:Name="email"
       Text="" />
<Entry x:Name="phone"
       Text="" />
<Button Text="Save">
    <Button.Triggers>
        <MultiTrigger TargetType="Button">
            <MultiTrigger.Conditions>
                <BindingCondition Binding="{Binding Source={x:Reference email},
                                            Path=Text.Length}"
                                  Value="0" />
                <BindingCondition Binding="{Binding Source={x:Reference phone},
                                            Path=Text.Length}"
                                  Value="0" />
            </MultiTrigger.Conditions>
            <Setter Property="IsEnabled" Value="False" />
            <!-- multiple Setter elements are allowed -->
        </MultiTrigger>
    </Button.Triggers>
</Button>
```

In addition, the `MultiTrigger.Conditions` collection can also contain `PropertyCondition` objects:

```xaml
<PropertyCondition Property="Text"
                   Value="OK" />
```

## EnterActions and ExitActions

An alternative approach to implementing changes when a trigger occurs is by specifying `EnterActions` and `ExitActions` collections, and creating `TriggerAction<T>` implementations.

The `EnterActions` collection, of type `IList<TriggerAction>`, defines a collection that will be invoked when the trigger condition is met. The `ExitActions` collection, of type `IList<TriggerAction>`, defines a collection that will be invoked after the trigger condition is no longer met.

> [!NOTE]
> The `TriggerAction` objects defined in the `EnterActions` and `ExitActions` collections are ignored by the `EventTrigger` class.

The following example shows a property trigger that specifies an `EnterAction` and an `ExitAction`:

```xaml
<Entry Placeholder="Enter job title">
    <Entry.Triggers>
        <Trigger TargetType="Entry"
                 Property="Entry.IsFocused"
                 Value="True">
            <Trigger.EnterActions>
                <local:FadeTriggerAction StartsFrom="0" />
            </Trigger.EnterActions>

            <Trigger.ExitActions>
                <local:FadeTriggerAction StartsFrom="1" />
            </Trigger.ExitActions>
        </Trigger>
    </Entry.Triggers>
</Entry>
```

A trigger action implementation must:

- Implement the generic `TriggerAction<T>` class, with the generic parameter corresponding with the type of control the trigger will be applied to. You can use classes such as `VisualElement` to write trigger actions that work with a variety of controls, or specify a control type like `Entry`.
- Override the `Invoke` method. This method is called whenever the trigger event occurs.
- Optionally expose properties that can be set in XAML when the trigger is declared.

The following example shows the `FadeTriggerAction` class:

```csharp
public class FadeTriggerAction : TriggerAction<VisualElement>
{
    public int StartsFrom { get; set; }

    protected override void Invoke(VisualElement sender)
    {
        sender.Animate("FadeTriggerAction", new Animation((d) =>
        {
            var val = StartsFrom == 1 ? d : 1 - d;
            sender.BackgroundColor = Color.FromRgb(1, val, 1);
        }),
        length: 1000, // milliseconds
        easing: Easing.Linear);
    }
}
```

> [!NOTE]
> You can provide `EnterActions` and `ExitActions` as well as `Setter` objects in a trigger, but be aware that the `Setter` objects are called immediately (they do not wait for the `EnterAction` or `ExitAction` to complete).

## State triggers

State triggers are a specialized group of triggers that define the conditions under which a `VisualState` should be applied.

State triggers are added to the `StateTriggers` collection of a `VisualState`. This collection can contain a single state trigger, or multiple state triggers. A `VisualState` will be applied when any state triggers in the collection are active.

When using state triggers to control visual states, .NET MAUI uses the following precedence rules to determine which trigger (and corresponding `VisualState`) will be active:

1. Any trigger that derives from `StateTriggerBase`.
1. An `AdaptiveTrigger` activated due to the `MinWindowWidth` condition being met.
1. An `AdaptiveTrigger` activated due to the `MinWindowHeight` condition being met.

If multiple triggers are simultaneously active (for example, two custom triggers) then the first trigger declared in the markup takes precedence.

> [!NOTE]
> State triggers can be set in a `Style`, or directly on elements.

<!-- For more information about visual states, see [Visual State Manager](~/user-interface/visual-state-manager.md). -->

### State trigger

The `StateTrigger` class, which derives from the `StateTriggerBase` class, has an `IsActive` bindable property. A `StateTrigger` triggers a `VisualState` change when the `IsActive` property changes value.

The `StateTriggerBase` class, which is the base class for all state triggers, has an `IsActive` property and an `IsActiveChanged` event. This event fires whenever a `VisualState` change occurs. In addition, the `StateTriggerBase` class has overridable `OnAttached` and `OnDetached` methods.

> [!IMPORTANT]
> The `StateTrigger.IsActive` bindable property hides the inherited `StateTriggerBase.IsActive` property.

The following XAML example shows a `Style` that includes `StateTrigger` objects:

```xaml
<Style TargetType="Grid">
    <Setter Property="VisualStateManager.VisualStateGroups">
        <VisualStateGroupList>
            <VisualStateGroup>
                <VisualState x:Name="Checked">
                    <VisualState.StateTriggers>
                        <StateTrigger IsActive="{Binding IsToggled}"
                                      IsActiveChanged="OnCheckedStateIsActiveChanged" />
                    </VisualState.StateTriggers>
                    <VisualState.Setters>
                        <Setter Property="BackgroundColor"
                                Value="Black" />
                    </VisualState.Setters>
                </VisualState>
                <VisualState x:Name="Unchecked">
                    <VisualState.StateTriggers>
                        <StateTrigger IsActive="{Binding IsToggled, Converter={StaticResource inverseBooleanConverter}}"
                                      IsActiveChanged="OnUncheckedStateIsActiveChanged" />
                    </VisualState.StateTriggers>
                    <VisualState.Setters>
                        <Setter Property="BackgroundColor"
                                Value="White" />
                    </VisualState.Setters>
                </VisualState>
            </VisualStateGroup>
        </VisualStateGroupList>
    </Setter>
</Style>
```

In this example, the implicit `Style` targets `Grid` objects. When the `IsToggled` property of the bound object is `true`, the background color of the `Grid` is set to black. When the `IsToggled` property of the bound object becomes `false`, a `VisualState` change is triggered, and the background color of the `Grid` becomes white.

In addition, every time a `VisualState` change occurs, the `IsActiveChanged` event for the `VisualState` is fired. Each `VisualState` registers an event handler for this event:

```csharp
void OnCheckedStateIsActiveChanged(object sender, EventArgs e)
{
    StateTriggerBase stateTrigger = sender as StateTriggerBase;
    Console.WriteLine($"Checked state active: {stateTrigger.IsActive}");
}

void OnUncheckedStateIsActiveChanged(object sender, EventArgs e)
{
    StateTriggerBase stateTrigger = sender as StateTriggerBase;
    Console.WriteLine($"Unchecked state active: {stateTrigger.IsActive}");
}
```

In this example, when a handler for the `IsActiveChanged` event is fired, the handler outputs whether the `VisualState` is active or not. For example, the following messages are output to the console window when changing from the `Checked` visual state to the `Unchecked` visual state:

```
Checked state active: False
Unchecked state active: True
```

> [!NOTE]
> Custom state triggers can be created by deriving from the `StateTriggerBase` class, and overriding the `OnAttached` and `OnDetached` methods to perform any required registrations and cleanup.

### Adaptive trigger

An `AdaptiveTrigger` triggers a `VisualState` change when the window is a specified height or width. This trigger has two bindable properties:

- `MinWindowHeight`, of type `double`, which indicates the minimum window height at which the `VisualState` should be applied.
- `MinWindowWidth`, of type `double`, which indicates the minimum window width at which the `VisualState` should be applied.

> [!NOTE]
> The `AdaptiveTrigger` derives from the `StateTriggerBase` class and can therefore attach an event handler to the `IsActiveChanged` event.

The following XAML example shows a `Style` that includes `AdaptiveTrigger` objects:

```xaml
<Style TargetType="StackLayout">
    <Setter Property="VisualStateManager.VisualStateGroups">
        <VisualStateGroupList>
            <VisualStateGroup>
                <VisualState x:Name="Vertical">
                    <VisualState.StateTriggers>
                        <AdaptiveTrigger MinWindowWidth="0" />
                    </VisualState.StateTriggers>
                    <VisualState.Setters>
                        <Setter Property="Orientation"
                                Value="Vertical" />
                    </VisualState.Setters>
                </VisualState>
                <VisualState x:Name="Horizontal">
                    <VisualState.StateTriggers>
                        <AdaptiveTrigger MinWindowWidth="800" />
                    </VisualState.StateTriggers>
                    <VisualState.Setters>
                        <Setter Property="Orientation"
                                Value="Horizontal" />
                    </VisualState.Setters>
                </VisualState>
            </VisualStateGroup>
        </VisualStateGroupList>
    </Setter>
</Style>
```

In this example, the implicit `Style` targets `StackLayout` objects. When the window width is between 0 and 800 device-independent units, `StackLayout` objects to which the `Style` is applied will have a vertical orientation. When the window width is >= 800 device-independent units, the `VisualState` change is triggered, and the `StackLayout` orientation changes to horizontal.

The `MinWindowHeight` and `MinWindowWidth` properties can be used independently or in conjunction with each other. The following XAML shows an example of setting both properties:

```xaml
<AdaptiveTrigger MinWindowWidth="800"
                 MinWindowHeight="1200"/>
```

In this example, the `AdaptiveTrigger` indicates that the corresponding `VisualState` will be applied when the current window width is >= 800 device-independent units and the current window height is >= 1200 device-independent units.

### Compare state trigger

The `CompareStateTrigger` triggers a `VisualState` change when a property is equal to a specific value. This trigger has two bindable properties:

- `Property`, of type `object`, which indicates the property being compared by the trigger.
- `Value`, of type `object`, which indicates the value at which the `VisualState` should be applied.

> [!NOTE]
> The `CompareStateTrigger` derives from the `StateTriggerBase` class and can therefore attach an event handler to the `IsActiveChanged` event.

The following XAML example shows a `Style` that includes `CompareStateTrigger` objects:

```xaml
<Style TargetType="Grid">
    <Setter Property="VisualStateManager.VisualStateGroups">
        <VisualStateGroupList>
            <VisualStateGroup>
                <VisualState x:Name="Checked">
                    <VisualState.StateTriggers>
                        <CompareStateTrigger Property="{Binding Source={x:Reference checkBox}, Path=IsChecked}"
                                             Value="True" />
                    </VisualState.StateTriggers>
                    <VisualState.Setters>
                        <Setter Property="BackgroundColor"
                                Value="Black" />
                    </VisualState.Setters>
                </VisualState>
                <VisualState x:Name="Unchecked">
                    <VisualState.StateTriggers>
                        <CompareStateTrigger Property="{Binding Source={x:Reference checkBox}, Path=IsChecked}"
                                             Value="False" />
                    </VisualState.StateTriggers>
                    <VisualState.Setters>
                        <Setter Property="BackgroundColor"
                                Value="White" />
                    </VisualState.Setters>
                </VisualState>
            </VisualStateGroup>
        </VisualStateGroupList>
    </Setter>
</Style>
...
<Grid>
    <Frame BackgroundColor="White"
           CornerRadius="12"
           Margin="24"
           HorizontalOptions="Center"
           VerticalOptions="Center">
        <StackLayout Orientation="Horizontal">
            <CheckBox x:Name="checkBox"
                      VerticalOptions="Center" />
            <Label Text="Check the CheckBox to modify the Grid background color."
                   VerticalOptions="Center" />
        </StackLayout>
    </Frame>
</Grid>
```

In this example, the implicit `Style` targets `Grid` objects. When the `IsChecked` property of the `CheckBox` is `false`, the background color of the `Grid` is set to white. When the `CheckBox.IsChecked` property becomes `true`, a `VisualState` change is triggered, and the background color of the `Grid` becomes black.

### Device state trigger

The `DeviceStateTrigger` triggers a `VisualState` change based on the device platform the app is running on. This trigger has a single bindable property:

- `Device`, of type `string`, which indicates the device platform on which the `VisualState` should be applied.

> [!NOTE]
> The `DeviceStateTrigger` derives from the `StateTriggerBase` class and can therefore attach an event handler to the `IsActiveChanged` event.

The following XAML example shows a `Style` that includes `DeviceStateTrigger` objects:

```xaml
<Style x:Key="DeviceStateTriggerPageStyle"
       TargetType="ContentPage">
    <Setter Property="VisualStateManager.VisualStateGroups">
        <VisualStateGroupList>
            <VisualStateGroup>
                <VisualState x:Name="iOS">
                    <VisualState.StateTriggers>
                        <DeviceStateTrigger Device="iOS" />
                    </VisualState.StateTriggers>
                    <VisualState.Setters>
                        <Setter Property="BackgroundColor"
                                Value="Silver" />
                    </VisualState.Setters>
                </VisualState>
                <VisualState x:Name="Android">
                    <VisualState.StateTriggers>
                        <DeviceStateTrigger Device="Android" />
                    </VisualState.StateTriggers>
                    <VisualState.Setters>
                        <Setter Property="BackgroundColor"
                                Value="#2196F3" />
                    </VisualState.Setters>
                </VisualState>
            </VisualStateGroup>
        </VisualStateGroupList>
    </Setter>
</Style>
```

In this example, the explicit `Style` targets `ContentPage` objects. `ContentPage` objects that consume the style set their background color to silver on iOS, and to pale blue on Android.

### Orientation state trigger

The `OrientationStateTrigger` triggers a `VisualState` change when the orientation of the device changes. This trigger has a single bindable property:

- `Orientation`, of type `DeviceOrientation`, which indicates the orientation to which the `VisualState` should be applied.

> [!NOTE]
> The `OrientationStateTrigger` derives from the `StateTriggerBase` class and can therefore attach an event handler to the `IsActiveChanged` event.

The following XAML example shows a `Style` that includes `OrientationStateTrigger` objects:

```xaml
<Style x:Key="OrientationStateTriggerPageStyle"
       TargetType="ContentPage">
    <Setter Property="VisualStateManager.VisualStateGroups">
        <VisualStateGroupList>
            <VisualStateGroup>
                <VisualState x:Name="Portrait">
                    <VisualState.StateTriggers>
                        <OrientationStateTrigger Orientation="Portrait" />
                    </VisualState.StateTriggers>
                    <VisualState.Setters>
                        <Setter Property="BackgroundColor"
                                Value="Silver" />
                    </VisualState.Setters>
                </VisualState>
                <VisualState x:Name="Landscape">
                    <VisualState.StateTriggers>
                        <OrientationStateTrigger Orientation="Landscape" />
                    </VisualState.StateTriggers>
                    <VisualState.Setters>
                        <Setter Property="BackgroundColor"
                                Value="White" />
                    </VisualState.Setters>
                </VisualState>
            </VisualStateGroup>
        </VisualStateGroupList>
    </Setter>
</Style>
```

In this example, the explicit `Style` targets `ContentPage` objects. `ContentPage` objects that consume the style set their background color to silver when the orientation is portrait, and set their background color to white when the orientation is landscape.
