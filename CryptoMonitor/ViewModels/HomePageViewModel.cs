using CommunityToolkit.Mvvm.Input;
using CryptoMonitor.Exceptions;
using CryptoMonitor.Models.CoinCap;
using CryptoMonitor.Models.CoinGecko;
using CryptoMonitor.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Wpf.Ui.Controls;

namespace CryptoMonitor.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public CoinGeckoApiService CoinGeckoApi { get; init; }
        public CoinCapApiService CoinCapApi { get; }
        public INavigationService NavigationService { get; }
        public ErrorService ErrorService { get; }
        public ScrollService ScrollService { get; private set; }

        private CoinsGlobalInfo _globalInfo;
        public CoinsGlobalInfo GlobalInfo
        {
            get
            {
                return _globalInfo;
            }
            set
            {
                _globalInfo = value;
                OnPropertyChanged(nameof(GlobalInfo));

            }
        }

        private List<MarketCapPercent> coinTop;
        public List<MarketCapPercent> CoinTopList
        {
            get
            {
                return coinTop;
            }
            set
            {
                coinTop = value;
                OnPropertyChanged();
            }
        }

        private List<CoinCapAsset> coinAssets;
        public List<CoinCapAsset> CoinAssets
        {
            get
            {
                return coinAssets;
            }
            set
            {
                coinAssets = value;
                OnPropertyChanged(nameof(CoinAssets));
            }
        }

        Stopwatch Watcher = new Stopwatch();
        TimeSpan LastWatchCheck { get; set; }
        bool Is10MinHavePassed { get => (Watcher.Elapsed - LastWatchCheck).TotalMinutes > 10; }
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
        public HomePageViewModel(CoinGeckoApiService coinGeckoApi, CoinCapApiService coinCapApiService,
                                    INavigationService navigationService, ErrorService errorService,
                                    ScrollService scrollService)
        {
            CoinGeckoApi = coinGeckoApi;
            CoinCapApi = coinCapApiService;
            NavigationService = navigationService;
            ErrorService = errorService;
            ScrollService = scrollService;
            Watcher.Start();
            
        }

        private async Task Loaded()
        {
            try
            {
                
                if (GlobalInfo == null || Is10MinHavePassed)
                {
                    GlobalInfo = await CoinGeckoApi.GetTop10CoinsASync();
                    CoinCapAsset.TotalMarketCapUsd = GlobalInfo.TotalMarketCapUsd;
                    LastWatchCheck = Watcher.Elapsed;
                }
                CoinTopList = new List<MarketCapPercent>();
                CoinAssets = await CoinCapApi.GetCoinsAssets(Page);
            }
            catch(CoinGeckoTooManyRequestException ex)
            {
                if(GlobalInfo== null)
                {
                    ErrorService.SetErrorMessage(ex.Message);
                    NavigationService.NavigateTo<ErrorPageViewModel>();
                }
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

        private async Task NextPage()
        {
           if(Page < int.MaxValue)
            {
                try
                {
                    Page += 1;
                    CoinAssets = await CoinCapApi.GetCoinsAssets(Page);
                    ScrollService.ScrollViewer.ScrollToTop();
                }
                catch(Exception ex) 
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
            if (Page >1)
            {
                try
                {
                    Page -= 1;
                    CoinAssets = await CoinCapApi.GetCoinsAssets(Page);
                    ScrollService.ScrollViewer.ScrollToTop();
                }
                catch(Exception ex)
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

        private async Task ToCoinPage(string symb)
        {
            try
            {
                await CoinGeckoApi.SetCurrentCoinIdFromSymb(symb);
                NavigationService.NavigateTo<CoinDataViewModel>();
            }
            catch(Exception ex) 
            {
                ErrorService.SetErrorMessage(ex.Message);
                NavigationService.NavigateTo<ErrorPageViewModel>();
            }
        }

        private AsyncRelayCommand<string> navigateToCoinPage;

        public AsyncRelayCommand<string> NavigateToCoinPage
        {
            get => navigateToCoinPage ?? (navigateToCoinPage = new AsyncRelayCommand<string>(ToCoinPage));
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
            get=> scroll ?? (scroll = new RelayCommand<object>(OnScroll));
          
        }

        private void EnterHyperLink(MouseEventArgs? e)
        {
            var source = e.Source;
            if (source == null) return;
            var hpl = source as System.Windows.Documents.Hyperlink;
            hpl.TextDecorations = TextDecorations.Underline;
        }
        private void LeaveHyperLink(MouseEventArgs? e)
        {
            var source = e.Source;
            if (source == null) return;
            var hpl = source as System.Windows.Documents.Hyperlink;
            hpl.TextDecorations =null;
        }
        private RelayCommand<MouseEventArgs> mouseEnterHyperLink;

        public RelayCommand<MouseEventArgs> MouseEnterHyperLink
        {
            get => mouseEnterHyperLink ?? (mouseEnterHyperLink = new RelayCommand<MouseEventArgs>(EnterHyperLink));

        }
        private RelayCommand<MouseEventArgs> mouseLeaveHyperLink;

        public RelayCommand<MouseEventArgs> MouseLeaveHyperLink
        {
            get => mouseLeaveHyperLink ?? (mouseLeaveHyperLink = new RelayCommand<MouseEventArgs>(LeaveHyperLink));

        }

        private void NavigateToHyperLink(RequestNavigateEventArgs? e)
        {
            try
            {
                var sInfo = new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri)
                {
                    UseShellExecute = true,
                };
                Process.Start(sInfo);
                e.Handled = true;
            }catch (Exception ex)
            {
                ErrorService.SetErrorMessage(ex.Message);
                NavigationService.NavigateTo<ErrorPageViewModel>();
            }
        }

        private RelayCommand<RequestNavigateEventArgs> hyperlinkNavigate;

        public RelayCommand<RequestNavigateEventArgs> HyperlinkNavigate
        {
            get => hyperlinkNavigate ?? (hyperlinkNavigate = new RelayCommand<RequestNavigateEventArgs>(NavigateToHyperLink));
        }

    }
}
