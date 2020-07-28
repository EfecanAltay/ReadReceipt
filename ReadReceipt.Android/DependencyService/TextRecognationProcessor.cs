using System;
using System.Collections.Generic;
using Android.Gms.Vision.Texts;
using Android.Util;
using Java.Interop;
using Java.Lang;
using ReadReceipt.Models;
using Xamarin.Forms;
using static Android.Gms.Vision.Detector;

namespace ReadReceipt.Droid.DependencyService
{
    class TextRecognationProcessor : Java.Lang.Object, IProcessor
    {
        public Action<IEnumerable<ImageTextBlock>> DetectedTextAction;
        public JniManagedPeerStates JniManagedPeerState => throw new NotImplementedException();

        public void Disposed()
        {
            //throw new NotImplementedException();
        }

        public void DisposeUnlessReferenced()
        {
            //throw new NotImplementedException();
        }

        public void Finalized()
        {
            //throw new NotImplementedException();
        }

        public void ReceiveDetections(Detections detections)
        {
            SparseArray items = detections.DetectedItems;
            if (items.Size() != 0)
            {
                List<ImageTextBlock> readingTextList = new List<ImageTextBlock>();
                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < items.Size(); ++i)
                {
                    var textBlock = (TextBlock)items.ValueAt(i);
                    var bbox = textBlock.BoundingBox;
                    readingTextList.Add(new ImageTextBlock()
                    {
                        Text = textBlock.Value,
                        Border = new Rectangle(bbox.Left, bbox.Top, bbox.Right - bbox.Left, bbox.Bottom - bbox.Top)
                    });
                }
                DetectedTextAction?.Invoke(readingTextList);
            }
        }

        public void Release()
        {
            //throw new NotImplementedException();
        }

        public void SetJniIdentityHashCode(int value)
        {
            //throw new NotImplementedException();
        }

        public void SetJniManagedPeerState(JniManagedPeerStates value)
        {
            //throw new NotImplementedException();
        }

        public void SetPeerReference(JniObjectReference reference)
        {
            // throw new NotImplementedException();
        }
    }
}