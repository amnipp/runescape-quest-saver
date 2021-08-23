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

namespace RunescapeQuests2022.Windows
{
    /// <summary>
    /// Interaction logic for QuestChecker.xaml
    /// </summary>
    public partial class QuestChecker : Window
    {
        //Delegates to allow other classes to write strings to the GUI
        //public delegate void AppendToQuestLogDelegate(string text);
        //public delegate void AppendToSkillLogDelegate(string text);
        public QuestChecker(string questName)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddBlazorWebView();
            serviceCollection.AddSingleton<string>(questName);
            //serviceCollection.AddSingleton<TestAppData>(testAppData);
            Resources.Add("services", serviceCollection.BuildServiceProvider());

            InitializeComponent();
        }

        public async void GetQuestRequirments(string questName)
        {
            /*var questNameFormatted = questName.Replace(' ', '_');
            var questLoader = new QuestLoader();
            await questLoader.LoadQuestInfo(questNameFormatted);
            AppendToQuestLog(questLoader.GetQuestListString());
            AppendToSkillLog(questLoader.GetSkillListString());*/

        }
        private void AppendToQuestLog(string toAppend)
        {
            //quests.Text += toAppend + "\r\n";
        }
        private void AppendToSkillLog(string toAppend)
        {
            //skills.Text += toAppend + "\r\n";
        }
    }
}
