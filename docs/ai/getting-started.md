---
title: "Get started with MAUI AI"
description: "Learn how to set up and use Microsoft.Maui.Essentials.AI for on-device AI capabilities including chat, embeddings, and tool calling in .NET MAUI apps."
ms.date: 02/02/2026
---

# Get started with MAUI AI

This article walks you through setting up Microsoft.Maui.Essentials.AI and demonstrates common AI scenarios including chat, streaming, tool calling, and embeddings.

## Prerequisites

Before you begin, ensure you have:

- .NET 9.0 SDK or later
- .NET MAUI workload installed
- For Apple platforms: Xcode 16+ with iOS 26 / macOS 26 SDK
- For Android: Google Play Services with ML Kit GenAI support
- For Windows: Windows 11 24H2 (build 26100) or later
- A compatible device (see [Requirements](requirements.md))

## Installation

Add the NuGet package to your .NET MAUI project:

# [.NET CLI](#tab/cli)

```shell
dotnet add package Microsoft.Maui.Essentials.AI
```

# [Package Manager](#tab/pm)

```powershell
Install-Package Microsoft.Maui.Essentials.AI
```

# [PackageReference](#tab/xml)

```xml
<PackageReference Include="Microsoft.Maui.Essentials.AI" Version="*" />
```

---

## Register AI services

Configure AI services in your `MauiProgram.cs` using dependency injection:

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Essentials.AI;

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
            });

        // Configure AI services
        ConfigureAIServices(builder.Services);

        return builder.Build();
    }

    private static void ConfigureAIServices(IServiceCollection services)
    {
#if IOS || MACCATALYST
        // Register Apple Intelligence chat client
        services.AddSingleton<AppleIntelligenceChatClient>();
        services.AddSingleton<IChatClient>(sp =>
        {
            var client = sp.GetRequiredService<AppleIntelligenceChatClient>();
            return client
                .AsBuilder()
                .UseLogging(sp.GetRequiredService<ILoggerFactory>())
                .Build();
        });

        // Register NL embedding generator
        services.AddSingleton<NLEmbeddingGenerator>();
        services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(sp =>
        {
            var generator = sp.GetRequiredService<NLEmbeddingGenerator>();
            return generator
                .AsBuilder()
                .UseLogging(sp.GetRequiredService<ILoggerFactory>())
                .Build();
        });
#elif ANDROID
        // Register Gemini Nano chat client (ML Kit GenAI)
        services.AddSingleton<GeminiNanoChatClient>();
        services.AddSingleton<IChatClient>(sp =>
        {
            var client = sp.GetRequiredService<GeminiNanoChatClient>();
            return client
                .AsBuilder()
                .UseLogging(sp.GetRequiredService<ILoggerFactory>())
                .Build();
        });
        // Note: Embeddings not available natively on Android
#elif WINDOWS
        // Register Phi Silica chat client (Windows Copilot Runtime)
        services.AddSingleton<PhiSilicaChatClient>();
        services.AddSingleton<IChatClient>(sp =>
        {
            var client = sp.GetRequiredService<PhiSilicaChatClient>();
            return client
                .AsBuilder()
                .UseLogging(sp.GetRequiredService<ILoggerFactory>())
                .Build();
        });

        // Register Phi Silica embedding generator
        services.AddSingleton<PhiSilicaEmbeddingGenerator>();
        services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(sp =>
        {
            var generator = sp.GetRequiredService<PhiSilicaEmbeddingGenerator>();
            return generator
                .AsBuilder()
                .UseLogging(sp.GetRequiredService<ILoggerFactory>())
                .Build();
        });
#endif
    }
}
```

## Basic chat

Inject `IChatClient` and use it for simple text generation:

```csharp
using Microsoft.Extensions.AI;

public class ChatViewModel
{
    private readonly IChatClient _chatClient;

    public ChatViewModel(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<string> AskQuestionAsync(string question)
    {
        var response = await _chatClient.GetResponseAsync(question);
        return response.Text;
    }
}
```

### Multi-turn conversations

Maintain conversation history for contextual responses:

```csharp
public class ConversationService
{
    private readonly IChatClient _chatClient;
    private readonly List<ChatMessage> _messages = new();

    public ConversationService(IChatClient chatClient)
    {
        _chatClient = chatClient;
        
        // Optional: Set a system prompt
        _messages.Add(new ChatMessage(ChatRole.System, 
            "You are a helpful assistant for a travel planning app."));
    }

    public async Task<string> SendMessageAsync(string userMessage)
    {
        // Add user message to history
        _messages.Add(new ChatMessage(ChatRole.User, userMessage));

        // Get response
        var response = await _chatClient.GetResponseAsync(_messages);

        // Add assistant response to history
        _messages.Add(new ChatMessage(ChatRole.Assistant, response.Text));

        return response.Text;
    }

    public void ClearHistory()
    {
        _messages.Clear();
    }
}
```

## Streaming responses

For real-time UI updates as the model generates text:

```csharp
public class StreamingChatService
{
    private readonly IChatClient _chatClient;

    public StreamingChatService(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async IAsyncEnumerable<string> StreamResponseAsync(
        string prompt, 
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var update in _chatClient.GetStreamingResponseAsync(prompt, cancellationToken: cancellationToken))
        {
            if (update.Text is not null)
            {
                yield return update.Text;
            }
        }
    }
}
```

### Using streaming in a ViewModel

```csharp
public class ChatPageViewModel : ObservableObject
{
    private readonly IChatClient _chatClient;
    
    [ObservableProperty]
    private string _responseText = string.Empty;
    
    [ObservableProperty]
    private bool _isGenerating;

    public ChatPageViewModel(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    [RelayCommand]
    private async Task SendMessageAsync(string message, CancellationToken cancellationToken)
    {
        IsGenerating = true;
        ResponseText = string.Empty;

        try
        {
            await foreach (var chunk in _chatClient.GetStreamingResponseAsync(message, cancellationToken: cancellationToken))
            {
                ResponseText += chunk.Text;
            }
        }
        finally
        {
            IsGenerating = false;
        }
    }
}
```

## Tool / Function calling

Enable the AI to invoke your application's functions:

```csharp
using Microsoft.Extensions.AI;

public class WeatherAssistant
{
    private readonly IChatClient _chatClient;

    public WeatherAssistant(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<string> GetWeatherResponseAsync(string userQuery)
    {
        // Define tools the AI can use
        var tools = new[]
        {
            AIFunctionFactory.Create(GetCurrentWeather, "GetCurrentWeather", 
                "Gets the current weather for a specified location"),
            AIFunctionFactory.Create(GetForecast, "GetForecast",
                "Gets the weather forecast for a specified location and number of days")
        };

        var options = new ChatOptions { Tools = tools };

        // The AI will automatically call these functions as needed
        var response = await _chatClient.GetResponseAsync(userQuery, options);
        return response.Text;
    }

    private static string GetCurrentWeather(string location)
    {
        // In a real app, call a weather API
        return $"Currently 72°F and sunny in {location}";
    }

    private static string GetForecast(string location, int days)
    {
        // In a real app, call a weather API
        return $"{days}-day forecast for {location}: Sunny with highs in the 70s";
    }
}
```

### Tools with complex parameters

```csharp
public record SearchParameters(string Query, int MaxResults, string[] Categories);

public class SearchAssistant
{
    private readonly IChatClient _chatClient;

    public SearchAssistant(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<string> SearchAsync(string userRequest)
    {
        var searchTool = AIFunctionFactory.Create(
            PerformSearch,
            "PerformSearch",
            "Searches the catalog based on user criteria");

        var options = new ChatOptions { Tools = [searchTool] };
        var response = await _chatClient.GetResponseAsync(userRequest, options);
        return response.Text;
    }

    private static string PerformSearch(SearchParameters parameters)
    {
        // Perform actual search logic
        return $"Found {parameters.MaxResults} results for '{parameters.Query}' " +
               $"in categories: {string.Join(", ", parameters.Categories)}";
    }
}
```

## Structured JSON output

Get type-safe responses by specifying a JSON schema:

```csharp
using System.Text.Json;
using Microsoft.Extensions.AI;

public record TravelItinerary(
    string Destination,
    int DurationDays,
    List<ItineraryDay> Days);

public record ItineraryDay(
    int DayNumber,
    string Theme,
    List<Activity> Activities);

public record Activity(
    string Name,
    string Description,
    string Time,
    string Location);

public class ItineraryGenerator
{
    private readonly IChatClient _chatClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ItineraryGenerator(IChatClient chatClient)
    {
        _chatClient = chatClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<TravelItinerary?> GenerateItineraryAsync(
        string destination, 
        int days,
        string preferences)
    {
        var prompt = $"""
            Create a {days}-day travel itinerary for {destination}.
            User preferences: {preferences}
            Include specific activities, times, and locations.
            """;

        var options = new ChatOptions
        {
            // IMPORTANT: Apple Intelligence requires a schema for JSON output
            ResponseFormat = ChatResponseFormat.ForJsonSchema<TravelItinerary>(_jsonOptions)
        };

        var response = await _chatClient.GetResponseAsync(prompt, options);
        
        return JsonSerializer.Deserialize<TravelItinerary>(response.Text, _jsonOptions);
    }
}
```

> [!IMPORTANT]
> Apple Intelligence **requires** a JSON schema when requesting structured output. Using `ChatResponseFormat.Json` without a schema will fail. Always use `ChatResponseFormat.ForJsonSchema<T>(jsonSerializerOptions)`.

## Text embeddings

Generate embeddings for semantic search and RAG (Retrieval Augmented Generation):

```csharp
using Microsoft.Extensions.AI;
using System.Numerics.Tensors;

public class SemanticSearchService
{
    private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;
    private readonly List<(string Text, Embedding<float> Embedding)> _documents = new();

    public SemanticSearchService(IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
    {
        _embeddingGenerator = embeddingGenerator;
    }

    public async Task IndexDocumentAsync(string text)
    {
        var embedding = await _embeddingGenerator.GenerateAsync(text);
        _documents.Add((text, embedding));
    }

    public async Task<IEnumerable<string>> SearchAsync(string query, int maxResults = 5)
    {
        var queryEmbedding = await _embeddingGenerator.GenerateAsync(query);

        return _documents
            .Select(doc => (
                doc.Text,
                Similarity: TensorPrimitives.CosineSimilarity(
                    queryEmbedding.Vector.Span,
                    doc.Embedding.Vector.Span)))
            .OrderByDescending(x => x.Similarity)
            .Take(maxResults)
            .Select(x => x.Text);
    }
}
```

### RAG (Retrieval Augmented Generation)

Combine embeddings with chat for context-aware responses:

```csharp
public class RAGAssistant
{
    private readonly IChatClient _chatClient;
    private readonly SemanticSearchService _searchService;

    public RAGAssistant(IChatClient chatClient, SemanticSearchService searchService)
    {
        _chatClient = chatClient;
        _searchService = searchService;
    }

    public async Task<string> AskWithContextAsync(string question)
    {
        // Find relevant documents
        var relevantDocs = await _searchService.SearchAsync(question, maxResults: 3);
        var context = string.Join("\n\n", relevantDocs);

        // Create prompt with context
        var prompt = $"""
            Use the following context to answer the question. 
            If the answer isn't in the context, say so.

            Context:
            {context}

            Question: {question}
            """;

        var response = await _chatClient.GetResponseAsync(prompt);
        return response.Text;
    }
}
```

## Configure chat options

Customize model behavior with `ChatOptions`:

```csharp
public async Task<string> GenerateCreativeTextAsync(string prompt)
{
    var options = new ChatOptions
    {
        // Higher temperature = more creative/random
        Temperature = 0.9,
        
        // Limit response length
        MaxOutputTokens = 500,
        
        // Top-K sampling
        TopK = 40,
        
        // Reproducible output
        Seed = 42
    };

    var response = await _chatClient.GetResponseAsync(prompt, options);
    return response.Text;
}

public async Task<string> GeneratePreciseTextAsync(string prompt)
{
    var options = new ChatOptions
    {
        // Lower temperature = more deterministic/focused
        Temperature = 0.1,
        MaxOutputTokens = 200
    };

    var response = await _chatClient.GetResponseAsync(prompt, options);
    return response.Text;
}
```

## Error handling

Handle AI-specific errors gracefully:

```csharp
public async Task<string> SafeGenerateAsync(string prompt)
{
    try
    {
        var response = await _chatClient.GetResponseAsync(prompt);
        return response.Text;
    }
    catch (OperationCanceledException)
    {
        return "Request was cancelled.";
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("not available"))
    {
        return "AI features are not available on this device. " +
               "Please ensure Apple Intelligence is enabled in Settings.";
    }
    catch (Exception ex)
    {
        // Log the exception
        Debug.WriteLine($"AI error: {ex}");
        return "Sorry, I couldn't process that request. Please try again.";
    }
}
```

## Cross-platform considerations

Use conditional compilation for platform-specific code:

```csharp
public static class AIServiceExtensions
{
    public static IServiceCollection AddAIServices(this IServiceCollection services)
    {
#if IOS || MACCATALYST
        services.AddSingleton<IChatClient>(sp =>
            new AppleIntelligenceChatClient()
                .AsBuilder()
                .UseLogging(sp.GetRequiredService<ILoggerFactory>())
                .Build());

        services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(sp =>
            new NLEmbeddingGenerator()
                .AsBuilder()
                .UseLogging(sp.GetRequiredService<ILoggerFactory>())
                .Build());
#elif ANDROID
        services.AddSingleton<IChatClient>(sp =>
            new GeminiNanoChatClient()
                .AsBuilder()
                .UseLogging(sp.GetRequiredService<ILoggerFactory>())
                .Build());
        // Embeddings not available natively - consider Azure OpenAI if needed
#elif WINDOWS
        services.AddSingleton<IChatClient>(sp =>
            new PhiSilicaChatClient()
                .AsBuilder()
                .UseLogging(sp.GetRequiredService<ILoggerFactory>())
                .Build());

        services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(sp =>
            new PhiSilicaEmbeddingGenerator()
                .AsBuilder()
                .UseLogging(sp.GetRequiredService<ILoggerFactory>())
                .Build());
#endif
        return services;
    }
}
```

## Next steps

- Learn about [platform-specific APIs](platform-apis.md)
- Compare [features across platforms](feature-comparison.md)
- Build [multi-agent workflows](agent-framework.md)
- Review [requirements](requirements.md) for your target platforms

## External resources

- [Microsoft.Extensions.AI documentation](/dotnet/ai/ai-extensions)
- [Apple Foundation Models](https://developer.apple.com/documentation/foundationmodels)
- [Apple Natural Language framework](https://developer.apple.com/documentation/naturallanguage)
- [Google ML Kit GenAI](https://developers.google.com/ml-kit/genai)
- [Windows Copilot Runtime - Phi Silica](/windows/ai/apis/phi-silica)
