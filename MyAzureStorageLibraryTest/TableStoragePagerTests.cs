using NUnit.Framework;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Linq;
using MyAzureStoragelibrary;

namespace MyAzureStoragelibraryTest
{
    public class TableStoragePagerTests
    {
        List<TableEntity> _dataSource;


        [SetUp]
        public void Setup()
        {
            _dataSource = new List<TableEntity>();
            for(int i = 0; i < 2500; i++)
            {
                _dataSource.Add(
                    new TableEntity
                    {
                        PartitionKey = "PartitionKey",
                        RowKey = $"RowKey{i + 1}",
                    });
            }
        }

        [Test]
        public void A_ページ1()
        {
            ITableStorageService tableStorageService = new MockTableStorageService(_dataSource);
            TableStoragePagerContext context = new TableStoragePagerContext
            {
                PageSize = 200,
            };
            TableStoragePager pager = new TableStoragePager(tableStorageService);
            TableStoragePagerResult result = pager.Get(context, 1);

            Assert.AreEqual("RowKey1", result.CurrentEntities.First().RowKey);
            Assert.AreEqual("RowKey200", result.CurrentEntities.Last().RowKey);
            Assert.AreEqual(1000, result.Context.TotalEntityCount);
            Assert.AreEqual(true, result.Context.HasMore);
            Assert.AreEqual(5, result.Context.GetMaxPageNumber());
        }

        [Test]
        public void B_ページ1_5()
        {
            ITableStorageService tableStorageService = new MockTableStorageService(_dataSource);
            TableStoragePagerContext context = new TableStoragePagerContext
            {
                PageSize = 200,
            };
            TableStoragePager pager = new TableStoragePager(tableStorageService);
            TableStoragePagerResult result = pager.Get(context, 5);

            Assert.AreEqual("RowKey801", result.CurrentEntities.First().RowKey);
            Assert.AreEqual("RowKey1000", result.CurrentEntities.Last().RowKey);
            Assert.AreEqual(1000, result.Context.TotalEntityCount);
            Assert.AreEqual(true, result.Context.HasMore);
            Assert.AreEqual(5, result.Context.GetMaxPageNumber());
        }

        [Test]
        public void C_ページ1_6()
        {
            ITableStorageService tableStorageService = new MockTableStorageService(_dataSource);
            TableStoragePagerContext context = new TableStoragePagerContext
            {
                PageSize = 200,
            };
            TableStoragePager pager = new TableStoragePager(tableStorageService);
            
            TableStoragePagerResult result1 = pager.Get(context, 1);
            TableStoragePagerResult result6 = pager.Get(result1.Context, 6);

            Assert.AreEqual("RowKey1001", result6.CurrentEntities.First().RowKey);
            Assert.AreEqual("RowKey1200", result6.CurrentEntities.Last().RowKey);
            Assert.AreEqual(2000, result6.Context.TotalEntityCount);
            Assert.AreEqual(true, result6.Context.HasMore);
            Assert.AreEqual(10, result6.Context.GetMaxPageNumber());
        }

        [Test]
        public void D_ページ1_10()
        {
            ITableStorageService tableStorageService = new MockTableStorageService(_dataSource);
            TableStoragePagerContext context = new TableStoragePagerContext
            {
                PageSize = 200,
            };
            TableStoragePager pager = new TableStoragePager(tableStorageService);

            TableStoragePagerResult result1 = pager.Get(context, 1);
            TableStoragePagerResult result10 = pager.Get(result1.Context, 10);

            Assert.AreEqual("RowKey1801", result10.CurrentEntities.First().RowKey);
            Assert.AreEqual("RowKey2000", result10.CurrentEntities.Last().RowKey);
            Assert.AreEqual(2000, result10.Context.TotalEntityCount);
            Assert.AreEqual(true, result10.Context.HasMore);
            Assert.AreEqual(10, result10.Context.GetMaxPageNumber());
        }

        [Test]
        public void E_ページ1_6_11()
        {
            ITableStorageService tableStorageService = new MockTableStorageService(_dataSource);
            TableStoragePagerContext context = new TableStoragePagerContext
            {
                PageSize = 200,
            };
            TableStoragePager pager = new TableStoragePager(tableStorageService);

            TableStoragePagerResult result1 = pager.Get(context, 1);
            TableStoragePagerResult result6 = pager.Get(result1.Context, 6);
            TableStoragePagerResult result11 = pager.Get(result6.Context, 11);

            Assert.AreEqual("RowKey2001", result11.CurrentEntities.First().RowKey);
            Assert.AreEqual("RowKey2200", result11.CurrentEntities.Last().RowKey);
            Assert.AreEqual(2500, result11.Context.TotalEntityCount);
            Assert.AreEqual(false, result11.Context.HasMore);
            Assert.AreEqual(13, result11.Context.GetMaxPageNumber());
        }

        [Test]
        public void F_ページ1_6_11_13()
        {
            ITableStorageService tableStorageService = new MockTableStorageService(_dataSource);
            TableStoragePagerContext context = new TableStoragePagerContext
            {
                PageSize = 200,
            };
            TableStoragePager pager = new TableStoragePager(tableStorageService);

            TableStoragePagerResult result1 = pager.Get(context, 1);
            TableStoragePagerResult result6 = pager.Get(result1.Context, 6);
            TableStoragePagerResult result11 = pager.Get(result6.Context, 11);
            TableStoragePagerResult result13 = pager.Get(result11.Context, 13);

            Assert.AreEqual("RowKey2401", result13.CurrentEntities.First().RowKey);
            Assert.AreEqual("RowKey2500", result13.CurrentEntities.Last().RowKey);
            Assert.AreEqual(2500, result13.Context.TotalEntityCount);
            Assert.AreEqual(false, result13.Context.HasMore);
            Assert.AreEqual(13, result13.Context.GetMaxPageNumber());
        }

        [Test]
        public void G_ページ1_6_11_13_10()
        {
            ITableStorageService tableStorageService = new MockTableStorageService(_dataSource);
            TableStoragePagerContext context = new TableStoragePagerContext
            {
                PageSize = 200,
            };
            TableStoragePager pager = new TableStoragePager(tableStorageService);

            TableStoragePagerResult result1 = pager.Get(context, 1);
            TableStoragePagerResult result6 = pager.Get(result1.Context, 6);
            TableStoragePagerResult result11 = pager.Get(result6.Context, 11);
            TableStoragePagerResult result13 = pager.Get(result11.Context, 13);
            TableStoragePagerResult result10 = pager.Get(result13.Context, 10);

            Assert.AreEqual("RowKey1801", result10.CurrentEntities.First().RowKey);
            Assert.AreEqual("RowKey2000", result10.CurrentEntities.Last().RowKey);
            Assert.AreEqual(2500, result10.Context.TotalEntityCount);
            Assert.AreEqual(false, result10.Context.HasMore);
            Assert.AreEqual(13, result10.Context.GetMaxPageNumber());
        }

        [Test]
        public void H_ページ1_6_11_13_10_6()
        {
            ITableStorageService tableStorageService = new MockTableStorageService(_dataSource);
            TableStoragePagerContext context = new TableStoragePagerContext
            {
                PageSize = 200,
            };
            TableStoragePager pager = new TableStoragePager(tableStorageService);

            TableStoragePagerResult result1 = pager.Get(context, 1);
            TableStoragePagerResult result6a = pager.Get(result1.Context, 6);
            TableStoragePagerResult result11 = pager.Get(result6a.Context, 11);
            TableStoragePagerResult result13 = pager.Get(result11.Context, 13);
            TableStoragePagerResult result10 = pager.Get(result13.Context, 10);
            TableStoragePagerResult result6b = pager.Get(result10.Context, 6);

            Assert.AreEqual("RowKey1001", result6b.CurrentEntities.First().RowKey);
            Assert.AreEqual("RowKey1200", result6b.CurrentEntities.Last().RowKey);
            Assert.AreEqual(2500, result6b.Context.TotalEntityCount);
            Assert.AreEqual(false, result6b.Context.HasMore);
            Assert.AreEqual(13, result6b.Context.GetMaxPageNumber());
        }

        [Test]
        public void I_ページ1_6_11_13_10_6_1()
        {
            ITableStorageService tableStorageService = new MockTableStorageService(_dataSource);
            TableStoragePagerContext context = new TableStoragePagerContext
            {
                PageSize = 200,
            };
            TableStoragePager pager = new TableStoragePager(tableStorageService);

            TableStoragePagerResult result1a = pager.Get(context, 1);
            TableStoragePagerResult result6a = pager.Get(result1a.Context, 6);
            TableStoragePagerResult result11 = pager.Get(result6a.Context, 11);
            TableStoragePagerResult result13 = pager.Get(result11.Context, 13);
            TableStoragePagerResult result10 = pager.Get(result13.Context, 10);
            TableStoragePagerResult result6b = pager.Get(result10.Context, 6);
            TableStoragePagerResult result1b = pager.Get(result6b.Context, 1);

            Assert.AreEqual("RowKey1", result1b.CurrentEntities.First().RowKey);
            Assert.AreEqual("RowKey200", result1b.CurrentEntities.Last().RowKey);
            Assert.AreEqual(2500, result1b.Context.TotalEntityCount);
            Assert.AreEqual(false, result1b.Context.HasMore);
            Assert.AreEqual(13, result1b.Context.GetMaxPageNumber());
        }
    }
}