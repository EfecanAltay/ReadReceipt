using ReadReceipt.Models;
using ReadReceipt.Services;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

[assembly: Dependency(typeof(ShareService))]
namespace ReadReceipt.Services
{
    public class ShareService : IShareService
    {
        public async Task ShareAsExcell(IEnumerable<Receipt> receipts, IEnumerable<string> recipients = null)
        {

            try
            {
                var message = new EmailMessage
                {
                    Subject = "FişMatik - Okunan Fişler",
                    Body = "Okunan Fişler Ektedir.",
                    To = recipients?.ToList(),
                };

                var fn = "Receipts.csv";
                var file = Path.Combine(FileSystem.CacheDirectory, fn);
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(Receipt.CSVHeaderFormat());
                receipts.ForEach(receipt =>
                {
                    builder.AppendLine(receipt.ToCSVFormat());
                }); ;

                File.WriteAllText(file, builder.ToString());

                message.Attachments.Add(new EmailAttachment(file));
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }

        public async Task ShareAsExcell(IEnumerable<ReceiptGroup> receiptGroups, IEnumerable<string> recipients = null)
        {

            try
            {
                var message = new EmailMessage
                {
                    Subject = "FişMatik - Okunan Fişler",
                    Body = "Okunan Fişler Ektedir.",
                    To = recipients?.ToList(),
                };

                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;
                    //Create a workbook with a worksheet
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);

                    //Access first worksheet from the workbook instance.
                    IWorksheet worksheet = workbook.Worksheets[0];

                    IStyle gheaderStyle = workbook.Styles.Add("GHeaderStyle");
                    gheaderStyle.BeginUpdate();
                    gheaderStyle.Color = Syncfusion.Drawing.Color.FromArgb(255, 174, 33);
                    gheaderStyle.Font.Bold = true;
                    gheaderStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                    gheaderStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    gheaderStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                    gheaderStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                    gheaderStyle.EndUpdate();

                    IStyle headerStyle = workbook.Styles.Add("HeaderStyle");
                    headerStyle.BeginUpdate();
                    headerStyle.Color = Syncfusion.Drawing.Color.FromArgb(102, 183, 254);
                    headerStyle.Font.Bold = true;
                    headerStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                    headerStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    headerStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                    headerStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                    headerStyle.EndUpdate();

                    var rGroups = receiptGroups.ToArray();
                    int row = 1;
                    for (int i = 0; i < rGroups.Length; i++)
                    {
                        var rGroup = rGroups[i];
                        worksheet.Range[row, 1, row, 9].Merge();
                        worksheet.Range[row, 1].CellStyle = gheaderStyle;
                        worksheet.Range[row, 1].Text = rGroup.GroupName;
                        row++;
                        worksheet.Range[row, 1, row, 9].CellStyle = headerStyle;
                        worksheet.Range[row, 1].Text = "Tarih";
                        worksheet.Range[row, 2].Text = "Saat";
                        worksheet.Range[row, 3].Text = "Fiş No";
                        worksheet.Range[row, 4].Text = "Şirket Adı";
                        worksheet.Range[row, 5].Text = "VD Adı";
                        worksheet.Range[row, 6].Text = "VD No";
                        worksheet.Range[row, 7].Text = "Matrah";
                        worksheet.Range[row, 8].Text = "KDV";
                        worksheet.Range[row, 9].Text = "Toplam";
                        row++;
                        if (rGroup.Receipts != null && rGroup.Receipts.Any())
                        {
                            rGroup.Receipts.ForEach(receipt =>
                            {
                                worksheet.Range[row, 1].Text = receipt.Header.Date.ToString("dd/MM/yyyy");
                                worksheet.Range[row, 2].Text = receipt.Header.Time.ToString("t", DateTimeFormatInfo.InvariantInfo);
                                worksheet.Range[row, 3].Text = receipt.Header.No;
                                worksheet.Range[row, 4].Text = receipt.Header.Title;
                                worksheet.Range[row, 5].Text = receipt.Header.VDName;
                                worksheet.Range[row, 6].Text = receipt.Header.VD;
                                worksheet.Range[row, 7].Text = receipt.Header.Matrah.ToString();
                                worksheet.Range[row, 8].Text = receipt.Header.KDV.ToString();
                                worksheet.Range[row, 9].Text = receipt.Header.Total.ToString();
                                row++;
                            });
                        }
                        else
                        {
                            worksheet.Range[row, 1, row, 9].Merge();
                            worksheet.Range[row, 1].Text = "Fiş Yok";
                            row++;
                        }
                        row++;
                    }

                    //Save the workbook to stream in xlsx format. 
                    MemoryStream stream = new MemoryStream();
                    workbook.SaveAs(stream);
                    workbook.Close();

                    var fn = "ReceiptsGroups.xlsx";
                    var file = Path.Combine(FileSystem.CacheDirectory, fn);

                    using (var fileStream = File.Create(file))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(fileStream);
                    }
                    message.Attachments.Add(new EmailAttachment(file));
                    await Email.ComposeAsync(message);
                }
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }
    }
}
