using OrchardCore.ContentManagement;
using System.Collections.Generic;
using System.Linq;

namespace Etch.OrchardCore.Search.ViewModels
{
    public class SiteSearchViewModel
    {
        public string Filter { get; set; }

        public string DisplayType { get; set; }

        public IList<ContentItem> Results { get; set; }

        public dynamic PagerShape { get; set; }

        public bool IsSearching
        {
            get { return !string.IsNullOrWhiteSpace(Filter); }
        }

        public bool HasResults
        {
            get { return Results != null && Results.Any(); }
        }
    }
}
