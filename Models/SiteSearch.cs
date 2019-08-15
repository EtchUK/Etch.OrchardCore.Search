using Etch.OrchardCore.Search.Settings;
using OrchardCore.ContentManagement;

namespace Etch.OrchardCore.Search.Models
{
    public class SiteSearch : ContentPart
    {
        private const int DefaultPageSize = 10;

        public string[] ContentTypes { get; set; } = new string[] { };

        public SiteSearchDisplayType DisplayType { get; set; } = SiteSearchDisplayType.List;

        public string ItemsDisplayType { get; set; } = "Summary";

        public int PageSize { get; set; } = DefaultPageSize;
    }
}
