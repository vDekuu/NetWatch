using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetWatchCS.Commands
{
    public class CheckCommand : ApplicationCommandModule
    {
        [SlashCommand("check", "Scans a link provided by a user.")]
        public async Task CheckCommandTask(InteractionContext ctx, [Option("link", "link that should be checked")] string link)
        {
            bool badLink = false;

            foreach (var domain in Program.domainList)
            {
                if (link.Contains(domain))
                {
                   badLink = true;
                }
            }

            if (badLink == true)
            {
                var badLinkEmbed = new DiscordEmbedBuilder
                {
                    Title = "Scan Result",
                    Description = $"The provided link `{link}` was identified as malicious.",
                    Color = DiscordColor.Red
                };

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(badLinkEmbed));
            }
            else
            {
                var goodLinkEmbed = new DiscordEmbedBuilder
                {
                    Title = "Scan Result",
                    Description = $"The provided link `{link}` is not in the malicious domain database.",
                    Color = DiscordColor.Green
                };

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(goodLinkEmbed));
            }
        }
    }
}
