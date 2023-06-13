using MastodonLib;
using MastodonLib.Models;
using MastodonMaui.Controls;
using MastodonMaui.Services;
using MastodonMaui.ViewModels;
using Moq;
using System.Reactive.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

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

        [Fact]
        public async Task VerifyStatusTextURLParsing()
        {
            string input =
                @"<p>" +
                @"<span><a href='https://google.com'>@<span>mike</span></a></span>" +
                @"<span><a href='https://bing.com'>@<span>jeff</span></a></span>have you seen this" +
                @"<a href='https://microsoft.com'>" +
                @"<span>https://www.</span><span>wow</span><span></span>" +
                @" </a>" +
                @"</p>";

            XElement element = XElement.Parse(input);

            List<Span> inlines = new();
            StatusTextControl.ParseText(element, inlines, null);

            Assert.True(inlines.Count > 0);
            Assert.True(inlines[0].Text == "@mike");
            Assert.True(inlines[0].TextDecorations == TextDecorations.Underline);

            Assert.True(inlines[1].Text == "@jeff");
            Assert.True(inlines[1].TextDecorations == TextDecorations.Underline);

            Assert.True(inlines[2].Text == "have you seen this");
            Assert.True(inlines[2].TextDecorations == TextDecorations.None);
        }
    }
}