using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static RunescapeQuests.gui.QuestChecker;

namespace RunescapeQuests.src
{
    class QuestLoader
    {
        private AppendToQuestLogDelegate AppendToQuestLog;
        private AppendToSkillLogDelegate AppendToSkillLog;

        private HtmlDocument WikiPageDoc;

        private List<KeyValuePair<int, string>> QuestList;

        public QuestLoader(AppendToQuestLogDelegate questLog, AppendToSkillLogDelegate skillLog)
        {
            AppendToQuestLog = questLog;
            AppendToSkillLog = skillLog;
            QuestList = new();
        }
        public async void LoadQuestInfo(string questName)
        {
            //create client and call the RS Wiki api with the given quest name, this is json formated, and it is loading only the Overview (section=2)
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://runescape.wiki/api.php?action=parse&format=json&page=" + questName + "&prop=text&section=2");
            var pageContents = await response.Content.ReadAsStringAsync();
            //get the page contents and parse the json class, most of it is unneeded; parse.parse.text.text contains the html for the overview
            ApiJsonRoot parse = JsonSerializer.Deserialize<ApiJsonRoot>(pageContents);
            //create the HAP page doc to parse the html
            WikiPageDoc = new HtmlDocument();
            WikiPageDoc.LoadHtml(parse.parse.text.text);
            LoadQuestRequirements();
            LoadSkillRequirments();
        }

        public string GetQuestListString()
        {
            string questListString = "";
            foreach(var quest in QuestList)
            {
                string tab = "      ";
                for(int i = 1; i <= quest.Key; ++i)
                {
                    questListString += tab;
                }
                questListString += quest.Value + "\r\n";
            }
            return questListString;
        }
        //todo load quests into an array then format and append
        public void LoadQuestRequirements()
        {
            if (WikiPageDoc.DocumentNode != null)
            {
                //The only part we care about is the questreq table, hap uses xpath refer to this cheatsheet https://devhints.io/xpath
                var table = WikiPageDoc.DocumentNode.SelectSingleNode("//table[@class='mw-collapsible mw-collapsed questreq']");
                if (table == null)
                {
                    AppendToQuestLog("None");
                    return;
                }
                if (table.ParentNode.ParentNode.ChildNodes[0].InnerText != "Requirements")
                {
                    AppendToQuestLog("None");
                    return;
                }
                //more xpath, this is mostly just looking through the wiki layout and going past the unneeded sections
                var quest = table.SelectSingleNode(".//tbody/tr[2]/td/ul/li/ul");
                //we should finally have the quest list, child nodes will be all the root quest requirments
                var rows = quest.ChildNodes;
                if (rows == null)
                    return;
                foreach (var row in rows)
                {
                    //iterate through the root quest reqs and get their children
                    GetRequirmentQuest(row);
                    //AppendToQuestLog(questList);
                    count = 0;
                }
                AppendToQuestLog(GetQuestListString());
            }
        }

        private int count = 0;
        private void GetRequirmentQuest(HtmlNode questNode)
        {
            string requiredQuests = "";
            //the wiki uses ul to define a new list of children quests for the given parents, we need to keep track of this
            if (questNode.Name == "ul")
                count++;
            //todo: write comments on how this works :^)
            if ( questNode.Name != "ul")
            {
                if (questNode.ChildNodes.Count == 0)
                    return;
                if (questNode.ChildNodes[0].InnerText == "Heroes' Quest")
                    Console.WriteLine();
                var questName = questNode.ChildNodes[0].InnerText;
                if (questNode.ChildNodes.Count == 2 && questNode.ChildNodes[1].Name == "a")
                {
                    questName += questNode.ChildNodes[1].InnerText;
                    questNode.ChildNodes.RemoveAt(1);
                }
                QuestList.Add(new KeyValuePair<int, string>(count, questName));
                questNode.ChildNodes.RemoveAt(0);
            }

            foreach (var quest in questNode.ChildNodes)
            {
                //iterate through the children quests and do recursion to get that child's required quest
                GetRequirmentQuest(quest);
            }
        }

        //todo load skills into array then append to skill log
        public void LoadSkillRequirments()
        {
            if (WikiPageDoc.DocumentNode != null)
            {
                //The only part we care about is the questreq table, hap uses xpath refer to this cheatsheet https://devhints.io/xpath
                var table = WikiPageDoc.DocumentNode.SelectSingleNode("//td[@class=\"questdetails-info qc-active\"]");
                if (table == null)
                {
                    AppendToSkillLog("None");
                    return;
                }
                var removeQuests = table.SelectNodes(".//table");
                if (removeQuests != null)
                    foreach (var tableNode in removeQuests)
                        table.ChildNodes.Remove(tableNode);
                var skills = table.SelectNodes(".//ul");
                if (skills == null)
                {
                    AppendToSkillLog("None");
                    return;
                }
                foreach (var skill in skills)
                {
                    AppendToSkillLog(skill.InnerText);
                }
            }
        }
    }

    //==================================
    // Json classes for the RS wiki 
    //==================================
    public class Text
    {
        [JsonPropertyName("*")]
        public string text { get; set; }
    }

    public class Parse
    {
        public string title { get; set; }
        public int pageid { get; set; }
        public Text text { get; set; }
    }

    public class ApiJsonRoot
    {
        public Parse parse { get; set; }
    }
}
