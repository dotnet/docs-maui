---
title: "Triggers"
description: "Triggers allow you to express actions declaratively in XAML that change the appearance of controls based on events or property changes."
ms.date: 01/13/2026
---

# Triggers

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-triggers)

.NET Multi-platform App UI (.NET MAUI) triggers allow you to express actions declaratively in XAML that change the appearance of controls based on events or data changes. In addition, state triggers, which are a specialized group of triggers, define when a <xref:Microsoft.Maui.Controls.VisualState> should be applied.

You can assign a trigger directly to a control's <xref:Microsoft.Maui.Controls.VisualElement.Triggers> collection, or add it to a page-level or app-level resource dictionary to be applied to multiple controls.

## Property triggers

A <xref:Microsoft.Maui.Controls.Trigger> represents a trigger that applies property values, or performs actions, when the specified property meets a specified condition.

The following example shows a <xref:Microsoft.Maui.Controls.Trigger> that changes an <xref:Microsoft.Maui.Controls.Entry> background color when it receives focus:

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

- <xref:Microsoft.Maui.Controls.TriggerBase.TargetType> - the control type that the trigger applies to.
- <xref:Microsoft.Maui.Controls.Trigger.Property> - the property on the control that is monitored.
- <xref:Microsoft.Maui.Controls.Trigger.Value> - the value, when it occurs for the monitored property, that causes the trigger to activate.
- <xref:Microsoft.Maui.Controls.Setter> - a collection of <xref:Microsoft.Maui.Controls.Setter> elements that are applied when the trigger condition is met.

In addition, optional <xref:Microsoft.Maui.Controls.TriggerBase.EnterActions> and <xref:Microsoft.Maui.Controls.TriggerBase.ExitActions> collections can be specified. For more information, see [EnterActions and ExitActions](#enteractions-and-exitactions).

The equivalent C# code is:

```csharp
Entry entry = new Entry { Placeholder = "Enter name" };
Trigger trigger = new Trigger(typeof(Entry))
{
    Property = Entry.IsFocusedProperty,
    Value = true
};
trigger.Setters.Add(new Setter
{
    Property = Entry.BackgroundColorProperty,
    Value = Colors.Yellow
});
entry.Triggers.Add(trigger);
```

### Apply a trigger using a style

Triggers can also be added to a <xref:Microsoft.Maui.Controls.Style> declaration on a control, in a page, or an application <xref:Microsoft.Maui.Controls.ResourceDictionary>. The following example declares an *implicit* style for all <xref:Microsoft.Maui.Controls.Entry> controls on the page:

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

The equivalent C# code is:

```csharp
Style entryStyle = new Style(typeof(Entry));
Trigger trigger = new Trigger(typeof(Entry))
{
    Property = Entry.IsFocusedProperty,
    Value = true
};
trigger.Setters.Add(new Setter
{
    Property = Entry.BackgroundColorProperty,
    Value = Colors.Yellow
});
entryStyle.Triggers.Add(trigger);
Resources.Add(entryStyle);
```

## Data triggers

A <xref:Microsoft.Maui.Controls.DataTrigger> represents a trigger that applies property values, or performs actions, when the bound data meets a specified condition. The `Binding` markup extension is used to monitor for the specified condition.

The following example shows a <xref:Microsoft.Maui.Controls.DataTrigger> that disables a <xref:Microsoft.Maui.Controls.Button> when the <xref:Microsoft.Maui.Controls.Entry> is empty:

```xaml
<Entry x:Name="entry"
       Text=""
       Placeholder="Enter text" />
<Button Text="Save">
    <Button.Triggers>
        <DataTrigger TargetType="Button"
                     Binding="{Binding x:DataType='Entry',
                                       Source={x:Reference entry},
                                       Path=Text.Length}"
                     Value="0">
            <Setter Property="IsEnabled"
                    Value="False" />
            <!-- Multiple Setter elements are allowed -->
        </DataTrigger>
    </Button.Triggers>
</Button>
```

In this example, when the length of the <xref:Microsoft.Maui.Controls.Entry> is zero, the trigger is activated.

> [!TIP]
> When evaluating `Path=Text.Length` always provide a default value for the target property (eg. `Text=""`) because otherwise it will be `null` and the trigger won't work like you expect.

In addition, optional <xref:Microsoft.Maui.Controls.TriggerBase.EnterActions> and <xref:Microsoft.Maui.Controls.TriggerBase.ExitActions> collections can be specified. For more information, see [EnterActions and ExitActions](#enteractions-and-exitactions).

The equivalent C# code is:

```csharp
Entry entry = new Entry
{
    Text = "",
    Placeholder = "Enter text"
};
Button button = new Button { Text = "Save" };

DataTrigger dataTrigger = new DataTrigger(typeof(Button))
{
    Binding = new Binding("Text.Length", source: entry),
    Value = 0
};
dataTrigger.Setters.Add(new Setter
{
    Property = Button.IsEnabledProperty,
    Value = false
});
button.Triggers.Add(dataTrigger);
```

## Event triggers

An <xref:Microsoft.Maui.Controls.EventTrigger> represents a trigger that applies a set of actions in response to an event. Unlike <xref:Microsoft.Maui.Controls.Trigger>, <xref:Microsoft.Maui.Controls.EventTrigger> has no concept of termination of state, so the actions will not be undone once the condition that raised the event is no longer true.

An <xref:Microsoft.Maui.Controls.EventTrigger> only requires an `Event` property to be set:

```xaml
<EventTrigger Event="TextChanged">
    <local:NumericValidationTriggerAction />
</EventTrigger>
```

In this example, there are no <xref:Microsoft.Maui.Controls.Setter> elements. Instead, there's a `NumericalValidationTriggerAction` object.

> [!NOTE]
> Event triggers don't support <xref:Microsoft.Maui.Controls.TriggerBase.EnterActions> and <xref:Microsoft.Maui.Controls.TriggerBase.ExitActions>.

A trigger action implementation must:

- Implement the generic <xref:Microsoft.Maui.Controls.TriggerAction`1> class, with the generic parameter corresponding with the type of control the trigger will be applied to. You can use classes such as <xref:Microsoft.Maui.Controls.VisualElement> to write trigger actions that work with a variety of controls, or specify a control type like <xref:Microsoft.Maui.Controls.Entry>.
- Override the <xref:Microsoft.Maui.Controls.TriggerAction`1.Invoke%2A> method. This method is called whenever the trigger event occurs.
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

The equivalent C# code to attach an event trigger is:

```csharp
Entry entry = new Entry();
EventTrigger eventTrigger = new EventTrigger
{
    Event = "TextChanged"
};
eventTrigger.Actions.Add(new NumericValidationTriggerAction());
entry.Triggers.Add(eventTrigger);
```

> [!WARNING]
> Be careful when sharing triggers in a <xref:Microsoft.Maui.Controls.ResourceDictionary>. One instance will be shared among controls so any state that is configured once will apply to them all.

## Multi-triggers

A <xref:Microsoft.Maui.Controls.MultiTrigger> represents a trigger that applies property values, or performs actions, when a set of conditions are satisfied. All the conditions must be true before the <xref:Microsoft.Maui.Controls.Setter> objects are applied.

The following example shows a <xref:Microsoft.Maui.Controls.MultiTrigger> that binds to two <xref:Microsoft.Maui.Controls.Entry> objects:

```xaml
<Entry x:Name="email"
       Text="" />
<Entry x:Name="phone"
       Text="" />
<Button Text="Save">
    <Button.Triggers>
        <MultiTrigger TargetType="Button">
            <MultiTrigger.Conditions>
                <BindingCondition Binding="{Binding x:DataType='Entry',
                                                    Source={x:Reference email},
                                                    Path=Text.Length}"
                                  Value="0" />
                <BindingCondition Binding="{Binding x:DataType='Entry',
                                                    Source={x:Reference phone},
                                                    Path=Text.Length}"
                                  Value="0" />
            </MultiTrigger.Conditions>
            <Setter Property="IsEnabled" Value="False" />
            <!-- multiple Setter elements are allowed -->
        </MultiTrigger>
    </Button.Triggers>
</Button>
```

In addition, the `MultiTrigger.Conditions` collection can also contain <xref:Microsoft.Maui.Controls.PropertyCondition> objects:

```xaml
<PropertyCondition Property="Text"
                   Value="OK" />
```

The equivalent C# code is:

```csharp
Entry email = new Entry { Text = "" };
Entry phone = new Entry { Text = "" };
Button button = new Button { Text = "Save" };

MultiTrigger multiTrigger = new MultiTrigger(typeof(Button));
multiTrigger.Conditions.Add(new BindingCondition
{
    Binding = new Binding("Text.Length", source: email),
    Value = 0
});
multiTrigger.Conditions.Add(new BindingCondition
{
    Binding = new Binding("Text.Length", source: phone),
    Value = 0
});
multiTrigger.Setters.Add(new Setter
{
    Property = Button.IsEnabledProperty,
    Value = false
});
button.Triggers.Add(multiTrigger);
```

You can also use <xref:Microsoft.Maui.Controls.PropertyCondition> objects in C#:

```csharp
multiTrigger.Conditions.Add(new PropertyCondition
{
    Property = Entry.TextProperty,
    Value = "OK"
});
```

## EnterActions and ExitActions

An alternative approach to implementing changes when a trigger occurs is by specifying <xref:Microsoft.Maui.Controls.TriggerBase.EnterActions> and <xref:Microsoft.Maui.Controls.TriggerBase.ExitActions> collections, and creating <xref:Microsoft.Maui.Controls.TriggerAction`1> implementations.

The <xref:Microsoft.Maui.Controls.TriggerBase.EnterActions> collection, of type `IList<TriggerAction>`, defines a collection that will be invoked when the trigger condition is met. The <xref:Microsoft.Maui.Controls.TriggerBase.ExitActions> collection, of type `IList<TriggerAction>`, defines a collection that will be invoked after the trigger condition is no longer met.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.TriggerAction> objects defined in the <xref:Microsoft.Maui.Controls.TriggerBase.EnterActions> and <xref:Microsoft.Maui.Controls.TriggerBase.ExitActions> collections are ignored by the <xref:Microsoft.Maui.Controls.EventTrigger> class.

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

- Implement the generic <xref:Microsoft.Maui.Controls.TriggerAction`1> class, with the generic parameter corresponding with the type of control the trigger will be applied to. You can use classes such as <xref:Microsoft.Maui.Controls.VisualElement> to write trigger actions that work with a variety of controls, or specify a control type like <xref:Microsoft.Maui.Controls.Entry>.
- Override the <xref:Microsoft.Maui.Controls.TriggerAction`1.Invoke%2A> method. This method is called whenever the trigger event occurs.
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

The equivalent C# code to attach EnterActions and ExitActions is:

```csharp
Entry entry = new Entry { Placeholder = "Enter job title" };
Trigger trigger = new Trigger(typeof(Entry))
{
    Property = Entry.IsFocusedProperty,
    Value = true
};
trigger.EnterActions.Add(new FadeTriggerAction { StartsFrom = 0 });
trigger.ExitActions.Add(new FadeTriggerAction { StartsFrom = 1 });
entry.Triggers.Add(trigger);
```

> [!NOTE]
> You can provide <xref:Microsoft.Maui.Controls.TriggerBase.EnterActions> and <xref:Microsoft.Maui.Controls.TriggerBase.ExitActions> as well as <xref:Microsoft.Maui.Controls.Setter> objects in a trigger, but be aware that the <xref:Microsoft.Maui.Controls.Setter> objects are called immediately (they do not wait for the `EnterAction` or `ExitAction` to complete).

## State triggers

State triggers are a specialized group of triggers that define the conditions under which a <xref:Microsoft.Maui.Controls.VisualState> should be applied.

State triggers are added to the <xref:Microsoft.Maui.Controls.VisualState.StateTriggers> collection of a <xref:Microsoft.Maui.Controls.VisualState>. This collection can contain a single state trigger, or multiple state triggers. A <xref:Microsoft.Maui.Controls.VisualState> will be applied when any state triggers in the collection are active.

When using state triggers to control visual states, .NET MAUI uses the following precedence rules to determine which trigger (and corresponding <xref:Microsoft.Maui.Controls.VisualState>) will be active:

1. Any trigger that derives from <xref:Microsoft.Maui.Controls.StateTriggerBase>.
1. An <xref:Microsoft.Maui.Controls.AdaptiveTrigger> activated due to the <xref:Microsoft.Maui.Controls.AdaptiveTrigger.MinWindowWidth> condition being met.
1. An <xref:Microsoft.Maui.Controls.AdaptiveTrigger> activated due to the <xref:Microsoft.Maui.Controls.AdaptiveTrigger.MinWindowHeight> condition being met.

If multiple triggers are simultaneously active (for example, two custom triggers) then the first trigger declared in the markup takes precedence.

> [!NOTE]
> State triggers can be set in a <xref:Microsoft.Maui.Controls.Style>, or directly on elements.

For more information about visual states, see [Visual states](~/user-interface/visual-states.md).

### State trigger

The <xref:Microsoft.Maui.Controls.StateTrigger> class, which derives from the <xref:Microsoft.Maui.Controls.StateTriggerBase> class, has an <xref:Microsoft.Maui.Controls.StateTrigger.IsActive> bindable property. A <xref:Microsoft.Maui.Controls.StateTrigger> triggers a <xref:Microsoft.Maui.Controls.VisualState> change when the <xref:Microsoft.Maui.Controls.StateTrigger.IsActive> property changes value.

The <xref:Microsoft.Maui.Controls.StateTriggerBase> class, which is the base class for all state triggers, has an <xref:Microsoft.Maui.Controls.StateTrigger.IsActive> property and an <xref:Microsoft.Maui.Controls.StateTriggerBase.IsActiveChanged> event. This event fires whenever a <xref:Microsoft.Maui.Controls.VisualState> change occurs. In addition, the <xref:Microsoft.Maui.Controls.StateTriggerBase> class has overridable <xref:Microsoft.Maui.Controls.StateTriggerBase.OnAttached%2A> and <xref:Microsoft.Maui.Controls.StateTriggerBase.OnDetached%2A> methods.

> [!IMPORTANT]
> The `StateTrigger.IsActive` bindable property hides the inherited `StateTriggerBase.IsActive` property.

The following XAML example shows a <xref:Microsoft.Maui.Controls.Style> that includes <xref:Microsoft.Maui.Controls.StateTrigger> objects:

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

In this example, the implicit <xref:Microsoft.Maui.Controls.Style> targets <xref:Microsoft.Maui.Controls.Grid> objects. When the `IsToggled` property of the bound object is `true`, the background color of the <xref:Microsoft.Maui.Controls.Grid> is set to black. When the `IsToggled` property of the bound object becomes `false`, a <xref:Microsoft.Maui.Controls.VisualState> change is triggered, and the background color of the <xref:Microsoft.Maui.Controls.Grid> becomes white.

In addition, every time a <xref:Microsoft.Maui.Controls.VisualState> change occurs, the <xref:Microsoft.Maui.Controls.StateTriggerBase.IsActiveChanged> event for the <xref:Microsoft.Maui.Controls.VisualState> is raised. Each <xref:Microsoft.Maui.Controls.VisualState> registers an event handler for this event:

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

In this example, when a handler for the <xref:Microsoft.Maui.Controls.StateTriggerBase.IsActiveChanged> event is raised, the handler outputs whether the <xref:Microsoft.Maui.Controls.VisualState> is active or not. For example, the following messages are output to the console window when changing from the `Checked` visual state to the `Unchecked` visual state:

```
Checked state active: False
Unchecked state active: True
```

> [!NOTE]
> Custom state triggers can be created by deriving from the <xref:Microsoft.Maui.Controls.StateTriggerBase> class, and overriding the `OnAttached` and `OnDetached` methods to perform any required registrations and cleanup.

The equivalent C# code is:

```csharp
Grid grid = new Grid();

VisualStateGroup visualStateGroup = new VisualStateGroup();

// Checked state
VisualState checkedState = new VisualState { Name = "Checked" };
StateTrigger checkedTrigger = new StateTrigger();
checkedTrigger.SetBinding(StateTrigger.IsActiveProperty, new Binding("IsToggled"));
checkedTrigger.IsActiveChanged += OnCheckedStateIsActiveChanged;
checkedState.StateTriggers.Add(checkedTrigger);
checkedState.Setters.Add(new Setter
{
    Property = Grid.BackgroundColorProperty,
    Value = Colors.Black
});

// Unchecked state
VisualState uncheckedState = new VisualState { Name = "Unchecked" };
StateTrigger uncheckedTrigger = new StateTrigger();
uncheckedTrigger.SetBinding(StateTrigger.IsActiveProperty, new Binding("IsToggled", converter: new InverseBooleanConverter()));
uncheckedTrigger.IsActiveChanged += OnUncheckedStateIsActiveChanged;
uncheckedState.StateTriggers.Add(uncheckedTrigger);
uncheckedState.Setters.Add(new Setter
{
    Property = Grid.BackgroundColorProperty,
    Value = Colors.White
});

visualStateGroup.States.Add(checkedState);
visualStateGroup.States.Add(uncheckedState);
VisualStateManager.SetVisualStateGroups(grid, new VisualStateGroupList { visualStateGroup });

// Event handlers (as shown above)
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

In this example, you'll need to provide an `InverseBooleanConverter` implementation (a value converter that inverts boolean values) or use an alternative approach for the unchecked state.

### Adaptive trigger

An <xref:Microsoft.Maui.Controls.AdaptiveTrigger> triggers a <xref:Microsoft.Maui.Controls.VisualState> change when the window is a specified height or width. This trigger has two bindable properties:

- <xref:Microsoft.Maui.Controls.AdaptiveTrigger.MinWindowHeight>, of type `double`, which indicates the minimum window height at which the <xref:Microsoft.Maui.Controls.VisualState> should be applied.
- <xref:Microsoft.Maui.Controls.AdaptiveTrigger.MinWindowWidth>, of type `double`, which indicates the minimum window width at which the <xref:Microsoft.Maui.Controls.VisualState> should be applied.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.AdaptiveTrigger> derives from the <xref:Microsoft.Maui.Controls.StateTriggerBase> class and can therefore attach an event handler to the `IsActiveChanged` event.

The following XAML example shows a <xref:Microsoft.Maui.Controls.Style> that includes <xref:Microsoft.Maui.Controls.AdaptiveTrigger> objects:

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

In this example, the implicit <xref:Microsoft.Maui.Controls.Style> targets <xref:Microsoft.Maui.Controls.StackLayout> objects. When the window width is between 0 and 800 device-independent units, <xref:Microsoft.Maui.Controls.StackLayout> objects to which the <xref:Microsoft.Maui.Controls.Style> is applied will have a vertical orientation. When the window width is >= 800 device-independent units, the <xref:Microsoft.Maui.Controls.VisualState> change is triggered, and the <xref:Microsoft.Maui.Controls.StackLayout> orientation changes to horizontal.

The <xref:Microsoft.Maui.Controls.AdaptiveTrigger.MinWindowHeight> and <xref:Microsoft.Maui.Controls.AdaptiveTrigger.MinWindowWidth> properties can be used independently or in conjunction with each other. The following XAML shows an example of setting both properties:

```xaml
<AdaptiveTrigger MinWindowWidth="800"
                 MinWindowHeight="1200"/>
```

In this example, the <xref:Microsoft.Maui.Controls.AdaptiveTrigger> indicates that the corresponding <xref:Microsoft.Maui.Controls.VisualState> will be applied when the current window width is >= 800 device-independent units and the current window height is >= 1200 device-independent units.

> [!NOTE]
> For more information about device-independent units, see [Device-independent units](../user-interface/device-independent-units.md).

The equivalent C# code is:

```csharp
StackLayout stackLayout = new StackLayout();

VisualStateGroup visualStateGroup = new VisualStateGroup();

// Vertical state
VisualState verticalState = new VisualState { Name = "Vertical" };
AdaptiveTrigger verticalTrigger = new AdaptiveTrigger { MinWindowWidth = 0 };
verticalState.StateTriggers.Add(verticalTrigger);
verticalState.Setters.Add(new Setter
{
    Property = StackLayout.OrientationProperty,
    Value = StackOrientation.Vertical
});

// Horizontal state
VisualState horizontalState = new VisualState { Name = "Horizontal" };
AdaptiveTrigger horizontalTrigger = new AdaptiveTrigger { MinWindowWidth = 800 };
horizontalState.StateTriggers.Add(horizontalTrigger);
horizontalState.Setters.Add(new Setter
{
    Property = StackLayout.OrientationProperty,
    Value = StackOrientation.Horizontal
});

visualStateGroup.States.Add(verticalState);
visualStateGroup.States.Add(horizontalState);
VisualStateManager.SetVisualStateGroups(stackLayout, new VisualStateGroupList { visualStateGroup });
```

For combined width and height conditions:

```csharp
AdaptiveTrigger adaptiveTrigger = new AdaptiveTrigger
{
    MinWindowWidth = 800,
    MinWindowHeight = 1200
};
```

### Compare state trigger

The <xref:Microsoft.Maui.Controls.CompareStateTrigger> triggers a <xref:Microsoft.Maui.Controls.VisualState> change when a property is equal to a specific value. This trigger has two bindable properties:

- <xref:Microsoft.Maui.Controls.CompareStateTrigger.Property>, of type `object`, which indicates the property being compared by the trigger.
- <xref:Microsoft.Maui.Controls.CompareStateTrigger.Value>, of type `object`, which indicates the value at which the <xref:Microsoft.Maui.Controls.VisualState> should be applied.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.CompareStateTrigger> derives from the <xref:Microsoft.Maui.Controls.StateTriggerBase> class and can therefore attach an event handler to the `IsActiveChanged` event.

The following XAML example shows a <xref:Microsoft.Maui.Controls.Style> that includes <xref:Microsoft.Maui.Controls.CompareStateTrigger> objects:

```xaml
<Style TargetType="Grid">
    <Setter Property="VisualStateManager.VisualStateGroups">
        <VisualStateGroupList>
            <VisualStateGroup>
                <VisualState x:Name="Checked">
                    <VisualState.StateTriggers>
                        <CompareStateTrigger Property="{Binding x:DataType='CheckBox', Source={x:Reference checkBox}, Path=IsChecked}"
                                             Value="True" />
                    </VisualState.StateTriggers>
                    <VisualState.Setters>
                        <Setter Property="BackgroundColor"
                                Value="Black" />
                    </VisualState.Setters>
                </VisualState>
                <VisualState x:Name="Unchecked">
                    <VisualState.StateTriggers>
                        <CompareStateTrigger Property="{Binding x:DataType='CheckBox', Source={x:Reference checkBox}, Path=IsChecked}"
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
    <Border BackgroundColor="White"
            StrokeShape="RoundRectangle 12"
            Margin="24"
            Padding="24"
            HorizontalOptions="Center"
            VerticalOptions="Center">
        <StackLayout Orientation="Horizontal">
            <CheckBox x:Name="checkBox"
                      VerticalOptions="Center" />
            <Label Text="Check the CheckBox to modify the Grid background color."
                   VerticalOptions="Center" />
        </StackLayout>
    </Border>
</Grid>
```

In this example, the implicit <xref:Microsoft.Maui.Controls.Style> targets <xref:Microsoft.Maui.Controls.Grid> objects. When the `IsChecked` property of the <xref:Microsoft.Maui.Controls.CheckBox> is `false`, the background color of the <xref:Microsoft.Maui.Controls.Grid> is set to white. When the `CheckBox.IsChecked` property becomes `true`, a <xref:Microsoft.Maui.Controls.VisualState> change is triggered, and the background color of the <xref:Microsoft.Maui.Controls.Grid> becomes black.

The equivalent C# code is:

```csharp
CheckBox checkBox = new CheckBox { VerticalOptions = LayoutOptions.Center };
Label label = new Label
{
    Text = "Check the CheckBox to modify the Grid background color.",
    VerticalOptions = LayoutOptions.Center
};

StackLayout stackLayout = new StackLayout
{
    Orientation = StackOrientation.Horizontal,
    Children = { checkBox, label }
};

Border border = new Border
{
    BackgroundColor = Colors.White,
    StrokeShape = new RoundRectangle { CornerRadius = 12 },
    Margin = new Thickness(24),
    Padding = new Thickness(24),
    HorizontalOptions = LayoutOptions.Center,
    VerticalOptions = LayoutOptions.Center,
    Content = stackLayout
};

Grid grid = new Grid { Children = { border } };

VisualStateGroup visualStateGroup = new VisualStateGroup();

// Checked state
VisualState checkedState = new VisualState { Name = "Checked" };
CompareStateTrigger checkedTrigger = new CompareStateTrigger
{
    Value = true
};
checkedTrigger.SetBinding(CompareStateTrigger.PropertyProperty, new Binding("IsChecked", source: checkBox));
checkedState.StateTriggers.Add(checkedTrigger);
checkedState.Setters.Add(new Setter
{
    Property = Grid.BackgroundColorProperty,
    Value = Colors.Black
});

// Unchecked state
VisualState uncheckedState = new VisualState { Name = "Unchecked" };
CompareStateTrigger uncheckedTrigger = new CompareStateTrigger
{
    Value = false
};
uncheckedTrigger.SetBinding(CompareStateTrigger.PropertyProperty, new Binding("IsChecked", source: checkBox));
uncheckedState.StateTriggers.Add(uncheckedTrigger);
uncheckedState.Setters.Add(new Setter
{
    Property = Grid.BackgroundColorProperty,
    Value = Colors.White
});

visualStateGroup.States.Add(checkedState);
visualStateGroup.States.Add(uncheckedState);
VisualStateManager.SetVisualStateGroups(grid, new VisualStateGroupList { visualStateGroup });
```

### Device state trigger

The <xref:Microsoft.Maui.Controls.DeviceStateTrigger> triggers a <xref:Microsoft.Maui.Controls.VisualState> change based on the device platform the app is running on. This trigger has a single bindable property:

- <xref:Microsoft.Maui.Controls.DeviceStateTrigger.Device>, of type `string`, which indicates the device platform on which the <xref:Microsoft.Maui.Controls.VisualState> should be applied.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.DeviceStateTrigger> derives from the <xref:Microsoft.Maui.Controls.StateTriggerBase> class and can therefore attach an event handler to the `IsActiveChanged` event.

The following XAML example shows a <xref:Microsoft.Maui.Controls.Style> that includes <xref:Microsoft.Maui.Controls.DeviceStateTrigger> objects:

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

In this example, the explicit <xref:Microsoft.Maui.Controls.Style> targets <xref:Microsoft.Maui.Controls.ContentPage> objects. <xref:Microsoft.Maui.Controls.ContentPage> objects that consume the style set their background color to silver on iOS, and to pale blue on Android.

The equivalent C# code is:

```csharp
ContentPage page = new ContentPage();

VisualStateGroup visualStateGroup = new VisualStateGroup();

// iOS state
VisualState iOSState = new VisualState { Name = "iOS" };
DeviceStateTrigger iOSTrigger = new DeviceStateTrigger { Device = "iOS" };
iOSState.StateTriggers.Add(iOSTrigger);
iOSState.Setters.Add(new Setter
{
    Property = ContentPage.BackgroundColorProperty,
    Value = Colors.Silver
});

// Android state
VisualState androidState = new VisualState { Name = "Android" };
DeviceStateTrigger androidTrigger = new DeviceStateTrigger { Device = "Android" };
androidState.StateTriggers.Add(androidTrigger);
androidState.Setters.Add(new Setter
{
    Property = ContentPage.BackgroundColorProperty,
    Value = Color.FromArgb("#2196F3")
});

visualStateGroup.States.Add(iOSState);
visualStateGroup.States.Add(androidState);
VisualStateManager.SetVisualStateGroups(page, new VisualStateGroupList { visualStateGroup });
```

### Orientation state trigger

The <xref:Microsoft.Maui.Controls.OrientationStateTrigger> triggers a <xref:Microsoft.Maui.Controls.VisualState> change when the orientation of the device changes. This trigger has a single bindable property:

- <xref:Microsoft.Maui.Controls.OrientationStateTrigger.Orientation>, of type <xref:Microsoft.Maui.Devices.DisplayOrientation>, which indicates the orientation to which the <xref:Microsoft.Maui.Controls.VisualState> should be applied.

> [!NOTE]
> The `OrientationStateTrigger` derives from the <xref:Microsoft.Maui.Controls.StateTriggerBase> class and can therefore attach an event handler to the `IsActiveChanged` event.

The following XAML example shows a <xref:Microsoft.Maui.Controls.Style> that includes <xref:Microsoft.Maui.Controls.OrientationStateTrigger> objects:

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

In this example, the explicit <xref:Microsoft.Maui.Controls.Style> targets <xref:Microsoft.Maui.Controls.ContentPage> objects. <xref:Microsoft.Maui.Controls.ContentPage> objects that consume the style set their background color to silver when the orientation is portrait, and set their background color to white when the orientation is landscape.

The equivalent C# code is:

```csharp
ContentPage page = new ContentPage();

VisualStateGroup visualStateGroup = new VisualStateGroup();

// Portrait state
VisualState portraitState = new VisualState { Name = "Portrait" };
OrientationStateTrigger portraitTrigger = new OrientationStateTrigger
{
    Orientation = DisplayOrientation.Portrait
};
portraitState.StateTriggers.Add(portraitTrigger);
portraitState.Setters.Add(new Setter
{
    Property = ContentPage.BackgroundColorProperty,
    Value = Colors.Silver
});

// Landscape state
VisualState landscapeState = new VisualState { Name = "Landscape" };
OrientationStateTrigger landscapeTrigger = new OrientationStateTrigger
{
    Orientation = DisplayOrientation.Landscape
};
landscapeState.StateTriggers.Add(landscapeTrigger);
landscapeState.Setters.Add(new Setter
{
    Property = ContentPage.BackgroundColorProperty,
    Value = Colors.White
});

visualStateGroup.States.Add(portraitState);
visualStateGroup.States.Add(landscapeState);
VisualStateManager.SetVisualStateGroups(page, new VisualStateGroupList { visualStateGroup });
```
