using ReadReceipt.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using Xamarin.Forms;
using ReadReceipt.Services;

namespace ReadReceipt.ViewModels
{
    public class ReceiptGroupListPageVM : BaseViewModel
    {
        ShareService _shareService => DependencyService.Get<ShareService>();

        private ObservableCollection<ReceiptGroup> receiptGroupList;
        public ObservableCollection<ReceiptGroup> ReceiptGroupList
        {
            get { return receiptGroupList; }
            set 
            {
                receiptGroupList = value;
                OnPropertyChanged(nameof(ReceiptGroupList));
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

        private ReceiptGroup[] checkedList;
        public ReceiptGroupListPageVM()
        {
            ReceiptGroupList = new ObservableCollection<ReceiptGroup>();
            AddItemCommand = new Command<string>(OnAddItem);
            CheckCheckedCommand = new Command(OnCheckCheckedCommand);
            ShareAllCommand = new Command(OnShareAll);
        }

        public void OnAppearing()
        {
            AllSetCheck(false);
            IsEmptyList = ReceiptGroupList.Any() == false;
        }

        public ICommand CheckCheckedCommand { get; set; }
        public ICommand AddItemCommand { get; set; }
        public ICommand ShareAllCommand { get; set; }

        public void OnAddItem(string name)
        {
            ReceiptGroupList.Add(new ReceiptGroup() { GroupName = name });
            IsEmptyList = false;
        }

        public async void OnShareAll()
        {
            if (ReceiptGroupList != null && ReceiptGroupList.Any())
                await _shareService.ShareAsExcell(ReceiptGroupList);
        }

        public void AllSetCheck(bool check)
        {
            foreach (var receiptGroup in ReceiptGroupList)
            {
                receiptGroup.IsChecked = check;
            }
        }

        public void AllSetCheckToggle()
        {
            var items = ReceiptGroupList.Where(x => x.IsChecked == false);
            if(items.Any())
            {
                foreach (var receiptGroup in ReceiptGroupList)
                {
                    receiptGroup.IsChecked = true;
                }
            }
            else
            {
                foreach (var receiptGroup in ReceiptGroupList)
                {
                    receiptGroup.IsChecked = false;
                }
            }
        }

        public void DeleteAllChecked()
        {
            var items = ReceiptGroupList.Where(x => x.IsChecked == true).ToArray();
            foreach (var item in items)
            {
                ReceiptGroupList.Remove(item);
            }
            IsEmptyList = ReceiptGroupList.Any() == false;
        }

        public void OnCheckCheckedCommand()
        {
            checkedList = ReceiptGroupList.Where(x => x.IsChecked).ToArray();
            IsShowSelectMenu = checkedList.Any();
        }
    }
}
