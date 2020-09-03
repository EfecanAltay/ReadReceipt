using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using ReadReceipt.Models;
using ReadReceipt.Services;

namespace ReadReceipt.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        //public IDataStore<Receipt> DataStore => DependencyService.Get<IDataStore<Receipt>>();
        public ICachingService CachingService;
        INavigation navigation;

        public BaseViewModel(INavigation nav)
        {
            CachingService = DependencyService.Get<ICachingService>();
            navigation = nav; 
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set {
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public void BackNavigate()
        {
            navigation.PopAsync();
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
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
}
