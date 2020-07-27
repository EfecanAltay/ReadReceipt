using System;
using System.Collections.Generic;
using Android.Gms.Vision.Texts;
using Android.Util;
using Java.Interop;
using Java.Lang;
using Xamarin.Forms;
using static Android.Gms.Vision.Detector;

namespace ReadReceipt.Droid.DependencyService
{
    class TextRecognationProcessor : Java.Lang.Object, IProcessor
    {
        public Action<IEnumerable<string>> DetectedTextAction;
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
                List<string> readingTextList = new List<string>();
                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < items.Size(); ++i)
                {
                    readingTextList.Add(((TextBlock)items.ValueAt(i)).Value);
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