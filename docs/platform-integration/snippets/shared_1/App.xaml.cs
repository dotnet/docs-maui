namespace PlatformIntegration;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    //<appaction_handle>
    public static void HandleAppActions(AppAction appAction)
    {
        App.Current.Dispatcher.Dispatch(async () =>
        {
            var page = appAction.Id switch
            {
                "battery_info" => new SensorsPage(),
                "app_info" => new AppModelPage(),
                _ => default(Page)
            };

            if (page != null)
            {
                // Assume an app with a single window.
                await Application.Current.Windows[0].Page.Navigation.PopToRootAsync();
                await Application.Current.Windows[0].Page.Navigation.PushAsync(page);
            }
        });
    }
    //</appaction_handle>
}
