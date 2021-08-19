using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RunescapeQuests;
using System.ComponentModel;

namespace RunescapeQuests2022
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Settings userSettings;
        public MainWindow()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddBlazorWebView();
            Resources.Add("services", serviceCollection.BuildServiceProvider());

            userSettings = new();
            InitializeComponent();
            if (!string.IsNullOrEmpty(userSettings.LastUser))
            {
                playerNameBox.Text = userSettings.LastUser;
                LoadPlayerInfo(userSettings.LastUser);
            }
        }
        private async void LoadPlayerInfo(string PlayerName)
        {
            RSPlayer player = RSPlayer.Instance;
            player.SetPlayerName(PlayerName);
            await player.LoadPlayerInformation();
            if (player.IsValidPlayer == false)
            {
                MessageBox.Show(PlayerName + " is not a valid player name, please try another name", "Error");
                return;
            }

            userSettings.LastUser = PlayerName;
            userSettings.SaveSettings();

            quests.ItemsSource = player.PlayerQuests.PlayerQuestList;

            foreach (var skill in player.PlayerSkills.PlayerSkills)
            {
                AppendToSkillLog(skill.name + ": " + skill.level);
            }
        }
        /*private void AppendToQuestLog(string toAppend)
        {
            quests.Text += toAppend + "\r\n";
        }*/
        private void AppendToSkillLog(string toAppend)
        {
            skills.Text += toAppend + "\r\n";
        }

        private void questsbox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Quest questObj = (Quest)quests.SelectedItem;
            if (questObj == null) return;
            QuestChecker questChecker = new();
            questChecker.GetQuestRequirments(questObj.title);
            questChecker.Show();
        }

        private void loadPlayer_Click(object sender, RoutedEventArgs e)
        {
            skills.Text = "";
            var playerName = playerNameBox.Text;
            if (string.IsNullOrEmpty(playerName))
            {
                MessageBox.Show("Please enter a user!", "Error");
                return;
            }
            LoadPlayerInfo(playerName);
        }

        private void searchQuestBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(quests.ItemsSource);
            view.Filter = (obj) => {
                Quest quest = obj as Quest;
                return quest.title.ToLower().Contains(searchQuestBox.Text.ToLower());
            };
        }

        private void filterAll_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(quests.ItemsSource);
            view.Filter = null;
        }

        private void filterStarted_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(quests.ItemsSource);
            view.Filter = (obj) => {
                Quest quest = obj as Quest;
                return quest.status == QUEST_STATUS.Started;
            };
        }

        private void filterCompleted_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(quests.ItemsSource);
            view.Filter = (obj) => {
                Quest quest = obj as Quest;
                return quest.status == QUEST_STATUS.Completed;
            };
        }

        private void filterEligible_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(quests.ItemsSource);
            view.Filter = (obj) => {
                Quest quest = obj as Quest;
                return quest.status == QUEST_STATUS.NotStarted && quest.userEligible == true;
            };
        }

        private void filterNotEligible_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(quests.ItemsSource);
            view.Filter = (obj) => {
                Quest quest = obj as Quest;
                return quest.status == QUEST_STATUS.NotStarted && quest.userEligible == false;
            };
        }
    }
}
