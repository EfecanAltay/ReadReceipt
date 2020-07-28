using System;

using ReadReceipt.Models;

namespace ReadReceipt.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public ReceiptItem Item { get; set; }
        public ItemDetailViewModel(ReceiptItem item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
