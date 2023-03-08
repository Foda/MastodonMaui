using MastodonLib;

namespace MastodonMaui.Services
{
    internal interface ISiteInstance
    {
        MastodonClient Client { get; }
        string InstanceUrl { get; }
    }
}
