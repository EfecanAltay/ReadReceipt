using System;
using System.ComponentModel;
using Xamarin.Forms;
using ReadReceipt.Models;
using ReadReceipt.ViewModels;
using ReadReceipt.Services;

namespace ReadReceipt.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel bindingContext;

        public ItemsPage(ReceiptGroup receiptGroup)
        {
            InitializeComponent();
            BindingContext = bindingContext = new ItemsViewModel(receiptGroup, Navigation);
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (Receipt)layout.BindingContext;
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item, bindingContext.ReceiptGroup, Navigation)));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Fiş Adı", "Yeni Fiş Adını giriniz", placeholder: "Fiş Şirket Adı");
            if (string.IsNullOrEmpty(result) == false)
                bindingContext.AddItemCommand.Execute(result);
            else
            {
                await DisplayAlert("Uyarı", "Bir Fiş ismi girmeden fiş oluşturamazsın!", "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            bindingContext.OnAppearing();
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bindingContext.CheckCheckedCommand.Execute(null);
        }

        private async void EditGroupName_Tapped(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Gurup Adı", "Yeni Gurup Adını giriniz", placeholder: "Yeni Gurup İsmi", initialValue: bindingContext.ReceiptGroup.GroupName);
            if (string.IsNullOrEmpty(result) == false && bindingContext.ReceiptGroup.GroupName.Equals(result) == false)
                bindingContext.ChangeGroupNameCommand.Execute(result);
            else
            {
                await DisplayAlert("Uyarı", "Bir Gurup ismi girmeden gurup oluşturamazsın!", "OK");
            }
        }

        private void TapGestureRecognizerSelectAll_Tapped(object sender, EventArgs e)
        {
            bindingContext.AllSetCheckToggle();
        }

        private async void TapGestureRecognizerSelectDelete_Tapped(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Sil", "Seçilenler Silinicektir. Eminmisiniz ?", "Evet", "İptal");
            if (result)
                bindingContext.DeleteAllChecked();
        }

        private void TapGestureRecognizerShareSelected_Tapped(object sender, EventArgs e)
        {
            bindingContext.ShareAllChecked();
        }
    }
}