using System.Security.AccessControl;

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
            Children = {
                new Button { Text = "Read",
                             Command = new Command(PickAndShow) },
                new Button { Text = "ToUpperFile",
                             Command = new Command(ToUpperFile) },
            }
        };
    }

    private void PickAndShow()
    {
        var customFileType = new FilePickerFileType(
                        new Dictionary<DevicePlatform, IEnumerable<string>>
                        {
                            { DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // UTType values
                            { DevicePlatform.Android, new[] { "application/comics" } }, // MIME type
                            { DevicePlatform.WinUI, new[] { ".cbr", ".cbz" } }, // file extension
                            { DevicePlatform.Tizen, new[] { "*/*" } },
                            { DevicePlatform.MacCatalyst, new[] { "cbr", "cbz" } }, // UTType values
                        });

        PickOptions options = new()
        {
            PickerTitle = "Please select a comic file",
            FileTypes = customFileType,
        };

        PickAndShow(options);
    }

    async void ToUpperFile(object param1)
    {
        await ConvertFileToUpperCase("AboutAssets.txt", "NewAssets.txt");

        string targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "NewAssets.txt");

        ((VerticalStackLayout)Content).Children.Add(new Label() { Text = System.IO.File.ReadAllText(targetFile) });
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
                            { DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // UTType values
                            { DevicePlatform.Android, new[] { "application/comics" } }, // MIME type
                            { DevicePlatform.WinUI, new[] { ".cbr", ".cbz" } }, // file extension
                            { DevicePlatform.Tizen, new[] { "*/*" } },
                            { DevicePlatform.macOS, new[] { "cbr", "cbz" } }, // UTType values
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

    public async Task ConvertFileToUpperCase(string sourceFile, string targetFileName)
    {
        // Read the source file
        using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(sourceFile);
        using StreamReader reader = new StreamReader(fileStream);

        string content = await reader.ReadToEndAsync();

        // Transform file content to upper case text
        content = content.ToUpperInvariant();

        // Write the file content to the app data directory
        string targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, targetFileName);

        using FileStream outputStream = System.IO.File.OpenWrite(targetFile);
        using StreamWriter streamWriter = new StreamWriter(outputStream);

        await streamWriter.WriteAsync(content);
    }

    //<filesys_copyfile>
    public async Task CopyFileToAppDataDirectory(string filename)
    {
        // Open the source file
        using Stream inputStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);

        // Create an output filename
        string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, filename);

        // Copy the file to the AppDataDirectory
        using FileStream outputStream = File.Create(targetFile);
        await inputStream.CopyToAsync(outputStream);
    }
    //</filesys_copyfile>

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
        string firstName = Preferences.Default.Get("first_name", "Unknown");
        int age = Preferences.Default.Get("age", -1);
        bool hasPets = Preferences.Default.Get("has_pets", false);
        //</prefs_defaults>

        //<prefs_containskey>
        bool hasKey = Preferences.Default.ContainsKey("my_key");
        //</prefs_containskey>

        //<prefs_remove>
        Preferences.Default.Remove("first_name");
        //</prefs_remove>

        //<prefs_clear>
        Preferences.Default.Clear();
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
