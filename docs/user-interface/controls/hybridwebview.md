---
title: HybridWebView
description: Learn how to use a HybridWebView to host HTML/JS/CSS content in a WebView, and communicate between that content and .NET.
ms.topic: concept-article
ms.date: 08/20/2025
monikerRange: ">=net-maui-9.0"

#customer intent: As a developer, I want to host HTML/JS/CSS content in a web view so that I can publish the web app as a mobile app.
---

# HybridWebView

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-hybridwebview)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.HybridWebView> enables hosting arbitrary HTML/JS/CSS content in a web view, and enables communication between the code in the web view (JavaScript) and the code that hosts the web view (C#/.NET). For example, if you have an existing React JS app, you could host it in a cross-platform .NET MAUI native app, and build the back-end of the app using C# and .NET.

<xref:Microsoft.Maui.Controls.HybridWebView> defines the following properties:

- <xref:Microsoft.Maui.Controls.HybridWebView.DefaultFile>, of type `string?`, which specifies the file within the <xref:Microsoft.Maui.Controls.HybridWebView.HybridRoot> that should be served as the default file. The default value is *index.html*.
- <xref:Microsoft.Maui.Controls.HybridWebView.HybridRoot>, of type `string?`, which is the path within the app's raw asset resources that contain the web app's contents. The default value is *wwwroot*, which maps to *Resources/Raw/wwwroot*.

In addition, <xref:Microsoft.Maui.Controls.HybridWebView> defines a <xref:Microsoft.Maui.Controls.HybridWebView.RawMessageReceived> event that's raised when a raw message is received. The <xref:Microsoft.Maui.Controls.HybridWebViewRawMessageReceivedEventArgs> object that accompanies the event defines a <xref:Microsoft.Maui.Controls.HybridWebViewRawMessageReceivedEventArgs.Message> property that contains the message.

Your app's C# code can invoke synchronous and asynchronous JavaScript methods within the <xref:Microsoft.Maui.Controls.HybridWebView> with the <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A> and <xref:Microsoft.Maui.Controls.HybridWebView.EvaluateJavaScriptAsync%2A> methods. Your app's JavaScript code can also invoke synchronous and asynchronous C# methods. For more information, see [Invoke JavaScript from C#](#invoke-javascript-from-c) and [Invoke C# from JavaScript](#invoke-c-from-javascript).

To create a .NET MAUI app with <xref:Microsoft.Maui.Controls.HybridWebView> you need:

- The web content of the app, which consists of static HTML, JavaScript, CSS, images, and other files.
- A <xref:Microsoft.Maui.Controls.HybridWebView> control as part of the app's UI. This can be achieved by referencing it in the app's XAML.
- Code in the web content, and in C#/.NET, that uses the <xref:Microsoft.Maui.Controls.HybridWebView> APIs to send messages between the two components.

The entire app, including the web content, is packaged and runs locally on a device, and can be published to applicable app stores. The web content is hosted within a native web view control and runs within the context of the app. Any part of the app can access external web services, but isn't required to.

> [!IMPORTANT]
> By default, the <xref:Microsoft.Maui.Controls.HybridWebView> control won't be available when full trimming or Native AOT is enabled. To change this behavior, see [Trimming feature switches](~/deployment/trimming.md#trimming-feature-switches).

## Create a .NET MAUI HybridWebView app

To create a .NET MAUI app with a <xref:Microsoft.Maui.Controls.HybridWebView>:

1. Open an existing .NET MAUI app project or create a new .NET MAUI app project.
1. Add your web content to the .NET MAUI app project.

    Your app's web content should be included as part of a .NET MAUI project as raw assets. A raw asset is any file in the app's *Resources\Raw* folder, and includes sub-folders. For a default <xref:Microsoft.Maui.Controls.HybridWebView>, web content should be placed in the *Resources\Raw\wwwroot* folder, with the main file named *index.html*.

    A simple app might have the following files and contents:

    - *Resources\Raw\wwwroot\index.html* with content for the main UI:
        ::: moniker range="<=net-maui-9.0"

        ```html
        <!DOCTYPE html>

        <html lang="en" xmlns="http://www.w3.org/1999/xhtml">
        <head>
            <meta charset="utf-8" />
            <title></title>
            <link rel="icon" href="data:,">
            <link rel="stylesheet" href="styles/app.css">
            <script src="scripts/HybridWebView.js"></script>
            <script>
                function LogMessage(msg) {
                    var messageLog = document.getElementById("messageLog");
                    messageLog.value += '\r\n' + msg;
                }

                window.addEventListener(
                    "HybridWebViewMessageReceived",
                    function (e) {
                        LogMessage("Raw message: " + e.detail.message);
                    });

                function AddNumbers(a, b) {
                    var result = {
                        "result": a + b,
                        "operationName": "Addition"
                    };
                    return result;
                }

                var count = 0;

                async function EvaluateMeWithParamsAndAsyncReturn(s1, s2) {
                    const response = await fetch("/asyncdata.txt");
                    if (!response.ok) {
                        throw new Error(`HTTP error: ${response.status}`);
                    }
                    var jsonData = await response.json();

                    jsonData[s1] = s2;

                    const msg = 'JSON data is available: ' + JSON.stringify(jsonData);
                    window.HybridWebView.SendRawMessage(msg)

                    return jsonData;
                }

                async function InvokeDoSyncWork() {
                    LogMessage("Invoking DoSyncWork");
                    await window.HybridWebView.InvokeDotNet('DoSyncWork');
                    LogMessage("Invoked DoSyncWork");
                }

                async function InvokeDoSyncWorkParams() {
                    LogMessage("Invoking DoSyncWorkParams");
                    await window.HybridWebView.InvokeDotNet('DoSyncWorkParams', [123, 'hello']);
                    LogMessage("Invoked DoSyncWorkParams");
                }

                async function InvokeDoSyncWorkReturn() {
                    LogMessage("Invoking DoSyncWorkReturn");
                    const retValue = await window.HybridWebView.InvokeDotNet('DoSyncWorkReturn');
                    LogMessage("Invoked DoSyncWorkReturn, return value: " + retValue);
                }

                async function InvokeDoSyncWorkParamsReturn() {
                    LogMessage("Invoking DoSyncWorkParamsReturn");
                    const retValue = await window.HybridWebView.InvokeDotNet('DoSyncWorkParamsReturn', [123, 'hello']);
                    LogMessage("Invoked DoSyncWorkParamsReturn, return value: message=" + retValue.Message + ", value=" + retValue.Value);
                }

                async function InvokeDoAsyncWork() {
                    LogMessage("Invoking DoAsyncWork");
                    await window.HybridWebView.InvokeDotNet('DoAsyncWork');
                    LogMessage("Invoked DoAsyncWork");
                }

                async function InvokeDoAsyncWorkParams() {
                    LogMessage("Invoking DoAsyncWorkParams");
                    await window.HybridWebView.InvokeDotNet('DoAsyncWorkParams', [123, 'hello']);
                    LogMessage("Invoked DoAsyncWorkParams");
                }

                async function InvokeDoAsyncWorkReturn() {
                    LogMessage("Invoking DoAsyncWorkReturn");
                    const retValue = await window.HybridWebView.InvokeDotNet('DoAsyncWorkReturn');
                    LogMessage("Invoked DoAsyncWorkReturn, return value: " + retValue);
                }

                async function InvokeDoAsyncWorkParamsReturn() {
                    LogMessage("Invoking DoAsyncWorkParamsReturn");
                    const retValue = await window.HybridWebView.InvokeDotNet('DoAsyncWorkParamsReturn', [123, 'hello']);
                    LogMessage("Invoked DoAsyncWorkParamsReturn, return value: message=" + retValue.Message + ", value=" + retValue.Value);
                }                

            </script>
        </head>
        <body>
            <div>
                Hybrid sample!
            </div>
            <div>
                <button onclick="window.HybridWebView.SendRawMessage('Message from JS! ' + (count++))">Send message to C#</button>
            </div>
            <div>
                <button onclick="InvokeDoSyncWork()">Call C# sync method (no params)</button>
                <button onclick="InvokeDoSyncWorkParams()">Call C# sync method (params)</button>
                <button onclick="InvokeDoSyncWorkReturn()">Call C# method (no params) and get simple return value</button>
                <button onclick="InvokeDoSyncWorkParamsReturn()">Call C# method (params) and get complex return value</button>
            </div>
            <div>
                <button onclick="InvokeDoAsyncWork()">Call C# async method (no params)</button>
                <button onclick="InvokeDoAsyncWorkParams()">Call C# async method (params)</button>
                <button onclick="InvokeDoAsyncWorkReturn()">Call C# async method (no params) and get simple return value</button>
                <button onclick="InvokeDoAsyncWorkParamsReturn()">Call C# async method (params) and get complex return value</button>
            </div>            
            <div>
                Log: <textarea readonly id="messageLog" style="width: 80%; height: 10em;"></textarea>
            </div>
            <div>
                Consider checking out this PDF: <a href="docs/sample.pdf">sample.pdf</a>
            </div>
        </body>
        </html>
        ```

        ::: moniker-end
        ::: moniker range=">=net-maui-10.0"

        ```html
        <!DOCTYPE html>

        <html lang="en" xmlns="http://www.w3.org/1999/xhtml">
        <head>
            <meta charset="utf-8" />
            <title></title>
            <link rel="icon" href="data:,">
            <link rel="stylesheet" href="styles/app.css">
            <script src="_framework/hybridwebview.js"></script>
            <script>
                function LogMessage(msg) {
                    var messageLog = document.getElementById("messageLog");
                    messageLog.value += '\r\n' + msg;
                }

                window.addEventListener(
                    "HybridWebViewMessageReceived",
                    function (e) {
                        LogMessage("Raw message: " + e.detail.message);
                    });

                function AddNumbers(a, b) {
                    var result = {
                        "result": a + b,
                        "operationName": "Addition"
                    };
                    return result;
                }

                var count = 0;

                async function EvaluateMeWithParamsAndAsyncReturn(s1, s2) {
                    const response = await fetch("/asyncdata.txt");
                    if (!response.ok) {
                        throw new Error(`HTTP error: ${response.status}`);
                    }
                    var jsonData = await response.json();

                    jsonData[s1] = s2;

                    const msg = 'JSON data is available: ' + JSON.stringify(jsonData);
                    window.HybridWebView.SendRawMessage(msg)

                    return jsonData;
                }

                async function InvokeDoSyncWork() {
                    LogMessage("Invoking DoSyncWork");
                    await window.HybridWebView.InvokeDotNet('DoSyncWork');
                    LogMessage("Invoked DoSyncWork");
                }

                async function InvokeDoSyncWorkParams() {
                    LogMessage("Invoking DoSyncWorkParams");
                    await window.HybridWebView.InvokeDotNet('DoSyncWorkParams', [123, 'hello']);
                    LogMessage("Invoked DoSyncWorkParams");
                }

                async function InvokeDoSyncWorkReturn() {
                    LogMessage("Invoking DoSyncWorkReturn");
                    const retValue = await window.HybridWebView.InvokeDotNet('DoSyncWorkReturn');
                    LogMessage("Invoked DoSyncWorkReturn, return value: " + retValue);
                }

                async function InvokeDoSyncWorkParamsReturn() {
                    LogMessage("Invoking DoSyncWorkParamsReturn");
                    const retValue = await window.HybridWebView.InvokeDotNet('DoSyncWorkParamsReturn', [123, 'hello']);
                    LogMessage("Invoked DoSyncWorkParamsReturn, return value: message=" + retValue.Message + ", value=" + retValue.Value);
                }

                async function InvokeDoAsyncWork() {
                    LogMessage("Invoking DoAsyncWork");
                    await window.HybridWebView.InvokeDotNet('DoAsyncWork');
                    LogMessage("Invoked DoAsyncWork");
                }

                async function InvokeDoAsyncWorkParams() {
                    LogMessage("Invoking DoAsyncWorkParams");
                    await window.HybridWebView.InvokeDotNet('DoAsyncWorkParams', [123, 'hello']);
                    LogMessage("Invoked DoAsyncWorkParams");
                }

                async function InvokeDoAsyncWorkReturn() {
                    LogMessage("Invoking DoAsyncWorkReturn");
                    const retValue = await window.HybridWebView.InvokeDotNet('DoAsyncWorkReturn');
                    LogMessage("Invoked DoAsyncWorkReturn, return value: " + retValue);
                }

                async function InvokeDoAsyncWorkParamsReturn() {
                    LogMessage("Invoking DoAsyncWorkParamsReturn");
                    const retValue = await window.HybridWebView.InvokeDotNet('DoAsyncWorkParamsReturn', [123, 'hello']);
                    LogMessage("Invoked DoAsyncWorkParamsReturn, return value: message=" + retValue.Message + ", value=" + retValue.Value);
                }                

            </script>
        </head>
        <body>
            <div>
                Hybrid sample!
            </div>
            <div>
                <button onclick="window.HybridWebView.SendRawMessage('Message from JS! ' + (count++))">Send message to C#</button>
            </div>
            <div>
                <button onclick="InvokeDoSyncWork()">Call C# sync method (no params)</button>
                <button onclick="InvokeDoSyncWorkParams()">Call C# sync method (params)</button>
                <button onclick="InvokeDoSyncWorkReturn()">Call C# method (no params) and get simple return value</button>
                <button onclick="InvokeDoSyncWorkParamsReturn()">Call C# method (params) and get complex return value</button>
            </div>
            <div>
                <button onclick="InvokeDoAsyncWork()">Call C# async method (no params)</button>
                <button onclick="InvokeDoAsyncWorkParams()">Call C# async method (params)</button>
                <button onclick="InvokeDoAsyncWorkReturn()">Call C# async method (no params) and get simple return value</button>
                <button onclick="InvokeDoAsyncWorkParamsReturn()">Call C# async method (params) and get complex return value</button>
            </div>            
            <div>
                Log: <textarea readonly id="messageLog" style="width: 80%; height: 10em;"></textarea>
            </div>
            <div>
                Consider checking out this PDF: <a href="docs/sample.pdf">sample.pdf</a>
            </div>
        </body>
        </html>
        ```

        ::: moniker-end
    ::: moniker range="<=net-maui-9.0"
    - *Resources\Raw\wwwroot\scripts\HybridWebView.js* with the standard <xref:Microsoft.Maui.Controls.HybridWebView> JavaScript library:

        ```js
        window.HybridWebView = {
            "Init": function Init() {
                function DispatchHybridWebViewMessage(message) {
                    const event = new CustomEvent("HybridWebViewMessageReceived", { detail: { message: message } });
                    window.dispatchEvent(event);
                }

                if (window.chrome && window.chrome.webview) {
                    // Windows WebView2
                    window.chrome.webview.addEventListener('message', arg => {
                        DispatchHybridWebViewMessage(arg.data);
                    });
                }
                else if (window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.webwindowinterop) {
                    // iOS and MacCatalyst WKWebView
                    window.external = {
                        "receiveMessage": message => {
                            DispatchHybridWebViewMessage(message);
                        }
                    };
                }
                else {
                    // Android WebView
                    window.addEventListener('message', arg => {
                        DispatchHybridWebViewMessage(arg.data);
                    });
                }
            },

            "SendRawMessage": function SendRawMessage(message) {
                window.HybridWebView.__SendMessageInternal('__RawMessage', message);
            },

            "InvokeDotNet": async function InvokeDotNetAsync(methodName, paramValues) {
                const body = {
                    MethodName: methodName
                };

                if (typeof paramValues !== 'undefined') {
                    if (!Array.isArray(paramValues)) {
                        paramValues = [paramValues];
                    }

                    for (var i = 0; i < paramValues.length; i++) {
                        paramValues[i] = JSON.stringify(paramValues[i]);
                    }

                    if (paramValues.length > 0) {
                        body.ParamValues = paramValues;
                    }
                }

                const message = JSON.stringify(body);

                var requestUrl = `${window.location.origin}/__hwvInvokeDotNet?data=${encodeURIComponent(message)}`;

                const rawResponse = await fetch(requestUrl, {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json'
                    }
                });
                const response = await rawResponse.json();

                if (response) {
                    if (response.IsJson) {
                        return JSON.parse(response.Result);
                    }

                    return response.Result;
                }

                return null;
            },

            "__SendMessageInternal": function __SendMessageInternal(type, message) {

                const messageToSend = type + '|' + message;

                if (window.chrome && window.chrome.webview) {
                    // Windows WebView2
                    window.chrome.webview.postMessage(messageToSend);
                }
                else if (window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.webwindowinterop) {
                    // iOS and MacCatalyst WKWebView
                    window.webkit.messageHandlers.webwindowinterop.postMessage(messageToSend);
                }
                else {
                    // Android WebView
                    hybridWebViewHost.sendMessage(messageToSend);
                }
            },

            "__InvokeJavaScript": async function __InvokeJavaScript(taskId, methodName, args) {
                try {
                    var result = null;
                    if (methodName[Symbol.toStringTag] === 'AsyncFunction') {
                        result = await methodName(...args);
                    } else {
                        result = methodName(...args);
                    }
                    window.HybridWebView.__TriggerAsyncCallback(taskId, result);
                } catch (ex) {
                    console.error(ex);
                    window.HybridWebView.__TriggerAsyncFailedCallback(taskId, ex);
                }
            },

            "__TriggerAsyncCallback": function __TriggerAsyncCallback(taskId, result) {
                const json = JSON.stringify(result);
                window.HybridWebView.__SendMessageInternal('__InvokeJavaScriptCompleted', taskId + '|' + json);
            },

            "__TriggerAsyncFailedCallback": function __TriggerAsyncCallback(taskId, error) {

                if (!error) {
                    json = {
                        Message: "Unknown error",
                        StackTrace: Error().stack
                    };
                } else if (error instanceof Error) {
                    json = {
                        Name: error.name,
                        Message: error.message,
                        StackTrace: error.stack
                    };
                } else if (typeof (error) === 'string') {
                    json = {
                        Message: error,
                        StackTrace: Error().stack
                    };
                } else {
                    json = {
                        Message: JSON.stringify(error),
                        StackTrace: Error().stack
                    };
                }

                json = JSON.stringify(json);
                window.HybridWebView.__SendMessageInternal('__InvokeJavaScriptFailed', taskId + '|' + json);
            }
        }

        window.HybridWebView.Init();
        ```

    ::: moniker-end
    Then, add any additional web content to your project.

    > [!WARNING]
    > In some cases Visual Studio might add entries to the project's *.csproj* file that are incorrect. When using the default location for raw assets there shouldn't be any entries for these files or folders in the *.csproj* file.

1. Add the <xref:Microsoft.Maui.Controls.HybridWebView> control to your app:

    ```xaml
    <Grid RowDefinitions="Auto,*"
          ColumnDefinitions="*">
        <Button Text="Send message to JavaScript"
                Clicked="OnSendMessageButtonClicked" />
        <HybridWebView x:Name="hybridWebView"
                       RawMessageReceived="OnHybridWebViewRawMessageReceived"
                       Grid.Row="1" />
    </Grid>
    ```

1. Modify the `CreateMauiApp` method of your `MauiProgram` class to enable developer tools on the underlying WebView controls when your app is running in debug configuration. To do this, call the <xref:Microsoft.Maui.Hosting.HybridWebViewServiceCollectionExtensions.AddHybridWebViewDeveloperTools%2A> method on the <xref:Microsoft.Extensions.DependencyInjection.IServiceCollection> object:

    ```csharp
    using Microsoft.Extensions.Logging;

    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

    #if DEBUG
            builder.Services.AddHybridWebViewDeveloperTools();
            builder.Logging.AddDebug();            
    #endif
            // Register any app services on the IServiceCollection object

            return builder.Build();
        }
    }
    ```

1. Use the <xref:Microsoft.Maui.Controls.HybridWebView> APIs to send messages between the JavaScript and C# code:

    ::: moniker range="<=net-maui-9.0"

    ```csharp
    private void OnSendMessageButtonClicked(object sender, EventArgs e)
    {
        hybridWebView.SendRawMessage($"Hello from C#!");
    }

    private async void OnHybridWebViewRawMessageReceived(object sender, HybridWebViewRawMessageReceivedEventArgs e)
    {
        await DisplayAlert("Raw Message Received", e.Message, "OK");
    }
    ```

    ::: moniker-end

    ::: moniker range=">=net-maui-10.0"

    ```csharp
    private void OnSendMessageButtonClicked(object sender, EventArgs e)
    {
        hybridWebView.SendRawMessage($"Hello from C#!");
    }

    private async void OnHybridWebViewRawMessageReceived(object sender, HybridWebViewRawMessageReceivedEventArgs e)
    {
        await DisplayAlertAsync("Raw Message Received", e.Message, "OK");
    }
    ```

    ::: moniker-end

    The messages above are classed as raw because no additional processing is performed. You can also encode data within the message to perform more advanced messaging.

## Invoke JavaScript from C\#

Your app's C# code can synchronously and asynchronously invoke JavaScript methods within the <xref:Microsoft.Maui.Controls.HybridWebView>, with optional parameters and an optional return value. This can be achieved with the <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A> and <xref:Microsoft.Maui.Controls.HybridWebView.EvaluateJavaScriptAsync%2A> methods:

- The <xref:Microsoft.Maui.Controls.HybridWebView.EvaluateJavaScriptAsync%2A> method runs the JavaScript code provided via a parameter and returns the result as a string.
- The <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A> method invokes a specified JavaScript method, optionally passing in parameter values, and specifies a generic argument that indicates the type of the return value. It returns an object of the generic argument type that contains the return value of the called JavaScript method. Internally, parameters and return values are JSON encoded.

::: moniker range=">=net-maui-10.0"

> [!NOTE]
> .NET 10 includes an <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A> overload that invokes a specified JavaScript method without specifying any information about the return type. For more information, see [Invoke JavaScript methods that don't return a value](#invoke-javascript-methods-that-dont-return-a-value).

::: moniker-end

### Invoke synchronous JavaScript

Synchronous JavaScript methods can be invoked with the <xref:Microsoft.Maui.Controls.HybridWebView.EvaluateJavaScriptAsync%2A> and <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A> methods. In the following example the <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A> method is used to demonstrate invoking JavaScript that's embedded in an app's web content. For example, a simple Javascript method to add two numbers could be defined in your web content:

```javascript
function AddNumbers(a, b) {
    return a + b;
}
```

The `AddNumbers` JavaScript method can be invoked from C# with the <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A> method:

```csharp
double x = 123d;
double y = 321d;

double result = await hybridWebView.InvokeJavaScriptAsync<double>(
    "AddNumbers", // JavaScript method name
    HybridSampleJSContext.Default.Double, // JSON serialization info for return type
    [x, y], // Parameter values
    [HybridSampleJSContext.Default.Double, HybridSampleJSContext.Default.Double]); // JSON serialization info for each parameter
```

The method invocation requires specifying `JsonTypeInfo` objects that include serialization information for the types used in the operation. These objects are automatically created by including the following `partial` class in your project:

```csharp
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(double))]
internal partial class HybridSampleJsContext : JsonSerializerContext
{
    // This type's attributes specify JSON serialization info to preserve type structure
    // for trimmed builds.
}
```

> [!IMPORTANT]
> The `HybridSampleJsContext` class must be `partial` so that code generation can provide the implementation when the project is compiled. If the type is nested into another type, then that type must also be `partial`.

### Invoke asynchronous JavaScript

Asynchronous JavaScript methods can be invoked with the <xref:Microsoft.Maui.Controls.HybridWebView.EvaluateJavaScriptAsync%2A> and <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A> methods. In the following example the <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A> method is used to demonstrate invoking JavaScript that's embedded in an app's web content. For example, a Javascript method that asynchronously retrieves data could be defined in your web content:

```javascript
async function EvaluateMeWithParamsAndAsyncReturn(s1, s2) {
    const response = await fetch("/asyncdata.txt");
    if (!response.ok) {
            throw new Error(`HTTP error: ${response.status}`);
    }
    var jsonData = await response.json();
    jsonData[s1] = s2;

    return jsonData;
}
```

The `EvaluateMeWithParamsAndAsyncReturn` JavaScript method can be invoked from C# with the <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A> method:

```csharp
Dictionary<string, string> asyncResult = await hybridWebView.InvokeJavaScriptAsync<Dictionary<string, string>>(
    "EvaluateMeWithParamsAndAsyncReturn", // JavaScript method name
    HybridSampleJSContext.Default.DictionaryStringString, // JSON serialization info for return type
    ["new_key", "new_value"], // Parameter values
    [HybridSampleJSContext.Default.String, HybridSampleJSContext.Default.String]); // JSON serialization info for each parameter
```

In this example, `asyncResult` is a `Dictionary<string, string>` that contains the JSON data from the web request.

The method invocation requires specifying `JsonTypeInfo` objects that include serialization information for the types used in the operation. These objects are automatically created by including the following `partial` class in your project:

```csharp
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(string))]
internal partial class HybridSampleJSContext : JsonSerializerContext
{
    // This type's attributes specify JSON serialization info to preserve type structure
    // for trimmed builds.  
}
```

> [!IMPORTANT]
> The `HybridSampleJsContext` class must be `partial` so that code generation can provide the implementation when the project is compiled. If the type is nested into another type, then that type must also be `partial`.

### Invoke JavaScript methods that don't return a value

The <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A> method can also be used to invoke JavaScript methods that don't return a value. There are alternative approaches to doing this:

::: moniker range=">=net-maui-10.0"

- Invoke the <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A>, specifying the JavaScript method name and any optional parameters:

    ```csharp
    await hybridWebView.InvokeJavaScriptAsync("javaScriptWithVoidReturn"); // JavaScript method name
    ```

    In this example, only the JavaScript method name is specified.

::: moniker-end

- Invoke the <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A> method without specifying the generic argument:

    ```csharp
    await hybridWebView.InvokeJavaScriptAsync(
         "javaScriptWithParamsAndVoidReturn",  // JavaScript method name
         HybridSampleJSContext.Default.Double, // JSON serialization info for return type
         [x, y], // Parameter values
         [HybridSampleJSContext.Default.Double, HybridSampleJSContext.Default.Double]); // JSON serialization info for each parameter
    ```

    In this example, while the generic argument isn't required it's still necessary to supply JSON serialization information for the return type even though it isn't used.

- Invoke the <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A> method while specifying the generic argument:

    ```csharp
    await hybridWebView.InvokeJavaScriptAsync<double>(
        "javaScriptWithParamsAndVoidReturn", // JavaScript method name
        null, // JSON serialization info for return type
        [x, y], // Parameter values
        [HybridSampleJSContext.Default.Double, HybridSampleJSContext.Default.Double]); // JSON serialization info for each parameter
    ```

    In this example, the generic argument is required and `null` can be passed as the value of the JSON serialization information for the return type.

### Send JavaScript exceptions to .NET

::: moniker range="<=net-maui-9.0"

By default, invocation of JavaScript methods in a <xref:Microsoft.Maui.Controls.HybridWebView> can hide exceptions thrown by your JavaScript code. To opt into JavaScript exceptions being sent to .NET, where they're re-thrown as .NET exceptions, add the following code to your `MauiProgram` class:

```csharp
static MauiProgram()
{
    AppContext.SetSwitch("HybridWebView.InvokeJavaScriptThrowsExceptions", true);
}
```

::: moniker-end

::: moniker range=">=net-maui-9.0"

By default, any exceptions that are thrown by your JavaScript code will be sent to .NET, where they're re-thrown as .NET exceptions.

::: moniker-end

This enables scenarios where if your C# code calls JavaScript code, and the JavaScript code fails, the JavaScript failure will be sent to .NET where it's re-thrown as a .NET exception that can be caught and handled.

## Invoke C\# from JavaScript

Your app's JavaScript code within the <xref:Microsoft.Maui.Controls.HybridWebView> can synchronously and asynchronously invoke C# methods, with optional parameters and an optional return value. This can be achieved by:

- Defining public C# methods that will be invoked from JavaScript.
- Calling the <xref:Microsoft.Maui.Controls.HybridWebView.SetInvokeJavaScriptTarget%2A> method to set the object that will be the target of JavaScript calls from the <xref:Microsoft.Maui.Controls.HybridWebView>.
- Calling the C# methods from JavaScript.

The following example defines public synchronous and asynchronous methods for invoking from JavaScript:

```csharp
public partial class MainPage : ContentPage
{
    ...  

    public void DoSyncWork()
    {
        Debug.WriteLine("DoSyncWork");
    }

    public void DoSyncWorkParams(int i, string s)
    {
        Debug.WriteLine($"DoSyncWorkParams: {i}, {s}");
    }

    public string DoSyncWorkReturn()
    {
        Debug.WriteLine("DoSyncWorkReturn");
        return "Hello from C#!";
    }

    public SyncReturn DoSyncWorkParamsReturn(int i, string s)
    {
        Debug.WriteLine($"DoSyncWorkParamReturn: {i}, {s}");
        return new SyncReturn
        {
            Message = "Hello from C#!" + s,
            Value = i
        };
    }

    public async Task DoAsyncWork()
    {
        Debug.WriteLine("DoAsyncWork");
        await Task.Delay(1000);
    }

    public async Task DoAsyncWorkParams(int i, string s)
    {
        Debug.WriteLine($"DoAsyncWorkParams: {i}, {s}");
        await Task.Delay(1000);
    }

    public async Task<String> DoAsyncWorkReturn()
    {
        Debug.WriteLine("DoAsyncWorkReturn");
        await Task.Delay(1000);
        return "Hello from C#!";
    }

    public async Task<SyncReturn> DoAsyncWorkParamsReturn(int i, string s)
    {
        Debug.WriteLine($"DoAsyncWorkParamsReturn: {i}, {s}");
        await Task.Delay(1000);
        return new SyncReturn
        {
            Message = "Hello from C#!" + s,
            Value = i
        };
    }    

    public class SyncReturn
    {
        public string? Message { get; set; }
        public int Value { get; set; }
    }  
}
```

You must then call the <xref:Microsoft.Maui.Controls.HybridWebView.SetInvokeJavaScriptTarget%2A> method to set the object that will be the target of JavaScript calls from the <xref:Microsoft.Maui.Controls.HybridWebView>:

```csharp
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        hybridWebView.SetInvokeJavaScriptTarget(this);
    }
    ...
}
```

The public methods on the object set via the <xref:Microsoft.Maui.Controls.HybridWebView.SetInvokeJavaScriptTarget%2A> method can then be invoked from JavaScript with the `window.HybridWebView.InvokeDotNet` function:

```js
// Synchronous methods
await window.HybridWebView.InvokeDotNet('DoSyncWork');
await window.HybridWebView.InvokeDotNet('DoSyncWorkParams', [123, 'hello']);
const retValue = await window.HybridWebView.InvokeDotNet('DoSyncWorkReturn');
const retValue = await window.HybridWebView.InvokeDotNet('DoSyncWorkParamsReturn', [123, 'hello']);

// Asynchronous methods
await window.HybridWebView.InvokeDotNet('DoAsyncWork');
await window.HybridWebView.InvokeDotNet('DoAsyncWorkParams', [123, 'hello']);
const retValue = await window.HybridWebView.InvokeDotNet('DoAsyncWorkReturn');
const retValue = await window.HybridWebView.InvokeDotNet('DoAsyncWorkParamsReturn', [123, 'hello']);
```

The `window.HybridWebView.InvokeDotNet` JavaScript function invokes a specified C# method, with optional parameters and an optional return value.

> [!NOTE]
> Invoking the `window.HybridWebView.InvokeDotNet` JavaScript function requires your app to include the *HybridWebView.js* JavaScript library listed earlier in this article.

::: moniker range=">=net-maui-10.0"

## Customize initialization and access platform web views

While <xref:Microsoft.Maui.Controls.HybridWebView> doesn’t expose app-facing initializing/initialized events like <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView>, you can still customize the underlying platform web views and run code after they’re ready:

- Windows (WebView2): the platform view is <xref:Microsoft.Maui.Controls.HybridWebView>, which inherits `WebView2` and adds `RunAfterInitialize(Action)` so you can safely access `CoreWebView2` once it’s ready.
- Android (android.webkit.WebView): access and configure the platform `WebView` via the handler once it’s created.
- iOS/Mac Catalyst (WKWebView): access and configure the platform `WKWebView` after creation. Some options (such as certain `WKWebViewConfiguration` settings) must be set at creation time; .NET MAUI sets sensible defaults for these.

### Access the platform view after handler creation

Handle `HandlerChanged` (or override `OnHandlerChanged` in a custom control) and branch by platform:

```csharp
using Microsoft.Maui.Platform; // For MauiHybridWebView on Windows

void HybridWebView_HandlerChanged(object? sender, EventArgs e)
{
    if (sender is not HybridWebView hv || hv.Handler?.PlatformView is null)
        return;

#if WINDOWS
    if (hv.Handler.PlatformView is MauiHybridWebView winView)
    {
        winView.RunAfterInitialize(() =>
        {
            // CoreWebView2 is guaranteed to be initialized here
            winView.CoreWebView2.Settings.IsZoomControlEnabled = false;
            winView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
        });
    }
#elif ANDROID
    if (hv.Handler.PlatformView is Android.Webkit.WebView androidView)
    {
        // Safe to tweak most settings after creation
        androidView.Settings.BuiltInZoomControls = false;
        androidView.Settings.DisplayZoomControls = false;
    }
#elif IOS || MACCATALYST
    if (hv.Handler.PlatformView is WebKit.WKWebView wk)
    {
        wk.AllowsBackForwardNavigationGestures = true;
        // Many WKWebViewConfiguration options can’t be changed now – see note below
    }
#endif
}
```

Wire this up once, for example in XAML code-behind:

```csharp
public MainPage()
{
    InitializeComponent();
    hybridWebView.HandlerChanged += HybridWebView_HandlerChanged;
}
```

> [!IMPORTANT]
> On iOS/Mac Catalyst, some `WKWebViewConfiguration` options must be set before the view is created. .NET MAUI enables common options by default (inline media playback, autoplay, JavaScript, etc.) so typical scenarios work without extra code. If you need different creation-time options, use the advanced approach below.

### Advanced: provide creation-time configuration with a custom handler

If you need to alter creation-time options (for example, to change `WKWebViewConfiguration` on iOS/Mac Catalyst), register a custom handler and override `CreatePlatformView`:

```csharp
// In MauiProgram.cs
builder.ConfigureMauiHandlers(handlers =>
{
    handlers.AddHandler<HybridWebView, MyHybridWebViewHandler>();
});

// Custom handler (iOS/Mac Catalyst shown; similar ideas apply for other platforms)
public class MyHybridWebViewHandler : HybridWebViewHandler
{
#if IOS || MACCATALYST
    protected override WebKit.WKWebView CreatePlatformView()
    {
        var config = new WebKit.WKWebViewConfiguration
        {
            // Example: change defaults established by MAUI
            AllowsInlineMediaPlayback = false,
        };

        // Recreate the platform view with your configuration
        var webview = new MauiHybridWebView(this, CoreGraphics.CGRect.Empty, config);
        return webview;
    }
#endif
}
```

> [!CAUTION]
> Creation-time configuration is an advanced scenario. Validate behavior on each platform, and prefer post-initialization tweaks when possible.

## Intercept web requests

<xref:Microsoft.Maui.Controls.HybridWebView> can intercept and respond to web requests that originate from within the hosted web content. This enables scenarios such as modifying headers, redirecting requests, or supplying local responses.

To intercept web requests, handle the `WebResourceRequested` event. In the event handler, set `Handled` to `true` and provide a response via `SetResponse(statusCode, statusDescription, contentType, streamOrTaskOfStream)`:

```xaml
<HybridWebView WebResourceRequested="HybridWebView_WebResourceRequested" />
```

```csharp
private void HybridWebView_WebResourceRequested(object sender, HybridWebViewWebResourceRequestedEventArgs e)
{
    // NOTE:
    // - This method MUST be synchronous; it's invoked on the WebView's thread.
    // - You MUST call SetResponse (even a minimal response) if you set Handled = true.

    // Example: serve a local image instead of the network resource
    if (e.Uri.ToString().EndsWith("sample-image.png", StringComparison.OrdinalIgnoreCase))
    {
        e.Handled = true;
        e.SetResponse(200, "OK", "image/png", GetLocalImageStreamAsync());
        return;
    }

    // Example: inject an authorization header for API calls
    if (e.Uri.Host.Equals("api.contoso.com", StringComparison.OrdinalIgnoreCase))
    {
        e.RequestHeaders["Authorization"] = $"Bearer {GetToken()}";
        // Fall through without setting Handled so the request proceeds normally
    }
}

private Task<Stream> GetLocalImageStreamAsync()
{
    // Return a stream containing PNG bytes (for example from a MauiAsset)
    return FileSystem.OpenAppPackageFileAsync("wwwroot/images/sample-image.png");
}
```

> [!CAUTION]
> Avoid long-running work in `WebResourceRequested`. If you set `Handled = true`, you must supply a response immediately. For asynchronous content, pass a `Task<Stream>` to `SetResponse` so the WebView can continue while the stream completes.

Common patterns include:

- Injecting or rewriting headers for specific hosts.
- Returning local files or in-memory content for offline or testing scenarios.
- Redirecting to a different URI by returning a 3xx status code with an appropriate `Location` header.

::: moniker-end
