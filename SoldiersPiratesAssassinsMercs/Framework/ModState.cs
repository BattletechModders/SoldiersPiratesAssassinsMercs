using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using BattleTech.Framework;
using IRBTModUtils;
using Newtonsoft.Json;

namespace SoldiersPiratesAssassinsMercs.Framework
{
    public class ModState
    {
        //these stay through play session
        public static List<string> simDisplayedFactions = new List<string>();
        public static List<string> simMercFactions = new List<string>();
        public static ConcurrentDictionary<string, List<Classes.MercDialogue>> DialogueStrings = new ConcurrentDictionary<string, List<Classes.MercDialogue>>();

        //reset these after contract
        public static TeamDefinition HostileMercTeamDefinition = new TeamDefinition("ddfd570d-f9e4-42f8-b2e8-671eb1e8f43a", "HostileMercenaryTeam");
        public static TeamDefinition FriendlyMercTeamDefinition = new TeamDefinition("be68d8cb-6e32-401e-889e-c37cf10c0528", "FriendlyMercenaryTeam");
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
                if (ModInit.modSettings.MercFactionConfigs.Any(x=>x.MercFactionName == faction.Name))
                {
                    ModState.simMercFactions.Add(faction.Name);
                }
            }
       }

       public static void InitializeDialogueStrings()
       {
           using (StreamReader reader = new StreamReader($"{ModInit.modDir}/Dialogue.json"))
           {
               string jdata = reader.ReadToEnd(); //dictionary key is "personality attribute" associated with dialogue.
               ModState.DialogueStrings = JsonConvert.DeserializeObject<ConcurrentDictionary<string, List<Classes.MercDialogue>>>(jdata);
               ModInit.modLog?.Trace?.Write($"[InitializeDialogueStrings] Initializing Dialogue");
           }
       }

       public static void ResetStateAfterContract()
       { 
           HostileMercTeamDefinition = new TeamDefinition("ddfd570d-f9e4-42f8-b2e8-671eb1e8f43a", "HostileMercenaryTeam"); 
           FriendlyMercTeamDefinition = new TeamDefinition("be68d8cb-6e32-401e-889e-c37cf10c0528", "FriendlyMercenaryTeam"); 
           MercFactionTeamOverride = null;
           OriginalTargetFactionTeamOverride = null;
           ActiveContractShouldReplaceLanceWithMercs = false;
           HostileMercLanceTeamOverride = null;
           ActiveContractShouldSpawnAlliedMercs = false;
       }
    }
}
