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
		count++;
		CounterLabel.Text = $"Current count: {count}";

		SemanticScreenReader.Announce(CounterLabel.Text);
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
}

