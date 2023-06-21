using CommunityToolkit.Mvvm.Input;
using CryptoMonitor.Exceptions;
using CryptoMonitor.Models.CoinCap;
using CryptoMonitor.Models.CoinGecko;
using CryptoMonitor.Services;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
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

        public ScrollService ScrollService { get; }
        public ErrorService ErrorService { get; }
        public CoinGeckoApiService CoinGeckoApi { get; }

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

        private SolidColorBrush themeColor = new SolidColorBrush(Color.FromRgb(32, 32, 32));
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

        private List<string> currencies;
        public List<string> Currencies
        {
            get
            {
                return currencies;
            }
            set
            {
                currencies = value;
                OnPropertyChanged(nameof(Currencies));
            }
        }

        private List<CoinShortData> coins;
        public List<CoinShortData> Coins
        {
            get => coins;
            set
            {
                coins = value;
                OnPropertyChanged(nameof(Coins));
                OnPropertyChanged(nameof(NameSymbolCoins));
            }
        }
        private CoinShortData selectedCoinBySearch;
        public CoinShortData SelectedCoinBySearch
        {
            get
            {
                return selectedCoinBySearch;
            }
            set
            {
                selectedCoinBySearch = value;

                OnPropertyChanged(nameof(SelectedCoinBySearch));
            }
        }

        private int  MaxElementsWhenSearchCoin { get; set; } = 10;
        private ObservableCollection<CoinShortData> nameSymbolCoins = new ObservableCollection<CoinShortData>();
        public IEnumerable<CoinShortData> NameSymbolCoins 
        { 
            get => nameSymbolCoins.TakeLast(10).Reverse();
        }
        public MainWindowViewModel(Services.INavigationService navigationService,  IThemeService themeService,
            ScrollService scrollService, ErrorService errorService, CoinGeckoApiService coinGeckoApiService)
        {
            Navigation = navigationService;
            ThemeService = themeService;
            Navigation.NavigateTo<HomePageViewModel>();
            ScrollService = scrollService;
            ErrorService = errorService;
            CoinGeckoApi = coinGeckoApiService;
        }

        private void LightTheme()
        {
            ThemeService.SetTheme(ThemeType.Light);
            ThemeColor = new SolidColorBrush(Color.FromRgb(250, 250, 250));
            IconPath = "pack://application:,,,/Resources/appIconBlack.ico";
            
        }

        private void DarkTheme()
        {
            ThemeService.SetTheme(ThemeType.Dark);
            ThemeColor = new SolidColorBrush(Color.FromRgb(32,32,32));
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

        private RelayCommand navigateToHomePage;
        public RelayCommand NavigateToHomePage
        {
            get => navigateToHomePage ?? (navigateToHomePage = new RelayCommand(
                () => Navigation.NavigateTo<HomePageViewModel>()));
        }

        private void OnLoadScroll(object? e)
        {
            var data = e as RoutedEventArgs;
            var scv = data.Source;
            ScrollService.ScrollViewer = (ScrollViewer)scv;
        }
        private RelayCommand<object> loadScroll;

        public RelayCommand<object> LoadScroll
        {
            get => loadScroll ?? (loadScroll = new RelayCommand<object>(OnLoadScroll));

        }

        private async Task Loaded()
        {
            try
            {
                Currencies = await CoinGeckoApi.GetSupportedCurrencies();
                Coins = await CoinGeckoApi.GetSupportedCoins();
                var res = Coins;
                foreach(var coin in res)
                {
                    nameSymbolCoins.Add(coin);   
                }
               
            }
            catch (Exception ex)
            {
                ErrorService.SetErrorMessage(ex.Message);
                Navigation.NavigateTo<ErrorPageViewModel>();
            }
        }

        AsyncRelayCommand onLoaded;
        public AsyncRelayCommand OnLoaded
        {
            get => onLoaded ?? (onLoaded = new AsyncRelayCommand(Loaded));
        }

        private bool searchFieldDropDown;
        public bool SearchFieldDropDown
        {
            get
            {
                return searchFieldDropDown;
            }
            set
            {
                searchFieldDropDown = value;
                OnPropertyChanged(nameof(SearchFieldDropDown));
            }
        }
       
        private void StartSearch(string? text)
        {
            if (text == null) return;
            SearchFieldDropDown = true;
            var txt = text.Replace(" ", "").ToLower();
            Regex expr = new Regex($"^{txt}");
            Func<CoinShortData, int> match = (coin) =>
            {
                if(coin.NameSymbol.Replace(" ", "").ToLower() == txt)
                {
                    return 1000;
                }
                else
                {
                    return 
                    expr.Matches(coin.Symbol.ToLower()).Count * 4
                  + expr.Matches(coin.Name.Replace(" ", "").ToLower()).Count;
                }
             
            };
            var sorted =  nameSymbolCoins.OrderBy(coin => match(coin)).ToList();
            for(int i = 0; i < sorted.Count; i++)
            {
                nameSymbolCoins.Move(nameSymbolCoins.IndexOf(sorted[i]),i);
            }
            OnPropertyChanged(nameof(NameSymbolCoins));
        }

        RelayCommand<string> onSearch;
        public RelayCommand<string> OnSearch
        {
            get => onSearch ?? (onSearch = new RelayCommand<string>(StartSearch));
        }
        private async Task ToCoinPage()
        {
            try
            {
                if (SelectedCoinBySearch == null) return;
                await CoinGeckoApi.SetCurrentCoinIdFromId(selectedCoinBySearch.Id);
                Navigation.NavigateTo<CoinDataViewModel>();
            }
            catch(Exception ex)
            {
                ErrorService.SetErrorMessage(ex.Message);
                Navigation.NavigateTo<ErrorPageViewModel>();
            }
        }

        AsyncRelayCommand goToCoinPage;
        public AsyncRelayCommand GoToCoinPage
        {
            get => goToCoinPage ?? (goToCoinPage = new AsyncRelayCommand(ToCoinPage));
        }
    }
}
