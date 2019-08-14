using Etch.OrchardCore.Search.Models;
using Etch.OrchardCore.Search.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Search.Drivers
{
    public class SearchablePartDisplay : ContentPartDisplayDriver<SearchablePart>
    {
        #region Overrides
        
        public override IDisplayResult Edit(SearchablePart part, BuildPartEditorContext context)
        {
            return Initialize<SearchablePartEditViewModel>("SearchablePart_Edit", m =>
            {
                m.ExcludeFromResults = part.ExcludeFromResults;
                m.Keywords = part.Keywords;
            });
        }

        public async override Task<IDisplayResult> UpdateAsync(SearchablePart part, IUpdateModel updater)
        {
            var model = new SearchablePartEditViewModel();

            if (await updater.TryUpdateModelAsync(model, Prefix))
            {
                part.ExcludeFromResults = model.ExcludeFromResults;
                part.Keywords = !string.IsNullOrWhiteSpace(model.Keywords) ? model.Keywords : string.Empty;
            }

            return Edit(part);
        }

        #endregion
    }
}
