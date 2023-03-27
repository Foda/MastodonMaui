using CommunityToolkit.Maui.Layouts;
using MastodonMaui.Services;
using MastodonMaui.ViewModels;
using ReactiveUI;
using ReactiveUI.Maui;
using Splat;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MastodonMaui.Views;

public partial class HomePage : ReactiveContentPage<HomePageViewModel>
{
    public HomePage()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            ISiteInstance siteInstance = Locator.Current.GetService<ISiteInstance>();
            ICurrentUserService currentUser = Locator.Current.GetService<ICurrentUserService>();

            this.ViewModel = new(siteInstance);
            this.TrendingView.ViewModel = new(siteInstance);
            this.NewPostView.ViewModel = new(null, siteInstance);
            this.CurrentUserView.ViewModel = new CurrentUserViewModel(currentUser.Account, siteInstance);
            this.CurrentStatus.ViewModel = new StatusPageViewModel();
            this.CurrentStatus.ViewModel.NavigateBack.Subscribe(_ =>
            {
                this.ViewModel.SelectedStatus = null;
            })
            .DisposeWith(disposable);

            this.BindCommand(ViewModel, vm => vm.RefreshTimeline, v => v.RefreshButton)
                .DisposeWith(disposable);

            this.Bind(ViewModel, vm => vm.SelectedStatus, v => v.StatusItems.SelectedItem)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.Items, v => v.StatusItems.ItemsSource)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.IsLoading, v => v.TimelineLoading.IsRunning)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.SelectedStatus, v => v.CurrentStatus.ViewModel.ParentStatus)
                .DisposeWith(disposable);

            this.WhenAnyValue(v => v.ViewModel.CurrentState)
                .Subscribe(async state =>
            {
                await StateContainer.ChangeStateWithAnimation(TimelineContainer,
                        state,
                        (element, token) => element.TranslateTo(-200, 0, 150, Easing.CubicIn).WaitAsync(token),
                        (element, token) => element.TranslateTo(0, 0, 350, Easing.CubicOut).WaitAsync(token),
                        CancellationToken.None);
            })
            .DisposeWith(disposable);
        });
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await ViewModel.RefreshTimeline.Execute();
    }
}