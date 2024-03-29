using MastodonMaui.ViewModels;
using ReactiveUI;
using ReactiveUI.Maui;
using System.Reactive.Disposables;

namespace MastodonMaui.Views;

public partial class StatusPage : ReactiveContentView<StatusPageViewModel>
{
    public StatusPage()
    {
        InitializeComponent();

        this.ViewModel = new StatusPageViewModel();
        this.WhenActivated(disposable =>
        {
            this.OneWayBind(ViewModel, vm => vm.ParentStatus, v => v.ParentStatus.ViewModel)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.Replies, v => v.ReplyItems.ItemsSource)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.IsLoading, v => v.IsLoading.IsRunning)
                .DisposeWith(disposable);

            this.BindCommand(ViewModel, vm => vm.NavigateBack, v => v.BackButton)
                .DisposeWith(disposable);
        });
    }
}