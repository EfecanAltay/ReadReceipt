using System.Collections.ObjectModel;
using Xamarin.Forms;
using ReadReceipt.Models;
using ReadReceipt.Views;

namespace ReadReceipt.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private ObservableCollection<ReceiptItem> items;
        public ObservableCollection<ReceiptItem> Items
        {
            get { return items; }
            set { items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public ItemsViewModel()
        {
            Title = "Fatura Listesi";
            Items = new ObservableCollection<ReceiptItem>();

            MessagingCenter.Subscribe<CameraPage, ReceiptItem>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as ReceiptItem;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }
    }
}