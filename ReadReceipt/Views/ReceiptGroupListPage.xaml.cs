using ReadReceipt.Models;
using ReadReceipt.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReadReceipt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceiptGroupListPage : ContentPage
    {
        ReceiptGroupListPageVM bindingContext;
        public ReceiptGroupListPage()
        {
            InitializeComponent();
            BindingContext = bindingContext = new ReceiptGroupListPageVM();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            bindingContext.OnAppearing();
        }

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            var layout = (BindableObject)sender;
            var item = (ReceiptGroup)layout.BindingContext;
            await Navigation.PushAsync(new ItemsPage(item),false);
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
    }
}