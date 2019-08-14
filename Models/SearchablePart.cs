using OrchardCore.ContentManagement;

namespace Etch.OrchardCore.Search.Models
{
    public class SearchablePart : ContentPart
    {
        public bool ExcludeFromResults { get; set; }

        public string Keywords { get; set; }
    }
}
