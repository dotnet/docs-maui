---
title: "Migrate version tracking data from a Xamarin.Forms app to a .NET MAUI app"
description: "Learn how to migrate your version tracking data from a Xamarin.Forms app to a .NET MAUI app."
ms.date: 10/06/2023
---

# Migrate version tracking data from a Xamarin.Forms app to a .NET MAUI app

Xamarin.Essentials and .NET Multi-platform App UI (.NET MAUI) both have a `VersionTracking` class that lets you check the app's version and build numbers, along with additional information such as if it's the first time the app was launched. However, in Xamarin.Essentials the version tracking data is stored in a platform-specific preferences container with a name of `{your-app-package-id}.xamarinessentials.versiontracking`, while in .NET MAUI the version tracking data is stored in a platform-specific preferences container with a name of `{your-app-package-id}.microsoft.maui.essentials.versiontracking`.

When migrating a Xamarin.Forms app that uses the `VersionTracking` class to .NET MAUI, you must deal with this preferences container naming difference to provide users with a smooth upgrade experience. This article describes how you can use the `LegacyVersionTracking` class and helper classes to deal with the preferences container. The `LegacyVersionTracking` class enables your .NET MAUI app on Android, iOS, and Windows, to read version tracking data that was created with a previous Xamarin.Forms version of your app.

> [!IMPORTANT]
> For the `LegacyVersionTracking` class to work correctly, your .NET MAUI app must have a higher version number than the version number of your Xamarin.Forms app. The version number can be set in your .NET MAUI app's project file with the `$(ApplicationVersion)` and `$(ApplicationDisplayVersion)` build properties.

For more information about the `VersionTracking` class in Xamarin.Essentials, see [Xamarin.Essentials: Version tracking](/xamarin/essentials/version-tracking). For more information about the <xref:Microsoft.Maui.ApplicationModel.VersionTracking> class in .NET MAUI, see [Version tracking](~/platform-integration/appmodel/version-tracking.md).

## Access legacy version tracking data

The following code shows the `LegacyVersionTracking` class, which provides access to the version tracking data created by your Xamarin.Forms app:

> [!NOTE]
> To use this code, add it to a class named `LegacyVersionTracking` in your .NET MAUI app project.

```csharp
namespace MigrationHelpers;

public static class LegacyVersionTracking
{
    const string versionsKey = "VersionTracking.Versions";
    const string buildsKey = "VersionTracking.Builds";

    static readonly string sharedName = LegacyPreferences.GetPrivatePreferencesSharedName("versiontracking");

    static Dictionary<string, List<string>> versionTrail;
    static string LastInstalledVersion => versionTrail[versionsKey].LastOrDefault();
    static string LastInstalledBuild => versionTrail[buildsKey].LastOrDefault();

    public static string VersionsKey => versionsKey;
    public static string BuildsKey => buildsKey;
    public static string SharedName => sharedName;
    public static bool IsFirstLaunchEver { get; private set; }
    public static bool IsFirstLaunchForCurrentVersion { get; private set; }
    public static bool IsFirstLaunchForCurrentBuild { get; private set; }
    public static string CurrentVersion => AppInfo.VersionString;
    public static string CurrentBuild => AppInfo.BuildString;
    public static string PreviousVersion => GetPrevious(versionsKey);
    public static string PreviousBuild => GetPrevious(buildsKey);
    public static string FirstInstalledVersion => versionTrail[versionsKey].FirstOrDefault();
    public static string FirstInstalledBuild => versionTrail[buildsKey].FirstOrDefault();
    public static IEnumerable<string> VersionHistory => versionTrail[versionsKey].ToArray();
    public static IEnumerable<string> BuildHistory => versionTrail[buildsKey].ToArray();
    public static bool IsFirstLaunchForVersion(string version) => CurrentVersion == version && IsFirstLaunchForCurrentVersion;
    public static bool IsFirstLaunchForBuild(string build) => CurrentBuild == build && IsFirstLaunchForCurrentBuild;

    static LegacyVersionTracking()
    {
        InitVersionTracking();
    }

    internal static void InitVersionTracking()
    {
        IsFirstLaunchEver = !LegacyPreferences.ContainsKey(versionsKey, sharedName) || !LegacyPreferences.ContainsKey(buildsKey, sharedName);
        if (IsFirstLaunchEver)
        {
            versionTrail = new Dictionary<string, List<string>>
                {
                    { versionsKey, new List<string>() },
                    { buildsKey, new List<string>() }
                };
        }
        else
        {
            versionTrail = new Dictionary<string, List<string>>
                {
                    { versionsKey, ReadHistory(versionsKey).ToList() },
                    { buildsKey, ReadHistory(buildsKey).ToList() }
                };
        }

        IsFirstLaunchForCurrentVersion = !versionTrail[versionsKey].Contains(CurrentVersion) || CurrentVersion != LastInstalledVersion;
        if (IsFirstLaunchForCurrentVersion)
        {
            // Avoid duplicates and move current version to end of list if already present
            versionTrail[versionsKey].RemoveAll(v => v == CurrentVersion);
            versionTrail[versionsKey].Add(CurrentVersion);
        }

        IsFirstLaunchForCurrentBuild = !versionTrail[buildsKey].Contains(CurrentBuild) || CurrentBuild != LastInstalledBuild;
        if (IsFirstLaunchForCurrentBuild)
        {
            // Avoid duplicates and move current build to end of list if already present
            versionTrail[buildsKey].RemoveAll(b => b == CurrentBuild);
            versionTrail[buildsKey].Add(CurrentBuild);
        }
    }

    static string GetPrevious(string key)
    {
        var trail = versionTrail[key];
        return (trail.Count >= 2) ? trail[trail.Count - 2] : null;
    }

    static string[] ReadHistory(string key) => LegacyPreferences.Get(key, null, sharedName)?.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
}
```

The `LegacyVersionTracking` class uses the `LegacyPreferences` class, which provides access to version tracking data stored by the Xamarin.Essentials `Preferences` class from your Xamarin.Forms app:

> [!NOTE]
> To use this code, add it to a class named `LegacyPreferences` in your .NET MAUI app project.

```csharp
#if ANDROID || IOS || WINDOWS
namespace MigrationHelpers;

public static partial class LegacyPreferences
{
    internal static string GetPrivatePreferencesSharedName(string feature) => $"{AppInfo.PackageName}.xamarinessentials.{feature}";

    public static bool ContainsKey(string key, string sharedName) => PlatformContainsKey(key, sharedName);
    public static void Remove(string key, string sharedName) => PlatformRemove(key, sharedName);
    public static string Get(string key, string defaultValue, string sharedName) => PlatformGet<string>(key, defaultValue, sharedName);
}
#endif
```

The `LegacyPreferences` class is a `partial` class whose remaining implementation is platform-specific.

### Android

On Android, the `LegacyPreferences` class provides the preferences container implementation that retrieves data from shared preferences. The following code shows the `LegacyPreferences` class:

> [!NOTE]
> To use this code, add it to a class named `LegacyPreferences` in the *Platforms\Android* folder of your .NET MAUI app project.

```csharp
using System.Globalization;
using Android.Content;
using Android.Preferences;
using Application = Android.App.Application;

namespace MigrationHelpers;

public static partial class LegacyPreferences
{
    static readonly object locker = new object();

    static bool PlatformContainsKey(string key, string sharedName)
    {
        lock (locker)
        {
            using (var sharedPreferences = GetSharedPreferences(sharedName))
            {
                return sharedPreferences.Contains(key);
            }
        }
    }

    static void PlatformRemove(string key, string sharedName)
    {
        lock (locker)
        {
            using (var sharedPreferences = GetSharedPreferences(sharedName))
            using (var editor = sharedPreferences.Edit())
            {
                editor.Remove(key).Apply();
            }
        }
    }

    static T PlatformGet<T>(string key, T defaultValue, string sharedName)
    {
        lock (locker)
        {
            object value = null;
            using (var sharedPreferences = GetSharedPreferences(sharedName))
            {
                if (defaultValue == null)
                {
                    value = sharedPreferences.GetString(key, null);
                }
                else
                {
                    switch (defaultValue)
                    {
                        case int i:
                            value = sharedPreferences.GetInt(key, i);
                            break;
                        case bool b:
                            value = sharedPreferences.GetBoolean(key, b);
                            break;
                        case long l:
                            value = sharedPreferences.GetLong(key, l);
                            break;
                        case double d:
                            var savedDouble = sharedPreferences.GetString(key, null);
                            if (string.IsNullOrWhiteSpace(savedDouble))
                            {
                                value = defaultValue;
                            }
                            else
                            {
                                if (!double.TryParse(savedDouble, NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out var outDouble))
                                {
                                    var maxString = Convert.ToString(double.MaxValue, CultureInfo.InvariantCulture);
                                    outDouble = savedDouble.Equals(maxString) ? double.MaxValue : double.MinValue;
                                }

                                value = outDouble;
                            }
                            break;
                        case float f:
                            value = sharedPreferences.GetFloat(key, f);
                            break;
                        case string s:
                            // the case when the string is not null
                            value = sharedPreferences.GetString(key, s);
                            break;
                    }
                }
            }

            return (T)value;
        }
    }

    static ISharedPreferences GetSharedPreferences(string sharedName)
    {
        var context = Application.Context;

        return string.IsNullOrWhiteSpace(sharedName) ?
#pragma warning disable CS0618 // Type or member is obsolete
            PreferenceManager.GetDefaultSharedPreferences(context) :
#pragma warning restore CS0618 // Type or member is obsolete
                context.GetSharedPreferences(sharedName, FileCreationMode.Private);
    }
}
```

### iOS

On iOS, the `LegacyPreferences` class provides the preferences container implementation that retrieves data from `NSUserDefaults`. The following code shows the `LegacyPreferences` class:

> [!NOTE]
> To use this code, add it to a class named `LegacyPreferences` in the *Platforms\iOS* folder of your .NET MAUI app project.

```csharp
using Foundation;
using System.Globalization;

namespace MigrationHelpers;

public static partial class LegacyPreferences
{
    static readonly object locker = new object();

    static bool PlatformContainsKey(string key, string sharedName)
    {
        lock (locker)
        {
            return GetUserDefaults(sharedName)[key] != null;
        }
    }

    static void PlatformRemove(string key, string sharedName)
    {
        lock (locker)
        {
            using (var userDefaults = GetUserDefaults(sharedName))
            {
                if (userDefaults[key] != null)
                    userDefaults.RemoveObject(key);
            }
        }
    }

    static T PlatformGet<T>(string key, T defaultValue, string sharedName)
    {
        object value = null;

        lock (locker)
        {
            using (var userDefaults = GetUserDefaults(sharedName))
            {
                if (userDefaults[key] == null)
                    return defaultValue;

                switch (defaultValue)
                {
                    case int i:
                        value = (int)(nint)userDefaults.IntForKey(key);
                        break;
                    case bool b:
                        value = userDefaults.BoolForKey(key);
                        break;
                    case long l:
                        var savedLong = userDefaults.StringForKey(key);
                        value = Convert.ToInt64(savedLong, CultureInfo.InvariantCulture);
                        break;
                    case double d:
                        value = userDefaults.DoubleForKey(key);
                        break;
                    case float f:
                        value = userDefaults.FloatForKey(key);
                        break;
                    case string s:
                        // the case when the string is not null
                        value = userDefaults.StringForKey(key);
                        break;
                    default:
                        // the case when the string is null
                        if (typeof(T) == typeof(string))
                            value = userDefaults.StringForKey(key);
                        break;
                }
            }
        }

        return (T)value;
    }

    static NSUserDefaults GetUserDefaults(string sharedName)
    {
        if (!string.IsNullOrWhiteSpace(sharedName))
            return new NSUserDefaults(sharedName, NSUserDefaultsType.SuiteName);
        else
            return NSUserDefaults.StandardUserDefaults;
    }
}
```

### Windows

On Windows, the `LegacyVersionTracking` class provides the preferences container implementation that retrieves data from <xref:Windows.Storage.ApplicationDataContainer>. The following code shows the `LegacyPreferences` class:

> [!NOTE]
> To use this code, add it to a class named `LegacyPreferences` in the *Platforms\Windows* folder of your .NET MAUI app project.

```csharp
using Windows.Storage;

namespace MigrationHelpers;

public static partial class LegacyPreferences
{
    static readonly object locker = new object();

    static bool PlatformContainsKey(string key, string sharedName)
    {
        lock (locker)
        {
            var appDataContainer = GetApplicationDataContainer(sharedName);
            return appDataContainer.Values.ContainsKey(key);
        }
    }

    static void PlatformRemove(string key, string sharedName)
    {
        lock (locker)
        {
            var appDataContainer = GetApplicationDataContainer(sharedName);
            if (appDataContainer.Values.ContainsKey(key))
                appDataContainer.Values.Remove(key);
        }
    }

    static T PlatformGet<T>(string key, T defaultValue, string sharedName)
    {
        lock (locker)
        {
            var appDataContainer = GetApplicationDataContainer(sharedName);
            if (appDataContainer.Values.ContainsKey(key))
            {
                var tempValue = appDataContainer.Values[key];
                if (tempValue != null)
                    return (T)tempValue;
            }
        }

        return defaultValue;
    }

    static ApplicationDataContainer GetApplicationDataContainer(string sharedName)
    {
        var localSettings = ApplicationData.Current.LocalSettings;
        if (string.IsNullOrWhiteSpace(sharedName))
            return localSettings;

        if (!localSettings.Containers.ContainsKey(sharedName))
            localSettings.CreateContainer(sharedName, ApplicationDataCreateDisposition.Always);

        return localSettings.Containers[sharedName];
    }
}
```

## Consume legacy version tracking data

The `LegacyVersionTracking` class can be used to retrieve version tracking data on Android, iOS, and Windows that was created with a previous Xamarin.Forms version of your app:

```csharp
#if ANDROID || IOS || WINDOWS
using MigrationHelpers;
...

string isFirstLaunchEver = LegacyVersionTracking.IsFirstLaunchEver.ToString();
string currentVersionIsFirst = LegacyVersionTracking.IsFirstLaunchForCurrentVersion.ToString();
string currentBuildIsFirst = LegacyVersionTracking.IsFirstLaunchForCurrentBuild.ToString();
string currentVersion = LegacyVersionTracking.CurrentVersion.ToString();
string currentBuild = LegacyVersionTracking.CurrentBuild.ToString();
string firstInstalledVer = LegacyVersionTracking.FirstInstalledVersion.ToString();
string firstInstalledBuild = LegacyVersionTracking.FirstInstalledBuild.ToString();
string versionHistory = String.Join(',', LegacyVersionTracking.VersionHistory);
string buildHistory = String.Join(',', LegacyVersionTracking.BuildHistory);
string previousVersion = LegacyVersionTracking.PreviousVersion?.ToString() ?? "none";
string previousBuild = LegacyVersionTracking.PreviousBuild?.ToString() ?? "none";
#endif
```

This example shows using the `LegacyVersionTracking` class to read legacy version tracking data. However, this data can't be assigned to .NET MAUI's <xref:Microsoft.Maui.ApplicationModel.VersionTracking> class, because its properties can't be set. Instead, the data can be written to .NET MAUI preferences with the `WriteHistory` method:

```csharp
void WriteHistory(string key, IEnumerable<string> history)
{
    Preferences.Default.Set(key, string.Join("|", history), $"{AppInfo.Current.PackageName}.microsoft.maui.essentials.versiontracking");
}

#if ANDROID || IOS || WINDOWS
WriteHistory(LegacyVersionTracking.VersionsKey, LegacyVersionTracking.VersionHistory);
WriteHistory(LegacyVersionTracking.BuildsKey, LegacyVersionTracking.BuildHistory);
#endif
```

Once the legacy version tracking data has been written to .NET MAUI preferences, it can be consumed by .NET MAUI's <xref:Microsoft.Maui.ApplicationModel.VersionTracking> class:

```csharp
var mauiVersionHistory = VersionTracking.Default.VersionHistory;
var mauiBuildHistory = VersionTracking.Default.BuildHistory;
```

The legacy version tracking data can then be removed from the device:

```csharp
#if ANDROID || IOS || WINDOWS
LegacyPreferences.Remove(LegacyVersionTracking.VersionsKey, LegacyVersionTracking.SharedName);
LegacyPreferences.Remove(LegacyVersionTracking.BuildsKey, LegacyVersionTracking.SharedName);
#endif
```
