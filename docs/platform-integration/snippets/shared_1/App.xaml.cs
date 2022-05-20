namespace PlatformIntegration;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
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
                await Application.Current.MainPage.Navigation.PopToRootAsync();
                await Application.Current.MainPage.Navigation.PushAsync(page);
            }
        });
    }
    //</appaction_handle>
}
