using OrchardCore.ContentManagement;

namespace Etch.OrchardCore.Search.Models
{
    public class SiteSearch : ContentPart
    {
        private const int DefaultPageSize = 10;

        public string[] ContentTypes { get; set; } = new string[] { };

        public string DisplayType { get; set; } = "Summary";

        public int PageSize { get; set; } = DefaultPageSize;
    }
}
