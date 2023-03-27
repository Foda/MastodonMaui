using MastodonMaui.ViewModels;
using ReactiveUI.Maui;
using System.Reactive.Linq;

namespace MastodonMaui.Views;

public partial class TrendingView : ReactiveContentView<TrendingViewModel>
{
	public TrendingView()
	{
		InitializeComponent();
        this.Loaded += TrendingView_Loaded;
	}

    private async void TrendingView_Loaded(object sender, EventArgs e)
    {
        this.Loaded -= TrendingView_Loaded;

        await ViewModel.RefreshTrending.Execute();

        BindableLayout.SetItemsSource(Trending, ViewModel.TrendingTags);
    }
}