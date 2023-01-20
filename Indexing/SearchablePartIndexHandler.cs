using Etch.OrchardCore.Search.Models;
using OrchardCore.Indexing;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Search.Indexing
{
    public class SearchablePartIndexHandler : ContentPartIndexHandler<SearchablePart>
    {
        public override Task BuildIndexAsync(SearchablePart part, BuildPartIndexContext context)
        {
            var options = context.Settings.ToOptions();
            
            context.DocumentIndex.Set("SearchablePart.Keywords", part.Keywords, options);

            context.DocumentIndex.Entries.Add(
                new DocumentIndex.DocumentIndexEntry(
                     "SearchablePart.ExcludeFromResults",
                    part.ExcludeFromResults,
                    DocumentIndex.Types.Boolean,
                    DocumentIndexOptions.Store
                )
            );

            return Task.CompletedTask;
        }
    }
}
