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
using RunescapeQuests2022.Windows;
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
            QuestOrganizerStates.QuestNameChanged += QuestNameChanged;

            base.OnInitialized();
        }
        private void SetQuestMarkup()
        {
            questMarkup = "";
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
            skillMarkup = "";
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
        private bool _shouldUpdate = true;
        protected override void OnAfterRender(bool firstRender)
        {
            if (CachedQuests.Instance.CachedQuestData == null) return;
            var foundQuest = CachedQuests.Instance.CachedQuestData.Where(q => q.QuestName == questName.Replace("_", " ")).FirstOrDefault();
            if (foundQuest == null) return;
            QuestList = foundQuest.QuestRequirements;
            SkillList = new();
            foreach (var skill in foundQuest.SkillRequirements)
            {
                if (skill != null)
                    SkillList.Add(new KeyValuePair<int, string>(skill.level, skill.name));
            }
            SetQuestMarkup();
            SetSkillMarkup();
            if(_shouldUpdate == true)
            {
                _shouldUpdate = false;
                StateHasChanged();
            }
            else
            {
                _shouldUpdate = true;
            }
            base.OnAfterRender(firstRender);
        }

        private void QuestNameChanged(object sender, EventArgs e)
        {
            questName = ((QuestOrganizerState)sender).QuestName;
            StateHasChanged();
        }

        public void AddQuests()
        {
            //List<KeyValuePair<int, string>> QuestList
            SavedQuestOrganizer.AddQuestChain(questName);
        }
    }
}
