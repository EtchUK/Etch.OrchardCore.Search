using Etch.OrchardCore.Search.Extensions;
using Etch.OrchardCore.Search.Indexes;
using Etch.OrchardCore.Search.Models;
using Etch.OrchardCore.Search.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Records;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YesSql;

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

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly YesSql.ISession _session;

        #endregion

        #region Constructor

        public SiteSearchPartDisplay(IHttpContextAccessor httpContextAccessor, YesSql.ISession session)
        {
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
                    )
                    .OrderBy(x => x.DisplayText);

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
            return Initialize<SiteSearchEditViewModel>("SiteSearch_Edit", m =>
            {
                m.DisplayType = part.DisplayType;
                m.PageSize = part.PageSize;
            });
        }

        public async override Task<IDisplayResult> UpdateAsync(SiteSearch part, IUpdateModel updater)
        {
            var model = new SiteSearchEditViewModel();

            if (await updater.TryUpdateModelAsync(model, Prefix))
            {
                part.DisplayType = string.IsNullOrWhiteSpace(model.DisplayType) ? DefaultDisplayType : model.DisplayType;
                part.PageSize = model.PageSize;
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
