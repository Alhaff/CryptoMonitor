using CryptoMonitor.Models.CoinCap;
using CryptoMonitor.Models.CoinGecko;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMonitor.Services
{
    public class CoinCapApiService
    {
        HttpClient HttpClient { get; init; }
        public CoinCapApiService(IHttpClientFactory httpClientFactory)
        {
            HttpClient = httpClientFactory.CreateClient();
        }

        public int CoinsPerPage { get; } = 50;

        public int CoinsOffset { get; set; } = 0;

        public async  Task<List<CoinCapAsset>> GetCoinsAssets(int page = 1)
        {
            if(page >=1)
            {
                CoinsOffset = (page-1) * CoinsPerPage;
            }
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.coincap.io/v2/assets?limit={CoinsPerPage}&offset={CoinsOffset}");
                var response = await HttpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var res = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(res);
                var coins = obj["data"].ToObject<List<CoinCapAsset>>();
                return coins;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
