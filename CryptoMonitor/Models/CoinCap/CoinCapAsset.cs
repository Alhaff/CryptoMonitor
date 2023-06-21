using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMonitor.Models.CoinCap
{
    public class CoinCapAsset
    {
        public string Id { get; set; }
        public int Rank { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public double? VolumeUsd24Hr { get; set; }
        public double? MarketCapUsd { get; set; }
        public double? PriceUsd { get; set; }
        public double? ChangePercent24Hr { get; set; }
        public static double TotalMarketCapUsd { get; set; } = 0;
        public double? MarketCapPercent { get => ((MarketCapUsd / TotalMarketCapUsd )* 100);}
    }
}
