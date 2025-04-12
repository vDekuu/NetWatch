using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetWatchCS.Commands
{
    public class PrivacyCommand :ApplicationCommandModule
    {
        public async Task PrivacyCommandTask(InteractionContext ctx)
        {
            var privacyEmbed = new DiscordEmbedBuilder
            {
                Title = "Terms of Service",
                Description = "**Our privacy policy can be found at https://netwatch-bot.com/privacy**",
                Color = new DiscordColor("#1abc9c"),
            };

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(privacyEmbed));
        }
    }
}
