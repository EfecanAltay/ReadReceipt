using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReadReceipt.Models
{
    public class Receipt
    {
        public string Id { get; set; }
        public ReceiptHeader Header { get; set; }
        public ReceiptContent Content { get; set; }

        #region ctors
        public Receipt()
        {
            
        }

        public Receipt(ReceiptHeader header)
        {
            Header = header;
        }

        public Receipt(ReceiptContent content)
        {
            Content = content;
        }

        public Receipt(ReceiptHeader header, ReceiptContent content)
        {
            Header = header;
            Content = content;
        }

        public static string CSVHeaderFormat()
        {
            return "Tarih,Saat,FisNo,ŞirketAdi,VD Adi,VD No,Matrah,KDV,Toplam";
        }

        public string ToCSVFormat()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Header.Date.ToString("dd/MM/yyyy"));
            builder.Append(",");
            builder.Append(Header.Time.ToString("t", DateTimeFormatInfo.InvariantInfo));
            builder.Append(",");
            builder.Append(Header.No);
            builder.Append(",");
            builder.Append(Header.Title);
            builder.Append(",");
            builder.Append(Header.VDName);
            builder.Append(",");
            builder.Append(Header.VD);
            builder.Append(",");
            builder.Append(Header.Matrah);
            builder.Append(",");
            builder.Append(Header.KDV);
            builder.Append(",");
            builder.Append(Header.Total);
            builder.Append(",");
            return builder.ToString();
        }
        #endregion
    }

    public class ReceiptHeader : INotifyPropertyChanged
    {
        private string no;
        public string No
        {
            get { return no; }
            set { no = value;
                OnPropertyChanged(nameof(No));
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private TimeSpan time;
        public TimeSpan Time
        {
            get { return time; }
            set
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string vd; // Firmanın Vergi Dairesi veya Sicil
        public string VD 
        {
            get { return vd; }
            set
            {
                vd = value;
                OnPropertyChanged(nameof(VD));
            }
        }

        private string vdName; // Firmanın Vergi Dairesi veya Sicil
        public string VDName
        {
            get { return vdName; }
            set
            {
                vdName = value;
                OnPropertyChanged(nameof(VDName));
            }
        }

        private double matrah; // Total - KDV
        public double Matrah
        {
            get { return matrah; }
            set
            {
                matrah = value;
                OnPropertyChanged(nameof(Matrah));
            }
        }

        private double matrahResults;
        public double MatrahResult
        {
            get { return matrahResults; }
            set
            {
                matrahResults = value;
                OnPropertyChanged(nameof(MatrahResult));
            }
        }

        private double kdv; // Total - KDV
        public double KDV
        {
            get { return kdv; }
            set
            {
                kdv = value;
                OnPropertyChanged(nameof(KDV));
            }
        }
        
        private double kdvResults;
        public double KDVResults
        {
            get { return kdvResults; }
            set
            {
                kdvResults = value;
                OnPropertyChanged(nameof(KDVResults));
            }
        }

        private double total; // Total
        public double Total
        {
            get { return total; }
            set
            {
                total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        private double totalResults;
        public double TotalResults
        {
            get { return totalResults; }
            set
            {
                totalResults = value;
                OnPropertyChanged(nameof(TotalResults));
            }
        }

        private string description; //Tüm Header Kısmı içerisinde Title da bulunabilir
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public ReceiptInfo Infos { get; set; } = new ReceiptInfo();
        public override string ToString()
        {
            return Title;
        }

        private bool calculatedResults = false;
        public bool CalculatedResults
        {
            get { return calculatedResults; }
            set
            {
                calculatedResults = value;
                OnPropertyChanged(nameof(CalculatedResults));
            }
        }

        public ObservableCollection<KDVVal> kdv1Items;
        public ObservableCollection<KDVVal> KDV1Items
        {
            get { return kdv1Items; }
            set
            {
                kdv1Items = value;
                OnPropertyChanged(nameof(KDV1Items));
            }
        }

        public ObservableCollection<KDVVal> kdv8Items;
        public ObservableCollection<KDVVal> KDV8Items
        {
            get { return kdv8Items; }
            set
            {
                kdv8Items = value;
                OnPropertyChanged(nameof(KDV8Items));
            }
        }

        public ObservableCollection<KDVVal> kdv18Items;
        public ObservableCollection<KDVVal> KDV18Items
        {
            get { return kdv18Items; }
            set
            {
                kdv18Items = value;
                OnPropertyChanged(nameof(KDV18Items));
            }
        }

        public class KDVVal : BindableObject
        {
            private double val;
            public double Val
            {
                get { return val; }
                set
                {
                    val = value;
                    OnPropertyChanged(nameof(Val));
                }
            }
        }

        private double kdv1Diff;
        public double KDV1Diff
        {
            get { return kdv1Diff; }
            set
            {
                kdv1Diff = value;
                OnPropertyChanged(nameof(KDV1Diff));
            }
        }

        private double kdv8Diff;
        public double KDV8Diff
        {
            get { return kdv8Diff; }
            set
            {
                kdv8Diff = value;
                OnPropertyChanged(nameof(KDV8Diff));
            }
        }

        private double kdv18Diff;
        public double KDV18Diff
        {
            get { return kdv18Diff; }
            set
            {
                kdv18Diff = value;
                OnPropertyChanged(nameof(KDV18Diff));
            }
        }

        private double kdv1withoutVal;
        public double KDV1WithoutVal
        {
            get { return kdv1withoutVal; }
            set
            {
                kdv1withoutVal = value;
                OnPropertyChanged(nameof(KDV1WithoutVal));
            }
        }

        private double kdv8withoutVal;
        public double KDV8WithoutVal
        {
            get { return kdv8withoutVal; }
            set
            {
                kdv8withoutVal = value;
                OnPropertyChanged(nameof(KDV8WithoutVal));
            }
        }

        private double kdv18withoutVal;
        public double KDV18WithoutVal
        {
            get { return kdv18withoutVal; }
            set
            {
                kdv18withoutVal = value;
                OnPropertyChanged(nameof(KDV18WithoutVal));
            }
        }

        public ICommand CalculateResultCommand { get; set; }
        public ICommand SumResultCommand { get; set; }
        public ICommand MoveResultCommand { get; set; }
        public ICommand AddKDVCommand { get; set; }
        public ICommand RemoveKDV1Command { get; set; }
        public ICommand RemoveKDV8Command { get; set; }
        public ICommand RemoveKDV18Command { get; set; }
        public ICommand ResetResults { get; set; }

        public ReceiptHeader()
        {
            CalculateResultCommand = new Command(OnCalculateResult);
            SumResultCommand = new Command(OnSumResult);
            MoveResultCommand = new Command(OnMoveResult);
            ResetResults = new Command(OnResetResult);
            AddKDVCommand = new Command(OnAddKDV);
            RemoveKDV1Command = new Command(OnRemoveKDV1);
            RemoveKDV8Command = new Command(OnRemoveKDV8);
            RemoveKDV18Command = new Command(OnRemoveKDV18);
            KDV1Items = new ObservableCollection<KDVVal>();
            KDV8Items = new ObservableCollection<KDVVal>();
            KDV18Items = new ObservableCollection<KDVVal>();
            date = DateTime.Now;
            time = Date.TimeOfDay;
        }

        public void OnCalculateResult()
        {
            double kdvSum = 0;
            double sum = 0;
            if(kdv1Items != null && kdv1Items.Any())
            {
                kdv1Items.ForEach(x => {
                    kdvSum += (x.Val / 100);
                    sum += x.Val;
                });
                KDV1Diff = kdvSum;
                KDV1WithoutVal = sum - kdvSum;
                kdvSum = 0;
                sum = 0;
            }
            else
            {
                KDV1Diff = 0;
                KDV1WithoutVal = 0;
            }
            if (kdv8Items != null && kdv8Items.Any())
            {
                kdv8Items?.ForEach(x =>
                {
                    kdvSum += (x.Val / 100) * 8;
                    sum += x.Val;
                });
                KDV8Diff = kdvSum;
                KDV8WithoutVal = sum - kdvSum;
                kdvSum = 0;
                sum = 0;
            }
            else
            {
                KDV8Diff = 0;
                KDV8WithoutVal = 0;
            }
            if (kdv18Items != null && kdv18Items.Any())
            {
                kdv18Items.ForEach(x =>
                {
                    kdvSum += (x.Val / 100) * 18;
                    sum += x.Val;
                });
                KDV18Diff = kdvSum;
                KDV18WithoutVal = sum - kdvSum;
            }
            else
            {
                KDV18Diff = 0;
                KDV18WithoutVal = 0;
            }
        }
        
        public void OnSumResult()
        {
            KDVResults = kdv1Diff + kdv8Diff + kdv18Diff;
            MatrahResult = kdv1withoutVal + kdv8withoutVal + kdv18withoutVal;
            TotalResults = KDVResults + MatrahResult;
            CalculatedResults = true;
        }

        public void OnMoveResult()
        {
            KDV = KDVResults;
            Matrah = MatrahResult;
            Total = TotalResults;
            CalculatedResults = false;
        }
       
        public void OnAddKDV(object kdv)
        {
            switch (kdv as string)
            {
                case "1":
                    KDV1Items.Add(new KDVVal());
                    break;
                case "8":
                    KDV8Items.Add(new KDVVal());
                    break;
                case "18":
                    KDV18Items.Add(new KDVVal());
                    break;
                default:
                    break;
            }
        
        }

        public void OnRemoveKDV1(object kdvVal)
        {
            var kdv = (KDVVal)kdvVal;
            KDV1Items.Remove(kdv);
        }

        public void OnRemoveKDV8(object kdvVal)
        {
            var kdv = (KDVVal)kdvVal;
            KDV8Items.Remove(kdv);
        }

        public void OnRemoveKDV18(object kdvVal)
        {
            var kdv = (KDVVal)kdvVal;
            KDV18Items.Remove(kdv);
        }

        public void OnResetResult()
        {
            CalculatedResults = false;
            KDVResults = 0;
            MatrahResult = 0;
            TotalResults = 0;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class ReceiptInfo
    {
        public Dictionary<string, string> infos { get; set; }
    }

    public class ReceiptContent
    {
        public ObservableCollection<PairingItem> PairingItems { get; set; } = new ObservableCollection<PairingItem>();
        public ReceiptContent(IEnumerable<PairingItem> pairingItems = null)
        {
            if (pairingItems != null)
                PairingItems = new ObservableCollection<PairingItem>(pairingItems);
            else
                PairingItems = new ObservableCollection<PairingItem>();
        }

        public ICommand RemoveItem => new Command(async (item) =>
        {
            var pItem = (PairingItem)item;
            var result = await Application.Current.MainPage.DisplayAlert("Satır Silinecektir.", "Devam etmek ister misiniz ?", "Evet", "Hayır");
            if (result)
                PairingItems.Remove(pItem);
        });

        public ICommand AddItem => new Command(() =>
        {
            PairingItems.Add(new PairingItem());
        });
    }
}
