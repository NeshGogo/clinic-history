using Microsoft.EntityFrameworkCore;

namespace HistoryService.Helppers
{
    public static class HttpContextExtensions
    {
        public static void InsertPaginationParams<T>(this HttpContext httpContext, IEnumerable<T> queryable, int recordPerPage)
        {
            double count =  queryable.Count();
            double pages = Math.Ceiling(count / recordPerPage);
            httpContext.Response.Headers.Add("pages", pages.ToString());
            httpContext.Response.Headers.Add("total", count.ToString());
        }
    }
}
