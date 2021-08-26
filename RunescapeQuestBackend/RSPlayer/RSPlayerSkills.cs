using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RunescapeQuestsBackend.RSPlayer
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
        [JsonInclude]
        public Skillvalue Attack; //id=0
        [JsonInclude]
        public Skillvalue Defence; //id=1
        [JsonInclude]
        public Skillvalue Strength; //id=2
        [JsonInclude]
        public Skillvalue Constitution; //id=3
        [JsonInclude]
        public Skillvalue Ranged; //id=4
        [JsonInclude]
        public Skillvalue Prayer; //id=5
        [JsonInclude]
        public Skillvalue Magic; //id=6
        [JsonInclude]
        public Skillvalue Cooking; //id=7
        [JsonInclude]
        public Skillvalue Woodcutting; //id=8
        [JsonInclude]
        public Skillvalue Fletching; //id=9
        [JsonInclude]
        public Skillvalue Fishing; //id=10
        [JsonInclude]
        public Skillvalue Firemaking; //id=11
        [JsonInclude]
        public Skillvalue Crafting; //id=12
        [JsonInclude]
        public Skillvalue Smithing; //id=13
        [JsonInclude]
        public Skillvalue Mining; //id=14
        [JsonInclude]
        public Skillvalue Herblore; //id=15
        [JsonInclude]
        public Skillvalue Agility; //id=16
        [JsonInclude]
        public Skillvalue Thieving; //id=17
        [JsonInclude]
        public Skillvalue Slayer; //id=18
        [JsonInclude]
        public Skillvalue Farming; //id=19
        [JsonInclude]
        public Skillvalue Runecrafting; //id=20
        [JsonInclude]
        public Skillvalue Hunter; //id=21
        [JsonInclude]
        public Skillvalue Construction; //id=22
        [JsonInclude]
        public Skillvalue Summoning; //id=23
        [JsonInclude]
        public Skillvalue Dungeoneering; //id=24
        [JsonInclude]
        public Skillvalue Divination; //id=25
        [JsonInclude]
        public Skillvalue Invention; //id=26
        [JsonInclude]
        public Skillvalue Archaeology; //id=27
        public IEnumerator<Skillvalue> GetEnumerator()
        {
            var fields = GetType().GetFields();
            foreach(var field in fields)
            {
                yield return (Skillvalue)field.GetValue(this);
            }
        }
        public void SetSkillValue(string fieldName, int value)
        {
            var field = GetType().GetFields().Where(f => fieldName.Contains(f.Name)).FirstOrDefault();
            if (field == null) return;
            Skillvalue sv = new();
            sv.id = (int)((SkillID)Enum.Parse(typeof(SkillID), field.Name));
            sv.level = value;
            sv.name = field.Name;
            sv.rank = -1;
            sv.xp = -1;
            field.SetValue(this, sv);
        }
    }
    public enum SkillID
    {
        Attack, 
        Defence,
        Strength,
        Constitution,
        Ranged,
        Prayer,
        Magic,
        Cooking,
        Woodcutting,
        Fletching,
        Fishing,
        Firemaking,
        Crafting,
        Smithing,
        Mining,
        Herblore,
        Agility,
        Thieving,
        Slayer,
        Farming,
        Runecrafting,
        Hunter,
        Construction,
        Summoning,
        Dungeoneering,
        Divination,
        Invention,
        Archaeology

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
