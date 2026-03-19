---
title: "Performance best practices for .NET MAUI"
description: "A practical guide to building high-performance .NET MAUI apps — covering compiled bindings, layout optimization, control selection, image handling, async patterns, startup time, and deployment options."
ms.date: 02/10/2026
ms.topic: conceptual
---

# Performance best practices for .NET MAUI

This guide covers the most impactful performance optimizations for .NET MAUI apps. Recommendations are ordered roughly by impact — start at the top.

## 1. Compiled bindings

This is the single biggest performance win in most MAUI apps.

### The problem

Reflection-based bindings resolve property paths at runtime using `System.Reflection`. This adds overhead on every property change notification, every binding evaluation, and every `DataTemplate` instantiation. In a list with hundreds of items, this cost compounds fast.

### The fix

Set `x:DataType` on your page and on every `DataTemplate`. This tells the XAML compiler to generate direct property access code at build time instead of using reflection, resulting in 8–20x faster binding resolution.

```xaml
<!-- DO: Compiled binding — x:DataType enables build-time resolution -->
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:MyApp.ViewModels"
             x:DataType="vm:MainViewModel">
    <Label Text="{Binding UserName}" />
</ContentPage>
```

```xaml
<!-- DON'T: No x:DataType — falls back to slow reflection binding -->
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">
    <Label Text="{Binding UserName}" />
</ContentPage>
```

Set `x:DataType` on `DataTemplate` elements too — they don't inherit it from the page:

```xaml
<CollectionView ItemsSource="{Binding People}">
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="vm:PersonViewModel">
            <Label Text="{Binding FullName}" />
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

In C#, use the expression-based `SetBinding` overload for compiled, type-safe bindings:

```csharp
// DO: Compiled, type-safe — no reflection
label.SetBinding(Label.TextProperty, static (MainViewModel vm) => vm.UserName);

// DON'T: String-based — uses reflection at runtime
label.SetBinding(Label.TextProperty, "UserName");
```

### Don't bind static values

If a value never changes and doesn't come from a view model, set it directly. Every binding — even a compiled one — has overhead compared to a direct property assignment.

```xaml
<!-- DO: Static text, set directly -->
<Label Text="Welcome to the app" />

<!-- DON'T: Binding to a constant view model property -->
<Label Text="{Binding WelcomeMessage}" />
```

## 2. Control selection

Choosing the right control eliminates entire categories of performance problems.

### Use Grid instead of nested StackLayouts

Nested `StackLayout` controls cause exponential layout passes. A single `Grid` with defined rows and columns achieves the same visual result with one layout pass.

```xaml
<!-- DON'T: Nested StackLayouts — multiple layout passes -->
<StackLayout>
    <StackLayout Orientation="Horizontal">
        <Label Text="{Binding Name}" />
        <Label Text="{Binding Age}" />
    </StackLayout>
    <Label Text="{Binding Description}" />
</StackLayout>
```

```xaml
<!-- DO: Flat Grid — single layout pass -->
<Grid RowDefinitions="Auto,Auto" ColumnDefinitions="*,Auto">
    <Label Text="{Binding Name}" />
    <Label Text="{Binding Age}" Grid.Column="1" />
    <Label Text="{Binding Description}" Grid.Row="1" Grid.ColumnSpan="2" />
</Grid>
```

### Use specific stack layouts

`VerticalStackLayout` and `HorizontalStackLayout` are lighter than the general-purpose `StackLayout` because they skip the orientation check and have a simpler layout algorithm.

```xaml
<!-- DO: Specific, lightweight -->
<VerticalStackLayout>
    <Label Text="First" />
    <Label Text="Second" />
</VerticalStackLayout>

<HorizontalStackLayout>
    <Label Text="Left" />
    <Label Text="Right" />
</HorizontalStackLayout>
```

```xaml
<!-- DON'T: General-purpose StackLayout -->
<StackLayout Orientation="Vertical">
    <Label Text="First" />
    <Label Text="Second" />
</StackLayout>

<StackLayout Orientation="Horizontal">
    <Label Text="Left" />
    <Label Text="Right" />
</StackLayout>
```

### Border vs Frame

`Frame` is obsolete. Use `Border`, which is lighter and more flexible.

```xaml
<!-- DO: Border — modern, lightweight -->
<Border Stroke="Gray" StrokeThickness="1"
        StrokeShape="RoundRectangle 8">
    <Label Text="Content" Padding="10" />
</Border>
```

```xaml
<!-- DON'T: Frame — obsolete -->
<Frame BorderColor="Gray" CornerRadius="8">
    <Label Text="Content" Padding="10" />
</Frame>
```

### CollectionView vs ListView vs BindableLayout

| Control | Use when | Virtualizes | Notes |
|---------|----------|:-----------:|-------|
| `CollectionView` | More than 20 items | Yes | Recommended for all list scenarios |
| `BindableLayout` | 20 items or fewer | No | Simpler, attach to any layout |
| `ListView` | Never | Yes | Obsolete — migrate to `CollectionView` |

> **Warning:** Never place a `CollectionView` or `ScrollView` inside a `StackLayout`. The `StackLayout` gives infinite space, which disables virtualization and creates the items all at once. Wrap them in a `Grid` instead.

```xaml
<!-- DO: CollectionView in a Grid -->
<Grid>
    <CollectionView ItemsSource="{Binding Items}">
        <CollectionView.ItemTemplate>
            <DataTemplate x:DataType="vm:ItemViewModel">
                <Label Text="{Binding Title}" />
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</Grid>
```

```xaml
<!-- DO: BindableLayout for small collections -->
<VerticalStackLayout BindableLayout.ItemsSource="{Binding Tags}">
    <BindableLayout.ItemTemplate>
        <DataTemplate x:DataType="vm:TagViewModel">
            <Label Text="{Binding Name}" />
        </DataTemplate>
    </BindableLayout.ItemTemplate>
</VerticalStackLayout>
```

### Background property vs BackgroundColor

Prefer the `Background` property over the obsolete `BackgroundColor`:

```csharp
// DO
view.Background = Colors.Red;

// DON'T (obsolete)
view.BackgroundColor = Colors.Red;
```

## 3. Layout optimization

### Flatten the visual tree

Every nested layout adds measurement and arrangement passes. Aim for the flattest possible hierarchy.

```xaml
<!-- DON'T: 4 levels of nesting -->
<StackLayout>
    <StackLayout Orientation="Horizontal">
        <StackLayout>
            <Label Text="{Binding Title}" />
            <Label Text="{Binding Subtitle}" />
        </StackLayout>
        <StackLayout>
            <Image Source="{Binding Icon}" />
        </StackLayout>
    </StackLayout>
    <Label Text="{Binding Description}" />
</StackLayout>
```

```xaml
<!-- DO: 1 level — single Grid -->
<Grid RowDefinitions="Auto,Auto,Auto" ColumnDefinitions="*,Auto">
    <Label Text="{Binding Title}" />
    <Label Text="{Binding Subtitle}" Grid.Row="1" />
    <Image Source="{Binding Icon}" Grid.RowSpan="2" Grid.Column="1"
           VerticalOptions="Center" />
    <Label Text="{Binding Description}" Grid.Row="2" Grid.ColumnSpan="2" />
</Grid>
```

### Remove single-child layouts

A layout containing a single child is pure overhead.

```xaml
<!-- DON'T: Unnecessary wrapper -->
<StackLayout>
    <Label Text="Hello" />
</StackLayout>

<!-- DO: Remove the wrapper -->
<Label Text="Hello" />
```

### Hide vs Remove

Use `IsVisible="false"` to exclude a view from layout. The view remains in the visual tree but skips measurement and rendering. This is cheaper than adding and removing views for items that toggle frequently.

For items shown rarely (for example, an error banner), consider removing them from the tree entirely and adding them only when needed.

```xaml
<!-- Toggle visibility instead of add/remove -->
<Label Text="Loading..." IsVisible="{Binding IsLoading}" />
```

### Reduce the app resource dictionary

App-level resources are checked for every control creation across the entire app. Move page-specific styles to page-level resource dictionaries to reduce lookup scope.

```xaml
<!-- DO: Page-specific styles in the page -->
<ContentPage.Resources>
    <Style x:Key="DetailLabel" TargetType="Label">
        <Setter Property="FontSize" Value="12" />
        <Setter Property="TextColor" Value="Gray" />
    </Style>
</ContentPage.Resources>
```

## 4. Image optimization

Images are a common source of memory pressure and slow rendering. Follow these rules:

- **Reference SVGs as PNG** — .NET MAUI converts SVGs to PNGs at build time. Always reference `.png` in your XAML even if the source file is `.svg`.
- **Size images appropriately** — don't load a 4000×3000 image for a 60×60 thumbnail. Resize source images or constrain dimensions.
- **Set explicit dimensions** — use `HeightRequest` and `WidthRequest` with an `Aspect` mode so the layout engine doesn't need to measure the decoded image.
- **Keep list images small and consistent** — in `CollectionView` item templates, use fixed-size images to avoid layout recalculations during scrolling.
- **Cache remote images** — for images loaded from URLs, use an image caching library to avoid re-downloading.

```xaml
<!-- DO: Constrained size, explicit aspect -->
<Image Source="profile.png"
       HeightRequest="60" WidthRequest="60"
       Aspect="AspectFill" />
```

```xaml
<!-- DON'T: Unconstrained image in a list item — causes layout thrashing -->
<Image Source="{Binding LargeImageUrl}" />
```

## 5. Startup optimization

### Use Shell

`Shell` loads pages on demand — only the visible page is created at startup. `TabbedPage` creates all tab pages eagerly, increasing startup time proportionally to the number of tabs.

```csharp
// DO: Shell with route-based navigation
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("details", typeof(DetailsPage));
    }
}
```

### Delay non-critical work

Don't perform heavy initialization in the `App` constructor or `MainPage` constructor. Use the `OnAppearing` override or the `Loaded` event to defer work until the UI is visible.

```csharp
public MainPage()
{
    InitializeComponent();
    // DON'T: Load data in the constructor — blocks startup
}

protected override async void OnAppearing()
{
    base.OnAppearing();
    // DO: Load data when the page is about to appear
    await viewModel.LoadDataAsync();
}
```

### Minimize DI registrations at startup

Only register services you actually need. Use `AddTransient` for lightweight, stateless services and `AddSingleton` for expensive-to-create or shared-state services.

```csharp
// DO: Register only what you need, with appropriate lifetimes
builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddTransient<DetailsViewModel>();
```

### Consider Native AOT (iOS/Mac Catalyst)

Native AOT compilation provides approximately 2x faster startup on iOS and approximately 1.2x faster startup on Mac Catalyst. It requires fully trim-compatible code — no dynamic reflection. See [Native AOT deployment](nativeaot.md) for setup instructions.

## 6. Data binding performance

### Binding modes matter

Choose the cheapest binding mode that satisfies your requirements:

| Mode | Cost | Use when |
|------|------|----------|
| `OneTime` | Cheapest | Data is set once and never changes |
| `OneWay` | Low | Data updates from source to UI only (default) |
| `TwoWay` | Highest | User can edit the value (`Entry`, `Switch`, `Slider`) |

```xaml
<!-- DO: OneTime for static data -->
<Label Text="{Binding CreatedDate, Mode=OneTime}" />

<!-- DO: TwoWay only for editable controls -->
<Entry Text="{Binding UserName, Mode=TwoWay}" />
```

```xaml
<!-- DON'T: Binding for static content -->
<Label Text="{Binding NameLabel}" /> <!-- where NameLabel always returns "Name:" -->

<!-- DO: Just set the text directly -->
<Label Text="Name:" />
```

### Avoid complex binding paths

Deep binding paths like `{Binding Parent.Child.GrandChild.Value}` require multiple property lookups and event subscriptions at every level. Flatten your view model instead.

```csharp
// DON'T: Deep property path
// {Binding Order.Customer.Address.City}

// DO: Flatten in the view model
public string CustomerCity => Order.Customer.Address.City;
```

## 7. Async and threading

### Use async/await correctly

Always use `async`/`await` end to end. Never block on async code with `.Result` or `.Wait()` — this risks deadlocks and blocks the UI thread.

```csharp
// DO: Async all the way
public async Task LoadDataAsync()
{
    var data = await _service.GetDataAsync();
    Items = new ObservableCollection<Item>(data);
}
```

```csharp
// DON'T: Blocking on async — deadlock risk
public void LoadData()
{
    var data = _service.GetDataAsync().Result; // Can deadlock!
    Items = new ObservableCollection<Item>(data);
}
```

### Use Task.WhenAll for independent operations

When you have multiple independent async operations, run them concurrently instead of sequentially.

```csharp
// DO: Run independent operations in parallel
await Task.WhenAll(
    LoadUserAsync(),
    LoadSettingsAsync(),
    LoadNotificationsAsync()
);

// DON'T: Sequential when operations are independent
await LoadUserAsync();
await LoadSettingsAsync();
await LoadNotificationsAsync();
```

### Thread safety

UI controls can only be modified on the main thread. Use `IDispatcher` (via dependency injection) or `BindableObject.Dispatcher` to marshal calls back to the UI thread. `MainThread.BeginInvokeOnMainThread()` works as a fallback.

```csharp
// DO: Use the injected dispatcher for UI updates from background threads
await dispatcher.DispatchAsync(() =>
{
    Items.Add(newItem);
});
```

> **Warning:** Never update an `ObservableCollection` bound to a UI control from a background thread. This causes cross-thread access exceptions or silent data corruption.

### Avoid async void

Only use `async void` for event handlers. Every other async method should return `Task` or `Task<T>` so callers can await it and exceptions propagate correctly.

```csharp
// DO: Return Task
public async Task SaveAsync() { ... }

// OK: async void for event handlers only
private async void OnSaveClicked(object sender, EventArgs e)
{
    await SaveAsync();
}

// DON'T: async void for non-event-handler methods
private async void Save() { ... } // Exceptions are swallowed
```

## 8. Memory management

### Unsubscribe from events

Event subscriptions keep objects alive. Always unsubscribe when a page disappears or a view model is no longer needed.

```csharp
protected override void OnAppearing()
{
    base.OnAppearing();
    _service.DataChanged += OnDataChanged;
}

protected override void OnDisappearing()
{
    base.OnDisappearing();
    _service.DataChanged -= OnDataChanged;
}
```

### Use WeakEventManager for custom events

If your class exposes events that external objects subscribe to, use `WeakEventManager` to avoid preventing garbage collection of subscribers.

```csharp
readonly WeakEventManager _eventManager = new();

public event EventHandler DataChanged
{
    add => _eventManager.AddEventHandler(value);
    remove => _eventManager.RemoveEventHandler(value);
}

protected void OnDataChanged()
{
    _eventManager.HandleEvent(this, EventArgs.Empty, nameof(DataChanged));
}
```

### Dispose IDisposable resources

Use `using` statements for short-lived resources. For view model–scoped resources, implement `IDisposable` and clean up when the page disappears or when the DI container disposes the service.

```csharp
// DO: using statement for short-lived resources
using var stream = await FileSystem.OpenAppPackageFileAsync("data.json");
using var reader = new StreamReader(stream);
var content = await reader.ReadToEndAsync();
```

### Avoid strong circular references (iOS/Mac Catalyst)

On Apple platforms, circular references between managed (.NET) objects and native (Objective-C/Swift) objects prevent garbage collection because both runtimes hold strong references. Use weak references to break cycles between views and their delegates or data sources.

## 9. Trimming and AOT

Trimming and ahead-of-time compilation reduce app size and improve startup time.

| Option | Effect | Trade-off |
|--------|--------|-----------|
| Full trimming | Removes unused code, reducing app size | Code must be trim-compatible — no unchecked reflection |
| Native AOT (iOS/Mac) | ~2x faster startup, ~2.5x smaller apps | Requires fully trim-compatible code |

Enable full trimming in your project file:

```xml
<PropertyGroup>
    <TrimMode>full</TrimMode>
</PropertyGroup>
```

For Native AOT on iOS or Mac Catalyst:

```xml
<PropertyGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">
    <PublishAot>true</PublishAot>
</PropertyGroup>
```

Both options require that your code and all dependencies are trim-compatible. Avoid patterns that rely on dynamic reflection, such as `Type.GetType()` with runtime strings or unanalyzable `Activator.CreateInstance` calls.

For details, see [Trimming a .NET MAUI app](trimming.md) and [Native AOT deployment](nativeaot.md).

## 10. Profiling

You can't optimize what you can't measure. Use these approaches to find bottlenecks:

- **Platform profilers** — Use Android Studio Profiler for Android, Xcode Instruments for iOS/Mac Catalyst, and Visual Studio Diagnostic Tools for Windows. These show CPU, memory, and rendering performance at the platform level.
- **Profile on real devices** — Emulators and simulators don't reflect real-world performance. Always validate on physical hardware.
- **Test on your lowest-spec target device** — Performance that's acceptable on a flagship phone might be unusable on a budget device.
- **Use `Stopwatch` for quick timing** — For development-time checks, `Stopwatch` gives you fast, focused measurements without a full profiler.

```csharp
var sw = System.Diagnostics.Stopwatch.StartNew();
await LoadDataAsync();
sw.Stop();
Debug.WriteLine($"LoadDataAsync took {sw.ElapsedMilliseconds}ms");
```

## See also

- [Improve app performance](performance.md)
- [Trimming a .NET MAUI app](trimming.md)
- [Native AOT deployment](nativeaot.md)
- [Compiled bindings](../fundamentals/data-binding/compiled-bindings.md)
