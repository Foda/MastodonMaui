using MastodonLib.Models;
using MastodonMaui.Services;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace MastodonMaui.ViewModels
{
    public class TimelineViewModel : ReactiveObject
    {
        private readonly ISiteInstance _siteInstance;

        public ObservableCollection<StatusViewModel> Items { get; }
        public ReactiveCommand<Unit, Unit> RefreshTimeline { get; }

        internal TimelineViewModel(ISiteInstance siteInstance)
        {
            _siteInstance = siteInstance;
            Items = new ObservableCollection<StatusViewModel>();
            RefreshTimeline = ReactiveCommand.CreateFromTask(RefreshTimeline_Impl);
        }

        private async Task RefreshTimeline_Impl()
        {
            Items.Clear();
            try
            {
                List<Status> items = await _siteInstance.Client.GetHomeTimeline();

                Dictionary<string, StatusViewModel> statusLookup = new();
                List<StatusViewModel> replyItems = new();

                // Build our display list from non-replies
                foreach (var item in items)
                {
                    StatusViewModel statusViewModel = new(item, _siteInstance);
                    if (statusViewModel.IsReply)
                    {
                        replyItems.Add(statusViewModel);
                    }
                    else
                    {
                        statusLookup.Add(item.Id, statusViewModel);
                        Items.Add(statusViewModel);
                    }
                }

                // See if we have a parent status, and if so, add it to their reply list
                foreach (StatusViewModel reply in replyItems)
                {
                    if (statusLookup.TryGetValue(reply.ReplyToId, out StatusViewModel parentStatus))
                    {
                        parentStatus.Replies.Add(reply);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
