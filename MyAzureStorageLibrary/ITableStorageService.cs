using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;

namespace MyAzureStoragelibrary
{
    public interface ITableStorageService<T> where T : ITableEntity
    {
        TableStorageServiceResult<T> Get(string continuationToken);
    }

    public class TableStorageServiceResult<T> where T : ITableEntity
    {
        public List<T> Entities { get; set; }
        public string ContinuationToken { get; set; }
    }
}