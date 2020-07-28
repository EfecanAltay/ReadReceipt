using Xamarin.Forms;

namespace ReadReceipt.Models
{
    public class ImageTextBlock
    {
        public string Text { get; set; }
        public Rectangle Border { get; set; }
        public TextBlockType TextBlockType { get; set; } = TextBlockType.None;
    }

    public enum TextBlockType
    {
        None,
        Key,
        Value
    }
}
