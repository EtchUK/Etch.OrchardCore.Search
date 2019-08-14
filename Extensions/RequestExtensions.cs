using Microsoft.AspNetCore.Http;
using OrchardCore.Navigation;

namespace Etch.OrchardCore.Search.Extensions
{
    public static class RequestExtensions
    {
        public static string GetQueryString(this HttpRequest request, string field)
        {
            if (!request.Query.Keys.Contains(field))
            {
                return string.Empty;
            }

            return request.Query[field];
        }

        public static PagerParameters GetPagerParameters(this HttpRequest request, int defaultPageSize)
        {
            return new PagerParameters
            {
                Page = int.TryParse(request.GetQueryString("Page"), out int page) ? page : 0,
                PageSize = int.TryParse(request.GetQueryString("PageSize"), out int pageSize) ? pageSize : defaultPageSize
            };
        }
    }
}
