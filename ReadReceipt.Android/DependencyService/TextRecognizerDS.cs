using Xamarin.Forms;
using Android.Gms.Vision.Texts;
using ReadReceipt.Dependencies;
using ReadReceipt.Droid.DependencyService;
using Android.Util;
using System;
using System.Collections.Generic;
using ReadReceipt.Models;

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
            if(textRecognizer != null)
            {
                if (!textRecognizer.IsOperational)
                {
                    Log.Error("TextRecognizer", "Detector dependencies are not yet available !");
                }
                var textProcessor = new TextRecognationProcessor();
                textProcessor.DetectedTextAction = (texts) => {
                    readingTexts?.Invoke(texts);
                };
                textRecognizer.SetProcessor(textProcessor);
                textRecognizer.ReceiveFrame(AndroidFrameHelper.GetFrame(data));
            }
        }
    }
}