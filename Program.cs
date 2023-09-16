using System.Net;

namespace AoCDiscord
{
    internal class Program
    {

        
        static async Task Main()
        {
            string authToken = "TOKEN_HERE";
            string leaderboardOwnerID = "1080248";
            var baseAddress = new Uri(@$"https://adventofcode.com/2015/leaderboard/private/view/{leaderboardOwnerID}.json");
            var cookieContainer = new CookieContainer();

            using var handler = new HttpClientHandler()
            {
                CookieContainer = cookieContainer,
            };

            HttpClient client = new HttpClient(handler);
            
            try
            {
                cookieContainer.Add(baseAddress, new Cookie("session", authToken));
                
                string responseBody  = await client.GetStringAsync(baseAddress);

                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}
