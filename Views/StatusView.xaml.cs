using MastodonMaui.ViewModels;
using ReactiveUI;
using ReactiveUI.Maui;
using System.Reactive.Disposables;
using CommunityToolkit.Maui.Views;

namespace MastodonMaui.Views;

public partial class StatusView : ReactiveContentView<StatusViewModel>
{
	public StatusView()
	{
		InitializeComponent();

        this.WhenActivated(disposable =>
        {
            this.OneWayBind(ViewModel, vm => vm.Account.AvatarUrl, v => v.Avatar.Source)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.Account.DisplayName, v => v.DisplayName.Text)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.Account.UserName, v => v.UserName.Text, username => $"@{username}")
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.CreatedAt, v => v.CreatedAt.Text)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.Content, v => v.Content.Text)
                .DisposeWith(disposable);

            // Reblog
            this.OneWayBind(ViewModel, vm => vm.IsReblog, v => v.ReblogIcon.IsVisible)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.IsReblog, v => v.ReblogDisplayName.IsVisible)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.ReblogHint, v => v.ReblogDisplayName.Text)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.IsReblog, v => v.ReblogRow.Height, isReblog => isReblog ? 32 : 0)
                .DisposeWith(disposable);

            // Status counters
            this.OneWayBind(ViewModel, vm => vm.ReblogCount, v => v.ReblogCount.Text)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.FavoriteCount, v => v.FavoriteCount.Text)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.RepliesCount, v => v.ReplyCount.Text)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.Replies, v => v.ReplyMarker.IsVisible,
                replies => replies.Count > 0)
                .DisposeWith(disposable);

            this.BindCommand(ViewModel, vm => vm.ToggleFavorited, v => v.FavoriteButton)
                .DisposeWith(disposable);
            this.BindCommand(ViewModel, vm => vm.ToggleReblogged, v => v.ReblogButton)
                .DisposeWith(disposable);
        });
    }

    private void ReplyButton_Clicked(object sender, EventArgs e)
    {
        Popup popup = new();
        
        ReplyViewModel replyVM = new(ViewModel, ViewModel.SiteInstance);
        replyVM.CancelReply.Subscribe(_ =>
        {
            popup.Close();
        });

        popup.Content = new ReplyView()
        {
            ViewModel = replyVM
        };
        App.Current.MainPage.ShowPopup(popup);
    }
}