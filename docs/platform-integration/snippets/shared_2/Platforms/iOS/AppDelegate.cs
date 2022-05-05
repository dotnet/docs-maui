using Foundation;

namespace PlatformIntegration;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    //<perform_action>
    public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
        => Microsoft.Maui.Platform.PerformActionForShortcutItem(application, shortcutItem, completionHandler);
    //</perform_action>
}
