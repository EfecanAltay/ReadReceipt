using ReadReceipt.Models;
using ReadReceipt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(ReceiptStoreService))]
namespace ReadReceipt.Services
{
    public class ReceiptStoreService : IReceiptStoreService
    {
        ICachingService cachingService;

        public ReceiptStoreService()
        {
            cachingService = DependencyService.Get<ICachingService>();
        }

        public async Task<IEnumerable<ReceiptGroup>> GetAllReceiptGroup()
        {
            return await cachingService.GetAll<ReceiptGroup>().Catch(Observable.Return(new List<ReceiptGroup>()));
        }

        public void GetReceiptGroup(string groupName,Action<ReceiptGroup> success)
        {
            cachingService.Get<ReceiptGroup>(groupName).Subscribe(success);
        }

        public bool SetReceiptGroup(ReceiptGroup receiptGroup)
        {
            if (string.IsNullOrEmpty(receiptGroup.GroupName)) return false;
            cachingService.Set<ReceiptGroup>(receiptGroup.GroupName,receiptGroup);
            return true;
        }

        public bool RemoveReceiptGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName)) return false;
            cachingService.Remove<ReceiptGroup>(groupName);
            return true;
        }

        public bool RemoveReceiptGroup(IEnumerable<string> groupNames)
        {
            if (groupNames == null || (groupNames != null && groupNames.Any() == false)) return false;
            cachingService.Remove<ReceiptGroup>(groupNames);
            return true;
        }
    }
}
