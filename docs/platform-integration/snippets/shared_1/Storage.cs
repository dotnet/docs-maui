namespace PlatformIntegration;

public class StoragePage : ContentPage
{
    public ImageSource ImageItem
    {
        get { return (ImageSource)GetValue(ImageItemProperty); }
        set { SetValue(ImageItemProperty, value); }
    }

    public static readonly BindableProperty ImageItemProperty =
        BindableProperty.Create("ImageItem", typeof(ImageSource), typeof(MediaPage), null);

    public StoragePage()
	{
        this.BindingContext = this;

        Content = new VerticalStackLayout
        {
        };
    }

    //<file_pick>
    public async Task<FileResult> PickAndShow(PickOptions options)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    using var stream = await result.OpenReadAsync();
                    var image = ImageSource.FromStream(() => stream);
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
        }

        return null;
    }
    //</file_pick>

    public void Nothing()
    {
        //<file_types>
        var customFileType = new FilePickerFileType(
                        new Dictionary<DevicePlatform, IEnumerable<string>>
                        {
                            { DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // or general UTType values
                            { DevicePlatform.Android, new[] { "application/comics" } },
                            { DevicePlatform.WinUI, new[] { ".cbr", ".cbz" } },
                            { DevicePlatform.Tizen, new[] { "*/*" } },
                            { DevicePlatform.macOS, new[] { "cbr", "cbz" } }, // or general UTType values
                        });

        PickOptions options = new()
        {
            PickerTitle = "Please select a comic file",
            FileTypes = customFileType,
        };
        //</file_types>
    }

    public void FileSystem1()
    {
        //<filesys_cache>
        string cacheDir = FileSystem.Current.CacheDirectory;
        //</filesys_cache>
        //<filesys_appdata>
        string mainDir = FileSystem.Current.AppDataDirectory;
        //</filesys_appdata>
    }

    //<filesys_readtxtfile>
    public async Task<string> ReadTextFile(string filePath)
    {
        using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filePath);
        using StreamReader reader = new StreamReader(fileStream);

        return await reader.ReadToEndAsync();
    }
    //</filesys_readtxtfile>

    public void PreferencesSet()
    {
        //<prefs_set>
        // Set a string value:
        Preferences.Default.Set("first_name", "John");

        // Set an numerical value:
        Preferences.Default.Set("age", 28);

        // Set a boolean value:
        Preferences.Default.Set("has_pets", true);
        //</prefs_set>

        //<prefs_defaults>
        string firstName = Preferences.Get("first_name", "Unknown");
        int age = Preferences.Get("age", -1);
        bool hasPets = Preferences.Get("has_pets", false);
        //</prefs_defaults>

        //<prefs_containskey>
        bool hasKey = Preferences.ContainsKey("my_key");
        //</prefs_containskey>

        //<prefs_remove>
        Preferences.Remove("first_name");
        //</prefs_remove>

        //<prefs_clear>
        Preferences.Clear();
        //</prefs_clear>
    }

    public async Task Storage()
    {
        //<secstorage_set>
        await SecureStorage.Default.SetAsync("oauth_token", "secret-oauth-token-value");
        //</secstorage_set>

        //<secstorage_get>
        string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");

        if (oauthToken == null)
        {
            // No value is associated with the key "oauth_token"
        }
        //</secstorage_get>

        //<secstorage_remove>
        bool success = SecureStorage.Default.Remove("oauth_token");
        //</secstorage_remove>

        //<secstorage_remove_all>
        SecureStorage.Default.RemoveAll();
        //</secstorage_remove_all>
    }
}