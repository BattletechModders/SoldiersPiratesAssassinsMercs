using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using BattleTech.Framework;

namespace SoldiersPiratesAssassinsMercs.Framework
{
    public class ModState
    {
        public static List<string> simDisplayedFactions = new List<string>();
        public static List<string> simMercFactions = new List<string>();
        public static TeamOverride MercFactionOverride = null;
        public static TeamOverride OriginalTargetFactionOverride = null;

        public static bool ActiveContractShouldReplaceLanceWithMercs = false;
        public static TeamOverride HostileMercLanceTeamOverride = null;



        public static bool ActiveContractShouldSpawnAlliedMercs = false;

        public static GameContext OriginalGameContext
        {
            get;
            set; 
        }

        public static void InitializeMercFactionList(SimGameState sim)
        {
            if (simMercFactions.Count != 0) return;
            ModState.simDisplayedFactions = new List<string>(sim.displayedFactions);
            foreach (var faction in FactionEnumeration.FactionList)
            {
                if (!faction.IsRealFaction ||
                    (!faction.IsMercenary)) continue;
                if (!ModInit.modSettings.FactionBlacklist.Contains(faction.Name))
                {
                    ModState.simMercFactions.Add(faction.Name);
                }
            }
        }

        public static void ResetStateAfterContract()
        {
            MercFactionOverride = null;
            OriginalTargetFactionOverride = null;
            OriginalGameContext = null;
            ActiveContractShouldReplaceLanceWithMercs = false;
            ActiveContractShouldSpawnAlliedMercs = false;
            HostileMercLanceTeamOverride = null;
        }
    }
}
