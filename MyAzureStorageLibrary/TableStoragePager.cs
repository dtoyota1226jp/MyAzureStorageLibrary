using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAzureStoragelibrary
{
    public class TableStoragePager<T> where T : ITableEntity
    {
        private const int MAX_RESPONSE = 1000;

        private ITableStorageService<T> _tableStorageService;



        public TableStoragePager(ITableStorageService<T> tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }

        public TableStoragePagerResult<T> Get(TableStoragePagerContext context, int pageNumber) 
        {
            // Table からデータ取得
            string continuationToken = "";
            int segment = context.PageSize * (pageNumber - 1) / MAX_RESPONSE;
            if(context.ContinuationTokens != null && segment > 0)
            {
                continuationToken = context.ContinuationTokens[segment - 1];
            }
            TableStorageServiceResult<T> result = _tableStorageService.Get(continuationToken);

            // ContinuationToken の保存
            List<string> tokens = new List<string>();
            if (context.ContinuationTokens != null)
            {
                tokens = context.ContinuationTokens.ToList();
            }
            if (!string.IsNullOrEmpty(result.ContinuationToken))
            {
                if (segment > context.MaxSegment)
                {
                    tokens.Add(result.ContinuationToken);
                }
            }

            // Table データ総件数の更新
            int totalEntityCount = context.TotalEntityCount;
            if(segment > context.MaxSegment)
            {
                totalEntityCount += result.Entities.Count;
            }

            // 未取得データの有無を判定＆保存
            int maxSegment = Math.Max(context.MaxSegment, segment);
            bool hasMore = context.HasMore;
            if (segment == maxSegment)
            {
                hasMore = !string.IsNullOrEmpty(result.ContinuationToken);
            }

            // 指定ページ番号のデータを抽出
            int offset = (pageNumber - 1) * context.PageSize % MAX_RESPONSE;
            int count = Math.Min(context.PageSize, result.Entities.Count - offset);
            List<T> currentEntities = result.Entities.GetRange(offset, count);

            // データとコンテキストを返却
            return new TableStoragePagerResult<T>
            {
                CurrentEntities = currentEntities,
                Context = new TableStoragePagerContext
                {
                    PageSize = context.PageSize,
                    ContinuationTokens = tokens.ToArray(),
                    HasMore = hasMore,
                    TotalEntityCount = totalEntityCount,
                    MaxSegment = maxSegment,
                }
            };
        }
    }
}