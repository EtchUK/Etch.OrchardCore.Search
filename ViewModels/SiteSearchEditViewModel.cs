using Etch.OrchardCore.Search.Settings;
using OrchardCore.Queries;

namespace Etch.OrchardCore.Search.ViewModels
{
    public class SiteSearchEditViewModel
    {
        public string ContentTypeSettings { get; set; }

        public SiteSearchDisplayType DisplayType { get; set; }

        public string EmptyResultsContent { get; set; }

        public string FilterInputPlaceholder { get; set; }

        public string SubmitButtonLabel { get; set; }

        public string Query { get; set; }

        public Query[] Queries { get; set; }

        public string ItemsDisplayType { get; set; }

        public int PageSize { get; set; }
    }
}
