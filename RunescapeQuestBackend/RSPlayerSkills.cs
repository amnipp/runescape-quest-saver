using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RunescapeQuests
{
    public class RSPlayerSkills
    {
        public Skills PlayerSkills { get; private set; }
        public async Task LoadPlayerStats(string PlayerName)
        {
            ValidPlayerStats = false;
            PlayerSkills = new();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://apps.runescape.com/runemetrics/profile/profile?user=" + PlayerName);
            var pageContents = await response.Content.ReadAsStringAsync();
            RuneMetricProfile parse = JsonSerializer.Deserialize<RuneMetricProfile>(pageContents);
            if (String.IsNullOrEmpty(parse.name))
                return;
            parse.skillvalues.Sort(delegate (Skillvalue s1, Skillvalue s2) { return s1.id.CompareTo(s2.id);});
            FieldInfo[] fields = typeof(Skills).GetFields();
            int skillIter = 0;
            foreach(var field in fields)
            {
                parse.skillvalues[skillIter].name = field.Name;
                field.SetValue(PlayerSkills, parse.skillvalues[skillIter]);
                skillIter++;
            }
            ValidPlayerStats = true;
        }
        public bool ValidPlayerStats { get; private set; }
    }
    public class Skills
    {
        public Skillvalue Attack; //id=0
        public Skillvalue Defence; //id=1
        public Skillvalue Strength; //id=2
        public Skillvalue Constitution; //id=3
        public Skillvalue Ranged; //id=4
        public Skillvalue Prayer; //id=5
        public Skillvalue Magic; //id=6
        public Skillvalue Cooking; //id=7
        public Skillvalue Woodcutting; //id=8
        public Skillvalue Fletching; //id=9
        public Skillvalue Fishing; //id=10
        public Skillvalue Firemaking; //id=11
        public Skillvalue Crafting; //id=12
        public Skillvalue Smithing; //id=13
        public Skillvalue Mining; //id=14
        public Skillvalue Herblore; //id=15
        public Skillvalue Agility; //id=16
        public Skillvalue Thieving; //id=17
        public Skillvalue Slayer; //id=18
        public Skillvalue Farming; //id=19
        public Skillvalue Runecrafting; //id=20
        public Skillvalue Hunter; //id=21
        public Skillvalue Construction; //id=22
        public Skillvalue Summoning; //id=23
        public Skillvalue Dungeoneering; //id=24
        public Skillvalue Divination; //id=25
        public Skillvalue Invention; //id=26
        public Skillvalue Archaeology; //id=27
        public IEnumerator<Skillvalue> GetEnumerator()
        {
            var fields = GetType().GetFields();
            foreach(var field in fields)
            {
                yield return (Skillvalue)field.GetValue(this);
            }
        }
    }

    // Runemetrics Profile JSON Classes
    public class Activity
    {
        public string date { get; set; }
        public string details { get; set; }
        public string text { get; set; }
    }

    public class Skillvalue
    {
        public int level { get; set; }
        public int xp { get; set; }
        public int rank { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class RuneMetricProfile
    {
        public int magic { get; set; }
        public int questsstarted { get; set; }
        public int totalskill { get; set; }
        public int questscomplete { get; set; }
        public int questsnotstarted { get; set; }
        public int totalxp { get; set; }
        public int ranged { get; set; }
        public List<Activity> activities { get; set; }
        public List<Skillvalue> skillvalues { get; set; }
        public string name { get; set; }
        public string rank { get; set; }
        public int melee { get; set; }
        public int combatlevel { get; set; }
        public string loggedIn { get; set; }
    }
}
