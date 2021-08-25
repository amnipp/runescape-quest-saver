using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;

using RunescapeQuestsBackend;
using RunescapeQuestsBackend.QuestSaver;
using RunescapeQuestsBackend.RSPlayer;

namespace RunescapeQuests2022.Blazor
{
    public partial class QuestCheckerRazor
    {
        private List<KeyValuePair<int, string>> QuestList = new();
        private List<KeyValuePair<int, string>> SkillList = new();
        private string questMarkup = "";
        private string skillMarkup = "";
        protected override void OnInitialized()
        {
            var foundQuest = CachedQuests.Instance.CachedQuestData.Where(q => q.QuestName == questName.Replace("_", " ")).FirstOrDefault();
            if (foundQuest == null) return;
            QuestList = foundQuest.QuestRequirements;
            foreach(var skill in foundQuest.SkillRequirements)
            {
                if(skill != null)
                    SkillList.Add(new KeyValuePair<int, string>(skill.level, skill.name));
            }
            SetQuestMarkup();
            SetSkillMarkup();
            base.OnInitialized();
        }
        private void SetQuestMarkup()
        {
            questMarkup += "<ul style=\"list-style-type:none;\">";
            foreach (var quest in QuestList)
            {
                questMarkup += "<li>";
                for (int i=1; i <= quest.Key;i++)
                {
                    questMarkup += "&emsp;";
                }
                questMarkup += quest.Value;
                var playerQuests = RSPlayer.Instance.PlayerQuests.PlayerQuestList.Where(q => q.title == quest.Value).FirstOrDefault();
                if (playerQuests != null)
                {
                    if (playerQuests.status == "COMPLETED")
                        questMarkup += " - COMPLETED";
                }
                questMarkup += "</li>";
            }
            questMarkup += "</ul>";
        }
        private void SetSkillMarkup()
        {
            /*            string skillListString = ""; 
            var playerSkills = RSPlayer.Instance.PlayerSkills.PlayerSkills;
            FieldInfo[] fields = typeof(Skills).GetFields();
            foreach (var skill in SkillList)
            {
                if (skill.Key != -1)
                    skillListString += skill.Value + " " + skill.Key;
                else
                    skillListString += skill.Value + " ";
                var skillField = fields.Where(f => f.Name == skill.Value).FirstOrDefault();
                if (skillField != null)
                {
                    var skillLevel = ((Skillvalue)skillField.GetValue(playerSkills)).level;
                    if (skillLevel >= skill.Key)
                    {
                        skillListString += " - COMPLETED";
                    }
                    else
                    {
                        skillListString += " - NOT COMPLETED";
                    }
                }
                skillListString += "\r\n";
            }
            return skillListString;*/
            skillMarkup += "<ul style=\"list-style-type:none;\">";
            var playerSkills = RSPlayer.Instance.PlayerSkills.PlayerSkills;
            FieldInfo[] fields = typeof(Skills).GetFields();
            foreach (var skill in SkillList)
            {
                skillMarkup += "<li>";
                if (skill.Key != -1)
                    skillMarkup += skill.Value + " " + skill.Key;
                else
                    skillMarkup += skill.Value + " ";
                var skillField = fields.Where(f => f.Name == skill.Value).FirstOrDefault();
                if (skillField != null)
                {
                    var skillLevel = ((Skillvalue)skillField.GetValue(playerSkills)).level;
                    if (skillLevel >= skill.Key)
                    {
                        skillMarkup += " - COMPLETED";
                    }
                    else
                    {
                        skillMarkup += " - NOT COMPLETED";
                    }
                }
                skillMarkup += "</li>";
            }
            skillMarkup += "</ul>";
        }

        public void AddQuests()
        {
            //List<KeyValuePair<int, string>> QuestList
            SavedQuestOrganizer.AddQuestChain(questName);
        }
    }
}
