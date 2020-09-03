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

        public string ToCSVFormat()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Gurup Adı;{GroupName}");
            if (Receipts != null && Receipts.Any())
            {
                builder.AppendLine(Receipt.CSVHeaderFormat());
                foreach (var receipt in Receipts)
                {
                    builder.AppendLine(receipt.ToCSVFormat());
                }     
            }
            else
            {
                builder.AppendLine(Receipt.CSVHeaderFormat());
                builder.AppendLine("Fiş Yok");
            }
            return builder.ToString();
        }
    }
}
