using HRMBackend.Types;
using System.Collections.Generic;
namespace HRMBackend.Utilities
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; } = 1;
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; } = 10;
        public int TotalCount { get; private set; }
        public HttpContext httpContext { get; private set; }
        public string fullUrl
        {
            get
            {
                return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}";
            }
        }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public string nextPageUrl
        {
            get
            {
                return HasNext ? UrlHelper.UpdateQueryStringParameters(
                 fullUrl,
                 new Dictionary<string, string>
                 {
                     { "page", (CurrentPage + 1).ToString() }

                 }
             ) : null;
            }
        }

        public string prevPageUrl
        {
            get
            {
                return HasPrevious ? UrlHelper.UpdateQueryStringParameters(
                 fullUrl,
                 new Dictionary<string, string>
                 {
                     { "page", (CurrentPage - 1).ToString() }

                 }
             ) : null;
            }
        }



        public PagedList(List<T> items, int count, int pageNumber, int pageSize, HttpContext httpctxt)
        {
            httpContext = httpctxt;
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static PaginatedData<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize, HttpContext httpctxt)
        {
            var count = source.Count();
            var items =  source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var paggedData = new PagedList<T>(items, count, pageNumber, pageSize, httpctxt);

            return new PaginatedData<T>
            {
                data = items,
                TotalCount = paggedData.TotalCount,
                NextPageUrl = paggedData.nextPageUrl,
                PreviousPageUrl = paggedData.prevPageUrl,
                CurrentPage = paggedData.CurrentPage,
                TotalPages = paggedData.TotalPages,
                PageSize = paggedData.PageSize
            };
        }
    }
}
