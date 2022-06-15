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
        public static TeamDefinition HostileMercTeamDefinition = new TeamDefinition("ddfd570d-f9e4-42f8-b2e8-671eb1e8f43a", "HostileMercenaryTeam");
        public static TeamDefinition FriendlyMercTeamDefinition = new TeamDefinition("be68d8cb-6e32-401e-889e-c37cf10c0528", "FriendlyMercenaryTeam");
        public static List<string> simDisplayedFactions = new List<string>();
        public static List<string> simMercFactions = new List<string>();
        public static TeamOverride MercFactionTeamOverride = null;
        public static TeamOverride OriginalTargetFactionTeamOverride = null;

        public static bool ActiveContractShouldReplaceLanceWithMercs = false;
        public static TeamOverride HostileMercLanceTeamOverride = null;



        public static bool ActiveContractShouldSpawnAlliedMercs = false;

       public static void InitializeMercFactionList(SimGameState sim)
       {
            if (simMercFactions.Count != 0) return;
            ModState.simDisplayedFactions = new List<string>(sim.displayedFactions);
            foreach (var faction in FactionEnumeration.FactionList)
            {
                if (!faction.IsRealFaction ||
                    (!faction.IsMercenary)) continue; //used to require does gain reputation, but maybe shouldnt
                if (!ModInit.modSettings.FactionBlacklist.Contains(faction.Name))
                {
                    ModState.simMercFactions.Add(faction.Name);
                }
            }

       }

       public static void ResetStateAfterContract()
       { 
           HostileMercTeamDefinition = new TeamDefinition("ddfd570d-f9e4-42f8-b2e8-671eb1e8f43a", "HostileMercenaryTeam"); 
           FriendlyMercTeamDefinition = new TeamDefinition("be68d8cb-6e32-401e-889e-c37cf10c0528", "FriendlyMercenaryTeam"); 
           MercFactionTeamOverride = null;
           OriginalTargetFactionTeamOverride = null;
           ActiveContractShouldReplaceLanceWithMercs = false;
           ActiveContractShouldSpawnAlliedMercs = false;
           HostileMercLanceTeamOverride = null;
       }
    }
}
