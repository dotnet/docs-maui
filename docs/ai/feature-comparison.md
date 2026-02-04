---
title: "AI feature comparison by platform"
description: "Compare AI capabilities across iOS, macOS, Android, and Windows in Microsoft.Maui.Essentials.AI including chat, embeddings, streaming, and tool calling."
ms.date: 02/02/2026
---

# AI feature comparison by platform

This article provides a detailed comparison of AI features available on each platform through Microsoft.Maui.Essentials.AI.

## Feature matrix

| Feature | iOS | macOS | Mac Catalyst | tvOS | Android | Windows |
|---------|-----|-------|--------------|------|---------|---------|
| **Chat / Text Generation** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Streaming responses** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| **System prompts** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Tool / Function calling** | ✅ | ✅ | ✅ | ✅ | ❌ | ❌ |
| **JSON structured output** | ✅¹ | ✅¹ | ✅¹ | ✅¹ | ❌ | ❌ |
| **Text embeddings** | ✅ | ✅ | ✅ | ✅ | ❌ | ✅ |
| **Image input** | ❌ | ❌ | ❌ | ❌ | ✅² | ❌ |
| **Vision / Image analysis** | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| **Speech-to-text** | ❌³ | ❌³ | ❌³ | ❌³ | ❌³ | ❌³ |
| **Text-to-speech** | ❌³ | ❌³ | ❌³ | ❌³ | ❌³ | ❌³ |

**Legend**: ✅ Supported | ❌ Not available

¹ JSON structured output on Apple platforms **requires** a JSON schema. Schemaless JSON mode is not supported.

² Android supports single image input per request via `DataContent`.

³ Speech capabilities are available through other .NET MAUI APIs (see [Speech](/dotnet/maui/platform-integration/communication/text-to-speech)).

## Detailed feature comparison

### Chat and text generation

Chat and text generation enables conversational AI interactions using natural language.

| Capability | Apple | Android | Windows |
|------------|-------|---------|---------|
| Single-turn chat | ✅ | ✅ | ✅ |
| Multi-turn conversations | ✅ | ✅ | ✅ |
| System prompts | ✅ | ✅ | ✅ |
| Temperature control | ✅ | ✅ | ✅ |
| Max tokens limit | ✅ | ❌ | ❌ |
| Top-K sampling | ✅ | ✅ | ✅ |
| Top-P sampling | ❌ | ❌ | ✅ |
| Reproducible output (seed) | ✅ | ✅ | ❌ |

**Platform implementations**:
- **Apple**: Uses [Foundation Models](https://developer.apple.com/documentation/foundationmodels) framework with on-device LLM
- **Android**: Uses [ML Kit GenAI](https://developers.google.com/ml-kit/genai) with Gemini Nano
- **Windows**: Uses [Windows Copilot Runtime](/windows/ai/apis/phi-silica) with Phi Silica

### Streaming responses

Streaming allows receiving partial responses as the model generates them, enabling real-time UI updates.

| Capability | Apple | Android | Windows |
|------------|-------|---------|---------|
| Token-by-token streaming | ✅ | ✅ | ✅ |
| Async enumerable (`IAsyncEnumerable`) | ✅ | ✅ | ✅ |
| Cancellation support | ✅ | ✅ | ✅ |

All platforms implement `GetStreamingResponseAsync()` returning `IAsyncEnumerable<ChatResponseUpdate>` for incremental content delivery.

### Tool and function calling

Function calling allows the AI model to invoke application-defined functions to extend its capabilities.

| Capability | Apple | Android | Windows |
|------------|-------|---------|---------|
| Define custom tools | ✅ | ❌ | ❌ |
| Tool invocation | ✅ | ❌ | ❌ |
| Multiple tools per request | ✅ | ❌ | ❌ |
| Automatic tool execution | ✅ | ❌ | ❌ |

**Apple implementation**: Uses native `AIToolNative` protocol bridged to `Microsoft.Extensions.AI.AIFunction`.
> [!NOTE]
> Tool calling is currently only available on Apple platforms. Android (ML Kit GenAI) and Windows (Phi Silica) do not support function calling.
Example:

```csharp
var weatherTool = AIFunctionFactory.Create(
    (string location) => $"Sunny, 72°F in {location}",
    name: "GetWeather",
    description: "Gets current weather for a location");

var options = new ChatOptions { Tools = [weatherTool] };
var response = await chatClient.GetResponseAsync("What's the weather in Seattle?", options);
```

### JSON structured output

Structured output ensures the model returns valid JSON conforming to a specified schema.

| Capability | Apple | Android | Windows |
|------------|-------|---------|---------|
| JSON output mode | ✅¹ | ❌ | ❌ |
| Schema enforcement | ✅ | ❌ | ❌ |
| Type-safe deserialization | ✅ | ❌ | ❌ |

¹ Requires JSON schema; schemaless JSON mode is not supported.

> [!IMPORTANT]
> Apple Intelligence differs from OpenAI and other cloud providers by **requiring** a JSON schema for structured output. You must use `ChatResponseFormat.ForJsonSchema<T>(jsonSerializerOptions)` rather than `ChatResponseFormat.Json`.

**Apple implementation example**:

```csharp
public record WeatherInfo(string Location, int Temperature, string Condition);

var options = new ChatOptions
{
    ResponseFormat = ChatResponseFormat.ForJsonSchema<WeatherInfo>(
        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
};

var response = await chatClient.GetResponseAsync(
    "What's the weather in Paris?", options);
var weather = JsonSerializer.Deserialize<WeatherInfo>(response.Text);
```

### Text embeddings

Embeddings convert text into numerical vectors for semantic similarity, search, and retrieval-augmented generation (RAG).

| Capability | Apple | Android | Windows |
|------------|-------|---------|---------|
| Sentence embeddings | ✅ | ❌ | ✅ |
| Multi-language support | ✅ | ❌ | ❌ |
| Batch generation | ✅ | ❌ | ✅ |
| Cosine similarity search | ✅ | ❌ | ✅ |

**Platform implementations**:
- **Apple**: Uses [NLEmbedding](https://developer.apple.com/documentation/naturallanguage/nlembedding) from the Natural Language framework. Available on iOS 13+ (independent of Apple Intelligence).
- **Windows**: Uses `LanguageModel.GenerateEmbeddingVectors()` from Windows Copilot Runtime.

> [!NOTE]
> Android (ML Kit GenAI) does not currently provide an embedding API. Consider using a cloud-based embedding service if you need embeddings on Android.

### Vision and image input

Image input allows the model to analyze images along with text prompts.

| Capability | Apple | Android | Windows |
|------------|-------|---------|----------|
| Image input with prompt | ❌ | ✅¹ | ❌ |
| Image description | ❌ | ❌ | ❌ |
| Object detection | ❌ | ❌ | ❌ |
| OCR / Text extraction | ❌ | ❌ | ❌ |

¹ Android supports one image per request via `DataContent`.

For dedicated vision capabilities, consider using:
- [Apple Vision framework](https://developer.apple.com/documentation/vision) directly
- [ML Kit Vision APIs](https://developers.google.com/ml-kit/vision) for Android
- [Windows Vision Skills](/windows/ai/windows-vision-skills/) for Windows

## Privacy and data handling

All implementations process data **on-device**:

| Aspect | Apple | Android | Windows |
|--------|-------|---------|----------|
| Data leaves device | ❌ No | ❌ No | ❌ No |
| Internet required | ❌ No | ❌ No | ❌ No |
| Model location | On-device | On-device | On-device |
| User data stored | ❌ No | ❌ No | ❌ No |

On-device processing ensures:
- **Privacy**: User data never leaves the device
- **Offline support**: AI works without network connectivity
- **Low latency**: No network round-trips
- **No usage costs**: No API calls to cloud services

## Performance characteristics

| Metric | Apple | Android | Windows |
|--------|-------|---------|----------|
| First token latency | ~100-500ms | Device dependent | Device dependent |
| Hardware acceleration | Neural Engine | NPU/GPU | NPU |
| Memory usage | Model dependent | Model dependent | Model dependent |

Performance varies based on:
- Device hardware (Neural Engine, NPU capabilities, RAM)
- Model complexity
- Input/output length
- Concurrent system load

**Hardware accelerators by platform:**
- **Apple**: Apple Neural Engine (ANE) on A/M-series chips
- **Android**: Device NPU or GPU depending on SoC
- **Windows**: NPU on Copilot+ PCs (Qualcomm, Intel, AMD)

## Cloud fallback options

For features not available on a particular platform, consider these cloud alternatives:

| Feature | Cloud alternative |
|---------|-------------------|
| Tool/function calling (Android, Windows) | Azure OpenAI, OpenAI |
| JSON structured output (Android, Windows) | Azure OpenAI, OpenAI |
| Embeddings (Android) | Azure OpenAI Embeddings, OpenAI |
| Vision | Azure AI Vision, Google Cloud Vision |

See [Cross-platform pattern](platform-apis.md#cross-platform-pattern) for implementation details.

## See also

- [Platform APIs](platform-apis.md)
- [Requirements](requirements.md)
- [Getting started](getting-started.md)
