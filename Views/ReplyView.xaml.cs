using MastodonMaui.ViewModels;
using ReactiveUI;
using ReactiveUI.Maui;
using System.Reactive.Disposables;

namespace MastodonMaui.Views;

public partial class ReplyView : ReactiveContentView<ReplyViewModel>
{
    public ReplyView()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            this.OneWayBind(ViewModel, vm => vm.TargetStatus, v => v.ReplyToStatus.ViewModel)
                .DisposeWith(disposable);
            this.Bind(ViewModel, vm => vm.ReplyText, v => v.ReplyText.Text)
                .DisposeWith(disposable);
            this.BindCommand(ViewModel, vm => vm.SendReply, v => v.ReplyButton)
                .DisposeWith(disposable);
            this.BindCommand(ViewModel, vm => vm.CancelReply, v => v.CancelButton)
                .DisposeWith(disposable);
        });
    }
}