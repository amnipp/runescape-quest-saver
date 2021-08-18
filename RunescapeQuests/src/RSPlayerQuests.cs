using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RunescapeQuests.src
{
    public class RSPlayerQuests
    {
        public List<Quest> PlayerQuestList { get; private set; }
        public async Task LoadPlayerQuests(string PlayerName)
        {
            ValidPlayerQuests = false;
            PlayerQuestList = new();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://apps.runescape.com/runemetrics/quests?user=" + PlayerName);
            var pageContents = await response.Content.ReadAsStringAsync();
            RunemetricQuests parse = JsonSerializer.Deserialize<RunemetricQuests>(pageContents);
            if (parse.quests == null)
                return;
            PlayerQuestList = parse.quests;
            foreach(var quest in PlayerQuestList)
            {
                switch (quest.status)
                {
                    case "COMPLETED":
                        quest.color = "Green";
                        break;
                    case "STARTED":
                        quest.color = "Blue";
                        break;
                    case "NOT_STARTED":
                        if (quest.userEligible == false)
                        {
                            quest.color = "Black";
                        }
                        else
                        {
                            quest.color = "Red";
                        }
                        break;
                }
            }
            ValidPlayerQuests = true;
        }
        public bool ValidPlayerQuests { get; private set; }
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
        public string color { get; set; }
    }

    public class RunemetricQuests
    {
        public List<Quest> quests { get; set; }
        public string loggedIn { get; set; }
    }
}
