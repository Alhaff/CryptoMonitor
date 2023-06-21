using CommunityToolkit.Mvvm.Input;
using CryptoMonitor.Models.CoinCap;
using CryptoMonitor.Models.CoinGecko;
using CryptoMonitor.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CryptoMonitor.ViewModels
{
    class CoinDataViewModel : ViewModelBase
    {
        public CoinGeckoApiService CoinGeckoApi { get; }
        public INavigationService NavigationService { get; }
        public ErrorService ErrorService { get; }
        public ScrollService ScrollService { get; }

        private CoinFullData coinData;
        public CoinFullData CoinData
        {
            get
            {
                return coinData;
            }
            set
            {
                coinData = value;
                OnPropertyChanged(nameof(CoinData));
            }
        }

        private CoinMarketDataOneCurrency coinMarket;
        public CoinMarketDataOneCurrency CoinMarket
        {
            get { return coinMarket; }
            set
            {
                coinMarket = value;
                OnPropertyChanged(nameof(CoinMarket));
            }
        }
        public CoinDataViewModel(CoinGeckoApiService coinGeckoApiService, INavigationService navigationService,
                                 ErrorService errorService, ScrollService scrollService)
        {
            CoinGeckoApi = coinGeckoApiService;
            NavigationService = navigationService;
            ErrorService = errorService;
            ScrollService = scrollService;
            CoinGeckoApi.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == "CurrentCurrency")
                {
                    CoinMarket.Currency = CoinGeckoApi.CurrentCurrency;
                }
                else if(e.PropertyName == "CurrentCoinId")
                {
                    await Loaded();
                }
            };
        }

        private int page = 1;
        public int Page
        {
            get
            {
                return page;
            }
            set
            {
                if (value <= 0) return; 
                page = value;
                OnPropertyChanged(nameof(Page));
            }
        }
        private async Task Loaded()
        {
            try
            {
                CoinData = await CoinGeckoApi.GetCoinFullData(CoinGeckoApi.CurrentCoinId);
                CoinMarket = new CoinMarketDataOneCurrency(CoinData.MarketData, CoinGeckoApi.CurrentCurrency);
                CoinData.Tickers = await CoinGeckoApi.GetCoinTickers(CoinGeckoApi.CurrentCoinId, Page);
                OnPropertyChanged(nameof(CoinData));
            }
            catch (Exception ex)
            {
                ErrorService.SetErrorMessage(ex.Message);
                NavigationService.NavigateTo<ErrorPageViewModel>();
            }
        }
        AsyncRelayCommand onLoaded;
        public AsyncRelayCommand OnLoaded
        {
            get => onLoaded ?? (onLoaded = new AsyncRelayCommand(Loaded));
        }

        private void OnScroll(object? e)
        {
            MouseWheelEventArgs data = e as MouseWheelEventArgs;
            var args = new MouseWheelEventArgs(data.MouseDevice, data.Timestamp, data.Delta);
            args.RoutedEvent = ScrollViewer.MouseWheelEvent;
            ScrollService.ScrollViewer.RaiseEvent(args);
        }
        private RelayCommand<object> scroll;

        public RelayCommand<object> Scroll
        {
            get => scroll ?? (scroll = new RelayCommand<object>(OnScroll));

        }

        private async Task NextPage()
        {
            if (Page < int.MaxValue)
            {
                try
                {
                    Page += 1;
                    CoinData.Tickers = await CoinGeckoApi.GetCoinTickers(CoinGeckoApi.CurrentCoinId, Page);
                    OnPropertyChanged(nameof(CoinData));
                    ScrollService.ScrollViewer.ScrollToTop();
                }
                catch (Exception ex)
                {
                    ErrorService.SetErrorMessage(ex.Message);
                    NavigationService.NavigateTo<ErrorPageViewModel>();
                }
            }
        }
        AsyncRelayCommand nextCoinsPage;
        public AsyncRelayCommand NextCoinsPage
        {
            get => nextCoinsPage ?? (nextCoinsPage = new AsyncRelayCommand(NextPage));
        }
        private async Task PrevPage()
        {
            if (Page > 1)
            {
                try
                {
                    Page -= 1;
                    CoinData.Tickers = await CoinGeckoApi.GetCoinTickers(CoinGeckoApi.CurrentCoinId, Page);
                    OnPropertyChanged(nameof(CoinData));
                    ScrollService.ScrollViewer.ScrollToTop();
                }
                catch (Exception ex)
                {
                    ErrorService.SetErrorMessage(ex.Message);
                    NavigationService.NavigateTo<ErrorPageViewModel>();
                }
            }
        }
        AsyncRelayCommand prevCoinsPage;
        public AsyncRelayCommand PrevCoinsPage
        {
            get => prevCoinsPage ?? (prevCoinsPage = new AsyncRelayCommand(PrevPage));
        }

        private void NavigateToHyperLink(string? uri)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uri)) return;
                var sInfo = new System.Diagnostics.ProcessStartInfo(uri)
                {
                    UseShellExecute = true,
                };
                Process.Start(sInfo);
            }
            catch (Exception ex)
            {
                ErrorService.SetErrorMessage(ex.Message);
                NavigationService.NavigateTo<ErrorPageViewModel>();
            }
        }

        private RelayCommand<string> navigateToUrl;

        public RelayCommand<string> NavigateToUrl
        {
            get => navigateToUrl ?? (navigateToUrl = new RelayCommand<string>(NavigateToHyperLink));
        }

    }
}
