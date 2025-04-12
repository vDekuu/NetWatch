using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using NetWatchCS.Commands;
using NetWatchCS.Config;

namespace NetWatchCS
{
    public class Program
    {
        public static DiscordClient Client { get; set; }
        public static CommandsNextExtension Commands { get; set; }
        public static SlashCommandsExtension SlashCommands { get; set; }
        public static List<string> domainList = LoadCsvFiles();

        public static async Task Main(string[] args)
        {
            var botConfig = new BotConfig();
            await botConfig.ReadJSON();

            var config = new DiscordConfiguration()
            {
                Intents = DiscordIntents.GuildMessages | DiscordIntents.MessageContents | DiscordIntents.Guilds,
                Token = botConfig.DiscordBotToken,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };

            Client = new DiscordClient(config);

            Client.Ready += Client_Ready;
            Client.MessageCreated += Client_MessageCreated;
            Client.GuildCreated += Client_GuildCreated;
            Client.GuildDeleted += Client_GuildDeleted;

            var slash = Client.UseSlashCommands();

            slash.RegisterCommands<HelpCommand>();
            slash.RegisterCommands<CheckCommand>();
            slash.RegisterCommands<ToSCommand>();

            await Client.ConnectAsync();
            Console.WriteLine("NetWatch is online");
            await Task.Delay(-1);
        }

        private static async Task Client_GuildDeleted(DiscordClient sender, GuildDeleteEventArgs args)
        {
            await UpdateStatus();
        }

        private static async Task Client_GuildCreated(DiscordClient sender, GuildCreateEventArgs args)
        {
            await UpdateStatus();
        }

        private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }

        private static async Task Client_MessageCreated(DiscordClient client, MessageCreateEventArgs message)
        {
            if (!message.Author.IsBot)
            {
                string messageContent = message.Message.Content;
                

                foreach (var domain in domainList)
                {
                    if (messageContent.Contains(domain))
                    {
                        message.Message.DeleteAsync();
                        var chatEmbed = new DiscordEmbedBuilder
                        {
                            Description = "The domain in your message was identified as malicious. If you think that this is a mistake, please report it in our [discord](https://discord.gg/YYMXMvyUX8).",
                            Color = new DiscordColor("#1abc9c"),
                        }
                        .WithFooter("https://discord.gg/YYMXMvyUX8", "https://cdn.discordapp.com/app-icons/1178385597786763274/eabc31fc418856348bfe666af6ee1458.png?size=512");
                        await message.Channel.SendMessageAsync(chatEmbed);

                        var guild = message.Guild;

                        var logChannel = guild.Channels.Values.FirstOrDefault(c => c.Name.Equals("netwatch-logs"));
                        if (logChannel == null)
                        {
                            logChannel = await CreateLogChannel(guild);
                        }

                        var logEmbed = new DiscordEmbedBuilder
                        {
                            Title = "Message Deleted",
                            Color = DiscordColor.Red,
                        }
                        .AddField("Author: ", $"```{message.Author}```")
                        .AddField("Content: ", $"```\n{messageContent}\n```")
                        .WithFooter("Report false positives on our discord! /help", "https://cdn.discordapp.com/app-icons/1178385597786763274/eabc31fc418856348bfe666af6ee1458.png?size=512");
                        await logChannel.SendMessageAsync(logEmbed);
                        break;
                    }
                }
            }
            
            return;
        }

        private static async Task<DiscordChannel> CreateLogChannel(DiscordGuild guild)
        {
            var logChannel = guild.Channels.Values.FirstOrDefault(c => c.Name.Equals("netwatch-logs"));

            if (logChannel == null)
            {
                var everyone = guild.EveryoneRole;

                var permissionOverwrite = new[]
                {
                    new DiscordOverwriteBuilder(everyone)
                        .Deny(Permissions.AccessChannels),

                    new DiscordOverwriteBuilder(guild.CurrentMember)
                        .Allow(Permissions.AccessChannels)
                };

                logChannel = await guild.CreateTextChannelAsync("netwatch-logs", overwrites: permissionOverwrite);
            }
            return logChannel;
        }

        public static List<string> LoadCsvFiles()
        {
            var csvFiles = new List<string>();
            string filePath = "Domains/activeDomains.csv";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split('\n');

                    foreach (var value in values)
                    {
                        csvFiles.Add(value);
                    }
                }
            }
            Console.WriteLine("Loaded CSV file");
            return csvFiles;
        }

        private static async Task UpdateStatus()
        {
            var numberOfGuilds = Client.Guilds.Count;
            var status = $"over {numberOfGuilds} Guilds";
            var activity = new DiscordActivity(status, ActivityType.Watching);
            var update = Client.UpdateStatusAsync(activity);
        }
    }
}
