using MastodonLib.Models;
using ReactiveUI;

namespace MastodonMaui.ViewModels
{
    public class CurrentUserViewModel : ReactiveObject
    {
        internal Account Account { get; }

        internal CurrentUserViewModel(Account account)
        {
            Account = account;
        }
    }
}
