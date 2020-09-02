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
            Akavache.Registrations.Start(Consts.ApplicationName);
            DependencyService.Get<ICachingService>().Init();
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
