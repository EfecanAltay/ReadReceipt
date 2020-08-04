using Xamarin.Forms;
using Android.Gms.Vision.Texts;
using ReadReceipt.Dependencies;
using ReadReceipt.Droid.DependencyService;
using Android.Util;
using System;
using System.Collections.Generic;
using ReadReceipt.Models;
using Org.Opencv.Core;
using Size = Org.Opencv.Core.Size;
using Org.Opencv.Imgcodecs;
using Android.Graphics;
using Org.Opencv.Imgproc;
using Xamarin.Forms.Internals;
using Org.Apache.Http.Conn;

[assembly: Dependency(typeof(TextRecognizerDS))]
namespace ReadReceipt.Droid.DependencyService
{
    public class TextRecognizerDS : ITextRecognizer
    {
        TextRecognizer textRecognizer = null;

        public void Init()
        {
            textRecognizer = new TextRecognizer.Builder(Android.App.Application.Context).Build();
        }

        public void Read(byte[] data, Action<IEnumerable<ImageTextBlock>> readingTexts)
        {
            if (textRecognizer != null)
            {
                if (!textRecognizer.IsOperational)
                {
                    Android.Util.Log.Error("TextRecognizer", "Detector dependencies are not yet available !");
                }
                var textProcessor = new TextRecognationProcessor();
                textProcessor.DetectedTextAction = (texts) =>
                {
                    readingTexts?.Invoke(texts);
                };
                textRecognizer.SetProcessor(textProcessor);

                var filteredData = FilterReceipt(data);

                textRecognizer.ReceiveFrame(AndroidFrameHelper.GetFrame(filteredData));
            }
        }

        public byte[] OpenCv(byte[] imageBuff)
        {
            var m_src = ByteToMat(imageBuff);
            return CropReceiptPaper(m_src);
        }

        private byte[] FilterReceipt(byte[] imageBuff)
        {
            var m_src = ByteToMat(imageBuff);
            return CropReceiptPaper(m_src);
        }

        private byte[] CropReceiptPaper(Mat src)
        {
            Android.Runtime.JavaList<MatOfPoint> contors = new Android.Runtime.JavaList<MatOfPoint>();
            Mat gray = new Mat();
            Mat srcBlur = new Mat();
            Mat result = new Mat();

            Mat detectedEdges = new Mat();
            Mat hierarchy = new Mat();
           
            Imgproc.CvtColor(src, gray, Imgproc.ColorBgr2gray);

            Imgproc.MedianBlur(gray, srcBlur, 41);
            Imgproc.MedianBlur(srcBlur, srcBlur, 53);
            Imgproc.MedianBlur(srcBlur, srcBlur, 61);

            Imgproc.Threshold(srcBlur, srcBlur, 180, 255, Imgproc.ThreshBinary | Imgproc.ThreshOtsu);

            Imgproc.MedianBlur(srcBlur, srcBlur, 101);
            Imgproc.MedianBlur(srcBlur, srcBlur, 101);

            Imgproc.Canny(srcBlur, detectedEdges, 100, 200);
            Imgproc.FindContours(detectedEdges, contors, hierarchy, Imgproc.RetrList, Imgproc.ChainApproxSimple);

            var middlePoint = new Org.Opencv.Core.Point(detectedEdges.Width() / 2, detectedEdges.Height() / 2);
            Mat mask = new Mat(srcBlur.Rows(), srcBlur.Cols(), srcBlur.Type(), new Scalar(0));
            contors.ForEach(x =>
            {
                var rect = Imgproc.BoundingRect(x);
                if ((rect.Width < 100 || rect.Height < 100 || rect.Width * rect.Height < 2000) == false)
                {
                    if (rect.Contains(middlePoint))
                    {
                        Imgproc.Rectangle(src, new Org.Opencv.Core.Point(rect.X, rect.Y), new Org.Opencv.Core.Point(rect.X + rect.Width, rect.Y + rect.Height), new Scalar(0, 255, 0), 5);
                        Imgproc.Rectangle(mask, new Org.Opencv.Core.Point(rect.X, rect.Y), new Org.Opencv.Core.Point(rect.X + rect.Width, rect.Y + rect.Height), new Scalar(255, 255, 255), -1);
                    }
                }
            });

            Core.Bitwise_and(mask, srcBlur, mask);

            Mat mask2 = Mat.Zeros(mask.Rows() + 2, mask.Cols() + 2, CvType.Cv8u);
            Imgproc.FloodFill(mask, mask2, new Org.Opencv.Core.Point(), new Scalar(0, 0, 0), new Org.Opencv.Core.Rect(),
                new Scalar(0, 0, 0), new Scalar(0, 0, 0), 4 + (255 << 8) + Imgproc.FloodfillMaskOnly);

            Core.Bitwise_not(mask2, mask2);
            mask2 = new Mat(mask2, new Org.Opencv.Core.Rect(1, 1, mask2.Width() - 2, mask2.Height() - 2));
            Imgproc.CvtColor(mask2, mask2, Imgproc.ColorGray2bgr);
         
            Core.Bitwise_and(src, mask2, result);
            return MatToByteArray(result);
        }

        private Mat ByteToMat(byte[] imageBuffer)
        {
            return Imgcodecs.Imdecode(new MatOfByte(imageBuffer), -1);
        }

        private Byte[] MatToByteArray(Mat mat)
        {
            MatOfByte matOfByte = new MatOfByte();
            Imgcodecs.Imencode(".jpg", mat, matOfByte);
            return matOfByte.ToArray();
        }
    }
}