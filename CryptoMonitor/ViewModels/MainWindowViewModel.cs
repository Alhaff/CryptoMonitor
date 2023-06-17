using CryptoMonitor.Core;
using CryptoMonitor.Services;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

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

        private string iconPath = "pack://application:,,,/Resources/appIconWhite.ico";
        public string IconPath
        {
            get
            {
                return iconPath;
            }
            set
            {
                iconPath = value;
                OnPropertyChanged(nameof(IconPath));
            }
        }

        private SolidColorBrush themeColor = new SolidColorBrush(Colors.Black);
        public SolidColorBrush ThemeColor
        {
            get
            {
                return themeColor;
            }
            set
            {
                themeColor = value;
                OnPropertyChanged(nameof(ThemeColor));
            }
        }

        public MainWindowViewModel(Services.INavigationService navigationService, IThemeService themeService)
        {
            Navigation = navigationService;
            ThemeService = themeService;
            Navigation.NavigateTo<Top10CoinViewModel>();
        }

        private void LightTheme(object e)
        {
            ThemeService.SetTheme(ThemeType.Light);
            ThemeColor = new SolidColorBrush(Colors.White);
            IconPath = "pack://application:,,,/Resources/appIconBlack.ico";
            
        }

        private void DarkTheme(object e)
        {
            ThemeService.SetTheme(ThemeType.Dark);
            ThemeColor = new SolidColorBrush(Colors.Black);
            IconPath = "pack://application:,,,/Resources/appIconWhite.ico";
        }

        private RelayCommand applyLightTheme;
        public RelayCommand ApplyLightTheme
        {
            get
            {
                return applyLightTheme ?? (applyLightTheme = new RelayCommand(LightTheme));
            }
        }

        private RelayCommand applyDarkTheme;

        public RelayCommand ApplyDarkTheme
        {
            get
            {
                return applyDarkTheme ?? (applyDarkTheme = new RelayCommand(DarkTheme));
            }
        }

    }
}
