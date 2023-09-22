using System.Text.Json;
using System.Net;

namespace AoCDiscord
{
    public class Program
    {
        
        
        static async Task Main()
        {
            string authToken = "TOKEN_HERE";
            string leaderboardOwnerID = "1080248";
            var baseAddress = new Uri(@$"https://adventofcode.com/2015/leaderboard/private/view/{leaderboardOwnerID}.json");

            Requestor requestor = new Requestor(authToken,baseAddress,leaderboardOwnerID);

            Console.WriteLine(requestor.makeRequest().Result);
            
        }

 
    }
}
