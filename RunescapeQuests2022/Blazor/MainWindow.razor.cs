using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;

using RunescapeQuests;

namespace RunescapeQuests2022.Blazor
{
    public partial class MainWindow
    {
        private Settings userSettings;
        private string playerName;
        private bool needToReloadQuests = true;
        public Skills PlayerSkillsList { get; set; }
        public List<Quest> PlayerQuestList { get; set;  }
        protected override async void OnInitialized()
        {
            if (RSPlayer.Instance.PlayerSkills != null)
                PlayerSkillsList = RSPlayer.Instance.PlayerSkills.PlayerSkills;
            if (RSPlayer.Instance.PlayerQuests != null)
                PlayerQuestList = RSPlayer.Instance.PlayerQuests.PlayerQuestList;
            await LoadPlayerInfo();
        }

        public async Task LoadPlayerInfo()
        {
            userSettings = new();
            if (!string.IsNullOrEmpty(userSettings.LastUser))
            {
                //playerNameBox.Text = userSettings.LastUser;
                playerName = userSettings.LastUser;
            }
            RSPlayer player = RSPlayer.Instance;
            player.SetPlayerName(playerName);
            await player.LoadPlayerInformation();
            if (player.IsValidPlayer == false)
            {
                //MessageBox.Show(playerName + " is not a valid player name, please try another name", "Error");
                return;
            }

            userSettings.LastUser = playerName;
            userSettings.SaveSettings();
            StateHasChanged();  
        }
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (RSPlayer.Instance.PlayerSkills != null)
                PlayerSkillsList = RSPlayer.Instance.PlayerSkills.PlayerSkills;
            if (RSPlayer.Instance.PlayerQuests != null && RSPlayer.Instance.PlayerQuests.PlayerQuestList.Count != 0 && needToReloadQuests == true)
            {
                PlayerQuestList = RSPlayer.Instance.PlayerQuests.PlayerQuestList;
                needToReloadQuests = false;
                StateHasChanged();
            }
        }
    }
}
