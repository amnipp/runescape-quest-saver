using System.Threading.Tasks;
using System.Windows;
using RunescapeQuests.src;

namespace RunescapeQuests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Delegates to allow other classes to write strings to the GUI
        public delegate void AppendToQuestLogDelegate(string text);
        public delegate void AppendToSkillLogDelegate(string text);
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

            foreach (var quest in player.PlayerQuests.PlayerQuestList)
            {
                if (quest.userEligible)
                    AppendToQuestLog(quest.title + ": " + quest.status);
                else
                    AppendToQuestLog(quest.title + ": NOT_ELIGIBLE");
            }
            foreach(var skill in player.PlayerSkills.PlayerSkills)
            {
                AppendToSkillLog(skill.name + ": " + skill.level);
            }
        }
        private void AppendToQuestLog(string toAppend)
        {
            quests.Text += toAppend + "\r\n";
        }
        private void AppendToSkillLog(string toAppend)
        {
            skills.Text += toAppend + "\r\n";
        }
    }


}
