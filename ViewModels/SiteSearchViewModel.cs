namespace Etch.OrchardCore.Search.ViewModels
{
    public class SiteSearchViewModel
    {
        public string Filter { get; set; }

        public string ItemsDisplayType { get; set; }

        public bool IsSearching
        {
            get { return !string.IsNullOrWhiteSpace(Filter); }
        }
    }
}
