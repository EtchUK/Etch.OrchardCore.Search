using Etch.OrchardCore.Search.Extensions;
using Etch.OrchardCore.Search.Indexes;
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
using OrchardCore.ContentManagement.Records;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YesSql;
using YesSql.Services;

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
        private readonly YesSql.ISession _session;

        #endregion

        #region Constructor

        public SiteSearchPartDisplay(IContentDefinitionManager contentDefinitionManager, IHttpContextAccessor httpContextAccessor, YesSql.ISession session)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _httpContextAccessor = httpContextAccessor;
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

        public override IDisplayResult Edit(SiteSearch part, BuildPartEditorContext context)
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

            return Initialize<SiteSearchEditViewModel>("SiteSearch_Edit", m =>
            {
                m.ContentTypeSettings = JsonConvert.SerializeObject(part.ContentTypeSettings);
                m.DisplayType = part.DisplayType;
                m.ItemsDisplayType = part.ItemsDisplayType;
                m.PageSize = part.PageSize;
            });
        }

        public async override Task<IDisplayResult> UpdateAsync(SiteSearch part, IUpdateModel updater)
        {
            var model = new SiteSearchEditViewModel();

            if (await updater.TryUpdateModelAsync(model, Prefix))
            {
                part.DisplayType = model.DisplayType;
                part.ItemsDisplayType = string.IsNullOrWhiteSpace(model.ItemsDisplayType) ? DefaultItemsDisplayType : model.ItemsDisplayType;
                part.PageSize = model.PageSize;
                part.ContentTypeSettings = JsonConvert.DeserializeObject<SiteSearchContentTypeSettings[]>(model.ContentTypeSettings);
            }

            return Edit(part);
        }

        #endregion

        #region HelperMethods

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

                foreach (var type in types)
                {
                    var query = _session.Query<ContentItem>()
                        .With<SearchableIndex>(x =>
                            x.Keywords.Contains(term) || x.DisplayText.Contains(term)
                        )
                        .With<ContentItemIndex>(x =>
                            x.Published && x.Latest && x.ContentType == type.ContentType
                        )
                        .OrderBy(x => x.DisplayText);

                    var displayName = searchableTypes.Where(x => x.Name == type.ContentType).SingleOrDefault()?.DisplayName ?? string.Empty;

                    if (!string.IsNullOrEmpty(displayName))
                    {
                        results.Add(new SiteSearchGroupedResultsGroup
                        {
                            ContentType = displayName,
                            Items = (await query
                                .Take(part.PageSize)
                                .ListAsync())
                                .ToList(),
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
            var items = new List<ContentItem>();

            if (!string.IsNullOrWhiteSpace(term))
            {
                var query = _session.Query<ContentItem>()
                    .With<SearchableIndex>(x =>
                        x.Keywords.Contains(term) || x.DisplayText.Contains(term)
                    )
                    .With<ContentItemIndex>(x =>
                        x.Published && x.Latest
                    );

                if (part.ContentTypeSettings.Any(x => x.Included))
                {
                    query = query.With<ContentItemIndex>(x =>
                        x.ContentType.IsIn(contentTypes)
                    );
                }

                query = query.OrderBy(x => x.DisplayText);

                totalItems = await query.CountAsync();

                items = (await query
                    .Skip((pager.Page - 1) * pager.PageSize)
                    .Take(pager.PageSize)
                    .ListAsync())
                    .ToList();
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
