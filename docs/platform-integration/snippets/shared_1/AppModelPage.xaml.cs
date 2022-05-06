namespace PlatformIntegration;

public partial class AppModelPage : ContentPage
{
	public AppModelPage()
	{
		InitializeComponent();
	}

    private void ReadAppInfoButton_Clicked(object sender, EventArgs e)
    {
		//<read_info>
		string name = AppInfo.Current.Name;
		string package = AppInfo.Current.PackageName;
		string version = AppInfo.Current.VersionString;
		string build = AppInfo.Current.BuildString;
		//</read_info>

		//<show_settings>
		AppInfo.Current.ShowSettingsUI();
		//</show_settings>
	}
}