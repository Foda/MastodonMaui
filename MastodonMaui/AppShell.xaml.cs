using MastodonMaui.Views;

namespace MastodonMaui;

public partial class AppShell : Shell
{
    internal AppShell()
    {
        InitializeComponent();

        BootstrapPage.ViewModel = new ViewModels.BootstrapViewModel();

        Routing.RegisterRoute(MastodonMaui.Navigation.BootstrapPageRoute, typeof(BootstrapPage));
        Routing.RegisterRoute(MastodonMaui.Navigation.StatusPageRoute, typeof(StatusPage));
        Routing.RegisterRoute(MastodonMaui.Navigation.HomePageRoute, typeof(HomePage));
    }
}
