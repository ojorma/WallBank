﻿namespace WallBank.Application.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? TotalCount { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, int? totalCount = null)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
            Message = null;
            Succeeded = true;
            Errors = null;
            TotalCount = totalCount;
        }
    }
}
