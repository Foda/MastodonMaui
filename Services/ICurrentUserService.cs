using MastodonLib.Models;

namespace MastodonMaui.Services
{
    internal interface ICurrentUserService
    {
        Account Account { get; }
    }
}
