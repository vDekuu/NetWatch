using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace NetWatchCS.Commands
{
    public class PrivacyCommand :ApplicationCommandModule
    {
        [SlashCommand("privacy", "Link to the privacy policy.")]
        public async Task PrivacyCommandTask(InteractionContext ctx)
        {
            var privacyEmbed = new DiscordEmbedBuilder
            {
                Title = "Terms of Service",
                Description = "**Our privacy policy can be found at:**\nhttps://netwatch-bot.com/privacy",
                Color = new DiscordColor("#1abc9c"),
            };

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(privacyEmbed));
        }
    }
}
