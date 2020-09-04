using ReadReceipt.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadReceipt.Services
{
    public interface IReceiptStoreService
    {
        Task<IEnumerable<ReceiptGroup>> GetAllReceiptGroup();
        void GetReceiptGroup(string groupName, Action<ReceiptGroup> success);
        bool SetReceiptGroup(ReceiptGroup receiptGroup);
        bool RemoveReceiptGroup(string groupName);
        bool RemoveReceiptGroup(IEnumerable<string> groupNames);
    }
}