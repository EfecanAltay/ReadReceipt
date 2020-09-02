using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Util;
using Org.Opencv.Android;
using System;
using Android.Widget;

namespace ReadReceipt.Droid
{
    [Activity(Label = "ReadReceipt", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;

                base.OnCreate(savedInstanceState);

                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

                LoadApplication(new App());
            }
            catch (Exception e)
            {
                //unhandled Exception..
                Toast.MakeText(this, e.Message, ToastLength.Long).Show();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (!OpenCVLoader.InitDebug())
            {
                Log.Debug("OpenCV", "Internal OpenCV library not found. Using OpenCV Manager for initialization");
                OpenCVLoader.InitAsync(OpenCVLoader.OpencvVersion300, this, null);
            }
            else
            {
                Log.Debug("OpenCV", "OpenCV library found inside package. Using it!");
                //mLoaderCallback.onManagerConnected(LoaderCallbackInterface.SUCCESS);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}