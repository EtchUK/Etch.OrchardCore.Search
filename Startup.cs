using Etch.OrchardCore.Search.Drivers;
using Etch.OrchardCore.Search.Indexing;
using Etch.OrchardCore.Search.Models;
using Etch.OrchardCore.Search.Shapes;
using Etch.OrchardCore.Search.ViewModels;
using Fluid;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.DisplayManagement.Descriptors;
using OrchardCore.Indexing;
using OrchardCore.Modules;

namespace Etch.OrchardCore.Search
{
    public class Startup : StartupBase
    {
        public Startup()
        {
            TemplateContext.GlobalMemberAccessStrategy.Register<SiteSearchGroupedViewModel>();
            TemplateContext.GlobalMemberAccessStrategy.Register<SiteSearchListViewModel>();
            TemplateContext.GlobalMemberAccessStrategy.Register<SiteSearchGroupedResultsGroup>();
            TemplateContext.GlobalMemberAccessStrategy.Register<SiteSearchContentTypeSettings>();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddContentPart<SiteSearch>()
                .UseDisplayDriver<SiteSearchPartDisplay>();

            services.AddContentPart<SearchablePart>()
                .UseDisplayDriver<SearchablePartDisplay>();

            services.AddScoped<IContentPartIndexHandler, SearchablePartIndexHandler>();

            services.AddScoped<IShapeTableProvider, PagerShapesTableProvider>();
            services.AddShapeAttributes<PagerShapes>();

            services.AddScoped<IDataMigration, Migrations>();
        }
    }
}
