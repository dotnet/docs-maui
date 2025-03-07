---
title: "Text-to-Speech"
description: "Learn how to use the .NET MAUI ITextToSpeech interface, which enables an application utilize the built-in text-to-speech engines to speak back text from the device."
ms.date: 02/02/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Media", "TextToSpeech"]
---

# Text-to-Speech

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Media.ITextToSpeech> interface. This interface enables an application to utilize the built-in text-to-speech engines to speak back text from the device. You can also use it to query for available languages.

The default implementation of the `ITextToSpeech` interface is available through the <xref:Microsoft.Maui.Media.TextToSpeech.Default?displayProperty=nameWithType> property. Both the `ITextToSpeech` interface and `TextToSpeech` class are contained in the `Microsoft.Maui.Media` namespace.

## Get started

To access text-to-speech functionality, the following platform-specific setup is required.

# [Android](#tab/android)

If your project's Target Android version is set to **Android 11 (R API 30)** or higher, you must update your _Android Manifest_ with an intent filter for the text-to-speech (TTS) engine. For more information about intents, see Android's documentation on [Intents and Intent Filters](https://developer.android.com/guide/components/intents-filters).

In the _Platforms/Android/AndroidManifest.xml_ file, add the following `queries/intent` nodes to the `manifest` node:

```xml
<queries>
  <intent>
    <action android:name="android.intent.action.TTS_SERVICE" />
  </intent>
</queries>
```

# [iOS/Mac Catalyst](#tab/macios)

No setup is required.

# [Windows](#tab/windows)

No setup is required.

-----

## Using Text-to-Speech

Text-to-speech works by calling the <xref:Microsoft.Maui.Media.ITextToSpeech.SpeakAsync%2A> method with the text to speak, as the following code example demonstrates:

:::code language="csharp" source="../snippets/shared_1/MediaPage.cs" id="speak":::

This method takes in an optional `CancellationToken` to stop the utterance once it starts.

:::code language="csharp" source="../snippets/shared_1/MediaPage.cs" id="speak_cancel":::

Text-to-Speech will automatically queue speech requests from the same thread.

:::code language="csharp" source="../snippets/shared_1/MediaPage.cs" id="speak_queue":::

## Settings

::: moniker range=">=net-maui-8.0 <=net-maui-9.0"

To control the volume, pitch, and locale of the voice, use the <xref:Microsoft.Maui.Media.SpeechOptions> class. Pass an instance of that class to the <xref:Microsoft.Maui.Media.ITextToSpeech.SpeakAsync(System.String,Microsoft.Maui.Media.SpeechOptions,System.Threading.CancellationToken)> method. The <xref:Microsoft.Maui.Media.ITextToSpeech.GetLocalesAsync> method retrieves a collection of the locales provided by the operating system.

:::code language="csharp" source="../snippets/shared_1/MediaPage.cs" id="speak_options_old":::

The following are supported values for these parameters:

| Parameter | Minimum | Maximum |
|-----------|:-------:|:-------:|
| `Pitch`   | 0       | 2.0     |
| `Volume`  | 0       | 1.0     |

::: moniker-end

::: moniker range=">=net-maui-10.0"

To control the volume, pitch, rate, and locale of the voice, use the <xref:Microsoft.Maui.Media.SpeechOptions> class. Pass an instance of that class to the <xref:Microsoft.Maui.Media.ITextToSpeech.SpeakAsync(System.String,Microsoft.Maui.Media.SpeechOptions,System.Threading.CancellationToken)> method. The <xref:Microsoft.Maui.Media.ITextToSpeech.GetLocalesAsync> method retrieves a collection of the locales provided by the operating system.

:::code language="csharp" source="../snippets/shared_1/MediaPage.cs" id="speak_options":::

The following are supported values for these parameters:

| Parameter | Minimum | Maximum |
|-----------|:-------:|:-------:|
| `Pitch`   | 0       | 2.0     |
| `Volume`  | 0       | 1.0     |
| `Rate`    | 0.1     | 2.0     |

::: moniker-end

## Limitations

- Utterance queueing isn't guaranteed if called across multiple threads.
- Background audio playback isn't officially supported.
