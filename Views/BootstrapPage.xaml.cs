using MastodonMaui.ViewModels;
using ReactiveUI;
using ReactiveUI.Maui;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MastodonMaui.Views;

public partial class BootstrapPage : ReactiveContentPage<BootstrapViewModel>
{
    public BootstrapPage()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            this.BindCommand(ViewModel, vm => vm.Login, v => v.LoginButton)
                .DisposeWith(disposable);

            this.Bind(ViewModel, vm => vm.SiteInstanceUrl, v => v.SiteInstance.Text)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.IsAttemptingNewLogin, v => v.SiteInstance.IsEnabled, val => !val)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.IsAttemptingNewLogin, v => v.LoginButton.IsEnabled, val => !val)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.IsAttemptingNewLogin, v => v.ActivitySpinner.IsRunning)
                .DisposeWith(disposable);

            // Hide login controls if we're checking for the existing login
            //this.OneWayBind(ViewModel, vm => vm.IsCheckingForExistingLogin, v => v.SiteInstance.IsVisible, val => !val)
            //    .DisposeWith(disposable);
            //this.OneWayBind(ViewModel, vm => vm.IsCheckingForExistingLogin, v => v.LoginButton.IsVisible, val => !val)
            //    .DisposeWith(disposable);
        });

        this.Loaded += BootstrapPage_Loaded;
    }

    private async void BootstrapPage_Loaded(object sender, EventArgs e)
    {
        this.Loaded -= BootstrapPage_Loaded;

        // Attempt to auto-login
        await ViewModel.LoginExisting.Execute();
    }
}