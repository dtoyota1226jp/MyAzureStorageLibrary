using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;

namespace MyAzureStoragelibrary
{
    public class TableStoragePagerResult
    {
        public List<TableEntity> CurrentEntities { get; set; }
        public TableStoragePagerContext Context { get; internal set; }
    }
}