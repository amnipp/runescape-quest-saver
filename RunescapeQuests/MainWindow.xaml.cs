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
            LoadPlayerInfo();
            //new QuestLoader(
            //    new AppendToQuestLogDelegate(AppendToQuestLog), new AppendToSkillLogDelegate(AppendToSkillLog)
            //    ).LoadQuestInfo("Plague's_End");
        }
        private async void LoadPlayerInfo()
        {
            RSPlayer player = new("Dagrondx11");
            await player.LoadPlayerInformation();
            quests.ItemsSource = player.PlayerQuests.PlayerQuestList;
            quests.DisplayMemberPath = "title";
            /*foreach (var quest in player.PlayerQuests.PlayerQuestList)
            {
                if (quest.userEligible)
                    AppendToQuestLog(quest.title + ": " + quest.status);
                else
                    AppendToQuestLog(quest.title + ": NOT_ELIGIBLE");
            }*/
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
    }
}
