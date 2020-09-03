using Xamarin.Forms;
using System.Collections.ObjectModel;
using System;
using System.Text;
using System.Linq;

namespace ReadReceipt.Models
{
    public class ReceiptGroup : BindableObject
    {
        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        private string groupName;
        public string GroupName
        {
            get { return groupName; }
            set { 
                groupName = value;
                OnPropertyChanged(nameof(GroupName));
            }
        }

        private ObservableCollection<Receipt> receipts { get; set; }
        public ObservableCollection<Receipt> Receipts
        {
            get { return receipts; }
            set
            {
                receipts = value;
                OnPropertyChanged(nameof(Receipts));
            }
        }

        public ReceiptGroup()
        {
            Receipts = new ObservableCollection<Receipt>();
        }
    }
}
