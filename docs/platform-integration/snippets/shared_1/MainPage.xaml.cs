namespace PlatformIntegration;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
	}

    private void Battery_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new BatteryTestPage());
    }

    private void Sensors_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new SensorsPage());

	}

    private void Details1_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new DeviceDetailsPage());
	}

    private void Data_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new DataPage());
    }

    private void AppModel_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new AppModelPage());
    }

    private void Comms_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CommsPage());
    }

    private void Reader_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ScreenReaderPage());
    }
}

