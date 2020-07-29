using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadReceipt.Models;

namespace ReadReceipt.Services
{
    public class MockDataStore : IDataStore<Receipt>
    {
        readonly List<Receipt> items;

        public MockDataStore()
        {
            items = new List<Receipt>()
            {
         
            };
        }

        public async Task<bool> AddItemAsync(Receipt item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Receipt item)
        {
            var oldItem = items.Where((Receipt arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Receipt arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Receipt> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Receipt>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}