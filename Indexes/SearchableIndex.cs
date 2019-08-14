using Etch.OrchardCore.Search.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Etch.OrchardCore.Search.Indexes
{
    public class SearchableIndex : MapIndex
    {
        public string DisplayText { get; set; }
        public string Keywords { get; set; }
    }

    public class SearchableIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<SearchableIndex>()
                .Map(contentItem =>
                {
                    var searchablePart = contentItem.As<SearchablePart>();

                    if (searchablePart == null || searchablePart.ExcludeFromResults)
                    {
                        return null;
                    }

                    return new SearchableIndex
                    {
                        DisplayText = contentItem.DisplayText,
                        Keywords = searchablePart.Keywords
                    };
                });
        }
    }
}
