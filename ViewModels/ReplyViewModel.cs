using MastodonMaui.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace MastodonMaui.ViewModels
{
    public class ReplyViewModel : ReactiveObject
    {
        private readonly ISiteInstance _siteInstance;

        private StatusViewModel _targetStatus;
        public StatusViewModel TargetStatus
        {
            get => _targetStatus;
            private set => this.RaiseAndSetIfChanged(ref _targetStatus, value);
        }

        private string _replyText = "";
        public string ReplyText
        {
            get => _replyText;
            set => this.RaiseAndSetIfChanged(ref _replyText, value);
        }

        public ReactiveCommand<Unit, Unit> SendReply { get; }
        public ReactiveCommand<Unit, Unit> CancelReply { get; }

        internal ReplyViewModel(StatusViewModel targetStatus, ISiteInstance siteInstance)
        {
            _siteInstance = siteInstance;
            TargetStatus = targetStatus;

            SendReply = ReactiveCommand.CreateFromTask(SendReply_Impl);
            CancelReply = ReactiveCommand.Create(() => { });
        }

        private async Task SendReply_Impl()
        {

        }
    }
}
