using Newtonsoft.Json;

namespace NetWatchCS.Config
{
    internal class BotConfig
    {
        public string DiscordBotToken { get; set; }
        public string DiscordBotPrefix { get; set; }

        public async Task ReadJSON()
        {
            using (StreamReader streamReader = new StreamReader("Config/config.json"))
            {
                string json = await streamReader.ReadToEndAsync();
                ConfigStruct jsonData = JsonConvert.DeserializeObject<ConfigStruct>(json);

                this.DiscordBotToken = jsonData.token;
                this.DiscordBotPrefix = jsonData.prefix;
            }
        }
    }
}
