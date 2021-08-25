using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeQuestsBackend.QuestSaver
{
    public class QuestOrganizer
    {
        public List<List<KeyValuePair<int, QuestData>>> SavedQuestList {  get; set; }

        public QuestOrganizer() 
        {
            //todo load quests from file
            SavedQuestList = new();
        }

        public void AddQuestChain(string questName)
        {
            List<KeyValuePair<int, QuestData>> questList = new();
            var mainQuest = CachedQuests.Instance.CachedQuestData.Where(q => q.QuestName == questName.Replace("_", " ")).FirstOrDefault();

            var foundQuest = SavedQuestList.SelectMany(q => q.Where(c => c.Key == -1 && c.Value == mainQuest)).FirstOrDefault();
            if(foundQuest.Value != null)
            {
                return;
            }
            //add main quest to list
            questList.Add(new KeyValuePair<int, QuestData>(-1, mainQuest));
            foreach (var quest in mainQuest.QuestRequirements)
            {
                var childQuest = CachedQuests.Instance.CachedQuestData.Where(q => q.QuestName == quest.Value.Replace("_", " ")).FirstOrDefault();
                if (childQuest != null)
                {
                    if (childQuest.Quest.status != QUEST_STATUS.Completed)
                        questList.Add(new KeyValuePair<int, QuestData>(quest.Key, childQuest));
                }
            }
            SavedQuestList.Add(questList);
        }

        
    }
}
