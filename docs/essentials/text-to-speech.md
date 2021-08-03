---
title: "Xamarin.Essentials: Text-to-Speech"
description: "The TextToSpeech class in Xamarin.Essentials enables an application utilize the built in text-to-speech engines to speak back text from the device and also to query available languages that the engine can support."
author: jamesmontemagno
ms.custom: video
ms.author: jamont
ms.date: 11/04/2018
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Text-to-Speech

The **TextToSpeech** class enables an application to utilize the built-in text-to-speech engines to speak back text from the device and also to query available languages that the engine can support.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using Text-to-Speech

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

Text-to-Speech works by calling the `SpeakAsync` method with text and optional parameters, and returns after the utterance has finished.

```csharp
public async Task SpeakNowDefaultSettings()
{
    await TextToSpeech.SpeakAsync("Hello World");

    // This method will block until utterance finishes.
}

public void SpeakNowDefaultSettings2()
{
    TextToSpeech.SpeakAsync("Hello World").ContinueWith((t) =>
    {
        // Logic that will run after utterance finishes.

    }, TaskScheduler.FromCurrentSynchronizationContext());
}
```

This method takes in an optional `CancellationToken` to stop the utterance once it starts.

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

### Speech Settings

For more control over how the audio is spoken back with `SpeechOptions` that allows setting the volume, pitch, and locale.

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
| --- | :---: | :---: |
| Pitch | 0 | 2.0 |
| Volume | 0 | 1.0 |

### Speech Locales

Each platform supports different locales, to speak back text in different languages and accents. Platforms have different codes and ways of specifying the locale, which is why Xamarin.Essentials provides a cross-platform `Locale` class and a way to query them with `GetLocalesAsync`.

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

- Utterance queue is not guaranteed if called across multiple threads.
- Background audio playback is not officially supported.

## API

- [TextToSpeech source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/TextToSpeech)
- [TextToSpeech API documentation](xref:Xamarin.Essentials.TextToSpeech)

## Related Video

> [!Video https://channel9.msdn.com/Shows/XamarinShow/Text-to-Speech-XamarinEssentials-API-of-the-Week/player]

[!INCLUDE [xamarin-show-essentials](includes/xamarin-show-essentials.md)]
