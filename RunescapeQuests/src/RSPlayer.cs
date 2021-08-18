using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeQuests.src
{
    public sealed class RSPlayer
    {
        private static readonly Lazy<RSPlayer> lazyPlayer = new Lazy<RSPlayer>(() => new RSPlayer());
        public static RSPlayer Instance { get { return lazyPlayer.Value; } }
        public RSPlayerSkills PlayerSkills { get; private set; }
        public RSPlayerQuests PlayerQuests { get; private set; }
        private string PlayerName;

        private RSPlayer()
        {
        }

        public void SetPlayerName(string PlayerName)
        {
            this.PlayerName = PlayerName;
        }
        public async Task LoadPlayerInformation()
        {
            IsValidPlayer = false;
            if (String.IsNullOrEmpty(PlayerName))
                return;
            PlayerSkills = new();
            await PlayerSkills.LoadPlayerStats(PlayerName);
            if (PlayerSkills.ValidPlayerStats == false)
                return;
            PlayerQuests = new();
            await PlayerQuests.LoadPlayerQuests(PlayerName);
            if (PlayerQuests.ValidPlayerQuests == false)
                return;
            IsValidPlayer = true;
        }
        public bool IsValidPlayer { get; private set; }
    }
}
