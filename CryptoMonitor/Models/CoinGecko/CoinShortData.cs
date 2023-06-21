using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMonitor.Models.CoinGecko
{
    public class CoinShortData
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public string NameSymbol { get=> this.ToString(); }
        public override string ToString()
        {
            return $"{Name} {Symbol.ToUpper()}"; ;
        }
    }
}
