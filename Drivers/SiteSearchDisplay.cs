using Etch.OrchardCore.Search.Extensions;
using Etch.OrchardCore.Search.Indexes;
using Etch.OrchardCore.Search.Models;
using Etch.OrchardCore.Search.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Records;
using OrchardCore.ContentTypes.ViewModels;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Navigation;
using System;
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
        private const string DefaultDisplayType = "Summary";
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
           var request = _httpContextAccessor.HttpContext.Request;
            var term = request.GetQueryString(FilterQueryStringParameter);
            var pager = new Pager(request.GetPagerParameters(part.PageSize), part.PageSize);

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

                if (part.ContentTypes.Any())
                {
                    query = query.With<ContentItemIndex>(x =>
                        x.ContentType.IsIn(part.ContentTypes)
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

            return Initialize<SiteSearchViewModel>("SiteSearch", model =>
            {
                model.DisplayType = part.DisplayType;
                model.Filter = term;
                model.PagerShape = pagerShape;
                model.Results = items;
            })
            .Location("Detail", "Content:5");
        }

        public override IDisplayResult Edit(SiteSearch part, BuildPartEditorContext context)
        {
            var searchableTypes = _contentDefinitionManager.ListTypeDefinitions().Where(x => x.Parts.Any(p => p.Name == typeof(SearchablePart).Name)).ToList();

            return Initialize<SiteSearchEditViewModel>("SiteSearch_Edit", m =>
            {
                m.ContentTypes = part.ContentTypes;
                m.DisplayType = part.DisplayType;
                m.PageSize = part.PageSize;
                m.SearchableContentTypes = searchableTypes
                    .Select(x => new ContentTypeSelection
                    {
                        ContentTypeDefinition = x,
                        IsSelected = part.ContentTypes.Contains(x.Name)
                    })
                    .OrderBy(x => x.ContentTypeDefinition.DisplayName)
                    .ToArray();
            });
        }

        public async override Task<IDisplayResult> UpdateAsync(SiteSearch part, IUpdateModel updater)
        {
            var model = new SiteSearchEditViewModel();

            if (await updater.TryUpdateModelAsync(model, Prefix))
            {
                part.DisplayType = string.IsNullOrWhiteSpace(model.DisplayType) ? DefaultDisplayType : model.DisplayType;
                part.PageSize = model.PageSize;
                part.ContentTypes = model.ContentTypes;
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

        #endregion
    }
}
