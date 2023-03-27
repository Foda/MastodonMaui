using MastodonLib.Models;
using MastodonMaui.Services;
using ReactiveUI;
using Splat;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

namespace MastodonMaui.ViewModels
{
    public class StatusPageViewModel : ReactiveObject
    {
        internal ISiteInstance SiteInstance { get; private set; }

        private StatusViewModel _parentStatus;
        public StatusViewModel ParentStatus
        {
            get => _parentStatus;
            set => this.RaiseAndSetIfChanged(ref _parentStatus, value);
        }

        private ObservableCollection<StatusViewModel> _replies = new();
        public ObservableCollection<StatusViewModel> Replies
        {
            get => _replies;
            private set => this.RaiseAndSetIfChanged(ref _replies, value);
        }

        readonly ObservableAsPropertyHelper<bool> _isLoading;
        public bool IsLoading => _isLoading.Value;

        public ReactiveCommand<Unit, Unit> FetchStatusContext { get; }
        public ReactiveCommand<Unit, Unit> NavigateBack { get; }

        internal StatusPageViewModel(ISiteInstance siteInstance = null)
        {
            SiteInstance = siteInstance ?? Locator.Current.GetService<ISiteInstance>();
            NavigateBack = ReactiveCommand.CreateFromTask(NavigateBack_Impl);
            FetchStatusContext = ReactiveCommand.CreateFromTask(FetchStatusContext_Impl);

            _isLoading = FetchStatusContext.IsExecuting.ToProperty(this, nameof(IsLoading));

            this.WhenAnyValue(vm => vm.ParentStatus)
                .Where(status => status != null)
                .Select(_ => Unit.Default)
                .InvokeCommand(FetchStatusContext);
        }

        private async Task NavigateBack_Impl() { }

        private async Task FetchStatusContext_Impl()
        {
            StatusContext statusContext = null;
            try
            {
                statusContext = await SiteInstance.Client.GetStatusContext(
                    ParentStatus.IsReblog ? ParentStatus.ReblogId : ParentStatus.Id);
            }
            catch (Exception ex)
            {
                // TODO
            }
            
            if (statusContext != null)
            {
                foreach (Status model in statusContext.Descendants)
                {
                    if (ParentStatus.Replies.Any(reply => reply.Id == model.Id))
                        continue;

                    Replies.Add(new StatusViewModel(model, SiteInstance));
                }
            }
        }
    }
}
