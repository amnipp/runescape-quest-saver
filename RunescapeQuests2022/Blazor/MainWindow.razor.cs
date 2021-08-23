using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using RunescapeQuests;

namespace RunescapeQuests2022.Blazor
{
    public partial class MainWindow
    {
        public Skills PlayerSkillsList { get; set; }
        public List<Quest> PlayerQuestList { get; set;  }
        private string playerNameInput;
        private bool shouldRedraw = true;
        private Settings userSettings;


        protected override async void OnInitialized()
        {
            if (RSPlayer.Instance.PlayerSkills != null)
                PlayerSkillsList = RSPlayer.Instance.PlayerSkills.PlayerSkills;

            if (RSPlayer.Instance.PlayerQuests != null)
                PlayerQuestList = RSPlayer.Instance.PlayerQuests.PlayerQuestList;
            
            string playerName = "";
            userSettings = new();
            if (!string.IsNullOrEmpty(userSettings.LastUser))
            {
                //playerNameBox.Text = userSettings.LastUser;
                playerName = userSettings.LastUser;
                playerNameInput = playerName;
            }

            await LoadPlayer(playerName).ContinueWith( t => { 
                base.OnInitialized();
            });
        }
        
        public async Task LoadPlayer(string plrName)
        {
            RSPlayer player = RSPlayer.Instance;
            player.SetPlayerName(plrName);
            await player.LoadPlayerInformation();
            if (player.IsValidPlayer == false)
            {
                //MessageBox.Show(playerName + " is not a valid player name, please try another name", "Error");
                return;
            }
            StateHasChanged();
        }
        public async void FilterTable()
        {
            await JSRuntime.InvokeVoidAsync("filterTable", "QuestNameInput", "PlayerQuestTable");
        }
        protected override async void OnAfterRender(bool firstRender)
        {
            if (!firstRender)
            {
                if (RSPlayer.Instance.PlayerSkills != null)
                    PlayerSkillsList = RSPlayer.Instance.PlayerSkills.PlayerSkills;
                if (RSPlayer.Instance.PlayerQuests != null)
                {
                    PlayerQuestList = RSPlayer.Instance.PlayerQuests.PlayerQuestList;

                }
                if (PlayerQuestList != null && PlayerQuestList.Count != 0 && shouldRedraw)
                {
                    shouldRedraw = false;
                    StateHasChanged();
                }
                else if (shouldRedraw == false) shouldRedraw = true;
            }
            base.OnAfterRender(firstRender);
        }

        public void OpenQuestCheckerWindow(string questName)
        {
            Windows.QuestChecker questChecker = new (questName.Replace(" ", "_"));
            questChecker.Show();
        }

        public async void UpdatePlayer()
        {
            if (!string.IsNullOrEmpty(playerNameInput))
            {
                userSettings.LastUser = playerNameInput;
                userSettings.SaveSettings();
                
                await LoadPlayer(playerNameInput);
            }

        }
    }
}
