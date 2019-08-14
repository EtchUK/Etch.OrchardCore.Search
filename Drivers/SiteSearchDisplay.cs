using Etch.OrchardCore.Search.Models;
using Etch.OrchardCore.Search.ViewModels;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Search.Drivers
{
    public class SiteSearchPartDisplay : ContentPartDisplayDriver<SiteSearch>
    {
        #region Constants

        private const string FilterQueryStringParameter = "filter";

        #endregion

        #region Dependencies

        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructor

        public SiteSearchPartDisplay(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Overrides

        public override IDisplayResult Display(SiteSearch part)
        {
            return Initialize<SiteSearchViewModel>("SiteSearch", model =>
            {
                model.Filter = GetFilterTerm();
            })
            .Location("Detail", "Content:5");
        }

        public override IDisplayResult Edit(SiteSearch part, BuildPartEditorContext context)
        {
            return Initialize<SiteSearchEditViewModel>("SiteSearch_Edit", m =>
            {
                
            });
        }

        public async override Task<IDisplayResult> UpdateAsync(SiteSearch part, IUpdateModel updater)
        {
            var viewModel = new SiteSearchEditViewModel();

            if (await updater.TryUpdateModelAsync(viewModel, Prefix))
            {
                
            }

            return Edit(part);
        }

        #endregion

        #region Helper Methods

        private string GetFilterTerm()
        {
            var query = _httpContextAccessor.HttpContext.Request.Query;

            if (!query.Keys.Contains(FilterQueryStringParameter))
            {
                return string.Empty;
            }

            return query[FilterQueryStringParameter];
        }

        #endregion
    }
}
