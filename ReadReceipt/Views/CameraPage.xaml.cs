using Plugin.Media;
using ReadReceipt.Dependencies;
using ReadReceipt.Models;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReadReceipt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        ITextRecognizer _textRecognizer;
        public CameraPage()
        {
            InitializeComponent();
            _textRecognizer = DependencyService.Get<ITextRecognizer>();
            _textRecognizer.Init();
        }

        private async void Take_Receipt_Clicked(object sender, System.EventArgs e)
        {

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }

            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
            {
                cameraStream.Source = ImageSource.FromStream(() =>
                {
                    return photo.GetStream();
                });
            }
        }

        private void Read_Receipt_Clicked(object sender, System.EventArgs e)
        {
            StreamImageSource streamImageSource = (StreamImageSource)cameraStream.Source;
            System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
            Task<Stream> task = streamImageSource.Stream(cancellationToken);
            Stream stream = task.Result;
            var data = ReadFully(stream);
            _textRecognizer.Read(data, (texts) =>
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var text in texts)
                {
                    stringBuilder.Append(text);
                    stringBuilder.Append("\n");
                }
                var str = stringBuilder.ToString();

                MessagingCenter.Send(this, "AddItem", new Item()
                {
                    Text = DateTime.Now.ToString("hh/mm/ss"),
                    Description = str
                });
            });
        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}