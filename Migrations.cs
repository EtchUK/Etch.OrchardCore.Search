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

            _contentDefinitionManager.AlterPartDefinition("SearchablePart", part => part
                .Attachable()
                .WithDescription("Makes content type included within site search.")
                .WithDisplayName("Searchable"));

            await _recipeMigrator.ExecuteAsync("create.recipe.json", this);

            return 1;
        }
        
        #endregion
    }
}
