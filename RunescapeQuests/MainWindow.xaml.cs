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
            new QuestLoader(
                new AppendToQuestLogDelegate(AppendToQuestLog), new AppendToSkillLogDelegate(AppendToSkillLog)
                ).LoadQuestInfo("Plague's_End");
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
