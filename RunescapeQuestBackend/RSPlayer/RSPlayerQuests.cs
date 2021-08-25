using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RunescapeQuestsBackend.QuestSaver;
namespace RunescapeQuestsBackend.RSPlayer
{
    public class RSPlayerQuests
    {
        public List<Quest> PlayerQuestList { get; private set; }
        
        public List<QuestData> PlayerSavedQuests { get; set; }

        public async Task LoadPlayerQuests(string PlayerName)
        {
            //Todo load saved quests from file
            PlayerSavedQuests = new();
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
                    case QUEST_STATUS.Completed:
                        quest.color = "Green";
                        break;
                    case QUEST_STATUS.Started:
                        quest.color = "Blue";
                        break;
                    case QUEST_STATUS.NotStarted:
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
}
