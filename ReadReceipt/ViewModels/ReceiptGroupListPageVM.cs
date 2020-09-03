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
        IShareService _shareService => DependencyService.Get<IShareService>();
        IReceiptStoreService _storeService => DependencyService.Get<IReceiptStoreService>();

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
        public ReceiptGroupListPageVM(INavigation nav = null) : base(nav)
        {
            ReceiptGroupList = new ObservableCollection<ReceiptGroup>();
            AddItemCommand = new Command<string>(OnAddItem);
            CheckCheckedCommand = new Command(OnCheckCheckedCommand);
            ShareAllCommand = new Command(OnShareAll);
        }

        private bool firstAppearing = true;
        public void OnAppearing()
        {
            AllSetCheck(false);
            if (firstAppearing)
            {
                firstAppearing = false;
                _storeService.GetAllReceiptGroup((receiptGroups) => {
                    if(receiptGroups != null)
                    {
                        ReceiptGroupList = new ObservableCollection<ReceiptGroup>(receiptGroups);
                    }
                    IsEmptyList = ReceiptGroupList.Any() == false;
                });
            }
        }

        public ICommand CheckCheckedCommand { get; set; }
        public ICommand AddItemCommand { get; set; }
        public ICommand ShareAllCommand { get; set; }

        public void OnAddItem(string name)
        {
            var newReceiptGroup = new ReceiptGroup() { GroupName = name };
            ReceiptGroupList.Add(newReceiptGroup);
            _storeService.SetReceiptGroup(newReceiptGroup);
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
            if (items.Any())
            {
                _storeService.RemoveReceiptGroup(items.Select(x => x.GroupName).ToArray());
                foreach (var item in items)
                {
                    ReceiptGroupList.Remove(item);
                }
                IsEmptyList = ReceiptGroupList.Any() == false;
            }
        }

        public async void ShareAllChecked()
        {
            var items = ReceiptGroupList.Where(x => x.IsChecked == true).ToArray();
            if (items.Any())
            {
                await _shareService.ShareAsExcell(items);
            }
        }

        public void OnCheckCheckedCommand()
        {
            checkedList = ReceiptGroupList.Where(x => x.IsChecked).ToArray();
            IsShowSelectMenu = checkedList.Any();
        }
    }
}
