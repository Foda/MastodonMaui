using MastodonLib.Models;
using MastodonMaui.Services;
using ReactiveUI;
using System.Reactive;

namespace MastodonMaui.ViewModels
{
    public class BootstrapViewModel : ReactiveObject
    {
        private const string SITE_INSTANCE_URL_KEY = "login_instance_url";

        private string _siteInstanceUrl;
        public string SiteInstanceUrl
        {
            get => _siteInstanceUrl;
            set => this.RaiseAndSetIfChanged(ref _siteInstanceUrl, value);
        }

        public ReactiveCommand<Unit, Unit> Login { get; }
        public ReactiveCommand<Unit, Unit> LoginExisting { get; }

        private bool _isAttemptingNewLogin;
        public bool IsAttemptingNewLogin
        {
            get => _isAttemptingNewLogin;
            private set => this.RaiseAndSetIfChanged(ref _isAttemptingNewLogin, value);
        }

        private bool _isCheckingForExistingLogin = true;
        public bool IsCheckingForExistingLogin
        {
            get => _isCheckingForExistingLogin;
            private set => this.RaiseAndSetIfChanged(ref _isCheckingForExistingLogin, value);
        }

        internal BootstrapViewModel()
        {
            Login = ReactiveCommand.CreateFromTask(AttemptLogin);
            Login.IsExecuting.ToProperty(
                this, vm => vm.IsAttemptingNewLogin, false, RxApp.MainThreadScheduler);

            LoginExisting = ReactiveCommand.CreateFromTask(AttemptLoginForExistingUser);
            LoginExisting.IsExecuting.ToProperty(
                this, vm => vm.IsCheckingForExistingLogin, false, RxApp.MainThreadScheduler);
        }

        private async Task AttemptLogin()
        {
            SiteInstanceService siteInstance = new(SiteInstanceUrl);

            try
            {
                var timeline = await siteInstance.Client.GetPublicTimeline();

                await CompleteBootstrap(siteInstance);
            }
            catch (Exception ex)
            {
                // TODO: Login failed
            }
        }

        private async Task AttemptLoginForExistingUser()
        {
            try
            {
                string existingSite = await SecureStorage.Default.GetAsync(SITE_INSTANCE_URL_KEY);
                if (!string.IsNullOrEmpty(existingSite))
                {
                    SiteInstanceService siteInstance = new(existingSite);
                    await CompleteBootstrap(siteInstance);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task CompleteBootstrap(SiteInstanceService siteInstance)
        {
            Account currentUser = await siteInstance.Client.GetCurrentUser();
            if (currentUser != null)
            {
                await SecureStorage.Default.SetAsync(SITE_INSTANCE_URL_KEY, siteInstance.InstanceUrl);

                App.Current.MainPage = new AppShell(siteInstance, currentUser);
            }
        }
    }
}
