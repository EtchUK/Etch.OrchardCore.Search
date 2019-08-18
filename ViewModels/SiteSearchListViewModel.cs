using OrchardCore.ContentManagement;
using System.Linq;

namespace Etch.OrchardCore.Search.ViewModels
{
    public class SiteSearchListViewModel : SiteSearchViewModel
    {
        public ContentItem[] Results { get; set; }

        public dynamic PagerShape { get; set; }

        public bool HasResults
        {
            get { return Results != null && Results.Any(); }
        }
    }
}
