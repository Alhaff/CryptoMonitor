using CommunityToolkit.Mvvm.ComponentModel;
using CryptoMonitor.Exceptions;
using CryptoMonitor.Models.CoinGecko;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMonitor.Services
{
    public class CoinGeckoApiService : ObservableObject
    {
        public CoinGeckoApiService(IHttpClientFactory httpClientFactory)
        {
            HttpClient = httpClientFactory.CreateClient();
        }
        HttpClient HttpClient { get; init; }

        private List<CoinShortData> SupportedCoins { get; set; }

        private List<string> SupportedCurrencies { get; set; }

        private string currentCurrency = "usd";
        public string CurrentCurrency 
        { 
            get => currentCurrency; 
            set
            {
                if (SupportedCurrencies == null) return;
                if(SupportedCurrencies.Contains(value.ToLower())) currentCurrency = value;
                OnPropertyChanged(nameof(CurrentCurrency));
            }
        }
        public string CurrentCoinId { get; private set; }

        public async Task SetCurrentCoinIdFromSymb(string symb)
        {
            try
            {
                var coins = await GetSupportedCoins();
                var coinId = coins.Where(c => c.Symbol.ToLower() == symb.ToLower()).FirstOrDefault();
                if (coinId == null) throw new UnsupportedCoinException($"Cannot find coin with symbol: {symb}");
                CurrentCoinId = coinId.Id;
                OnPropertyChanged(nameof(CurrentCoinId));
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SetCurrentCoinIdFromId(string id)
        {
            try
            {
                var coins = await GetSupportedCoins();
                var coinId = coins.Where(c => c.Id.ToLower() == id.ToLower()).FirstOrDefault();
                if (coinId == null) throw new UnsupportedCoinException($"Cannot find coin with id: {id}");
                CurrentCoinId = coinId.Id;
                OnPropertyChanged(nameof(CurrentCoinId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<string>> GetSupportedCurrencies()
        {
            if (SupportedCurrencies != null) return SupportedCurrencies;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.coingecko.com/api/v3/simple/supported_vs_currencies");
                var response = await HttpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var res = await response.Content.ReadFromJsonAsync<List<string>>();
                SupportedCurrencies = res;
                return res;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<List<CoinShortData>> GetSupportedCoins()
        {
            if (SupportedCoins != null) return SupportedCoins;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.coingecko.com/api/v3/coins/list?include_platform=false");
                var response = await HttpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var res = await response.Content.ReadFromJsonAsync<List<CoinShortData>>();
                SupportedCoins = res;
                return res;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CoinsGlobalInfo> GetTop10CoinsASync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.coingecko.com/api/v3/global");
                var response = await HttpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var res = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(res);
                var info = obj["data"].ToObject<CoinsGlobalInfo>();
                info.TotalMarketCap = obj["data"]["total_market_cap"][$"{CurrentCurrency}"].ToObject<double>();
                info.TotalMarketCapUsd = obj["data"]["total_market_cap"][$"usd"].ToObject<double>();
                info.TotalVolume = obj["data"]["total_volume"][$"{CurrentCurrency}"].ToObject<double>();
                return info;
            }
            catch (HttpRequestException ex)
            {
                if(ex.Message.Contains("403"))
                {
                    throw new CoinGeckoForbiddenException(ex.Message);
                }
                if(ex.Message.Contains("429"))
                {
                    throw new CoinGeckoTooManyRequestException(ex.Message);
                }
                throw new Exception(ex.Message);
            }
        }

        public async Task<CoinFullData> GetCoinFullData(string coinId)
        {
            try
            {
                var supported = await GetSupportedCoins();
                if (supported.Where(coin => coinId == coin.Id).Count() <= 0) 
                    throw new UnsupportedCoinException($"Coin with id {coinId} is unsuported!");
                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"https://api.coingecko.com/api/v3/coins/{coinId}?localization=false&community_data=false&developer_data=false");
                var response = await HttpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var res = await response.Content.ReadAsStringAsync();
                var coinData = JsonConvert.DeserializeObject<CoinFullData>(res);
                return coinData;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        public async Task<CoinTicker[]> GetCoinTickers(string coinId, int page = 1)
        {
            try
            {
                page = page >= 1 ? page - 1 : 0;
                var supported = await GetSupportedCoins();
                if (supported.Where(coin => coinId == coin.Id).Count() <= 0)
                    throw new UnsupportedCoinException($"Coin with id {coinId} is unsuported!");
                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"https://api.coingecko.com/api/v3/coins/{coinId}/tickers?include_exchange_logo=true&page={page}");
                var response = await HttpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var res = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(res);
                var tickers = obj["tickers"].ToObject<CoinTicker[]>();
                return tickers;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }
    }
}
