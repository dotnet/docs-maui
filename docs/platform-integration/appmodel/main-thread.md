---
title: Run code on the main UI thread
description: "In .NET MAUI, event handlers may be called on a secondary thread. The MainThread class allows an application to run code on the main UI thread. This article describes how to use the MainThread class."
ms.date: 02/02/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel"]
---

# Create a thread on the .NET MAUI UI thread

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.ApplicationModel.MainThread> class to run code on the main UI thread. Most operating systems use a single-threading model for code involving the user interface. This model is necessary to properly serialize user-interface events, including keystrokes and touch input. This thread is often called the _main thread_, the _user-interface thread_, or the _UI thread_. The disadvantage of this model is that all code that accesses user interface elements must run on the application's main thread.

The `MainThread` class is available in the `Microsoft.Maui.ApplicationModel` namespace.

## When is it required

Applications sometimes need to use events that call the event handler on a secondary thread, such as the [`Accelerometer`](../device/sensors.md#accelerometer) or [`Compass`](../device/sensors.md#compass) sensors. All sensors might return information on a secondary thread when used with faster sensing speeds. If the event handler needs to access user-interface elements, it must invoke code on the main thread.

## Run code on the UI thread

To run code on the main thread, call the static <xref:Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread%2A?displayProperty=nameWithType> method. The argument is an <xref:System.Action> object, which is simply a method with no arguments and no return value:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="runcode_lambda":::

It is also possible to define a separate method for the code, and then call that code with the `BeginInvokeOnMainThread` method:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="runcode_func_pointer":::

## Determine if invocation is required

With the <xref:Microsoft.Maui.ApplicationModel.MainThread> class, you can determine if the current code is running on the main thread. The <xref:Microsoft.Maui.ApplicationModel.MainThread.IsMainThread%2A?displayProperty=nameWithType> property returns `true` if the code calling the property is running on the main thread, and `false` if it isn't. It's logical to assume that you need to determine if the code is running on the main thread before calling <xref:Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread%2A?displayProperty=nameWithType>. For example, the following code uses the `IsMainThread` to detect if the `MyMainThreadCode` method should be called directly if the code is running on the main thread. If it isn't running on the main thread, the method is passed to `BeginInvokeOnMainThread`:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="runcode_test_thread":::

For this scenario, this check isn't necessary. `BeginInvokeOnMainThread` itself tests if the current code is running on the main thread or not. If the code is running on the main thread, `BeginInvokeOnMainThread` just calls the provided method directly. If the code is running on a secondary thread, `BeginInvokeOnMainThread` invokes the provided method on the main thread. Therefore, if the code you run is the same, regardless of the main or secondary thread, simply call `BeginInvokeOnMainThread` without checking if it's required. There is negligible overhead in doing so.

The only reason you would need to check the `IsMainThread` property is if you have branching logic that does something different based on the thread.

## Additional methods

The <xref:Microsoft.Maui.ApplicationModel.MainThread> class includes the following additional `static` methods that can be used to interact with user interface elements from backgrounds threads:

| Method                                                                                                                                                            | Arguments       | Returns   | Purpose                                                                     |
|-------------------------------------------------------------------------------------------------------------------------------------------------------------------|-----------------|-----------|-----------------------------------------------------------------------------|
| [`InvokeOnMainThreadAsync<T>`](xref:Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync%60%601(System.Func{%60%600}))                              | `Func<T>`       | `Task<T>` | Invokes a `Func<T>` on the main thread, and waits for it to complete.       |
| [`InvokeOnMainThreadAsync`](xref:Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync(System.Action))                                               | `Action`        | `Task`    | Invokes an `Action` on the main thread, and waits for it to complete.       |
| [`InvokeOnMainThreadAsync<T>`](xref:Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync%60%601(System.Func{System.Threading.Tasks.Task{%60%600}})) | `Func<Task<T>>` | `Task<T>` | Invokes a `Func<Task<T>>` on the main thread, and waits for it to complete. |
| [`InvokeOnMainThreadAsync`](xref:Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync(System.Func{System.Threading.Tasks.Task}))                    | `Func<Task>`    | `Task`    | Invokes a `Func<Task>` on the main thread, and waits for it to complete.    |
| [`GetMainThreadSynchronizationContextAsync`](xref:Microsoft.Maui.Controls.Device.GetMainThreadSynchronizationContextAsync)                                        |                 | `Task<SynchronizationContext>` | Returns the `SynchronizationContext` for the main thread. |
