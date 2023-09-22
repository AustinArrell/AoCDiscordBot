
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AoCDiscord
{
    public class Program
    {
        
        
        static async Task Main()
        {
            string authToken = "";
            string leaderboardOwnerID = "1080248";
            var baseAddress = new Uri(@$"https://adventofcode.com/2015/leaderboard/private/view/{leaderboardOwnerID}.json");

            Requestor requestor = new Requestor(authToken,baseAddress,leaderboardOwnerID);
            
            var result = await requestor.makeRequest();

            var people = new List<Person>();

            var obj = JObject.Parse(result);
            foreach (var person in obj["members"])
            {
                people.Add(new Person {
                    Id = (int)person.ElementAt(0)["id"],
                    Name = (string)person.ElementAt(0)["name"],
                    LocalScore = (int)person.ElementAt(0)["local_score"],
                    Stars = (int)person.ElementAt(0)["stars"],
                    LastStarTime = (int)person.ElementAt(0)["last_star_ts"]
                });
            }

            
        }

 
    }
}
