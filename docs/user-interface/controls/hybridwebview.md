---
title: HybridWebView
description: Learn how to use a HybridWebView to host HTML/JS/CSS content in a WebView, and communicate between that content and .NET.
ms.topic: concept-article
ms.date: 09/20/2024
monikerRange: ">=net-maui-9.0"

#customer intent: As a developer, I want to host HTML/JS/CSS content in a web view so that I can publish the web app as a mobile app.
---

# HybridWebView

<!-- [![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-hybridwebview) -->

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.HybridWebView> enables hosting arbitrary HTML/JS/CSS content in a web view, and enables communication between the code in the web view (JavaScript) and the code that hosts the web view (C#/.NET). For example, if you have an existing React JS app, you could host it in a cross-platform .NET MAUI native app, and build the back-end of the app using C# and .NET.

<xref:Microsoft.Maui.Controls.HybridWebView> defines the following properties:

- <xref:Microsoft.Maui.Controls.HybridWebView.DefaultFile>, of type `string?`, which specifies the file within the <xref:Microsoft.Maui.Controls.HybridWebView.HybridRoot> that should be served as the default file. The default value is *index.html*.
- <xref:Microsoft.Maui.Controls.HybridWebView.HybridRoot>, of type `string?`, which is the path within the app's raw asset resources that contain the web app's contents. The default value is *wwwroot*, which maps to *Resources/Raw/wwwroot*.

In addition, <xref:Microsoft.Maui.Controls.HybridWebView> defines a <xref:Microsoft.Maui.Controls.HybridWebView.RawMessageReceived> event that's raised when a raw message is received. The <xref:Microsoft.Maui.Controls.HybridWebViewRawMessageReceivedEventArgs> object that accompanies the event defines a <xref:Microsoft.Maui.Controls.HybridWebViewRawMessageReceivedEventArgs.Message> property that contains the message.

Your app's C# code can invoke synchronous and asynchronous JavaScript methods within the <xref:Microsoft.Maui.Controls.HybridWebView> with the `InvokeJavaScriptAsync` and `EvaluateJavaScriptAsync` methods. For more information, see [Invoke JavaScript from C#](#invoke-javascript-from-c).

To create a .NET MAUI app with <xref:Microsoft.Maui.Controls.HybridWebView> you need:

- The web content of the app, which consists of static HTML, JavaScript, CSS, images, and other files.
- A <xref:Microsoft.Maui.Controls.HybridWebView> control as part of the app's UI. This can be achieved by referencing it in the app's XAML.
- Code in the web content, and in C#/.NET, that uses the <xref:Microsoft.Maui.Controls.HybridWebView> APIs to send messages between the two components.

The entire app, including the web content, is packaged and runs locally on a device, and can be published to applicable app stores. The web content is hosted within a native web view control and runs within the context of the app. Any part of the app can access external web services, but isn't required to.

## Create a .NET MAUI HybridWebView app

To create a .NET MAUI app with a <xref:Microsoft.Maui.Controls.HybridWebView>:

1. Open an existing .NET MAUI app project or create a new .NET MAUI app project.
1. Add your web content to the .NET MAUI app project.

    Your app's web content should be included as part of a .NET MAUI project as raw assets. A raw asset is any file in the app's *Resources\Raw* folder, and includes sub-folders. For a default <xref:Microsoft.Maui.Controls.HybridWebView>, web content should be placed in the *Resources\Raw\wwwroot* folder, with the main file named *index.html*.

    A simple app might have the following files and contents:

    - *Resources\Raw\wwwroot\index.html* with content for the main UI:

        ```html
        <!DOCTYPE html>

        <html lang="en" xmlns="http://www.w3.org/1999/xhtml">
        <head>
            <meta charset="utf-8" />
            <title></title>
            <link rel="icon" href="data:,">
            <script src="scripts/HybridWebView.js"></script>
            <script>
                window.addEventListener(
                    "HybridWebViewMessageReceived",
                    function (e) {
                        var messageFromCSharp = document.getElementById("messageFromCSharp");
                        messageFromCSharp.value += '\r\n' + e.detail.message;
                    });
            </script>
        </head>
        <body>
            <h1>HybridWebView app!</h1>
            <div>
                <button onclick="window.HybridWebView.SendRawMessage('Message from JS!')">Send message to C#</button>
            </div>
            <div>
                Messages from C#: <textarea readonly id="messageFromCSharp" style="width: 80%; height: 300px;"></textarea>
            </div>
        </body>
        </html>
        ```

    - *Resources\Raw\wwwroot\scripts\HybridWebView.js* with the standard <xref:Microsoft.Maui.Controls.HybridWebView> JavaScript library:

        ```js
        window.HybridWebView = {
            "Init": function () {
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

            "SendRawMessage": function (message) {
                window.HybridWebView.__SendMessageInternal('RawMessage', message);
            },

            "__SendMessageInternal": function (type, message) {

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

            "InvokeMethod": function (taskId, methodName, args) {
                if (methodName[Symbol.toStringTag] === 'AsyncFunction') {
                    // For async methods, we need to call the method and then trigger the callback when it's done
                    const asyncPromise = methodName(...args);
                    asyncPromise
                        .then(asyncResult => {
                            window.HybridWebView.__TriggerAsyncCallback(taskId, asyncResult);
                        })
                        .catch(error => console.error(error));
                } else {
                    // For sync methods, we can call the method and trigger the callback immediately
                    const syncResult = methodName(...args);
                    window.HybridWebView.__TriggerAsyncCallback(taskId, syncResult);
                }
            },

            "__TriggerAsyncCallback": function (taskId, result) {
                // Make sure the result is a string
                if (result && typeof (result) !== 'string') {
                    result = JSON.stringify(result);
                }

                window.HybridWebView.__SendMessageInternal('InvokeMethodCompleted', taskId + '|' + result);
            }
        }

        window.HybridWebView.Init();
        ```

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

1. Use the <xref:Microsoft.Maui.Controls.HybridWebView> APIs to send messages between the JavaScript and C# code:

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

    The messages above are classed as raw because no additional processing is performed. You can also encode data within the message to perform more advanced messaging.

## Invoke JavaScript from C\#

Your app's C# code can synchronously and asynchronously invoke JavaScript methods within the <xref:Microsoft.Maui.Controls.HybridWebView>, with optional parameters and an optional return value. This can be achieved with the `InvokeJavaScriptAsync` and `EvaluateJavaScriptAsync` methods:

- The `EvaluateJavaScriptAsync` method runs the JavaScript code provided via a parameter and returns the result as a string.
- The `InvokeJavaScriptAsync` method invokes a specified JavaScript method, optionally passing in parameter values, and returns a string containing the return value of the JavaScript method. Internally, parameters and return values are JSON encoded.

### Invoke synchronous JavaScript

Synchronous JavaScript methods can be invoked with the `EvaluateJavaScriptAsync` and `InvokeJavaScriptAsync` methods. In the following example the `InvokeJavaScriptAsync` method is used to demonstrate invoking JavaScript that's embedded in an app's web content. For example, a simple Javascript method to add two numbers could be defined in your web content:

```javascript
function AddNumbers(a, b) {
    return a + b;
}
```

The `AddNumbers` JavaScript method can be invoked from C# with the `InvokeJavaScriptAsync` method:

```csharp
double x = 123d;
double y = 321d;

var result = await hybridWebView.InvokeJavaScriptAsync<double>(
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

Asynchronous JavaScript methods can be invoked with the `EvaluateJavaScriptAsync` and `InvokeJavaScriptAsync` methods. In the following example the `InvokeJavaScriptAsync` method is used to demonstrate invoking JavaScript that's embedded in an app's web content. For example, a Javascript method that asynchronously retrieves data could be defined in your web content:

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

The `EvaluateMeWithParamsAndAsyncReturn` JavaScript method can be invoked from C# with the `InvokeJavaScriptAsync` method:

```csharp
var asyncResult = await hybridWebView.InvokeJavaScriptAsync<Dictionary<string, string>>(
    "EvaluateMeWithParamsAndAsyncReturn", // JavaScript method name
    HybridSampleJSContext.Default.DictionaryStringString, // JSON serialization info for return type
    ["new_key", "new_value"], // Parameter values
    [HybridSampleJSContext.Default.String, HybridSampleJSContext.Default.String]); // JSON serialization info for each parameter
```

In this example, `asyncResult` is a `Dictionary<string, string>` containing the JSON data from the web request.

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
