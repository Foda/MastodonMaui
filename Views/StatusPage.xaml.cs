using MastodonMaui.ViewModels;
using ReactiveUI;
using ReactiveUI.Maui;
using System.Reactive.Disposables;

namespace MastodonMaui.Views;

public partial class StatusPage : ReactiveContentPage<StatusPageViewModel>
{
	public StatusPage()
	{
		InitializeComponent();

        this.WhenActivated(disposable =>
        {
            this.OneWayBind(ViewModel, vm => vm.ParentStatus, v => v.ParentStatus)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.Replies, v => v.ReplyItems.ItemsSource)
                .DisposeWith(disposable);
        });
    }
}