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
        ItemsViewModel viewModel;

        public ItemsPage(ReceiptGroup receiptGroup)
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel(receiptGroup);
            DependencyService.Get<IReceiptStoreService>().SetReceiptGroup(new ReceiptGroup());
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (Receipt)layout.BindingContext;
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item, viewModel)));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }

        private async void EditGroupName_Tapped(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Gurup Adı", "Yeni Gurup Adını giriniz",placeholder:"Yeni Gurup İsmi",initialValue: viewModel.ReceiptGroup.GroupName);
            if (string.IsNullOrEmpty(result) == false)
                viewModel.ReceiptGroup.GroupName = result;
            else
            {
                await DisplayAlert("Uyarı", "Bir Gurup ismi girmeden gurup oluşturamazsın!","OK");
            }
        }
    }
}