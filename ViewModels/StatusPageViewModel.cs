using MastodonMaui.Services;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace MastodonMaui.ViewModels
{
    public class StatusPageViewModel : ReactiveObject
    {
        internal ISiteInstance SiteInstance { get; }

        public StatusViewModel ParentStatus { get; }
        public ObservableCollection<StatusViewModel> Replies { get; }

        internal StatusPageViewModel(StatusViewModel parentStatus, ISiteInstance siteInstance)
        {
            ParentStatus = parentStatus;
            SiteInstance = siteInstance;
        }
    }
}
