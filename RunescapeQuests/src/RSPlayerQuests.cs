using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RunescapeQuests.src
{
    class RSPlayerQuests
    {
        public List<Quest> PlayerQuestList {get; private set;}
        public async Task LoadPlayerQuests(string PlayerName)
        {
            PlayerQuestList = new();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://apps.runescape.com/runemetrics/quests?user=" + PlayerName);
            var pageContents = await response.Content.ReadAsStringAsync();
            RunemetricQuests parse = JsonSerializer.Deserialize<RunemetricQuests>(pageContents);
            PlayerQuestList = parse.quests;
        }
    }
    
    //Runemetric quest json classes
    public class Quest
    {
        public string title { get; set; }
        public string status { get; set; }
        public int difficulty { get; set; }
        public bool members { get; set; }
        public int questPoints { get; set; }
        public bool userEligible { get; set; }
    }

    public class RunemetricQuests
    {
        public List<Quest> quests { get; set; }
        public string loggedIn { get; set; }
    }
}
