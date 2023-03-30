using MastodonLib.Models;
using MastodonMaui.Services;
using ReactiveUI;
using Splat;
using System.Reactive;
using System.Reactive.Linq;

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
        public ReactiveCommand<Unit, bool> LoginExisting { get; }

        private ObservableAsPropertyHelper<bool> _isAttemptingLogin;
        public bool IsAttemptingLogin => _isAttemptingLogin.Value;

        private ObservableAsPropertyHelper<bool> _isCheckingForExistingLogin;
        public bool IsCheckingForExistingLogin => _isCheckingForExistingLogin.Value;

        internal BootstrapViewModel()
        {
            Login = ReactiveCommand.CreateFromTask(AttemptLogin);
            LoginExisting = ReactiveCommand.CreateFromTask(AttemptLoginForExistingUser);

            _isAttemptingLogin = Login.IsExecuting.Merge(LoginExisting.IsExecuting)
                .ToProperty(this, vm => vm.IsAttemptingLogin, false, RxApp.MainThreadScheduler);

            _isCheckingForExistingLogin = LoginExisting.IsExecuting
                .ToProperty(this, vm => vm.IsCheckingForExistingLogin, false, RxApp.MainThreadScheduler);
        }

        private async Task AttemptLogin()
        {
            SiteInstanceService siteInstance = new(SiteInstanceUrl);
            try
            {
                await CompleteBootstrap(siteInstance);
            }
            catch (Exception ex)
            {
                // TODO: Login failed
            }
        }

        private async Task<bool> AttemptLoginForExistingUser()
        {
            try
            {
                string existingSite = await SecureStorage.Default.GetAsync(SITE_INSTANCE_URL_KEY);
                if (!string.IsNullOrEmpty(existingSite))
                {
                    SiteInstanceService siteInstance = new(existingSite);
                    return await CompleteBootstrap(siteInstance);
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        private async Task<bool> CompleteBootstrap(SiteInstanceService siteInstance)
        {
            Account currentUser = await siteInstance.Client.GetCurrentUser();
            if (currentUser != null)
            {
                await SecureStorage.Default.SetAsync(SITE_INSTANCE_URL_KEY, siteInstance.InstanceUrl);

                Locator.CurrentMutable.RegisterConstant(siteInstance, typeof(ISiteInstance));
                Locator.CurrentMutable.RegisterConstant(
                    new CurrentUserViewModel(currentUser, siteInstance), typeof(ICurrentUserService));

                await Shell.Current.GoToAsync(MastodonMaui.Navigation.HomePageRoute);

                return true;
            }
            return false;
        }
    }
}
