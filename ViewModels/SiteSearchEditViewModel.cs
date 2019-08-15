using OrchardCore.ContentTypes.ViewModels;

namespace Etch.OrchardCore.Search.ViewModels
{
    public class SiteSearchEditViewModel
    {
        public string[] ContentTypes { get; set; }

        public string DisplayType { get; set; }

        public int PageSize { get; set; }
        
        public ContentTypeSelection[] SearchableContentTypes { get; set; }
    }
}
