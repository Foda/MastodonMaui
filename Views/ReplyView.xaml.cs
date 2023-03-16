using MastodonMaui.ViewModels;
using ReactiveUI;
using ReactiveUI.Maui;
using System.Reactive.Disposables;

namespace MastodonMaui.Views;

public partial class ReplyView : ReactiveContentView<ReplyViewModel>
{
    public static readonly BindableProperty IsReplyToStatusProperty = BindableProperty.Create(
            "IsReplyToStatus", typeof(bool), typeof(ReplyView), true, propertyChanged: IsReplyToStatusChanged);

    public bool IsReplyToStatus
    {
        get { return (bool)GetValue(IsReplyToStatusProperty); }
        set { SetValue(IsReplyToStatusProperty, value); }
    }

    private static void IsReplyToStatusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ReplyView view = bindable as ReplyView;
        if (view != null)
        {
            view.UpdateReplyControlsVisiblity((bool)newValue);
        }
    }

    public ReplyView()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            this.OneWayBind(ViewModel, vm => vm.ReplyToStatus, v => v.ReplyToStatus.ViewModel)
                .DisposeWith(disposable);
            this.BindCommand(ViewModel, vm => vm.CancelReply, v => v.CancelButton)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.CurrentUser.Account.AvatarUrl, v => v.Avatar.Source)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.VisibilitySettingList, v => v.PrivacyPicker.ItemsSource)
                .DisposeWith(disposable);
            this.Bind(ViewModel, vm => vm.Visibility, v => v.PrivacyPicker.SelectedItem)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.IsSendingReply, v => v.IsSendingReply.IsRunning)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.RemaingCharacters, v => v.ReplyCharactersRemaining.Text)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.RemaingCharacters, v => v.ReplyCharactersRemaining.TextColor,
                charCount => charCount < 0 ? Colors.Red : Colors.White)
                .DisposeWith(disposable);

            this.Bind(ViewModel, vm => vm.ReplyText, v => v.ReplyText.Text)
                .DisposeWith(disposable);

            this.BindCommand(ViewModel, vm => vm.SendReply, v => v.ReplyButton)
                .DisposeWith(disposable);
        });
    }

    internal void UpdateReplyControlsVisiblity(bool isReplyControlsVisible)
    {
        if (isReplyControlsVisible)
            return;
        else
        {
            // Timeline view style
            RootBorder.Padding = new Thickness(0, 0, 0, 0);
            RootBorder.Background = Colors.Transparent;
            RootBorder.StrokeThickness = 0;

            ReplyToStatus.IsVisible = false;

            ReplyText.Placeholder = "What's on your mind?";

            TextContainer.HeightRequest = 64;
            TextContainer.Margin = new Thickness(0, 16, 0, 0);

            ReplyButton.Text = "Publish!";
            CancelButton.IsVisible = false;
        }
    }
}