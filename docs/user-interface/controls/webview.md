---
title: "WebView"
description: "This article explains how to use the .NET MAUI WebView to display remote web pages, local HTML files, and HTML strings."
ms.date: 10/11/2022
---

# WebView

The .NET Multi-platform App UI (.NET MAUI) `WebView` displays remote web pages, local HTML files, and HTML strings, in an app. The content displayed a `WebView` includes support for Cascading Style Sheets (CSS), and JavaScript. By default, .NET MAUI projects include the platform permissions required for a `WebView` to display a remote web page.

`WebView` defines the following properties:

- `Cookies`, of type `CookieContainer`, provides storage for a collection of cookies.
- `CanGoBack`, of type `bool`, indicates whether the user can navigate to previous pages. This is a read-only property.
- `CanGoForward`, of type `bool`, indicates whether the user can navigate forward. This is a read-only property.
- `Source`, of type `WebViewSource`, represents the location that the `WebView` displays.

These properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

The `Source` property can be set to an `UrlWebViewSource` object or a `HtmlWebViewSource` object, which both derive from `WebViewSource`. A `UrlWebViewSource` is used for loading a web page specified with a URL, while a `HtmlWebViewSource` object is used for loading a local HTML file, or local HTML.

`WebView` defines a `Navigating` event that's raised when page navigation starts, and a `Navigated` event that's raised when page navigation completes. The `WebNavigatingEventArgs` object that accompanies the `Navigating` event defines a `Cancel` property of type `bool` that can be used to cancel navigation. The `WebNavigatedEventArgs` object that accompanies the `Navigated` event defines a `Result` property of type `WebNavigationResult` that indicates the navigation result.

> [!IMPORTANT]
> A `WebView` must specify its `HeightRequest` and `WidthRequest` properties when contained in a `HorizontalStackLayout`, `StackLayout`, or `VerticalStackLayout`. If you fail to specify these properties, the `WebView` will not render.

## Display a web page

To display a remote web page, set the `Source` property to a `string` that specifies the URI:

```xaml
<WebView Source="https://learn.microsoft.com/dotnet/maui" />
```

The equivalent C# code is:

```csharp
WebView webvView = new WebView
{
    Source = "https://learn.microsoft.com/dotnet/maui"
};
```

URIs must be fully formed with the protocol specified.

> [!NOTE]
> Despite the `Source` property being of type `WebViewSource`, the property can be set to a string-based URI. This is because .NET MAUI includes a type converter, and an implicit conversion operator, that converts the string-based URI to a `UrlWebViewSource` object.

### Configure App Transport Security on iOS

Since version 9, iOS will only allow your app to communicate with secure servers. An app has to opt into enabling communication with insecure servers.

The following *Info.plist* configuration shows how to enable a specific domain to bypass Apple Transport Security (ATS) requirements:

```xml
<key>NSAppTransportSecurity</key>
    <dict>
        <key>NSExceptionDomains</key>
        <dict>
            <key>mydomain.com</key>
            <dict>
                <key>NSIncludesSubdomains</key>
                <true/>
                <key>NSTemporaryExceptionAllowsInsecureHTTPLoads</key>
                <true/>
                <key>NSTemporaryExceptionMinimumTLSVersion</key>
                <string>TLSv1.1</string>
            </dict>
        </dict>
    </dict>
    ...
</key>
```

It's best practice to only enable specific domains to bypass ATS, allowing you to use trusted sites while benefitting from additional security on untrusted domains.

The following *Info.plist* configuration shows how to disable ATS for an app:

```xml
<key>NSAppTransportSecurity</key>
    <dict>
        <key>NSAllowsArbitraryLoads</key>
        <true/>
    </dict>
    ...
</key>
```

> [!IMPORTANT]
> If your app requires a connection to an insecure website, you should always enter the domain as an exception using the `NSExceptionDomains` key instead of turning ATS off completely using the  `NSAllowsArbitraryLoads` key.

<!-- For more information, see [App Transport Security](~/ios/app-fundamentals/ats.md). -->

## Display local HTML

To display inline HTML, set the `Source` property to a `HtmlWebViewSource` object:

```xaml
<WebView>
    <WebView.Source>
        <HtmlWebViewSource Html="&lt;HTML&gt;&lt;BODY&gt;&lt;H1&gt;.NET MAUI&lt;/H1&gt;&lt;P&gt;Welcome to WebView.&lt;/P&gt;&lt;/BODY&gt;&lt;HTML&gt;" />
    </WebView.Source>
</WebView>
```

In XAML, HTML strings can become unreadable due to escaping the `<` and `>` symbols. Therefore, for greater readability the HTML can be inlined in a `CDATA` section:

```xaml
<WebView>
    <WebView.Source>
        <HtmlWebViewSource>
            <HtmlWebViewSource.Html>
                <![CDATA[
                <HTML>
                <BODY>
                <H1>.NET MAUI</H1>
                <P>Welcome to WebView.</P>
                </BODY>
                </HTML>
                ]]>
            </HtmlWebViewSource.Html>
        </HtmlWebViewSource>
    </WebView.Source>
</WebView>
```

The equivalent C# code is:

```csharp
WebView webView = new WebView
{
    Source = new HtmlWebViewSource
    {
        Html = @"<HTML><BODY><H1>.NET MAUI</H1><P>Welcome to WebView.</P></BODY></HTML>"
    }
};
```

## Display a local HTML file

To display a local HTML file, add the file to the *Resources\Raw* folder of your app project and set its build action to **MauiAsset**. Then, the file can be loaded from inline HTML that's defined in a `HtmlWebViewSource` object that's set as the value of the `Source` property:

```xaml
<WebView>
    <WebView.Source>
        <HtmlWebViewSource>
            <HtmlWebViewSource.Html>
                <![CDATA[
                <html>
                <head>
                </head>
                <body>
                <h1>.NET MAUI</h1>
                <p>The CSS and image are loaded from local files!</p>
                <p><a href="localfile.html">next page</a></p>
                </body>
                </html>                    
                ]]>
            </HtmlWebViewSource.Html>
        </HtmlWebViewSource>
    </WebView.Source>
</WebView>
```

The local HTML file can load Cascading Style Sheets (CSS), JavaScript, and images, if they've also been added to your app project with the **MauiAsset** build action.

For more information about raw assets, see [Raw assets](~/fundamentals/single-project.md#raw-assets).

<!-- Todo: This isn't ideal. You should be able to do <WebView Source="file.html" /> but you can't, as of P13. -->

## Reload content

`WebView` has a `Reload` method that can be called to reload its source:

```csharp
WebView webView = new WebView();
...
webView.Reload();
```

<!-- When the `Reload` method is invoked the `ReloadRequested` event is fired, indicating that a request has been made to reload the current content. -->

## Perform navigation

`WebView` supports programmatic navigation with the `GoBack` and `GoForward` methods. These methods enable navigation through the `WebView` page stack, and should only be called after inspecting the values of the `CanGoBack` and `CanGoForward` properties:

```csharp
WebView webView = new WebView();
...

// Go backwards, if allowed.
if (webView.CanGoBack)
{
    webView.GoBack();
}

// Go forwards, if allowed.
if (webView.CanGoForward)
{
    webView.GoForward();
}
```

When page navigation occurs in a `WebView`, either initiated programmatically or by the user, the following events occur:

- `Navigating`, which is raised when page navigation starts. The `WebNavigatingEventArgs` object that accompanies the `Navigating` event defines a `Cancel` property of type `bool` that can be used to cancel navigation.
- `Navigated`, which is raised when page navigation completes. The `WebNavigatedEventArgs` object that accompanies the `Navigated` event defines a `Result` property of type `WebNavigationResult` that indicates the navigation result.

## Set cookies

Cookies can be set on a `WebView` so that they are sent with the web request to the specified URL. Set the cookies by adding `Cookie` objects to a `CookieContainer`, and then set the container as the value of the `WebView.Cookies` bindable property. The following code shows an example:

```csharp
using System.Net;

CookieContainer cookieContainer = new CookieContainer();
Uri uri = new Uri("https://learn.microsoft.com/dotnet/maui", UriKind.RelativeOrAbsolute);

Cookie cookie = new Cookie
{
    Name = "DotNetMAUICookie",
    Expires = DateTime.Now.AddDays(1),
    Value = "My cookie",
    Domain = uri.Host,
    Path = "/"
};
cookieContainer.Add(uri, cookie);
webView.Cookies = cookieContainer;
webView.Source = new UrlWebViewSource { Url = uri.ToString() };
```

In this example, a single `Cookie` is added to the `CookieContainer` object, which is then set as the value of the `WebView.Cookies` property. When the  `WebView` sends a web request to the specified URL, the cookie is sent with the request.

## Invoke JavaScript

`WebView` includes the ability to invoke a JavaScript function from C# and return any result to the calling C# code. This interop is accomplished with the `EvaluateJavaScriptAsync` method, which is shown in the following example:

```csharp
Entry numberEntry = new Entry { Text = "5" };
Label resultLabel = new Label();
WebView webView = new WebView();
...

int number = int.Parse(numberEntry.Text);
string result = await webView.EvaluateJavaScriptAsync($"factorial({number})");
resultLabel.Text = $"Factorial of {number} is {result}.";
```

The `WebView.EvaluateJavaScriptAsync` method evaluates the JavaScript that's specified as the argument, and returns any result as a `string`. In this example, the `factorial` JavaScript function is invoked, which returns the factorial of `number` as a result. This JavaScript function is defined in the local HTML file that the `WebView` loads, and is shown in the following example:

```html
<html>
<body>
<script type="text/javascript">
function factorial(num) {
        if (num === 0 || num === 1)
            return 1;
        for (var i = num - 1; i >= 1; i--) {
            num *= i;
        }
        return num;
}
</script>
</body>
</html>
```

::: moniker range=">=net-maui-7.0"

## Configure the native WebView on iOS and Mac Catalyst

The native `WebView` control is a `MauiWKWebView` on iOS and Mac Catalyst, which derives from `WKWebView`. One of the `MauiWKWebView` constructor overloads enables a `WKWebViewConfiguration` object to be specified, which provides information about how to configure the `WKWebView` object. Typical configurations include setting the user agent, specifying cookies to make available to your web content, and injecting custom scripts into your web content.

You can create a `WKWebViewConfiguration` object in your app, and then configure its properties as required. Alternatively, you can call the static `MauiWKWebView.CreateConfiguration` method to retrieve .NET MAUI's `WKWebViewConfiguration` object and then modify it. The `WKWebViewConfiguration` object can then be passed to the `MauiWKWebView` constructor overload by modifying the factory method that `WebViewHandler` uses to create its native control on each platform:

```csharp
#if IOS || MACCATALYST
using WebKit;
using CoreGraphics;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Handlers;
#endif
...

#if IOS || MACCATALYST
    WKWebViewConfiguration config = MauiWKWebView.CreateConfiguration();
    config.ApplicationNameForUserAgent = "MyProduct/1.0.0";
    WebViewHandler.PlatformViewFactory =
        handler => new MauiWKWebView(CGRect.Empty, (WebViewHandler)handler, config);
#endif
```

> [!NOTE]
> You should configure `MauiWKWebView` with a `WKWebViewConfiguration` object before a `WebView` is displayed in your app. Suitable locations to do this are in your app's startup path, such as in *MauiProgram.cs* or *App.xaml.cs*. <!-- For more information about configuring a native .NET MAUI control, see [Customize controls](~/user-interface/handlers/customize.md). -->

::: moniker-end

## Launch the system browser

It's possible to open a URI in the system web browser with the `Launcher` class, which is provided by `Microsoft.Maui.Essentials`. Call the launcher's `OpenAsync` method and pass in a `string` or `Uri` argument that represents the URI to open:

```csharp
await Launcher.OpenAsync("https://learn.microsoft.com/dotnet/maui");
```

For more information, see [Launcher](~/platform-integration/appmodel/launcher.md).
