using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMonitor.Models.CoinGecko
{

    public class Market
    {
        public string Name { get; set; }

        public string Identifier { get; set; }

        private string logo;
        public string Logo 
        {
            get => logo;
            set
            {
                if (value == null)
                {
                    logo = " ";
                    return;
                }
                logo = value;
            }
        }
    }

    public class CoinTicker
    {
        public string Base { get; set; }
        public string Target { get; set; }

        public Market Market { get; set; }

        [JsonProperty("converted_last")]
        public Dictionary<string,double?> LastInCurrency { get; set; }
        public double? LastUsd { get => LastInCurrency?["usd"]; }

        [JsonProperty("converted_volume")]
        public Dictionary<string, double?> VolumeInCurrency { get; set; }
        public double? VolumeUsd { get => VolumeInCurrency?["usd"]; }

        [JsonProperty("bid_ask_spread_percentage")]
        public double? SpreadPercent { get; set; }
        [JsonProperty("trust_score")]
        public string TrustScore { get; set; }
        [JsonProperty("trade_url")]
        public string TradeUrl { get; set; }
    }
}
