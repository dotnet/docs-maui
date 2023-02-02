namespace PlatformIntegration;

public partial class DeviceDetailsPage : ContentPage
{
	public DeviceDetailsPage()
	{
		InitializeComponent();
	}

	private void ContentPage_Loaded(object sender, EventArgs e)
    {
		ReadDeviceDisplay();
		ReadDeviceInfo();
	}

	//<main_display>
	private void ReadDeviceDisplay()
    {
		System.Text.StringBuilder sb = new System.Text.StringBuilder();

		sb.AppendLine($"Pixel width: {DeviceDisplay.Current.MainDisplayInfo.Width} / Pixel Height: {DeviceDisplay.Current.MainDisplayInfo.Height}");
		sb.AppendLine($"Density: {DeviceDisplay.Current.MainDisplayInfo.Density}");
		sb.AppendLine($"Orientation: {DeviceDisplay.Current.MainDisplayInfo.Orientation}");
		sb.AppendLine($"Rotation: {DeviceDisplay.Current.MainDisplayInfo.Rotation}");
		sb.AppendLine($"Refresh Rate: {DeviceDisplay.Current.MainDisplayInfo.RefreshRate}");

		DisplayDetailsLabel.Text = sb.ToString();
    }
	//</main_display>

	//<read_info>
	private void ReadDeviceInfo()
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();

		sb.AppendLine($"Model: {DeviceInfo.Current.Model}");
		sb.AppendLine($"Manufacturer: {DeviceInfo.Current.Manufacturer}");
		sb.AppendLine($"Name: {DeviceInfo.Current.Name}");
		sb.AppendLine($"OS Version: {DeviceInfo.Current.VersionString}");
		sb.AppendLine($"Idiom: {DeviceInfo.Current.Idiom}");
		sb.AppendLine($"Platform: {DeviceInfo.Current.Platform}");

		//<device_type>
		bool isVirtual = DeviceInfo.Current.DeviceType switch
        {
            DeviceType.Physical => false,
            DeviceType.Virtual => true,
            _ => false
        };
		//</device_type>

		sb.AppendLine($"Virtual device? {isVirtual}");

		DisplayDeviceLabel.Text = sb.ToString();
	}
	//</read_info>

	//<check_for_android>
	private bool IsAndroid() =>
		DeviceInfo.Current.Platform == DevicePlatform.Android;
	//</check_for_android>


	//<always_on>
	private void AlwaysOnSwitch_Toggled(object sender, ToggledEventArgs e) =>
		DeviceDisplay.Current.KeepScreenOn = AlwaysOnSwitch.IsToggled;
	//</always_on>

	//<idiom>
	private void PrintIdiom()
    {
		if (DeviceInfo.Current.Idiom == DeviceIdiom.Desktop)
			Console.WriteLine("The current device is a desktop");
		else if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone)
			Console.WriteLine("The current device is a phone");
		else if (DeviceInfo.Current.Idiom == DeviceIdiom.Tablet)
			Console.WriteLine("The current device is a Tablet");
	}
	//</idiom>

	//<flashlight>
	private async void FlashlightSwitch_Toggled(object sender, ToggledEventArgs e)
	{
		try
		{
			if (FlashlightSwitch.IsToggled)
				await Flashlight.Default.TurnOnAsync();
			else
				await Flashlight.Default.TurnOffAsync();
		}
		catch (FeatureNotSupportedException ex)
		{
			// Handle not supported on device exception
		}
		catch (PermissionException ex)
		{
			// Handle permission exception
		}
		catch (Exception ex)
		{
			// Unable to turn on/off flashlight
		}
	}
	//</flashlight>

	//<hapticfeedback>
	private void HapticShortButton_Clicked(object sender, EventArgs e) =>
		HapticFeedback.Default.Perform(HapticFeedbackType.Click);

	private void HapticLongButton_Clicked(object sender, EventArgs e) =>
		HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
	//</hapticfeedback>


	//<vibrate>
	private void VibrateStartButton_Clicked(object sender, EventArgs e)
	{
		int secondsToVibrate = Random.Shared.Next(1, 7);
		TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);
		
		Vibration.Default.Vibrate(vibrationLength);
	}

	private void VibrateStopButton_Clicked(object sender, EventArgs e) =>
		Vibration.Default.Cancel();
    //</vibrate>
}
