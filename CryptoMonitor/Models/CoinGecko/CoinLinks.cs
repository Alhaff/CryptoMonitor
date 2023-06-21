using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CryptoMonitor.Models.CoinGecko
{

    public class ReposUrl
    {
        public string[] Github { get; set; }
        public string[] Bitbucket { get; set;}

        public string[] Combined { get => Github.Concat(Bitbucket).ToArray(); }
    }
    public class CoinLinks
    {
        [JsonProperty("homepage")]
        public string[] Homepages { get; set; }

        [JsonProperty("blockchain_site")]
        public string[] BlockchainSites { get; set; }

        [JsonProperty("official_forum_url")]
        public string[] OfficialForums { get; set; }

        [JsonProperty("chat_url")]
        public string[] Chats { get;}

        [JsonProperty("announcement_url")]
        public string[] Announcements { get; set; }

        [JsonProperty("twitter_screen_name")]
        public string TwitterScreenName { get; set; }

        [JsonProperty("facebook_username")]
        public string FacebookUsername { get; set; }

        [JsonProperty("telegram_channel_identifier")]
        public string TelegramChanelIdentifier { get; set; }

        [JsonProperty("subreddit_url")]
        public string SubredditUrl { get; set; }

        [JsonProperty("repos_url")]
        public ReposUrl RepoUrls { get; set; }

        private string[] GetCommunity()
        {
            var res = OfficialForums.ToList();
            if (!string.IsNullOrEmpty(TwitterScreenName))
            {
                res.Add($"https://twitter.com/{TwitterScreenName}");
            }
            if (!string.IsNullOrEmpty(FacebookUsername))
            {
                res.Add($"https://www.facebook.com/{FacebookUsername}");
            }
            if (!string.IsNullOrEmpty(TelegramChanelIdentifier))
            {
                res.Add($"https://t.me/s/{TelegramChanelIdentifier}");
            }
            res.Add(SubredditUrl);
            return res.ToArray();
        }
        string[] community;
        public string[] Communitity 
        {
            get => community ?? (community = GetCommunity());
        } 
    }

   
}
