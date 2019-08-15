using OrchardCore.ContentManagement;
using System.Collections.Generic;
using System.Linq;

namespace Etch.OrchardCore.Search.ViewModels
{
    public class SiteSearchGroupedViewModel : SiteSearchViewModel
    {
        public IList<SiteSearchGroupedResultsGroup> Results { get; set; }
    }

    public class SiteSearchGroupedResultsGroup
    {
        public string ContentType { get; set; }

        public IList<ContentItem> Items { get; set; }

        public bool HasResults
        {
            get { return Items != null && Items.Any();  }
        }
    }
}
