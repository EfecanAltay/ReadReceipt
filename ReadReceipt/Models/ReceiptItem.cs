using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReadReceipt.Models
{
    public class ReceiptItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public ObservableCollection<PairingItem> PairingItems { get; set; }

        public ICommand RemoveItem => new Command(async(item) =>
        {
            var pItem = (PairingItem)item;
            var result = await Application.Current.MainPage.DisplayAlert("Satır Silinecektir.", "Devam etmek ister misiniz ?", "Evet", "Hayır");
            if(result)
                PairingItems.Remove(pItem);
        });
    }
}