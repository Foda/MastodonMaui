using MastodonLib.Models;
using MastodonMaui.Services;
using ReactiveUI;
using Splat;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

namespace MastodonMaui.ViewModels
{
    public class ReplyViewModel : ReactiveObject
    {
        private readonly ISiteInstance _siteInstance;

        private StatusViewModel _replyToStatus;
        internal StatusViewModel ReplyToStatus
        {
            get => _replyToStatus;
            private set => this.RaiseAndSetIfChanged(ref _replyToStatus, value);
        }

        private string _replyText = "";
        internal string ReplyText
        {
            get => _replyText;
            set => this.RaiseAndSetIfChanged(ref _replyText, value);
        }

        private string _visibility = "";
        internal string Visibility
        {
            get => _visibility;
            set => this.RaiseAndSetIfChanged(ref _visibility, value);
        }

        private ObservableCollection<string> _visibilitySettingList;
        internal ObservableCollection<string> VisibilitySettingList
        {
            get => _visibilitySettingList;
            private set => this.RaiseAndSetIfChanged(ref _visibilitySettingList, value);
        }

        readonly ObservableAsPropertyHelper<int> _remaingCharacters;
        public int RemaingCharacters => _remaingCharacters.Value;

        readonly ObservableAsPropertyHelper<bool> _isSendingReply;
        public bool IsSendingReply => _isSendingReply.Value;

        readonly ObservableAsPropertyHelper<bool> _canSendReply;
        public bool CanSendReply => _canSendReply.Value;

        internal ICurrentUserService CurrentUser { get; }

        internal ReactiveCommand<Unit, Status> SendReply { get; }
        internal ReactiveCommand<Unit, Unit> CancelReply { get; }

        private const int CHARACTER_LIMIT = 500;

        internal ReplyViewModel(StatusViewModel replyToStatus = null,
            ISiteInstance siteInstance = null, ICurrentUserService currentUser = null)
        {
            ReplyToStatus = replyToStatus;
            _siteInstance = siteInstance ?? Locator.Current.GetService<ISiteInstance>();
            CurrentUser = currentUser ?? Locator.Current.GetService<ICurrentUserService>();

            // TODO: map to enum and tie to API model?
            // See: https://docs.joinmastodon.org/methods/statuses/#form-data-parameters
            VisibilitySettingList = new()
            {
                "public",
                "unlisted",
                "private",
                "direct"
            };
            Visibility = VisibilitySettingList[0];

            var canSendPost = this.WhenAnyValue(vm => vm.ReplyText, 
                (replyText) => !string.IsNullOrEmpty(replyText) && replyText.Length <= CHARACTER_LIMIT);

            SendReply = ReactiveCommand.CreateFromTask(SendReply_Impl, canSendPost);
            CancelReply = ReactiveCommand.Create(() => { });

            _canSendReply = SendReply.CanExecute.ToProperty(this, nameof(CanSendReply));
            _isSendingReply = SendReply.IsExecuting.ToProperty(this, nameof(IsSendingReply));
            _remaingCharacters = this.WhenAnyValue(vm => vm.ReplyText)
                .Select(text => CHARACTER_LIMIT - text.Length)
                .ToProperty(this, nameof(RemaingCharacters));
        }

        private async Task<Status> SendReply_Impl()
        {
            try
            {
                if (ReplyToStatus != null)
                {
                    Status newPost = await _siteInstance.Client.PostStatus(
                        ReplyText, Visibility, ReplyToStatus.Id);
                    return newPost;
                }
            }
            catch (Exception ex) 
            {
            }
            
            return null;
        }
    }
}
