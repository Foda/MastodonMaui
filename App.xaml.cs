using MastodonMaui.ViewModels;
using MastodonMaui.Views;

namespace MastodonMaui;

public partial class App : Application
{
	private BootstrapViewModel _bootstrapViewModel;

	public App()
	{
		InitializeComponent();

        _bootstrapViewModel = new BootstrapViewModel();
        _bootstrapViewModel.OnBootstrapComplete += _bootstrapViewModel_OnBootstrapComplete;

        MainPage = new NavigationPage(
            new BootstrapPage()
            {
                ViewModel = _bootstrapViewModel
            });
    }

    private void _bootstrapViewModel_OnBootstrapComplete(object sender, BootstrapCompleteEventArgs e)
    {
        _bootstrapViewModel.OnBootstrapComplete -= _bootstrapViewModel_OnBootstrapComplete;

        MainPage = new AppShell(e.SiteInstance, e.Account);
    }
}
