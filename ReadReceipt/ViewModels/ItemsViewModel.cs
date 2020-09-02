using System.Collections.ObjectModel;
using Xamarin.Forms;
using ReadReceipt.Models;
using ReadReceipt.Views;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Essentials;
using System.IO;
using System.Text;
using Xamarin.Forms.Internals;
using System;
using System.Windows.Input;
using System.Linq;

namespace ReadReceipt.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
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

        public ItemsViewModel(ReceiptGroup receiptGroup)
        {
            Title = "Fatura Listesi";
            this.ReceiptGroup = receiptGroup;
            MessagingCenter.Subscribe<CameraPage, Receipt>(this, "AddItem", (obj, item) =>
            {
                 var newItem = item as Receipt;
                 ReceiptGroup.Receipts.Add(newItem);
            });
            
            SendMailCommand = new Command(OnSendMail);
            AddItemCommand = new Command(OnAddItem);
        }

        public ICommand SendMailCommand { get; set; }
        public ICommand AddItemCommand { get; set; }

        public async void OnSendMail()
        {
            if (ReceiptGroup.Receipts != null && ReceiptGroup.Receipts.Any())
                await SendEmail(ReceiptGroup.Receipts, null);
        }

        public void OnAppearing()
        {
            AllSetCheck(false);
        }

        public void AllSetCheck(bool check)
        {
            foreach (var receipt in ReceiptGroup.Receipts)
            {
                receipt.IsChecked = check;
            }
        }

        public void OnAddItem()
        {
            if (ReceiptGroup.Receipts == null)
            {
                ReceiptGroup.Receipts = new ObservableCollection<Receipt>();
            }
            ReceiptGroup.Receipts.Add(new Receipt() { Header = new ReceiptHeader() { Title = "Yeni Fiş" } });
        }

        public async Task SendEmail(IEnumerable<Receipt> receipts, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = "FişMatik - Okunan Fişler",
                    Body = "Okunan Fişler Ektedir.",
                    To = recipients,
                };

                var fn = "Receipts.csv";
                var file = Path.Combine(FileSystem.CacheDirectory, fn);
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(Receipt.CSVHeaderFormat());
                receipts.ForEach(receipt =>
                {
                    builder.AppendLine(receipt.ToCSVFormat());
                }); ;

                File.WriteAllText(file, builder.ToString());

                message.Attachments.Add(new EmailAttachment(file));
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }
    }
}