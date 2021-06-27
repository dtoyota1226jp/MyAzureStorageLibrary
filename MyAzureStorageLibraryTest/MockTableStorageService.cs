using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using MyAzureStoragelibrary;

namespace MyAzureStoragelibraryTest
{
    internal class MockTableStorageService<T> : ITableStorageService<T> where T : ITableEntity
    {
        private List<T> _dataSource;
        public MockTableStorageService(List<T> dataSource)
        {
            _dataSource = dataSource;
        }

        public TableStorageServiceResult<T> Get(string continuationToken)
        {
            int offset;
            string newContinuationToken;

            if (string.IsNullOrEmpty(continuationToken))
            {
                offset = 0;
                newContinuationToken = "token1";
            }
            else if(continuationToken == "token1")
            {
                offset = 1000;
                newContinuationToken = "token2";
            }
            else
            {
                offset = 2000;
                newContinuationToken = "";
            }

            int count = Math.Min(_dataSource.Count - offset, 1000);

            return new TableStorageServiceResult<T>
            {
               Entities = _dataSource.GetRange(offset, count),
               ContinuationToken = newContinuationToken,
        };
        }
    }
}