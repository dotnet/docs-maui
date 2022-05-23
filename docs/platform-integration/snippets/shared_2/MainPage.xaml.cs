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

    private async void ActionButton_Clicked(object sender, EventArgs e)
    {
        //<app_actions>
        if (AppActions.Current.IsSupported)
        {
            await AppActions.Current.SetAsync(new[] { new AppAction("app_info", "App Info", icon: "app_info_action_icon"),
                                                      new AppAction("battery_info", "Battery Info") });
        }
        //</app_actions>
    }
}

