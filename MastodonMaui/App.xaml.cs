using MastodonMaui.ViewModels;
using MastodonMaui.Views;

namespace MastodonMaui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        MainPage = new AppShell();
    }
}
