using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;

namespace MyAzureStoragelibrary
{
    public interface ITableStorageService
    {
        TableStorageServiceResult Get(string continuationToken);
    }

    public class TableStorageServiceResult
    {
        public List<TableEntity> Entities { get; set; }
        public string ContinuationToken { get; set; }
    }
}