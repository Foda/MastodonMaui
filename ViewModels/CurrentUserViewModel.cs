using MastodonLib.Models;
using MastodonMaui.Services;
using MastodonMaui.Views;
using ReactiveUI;
using Splat;
using System.Reactive;

namespace MastodonMaui.ViewModels
{
    public class CurrentUserViewModel : ReactiveObject, ICurrentUserService
    {
        private readonly ISiteInstance _siteInstance;

        public Account Account { get; }

        public ReactiveCommand<Unit, Unit> Logout { get; }

        internal CurrentUserViewModel(Account account, ISiteInstance siteInstance = null)
        {
            _siteInstance = siteInstance ?? Locator.Current.GetService<ISiteInstance>();
            Account = account;

            Logout = ReactiveCommand.Create(Logout_Impl);
        }

        private void Logout_Impl()
        {
            _siteInstance.TokenStore.ClearToken();

            App.Current.MainPage = new NavigationPage(
                new BootstrapPage()
                {
                    ViewModel = new BootstrapViewModel()
                });
        }
    }
}
