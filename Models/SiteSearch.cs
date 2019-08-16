using Etch.OrchardCore.Search.Settings;
using Newtonsoft.Json;
using OrchardCore.ContentManagement;

namespace Etch.OrchardCore.Search.Models
{
    public class SiteSearch : ContentPart
    {
        private const int DefaultPageSize = 10;

        public SiteSearchContentTypeSettings[] ContentTypeSettings { get; set; }

        public SiteSearchDisplayType DisplayType { get; set; } = SiteSearchDisplayType.List;

        public string EmptyResultsContent { get; set; } = "Unable to find anything that matches your search.";

        public string ItemsDisplayType { get; set; } = "Summary";

        public int PageSize { get; set; } = DefaultPageSize;
    }

    public class SiteSearchContentTypeSettings
    {
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("included")]
        public bool Included { get; set; }

        [JsonProperty("emptyResultsContent")]
        public string EmptyResultsContent { get; set; }

        [JsonProperty("viewMoreLinkText")]
        public string ViewMoreLinkText { get; set; }

        [JsonProperty("viewMoreLinkUrl")]
        public string ViewMoreLinkUrl { get; set; }

        [JsonIgnore]
        public bool HasViewMoreLinkUrl
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ViewMoreLinkUrl);
            }
        }
    }
}
