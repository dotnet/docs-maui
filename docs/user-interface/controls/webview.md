---
title: "WebView"
description: "This article explains how to use the .NET MAUI WebView to display remote web pages, local HTML files, and HTML strings."
ms.date: 10/23/2023
zone_pivot_groups: devices-platforms
---

# WebView

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.WebView> displays remote web pages, local HTML files, and HTML strings, in an app. The content displayed a <xref:Microsoft.Maui.Controls.WebView> includes support for Cascading Style Sheets (CSS), and JavaScript. By default, .NET MAUI projects include the platform permissions required for a <xref:Microsoft.Maui.Controls.WebView> to display a remote web page.

<xref:Microsoft.Maui.Controls.WebView> defines the following properties:

- <xref:Microsoft.Maui.Controls.WebView.Cookies>, of type `CookieContainer`, provides storage for a collection of cookies.
- <xref:Microsoft.Maui.Controls.WebView.CanGoBack>, of type `bool`, indicates whether the user can navigate to previous pages. This is a read-only property.
- <xref:Microsoft.Maui.Controls.WebView.CanGoForward>, of type `bool`, indicates whether the user can navigate forward. This is a read-only property.
- <xref:Microsoft.Maui.Controls.WebView.Source>, of type `WebViewSource`, represents the location that the <xref:Microsoft.Maui.Controls.WebView> displays.
- <xref:Microsoft.Maui.Controls.WebView.UserAgent>, of type `string`, represents the user agent. The default value is the user agent of the underlying platform browser, or `null` if it can't be determined.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The `Source` property can be set to an `UrlWebViewSource` object or a `HtmlWebViewSource` object, which both derive from `WebViewSource`. A `UrlWebViewSource` is used for loading a web page specified with a URL, while a `HtmlWebViewSource` object is used for loading a local HTML file, or local HTML.

::: moniker range="=net-maui-8.0"

<xref:Microsoft.Maui.Controls.WebView> defines a `Navigating` event that's raised when page navigation starts, and a `Navigated` event that's raised when page navigation completes. The `WebNavigatingEventArgs` object that accompanies the `Navigating` event defines a `Cancel` property of type `bool` that can be used to cancel navigation. The `WebNavigatedEventArgs` object that accompanies the `Navigated` event defines a `Result` property of type `WebNavigationResult` that indicates the navigation result.
::: moniker-end

::: moniker range=">=net-maui-9.0"

<xref:Microsoft.Maui.Controls.WebView> defines the following events:

- `Navigating`, that's raised when page navigation starts. The `WebNavigatingEventArgs` object that accompanies this event defines a `Cancel` property of type `bool` that can be used to cancel navigation.
- `Navigated`, that's raised when page navigation completes. The `WebNavigatedEventArgs` object that accompanies this event defines a `Result` property of type `WebNavigationResult` that indicates the navigation result.
- `ProcessTerminated`, that's raised when a <xref:Microsoft.Maui.Controls.WebView> process ends unexpectedly. The `WebViewProcessTerminatedEventArgs` object that accompanies this event defines platform-specific properties that indicate why the process failed.

::: moniker-end

> [!IMPORTANT]
> A <xref:Microsoft.Maui.Controls.WebView> must specify its <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> and <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest> properties when contained in a <xref:Microsoft.Maui.Controls.HorizontalStackLayout>, <xref:Microsoft.Maui.Controls.StackLayout>, or <xref:Microsoft.Maui.Controls.VerticalStackLayout>. If you fail to specify these properties, the <xref:Microsoft.Maui.Controls.WebView> will not render.

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

:::zone pivot="devices-windows"

<!-- blank pivot to skirt the warning about using every pivot type -->

:::zone-end

:::zone pivot="devices-ios, devices-maccatalyst"

## Configure App Transport Security on iOS and Mac Catalyst

Since version 9, iOS will only allow your app to communicate with secure servers. An app has to opt into enabling communication with insecure servers.

The following *Info.plist* configuration shows how to enable a specific domain to bypass Apple Transport Security (ATS) requirements:

<!-- markdownlint-disable MD010 -->
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
```

It's best practice to only enable specific domains to bypass ATS, allowing you to use trusted sites while benefitting from additional security on untrusted domains.

The following *Info.plist* configuration shows how to disable ATS for an app:

```xml
	<key>NSAppTransportSecurity</key>
	<dict>
		<key>NSAllowsArbitraryLoads</key>
		<true/>
	</dict>
```
<!-- markdownlint-enable MD010 -->

> [!IMPORTANT]
> If your app requires a connection to an insecure website, you should always enter the domain as an exception using the `NSExceptionDomains` key instead of turning ATS off completely using the  `NSAllowsArbitraryLoads` key.

<!-- For more information, see [App Transport Security](~/ios/app-fundamentals/ats.md). -->

:::zone-end

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

<xref:Microsoft.Maui.Controls.WebView> has a `Reload` method that can be called to reload its source:

```csharp
WebView webView = new WebView();
...
webView.Reload();
```

When the `Reload` method is invoked the `ReloadRequested` event is fired, indicating that a request has been made to reload the current content.

## Perform navigation

<xref:Microsoft.Maui.Controls.WebView> supports programmatic navigation with the `GoBack` and `GoForward` methods. These methods enable navigation through the <xref:Microsoft.Maui.Controls.WebView> page stack, and should only be called after inspecting the values of the `CanGoBack` and `CanGoForward` properties:

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

When page navigation occurs in a <xref:Microsoft.Maui.Controls.WebView>, either initiated programmatically or by the user, the following events occur:

- `Navigating`, which is raised when page navigation starts. The `WebNavigatingEventArgs` object that accompanies the `Navigating` event defines a `Cancel` property of type `bool` that can be used to cancel navigation.
- `Navigated`, which is raised when page navigation completes. The `WebNavigatedEventArgs` object that accompanies the `Navigated` event defines a `Result` property of type `WebNavigationResult` that indicates the navigation result.

:::zone pivot="devices-android"

## Handle permissions on Android

When browsing to a page that requests access to the device's recording hardware, such as the camera or microphone, permission must be granted by the <xref:Microsoft.Maui.Controls.WebView> control. The `WebView` control uses the <xref:Android.Webkit.WebChromeClient?displayProperty=fullName> type on Android to react to permission requests. However, the `WebChromeClient` implementation provided by .NET MAUI ignores permission requests. You must create a new type that inherits from `MauiWebChromeClient` and approves the permission requests.

> [!IMPORTANT]
> Customizing the `WebView` to approve permission requests, using this approach, requires Android API 26 or later.

The permission requests from a web page to the `WebView` control are different than permission requests from the .NET MAUI app to the user. .NET MAUI app permissions are requested and approved by the user, for the whole app. The `WebView` control is dependent on the apps ability to access the hardware. To illustrate this concept, consider a web page that requests access to the device's camera. Even if that request is approved by the `WebView` control, yet the .NET MAUI app didn't have approval by the user to access the camera, the web page wouldn't be able to access the camera.

The following steps demonstrate how to intercept permission requests from the `WebView` control to use the camera. If you are trying to use the microphone, the steps would be similar except that you would use microphone-related permissions instead of camera-related permissions.

01. First, add the required app permissions to the Android manifest. Open the _Platforms/Android/AndroidManifest.xml_ file and add the following in the `manifest` node:

    ```xml
    <uses-permission android:name="android.permission.CAMERA" />
    ```

01. At some point in your app, such as when the page containing a `WebView` control is loaded, request permission from the user to allow the app access to the camera.

    ```csharp
    private async Task RequestCameraPermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Camera>();

        if (status != PermissionStatus.Granted)
            await Permissions.RequestAsync<Permissions.Camera>();
    }
    ```

01. Add the following class to the _Platforms/Android_ folder, changing the root namespace to match your project's namespace:

    ```csharp
    using Android.Webkit;
    using Microsoft.Maui.Handlers;
    using Microsoft.Maui.Platform;

    namespace MauiAppWebViewHandlers.Platforms.Android;

    internal class MyWebChromeClient: MauiWebChromeClient
    {
        public MyWebChromeClient(IWebViewHandler handler) : base(handler)
        {

        }

        public override void OnPermissionRequest(PermissionRequest request)
        {
            // Process each request
            foreach (var resource in request.GetResources())
            {
                // Check if the web page is requesting permission to the camera
                if (resource.Equals(PermissionRequest.ResourceVideoCapture, StringComparison.OrdinalIgnoreCase))
                {
                    // Get the status of the .NET MAUI app's access to the camera
                    PermissionStatus status = Permissions.CheckStatusAsync<Permissions.Camera>().Result;

                    // Deny the web page's request if the app's access to the camera is not "Granted"
                    if (status != PermissionStatus.Granted)
                        request.Deny();
                    else
                        request.Grant(request.GetResources());

                    return;
                }
            }

            base.OnPermissionRequest(request);
        }
    }
    ```

    In the previous snippet, the `MyWebChromeClient` class inherits from `MauiWebChromeClient`, and overrides the `OnPermissionRequest` method to intercept web page permission requests. Each permission item is checked to see if it matches the `PermissionRequest.ResourceVideoCapture` string constant, which represents the camera. If a camera permission is matched, the code checks to see if the app has permission to use the camera. If it has permission, the web page's request is granted.

01. Use the <xref:Android.Webkit.WebView.SetWebChromeClient%2A> method on the Android's `WebView` control to set the chrome client to `MyWebChromeClient`. The following two items demonstrate how you can set the chrome client:

    - Given a .NET MAUI `WebView` control named `theWebViewControl`, you can set the chrome client directly on the platform view, which is the Android control:

      ```csharp
      ((IWebViewHandler)theWebViewControl.Handler).PlatformView.SetWebChromeClient(new MyWebChromeClient((IWebViewHandler)theWebViewControl.Handler));
      ```

    - You can also use handler property mapping to force all `WebView` controls to use your chrome client. For more information, see [Handlers](../handlers/index.md).

      The following snippet's `CustomizeWebViewHandler` method should be called when the app starts, such as in the `MauiProgram.CreateMauiApp` method.

      ```csharp
      private static void CustomizeWebViewHandler()
      {
      #if ANDROID26_0_OR_GREATER
          Microsoft.Maui.Handlers.WebViewHandler.Mapper.ModifyMapping(
              nameof(Android.Webkit.WebView.WebChromeClient),
              (handler, view, args) => handler.PlatformView.SetWebChromeClient(new MyWebChromeClient(handler)));
      #endif
      }
      ```

:::zone-end

## Set cookies

Cookies can be set on a <xref:Microsoft.Maui.Controls.WebView> so that they are sent with the web request to the specified URL. Set the cookies by adding `Cookie` objects to a `CookieContainer`, and then set the container as the value of the `WebView.Cookies` bindable property. The following code shows an example:

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

In this example, a single `Cookie` is added to the `CookieContainer` object, which is then set as the value of the `WebView.Cookies` property. When the  <xref:Microsoft.Maui.Controls.WebView> sends a web request to the specified URL, the cookie is sent with the request.

## Invoke JavaScript

<xref:Microsoft.Maui.Controls.WebView> includes the ability to invoke a JavaScript function from C# and return any result to the calling C# code. This interop is accomplished with the `EvaluateJavaScriptAsync` method, which is shown in the following example:

```csharp
Entry numberEntry = new Entry { Text = "5" };
Label resultLabel = new Label();
WebView webView = new WebView();
...

int number = int.Parse(numberEntry.Text);
string result = await webView.EvaluateJavaScriptAsync($"factorial({number})");
resultLabel.Text = $"Factorial of {number} is {result}.";
```

The `WebView.EvaluateJavaScriptAsync` method evaluates the JavaScript that's specified as the argument, and returns any result as a `string`. In this example, the `factorial` JavaScript function is invoked, which returns the factorial of `number` as a result. This JavaScript function is defined in the local HTML file that the <xref:Microsoft.Maui.Controls.WebView> loads, and is shown in the following example:

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

:::zone pivot="devices-ios, devices-maccatalyst"

## Configure the native WebView on iOS and Mac Catalyst

The native <xref:Microsoft.Maui.Controls.WebView> control is a `MauiWKWebView` on iOS and Mac Catalyst, which derives from `WKWebView`. One of the `MauiWKWebView` constructor overloads enables a `WKWebViewConfiguration` object to be specified, which provides information about how to configure the `WKWebView` object. Typical configurations include setting the user agent, specifying cookies to make available to your web content, and injecting custom scripts into your web content.

You can create a `WKWebViewConfiguration` object in your app, and then configure its properties as required. Alternatively, you can call the static `MauiWKWebView.CreateConfiguration` method to retrieve .NET MAUI's `WKWebViewConfiguration` object and then modify it. The `WKWebViewConfiguration` object can then be specified as an argument to the `MauiWKWebView` constructor overload.

Since the configuration of the native <xref:Microsoft.Maui.Controls.WebView> can't be changed on iOS and Mac Catalyst once the handler's platform view is created, you should create a custom handler factory delegate to modify it:

```csharp
#if IOS || MACCATALYST
using WebKit;
using CoreGraphics;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Handlers;
#endif
...

#if IOS || MACCATALYST
    Microsoft.Maui.Handlers.WebViewHandler.PlatformViewFactory = (handler) =>
    {
        WKWebViewConfiguration config = MauiWKWebView.CreateConfiguration();
        config.ApplicationNameForUserAgent = "MyProduct/1.0.0";
        return new MauiWKWebView(CGRect.Empty, (WebViewHandler)handler, config);
    };
#endif
```

> [!NOTE]
> You should configure `MauiWKWebView` with a `WKWebViewConfiguration` object before a <xref:Microsoft.Maui.Controls.WebView> is displayed in your app. Suitable locations to do this are in your app's startup path, such as in *MauiProgram.cs* or *App.xaml.cs*. <!-- For more information about configuring a native .NET MAUI control, see [Customize controls](~/user-interface/handlers/customize.md). -->

:::zone-end

:::zone pivot="devices-ios, devices-maccatalyst"

## Set media playback preferences on iOS and Mac Catalyst

Inline media playback of HTML5 video, including autoplay and picture in picture, is enabled by default for the <xref:Microsoft.Maui.Controls.WebView> on iOS and Mac Catalyst. To change this default, or set other media playback preferences, you should create a custom handler factory delegate since media playback preferences can't be changed once the handler's platform view is created. The following code shows an example of doing this:

```csharp
#if IOS || MACCATALYST
using WebKit;
using CoreGraphics;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Handlers;
#endif
...

#if IOS || MACCATALYST
    Microsoft.Maui.Handlers.WebViewHandler.PlatformViewFactory = (handler) =>
    {
        WKWebViewConfiguration config = MauiWKWebView.CreateConfiguration();

        // True to play HTML5 videos inliine, false to use the native full-screen controller.
        config.AllowsInlineMediaPlayback = false;

        // True to play videos over AirPlay, otherwise false.
        config.AllowsAirPlayForMediaPlayback = false;

        // True to let HTML5 videos play Picture in Picture.
        config.AllowsPictureInPictureMediaPlayback = false;

        // Media types that require a user gesture to begin playing.
        config.MediaTypesRequiringUserActionForPlayback = WKAudiovisualMediaTypes.All;

        return new MauiWKWebView(CGRect.Empty, (WebViewHandler)handler, config);
    };
#endif
```

For more information about configuring a <xref:Microsoft.Maui.Controls.WebView> on iOS, see [Configure the native WebView on iOS and Mac Catalyst](#configure-the-native-webview-on-ios-and-mac-catalyst).

:::zone-end

:::zone pivot="devices-maccatalyst"

## Inspect a WebView on Mac Catalyst

To use Safari developer tools to inspect the content of a <xref:Microsoft.Maui.Controls.WebView> on Mac Catalyst, add the following code to your app:

```csharp
#if MACCATALYST
        Microsoft.Maui.Handlers.WebViewHandler.Mapper.AppendToMapping("Inspect", (handler, view) =>
        {
            if (OperatingSystem.IsMacCatalystVersionAtLeast(16, 6))
                handler.PlatformView.Inspectable = true;
        });
#endif
```

This code customizes the property mapper for the `WebViewHandler` on Mac Catalyst, to make <xref:Microsoft.Maui.Controls.WebView> content inspectable by Safari developer tools. For more information about handlers, see [Handlers](~/user-interface/handlers/index.md).

To use Safari developer tools with a Mac Catalyst app:

1. Open Safari on your Mac.
1. In Safari, select the **Safari > Settings > Advanced > Show Develop menu in menu bar** checkbox.
1. Run your .NET MAUI Mac Catalyst app.
1. In Safari, select the **Develop > {Device name}** menu, where the `{Device name}` placeholder is your device name such as `Macbook Pro`. Then select the entry under your app name, which will also highlight your running app. This will cause the **Web inspector** window to appear.

:::zone-end

## Launch the system browser

It's possible to open a URI in the system web browser with the `Launcher` class, which is provided by `Microsoft.Maui.Essentials`. Call the launcher's `OpenAsync` method and pass in a `string` or `Uri` argument that represents the URI to open:

```csharp
await Launcher.OpenAsync("https://learn.microsoft.com/dotnet/maui");
```

For more information, see [Launcher](~/platform-integration/appmodel/launcher.md).
