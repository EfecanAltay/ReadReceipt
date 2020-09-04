using ReadReceipt.Models;
using ReadReceipt.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReadReceipt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceiptGroupListPage : ContentPage
    {
        ReceiptGroupListPageVM bindingContext;
        bool HelperAnimTaskRunning = true;
        public ReceiptGroupListPage()
        {
            InitializeComponent();
            BindingContext = bindingContext = new ReceiptGroupListPageVM(Navigation);
            bindingContext.PropertyChanged += BindingContext_PropertyChanged;
            MessagingCenter.Subscribe<ItemDetailViewModel>(this, "Updated",(sender) =>
            {
                bindingContext.UpdateList();
            });
        }

        private void BindingContext_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(bindingContext.IsEmptyList)))
            {
                HelperAnimTaskRunning = true;
                if (bindingContext.IsEmptyList)
                {
                    Task.Run(async () =>
                    {
                        while (true)
                        {
                            await helper.TranslateTo(-50, 0, 500, Easing.CubicIn);
                            await helper.TranslateTo(-50, 50, 500, Easing.CubicOut);
                            if (HelperAnimTaskRunning == false)
                                break;
                        }
                    });
                }
                else
                {
                    HelperAnimTaskRunning = false;
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            HelperAnimTaskRunning = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            HelperAnimTaskRunning = true;
            bindingContext.OnAppearing();
        }

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            var layout = (BindableObject)sender;
            var item = (ReceiptGroup)layout.BindingContext;
            await Navigation.PushAsync(new ItemsPage(item), false);
        }

        private async void ToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            string result = await DisplayPromptAsync("Gurup Adı", "Yeni Gurup Adını giriniz");
            if (string.IsNullOrEmpty(result) == false)
                bindingContext.AddItemCommand.Execute(result);
            else
            {
                await DisplayAlert("Uyarı", "Bir Gurup ismi girmeden gurup oluşturamazsın!", "OK");
            }
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bindingContext.CheckCheckedCommand.Execute(null);
        }

        private void TapGestureRecognizer_Tapped_1(object sender, System.EventArgs e)
        {
            bindingContext.AllSetCheckToggle();
        }

        private async void TapGestureRecognizerDelete_Tapped(object sender, System.EventArgs e)
        {
            bool result = await DisplayAlert("Sil", "Seçilenler Silinicektir. Eminmisiniz ?","Evet","İptal");
            if (result)
                bindingContext.DeleteAllChecked();
        }

        private void TapGestureRecognizerShareSelected_Tapped(object sender, System.EventArgs e)
        {
            bindingContext.ShareAllChecked();
        }
    }
}