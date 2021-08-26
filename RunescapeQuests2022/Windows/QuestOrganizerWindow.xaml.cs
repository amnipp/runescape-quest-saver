using Microsoft.Extensions.DependencyInjection;
using RunescapeQuestsBackend.QuestSaver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace RunescapeQuests2022.Windows
{
    /// <summary>
    /// Interaction logic for QuestOrganizerWindow.xaml
    /// </summary>
    public partial class QuestOrganizerWindow : Window
    {
        private string _questName = "";
        private QuestOrganizerState _questOrganizerState = new();
        public QuestOrganizerWindow(QuestOrganizer SavedQuestOrganizer)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddBlazorWebView();
            serviceCollection.AddSingleton<QuestOrganizer>(SavedQuestOrganizer);
            serviceCollection.AddSingleton<string>(_questName);
            serviceCollection.AddSingleton<QuestOrganizerState>(_questOrganizerState);
            //serviceCollection.AddSingleton<TestAppData>(testAppData);
            Resources.Add("services", serviceCollection.BuildServiceProvider());
            InitializeComponent();
        }
    }

    public class QuestOrganizerState
    {
        private string _questName;
        [Category("Action")]
        [Description("Fires when the value is changed")]
        public event EventHandler QuestNameChanged;


        protected virtual void OnQuestNameChanged()
        {
            if (QuestName != null) QuestNameChanged(this, EventArgs.Empty);
        }


        public string QuestName 
        {
            get 
            {
                return _questName;
            } 
            set
            {
                _questName = value;
                OnQuestNameChanged();
            }
        }
    }
}
