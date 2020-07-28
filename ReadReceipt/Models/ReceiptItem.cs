using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ReadReceipt.Models
{
    public class ReceiptItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public ObservableCollection<PairingItem> PairingItems { get; set; }
    }
}