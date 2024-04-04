namespace PlatformIntegration;

public partial class BatteryTestPage : ContentPage
{
	public BatteryTestPage()
	{
		InitializeComponent();
	}

    //<watch_battery>
    private void BatterySwitch_Toggled(object sender, ToggledEventArgs e) =>
        WatchBattery();

    private bool _isBatteryWatched;

    private void WatchBattery()
    {
        
        if (!_isBatteryWatched)
        {
            Battery.Default.BatteryInfoChanged += Battery_BatteryInfoChanged;
        }
        else
        {
            Battery.Default.BatteryInfoChanged -= Battery_BatteryInfoChanged;
        }

        _isBatteryWatched = !_isBatteryWatched;
    }

    private void Battery_BatteryInfoChanged(object sender, BatteryInfoChangedEventArgs e)
    {
        BatteryStateLabel.Text = e.State switch
        {
            BatteryState.Charging => "Battery is currently charging",
            BatteryState.Discharging => "Charger is not connected and the battery is discharging",
            BatteryState.Full => "Battery is full",
            BatteryState.NotCharging => "The battery isn't charging.",
            BatteryState.NotPresent => "Battery is not available.",
            BatteryState.Unknown => "Battery is unknown",
            _ => "Battery is unknown"
        };
        
        BatteryLevelLabel.Text = $"Battery is {e.ChargeLevel * 100}% charged.";
    }

    //</watch_battery>

    //<energy_saver>
    private bool _isBatteryLow = false;

    private void BatterySaverSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        // Capture the initial state of the battery
        _isBatteryLow = Battery.Default.EnergySaverStatus == EnergySaverStatus.On;
        BatterySaverLabel.Text = _isBatteryLow.ToString();

        // Watch for any changes to the battery saver mode
        Battery.Default.EnergySaverStatusChanged += Battery_EnergySaverStatusChanged;
    }

    private void Battery_EnergySaverStatusChanged(object sender, EnergySaverStatusChangedEventArgs e)
    {
        // Update the variable based on the state
        _isBatteryLow = Battery.Default.EnergySaverStatus == EnergySaverStatus.On;
        BatterySaverLabel.Text = _isBatteryLow.ToString();
    }
    //</energy_saver>


    private void BatSourceButton_Clicked(object sender, EventArgs e)
    {
        SetChargeModeLabel();
    }

    //<charge_mode>
    private void SetChargeModeLabel()
    {
        BatteryPowerSourceLabel.Text = Battery.Default.PowerSource switch
        {
            BatteryPowerSource.Wireless => "Wireless charging",
            BatteryPowerSource.Usb => "USB cable charging",
            BatteryPowerSource.AC => "Device is plugged in to a power source",
            BatteryPowerSource.Battery => "Device isn't charging",
            _ => "Unknown"
        };
    }
    //</charge_mode>
}