using MastodonLib.Models;
using MastodonMaui.Services;
using MastodonMaui.ViewModels;
using MastodonMaui.Views;
using Splat;

namespace MastodonMaui;

public partial class AppShell : Shell
{
    internal AppShell(SiteInstanceService siteInstanceService, Account currentUser)
    {
        InitializeComponent();

        Locator.CurrentMutable.RegisterConstant(
            siteInstanceService, typeof(ISiteInstance));

        CurrentUser.ViewModel = new CurrentUserViewModel(currentUser, siteInstanceService);
        Locator.CurrentMutable.RegisterConstant(
            CurrentUser.ViewModel, typeof(ICurrentUserService));

        Routing.RegisterRoute(MastodonMaui.Navigation.StatusPageRoute, typeof(StatusPage));

        TimelinePage.Init(siteInstanceService);
    }
}
