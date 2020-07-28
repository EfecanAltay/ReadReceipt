using System.ComponentModel;
using Xamarin.Forms;
using ReadReceipt.Models;
using ReadReceipt.ViewModels;

namespace ReadReceipt.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }

        private void Editor_Focused(object sender, FocusEventArgs e)
        {

        }
    }
}