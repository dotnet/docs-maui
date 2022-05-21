namespace PlatformIntegration;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.ConfigureEssentials(essentials =>
			{
                essentials
                    .UseVersionTracking()
#if WINDOWS
					.UseMapServiceToken("YOUR-KEY-HERE")
#endif
                    .AddAppAction("app_info", "App Info", icon: "app_info_action_icon")
					.AddAppAction("battery_info", "Battery Info")
					.OnAppAction(App.HandleAppActions);
            })
			;

		return builder.Build();
	}

	private static class BootstrapVersionTracking
    {
        //<bootstrap_versiontracking>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureEssentials(essentials =>
                {
                    essentials.UseVersionTracking();
                });

            return builder.Build();
        }
        //</bootstrap_versiontracking>
    }

    private static class BootstrapAppAction
    {
        //<bootstrap_appaction>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureEssentials(essentials =>
                {
                    essentials
                        .AddAppAction("app_info", "App Info", icon: "app_info_action_icon")
                        .AddAppAction("battery_info", "Battery Info")
                        .OnAppAction(App.HandleAppActions);
                });

            return builder.Build();
        }
        //</bootstrap_appaction>
    }

    private static class BootstrapMapToken
    {
        //<bootstrap_maptoken>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureEssentials(essentials =>
                {
                    essentials.UseMapServiceToken("YOUR-API-TOKEN");
                });

            return builder.Build();
        }
        //</bootstrap_maptoken>
    }
}
}
