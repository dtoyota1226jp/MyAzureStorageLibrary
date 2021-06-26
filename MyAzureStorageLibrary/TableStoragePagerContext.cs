using System;
using System.Collections.Generic;

namespace MyAzureStoragelibrary
{
    public class TableStoragePagerContext
    {
        public int PageSize { get; set; }
        public string[] ContinuationTokens { get; set; }
        public bool HasMore { get; set; }
        public int TotalEntityCount { get; set; }
        public int MaxSegment { get; set; } = -1;

        public int GetMaxPageNumber()
        {
            return (int)Math.Ceiling(((double)TotalEntityCount / (double)PageSize));
        }
    }
}