using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetWatchCS.Commands
{
    public class ToSCommand : ApplicationCommandModule
    {
        public async Task ToSCommandTask(InteractionContext ctx)
        {
            var tosEmbed = new DiscordEmbedBuilder
            {
                Title = "Terms of Service",
                Description = "**Our Terms of Service can be found at https://netwatch-bot.com/tos**",
                Color = new DiscordColor("#1abc9c"),
            };

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(tosEmbed));
        }
    }
}
