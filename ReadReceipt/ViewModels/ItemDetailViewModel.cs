using System.Windows.Input;
using Xamarin.Forms;
using ReadReceipt.Models;

namespace ReadReceipt.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public ItemsViewModel _itemsViewModel = null;
        public Receipt Receipt { get; set; }
        public ItemDetailViewModel(Receipt item , ItemsViewModel itemsViewModel )
        {
            Title = item?.Header.Title;
            Receipt = item;
            _itemsViewModel = itemsViewModel;
        }

        public ICommand DeleteItemCommand => new Command(() =>
        {
            _itemsViewModel?.ReceiptGroup.Receipts.Remove(Receipt);
        });
    }
}
