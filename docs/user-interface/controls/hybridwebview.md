---
title: HybridWebView
description: Learn how to
ms.topic: concept-article
ms.date: 08/20/2024
monikerRange: ">=net-maui-9.0"

#customer intent: As a developer, I want to XXXXXXX so that I can YYYYYYY.
---

# HybridWebView

<xref:Microsoft.Maui.Controls.HybridWebView> enables hosting arbitrary HTML/JS/CSS content in a WebView, and enables communication between the code in the WebView (JavaScript) and the code that hosts the WebView (C#/.NET). For example, if you have an existing React JS app, you could host it in a cross-platform .NET MAUI native app, and build the back-end of the app using C# and .NET.

To build a .NET MAUI app with `HybridWebView` you need:

- The web content of the app, which consists of static HTML, JavaScript, CSS, images, and other files.
- A `HybridWebView` control as part of the app's UI. This can be achieved by referencing it in the app's XAML.
- Code in the web content, and in C#/.NET, that uses the `HybridWebView` APIs to send messages between the two components.

The entire app, including the web content, is packaged and runs locally on a device, and can be published to applicable app stores. The web content is hosted within a native WebView control and runs within the context of the app. Any part of the app can access external web services, but is'nt required to.

To build a hybrid app:

1. Open an existing .NET MAUI app project or create a new .NET MAUI app project.
1. Add your web content to the .NET MAUI app project.

    Your app's web content should be included as part of a .NET MAUI project as raw assets. A raw asset is any file in the app's *Resources\Raw* folder, and includes sub-folders. For a `HybridWebView`, web content should be placed in the *Resources\Raw\wwwroot* folder, with the main file named *index.html*.

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

    - *Resources\Raw\wwwroot\scripts\HybridWebView.js* with the standard `HybridWebView` JavaScript library:

        ```js
        function HybridWebViewInit() {

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
        }

        window.HybridWebView = {
            "SendRawMessage": function (message) {

                if (window.chrome && window.chrome.webview) {
                    // Windows WebView2
                    window.chrome.webview.postMessage(message);
                }
                else if (window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.webwindowinterop) {
                    // iOS and MacCatalyst WKWebView
                    window.webkit.messageHandlers.webwindowinterop.postMessage(message);
                }
                else {
                    // Android WebView
                    hybridWebViewHost.sendRawMessage(message);
                }
            }
        }

        HybridWebViewInit();
        ```

    Then, add any additional web content to your project.

    > [!WARNING]
    > In some cases Visual Studio might add entries to the project's *.csproj* file that are incorrect. When using the default location for raw assets there shouldn't be any entries for these files or folders in the *.csproj* file.

1. Add the `HybridWebView` control to your app:

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

1. Use the `HybridWebView` APIs to send messages between the JavaScript and C# code:

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
