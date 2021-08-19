using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RunescapeQuests
{
    public class QuestLoader
    {
        //private AppendToQuestLogDelegate AppendToQuestLog;
        //private AppendToSkillLogDelegate AppendToSkillLog;

        private HtmlDocument WikiPageDoc;

        public List<KeyValuePair<int, string>> QuestList { get; private set; }
        public List<KeyValuePair<int, string>> SkillList { get; private set; }
        public bool QuestLoaded;
        public QuestLoader()
        {
            QuestList = new();
            SkillList = new();
        }
        public async Task LoadQuestInfo(string questName)
        {
            QuestLoaded = false;
            //create client and call the RS Wiki api with the given quest name, this is json formated, and it is loading only the Overview (section=2)
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://runescape.wiki/api.php?action=parse&format=json&page=" + questName + "&prop=text");
            var pageContents = await response.Content.ReadAsStringAsync();
            //get the page contents and parse the json class, most of it is unneeded; parse.parse.text.text contains the html for the overview
            ApiJsonRoot parse = JsonSerializer.Deserialize<ApiJsonRoot>(pageContents);
            //create the HAP page doc to parse the html
            WikiPageDoc = new HtmlDocument();
            WikiPageDoc.LoadHtml(parse.parse.text.text);
            bool questReqLoaded =  LoadQuestRequirements();
            bool skillReqLoaded = LoadSkillRequirments();
            if (questReqLoaded && skillReqLoaded)
                QuestLoaded = true;
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
                questListString += quest.Value;
                var playerQuests = RSPlayer.Instance.PlayerQuests.PlayerQuestList.Where(q => q.title == quest.Value).FirstOrDefault();
                if (playerQuests != null)
                {
                    if (playerQuests.status == "COMPLETED")
                        questListString += " - COMPLETED";
                }
                questListString += "\r\n";
            }
            return questListString;
        }

        public string GetSkillListString()
        {
            string skillListString = ""; 
            var playerSkills = RSPlayer.Instance.PlayerSkills.PlayerSkills;
            FieldInfo[] fields = typeof(Skills).GetFields();
            foreach (var skill in SkillList)
            {
                if (skill.Key != -1)
                    skillListString += skill.Value + " " + skill.Key;
                else
                    skillListString += skill.Value + " ";
                var skillField = fields.Where(f => f.Name == skill.Value).FirstOrDefault();
                if (skillField != null)
                {
                    var skillLevel = ((Skillvalue)skillField.GetValue(playerSkills)).level;
                    if (skillLevel >= skill.Key)
                    {
                        skillListString += " - COMPLETED";
                    }
                    else
                    {
                        skillListString += " - NOT COMPLETED";
                    }
                }
                skillListString += "\r\n";
            }
            return skillListString;
        }
        private bool LoadQuestRequirements()
        {
            if (WikiPageDoc.DocumentNode != null)
            {
                //The only part we care about is the questreq table, hap uses xpath refer to this cheatsheet https://devhints.io/xpath
                var table = WikiPageDoc.DocumentNode.SelectSingleNode("//table[@class='mw-collapsible mw-collapsed questreq']");
                if (table == null)
                {
                    return false;
                }
                if (table.ParentNode.ParentNode.ChildNodes[0].InnerText != "Requirements")
                {
                    return false; ;
                }
                //more xpath, this is mostly just looking through the wiki layout and going past the unneeded sections
                var quest = table.SelectSingleNode(".//tbody/tr[2]/td/ul/li/ul");
                //we should finally have the quest list, child nodes will be all the root quest requirments
                var rows = quest.ChildNodes;
                if (rows == null)
                    return true;
                foreach (var row in rows)
                {
                    //iterate through the root quest reqs and get their children
                    GetRequirmentQuest(row);
                    //AppendToQuestLog(questList);
                    count = 0;
                }
                return true;
            }
            return false;
        }

        private int count = 0;
        private void GetRequirmentQuest(HtmlNode questNode)
        {
            //the wiki uses ul to define a new list of children quests for the given parents, we need to keep track of this
            if (questNode.Name == "ul")
                count++;
            //todo: write comments on how this works :^)
            if ( questNode.Name != "ul")
            {
                if (questNode.ChildNodes.Count == 0)
                    return;
                var questName = questNode.ChildNodes[0].InnerText;
                if ((questNode.ChildNodes.Count == 2 || questNode.ChildNodes.Count == 3) && questNode.ChildNodes[1].Name == "a")
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

        private bool LoadSkillRequirments()
        {
            if (WikiPageDoc.DocumentNode != null)
            {
                //The only part we care about is the questreq table, hap uses xpath refer to this cheatsheet https://devhints.io/xpath
                var table = WikiPageDoc.DocumentNode.SelectSingleNode("//td[@class=\"questdetails-info qc-active\"]");
                if (table == null)
                {
                    return false;
                }
                var removeQuests = table.SelectNodes(".//table");
                if (removeQuests != null)
                    foreach (var tableNode in removeQuests)
                        table.ChildNodes.Remove(tableNode);
                var skills = table.SelectNodes(".//ul/li");
                if (skills == null)
                {
                    return true;
                }
                foreach (var skill in skills)
                {
                    var skillSplit = skill.InnerText.Split(' ');
                    if (skillSplit.Length == 3)
                        SkillList.Add(new KeyValuePair<int, string>(int.Parse(skillSplit[0]), skillSplit[2]));
                    else
                    {
                        SkillList.Add(new KeyValuePair<int, string>(-1, skill.InnerText));
                    }
                }
                return true;
            }
            return false;
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
