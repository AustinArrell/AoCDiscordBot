using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoCDiscord
{
    public class DiscordBot
    {
        DiscordSocketClient _client = new DiscordSocketClient();
        CommandService _commands = new CommandService();
        string discordToken = File.ReadAllText("G:\\dev\\AoCDiscordBot\\discordtoken.txt");

        
        public Requestor requestor { get; set; }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }


        public async Task Run() 
        {
            
            _client.Log += Log;

            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.

            // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
            // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
            // var token = File.ReadAllText("token.txt");
            // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

            await _client.LoginAsync(TokenType.Bot, discordToken);
            await _client.StartAsync();

            

            // Let's hook the ready event for creating our commands in.
            _client.Ready += Client_Ready;

            _client.SlashCommandExecuted += SlashCommandHandler;

            // Block this task until the program is closed.
            await Task.Delay(-1);

        }

        public async Task Client_Ready()
        {

            // Let's build a guild command! We're going to need a guild so lets just put that in a variable.
            var guild = _client.GetGuild(1150911692587417720);

            // Next, lets create our slash command builder. This is like the embed builder but for slash commands.
            var guildCommand = new SlashCommandBuilder();

            // Note: Names have to be all lowercase and match the regular expression ^[\w-]{3,32}$
            guildCommand.WithName("print-leaderboard");

            // Descriptions can have a max length of 100.
            guildCommand.WithDescription("Print out the leaderboard for all to see");

            try
            {
                // Now that we have our builder, we can call the CreateApplicationCommandAsync method to make our slash command.
                await guild.CreateApplicationCommandAsync(guildCommand.Build());

            }
            catch (ApplicationCommandException exception)
            {
                // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                Console.WriteLine(json);
            }

            
        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            if (command.Data.Name == "print-leaderboard")
            {
                string authToken = File.ReadAllText("G:\\dev\\AoCDiscordBot\\aoctoken.txt");
                string leaderboardOwnerID = "1080248";
                var baseAddress = new Uri(@$"https://adventofcode.com/2015/leaderboard/private/view/{leaderboardOwnerID}.json");
                Requestor requestor = new Requestor(authToken, baseAddress, leaderboardOwnerID);
                string result = await requestor.makeRequest();

                List<Person> people = new List<Person>();
                var obj = JObject.Parse(result);
                foreach (var person in obj["members"])
                {
                    people.Add(new Person
                    {
                        Id = (int)person.ElementAt(0)["id"],
                        Name = (string)person.ElementAt(0)["name"],
                        LocalScore = (int)person.ElementAt(0)["local_score"],
                        Stars = (int)person.ElementAt(0)["stars"],
                        LastStarTime = (int)person.ElementAt(0)["last_star_ts"]
                    });
                }

                string stars = "";

                people = people.OrderByDescending(p => p.Stars).ToList();
                foreach (var person in people) 
                {
                    stars += $"**{person.Name}** : {person.Stars}\n";
                }

                var starEmbed = new EmbedBuilder
                {
                    Title = "Stars :star:",
                    Color = Color.Green,
                    Description = stars
                }.Build();


                string scores = "";
                people = people.OrderByDescending(p => p.LocalScore).ToList();
                foreach (var person in people)
                {
                    scores += $"**{person.Name}** : {person.LocalScore}\n";
                }
                var scoreEmbed = new EmbedBuilder
                {
                    Title = "Score :100:",
                    Color = Color.Red,
                    Description = scores
                }.Build();

                
                await command.RespondAsync("",new Embed[] { scoreEmbed,starEmbed });

             
            }
            
        }
    }
}
