using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMonitor.Models.CoinGecko
{
    public class CoinMarketData
    {
        [JsonProperty("current_price")]
        public Dictionary<string, double?> CurrentPrice { get; set; }
        [JsonProperty("high_24h")]
        public Dictionary<string, double?> HighPrice24h { get; set; }
        [JsonProperty("low_24h")]
        public Dictionary<string, double?> LowPrice24h { get; set; }

        [JsonProperty("market_cap")]
        public Dictionary<string, double?> MarketCap { get; set; }

        [JsonProperty("total_volume")]
        public Dictionary<string, double?> TotalVolume { get; set; }

        [JsonProperty("fully_diluted_valuation")]
        public Dictionary<string, double?> FullyDilutedValuation { get; set; }

        [JsonProperty("price_change_percentage_1h_in_currency")]
        public Dictionary<string, double?> PriceChange1h { get; set; }

        [JsonProperty("total_supply")]
        public double? TotalSupply { get; set; }

        [JsonProperty("max_supply")]
        public double? MaxSupply { get; set; }

        [JsonProperty("circulating_supply")]
        public double? CirculatingSupply { get; set; }
    }

    public class CoinMarketDataOneCurrency : ObservableObject
    {
        public double? CurrentPrice { get => _data.CurrentPrice[_currency]; }
        public double? HighPrice24h { get => _data.HighPrice24h[_currency]; }
        public double? LowPrice24h { get => _data.LowPrice24h[_currency]; }
        public double? MarketCap { get => _data.MarketCap[_currency]; }
        public double? TotalVolume { get => _data.TotalVolume[_currency]; }
        public double? FullyDilutedValuation { get => _data.FullyDilutedValuation[_currency]; }
        public double? PriceChange1h { get => _data.PriceChange1h[_currency]; }
        public double? TotalSupply { get => _data.TotalSupply; }
        public double? MaxSupply { get => _data.MaxSupply; }
        public double? CirculatingSupply { get => _data.CirculatingSupply; }

        private string _currency;
        public string Currency 
        {
            get => _currency.ToUpper();
            set
            {
                if (_data.CurrentPrice.Keys.Contains(value.ToLower()))
                {
                    _currency = value;
                    OnPropertyChanged(nameof(CurrentPrice));
                    OnPropertyChanged(nameof(HighPrice24h));
                    OnPropertyChanged(nameof(LowPrice24h));
                    OnPropertyChanged(nameof(MarketCap));
                    OnPropertyChanged(nameof(TotalVolume));
                    OnPropertyChanged(nameof(FullyDilutedValuation));
                    OnPropertyChanged(nameof(PriceChange1h));
                    OnPropertyChanged(nameof(Currency));
                }
            }
        }
        private CoinMarketData _data;
        public CoinMarketDataOneCurrency(CoinMarketData data, string currency)
        {
            _currency = currency;
            _data = data;
        }
    }
}
