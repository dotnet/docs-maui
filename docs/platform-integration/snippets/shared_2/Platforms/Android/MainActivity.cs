using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace PlatformIntegration;

//<intent_filter_1>
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
[IntentFilter(new[] { Platform.Intent.ActionAppAction },
              Categories = new[] { global::Android.Content.Intent.CategoryDefault })]
public class MainActivity : MauiAppCompatActivity {

    protected override void OnResume()
    {
        base.OnResume();

        Platform.OnResume(this);
    }

    protected override void OnNewIntent(Android.Content.Intent intent)
    {
        base.OnNewIntent(intent);

        Platform.OnNewIntent(intent);
    }

}
//</intent_filter_1>
{

}
