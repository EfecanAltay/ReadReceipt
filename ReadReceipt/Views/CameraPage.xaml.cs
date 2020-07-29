using Plugin.Media;
using ReadReceipt.Dependencies;
using ReadReceipt.Models;
using ReadReceipt.Utility;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public CameraPage()
        {
            InitializeComponent();
            cameraImage = new Image();
            camCanvas.PaintSurface += OnCanvasViewPaintSurface;
            _textRecognizer = DependencyService.Get<ITextRecognizer>();
            _textRecognizer.Init();
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

                CalculateTextRects(scaleRatioW, scaleRatioH ,canvas,paint);

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

        private void CalculateTextRects(float scaleRatioW, float scaleRatioH , SKCanvas canvas , SKPaint wordPaint)
        {
            ReceiptPaperBorder = new Rectangle();
            ReceiptPaperBorder.Width = -1;
            ReceiptTitleBorder.Width = -1;
            WordList.Clear();
            FilteredWordList.Clear();
            pairingList.Clear();

            if (textBlockList != null && textBlockList.Any())
            {
                #region Calculate_ALL_Wrod_Border
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
                            else if (stext.Contains("toplam") && ReceiptPaperBorder.Bottom < borderRect.Top)
                            {
                                ReceiptPaperBorder.Bottom = borderRect.Bottom + 20;
                            }
                        }
                    }
                });

                //Control for Receipt Borders

                #endregion

                #region Filtered
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

        private async void Take_Receipt_Clicked(object sender, System.EventArgs e)
        {

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });
            if (photo != null)
            {
                cameraImage.Source = ImageSource.FromStream(() =>
                {
                    return photo.GetStream();
                });
                skCameraImage = SKImage.FromEncodedData(ReadFully(photo.GetStream()));
                ReadyToSave = () =>
                {
                    SaveImageTextBlocks(pairingList);
                    ReadyToSave = null;
                };
                Read_Receipt();
                camCanvas.InvalidateSurface();
            }
        }

        private void Read_Receipt()
        {
            StreamImageSource streamImageSource = (StreamImageSource)cameraImage.Source;
            if (streamImageSource != null)
            {
                System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
                Task<Stream> task = streamImageSource.Stream(cancellationToken);
                Stream stream = task.Result;
                var data = ReadFully(stream);
                _textRecognizer.Read(data, (texts) =>
                {
                    textBlockList = texts;
                });
                //var ods = DependencyService.Get<IObjectDetection>();
                //ods.DetectObject(data);
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
            Receipt receipt = new Receipt(content);
            MessagingCenter.Send(this, "AddItem", receipt);
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