using Android.Content;
using ReadReceipt.CutomViews;
using ReadReceipt.Droid.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace ReadReceipt.Droid.CustomViews
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context ctx) : base(ctx)
        {

        }

        public Entry caller;
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if(Control != null)
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
            }
        }
    }
}