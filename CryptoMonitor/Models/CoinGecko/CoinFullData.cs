using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMonitor.Models.CoinGecko
{
    public class CoinImage
    {
        public string Thumb { get; set; }
        public string Small { get; set; }
        public string Large { get; set; }
    }
    public class Sparkline
    {
        public double[] Price { get; set; }
    }
    public class CoinFullData
    {
        public string Id { get; set; }

        private string symbol; 
        public string Symbol 
        {
            get => symbol.ToUpper(); 
            set => symbol = value;
        }

        public string Name { get; set; }
      
        public CoinLinks Links { get; set; }
        public CoinImage Image { get; set; }

        [JsonProperty("market_data")]
        public CoinMarketData MarketData { get; set; }
        public CoinTicker[] Tickers { get; set; }
    }
}
