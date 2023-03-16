using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using MastodonMaui.ViewModels;
using ReactiveUI;
using ReactiveUI.Maui;
using System.Reactive.Disposables;

namespace MastodonMaui.Views;

public partial class StatusView : ReactiveContentView<StatusViewModel>
{
    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(
            "IsReadOnly", typeof(bool), typeof(StatusView), false, propertyChanged: OnIsReadOnlyChanged);

    public bool IsReadOnly
    {
        get { return (bool)GetValue(IsReadOnlyProperty); }
        set { SetValue(IsReadOnlyProperty, value); }
    }

    private static void OnIsReadOnlyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        StatusView view = bindable as StatusView;
        if (view != null)
        {
            view.StatusActionBar.IsVisible = !(bool)newValue;
        }
    }

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
            this.OneWayBind(ViewModel, vm => vm.ReblogCount, v => v.ReblogButton.Text)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.FavoriteCount, v => v.FavoriteButton.Text)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.RepliesCount, v => v.ReplyButton.Text)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.DidReblog, v => v.ReblogGlyph.Color,
                didReblog => didReblog ? Color.FromArgb("#FF6CCB5F") : Color.FromArgb("#FFFFFFFF"))
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.DidFavorite, v => v.FavoriteGlyph.Color,
                didFav => didFav ? Color.FromArgb("#FFF91880") : Color.FromArgb("#FFFFFFFF"))
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.DidFavorite, v => v.FavoriteGlyph.Glyph,
                didFav => didFav ? "\uEB52" : "\uEB51")
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.Replies, v => v.ReplyMarker.IsVisible,
                replies => replies.Count > 0)
                .DisposeWith(disposable);

            this.BindCommand(ViewModel, vm => vm.ToggleFavorited, v => v.FavoriteButton)
                .DisposeWith(disposable);
            this.BindCommand(ViewModel, vm => vm.ToggleReblogged, v => v.ReblogButton)
                .DisposeWith(disposable);
        });

        StatusActionBar.IsVisible = !IsReadOnly;
    }

    private void ReplyButton_Clicked(object sender, EventArgs e)
    {
        Popup popup = new()
        {
            Color = Colors.Transparent,
            CanBeDismissedByTappingOutsideOfPopup = false
        };
        
        ReplyViewModel replyVM = new(ViewModel);
        replyVM.SendReply.Subscribe(async newPost =>
        {
            if (newPost != null)
            {
                popup.Close();
                
                var toast = Toast.Make("Your post was sent",
                    CommunityToolkit.Maui.Core.ToastDuration.Short);
                await toast.Show();
            }
        });
        replyVM.CancelReply.Subscribe(_ =>
        {
            popup.Close();
        });

        popup.Content = new ReplyView()
        {
            ViewModel = replyVM,
            WidthRequest = 500,
            HeightRequest = 450
        };
        App.Current.MainPage.ShowPopup(popup);
    }
}