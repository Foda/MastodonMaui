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
            FetchStatusContext = ReactiveCommand
                .CreateFromObservable(() => 
                    Observable.StartAsync(ct => FetchStatusContext_Impl(ct))
                              .TakeUntil(this.NavigateBack), outputScheduler: RxApp.MainThreadScheduler);
            NavigateBack = ReactiveCommand.Create(() => { });

            _isLoading = FetchStatusContext.IsExecuting.ToProperty(this, nameof(IsLoading),
                scheduler: RxApp.MainThreadScheduler);

            this.WhenAnyValue(vm => vm.ParentStatus)
                .Where(status => status != null)
                .Select(_ => Unit.Default)
                .InvokeCommand(FetchStatusContext);
        }

        private async Task FetchStatusContext_Impl(CancellationToken ct)
        {
            Replies.Clear();

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

            if (ct.IsCancellationRequested)
                return;
            
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
