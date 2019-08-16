using Etch.OrchardCore.Search.Settings;

namespace Etch.OrchardCore.Search.ViewModels
{
    public class SiteSearchEditViewModel
    {
        public string ContentTypeSettings { get; set; }

        public SiteSearchDisplayType DisplayType { get; set; }

        public string EmptyResultsContent { get; set; }

        public string ItemsDisplayType { get; set; }

        public int PageSize { get; set; }
    }
}
