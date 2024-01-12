﻿namespace HRMBackend.Types
{
    public class PaginatedData<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string? NextPageUrl { get; set; }
        public string? PreviousPageUrl { get; set; }
        public List<T> data { get; set; }
    }
}
