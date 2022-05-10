---
title: "Text-to-Speech"
description: "Learn how to use the .NET MAUI TextToSpeech class, which enables an application utilize the built in text-to-speech engines to speak back text from the device."
ms.date: 09/16/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Text-to-Speech

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `TextToSpeech` class. This class enables an application to utilize the built-in text-to-speech engines to speak back text from the device. You can also use it to query for available languages.

## Get started

[!INCLUDE [get-started](../includes/get-started.md)]

[!INCLUDE [essentials-namespace](../includes/essentials-namespace.md)]

## Using Text-to-Speech

Text-to-speech works by calling the `SpeakAsync` method with the text to speak.

```csharp
public async Task SpeakNowDefaultSettings()
{
    await TextToSpeech.SpeakAsync("Hello World");

    // This method will block until utterance finishes.
}

public void SpeakNowDefaultSettingsContinue()
{
    TextToSpeech.SpeakAsync("Hello World").ContinueWith((t) =>
    {
        // Logic that will run after utterance finishes.

    }, TaskScheduler.FromCurrentSynchronizationContext());
}
```

This method takes in an optional `CancellationToken` to stop the utterance once it starts.

<!-- TODO: The note below about an async call blocking seems to contradict the speak SpeakMultiple idea of queueing multiple. How can it queue if it blocks? -->

```csharp
CancellationTokenSource cts;
public async Task SpeakNowDefaultSettings()
{
    cts = new CancellationTokenSource();
    await TextToSpeech.SpeakAsync("Hello World", cancelToken: cts.Token);

    // This method will block until utterance finishes.
}

// Cancel speech if a cancellation token exists & hasn't been already requested.
public void CancelSpeech()
{
    if (cts?.IsCancellationRequested ?? true)
        return;

    cts.Cancel();
}
```

Text-to-Speech will automatically queue speech requests from the same thread.

```csharp
bool isBusy = false;
public void SpeakMultiple()
{
    isBusy = true;
    Task.Run(async () =>
    {
        await TextToSpeech.SpeakAsync("Hello World 1");
        await TextToSpeech.SpeakAsync("Hello World 2");
        await TextToSpeech.SpeakAsync("Hello World 3");
        isBusy = false;
    });

    // or you can query multiple without a Task:
    Task.WhenAll(
        TextToSpeech.SpeakAsync("Hello World 1"),
        TextToSpeech.SpeakAsync("Hello World 2"),
        TextToSpeech.SpeakAsync("Hello World 3"))
        .ContinueWith((t) => { isBusy = false; }, TaskScheduler.FromCurrentSynchronizationContext());
}
```

## Settings

To control the volume, pitch, and locale of the voice, use the `SpeechOptions` class. Pass an instance of that class to the `SpeakAsync` method:

```csharp
public async Task SpeakNow()
{
    var settings = new SpeechOptions()
        {
            Volume = .75f,
            Pitch = 1.0f
        };

    await TextToSpeech.SpeakAsync("Hello World", settings);
}
```

The following are supported values for these parameters:

| Parameter | Minimum | Maximum |
|-----------|:-------:|:-------:|
| `Pitch`     | 0       | 2.0     |
| `Volume`    | 0       | 1.0     |

### Speech locales

Each platform supports different locales, to speak back text in different languages and accents. Platforms have different codes and ways of specifying the locale. .NET MAUI helps in this in this regard with the `Locale` class. Use the `GetLocalesAsync` method to get which locales are available:

```csharp
public async Task SpeakNow()
{
    var locales = await TextToSpeech.GetLocalesAsync();

    // Grab the first locale
    var locale = locales.FirstOrDefault();

    var settings = new SpeechOptions()
        {
            Volume = .75f,
            Pitch = 1.0f,
            Locale = locale
        };

    await TextToSpeech.SpeakAsync("Hello World", settings);
}
```

## Limitations

- Utterance queueing is not guaranteed if called across multiple threads.
- Background audio playback is not officially supported.
