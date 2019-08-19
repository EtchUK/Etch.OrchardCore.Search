namespace Etch.OrchardCore.Search.ViewModels
{
    public class SiteSearchViewModel
    {
        public string EmptyResultsContent { get; set; }

        public string FilterInputPlaceholder { get; set; }

        public string Filter { get; set; }

        public string ItemsDisplayType { get; set; }

        public string SubmitButtonLabel { get; set; }

        public bool IsSearching
        {
            get { return !string.IsNullOrWhiteSpace(Filter); }
        }
    }
}
