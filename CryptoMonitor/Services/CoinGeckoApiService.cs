using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMonitor.Services
{
    public class CoinGeckoApiService
    {
        HttpClient HttpClient { get; init; }
        public CoinGeckoApiService(IHttpClientFactory httpClientFactory) 
        {
            HttpClient = httpClientFactory.CreateClient();
        }

         
    }
}
