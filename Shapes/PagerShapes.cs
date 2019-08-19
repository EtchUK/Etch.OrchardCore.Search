using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Descriptors;
using OrchardCore.DisplayManagement.Implementation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Search.Shapes
{
    public class PagerShapesTableProvider : IShapeTableProvider
    {
        public PagerShapesTableProvider(IStringLocalizer<PagerShapes> localizer)
        {
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("PagerSearch")
                .OnCreated(created =>
                {
                    dynamic pager = created.Shape;

                    // Intializes the common properties of a Pager shape
                    // such that views can safely add values to them.
                    pager.ItemClasses = new List<string>();
                    pager.ItemAttributes = new Dictionary<string, string>();
                });
        }
    }

    public class PagerShapes : IShapeAttributeProvider
    {
        public PagerShapes(IStringLocalizer<PagerShapes> localizer)
        {
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        [Shape]
        public async Task<IHtmlContent> PagerSearch(
            dynamic Shape,
            dynamic DisplayAsync, 
            dynamic New, 
            IHtmlHelper Html,
            DisplayContext DisplayContext,
            object PreviousText,
            object NextText,
            string PreviousClass,
            string NextClass)
        {
            Shape.Classes.Add("pager");
            Shape.Metadata.Alternates.Clear();
            Shape.Metadata.Type = "List";

            var previousText = PreviousText ?? T["Previous"];
            var nextText = NextText ?? T["Next"];

            var routeData = new RouteValueDictionary(Html.ViewContext.RouteData.Values);
            var httpContextAccessor = DisplayContext.ServiceProvider.GetService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var queryString = httpContext.Request.Query;
                if (queryString != null)
                {
                    foreach (var key in from string key in queryString.Keys where key != null && !routeData.ContainsKey(key) let value = queryString[key] select key)
                    {
                        routeData[key] = queryString[key];
                    }
                }
            }

            var currentPage = (int)Shape.CurrentPage;
            var hasMoreResults = (bool)Shape.HasMoreResults;

            if (currentPage != 1)
            {
                var previousRouteData = new RouteValueDictionary(routeData);
                previousRouteData["page"] = (string)(currentPage - 1).ToString();
                Shape.Add(await New.Pager_Previous(Value: previousText, RouteValues: previousRouteData, Pager: Shape));
                Shape.FirstClass = PreviousClass;
            }

            if (hasMoreResults)
            {
                var nextRouteData = new RouteValueDictionary(routeData);
                nextRouteData["page"] = (string)(currentPage + 1).ToString();
                Shape.Add(await New.Pager_Next(Value: nextText, RouteValues: nextRouteData, Pager: Shape));
                Shape.LastClass = NextClass;
            }

            return await DisplayAsync(Shape);
        }
    }
}
