using CryptoMonitor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Mvvm.Contracts;

namespace CryptoMonitor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Services.INavigationService _navigation;
        public Services.INavigationService Navigation 
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged(nameof(Navigation));
            }
        }
        private IThemeService _themeService;
        public IThemeService ThemeService 
        { 
            get => _themeService;
            set
            {
                _themeService = value;
                OnPropertyChanged(nameof(ThemeService));
            }
        }

        public MainWindowViewModel(Services.INavigationService navigationService, IThemeService themeService)
        {
            Navigation = navigationService;
            ThemeService = themeService;
            Navigation.NavigateTo<Top10CoinViewModel>();
        }
    }
}
