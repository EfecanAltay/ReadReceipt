using System.Windows.Input;
using Xamarin.Forms;
using ReadReceipt.Models;
using ReadReceipt.Services;
using System.Collections.Generic;

namespace ReadReceipt.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        IReceiptStoreService _storeService = DependencyService.Get<IReceiptStoreService>();

        public Receipt Receipt { get; set; }
        public ReceiptGroup Group { get; set; }
        public ItemDetailViewModel(Receipt item, ReceiptGroup group = null, INavigation nav = null) : base(nav)
        {
            Receipt = item;
            this.Group = group;
            IsCamEditMode = group == null;
            if(group == null)
            {
                Title = "Yeni Fiş Girişi";
            }
            else
            {
                Title = item?.Header.Title;
            }
            SaveCommand = new Command(OnSave);
        }

        public async void OnAppearing()
        {
            if (IsCamEditMode)
            {
                ReceiptGroups = await _storeService.GetAllReceiptGroup();
            }
        }

        private bool isCamEditMode = false;
        public bool IsCamEditMode
        {
            get { return isCamEditMode; }
            set
            {
                isCamEditMode = value;
                OnPropertyChanged(nameof(IsCamEditMode));
            }
        }

        private IEnumerable<ReceiptGroup> receiptGroups ;
        public IEnumerable<ReceiptGroup> ReceiptGroups
        {
            get { return receiptGroups; }
            set
            {
                receiptGroups = value;
                OnPropertyChanged(nameof(ReceiptGroups));
            }
        }

        public ICommand DeleteItemCommand => new Command(() =>
        {
            this.Group?.Receipts.Remove(Receipt);
        });

        public ICommand SaveCommand { get; set; }

        public void OnSave()
        {
            if (Group != null)
            {
                Group.Receipts.Add(Receipt);
                _storeService.SetReceiptGroup(Group);
                BackNavigate();
                MessagingCenter.Send(this, "Updated");
            }
        }

        public void OnDisAppearing()
        {
            if (Group != null && IsCamEditMode == false)
            {
                _storeService.RemoveReceiptGroup(Group.GroupName);
                _storeService.SetReceiptGroup(Group);
                BackNavigate();
            }
        }
    }
}
