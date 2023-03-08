using MastodonMaui.ViewModels;
using ReactiveUI;
using ReactiveUI.Maui;
using System.Reactive.Disposables;

namespace MastodonMaui.Views;

public partial class CurrentUserView : ReactiveContentView<CurrentUserViewModel>
{
	public CurrentUserView()
	{
		InitializeComponent();

        this.WhenActivated(disposable =>
        {
            this.OneWayBind(ViewModel, vm => vm.Account.AvatarUrl, v => v.Avatar.Source)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.Account.DisplayName, v => v.DisplayName.Text)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.Account.UserName, v => v.Username.Text, username => $"@{username}")
                .DisposeWith(disposable);
        });
    }
}