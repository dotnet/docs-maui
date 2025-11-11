namespace PlatformIntegration;

public partial class DataPage : ContentPage
{
	public DataPage()
	{
		InitializeComponent();
	}

    //<clipboard_set>
    private async void SetClipboardButton_Clicked(object sender, EventArgs e) =>
        await Clipboard.Default.SetTextAsync("This text was highlighted in the UI.");
    //</clipboard_set>

    //<clipboard_android_toast>
    private async void SetClipboardWithToastButton_Clicked(object sender, EventArgs e)
    {
        await Clipboard.Default.SetTextAsync("TestValue");
#if ANDROID
        // Show toast notification only on Android 12 and lower
        // Android 13+ automatically shows system notifications
        if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Tiramisu)
        {
            Android.Widget.Toast.MakeText(
                Microsoft.Maui.ApplicationModel.Platform.CurrentActivity ?? Android.App.Application.Context,
                "Copied to clipboard",
                Android.Widget.ToastLength.Short)?.Show();
        }
#endif
    }
    //</clipboard_android_toast>

    //<clipboard_read>
    private async void ReadClipboardButton_Clicked(object sender, EventArgs e)
    {
        if (Clipboard.Default.HasText)
        {
            ClipboardOutputLabel.Text = await Clipboard.Default.GetTextAsync();
            await ClearClipboard();
        }
        else
            ClipboardOutputLabel.Text = "Clipboard is empty";
    }

    //<clipboard_clear>
    private async Task ClearClipboard() =>
        await Clipboard.Default.SetTextAsync(null);
    //</clipboard_clear>
    //</clipboard_read>

    //<clipboard_event>
    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        Clipboard.Default.ClipboardContentChanged += Clipboard_ClipboardContentChanged;
    }

    private async void Clipboard_ClipboardContentChanged(object sender, EventArgs e)
    {
        ClipboardOutputLabel.Text = await Clipboard.Default.GetTextAsync();
    }
    //</clipboard_event>

    private async void ShareButton_Clicked(object sender, EventArgs e)
    {
        await ShareText("Hello, welcome to the show.");
        await ShareUri("http://www.microsoft.com", Share.Default);
        await ShareFile();
        await ShareMultipleFiles();
    }
    //<share_text_uri>
    public async Task ShareText(string text)
    {
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = text,
            Title = "Share Text"
        });
    }

    public async Task ShareUri(string uri, IShare share)
    {
        await share.RequestAsync(new ShareTextRequest
        {
            Uri = uri,
            Title = "Share Web Link"
        });
    }
    //</share_text_uri>

    //<share_file>
    public async Task ShareFile()
    {
        string fn = "Attachment.txt";
        string file = Path.Combine(FileSystem.CacheDirectory, fn);

        File.WriteAllText(file, "Hello World");

        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "Share text file",
            File = new ShareFile(file)
        });
    }
    //</share_file>

    //<share_file_multiple>
    public async Task ShareMultipleFiles()
    {
        string file1 = Path.Combine(FileSystem.CacheDirectory, "Attachment1.txt");
        string file2 = Path.Combine(FileSystem.CacheDirectory, "Attachment2.txt");

        File.WriteAllText(file1, "Content 1");
        File.WriteAllText(file2, "Content 2");

        await Share.Default.RequestAsync(new ShareMultipleFilesRequest
        {
            Title = "Share multiple files",
            Files = new List<ShareFile> { new ShareFile(file1), new ShareFile(file2) }
        });
    }
    //</share_file_multiple>
}