using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using MyAzureStoragelibrary;

namespace MyAzureStoragelibraryTest
{
    internal class MockTableStorageService : ITableStorageService
    {
        private List<TableEntity> _dataSource;
        public MockTableStorageService(List<TableEntity> dataSource)
        {
            _dataSource = dataSource;
        }

        public TableStorageServiceResult Get(string continuationToken)
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

            return new TableStorageServiceResult
            {
               Entities = _dataSource.GetRange(offset, count),
               ContinuationToken = newContinuationToken,
        };
        }
    }
}