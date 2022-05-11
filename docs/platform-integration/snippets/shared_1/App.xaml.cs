namespace PlatformIntegration;

//<version_track>
public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		VersionTracking.Default.Track();

		MainPage = new AppShell();
	}
}
//</version_track>
