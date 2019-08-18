using Etch.OrchardCore.Search.Drivers;
using Etch.OrchardCore.Search.Indexing;
using Etch.OrchardCore.Search.Models;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Indexing;
using OrchardCore.Modules;

namespace Etch.OrchardCore.Search
{
    public class Startup : StartupBase
    {
        public Startup()
        {
            
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ContentPart, SiteSearch>();
            services.AddSingleton<ContentPart, SearchablePart>();

            services.AddScoped<IContentPartDisplayDriver, SiteSearchPartDisplay>();
            services.AddScoped<IContentPartDisplayDriver, SearchablePartDisplay>();

            services.AddScoped<IContentPartIndexHandler, SearchablePartIndexHandler>();

            services.AddScoped<IDataMigration, Migrations>();
        }
    }
}
