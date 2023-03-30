using CommunityToolkit.Maui;
using MastodonMaui.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

#if WINDOWS
using WinUIEx;
#endif

namespace MastodonMaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Segoe MDL2 Assets.ttf", "SegoeMDL2");
                fonts.AddFont("SF-Pro.ttf", "SF-Pro");

                fonts.AddFont("Segoe-UI-Variable-Static-Display.ttf", "SegoeUIDisplay");
                fonts.AddFont("Segoe-UI-Variable-Static-Display-Bold.ttf", "SegoeUIDisplayBold");
                fonts.AddFont("Segoe-UI-Variable-Static-Display-Semibold.ttf", "SegoeUIDisplaySemibold");
                fonts.AddFont("Segoe-UI-Variable-Static-Display-Semilight.ttf", "SegoeUIDisplaySemilight");
                fonts.AddFont("Segoe-UI-Variable-Static-Small.ttf", "SegoeUIDisplaySmall");

            })
#if WINDOWS
                 .ConfigureLifecycleEvents(events =>
                 {
                     events.AddWindows(wndLifeCycleBuilder =>
                     {
                         wndLifeCycleBuilder.OnWindowCreated(window =>
                         {
                             var manager = WinUIEx.WindowManager.Get(window);
                             manager.PersistenceId = "Masto_MainWindowPersistanceId";
                             manager.MinWidth = 750;
                             manager.MinHeight = 512;
                             manager.Backdrop = new MicaSystemBackdrop();
                         });
                     });
                 })
#endif
            ;

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
