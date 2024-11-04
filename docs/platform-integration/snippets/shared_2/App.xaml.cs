namespace PlatformIntegration;

public partial class App : Application
{
    //<app_action_handler>
    public App()
    {
        InitializeComponent();

        AppActions.Current.AppActionActivated += App_AppActionActivated;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }    

    private void App_AppActionActivated(object sender, AppActionEventArgs e)
    {
        // If the app instance this code is running in is not the current app instance,
        // remove the handler and return.
        if (Application.Current != this && Application.Current is App app)
            AppActions.Current.AppActionActivated -= app.App_AppActionActivated;
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync($"//{e.AppAction.Id}");
            });
        }
    }
    //</app_action_handler>
}
