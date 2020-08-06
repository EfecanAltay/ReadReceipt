using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReadReceipt.Models
{
    public class Receipt
    {
        public string Id { get; set; }
        public ReceiptHeader Header { get; set; } = new ReceiptHeader();
        public ReceiptContent Content { get; set; } = new ReceiptContent();

        #region ctors
        public Receipt()
        {
        }

        public Receipt(ReceiptHeader header)
        {
            Header = header;
        }

        public Receipt(ReceiptContent content)
        {
            Content = content;
        }

        public Receipt(ReceiptHeader header, ReceiptContent content)
        {
            Header = header;
            Content = content;
        }
        #endregion
    }

    public class ReceiptHeader
    {
        public string No { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Title { get; set; }
        public string VD { get; set; } // Firmanın Vergi Dairesi veya Sicil
        public string Description { get; set; } //Tüm Header Kısmı içerisinde Title da bulunabilir
        public ReceiptInfo Infos { get; set; } = new ReceiptInfo();
        public override string ToString()
        {
            return Title;
        }
    }

    public class ReceiptInfo
    {
        public Dictionary<string, string> infos { get; set; }
    }

    public class ReceiptContent
    {
        public ObservableCollection<PairingItem> PairingItems { get; set; } = new ObservableCollection<PairingItem>();
        public ReceiptContent(IEnumerable<PairingItem> pairingItems = null)
        {
            if (pairingItems != null)
                PairingItems = new ObservableCollection<PairingItem>(pairingItems);
            else
                PairingItems = new ObservableCollection<PairingItem>();
        }

        public ICommand RemoveItem => new Command(async (item) =>
        {
            var pItem = (PairingItem)item;
            var result = await Application.Current.MainPage.DisplayAlert("Satır Silinecektir.", "Devam etmek ister misiniz ?", "Evet", "Hayır");
            if (result)
                PairingItems.Remove(pItem);
        });

        public ICommand AddItem => new Command(() =>
        {
            PairingItems.Add(new PairingItem());
        });
    }
}
