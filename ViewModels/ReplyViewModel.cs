using MastodonLib.Models;
using MastodonMaui.Services;
using ReactiveUI;
using System.Reactive;

namespace MastodonMaui.ViewModels
{
    public class ReplyViewModel : ReactiveObject
    {
        private readonly ISiteInstance _siteInstance;

        private StatusViewModel _targetStatus;
        internal StatusViewModel TargetStatus
        {
            get => _targetStatus;
            private set => this.RaiseAndSetIfChanged(ref _targetStatus, value);
        }

        private string _replyText = "";
        internal string ReplyText
        {
            get => _replyText;
            set => this.RaiseAndSetIfChanged(ref _replyText, value);
        }

        internal ReactiveCommand<Unit, Status> SendReply { get; }
        internal ReactiveCommand<Unit, Unit> CancelReply { get; }

        internal ReplyViewModel(StatusViewModel targetStatus, ISiteInstance siteInstance)
        {
            _siteInstance = siteInstance;
            TargetStatus = targetStatus;

            var canSendReply = this.WhenAnyValue(vm => vm.ReplyText, 
                (replyText) => !string.IsNullOrEmpty(replyText));

            SendReply = ReactiveCommand.CreateFromTask(SendReply_Impl, canSendReply);
            CancelReply = ReactiveCommand.Create(() => { });
        }

        private async Task<Status> SendReply_Impl()
        {
            try
            {
                Status newPost = await _siteInstance.Client.PostStatus(ReplyText, TargetStatus.Id);
                return newPost;
            }
            catch (Exception ex) 
            {
            }
            
            return null;
        }
    }
}
