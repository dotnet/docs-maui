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

    private void ReadThemeButton_Clicked(object sender, EventArgs e)
    {
        //<read_theme>
        ThemeInfoLabel.Text = AppInfo.Current.RequestedTheme switch
        {
            AppTheme.Dark => "Dark theme",
            AppTheme.Light => "Light theme",
            _ => "Unknown"
        };
        //</read_theme>
    }

    private async void OpenRideShareButton_Clicked(object sender, EventArgs e)
    {
        //<launcher_open>
        bool supportsUri = await Launcher.Default.CanOpenAsync("lyft://");

        if (supportsUri)
            await Launcher.Default.OpenAsync("lyft://ridetype?id=lyft_line");
        //</launcher_open>
    }

    private async void OpenRideShare2Button_Clicked(object sender, EventArgs e)
    {
        //<launcher_open_try>
        bool launcherOpened = await Launcher.Default.TryOpenAsync("lyft://ridetype?id=lyft_line");

        if (launcherOpened)
        {
            // Do something fun
        }
        //</launcher_open_try>
    }

    private async void OpenFileButton_Clicked(object sender, EventArgs e)
    {
        //<launcher_open_file>
        string popoverTitle = "Read text file";
        string name = "File.txt";
        string file = System.IO.Path.Combine(FileSystem.CacheDirectory, name);

        System.IO.File.WriteAllText(file, "Hello World");

        await Launcher.Default.OpenAsync(new OpenFileRequest(popoverTitle, new ReadOnlyFile(file)));
        //</launcher_open_file>
    }

    public void RunCode1()
    {
        //<runcode_lambda>
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Code to run on the main thread
        });
        //</runcode_lambda>
    }

    public void RunCode2()
    {
        //<runcode_func_pointer>
        void MyMainThreadCode()
        {
            // Code to run on the main thread
        }

        MainThread.BeginInvokeOnMainThread(MyMainThreadCode);
        //</runcode_func_pointer>
    }

    public void RunCode3()
    {
        void MyMainThreadCode()
        {
            // Code to run on the main thread
        }

        //<runcode_test_thread>
        if (MainThread.IsMainThread)
            MyMainThreadCode();

        else
            MainThread.BeginInvokeOnMainThread(MyMainThreadCode);
        //</runcode_test_thread>
    }

    private void Navigate1_Clicked(object sender, EventArgs e)
    {
        NavigateToBuilding25().Wait();
    }

    public async Task TryOpenMap()
    {
        //<navigate_tryopen>
        var location = new Location(47.645160, -122.1306032);
        var options = new MapLaunchOptions { Name = "Microsoft Building 25" };

        if (await Map.Default.TryOpenAsync(location, options) == false)
        {
            // Map failed to open
        }
        //</navigate_tryopen>
    }

    //<navigate_building>
    public async Task NavigateToBuilding25()
    {
        var location = new Location(47.645160, -122.1306032);
        var options = new MapLaunchOptions { Name = "Microsoft Building 25" };

        try
        {
            await Map.Default.OpenAsync(location, options);
        }
        catch (Exception ex)
        {
            // No map application available to open
        }
    }
    //</navigate_building>

    //<navigate_building_placemark>
    public async Task NavigateToBuilding()
    {
        var placemark = new Placemark
        {
            CountryName = "United States",
            AdminArea = "WA",
            Thoroughfare = "Microsoft Building 25",
            Locality = "Redmond"
        };
        var options = new MapLaunchOptions { Name = "Microsoft Building 25" };

        try
        {
            await Map.Default.OpenAsync(placemark, options);
        }
        catch (Exception ex)
        {
            // No map application available to open or placemark can not be located
        }
    }
    //</navigate_building_placemark>

    //<navigate_building_placemark_extension>
    public async Task NavigateToBuildingByPlacemark()
    {
        var placemark = new Placemark
        {
            CountryName = "United States",
            AdminArea = "WA",
            Thoroughfare = "Microsoft Building 25",
            Locality = "Redmond"
        };

        var options = new MapLaunchOptions { Name = "Microsoft Building 25" };

        try
        {
            await placemark.OpenMapsAsync(options);
        }
        catch (Exception ex)
        {
            // No map application available to open or placemark can not be located
        }
    }
    //</navigate_building_placemark_extension>

    //<navigate_building_driving>
    public async Task DriveToBuilding25()
    {
        var location = new Location(47.645160, -122.1306032);
        var options = new MapLaunchOptions { Name = "Microsoft Building 25",
                                             NavigationMode = NavigationMode.Driving };

        try
        {
            await Map.Default.OpenAsync(location, options);
        }
        catch (Exception ex)
        {
            // No map application available to open
        }
    }
    //</navigate_building_driving>

    //<browser_open>
    private async void BrowserOpen_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://www.microsoft.com");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            // An unexpected error occurred. No browser may be installed on the device.
        }
    }
    //</browser_open>

    //<browser_open_custom>
    private async void BrowserCustomOpen_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://www.microsoft.com");
            BrowserLaunchOptions options = new BrowserLaunchOptions()
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Colors.Violet,
                PreferredControlColor = Colors.SandyBrown
            };

            await Browser.Default.OpenAsync(uri, options);
        }
        catch (Exception ex)
        {
            // An unexpected error occurred. No browser may be installed on the device.
        }
    }
    //</browser_open_custom>

    private async void CheckLocationPermission()
    {
        try
        {
            //<permission_check>
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            //</permission_check>
            switch (status)
            {
                case PermissionStatus.Unknown:
                    break;
                case PermissionStatus.Denied:
                    break;
                case PermissionStatus.Disabled:
                    break;
                case PermissionStatus.Granted:
                    break;
                case PermissionStatus.Restricted:
                    break;
                case PermissionStatus.Limited:
                    break;
                default:
                    break;
            }
        }
        catch (PermissionException ex)
        {

            // Permission not declared
        }
    }

    private async void RequestLocationPermission()
    {
        try
        {
            //<permission_request>
            PermissionStatus status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            //</permission_request>
            switch (status)
            {
                case PermissionStatus.Unknown:
                    break;
                case PermissionStatus.Denied:
                    break;
                case PermissionStatus.Disabled:
                    break;
                case PermissionStatus.Granted:
                    break;
                case PermissionStatus.Restricted:
                    break;
                case PermissionStatus.Limited:
                    break;
                default:
                    break;
            }
        }
        catch (PermissionException ex)
        {

            // Permission not declared
        }
    }

    //<permission_check_and_request>
    public async Task<PermissionStatus> CheckAndRequestLocationPermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status == PermissionStatus.Granted)
            return status;

        if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
        {
            // Prompt the user to turn on in settings
            // On iOS once a permission has been denied it may not be requested again from the application
            return status;
        }

        if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
        {
            // Prompt the user with additional information as to why the permission is needed
        }

        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        return status;
    }
    //</permission_check_and_request>

    // The following code was removed from the article. I felt like it confused the previous code example without
    // adding anything much to the concept.
    //<permission_check_and_request_usage>
    public async Task<Location> GetLocationAsync()
    {
        PermissionStatus status = await CheckAndRequestPermissionAsync(new Permissions.LocationWhenInUse());
        if (status != PermissionStatus.Granted)
        {
            // Notify user permission was denied
            return null;
        }

        return await Geolocation.GetLocationAsync();
    }

    public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
                where T : Permissions.BasePermission
    {
        PermissionStatus status = await permission.CheckStatusAsync();

        if (status != PermissionStatus.Granted)
            status = await permission.RequestAsync();

        return status;
    }
    //<permission_check_and_request_usage>

    //<permission_class>
    public class MyPermission : Permissions.BasePermission
    {
        // This method checks if current status of the permission.
        public override Task<PermissionStatus> CheckStatusAsync()
        {
            throw new System.NotImplementedException();
        }

        // This method is optional and a PermissionException is often thrown if a permission is not declared.
        public override void EnsureDeclared()
        {
            throw new System.NotImplementedException();
        }

        // Requests the user to accept or deny a permission.
        public override Task<PermissionStatus> RequestAsync()
        {
            throw new System.NotImplementedException();
        }

        // Indicates that the requestor should prompt the user as to why the app requires the permission, because the
        // user has previously denied this permission.
        public override bool ShouldShowRationale()
        {
            throw new NotImplementedException();
        }
    }
    //</permission_class>

    //public class ReadWriteStoragePerms : Permissions.BasePlatformPermission
    //{
    //    protected override Func<IEnumerable<string>> RequiredDeclarations => () =>
    //        new[] { "ReadExternalStorage", "WriteExternalStorage" }; // TODO: Need correct values here.
    //}

#if ANDROID
    //<permission_readwrite>
    public class ReadWriteStoragePerms : Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string androidPermission, bool isRuntime)>
            {
            (global::Android.Manifest.Permission.ReadExternalStorage, true),
            (global::Android.Manifest.Permission.WriteExternalStorage, true)
            }.ToArray();
    }
    //</permission_readwrite>
#elif WINDOWS
    public class ReadWriteStoragePerms : Permissions.BasePlatformPermission
    {
        protected override Func<IEnumerable<string>> RequiredDeclarations => () =>
            new[] { "ReadExternalStorage", "WriteExternalStorage" }; // TODO: Need correct values here.
    }
#elif MACCATALYST
    public class ReadWriteStoragePerms : Permissions.BasePlatformPermission
    {
        protected override Func<IEnumerable<string>> RequiredInfoPlistKeys => () =>
            new[] { "ReadExternalStorage", "WriteExternalStorage" }; // TODO: Need correct values here.
    }
#elif IOS
    public class ReadWriteStoragePerms : Permissions.BasePlatformPermission
    {
        protected override Func<IEnumerable<string>> RequiredInfoPlistKeys => () =>
            new[] { "ReadExternalStorage", "WriteExternalStorage" }; // TODO: Need correct values here.
    }
#endif

    private async void RequestCustomPermission()
    {
        try
        {
            //<permission_readwrite_request>
            PermissionStatus status = await Permissions.RequestAsync<ReadWriteStoragePerms>();
            //</permission_readwrite_request>
            switch (status)
            {
                case PermissionStatus.Unknown:
                    break;
                case PermissionStatus.Denied:
                    break;
                case PermissionStatus.Disabled:
                    break;
                case PermissionStatus.Granted:
                    break;
                case PermissionStatus.Restricted:
                    break;
                case PermissionStatus.Limited:
                    break;
                default:
                    break;
            }
        }
        catch (PermissionException ex)
        {

            // Permission not declared
        }
    }


    //<version_read>
    private void ReadVersion_Clicked(object sender, EventArgs e)
    {
        labelIsFirst.Text = VersionTracking.Default.IsFirstLaunchEver.ToString();
        labelCurrentVersionIsFirst.Text = VersionTracking.Default.IsFirstLaunchForCurrentVersion.ToString();
        labelCurrentBuildIsFirst.Text = VersionTracking.Default.IsFirstLaunchForCurrentBuild.ToString();
        labelCurrentVersion.Text = VersionTracking.Default.CurrentVersion.ToString();
        labelCurrentBuild.Text = VersionTracking.Default.CurrentBuild.ToString();
        labelFirstInstalledVer.Text = VersionTracking.Default.FirstInstalledVersion.ToString();
        labelFirstInstalledBuild.Text = VersionTracking.Default.FirstInstalledBuild.ToString();
        labelVersionHistory.Text = String.Join(',', VersionTracking.Default.VersionHistory);
        labelBuildHistory.Text = String.Join(',', VersionTracking.Default.BuildHistory);

        // These two properties may be null if this is the first version
        labelPreviousVersion.Text = VersionTracking.Default.PreviousVersion?.ToString() ?? "none";
        labelPreviousBuild.Text = VersionTracking.Default.PreviousBuild?.ToString() ?? "none";
    }
    //</version_read>

}
