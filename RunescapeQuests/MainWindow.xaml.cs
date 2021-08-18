using System.Threading.Tasks;
using System.Windows;
using RunescapeQuests.src;
using RunescapeQuests.gui;
namespace RunescapeQuests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            
            //new QuestLoader(
            //    new AppendToQuestLogDelegate(AppendToQuestLog), new AppendToSkillLogDelegate(AppendToSkillLog)
            //    ).LoadQuestInfo("Plague's_End");
        }
        private async void LoadPlayerInfo(string PlayerName)
        {
            RSPlayer player = RSPlayer.Instance;
            player.SetPlayerName(PlayerName);
            await player.LoadPlayerInformation();
            if(player.IsValidPlayer == false)
            {
                MessageBox.Show(PlayerName + " is not a valid player name, please try another name", "Error");
                return;
            }
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
            if(string.IsNullOrEmpty(playerName))
            {
                MessageBox.Show("Please enter a user!", "Error");
                return;
            }
            LoadPlayerInfo(playerName);
        }
    }
}
