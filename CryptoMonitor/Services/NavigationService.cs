using CommunityToolkit.Mvvm.ComponentModel;
using CryptoMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Mvvm.Interfaces;

namespace CryptoMonitor.Services
{

    public interface INavigationService
    {
        ViewModelBase CurrentView { get; }

        public ViewModelBase PrevView { get; set; }

        void NavigateTo<T>() where T : ViewModelBase;
        void NavigateBack();
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
        private ViewModelBase _prevView;
        public ViewModelBase PrevView
        {
            get
            {
                return _prevView;
            }
            set
            {
                _prevView = value;
                OnPropertyChanged(nameof(PrevView));
            }
        }
        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            PrevView = CurrentView;
            var model = _viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = model;
        }

        public void NavigateBack()
        {
            var model = _viewModelFactory.Invoke(PrevView.GetType());
            CurrentView = model;
        }
    }
}
