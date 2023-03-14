using MastodonLib.Models;
using MastodonMaui.Services;
using MastodonMaui.ViewModels;
using MastodonMaui.Views;

namespace MastodonMaui;

public partial class AppShell : Shell
{
    private SiteInstanceService _siteInstanceService;

    internal AppShell(SiteInstanceService siteInstanceService, Account currentUser)
    {
        InitializeComponent();

        _siteInstanceService = siteInstanceService;

        TimelinePage.SetInstance(_siteInstanceService);
        CurrentUser.ViewModel = new CurrentUserViewModel(currentUser);

        Routing.RegisterRoute(MastodonMaui.Navigation.StatusPageRoute, typeof(StatusPage));
    }
}
