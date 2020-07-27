using Android.Graphics;
using Java.Nio;

namespace ReadReceipt.Droid.DependencyService
{
    public static class AndroidFrameHelper
    {
        public static Android.Gms.Vision.Frame GetFrame(Bitmap bitmap)
        {
            var builder = new Android.Gms.Vision.Frame.Builder();
            builder.SetBitmap(bitmap);
            return builder.Build();
        }

        public static Android.Gms.Vision.Frame GetFrame(byte[] byteArray)
        {
            var builder = new Android.Gms.Vision.Frame.Builder();
            builder.SetBitmap(ConvertCompressedByteArrayToBitmap(byteArray));
            return builder.Build();
        }

        public static byte[] ConvertBitmapToByteArrayUncompressed(Bitmap bitmap)
        {
            ByteBuffer byteBuffer = ByteBuffer.Allocate(bitmap.ByteCount);
            bitmap.CopyPixelsToBuffer(byteBuffer);
            byteBuffer.Rewind();
            return byteBuffer.ToArray<byte>();
        }
        public static Bitmap ConvertCompressedByteArrayToBitmap(byte[] src)
        {
            return BitmapFactory.DecodeByteArray(src, 0, src.Length);
        }
    }
}