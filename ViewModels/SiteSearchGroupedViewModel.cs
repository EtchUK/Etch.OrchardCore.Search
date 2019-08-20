using Etch.OrchardCore.Search.Models;
using OrchardCore.ContentManagement;
using System.Collections.Generic;
using System.Linq;

namespace Etch.OrchardCore.Search.ViewModels
{
    public class SiteSearchGroupedViewModel : SiteSearchViewModel
    {
        public IList<SiteSearchGroupedResultsGroup> Results { get; set; }

        public bool HasAnyMatches
        {
            get
            {
                return Results != null && Results.Any(x => x.Items != null && x.Items.Length > 0);
            }
        }
    }

    public class SiteSearchGroupedResultsGroup
    {
        public string ContentType { get; set; }

        public ContentItem[] Items { get; set; }

        public SiteSearchContentTypeSettings Settings { get; set; }

        public bool HasResults
        {
            get { return Items != null && Items.Any();  }
        }
    }
}
