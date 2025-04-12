using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace NetWatchCS.Commands
{
    public class ToSCommand : ApplicationCommandModule
    {
        [SlashCommand("tos", "Link to the terms of service.")]
        public async Task ToSCommandTask(InteractionContext ctx)
        {
            var tosEmbed = new DiscordEmbedBuilder
            {
                Title = "Terms of Service",
                Description = "**Our Terms of Service can be found at:**\nhttps://netwatch-bot.com/tos",
                Color = new DiscordColor("#1abc9c"),
            };

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(tosEmbed));
        }
    }
}
