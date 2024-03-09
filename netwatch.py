import discord
from discord.ext import commands
from activeDomains import activeDomainsList
import re
from discord.ext import tasks
import json

intents = discord.Intents.default()
intents.message_content = True
intents.members = True
intents.guilds = True
client = commands.Bot(command_prefix='!', intents=intents)

with open("C:/BotDev/config.json") as f:
    data= json.load(f)
    token = data['netWatchBetaToken']

@client.event
async def on_ready():
    await client.tree.sync()
    print('bot is ready')

    #sets the status
    await client.change_presence(activity=discord.activity.CustomActivity(name=f"Protecting {len(client.guilds)} Guilds"))

    #refreshes the number of guilds in the status
    @tasks.loop(minutes=60)
    async def update_status():
        await client.change_presence(activity=discord.activity.CustomActivity(name=f"Protecting {len(client.guilds)} Guilds"))

    update_status.start()

#creates log channel if it doesn't exist
@client.event
async def on_message(message):
    
    #checks if netwatch send the link
    if message.author == client.user:
        return
    
    #detects the links and delets it if necessary
    for item in activeDomainsList:
        if re.search(r'\b' + re.escape(item) + r'\b', message.content):
            await message.delete()
            embed = discord.Embed(description=f'The domain in your message was identified as malicious. If you think that this is a mistake, please report it in our [discord](https://discord.gg/YYMXMvyUX8).', colour=discord.Colour.teal())
            embed.set_footer(text='https://discord.gg/YYMXMvyUX8', icon_url='https://cdn.discordapp.com/app-icons/1178385597786763274/eabc31fc418856348bfe666af6ee1458.png?size=512')
            await message.channel.send(embed=embed)
            
            #sends report into logs
            embed = discord.Embed(title='Message Deleted', colour=discord.Colour.red())
            embed.add_field(name='Author: ', value=f"```{message.author}```")
            embed.add_field(name='Content: ', value=f"```\n{message.content}\n```", inline=False)
            embed.set_footer(text='Report false positives on our discord! /help', icon_url='https://cdn.discordapp.com/app-icons/1178385597786763274/eabc31fc418856348bfe666af6ee1458.png?size=512')
            netwatch_logs_channel = await get_netwatch_logs_channel(message.guild)
            if netwatch_logs_channel:
                await netwatch_logs_channel.send(embed=embed)
            else:
                #creates logs channel if necessary
                overwrites = {
                    message.guild.default_role: discord.PermissionOverwrite(read_messages=False),
                    message.guild.me: discord.PermissionOverwrite(read_messages=True)
                }
                new_channel = await message.guild.create_text_channel('netwatch-logs', overwrites=overwrites)
                await new_channel.send(f"Created netwatch-logs channel.")
                await new_channel.send(embed=embed)
            break

#function to get the logs channel
async def get_netwatch_logs_channel(guild):
    netwatch_logs_channel = discord.utils.get(guild.text_channels, name='netwatch-logs')
    return netwatch_logs_channel

#shows when the db was last updated
@client.tree.command(description='Shows when the database was last updated.')
async def dbstatus(interaction):
    embed = discord.Embed(title='DB Status', colour=discord.Colour.teal())
    embed.add_field(name='<:update:1182004864453201981> Last Updated:', value='<t:1708434000:f>', inline=False)
    await interaction.response.send_message(embed=embed)

#help command
@client.tree.command(description='Invite to the support discord.')
async def help(interaction):
    embed = discord.Embed(title='Help', description=f'**Join our discord for help:**\nhttps://discord.gg/YYMXMvyUX8', colour=discord.Colour.teal())
    await interaction.response.send_message(embed=embed)

client.run(token)