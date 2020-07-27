using Xamarin.Forms;
using ReadReceipt.Services;
using ReadReceipt.Views;
using Plugin.Media;

namespace ReadReceipt
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();

            CrossMedia.Current.Initialize();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
