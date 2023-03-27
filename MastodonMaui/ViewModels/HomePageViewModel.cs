using MastodonLib.Models;
using MastodonMaui.Services;
using ReactiveUI;
using Splat;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

namespace MastodonMaui.ViewModels
{
    public class HomePageViewModel : ReactiveObject
    {
        private readonly ISiteInstance _siteInstance;

        public ObservableCollection<StatusViewModel> Items { get; }
        public ReactiveCommand<Unit, Unit> RefreshTimeline { get; }
        public ReactiveCommand<Unit, Unit> LoadMoreTimelineItems { get; }

        readonly ObservableAsPropertyHelper<bool> _isLoading;
        public bool IsLoading => _isLoading.Value;

        private StatusViewModel _selectedStatus;
        public StatusViewModel SelectedStatus
        {
            get => _selectedStatus;
            set => this.RaiseAndSetIfChanged(ref _selectedStatus, value);
        }

        readonly ObservableAsPropertyHelper<string> _currentState;
        public string CurrentState
        {
            get => _currentState.Value;
        }

        internal HomePageViewModel(ISiteInstance siteInstance = null)
        {
            _siteInstance = siteInstance ?? Locator.Current.GetService<ISiteInstance>();

            Items = new ObservableCollection<StatusViewModel>();

            RefreshTimeline = ReactiveCommand.CreateFromTask(RefreshTimeline_Impl);
            LoadMoreTimelineItems = ReactiveCommand.CreateFromTask(LoadMoreTimelineItems_Impl);

            _isLoading = RefreshTimeline.IsExecuting.ToProperty(this, nameof(IsLoading));

            _currentState = this.WhenAnyValue(vm => vm.SelectedStatus)
                .Select(_ => SelectedStatus != null ? "view_status" : "timeline")
                .ToProperty(this, nameof(CurrentState), "timeline", false, RxApp.MainThreadScheduler);
        }

        private async Task RefreshTimeline_Impl()
        {
            Items.Clear();
            try
            {
                List<Status> items = await _siteInstance.Client.GetHomeTimeline();
                AddItemsToTimeline(items);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task LoadMoreTimelineItems_Impl()
        {
            try
            {
                StatusViewModel lastStatus = Items.Last();
                List<Status> items = await _siteInstance.Client.GetHomeTimeline(20, lastStatus.Id);
                AddItemsToTimeline(items);
            }
            catch (Exception ex)
            {

            }
        }

        private void AddItemsToTimeline(List<Status> items)
        {
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
    }
}
