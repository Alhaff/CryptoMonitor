using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using System.Reflection.PortableExecutable;

namespace CryptoMonitor.Models.CoinGecko
{

    public class MarketCapPercent
    {
        public string Symbol { get; set; }
        public double Percent { get; set; }
    }
    public class CoinsGlobalInfo
    {
        [JsonProperty("active_cryptocurrencies")]
        public int ActiveCryptocurrencies { get; set; }
        [JsonProperty("upcoming_icos")]
        public int UpcomingICOs { get; set; }
        [JsonProperty("ongoing_icos")]
        public int OngoingICOs { get; set; }
        [JsonProperty("ended_icos")]
        public int EndedICOs { get; set; }
       
        public int Markets { get; set; }
        public double TotalMarketCap { get; set; }
        public double TotalVolume { get; set; }
        public double TotalMarketCapUsd { get; set; }
    }
}
