using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadReceipt.Models;

namespace ReadReceipt.Services
{
    public class MockDataStore : IDataStore<ReceiptItem>
    {
        readonly List<ReceiptItem> items;

        public MockDataStore()
        {
            items = new List<ReceiptItem>()
            {
         
            };
        }

        public async Task<bool> AddItemAsync(ReceiptItem item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(ReceiptItem item)
        {
            var oldItem = items.Where((ReceiptItem arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((ReceiptItem arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<ReceiptItem> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ReceiptItem>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}