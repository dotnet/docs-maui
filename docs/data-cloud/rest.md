---
title: "Consume a REST-based web service"
description: "Learn how to consume a REST-based web service from a .NET MAUI app."
ms.date: ms.date: 09/30/2024
---

# Consume a REST-based web service

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/webservices-rest)

Representational State Transfer (REST) is an architectural style for building web services. REST requests are typically made over HTTPS using the same HTTP verbs that web browsers use to retrieve web pages and to send data to servers. The verbs are:

- **GET** – this operation is used to retrieve data from the web service.
- **POST** – this operation is used to create a new item of data on the web service.
- **PUT** – this operation is used to update an item of data on the web service.
- **PATCH** – this operation is used to update an item of data on the web service by describing a set of instructions about how the item should be modified.
- **DELETE** – this operation is used to delete an item of data on the web service.

Web service APIs that adhere to REST are defined using:

- A base URI.
- HTTP methods, such as GET, POST, PUT, PATCH, or DELETE.
- A media type for the data, such as JavaScript Object Notation (JSON).

REST-based web services typically use JSON messages to return data to the client. JSON is a text-based data-interchange format that produces compact payloads, which results in reduced bandwidth requirements when sending data. The simplicity of REST has helped make it the primary method for accessing web services in mobile apps.

> [!NOTE]
> Accessing a web service often requires asynchronous programming. For more information about asynchronous programming, see [Asynchronous programming with async and await](/dotnet/csharp/asynchronous-programming).

## Web service operations

The example REST service is written using ASP.NET Core and provides the following operations:

|Operation|HTTP method|Relative URI|Parameters|
|--- |--- |--- |--- |
|Get a list of todo items|GET|/api/todoitems/| |
|Create a new todo item|POST|/api/todoitems/|A JSON formatted TodoItem|
|Update a todo item|PUT|/api/todoitems/|A JSON formatted TodoItem|
|Delete a todo item|DELETE|/api/todoitems/{id}| |

The .NET MAUI app and web service uses the `TodoItem` class to model the data that is displayed and sent to the web service for storage:

```csharp
public class TodoItem
{
    public string ID { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    public bool Done { get; set; }
}
```

The `ID` property is used to uniquely identify each `TodoItem` object, and is used by the web service to identify data to be updated or deleted. For example, to delete the `TodoItem` whose ID is `6bb8a868-dba1-4f1a-93b7-24ebce87e243`, the .NET MAUI app sends a DELETE request to `https://hostname/api/todoitems/6bb8a868-dba1-4f1a-93b7-24ebce87e243`.

When the Web API framework receives a request, it routes the request to an action. These actions are public methods in the `TodoItemsController` class. The Web API framework uses routing middleware to match the URLs of incoming requests and map them to actions. REST APIs should use attribute routing to model the app's functionality as a set of resources whose operations are represented by HTTP verbs. Attribute routing uses a set of attributes to map actions directly to route templates. For more information about attribute routing, see [Attribute routing for REST APIs](/aspnet/core/mvc/controllers/routing#ar). For more information about building the REST service using ASP.NET Core, see [Creating backend services for native mobile applications](/aspnet/core/mobile/native-mobile-backend/).

## Create the HTTPClient object

A .NET Multi-platform App UI (.NET MAUI) app can consume a REST-based web service by sending requests to the web service with the [`HttpClient`](xref:System.Net.Http.HttpClient) class. This class provides functionality for sending HTTP requests and receiving HTTP responses from a URI identified resource. Each request is sent as an asynchronous operation.

The `HttpClient` object should be declared at the class-level so that it lives for as long as the app needs to make HTTP requests:

```csharp
public class RestService
{
    HttpClient _client;
    JsonSerializerOptions _serializerOptions;

    public List<TodoItem> Items { get; private set; }

    public RestService()
    {
        _client = new HttpClient();
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
    ...
}
```

The [`JsonSerializerOptions`](xref:System.Text.Json.JsonSerializerOptions) object is used to configure the formatting of the JSON payload that's received from and sent to the web service. For more information, see [How to instantiate JsonSerializerOptions instances with System.Text.Json](/dotnet/standard/serialization/system-text-json-configure-options?pivots=dotnet-6-0).

## Retrieve data

The `HttpClient.GetAsync` method is used to send a GET request to the web service specified by the URI, and then receive the response from the web service:

```csharp
public async Task<List<TodoItem>> RefreshDataAsync()
{
    Items = new List<TodoItem>();

    Uri uri = new Uri(string.Format(Constants.RestUrl, string.Empty));
    try
    {
        HttpResponseMessage response = await _client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            Items = JsonSerializer.Deserialize<List<TodoItem>>(content, _serializerOptions);
        }
    }
    catch (Exception ex)
    {
        Debug.WriteLine(@"\tERROR {0}", ex.Message);
    }

    return Items;
}
```

Data is received from the web service as a [`HttpResponseMessage`](xref:System.Net.Http.HttpResponseMessage) object. It contains information about the response, including the status code, headers, and any body. The REST service sends an HTTP status code in its response, which can be obtained from the `HttpResponseMessage.IsSuccessStatusCode` property, to indicate whether the HTTP request succeeded or failed. For this operation the REST service sends HTTP status code 200 (OK) in the response, which indicates that the request succeeded and that the requested information is in the response.

If the HTTP operation was successful, the content of the response is read. The `HttpResponseMessage.Content` property represents the content of the response, and is of type [`HttpContent`](xref:System.Net.Http.HttpContent). The `HttpContent` class represents the HTTP body and content headers, such as `Content-Type` and `Content-Encoding`. The content is then read into a `string` using the [`HttpContent.ReadAsStringAsync`](xref:System.Net.Http.HttpContent.ReadAsStringAsync) method. The `string` is then deserialized from JSON to a `List` of `TodoItem` objects.

> [!WARNING]
> Using the `ReadAsStringAsync` method to retrieve a large response can have a negative performance impact. In such circumstances the response should be directly deserialized to avoid having to fully buffer it.

## Create data

The `HttpClient.PostAsync` method is used to send a POST request to the web service specified by the URI, and then to receive the response from the web service:

```csharp
public async Task SaveTodoItemAsync(TodoItem item, bool isNewItem = false)
{
    Uri uri = new Uri(string.Format(Constants.RestUrl, string.Empty));

    try
    {
        string json = JsonSerializer.Serialize<TodoItem>(item, _serializerOptions);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = null;
        if (isNewItem)
            response = await _client.PostAsync(uri, content);
        else
            response = await _client.PutAsync(uri, content);

        if (response.IsSuccessStatusCode)
            Debug.WriteLine(@"\tTodoItem successfully saved.");
    }
    catch (Exception ex)
    {
        Debug.WriteLine(@"\tERROR {0}", ex.Message);
    }
}
```

In this example, the `TodoItem` instance is serialized to a JSON payload for sending to the web service. This payload is then embedded in the body of the HTTP content that will be sent to the web service before the request is made with the `PostAsync` method.

The REST service sends an HTTP status code in its response, which can be obtained from the `HttpResponseMessage.IsSuccessStatusCode` property, to indicate whether the HTTP request succeeded or failed. The typical responses for this operation are:

- **201 (CREATED)** – the request resulted in a new resource being created before the response was sent.
- **400 (BAD REQUEST)** – the request is not understood by the server.
- **409 (CONFLICT)** – the request could not be carried out because of a conflict on the server.

## Update data

The `HttpClient.PutAsync` method is used to send a PUT request to the web service specified by the URI, and then receive the response from the web service:

```csharp
public async Task SaveTodoItemAsync(TodoItem item, bool isNewItem = false)
{
  ...
  response = await _client.PutAsync(uri, content);
  ...
}
```

The operation of the `PutAsync` method is identical to the `PostAsync` method that's used for creating data in the web service. However, the possible responses sent from the web service differ.

The REST service sends an HTTP status code in its response, which can be obtained from the `HttpResponseMessage.IsSuccessStatusCode` property, to indicate whether the HTTP request succeeded or failed. The typical responses for this operation are:

- **204 (NO CONTENT)** – the request has been successfully processed and the response is intentionally blank.
- **400 (BAD REQUEST)** – the request is not understood by the server.
- **404 (NOT FOUND)** – the requested resource does not exist on the server.

## Delete data

The `HttpClient.DeleteAsync` method is used to send a DELETE request to the web service specified by the URI, and then receive the response from the web service:

```csharp
public async Task DeleteTodoItemAsync(string id)
{
    Uri uri = new Uri(string.Format(Constants.RestUrl, id));

    try
    {
        HttpResponseMessage response = await _client.DeleteAsync(uri);
        if (response.IsSuccessStatusCode)
            Debug.WriteLine(@"\tTodoItem successfully deleted.");
    }
    catch (Exception ex)
    {
        Debug.WriteLine(@"\tERROR {0}", ex.Message);
    }
}
```

The REST service sends an HTTP status code in its response, which can be obtained from the `HttpResponseMessage.IsSuccessStatusCode` property, to indicate whether the HTTP request succeeded or failed. The typical responses for this operation are:

- **204 (NO CONTENT)** – the request has been successfully processed and the response is intentionally blank.
- **400 (BAD REQUEST)** – the request is not understood by the server.
- **404 (NOT FOUND)** – the requested resource does not exist on the server.

## Local development

If you're developing a REST web service locally with a framework such as ASP.NET Core Web API, you can debug your web service and .NET MAUI app at the same time. In this scenario, to consume your web service over HTTP from Android emulators and iOS simulators, you must enable clear-text HTTP traffic in your .NET MAUI app. For more information, see [Connect to local web services from Android emulators and iOS simulators](local-web-services.md).
