using System.Windows.Input;
using Xamarin.Forms;
using ReadReceipt.Models;

namespace ReadReceipt.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public ItemsViewModel _itemsViewModel = null;
        public ReceiptItem Item { get; set; }
        public ItemDetailViewModel(ReceiptItem item , ItemsViewModel itemsViewModel )
        {
            Title = item?.Text;
            Item = item;
            _itemsViewModel = itemsViewModel;
        }

        public ICommand DeleteItemCommand => new Command(() =>
        {
            _itemsViewModel?.Items.Remove(Item);
        });
    }
}
