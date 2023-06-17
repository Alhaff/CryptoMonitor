using CryptoMonitor.Core;
using CryptoMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMonitor.Services
{

    public interface INavigationService
    {
        ViewModelBase CurrentView { get; }

        void NavigateTo<T>() where T : ViewModelBase;
    }
    public class NavigationService : ObservableObject, INavigationService
    {
        private ViewModelBase _currentView;

        private Func<Type, ViewModelBase> _viewModelFactory;

        public ViewModelBase CurrentView
        {
            get
            {
                return _currentView;
            }
            private set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public NavigationService(Func<Type,ViewModelBase> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }
        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            var model = _viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = model;
        }
    }
}
