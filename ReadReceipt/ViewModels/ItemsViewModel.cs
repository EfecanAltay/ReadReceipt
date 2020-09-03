using System.Collections.ObjectModel;
using Xamarin.Forms;
using ReadReceipt.Models;
using ReadReceipt.Views;
using System.Windows.Input;
using System.Linq;
using ReadReceipt.Services;

namespace ReadReceipt.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        ShareService _shareService => DependencyService.Get<ShareService>();

        private ReceiptGroup receiptGroup;
        public ReceiptGroup ReceiptGroup
        {
            get { return receiptGroup; }
            set
            {
                receiptGroup = value;
                OnPropertyChanged(nameof(ReceiptGroup));
            }
        }

        private bool isEmptyList;
        public bool IsEmptyList
        {
            get { return isEmptyList; }
            set
            {
                isEmptyList = value;
                OnPropertyChanged(nameof(IsEmptyList));
            }
        }

        private bool isShowSelectMenu;
        public bool IsShowSelectMenu
        {
            get { return isShowSelectMenu; }
            set
            {
                isShowSelectMenu = value;
                OnPropertyChanged(nameof(IsShowSelectMenu));
            }
        }

        public ItemsViewModel(ReceiptGroup receiptGroup)
        {
            Title = "Fatura Listesi";
            this.ReceiptGroup = receiptGroup;
            MessagingCenter.Subscribe<CameraPage, Receipt>(this, "AddItem", (obj, item) =>
            {
                 var newItem = item as Receipt;
                 ReceiptGroup.Receipts.Add(newItem);
            });

            ShareAllCommand = new Command(OnShareAll);
            AddItemCommand = new Command<string>(OnAddItem);
            CheckCheckedCommand = new Command(OnCheckCheckedCommand);
        }

        public ICommand ShareAllCommand { get; set; }
        public ICommand AddItemCommand { get; set; }
        public ICommand CheckCheckedCommand { get; set; }

        public async void OnShareAll()
        {
            if (ReceiptGroup.Receipts != null && ReceiptGroup.Receipts.Any())
                await _shareService.ShareAsExcell(ReceiptGroup.Receipts);
        }

        public void OnAppearing()
        {
            AllSetCheck(false);
            if(ReceiptGroup != null && ReceiptGroup.Receipts != null)
                IsEmptyList = ReceiptGroup.Receipts.Any() == false;
        }

        public void AllSetCheck(bool check)
        {
            foreach (var receipt in ReceiptGroup.Receipts)
            {
                receipt.IsChecked = check;
            }
        }

        public void AllSetCheckToggle()
        {
            var items = ReceiptGroup.Receipts.Where(x => x.IsChecked == false);
            if (items.Any())
            {
                foreach (var receiptGroup in ReceiptGroup.Receipts)
                {
                    receiptGroup.IsChecked = true;
                }
            }
            else
            {
                foreach (var receiptGroup in ReceiptGroup.Receipts)
                {
                    receiptGroup.IsChecked = false;
                }
            }
        }

        public void DeleteAllChecked()
        {
            var items = ReceiptGroup.Receipts.Where(x => x.IsChecked == true).ToArray();
            foreach (var item in items)
            {
                ReceiptGroup.Receipts.Remove(item);
            }
            IsEmptyList = ReceiptGroup.Receipts.Any() == false;
        }

        public void OnCheckCheckedCommand()
        {
            if (ReceiptGroup != null && ReceiptGroup.Receipts != null)
                IsShowSelectMenu = ReceiptGroup.Receipts.Where(x => x.IsChecked).ToArray().Any();
        }

        public void OnAddItem(string receiptName)
        {
            if (ReceiptGroup.Receipts == null)
            {
                ReceiptGroup.Receipts = new ObservableCollection<Receipt>();
            }
            ReceiptGroup.Receipts.Add(new Receipt() { Header = new ReceiptHeader() { Title = receiptName } });
            IsEmptyList = false;
        }
    }
}