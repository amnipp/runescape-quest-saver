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
using RunescapeQuestsBackend;
using System.ComponentModel;
using RunescapeQuests2022.Windows;
using Microsoft.AspNetCore.Components.WebView.Wpf;
namespace RunescapeQuests2022
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly TestAppData testAppData = new();
        public MainWindow()
        { 
            new FixStaticAssetsJson();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddBlazorWebView();
            //serviceCollection.AddSingleton<TestAppData>(testAppData);
            Resources.Add("services", serviceCollection.BuildServiceProvider());

            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private async void LoadPlayerInfo()
        {




            //help.UpdatePlayerSkillsList();
            /*var blazorSkillComponent = BlazorSkills.ComponentType;
            var blazorSkillType = Type.GetType(blazorSkillComponent.FullName);
            var updateMethod = blazorSkillComponent.GetMethod("UpdatePlayerSkillsList");*/

            /*(foreach (var skill in player.PlayerSkills.PlayerSkills)
            {
                AppendToSkillLog(skill.name + ": " + skill.level);
            }
        }
        private void AppendToQuestLog(string toAppend)
        {
             /*quests.Text += toAppend + "\r\n";*/
        }
        private void AppendToSkillLog(string toAppend)
        {
            //skills.Text += toAppend + "\r\n";
        }

        private void questsbox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            /*Quest questObj = (Quest)quests.SelectedItem;
            if (questObj == null) return;
            QuestChecker questChecker = new();
            questChecker.GetQuestRequirments(questObj.title);
            questChecker.Show();*/
        }

        private void loadPlayer_Click(object sender, RoutedEventArgs e)
        {
           /* //skills.Text = "";
            var playerName = playerNameBox.Text;
            if (string.IsNullOrEmpty(playerName))
            {
                MessageBox.Show("Please enter a user!", "Error");
                return;
            }
            LoadPlayerInfo(playerName);*/
        }

        private void searchQuestBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            /*ICollectionView view = CollectionViewSource.GetDefaultView(quests.ItemsSource);
            view.Filter = (obj) => {
                Quest quest = obj as Quest;
                return quest.title.ToLower().Contains(searchQuestBox.Text.ToLower());
            };*/
        }

        private void filterAll_Click(object sender, RoutedEventArgs e)
        {
            //BlazorView.WebView.CoreWebView2.ExecuteScriptAsync();
            /*ICollectionView view = CollectionViewSource.GetDefaultView(quests.ItemsSource);
            view.Filter = null;*/
        }

        private void filterStarted_Click(object sender, RoutedEventArgs e)
        {
            /*ICollectionView view = CollectionViewSource.GetDefaultView(quests.ItemsSource);
            view.Filter = (obj) => {
                Quest quest = obj as Quest;
                return quest.status == QUEST_STATUS.Started;
            };*/
        }

        private void filterCompleted_Click(object sender, RoutedEventArgs e)
        {
            /*ICollectionView view = CollectionViewSource.GetDefaultView(quests.ItemsSource);
            view.Filter = (obj) => {
                Quest quest = obj as Quest;
                return quest.status == QUEST_STATUS.Completed;
            };*/
        }

        private void filterEligible_Click(object sender, RoutedEventArgs e)
        {
            /*ICollectionView view = CollectionViewSource.GetDefaultView(quests.ItemsSource);
            view.Filter = (obj) => {
                Quest quest = obj as Quest;
                return quest.status == QUEST_STATUS.NotStarted && quest.userEligible == true;
            };*/
        }

        private void filterNotEligible_Click(object sender, RoutedEventArgs e)
        {
            /*ICollectionView view = CollectionViewSource.GetDefaultView(quests.ItemsSource);
            view.Filter = (obj) => {
                Quest quest = obj as Quest;
                return quest.status == QUEST_STATUS.NotStarted && quest.userEligible == false;
            };*/
        }

        private void testBtn_Click(object sender, RoutedEventArgs e)
        {
            TestWindow testWnd = new();
            testWnd.Title = "Test";
            try
            {
                testWnd.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error");
            }
        }
    }
}
