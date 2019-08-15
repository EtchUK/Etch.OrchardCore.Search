using Etch.OrchardCore.Search.Settings;
using OrchardCore.ContentTypes.ViewModels;

namespace Etch.OrchardCore.Search.ViewModels
{
    public class SiteSearchEditViewModel
    {
        public string[] ContentTypes { get; set; }

        public SiteSearchDisplayType DisplayType { get; set; }

        public string ItemsDisplayType { get; set; }

        public int PageSize { get; set; }

        public ContentTypeSelection[] SearchableContentTypes { get; set; }
    }
}
