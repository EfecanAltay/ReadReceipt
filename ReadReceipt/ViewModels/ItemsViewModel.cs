using System.Collections.ObjectModel;
using Xamarin.Forms;
using ReadReceipt.Models;
using ReadReceipt.Views;

namespace ReadReceipt.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private ObservableCollection<Receipt> items;
        public ObservableCollection<Receipt> Items
        {
            get { return items; }
            set { items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public ItemsViewModel()
        {
            Title = "Fatura Listesi";
            Items = new ObservableCollection<Receipt>();

            MessagingCenter.Subscribe<CameraPage, Receipt>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Receipt;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }
    }
}