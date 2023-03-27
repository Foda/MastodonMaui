using MastodonMaui.Services;
using ReactiveUI;

namespace MastodonMaui.ViewModels
{
    public class NotificationsViewModel : ReactiveObject
    {
        private readonly ISiteInstance _siteInstance;

        internal NotificationsViewModel(ISiteInstance siteInstance)
        {
            _siteInstance = siteInstance;
        }
    }
}
