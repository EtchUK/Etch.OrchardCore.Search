using Etch.OrchardCore.Search.Extensions;
using Etch.OrchardCore.Search.Models;
using Etch.OrchardCore.Search.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Navigation;
using OrchardCore.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Search.Drivers
{
    public class SiteSearchPartDisplay : ContentPartDisplayDriver<SiteSearch>
    {
        #region Constants

        private const string FilterQueryStringParameter = "filter";
        private const string DefaultItemsDisplayType = "Summary";
        private const int DefaultPageSize = 2;

        #endregion

        #region Dependencies

        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IQueryManager _queryManager;
        private readonly YesSql.ISession _session;

        #endregion

        #region Constructor

        public SiteSearchPartDisplay(IContentDefinitionManager contentDefinitionManager, IHttpContextAccessor httpContextAccessor, IQueryManager queryManager, YesSql.ISession session)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _httpContextAccessor = httpContextAccessor;
            _queryManager = queryManager;
            _session = session;
        }

        #endregion

        #region Overrides

        public override async Task<IDisplayResult> DisplayAsync(SiteSearch part, BuildPartDisplayContext context)
        {
            if (part.DisplayType == Settings.SiteSearchDisplayType.Grouped)
            {
                return await GroupedAsync(part, context);
            }

            return await ListAsync(part, context);
        }

        public override async Task<IDisplayResult> EditAsync(SiteSearch part, BuildPartEditorContext context)
        {
            if (part.ContentTypeSettings == null)
            {
                part.ContentTypeSettings = GetSearchableContentTypes()
                    .Select(x => new SiteSearchContentTypeSettings
                    {
                        ContentType = x.Name,
                        Included = true
                    })
                    .ToArray();
            }

            var queries = (await _queryManager.ListQueriesAsync()).ToArray();

            return Initialize<SiteSearchEditViewModel>("SiteSearch_Edit", m =>
            {
                m.ContentTypeSettings = JsonConvert.SerializeObject(part.ContentTypeSettings);
                m.DisplayType = part.DisplayType;
                m.EmptyResultsContent = part.EmptyResultsContent;
                m.ItemsDisplayType = part.ItemsDisplayType;
                m.PageSize = part.PageSize;
                m.Query = part.Query;
                m.Queries = queries;
            });
        }

        public async override Task<IDisplayResult> UpdateAsync(SiteSearch part, IUpdateModel updater, UpdatePartEditorContext context)
        {
            var model = new SiteSearchEditViewModel();

            if (await updater.TryUpdateModelAsync(model, Prefix))
            {
                part.ContentTypeSettings = CleanSettings(JsonConvert.DeserializeObject<SiteSearchContentTypeSettings[]>(model.ContentTypeSettings));
                part.DisplayType = model.DisplayType;
                part.EmptyResultsContent = model.EmptyResultsContent;
                part.ItemsDisplayType = string.IsNullOrWhiteSpace(model.ItemsDisplayType) ? DefaultItemsDisplayType : model.ItemsDisplayType;
                part.PageSize = model.PageSize;
                part.Query = model.Query;
            }

            return await EditAsync(part, context);
        }

        #endregion

        #region HelperMethods

        private SiteSearchContentTypeSettings[] CleanSettings(SiteSearchContentTypeSettings[] settings)
        {
            foreach (var setting in settings)
            {
                if (!string.IsNullOrEmpty(setting.ViewMoreLinkUrl) && setting.ViewMoreLinkUrl.StartsWith("/"))
                {
                    setting.ViewMoreLinkUrl = setting.ViewMoreLinkUrl.Substring(1);
                }
            }

            return settings;
        }

        private async Task<dynamic> CreatePager(BuildPartDisplayContext context, Pager pager, string term, int totalItems)
        {
            var routeData = new RouteData();

            if (!string.IsNullOrWhiteSpace(term))
            {
                routeData.Values.Add(FilterQueryStringParameter, term);
            }

            return (await context.New.Pager(pager)).TotalItemCount(totalItems).RouteData(routeData);
        }

        private IList<ContentTypeDefinition> GetSearchableContentTypes()
        {
            return _contentDefinitionManager
                .ListTypeDefinitions()
                .Where(x => x.Parts.Any(p => p.Name == typeof(SearchablePart).Name))
                .OrderBy(x => x.DisplayName)
                .ToList();
        }

        private async Task<IDisplayResult> GroupedAsync(SiteSearch part, BuildPartDisplayContext context)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var term = request.GetQueryString(FilterQueryStringParameter);
            var results = new List<SiteSearchGroupedResultsGroup>();

            if (!string.IsNullOrWhiteSpace(term))
            {
                var searchableTypes = GetSearchableContentTypes();
                var types = part.ContentTypeSettings.Where(x => x.Included).ToArray();
                var parameters = new Dictionary<string, object>
                    {
                        { "filter", term },
                        { "size", part.PageSize }
                    };

                foreach (var type in types)
                {
                    var query = await _queryManager.GetQueryAsync(type.Query);
                    var displayName = searchableTypes.Where(x => x.Name == type.ContentType).SingleOrDefault()?.DisplayName ?? string.Empty;

                    if (!string.IsNullOrEmpty(displayName))
                    {
                        results.Add(new SiteSearchGroupedResultsGroup
                        {
                            ContentType = displayName,
                            Items = await _queryManager.ExecuteQueryAsync(query, parameters) as ContentItem[],
                            Settings = type
                        });
                    }
                }
            }

            return Initialize<SiteSearchGroupedViewModel>("SiteSearch_Grouped", model =>
            {
                model.EmptyResultsContent = part.EmptyResultsContent;
                model.Filter = term;
                model.ItemsDisplayType = part.ItemsDisplayType;
                model.Results = results;
            })
            .Location("Detail", "Content:5");
        }

        private async Task<IDisplayResult> ListAsync(SiteSearch part, BuildPartDisplayContext context)
        {
            var contentTypes = part.ContentTypeSettings.Where(x => x.Included).Select(x => x.ContentType).ToList();

            var request = _httpContextAccessor.HttpContext.Request;
            var pager = new Pager(request.GetPagerParameters(part.PageSize), part.PageSize);
            var term = request.GetQueryString(FilterQueryStringParameter);

            var totalItems = 0;
            var items = new ContentItem[] { };

            if (!string.IsNullOrWhiteSpace(term))
            {
                var query = await _queryManager.GetQueryAsync(part.Query);
                var parameters = new Dictionary<string, object>
                    {
                        { "filter", term },
                        { "size", part.PageSize }
                    };

                items = await _queryManager.ExecuteQueryAsync(query, parameters) as ContentItem[];
            }

            dynamic pagerShape = await CreatePager(context, pager, term, totalItems);

            return Initialize<SiteSearchListViewModel>("SiteSearch_List", model =>
            {
                model.EmptyResultsContent = part.EmptyResultsContent;
                model.Filter = term;
                model.ItemsDisplayType = part.ItemsDisplayType;
                model.PagerShape = pagerShape;
                model.Results = items;
            })
            .Location("Detail", "Content:5");
        }

        #endregion
    }
}
