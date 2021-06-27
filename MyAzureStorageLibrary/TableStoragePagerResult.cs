using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;

namespace MyAzureStoragelibrary
{
    public class TableStoragePagerResult<T> where T:ITableEntity
    {
        public List<T> CurrentEntities { get; set; }
        public TableStoragePagerContext Context { get; internal set; }
    }
}