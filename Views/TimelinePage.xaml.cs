using MastodonMaui.Services;
using MastodonMaui.ViewModels;
using ReactiveUI;
using ReactiveUI.Maui;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MastodonMaui.Views;

public partial class TimelinePage : ReactiveContentPage<TimelineViewModel>
{
    public TimelinePage()
    {
        InitializeComponent();
    }

    internal void SetInstance(SiteInstanceService siteInstanceService)
    {
        this.ViewModel = new TimelineViewModel(siteInstanceService);

        this.WhenActivated(disposable =>
        {
            this.BindCommand(ViewModel, vm => vm.RefreshTimeline, v => v.RefreshButton)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.Items, v => v.StatusItems.ItemsSource)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.IsLoading, v => v.TimelineLoading.IsRunning)
                .DisposeWith(disposable);
        });

        this.Loaded += TimelinePage_Loaded;
    }

    private async void TimelinePage_Loaded(object sender, EventArgs e)
    {
        this.Loaded -= TimelinePage_Loaded;
        await ViewModel.RefreshTimeline.Execute();
    }
}