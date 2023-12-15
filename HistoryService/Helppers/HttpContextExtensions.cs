using Microsoft.EntityFrameworkCore;

namespace HistoryService.Helppers
{
    public static class HttpContextExtensions
    {
        public static void InsertPaginationParams<T>(this HttpContext httpContext, IEnumerable<T> queryable, int recordPerPage)
        {
            double count =  queryable.Count();
            double pages = Math.Ceiling(count / recordPerPage);
            var hasValue = !string.IsNullOrEmpty(httpContext.Response.Headers.AccessControlExposeHeaders);
            string val = hasValue ? ", " : "";

            httpContext.Response.Headers.AccessControlExposeHeaders += $"{val}Pages, Total";
            httpContext.Response.Headers.Add("Pages", pages.ToString());
            httpContext.Response.Headers.Add("Total", count.ToString());           
        }
    }
}
