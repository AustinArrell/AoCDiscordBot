
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Discord;
using Discord.WebSocket;

namespace AoCDiscord
{
    public class Program
    {
        static async Task Main()
        {
            
            DiscordBot bot = new DiscordBot();
            await bot.Run();

        }

 
    }
}
