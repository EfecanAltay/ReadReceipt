using System.ComponentModel;
using Xamarin.Forms;
using ReadReceipt.Models;
using ReadReceipt.ViewModels;
using System.Linq;
using System.Collections.Generic;
using static ReadReceipt.Models.ReceiptHeader;

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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.OnDisAppearing();
        }

        private void Editor_Focused(object sender, FocusEventArgs e)
        {

        }

        private async void ToolbarDeleteItem_Clicked(object sender, System.EventArgs e)
        {
            var result = await Application.Current.MainPage.DisplayAlert("Fatura Silinecektir.", "Devam etmek ister misiniz ?", "Evet", "Hayır");
            if (result)
            {
                viewModel.DeleteItemCommand.Execute(null);
                Navigation.PopAsync();
            }
        }

        private void ToolbarSendItem_Clicked(object sender, System.EventArgs e)
        {

        }

        private void KDV1Add_Clicked(object sender, System.EventArgs e)
        {
            UpdateListSize(1);
        }

        private void KDV1CollapseButton_Clicked(object sender, System.EventArgs e)
        {
            if (kdv1List.IsVisible)
            {
                KDV1CollapseButton.Text = "↓";
                kdv1List.IsVisible = false;
            }
            else
            {
                KDV1CollapseButton.Text = "↑";
                UpdateListSize(1);
                kdv1List.IsVisible = true;
            }
        }

        private void KDV8Add_Clicked(object sender, System.EventArgs e)
        {
            UpdateListSize(8);
        }

        private void KDV8CollapseButton_Clicked(object sender, System.EventArgs e)
        {
            if (kdv8List.IsVisible)
            {
                KDV8CollapseButton.Text = "↓";
                kdv8List.IsVisible = false;
            }
            else
            {
                KDV8CollapseButton.Text = "↑";
                UpdateListSize(8);
                kdv8List.IsVisible = true;
            }
        }

        private void KDV18Add_Clicked(object sender, System.EventArgs e)
        {
            UpdateListSize(18);
        }

        private void KDV18CollapseButton_Clicked(object sender, System.EventArgs e)
        {
            if (kdv18List.IsVisible)
            {
                KDV18CollapseButton.Text = "↓";
                kdv18List.IsVisible = false;
            }
            else
            {
                KDV18CollapseButton.Text = "↑";
                UpdateListSize(18);
                kdv18List.IsVisible = true;
            }
        }

        private void KDV1Remove_Clicked(object sender, System.EventArgs e)
        {
            UpdateListSize(1);
        }

        private void KDV8Remove_Clicked(object sender, System.EventArgs e)
        {
            UpdateListSize(8);
        }

        private void KDV18Remove_Clicked(object sender, System.EventArgs e)
        {
            UpdateListSize(18);
        }

        private void UpdateListSize(int kdv)
        {
            switch (kdv)
            {
                case 1:
                    if (kdv1List != null && kdv1List.ItemsSource != null)
                    {
                        var items = kdv1List.ItemsSource as ICollection<KDVVal>;
                        kdv1List.HeightRequest = (items.Count + 1) * 50;
                    }
                    else
                    {
                        kdv1List.HeightRequest = 50;
                    }
                    break;
                case 8:
                    if (kdv8List != null && kdv8List.ItemsSource != null)
                    {
                        var items = kdv8List.ItemsSource as ICollection<KDVVal>;
                        kdv8List.HeightRequest = (items.Count + 1) * 50;
                    }
                    else
                    {
                        kdv8List.HeightRequest = 50;
                    }
                    break;
                case 18:
                    if (kdv18List != null && kdv18List.ItemsSource != null)
                    {
                        var items = kdv18List.ItemsSource as ICollection<KDVVal>;
                        kdv18List.HeightRequest = (items.Count + 1) * 50;
                    }
                    else
                    {
                        kdv18List.HeightRequest = 50;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}