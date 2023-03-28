using MastodonLib;
using MastodonLib.Models;
using MastodonMaui.Services;
using MastodonMaui.ViewModels;
using Moq;
using System.Reactive.Linq;
using System.Text;

namespace MastodonMaui.Test
{
    public class ReplyViewModelTest
    {
        private readonly Mock<IMastodonClient> _client;
        private readonly Mock<ISiteInstance> _siteInstance;
        private readonly Mock<ICurrentUserService> _userService;
        private readonly Account _account;

        public ReplyViewModelTest()
        {
            _client = new Mock<IMastodonClient>();
            _siteInstance = new Mock<ISiteInstance>();
            _siteInstance.Setup(s => s.Client)
                .Returns(_client.Object);

            _account = new Account
            {
                Id = "a123",
                DisplayName = "Mike",
                AvatarUrl = "www.avatar.com",
                UserName = "MikeIsCool"
            };
            _userService = new Mock<ICurrentUserService>();
            _userService.Setup(s => s.Account)
                .Returns(_account);
        }

        [Fact]
        public void VerifyCanSendPostWithValidReply()
        {
            var model = new Status
            {
                Id = "123",
                Account = _account,
                Content = "blah",
                Reblogged = false,
                Favourited = false,
                ReblogsCount = 0,
                FavouritesCount = 0,
                MediaAttachments = new()
            };

            StatusViewModel replyToStatus = new(model, _siteInstance.Object);
            ReplyViewModel viewModel = new(replyToStatus, _siteInstance.Object, _userService.Object);

            // Act
            viewModel.ReplyText = "This is a test reply!";
            Assert.True(viewModel.RemaingCharacters == 479);
            Assert.True(viewModel.CanSendReply);
            Assert.False(viewModel.IsSendingReply);
        }

        [Fact]
        public async Task VerifySendReplyWithValidContents()
        {
            Status model = new()
            {
                Id = "123",
                Account = _account,
                Content = "blah",
                Reblogged = false,
                Favourited = false,
                ReblogsCount = 0,
                FavouritesCount = 0,
                MediaAttachments = new()
            };

            StatusViewModel replyToStatus = new(model, _siteInstance.Object);
            ReplyViewModel viewModel = new(replyToStatus, _siteInstance.Object, _userService.Object);

            // Mock the call to the API with our fake status
            string replyText = "This is a test reply!";
            Status newReply = new()
            {
                Id = "321",
                Account = _account,
                Content = replyText,
                InReplyToAccountId = _account.Id,
                InReplyToId = model.Id
            };
            _client.Setup(s => s.PostStatus(replyText, "public", model.Id))
                .Returns(Task.FromResult(newReply));

            // Act
            viewModel.ReplyText = replyText;
            Status newStatus = await viewModel.SendReply.Execute();

            Assert.True(newStatus.Content == replyText);
            Assert.True(newStatus.InReplyToId == model.Id);
        }

        [Fact]
        public void VerifyCannotSendPostWithInvalidReply()
        {
            var model = new Status
            {
                Id = "123",
                Account = _account,
                Content = "blah",
                Reblogged = false,
                Favourited = false,
                ReblogsCount = 0,
                FavouritesCount = 0,
                MediaAttachments = new()
            };

            StatusViewModel replyToStatus = new(model, _siteInstance.Object);
            ReplyViewModel viewModel = new(replyToStatus, _siteInstance.Object, _userService.Object);

            // Act
            StringBuilder longReply = new();
            for (int i = 0; i < 10; i++)
            {
                longReply.AppendLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            }
            viewModel.ReplyText = longReply.ToString();

            Assert.True(viewModel.RemaingCharacters < 0);
            Assert.False(viewModel.CanSendReply);
            Assert.False(viewModel.IsSendingReply);
        }
    }
}