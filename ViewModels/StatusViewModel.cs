using MastodonLib.Models;
using MastodonMaui.Services;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace MastodonMaui.ViewModels
{
    public class StatusViewModel : ReactiveObject
    {
        private readonly Status _model;

        private readonly ISiteInstance _siteInstance;
        internal ISiteInstance SiteInstance => _siteInstance;

        public string Id => _model.Id;
        public bool IsReblog => _model.Reblog != null;
        public bool IsReply => !string.IsNullOrEmpty(ReplyToId);
        public string ReplyToId => _model.InReplyToId;
        public string ReblogHint
        {
            get
            {
                if (!IsReblog)
                    return "";
                return $"{_model.Account.DisplayName} boosted";
            }
        }

        public string Content => IsReblog ? _model.Reblog.Content : _model.Content;
        public DateTime CreatedAt => IsReblog ? _model.Reblog.CreatedAt : _model.CreatedAt;
        internal Account Account => IsReblog ? _model.Reblog.Account : _model.Account;

        public int ReblogCount => IsReblog ? _model.Reblog.ReblogsCount : _model.ReblogsCount;
        public int FavoriteCount => IsReblog ? _model.Reblog.FavouritesCount : _model.FavouritesCount;
        public int RepliesCount => IsReblog ? _model.Reblog.RepliesCount : _model.RepliesCount;

        /// <summary>
        /// If the user favorited the status
        /// </summary>
        private bool _didFavorite;
        public bool DidFavorite
        {
            get => _didFavorite;
            private set => this.RaiseAndSetIfChanged(ref _didFavorite, value);
        }

        /// <summary>
        /// If the user reblogged the status
        /// </summary>
        private bool _didReblog;
        public bool DidReblog
        {
            get => _didFavorite;
            private set => this.RaiseAndSetIfChanged(ref _didReblog, value);
        }

        public ObservableCollection<StatusViewModel> Replies { get; }

        public ReactiveCommand<Unit, Unit> ToggleFavorited { get; }
        public ReactiveCommand<Unit, Unit> ToggleReblogged { get; }

        internal StatusViewModel(Status model, ISiteInstance siteInstance)
        {
            _model = model;
            _siteInstance = siteInstance;
            Replies = new ObservableCollection<StatusViewModel>();

            DidFavorite = IsReblog ? _model.Reblog.Favourited : _model.Favourited;
            DidReblog = IsReblog ? _model.Reblog.Reblogged : _model.Reblogged;

            ToggleFavorited = ReactiveCommand.CreateFromTask(ToggleFavorited_Impl);
            ToggleReblogged = ReactiveCommand.CreateFromTask(ToggleReblogged_Impl);
        }

        private async Task ToggleFavorited_Impl()
        {
            await _siteInstance.Client.ToggleFavoriteStatus(
                IsReblog ? _model.Reblog.Id : _model.Id, !DidFavorite);

            DidFavorite = !DidFavorite;
        }

        private async Task ToggleReblogged_Impl()
        {
            await _siteInstance.Client.ToggleReblogStatus(
                IsReblog ? _model.Reblog.Id : _model.Id, !DidReblog);

            DidReblog = !DidReblog;
        }
    }
}
