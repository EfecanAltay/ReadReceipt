#define B
using Plugin.Media;
using ReadReceipt.Dependencies;
using ReadReceipt.Models;
using ReadReceipt.Utility;
using ReadReceipt.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ReadReceipt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        ITextRecognizer _textRecognizer;
        Image cameraImage;
        SKImage skCameraImage;
        IEnumerable<ImageTextBlock> textBlockList;
        List<ImageTextBlock> WordList = new List<ImageTextBlock>();
        List<ImageTextBlock> FilteredWordList = new List<ImageTextBlock>();

        Rectangle ReceiptPaperBorder;
        string ReceiptTitle = "None";
        Rectangle ReceiptTitleBorder;

        Action ReadyToSave = null;
        Dictionary<ImageTextBlock, ImageTextBlock> pairingList = new Dictionary<ImageTextBlock, ImageTextBlock>();
        private bool isImageDetected = false;
        public CameraPage()
        {
            InitializeComponent();
            cameraImage = new Image();
            camCanvas.PaintSurface += OnCanvasViewPaintSurface;
            _textRecognizer = DependencyService.Get<ITextRecognizer>();
            _textRecognizer.Init();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "OnAppearing");
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            if (skCameraImage != null)
            {
                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;
                canvas.Clear();
                canvas.DrawImage(skCameraImage, new SKRect(0, 0, info.Width, info.Height));
                if (isImageDetected)
                {
                    var scaleRatioW = (float)info.Width / (float)skCameraImage.Width;
                    var scaleRatioH = (float)info.Height / (float)skCameraImage.Height;

                    #region Paint Palets
                    SKPaint paint = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = Color.Red.ToSKColor().WithAlpha(100),
                        StrokeWidth = 2
                    };

                    SKPaint paint_blue = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = Color.Blue.ToSKColor(),
                        StrokeWidth = 2
                    };

                    SKPaint paint_green = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = Color.Green.ToSKColor(),
                        StrokeWidth = 4
                    };

                    SKPaint paint_yellow = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = Color.Yellow.ToSKColor(),
                        StrokeWidth = 4
                    };

                    SKPaint paint_purple = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = Color.Purple.ToSKColor(),
                        StrokeWidth = 4
                    };
                    #endregion

                    CalculateTextRects(scaleRatioW, scaleRatioH, canvas, paint);

                    canvas.DrawRect((float)ReceiptPaperBorder.X, (float)ReceiptPaperBorder.Y, (float)ReceiptPaperBorder.Width, (float)ReceiptPaperBorder.Height, paint_green);

                    ReadyToSave?.Invoke();
                    //##Draw Connections
                    pairingList.ForEach((k) =>
                    {
                        var keyBorder = k.Key.Border;
                        var valueBorder = k.Value.Border;
                        canvas.DrawRect((float)keyBorder.X, (float)keyBorder.Y, (float)keyBorder.Width, (float)keyBorder.Height, paint_yellow);
                        canvas.DrawRect((float)valueBorder.X, (float)valueBorder.Y, (float)valueBorder.Width, (float)valueBorder.Height, paint_yellow);
                        canvas.DrawLine((float)keyBorder.Right, (float)keyBorder.Center.Y, (float)valueBorder.Left, (float)valueBorder.Center.Y, paint_green);
                    });
                }
            }
        }

        private void CalculateTextRects(float scaleRatioW, float scaleRatioH, SKCanvas canvas, SKPaint wordPaint)
        {
            ReceiptPaperBorder = new Rectangle();
            ReceiptPaperBorder.Width = -1;
            ReceiptTitleBorder.Width = -1;
            WordList.Clear();
            FilteredWordList.Clear();
            pairingList.Clear();

            if (textBlockList != null && textBlockList.Any())
            {
                #region Calculate_ALL_Word_Border
                textBlockList.ForEach(textBlock =>
                {
                    var rect = textBlock.Border;
                    if (ReceiptTitleBorder.Width == -1)
                    {
                        ReceiptTitleBorder = rect;
                        ReceiptTitle = textBlock.Text;
                    }
                    else if (ReceiptTitleBorder.Top > rect.Top)
                    {
                        ReceiptTitleBorder = rect;
                        ReceiptTitle = textBlock.Text;
                    }
                    canvas.DrawRect((float)rect.X * scaleRatioW, (float)rect.Y * scaleRatioH, (float)rect.Width * scaleRatioW, (float)rect.Height * scaleRatioH, wordPaint);
                    var words = textBlock.Text.Split('\n');
                    var devide = (float)((rect.Height * scaleRatioH) / words.Length);
                    for (int i = 0; i < words.Length; i++)
                    {
                        float offset = (float)(i * devide);
                        var text = words[i];
                        var borderRect = new Rectangle(rect.X * scaleRatioW, (rect.Y * scaleRatioH) + offset, rect.Width * scaleRatioW, devide);
                        //canvas.DrawRect((float)borderRect.X, (float)borderRect.Y, (float)borderRect.Width, (float)borderRect.Height, paint_blue);
                        WordList.Add(new ImageTextBlock() { Text = text, Border = borderRect });
                        if (ReceiptPaperBorder.Width == -1)
                        {
                            ReceiptPaperBorder = borderRect;
                        }
                        else
                        {
                            if (borderRect.X < ReceiptPaperBorder.X)
                                ReceiptPaperBorder.X = borderRect.X;
                            if (borderRect.Right > ReceiptPaperBorder.Right)
                                ReceiptPaperBorder.Right = borderRect.Right;

                            var stext = text.Trim().ToLower();
                            if (stext.Contains("saat"))
                            {
                                ReceiptPaperBorder.Y = borderRect.Bottom;
                            }
                            else if (stext.Contains("naktt") || stext.Contains("nakit"))
                            {
                                ReceiptPaperBorder.Bottom = borderRect.Top;
                            }
                            else if (stext.Contains("toplam") && stext.Contains("ara toplam") == false && ReceiptPaperBorder.Bottom < borderRect.Top)
                            {
                                ReceiptPaperBorder.Bottom = borderRect.Bottom + 20;
                            }
                            else if ((stext.Contains("lopkdv") || stext.Contains("topkdv")) && ReceiptPaperBorder.Bottom < borderRect.Top)
                            {
                                ReceiptPaperBorder.Bottom = borderRect.Bottom + 40;
                            }
                        }
                    }
                });

                //Control for Receipt Borders

                #endregion

                #region Content Key Value Filtered
                Rectangle KeysBorder = ReceiptPaperBorder;
                KeysBorder.Right = ReceiptPaperBorder.Left + ReceiptPaperBorder.Width / 3;

                Rectangle ValuesBorder = ReceiptPaperBorder;
                ValuesBorder.Left = ReceiptPaperBorder.Right - ReceiptPaperBorder.Width / 3;

                WordList.ForEach(word =>
                {
                    var border = word.Border;
                    if (ReceiptPaperBorder.Contains(border.Center))
                    {
                        if (FilterRules.IsReceitItem(word.Text))
                        {
                            FilteredWordList.Add(word);

                            if (KeysBorder.Contains(border.Center))
                            {
                                word.TextBlockType = TextBlockType.Key;
                                //canvas.DrawRect((float)border.X, (float)border.Y, (float)border.Width, (float)border.Height, paint_blue);
                            }
                            else if (ValuesBorder.Contains(border.Center))
                            {
                                word.Text = FilterRules.TrimforValue(word.Text);
                                word.TextBlockType = TextBlockType.Value;
                                //canvas.DrawRect((float)border.X, (float)border.Y, (float)border.Width, (float)border.Height, paint_yellow);
                            }
                        }
                    }
                });
                #endregion

                #region Pairing
                var keys = FilteredWordList.Where(w => w.TextBlockType.Equals(TextBlockType.Key));
                var values = FilteredWordList.Where(w => w.TextBlockType.Equals(TextBlockType.Value));
                List<ImageTextBlock> valuePairingList = new List<ImageTextBlock>(values);

                keys.ForEach(key =>
                {
                    var rawRect = key.Border;
                    rawRect.Right = ReceiptPaperBorder.Right;
                    //canvas.DrawRect((float)rawRect.X, (float)rawRect.Y, (float)rawRect.Width, (float)rawRect.Height, paint_blue);
                    ImageTextBlock selectedValue = null;
                    valuePairingList.ForEach(value =>
                    {
                        if (rawRect.Contains(value.Border.Center))
                            selectedValue = value;
                    });
                    if (selectedValue != null)
                    {
                        pairingList.Add(key, selectedValue);
                        valuePairingList.Remove(selectedValue);
                    }
                });
                #endregion
            }
        }

        public Dictionary<string, ImageTextBlock> FindText(IEnumerable<ImageTextBlock> imageTextBlocks, IEnumerable<string> searchWords)
        {
            Dictionary<string, ImageTextBlock> returnedList = null;
            if (imageTextBlocks != null)
            {
                foreach (var textblock in imageTextBlocks)
                {
                    var text = textblock.Text.ToLower();
                    foreach (var searchWord in searchWords)
                    {
                        var searcingWord = searchWord.ToLower();
                        if (text.Contains(searcingWord))
                        {
                            if (returnedList == null)
                                returnedList = new Dictionary<string, ImageTextBlock>();

                            if (returnedList.Any() == false || returnedList.ContainsKey(searcingWord) == false)
                            {
                                returnedList.Add(searcingWord, textblock);
                            }
                        }
                    }
                }
            }
            return returnedList;
        }

        public ImageTextBlock FindText(IEnumerable<ImageTextBlock> imageTextBlocks, string searchWord)
        {
            ImageTextBlock searchingText = null;
            if (imageTextBlocks != null)
            {
                foreach (var textblock in imageTextBlocks)
                {
                    var text = textblock.Text;
                    if (text.ToLower().Contains(searchWord))
                    {
                        return textblock;
                    }
                }
            }
            return searchingText;
        }

        public string FindValue(IEnumerable<ImageTextBlock> texts, string searchWord, bool before = false)
        {
            var findingName = FindText(texts, searchWord);
            if (findingName != null)
            {
                string n_SearchWord = null;
                if (searchWord.Contains(" "))
                {
                    n_SearchWord = searchWord.Replace(" ", "_");
                    findingName.Text = findingName.Text.ToLower().Replace(searchWord.ToLower(), n_SearchWord);
                }
                if (n_SearchWord == null)
                    return GetValueInText(findingName.Text, searchWord, before);
                else
                    return GetValueInText(findingName.Text, n_SearchWord, before);
            }
            return null;
        }

        public string FindValue(IEnumerable<ImageTextBlock> texts, IEnumerable<string> searchWords, bool before = false)
        {
            var findingName = FindText(texts, searchWords);
            if (findingName != null && findingName.Any())
            {
                var KeyValuePair = findingName.First();
                string n_SearchWord = null;
                if (KeyValuePair.Key.Contains(" "))
                {
                    n_SearchWord = KeyValuePair.Key.Replace(" ", "_").ToLower();
                    KeyValuePair.Value.Text = KeyValuePair.Value.Text.ToLower().Replace(KeyValuePair.Key.ToLower(), n_SearchWord);
                }
                if (n_SearchWord == null)
                    return GetValueInText(KeyValuePair.Value.Text, KeyValuePair.Key, before);
                else
                    return GetValueInText(KeyValuePair.Value.Text, n_SearchWord, before);
            }
            return null;
        }

        public string GetRowString(IEnumerable<ImageTextBlock> texts, IEnumerable<string> searchWords)
        {
            var findingNames = FindText(texts, searchWords);
            if (findingNames != null && findingNames.Any())
            {
                foreach (var findingName in findingNames)
                {
                    var Rows = findingName.Value.Text.Split('\n');
                    foreach (var searchingWord in searchWords)
                    {
                        var findingRows = Rows.Where(x => x.ToLower().Contains(searchingWord));
                        if (findingRows.Any())
                            return findingRows.First();
                    }
                }
            }
            return null;
        }

        public string GetValueInText(string text, string Key, bool before = false)
        {
            var index = text.ToLower().IndexOf(Key.ToLower());
            if (before == false)
            {
                var valueString = text.Substring(index + Key.Length);
                var values = valueString.Replace("\n", " ").Replace("  ", " ").Split(' ');
                if (values[0] != ":")
                {
                    return values[0];
                }
                else if (values.Length > 1)
                {
                    return values[1];
                }
            }
            else
            {
                var valueString = text.Substring(0, index);
                var values = valueString.Replace("\n", " ").Replace("  ", " ").Split(' ');
                if (values.Any())
                {
                    var returingText = values.Last();
                    if (string.IsNullOrEmpty(returingText.Trim()) == false)
                    {
                        if (returingText.Length < 4 && values.Length > 2)
                            return values[values.Length - 2] + returingText;
                        else
                            return returingText;
                    }
                    else if (values.Length > 1)
                    {
                        returingText = values[values.Length - 2];
                        if (returingText.Length < 4 && values.Length > 3)
                            return values[values.Length - 3] + returingText;
                        else
                            return returingText;
                    }
                    return values.Last();
                }
            }
            return null;
        }

        private async void Take_Receipt_Clicked(object sender, System.EventArgs e)
        {

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { Name = "receipt.png" });
            if (photo != null)
            {
#if A
                byte[] data;
                using (var memoryStream = new MemoryStream())
                {
                    photo.GetStream().CopyTo(memoryStream);
                    data = memoryStream.ToArray();
                }
                var processingImgBuffer = _textRecognizer.OpenCv(data);
                skCameraImage = SKImage.FromEncodedData(processingImgBuffer);
                cameraImage.Source = ImageSource.FromStream(() => new MemoryStream(processingImgBuffer));
#endif        
#if B
                cameraImage.Source = ImageSource.FromStream(() =>
                {
                    return photo.GetStream();
                });
                byte[] data = ReadFully(photo.GetStream());
                skCameraImage = SKImage.FromEncodedData(data);
                isImageDetected = false;
                loadingBar.IsRunning = true;
                camCanvas.InvalidateSurface();
                ReadyToSave = () =>
                {
                    SaveImageTextBlocks(pairingList);
                    ReadyToSave = null;
                };

                Task.Run(() =>
                {
                    Read_Receipt(data);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        loadingBar.IsRunning = false;
                        isImageDetected = true;
                        camCanvas.InvalidateSurface();
                    });
                });
#endif
            }
        }

        private void Read_Receipt(byte[] data)
        {
            StreamImageSource streamImageSource = (StreamImageSource)cameraImage.Source;
            if (streamImageSource != null)
            {
                System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
                Task<Stream> task = streamImageSource.Stream(cancellationToken);
                Stream stream = task.Result;
                //var data = ReadFully(stream);
                _textRecognizer.Read(data, (texts) =>
                {
                    textBlockList = texts;
                });
            }
        }

        public void SaveImageTextBlocks(Dictionary<ImageTextBlock, ImageTextBlock> pairingList)
        {
            List<PairingItem> pairingItems = new List<PairingItem>();
            pairingList.ForEach(kvp =>
            {
                pairingItems.Add(new PairingItem()
                {
                    Key = kvp.Key.Text,
                    Value = kvp.Value.Text
                });
            });
            ReceiptContent content = new ReceiptContent(pairingItems);

            //Get Infos...
            ReceiptHeader header = new ReceiptHeader();
            if (textBlockList != null)
            {
                var vd = FindValue(textBlockList, new string[] { "v.d.", "sicil" });
                if (string.IsNullOrEmpty(vd))
                {
                    var headerBlocks = FindText(textBlockList, new string[] { "a.s", "a.ş", "tic.", "san." });
                    if (headerBlocks != null && headerBlocks.Any())
                    {
                        var matching = Regex.Match(headerBlocks.First().Value.Text, @"\d{10}");
                        if (matching.Success)
                        {
                            vd = matching.Value;
                        }
                    }
                }
                var titleRow = GetRowString(textBlockList, new string[] { "a.s", "a.ş", "tic.", "t1c.", "san." });
                var vdName = "";
                if (string.IsNullOrEmpty(vd) == false)
                    vdName = FindValue(textBlockList, vd, before: true);
                var s_date = FindValue(textBlockList, new string[] { "tarlh", "tarih", "tarıh", "tar1h" });
                var s_time = FindValue(textBlockList, "saat");
                var date = DateTime.Now;
                var time = TimeSpan.Zero;
                if (string.IsNullOrEmpty(s_date) == false && Regex.Match(s_date, @"/^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$/i").Success)
                    date = DateTime.ParseExact(s_date, @"dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture);
                if (string.IsNullOrEmpty(s_time) == false)
                    time = TimeSpan.Parse(s_time);
                var receiptNo = FindValue(textBlockList, new string[] { "fis no", "f1$ no", "fi$ no", "f1ş no", "f1s no" });
                header = new ReceiptHeader()
                {
                    Title = titleRow,
                    No = receiptNo,
                    Date = date,
                    Time = time,
                    VD = vd,
                    VDName = vdName
                };
            }

            Receipt receipt = new Receipt(header, content);
            Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(receipt, nav: Navigation)));
            //MessagingCenter.Send(this, "AddItem", receipt);
        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
