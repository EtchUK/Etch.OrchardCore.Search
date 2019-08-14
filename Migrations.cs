using Etch.OrchardCore.Search.Indexes;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Search
{
    public class Migrations : DataMigration
    {
        #region Dependencies

        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IRecipeMigrator _recipeMigrator;

        #endregion

        #region Constructor

        public Migrations(IContentDefinitionManager contentDefinitionManager, IRecipeMigrator recipeMigrator)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _recipeMigrator = recipeMigrator;
        }

        #endregion

        #region Migrations

        public async Task<int> CreateAsync()
        {
            _contentDefinitionManager.AlterPartDefinition("SiteSearch", part => part
                .WithDescription("Adds site search to page.")
                .WithDisplayName("Site Search"));

            await _recipeMigrator.ExecuteAsync("create.recipe.json", this);

            _contentDefinitionManager.AlterPartDefinition("SearchablePart", part => part
                .Attachable()
                .WithDescription("Makes content type included within site search.")
                .WithDisplayName("Searchable"));

            return 1;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.CreateMapIndexTable(nameof(SearchableIndex), table => table
                .Column<string>(nameof(SearchableIndex.DisplayText), c => c.WithLength(500))
                .Column<string>(nameof(SearchableIndex.Keywords), c => c.WithLength(500))
            );

            return 2;
        }

        #endregion
    }
}
