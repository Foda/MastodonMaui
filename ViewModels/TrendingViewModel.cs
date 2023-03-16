using MastodonLib.Models;
using MastodonMaui.Services;
using ReactiveUI;
using Splat;
using System.Collections.ObjectModel;
using System.Reactive;

namespace MastodonMaui.ViewModels
{
    public class TrendingViewModel : ReactiveObject
    {
        private readonly ISiteInstance _siteInstance;

        public ObservableCollection<Tag> TrendingTags { get; }
        public ReactiveCommand<Unit, Unit> RefreshTrending { get; }

        internal TrendingViewModel(ISiteInstance siteInstance = null)
        {
            _siteInstance = siteInstance ?? Locator.Current.GetService<ISiteInstance>();
            TrendingTags = new ObservableCollection<Tag>();
            RefreshTrending = ReactiveCommand.CreateFromTask(RefreshTrending_Impl);
        }

        private async Task RefreshTrending_Impl()
        {
            try
            {
                var tags = await _siteInstance.Client.GetTrendingTags();
                foreach (var tag in tags) 
                {
                    TrendingTags.Add(tag);
                }
            }
            catch (Exception ex)
            {
                //TODO
            }
        }
    }
}
