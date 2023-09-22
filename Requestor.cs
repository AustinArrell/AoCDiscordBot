using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AoCDiscord
{
    public class Requestor
    {
        private string _authToken { get; set; }
        private Uri _baseAddress { get; set; }
        private string _leaderboardOwnerID { get; set; }
        private CookieContainer _cookieContainer = new CookieContainer();
        public HttpClientHandler _handler { get; set; } = new HttpClientHandler();
        private HttpClient _client { get; set; }
        private string pathToCache = "G:\\dev\\AoCDiscordBot\\cache.json";

        public Requestor(string authToken, Uri baseAddress, string leaderboardOwnerID) 
        {
            _authToken = authToken;
            _baseAddress = baseAddress;
            _leaderboardOwnerID = leaderboardOwnerID;

            _handler.CookieContainer = _cookieContainer;
            _client = new HttpClient(_handler);
        }

        public async Task<string> makeRequest() 
        {
            DateTime writeDate = File.GetLastWriteTime(pathToCache);

            if (writeDate > writeDate.AddHours(1))
            {
                try
                {
                    _cookieContainer.Add(_baseAddress, new Cookie("session", _authToken));

                    string responseBody = await _client.GetStringAsync(_baseAddress);

                    File.WriteAllText(pathToCache, responseBody);
                    return ("Downloaded File to Cache");
                }
                catch (HttpRequestException e)
                {
                    return("\nException Caught!" + $"\nMessage :{e.Message} ");
                }

            }
            return "Cache already up to date!";
        }

       
    }
}
