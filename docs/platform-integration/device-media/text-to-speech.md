---
title: "Text-to-Speech"
description: "Learn how to use the .NET MAUI ITextToSpeech interface, which enables an application utilize the built-in text-to-speech engines to speak back text from the device."
ms.date: 09/02/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Media", "TextToSpeech"]
---

# Text-to-Speech

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `ITextToSpeech` interface. This interface enables an application to utilize the built-in text-to-speech engines to speak back text from the device. You can also use it to query for available languages.

The default implementation of the `ITextToSpeech` interface is available through the `TextToSpeech.Default` property. Both the `ITextToSpeech` interface and `TextToSpeech` class are contained in the `Microsoft.Maui.Media` namespace.

## Using Text-to-Speech

Text-to-speech works by calling the `SpeakAsync` method with the text to speak, as the following code example demonstrates:

:::code language="csharp" source="../snippets/shared_1/MediaPage.cs" id="speak":::

This method takes in an optional `CancellationToken` to stop the utterance once it starts.

:::code language="csharp" source="../snippets/shared_1/MediaPage.cs" id="speak_cancel":::

Text-to-Speech will automatically queue speech requests from the same thread.

:::code language="csharp" source="../snippets/shared_1/MediaPage.cs" id="speak_queue":::

## Settings

To control the volume, pitch, and locale of the voice, use the `SpeechOptions` class. Pass an instance of that class to the `SpeakAsync` method. the `GetLocalesAsync` method retrieves a collection of the locales provided by the operating system.

:::code language="csharp" source="../snippets/shared_1/MediaPage.cs" id="speak_options":::

The following are supported values for these parameters:

| Parameter | Minimum | Maximum |
|-----------|:-------:|:-------:|
| `Pitch`   | 0       | 2.0     |
| `Volume`  | 0       | 1.0     |

## Limitations

- Utterance queueing isn't guaranteed if called across multiple threads.
- Background audio playback isn't officially supported.
