using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeQuests.src
{
    class RSPlayer
    {
        public RSPlayerSkills PlayerSkills { get; private set; }
        public RSPlayerQuests PlayerQuests { get; private set; }
        private string PlayerName;

        public RSPlayer(string PlayerName)
        {
            this.PlayerName = PlayerName;
        }
        public async Task LoadPlayerInformation()
        {
            PlayerSkills = new();
            await PlayerSkills.LoadPlayerStats(PlayerName);
            PlayerQuests = new();
            await PlayerQuests.LoadPlayerQuests(PlayerName);
        }
    }
}
