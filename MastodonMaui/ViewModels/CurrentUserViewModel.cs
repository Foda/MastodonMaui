using MastodonLib.Models;
using MastodonMaui.Services;
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
            Logout = ReactiveCommand.CreateFromTask(Logout_Impl);
        }

        private async Task Logout_Impl()
        {
            _siteInstance.TokenStore.ClearToken();

            await Shell.Current.GoToAsync(Navigation.BootstrapPageRoute);
        }
    }
}
