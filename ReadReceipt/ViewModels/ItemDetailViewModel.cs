using System.Windows.Input;
using Xamarin.Forms;
using ReadReceipt.Models;
using ReadReceipt.Services;

namespace ReadReceipt.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        IReceiptStoreService _storeService = DependencyService.Get<IReceiptStoreService>();

        public ItemsViewModel _itemsViewModel = null;
        public Receipt Receipt { get; set; }
        private ReceiptGroup group = null;
        public ItemDetailViewModel(Receipt item, ItemsViewModel itemsViewModel,INavigation nav=null) :base(nav)
        {
            Title = item?.Header.Title;
            Receipt = item;
            _itemsViewModel = itemsViewModel;
            this.group = itemsViewModel.ReceiptGroup;
            SaveCommand = new Command(OnSave);
        }

        public ICommand DeleteItemCommand => new Command(() =>
        {
            _itemsViewModel?.ReceiptGroup.Receipts.Remove(Receipt);
        });

        public ICommand SaveCommand { get; set; }

        public void OnSave()
        {
            if (group != null)
            {
                _storeService.SetReceiptGroup(group);
                BackNavigate();
            }
        }

        public void OnDisAppearing()
        {
            if (group != null)
            {
                _storeService.RemoveReceiptGroup(group.GroupName);
                _storeService.SetReceiptGroup(group);
            }
        }
    }
}
