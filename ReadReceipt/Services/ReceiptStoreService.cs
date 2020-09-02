using ReadReceipt.Models;
using ReadReceipt.Services;
using System;
using System.Collections.Generic;
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

        public void GetAllReceiptGroup(Action<IEnumerable<ReceiptGroup>> success)
        {
            cachingService.GetAll<ReceiptGroup>().Subscribe(success);
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
    }
}
