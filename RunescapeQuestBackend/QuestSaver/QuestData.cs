using RunescapeQuestsBackend.RSPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeQuestsBackend.QuestSaver
{
    public class QuestData
    {
        public string QuestName { get; set; }
        public Quest Quest {  get; set; }   
        public Skills SkillRequirements {  get; set; }  
        public List<KeyValuePair<int, string>> QuestRequirements {  get; set; } 
    }
    //Quest status class to be used like an ENUM
    public static class QUEST_STATUS
    {
        public const string Completed = "COMPLETED";
        public const string Started = "STARTED";
        public const string NotStarted = "NOT_STARTED";
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
