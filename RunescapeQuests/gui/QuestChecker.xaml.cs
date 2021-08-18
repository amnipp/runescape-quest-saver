using RunescapeQuests.src;
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
using System.Windows.Shapes;

namespace RunescapeQuests.gui
{
    /// <summary>
    /// Interaction logic for QuestChecker.xaml
    /// </summary>
    public partial class QuestChecker : Window
    {
        //Delegates to allow other classes to write strings to the GUI
        public delegate void AppendToQuestLogDelegate(string text);
        public delegate void AppendToSkillLogDelegate(string text);
        public QuestChecker()
        {
            InitializeComponent();
        }

        public async void GetQuestRequirments(string questName)
        {
            var questNameFormatted = questName.Replace(' ', '_');
            var questLoader = new QuestLoader();
            await questLoader.LoadQuestInfo(questNameFormatted);
            AppendToQuestLog(questLoader.GetQuestListString());
            AppendToSkillLog(questLoader.GetSkillListString());

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
