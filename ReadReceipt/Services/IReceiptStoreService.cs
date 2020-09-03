using ReadReceipt.Models;
using System;
using System.Collections.Generic;

namespace ReadReceipt.Services
{
    public interface IReceiptStoreService
    {
        void GetAllReceiptGroup(Action<IEnumerable<ReceiptGroup>> success);
        void GetReceiptGroup(string groupName, Action<ReceiptGroup> success);
        bool SetReceiptGroup(ReceiptGroup receiptGroup);
        bool RemoveReceiptGroup(string groupName);
        bool RemoveReceiptGroup(IEnumerable<string> groupNames);
    }
}