using ReadReceipt.Models;
using System;
using System.Collections.Generic;

namespace ReadReceipt.Dependencies
{
    public interface ITextRecognizer
    {
        void Init();
        void Read(byte[] data, Action<IEnumerable<ImageTextBlock>> callback);
    }
}
