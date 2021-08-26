using RunescapeQuestsBackend.RSPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RunescapeQuestsBackend.QuestSaver
{
    public static class CompileQuestDatabase
    {
        private static readonly string questInfoPath = System.AppDomain.CurrentDomain.BaseDirectory + "wiki_quest_data.json";
        public static async Task Compile()
        {
            List<QuestData> questDataList = new();
            List<Task> taskList = new();
            if (RSPlayer.RSPlayer.Instance.PlayerQuests == null)
                return;
            var questList = RSPlayer.RSPlayer.Instance.PlayerQuests.PlayerQuestList;
            foreach (var quest in questList)
            {
                QuestWikiLookup lookup = new();
                taskList.Add(lookup.LoadQuestInfo(quest.title.Replace(" ", "_")).ContinueWith(t => {
                    QuestData questData = new();
                    questData.QuestName = quest.title;
                    questData.Quest = quest;
                    questData.SkillRequirements = SetupRequiredSkills(lookup.SkillList);
                    questData.QuestRequirements = lookup.QuestList;
                    questDataList.Add(questData);
                }));
            }
            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };
            await Task.WhenAll(taskList.ToArray());
            CachedQuests.Instance.CachedQuestData = questDataList;
            string questSaveData = JsonSerializer.Serialize(questDataList, options);
            if (!File.Exists(questInfoPath))
            {
                File.Create(questInfoPath);
            }
            File.WriteAllText(questInfoPath, questSaveData);
        }
        private static Skills SetupRequiredSkills(List<KeyValuePair<int, string>> skillList)
        {
            Skills requiredSkills = new();
            foreach (var skill in skillList)
            {
                if (skill.Key != -1)
                {
                    requiredSkills.SetSkillValue(skill.Value, skill.Key);
                }
            }
            return requiredSkills;
        }
    }

    public class CachedQuests
    {
        private static readonly string questInfoPath = System.AppDomain.CurrentDomain.BaseDirectory + "wiki_quest_data.json";
        public List<QuestData> CachedQuestData { get; set;  }
        private static readonly Lazy<CachedQuests> lazyPlayer = new Lazy<CachedQuests>(() => new CachedQuests());
        public static CachedQuests Instance { get { return lazyPlayer.Value; } }
        private CachedQuests()
        {
        }
        public void LoadCachedQuests()
        {
            if (!File.Exists(questInfoPath))
            {
                return;
            }
            string questText = File.ReadAllText(questInfoPath);
            if (questText.Length == 0) return;
            CachedQuestData = JsonSerializer.Deserialize<List<QuestData>>(questText);
        }
    }
}
