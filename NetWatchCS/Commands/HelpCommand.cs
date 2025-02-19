using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace NetWatchCS.Commands
{
    public class HelpCommand : ApplicationCommandModule
    {
        [SlashCommand("help", "Invite to the support discord.")]
        public async Task HelpCommandTask(InteractionContext ctx)
        {
            var helpEmbed = new DiscordEmbedBuilder
            {
                Title = "Help",
                Description = "**Join our discord for help:**\nhttps://discord.gg/YYMXMvyUX8\n\n**ToS:**\nhttps://netwatch-bot.com/#tos\n\n**Privacy Policy:**\nhttps://netwatch-bot.com/#privacy-policy",
                Color = new DiscordColor("#1abc9c"),
            };

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(helpEmbed));
        }
    }
}
