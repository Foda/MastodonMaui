using MastodonLib;
using MastodonLib.Models;
using MastodonMaui.Services;
using MastodonMaui.ViewModels;
using Moq;
using System.Reactive.Linq;

namespace MastodonMaui.Test
{
    public class StatusViewModelTest
    {
        [Fact]
        public async Task VerifyFavoriteAndReblogWorks()
        {
            var client = new Mock<IMastodonClient>();
            var siteInstance = new Mock<ISiteInstance>();
            siteInstance.Setup(s => s.Client)
                .Returns(client.Object);

            var account = new Account
            {
                Id = "a123",
                DisplayName = "Mike",
                AvatarUrl = "www.avatar.com",
                UserName = "MikeIsCool"
            };

            var model = new Status
            {
                Id = "123",
                Account = account,
                Content = "blah",
                Reblogged = false,
                Favourited = false,
                ReblogsCount = 0,
                FavouritesCount = 0,
                MediaAttachments = new()
            };

            var viewModel = new StatusViewModel(model, siteInstance.Object);

            await viewModel.ToggleFavorited.Execute();
            await viewModel.ToggleReblogged.Execute();

            Assert.True(viewModel.DidReblog);
            Assert.True(viewModel.DidFavorite);
        }
    }
}