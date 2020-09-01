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
        private ObservableCollection<Receipt> items;
        public ObservableCollection<Receipt> Items
        {
            get { return items; }
            set { 
                items = value;
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
                //await DataStore.AddItemAsync(newItem);
            });

            SendMailCommand = new Command(OnSendMail);
            AddItemCommand = new Command(OnAddItem);
        }

        public ICommand SendMailCommand { get; set; }
        public ICommand AddItemCommand { get; set; }

        public async void OnSendMail()
        {
            if(Items != null && Items.Any())
                await SendEmail(Items, null);
        }

        public void OnAddItem()
        {
            if (Items == null)
            {
                Items = new ObservableCollection<Receipt>();
            }
            Items.Add(new Receipt() { Header = new ReceiptHeader() { Title = "Yeni Fiş" } });   
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
                });;

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