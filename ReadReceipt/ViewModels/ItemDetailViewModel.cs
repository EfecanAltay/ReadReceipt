using System.Windows.Input;
using Xamarin.Forms;
using ReadReceipt.Models;

namespace ReadReceipt.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public ItemsViewModel _itemsViewModel = null;
        public Receipt Item { get; set; }
        public ItemDetailViewModel(Receipt item , ItemsViewModel itemsViewModel )
        {
            Title = item?.Header.Title;
            Item = item;
            _itemsViewModel = itemsViewModel;
        }

        public ICommand DeleteItemCommand => new Command(() =>
        {
            _itemsViewModel?.Items.Remove(Item);
        });
    }
}
