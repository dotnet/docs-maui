---
title: "Migrate data from the Xamarin.Forms app properties dictionary to .NET MAUI preferences"
description: "Learn how to migrate your app data from the Xamarin.Forms app properties dictionary to .NET MAUI preferences."
ms.date: 09/29/2023
---

# Migrate data from the Xamarin.Forms app properties dictionary to .NET MAUI preferences

Xamarin.Forms has a `Properties` dictionary that can be used to store data, and which is accessed using the `Application.Current.Properties` property. This dictionary uses a `string` key and stores an `object` value. The values in the dictionary are saved to the device when an app is paused or shut down, and loaded when an app is restarted or returns from the background. For more information about the properties dictionary, see [Properties dictionary](/xamarin/xamarin-forms/app-fundamentals/application-class#properties-dictionary).

When migrating a Xamarin.Forms app that stores data in the app properties dictionary to .NET MAUI, you should migrate this data to .NET MAUI preferences. This can be accomplished with the `LegacyApplication` class, and helper classes, which is presented in this article. This class enables your .NET MAUI app on Android, iOS, and Windows, to read data from the app properties dictionary that was created with a previous Xamarin.Forms version of your app. For more information about .NET MAUI preferences, see [Preferences](~/platform-integration/storage/preferences.md).

> [!IMPORTANT]
> There's no API to access the app properties dictionary in .NET MAUI.

## Access legacy app properties data

The following code shows the `LegacyApplication` class, which provides access to the app properties data created by your Xamarin.Forms app:

> [!NOTE]
> To use this code, add it to a class named `LegacyApplication` in your .NET MAUI app project.

```csharp
namespace MigrationHelpers;

public class LegacyApplication
{
    readonly PropertiesDeserializer deserializer;
    Task<IDictionary<string, object>>? propertiesTask;

    static LegacyApplication? current;
    public static LegacyApplication? Current
    {
        get
        {
            current ??= (LegacyApplication)Activator.CreateInstance(typeof(LegacyApplication));
            return current;
        }
    }

    public LegacyApplication()
    {
        deserializer = new PropertiesDeserializer();
    }

    public IDictionary<string, object> Properties
    {
        get
        {
            propertiesTask ??= GetPropertiesAsync();
            return propertiesTask.Result;
        }
    }

    async Task<IDictionary<string, object>> GetPropertiesAsync()
    {
        IDictionary<string, object> properties = await deserializer.DeserializePropertiesAsync().ConfigureAwait(false);
        properties ??= new Dictionary<string, object>(4);
        return properties;
    }
}
```

### Android

On Android, the `LegacyApplication` class uses the `PropertiesDeserializer` class to deserialize data from the app properties dictionary file. The following code shows the `PropertiesDeserializer` class:

> [!NOTE]
> To use this code, add it to a class named `PropertiesDeserializer` in the *Platforms\Android* folder of your .NET MAUI app project.

```csharp
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Xml;

namespace MigrationHelpers;

public class PropertiesDeserializer
{
    const string PropertyStoreFile = "PropertyStore.forms";

    public Task<IDictionary<string, object>> DeserializePropertiesAsync()
    {
        // Deserialize property dictionary to local storage
        return Task.Run(() =>
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.FileExists(PropertyStoreFile))
                    return null;

                using (IsolatedStorageFileStream stream = store.OpenFile(PropertyStoreFile, FileMode.Open, FileAccess.Read))
                using (XmlDictionaryReader reader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max))
                {
                    if (stream.Length == 0)
                        return null;

                    try
                    {
                        var dcs = new DataContractSerializer(typeof(Dictionary<string, object>));
                        return (IDictionary<string, object>)dcs.ReadObject(reader);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Could not deserialize properties: " + e.Message);
                        Console.WriteLine($"PropertyStore Exception while reading Application properties: {e}");
                    }
                }
            }
            return null;
        });
    }
}
```

### iOS

On iOS, the `LegacyApplication` class uses the `PropertiesDeserializer` class to deserialize data from the app properties dictionary file. The following code shows the `PropertiesDeserializer` class:

> [!NOTE]
> To use this code, add it to a class named `PropertiesDeserializer` in the *Platforms\iOS* folder of your .NET MAUI app project.

```csharp
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Xml;

namespace MigrationHelpers;

public class PropertiesDeserializer
{
    const string PropertyStoreFile = "PropertyStore.forms";

    public Task<IDictionary<string, object>> DeserializePropertiesAsync()
    {
        // Deserialize property dictionary to local storage
        return Task.Run(() =>
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = store.OpenFile(PropertyStoreFile, System.IO.FileMode.OpenOrCreate))
            using (var reader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max))
            {
                if (stream.Length == 0)
                    return null;

                try
                {
                    var dcs = new DataContractSerializer(typeof(Dictionary<string, object>));
                    return (IDictionary<string, object>)dcs.ReadObject(reader);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Could not deserialize properties: " + e.Message);
                    Console.WriteLine($"PropertyStore Exception while reading Application properties: {e}");
                }
            }
            return null;
        });
    }
}
```

### Windows

On Windows, the `LegacyApplication` class uses the `PropertiesDeserializer` class to deserialize data from the app properties dictionary file. The following code shows the `PropertiesDeserializer` class:

> [!NOTE]
> To use this code, add it to a class named `PropertiesDeserializer` in the *Platforms\Windows* folder of your .NET MAUI app project.

```csharp
using System.Diagnostics;
using System.Runtime.Serialization;
using Windows.Storage;

namespace MigrationHelpers;

public class PropertiesDeserializer
{
    const string PropertyStoreFile = "PropertyStore.forms";

    public async Task<IDictionary<string, object>> DeserializePropertiesAsync()
    {
        try
        {
            StorageFile file = await ApplicationData.Current.RoamingFolder.GetFileAsync(PropertyStoreFile).DontSync();
            using (Stream stream = (await file.OpenReadAsync().DontSync()).AsStreamForRead())
            {
                if (stream.Length == 0)
                    return new Dictionary<string, object>(4);

                try
                {
                    var serializer = new DataContractSerializer(typeof(IDictionary<string, object>));
                    return (IDictionary<string, object>)serializer.ReadObject(stream);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Could not deserialize properties: " + e.Message);
                    Console.WriteLine($"PropertyStore Exception while reading Application properties: {e}");
                }
                return null;
            }
        }
        catch (FileNotFoundException)
        {
            return new Dictionary<string, object>(4);
        }
    }
}
```

This Windows version of the `PropertiesDeserializer` class requires the `DontSync` extension method. The following code shows this extension method:

> [!NOTE]
> To use this code, add it to a class named `Extensions` in the *Platforms\Windows* folder of your .NET MAUI app project.

```csharp
using System.Runtime.CompilerServices;
using Windows.Foundation;

namespace MigrationHelpers;

internal static class Extensions
{
    public static ConfiguredTaskAwaitable<T> DontSync<T>(this IAsyncOperation<T> self)
    {
        return self.AsTask().ConfigureAwait(false);
    }
}
```

## Consume legacy app property data

The `LegacyApplication` class can be used to consume data from the app properties dictionary, on Android, iOS, and Windows, that was created with a previous Xamarin.Forms version of your app:

```csharp
#if ANDROID || IOS || WINDOWS
using MigrationHelpers;
...

int id;
if (LegacyApplication.Current.Properties.ContainsKey("id"))
{
    id = (int)LegacyApplication.Current.Properties["id"];
    Preferences.Set("id", id);
}
#endif
```

This example shows using the `LegacyApplication` class to read a value from the app properties dictionary, and then write the value to .NET MAUI preferences.

> [!IMPORTANT]
> Always check for the presence of the key in the app properties dictionary before accessing it, to prevent unexpected errors.
