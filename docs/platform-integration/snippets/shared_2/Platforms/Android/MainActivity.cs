using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace PlatformIntegration;

//<intent_filter_1>
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
[IntentFilter(actions: new[] { Microsoft.Maui.ApplicationModel.Platform.Intent.ActionAppAction },
              Categories = new[] { global::Android.Content.Intent.CategoryDefault })]
public class MainActivity : MauiAppCompatActivity
{
//</intent_filter_1>

    //<intent_implementation>
    protected override void OnResume()
    {
        base.OnResume();

        Platform.OnResume(this);
    }

    protected override void OnNewIntent(Intent intent)
    {
        base.OnNewIntent(intent);

        Platform.OnNewIntent(intent);
    }
    //</intent_implementation>

//<intent_filter_2>
}
//</intent_filter_2>
