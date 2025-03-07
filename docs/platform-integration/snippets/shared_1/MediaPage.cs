namespace PlatformIntegration;

public class MediaPage : ContentPage
{
    public ImageSource ImageItem
    {
        get { return (ImageSource)GetValue(ImageItemProperty); }
        set { SetValue(ImageItemProperty, value); }
    }

    public static readonly BindableProperty ImageItemProperty =
        BindableProperty.Create("ImageItem", typeof(ImageSource), typeof(MediaPage), null);


    public MediaPage()
	{
        this.BindingContext = this;

        var imageControl = new Image();
        imageControl.SetBinding(Image.SourceProperty, Binding.Create(static (ImageItem item) => item.ImageItem));

        Content = new VerticalStackLayout
        {
            imageControl,
            new Button { Text = "Take photo",
                            Command = new Command(TakePhoto) },
            new Button { Text = "Take screenshot",
                            Command = new Command(() => { ImageItem = TakeScreenshotAsync().Result; }) },
            new Button { Text = "Text to speech",
                            Command = new Command(Speak) },
        };
	}

	//<photo_take_and_save>
	public async void TakePhoto()
    {
		if (MediaPicker.Default.IsCaptureSupported)
        {
			FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

			if (photo != null)
            {
                // save the file into local storage
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);
            }
        }
    }
    //</photo_take_and_save>

    //<screenshot>
    public async Task<ImageSource> TakeScreenshotAsync()
    {
        if (Screenshot.Default.IsCaptureSupported)
        {
            IScreenshotResult screen = await Screenshot.Default.CaptureAsync();

            Stream stream = await screen.OpenReadAsync();

            return ImageSource.FromStream(() => stream);
        }

        return null;
    }
    //</screenshot>

    //<speak>
    public async void Speak() =>
        await TextToSpeech.Default.SpeakAsync("Hello World");
    //</speak>

    //<speak_options_old>
    public async void SpeakSettings()
    {
        IEnumerable<Locale> locales = await TextToSpeech.Default.GetLocalesAsync();

        SpeechOptions options = new SpeechOptions()
        {
            Pitch = 1.5f,   // 0.0 - 2.0
            Volume = 0.75f, // 0.0 - 1.0
            Locale = locales.FirstOrDefault()
        };

        await TextToSpeech.Default.SpeakAsync("How nice to meet you!", options);
    }
    //</speak_options>

    //<speak_options>
    public async void SpeakSettings()
    {
        IEnumerable<Locale> locales = await TextToSpeech.Default.GetLocalesAsync();

        SpeechOptions options = new SpeechOptions()
        {
            Pitch = 1.5f,   // 0.0 - 2.0
            Volume = 0.75f, // 0.0 - 1.0
            Rate = 1.5f,    // 0.1 - 2.0
            Locale = locales.FirstOrDefault()
        };

        await TextToSpeech.Default.SpeakAsync("How nice to meet you!", options);
    }
    //</speak_options>

    //<speak_cancel>
    CancellationTokenSource cts;

    public async Task SpeakNowDefaultSettingsAsync()
    {
        cts = new CancellationTokenSource();
        await TextToSpeech.Default.SpeakAsync("Hello World", cancelToken: cts.Token);

        // This method will block until utterance finishes.
    }

    // Cancel speech if a cancellation token exists & hasn't been already requested.
    public void CancelSpeech()
    {
        if (cts?.IsCancellationRequested ?? true)
            return;

        cts.Cancel();
    }
    //</speak_cancel>


    //<speak_queue>
    bool isBusy = false;

    public void SpeakMultiple()
    {
        isBusy = true;

        Task.WhenAll(
            TextToSpeech.Default.SpeakAsync("Hello World 1"),
            TextToSpeech.Default.SpeakAsync("Hello World 2"),
            TextToSpeech.Default.SpeakAsync("Hello World 3"))
            .ContinueWith((t) => { isBusy = false; }, TaskScheduler.FromCurrentSynchronizationContext());
    }
    //</speak_queue>
}
