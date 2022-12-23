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
        public static ConcurrentDictionary<string, List<Classes.MercDialogueBucket>> MercDialogueStrings = new ConcurrentDictionary<string, List<Classes.MercDialogueBucket>>();

        public static ConcurrentDictionary<string, List<string>> GenericDialogueStrings =
            new ConcurrentDictionary<string, List<string>>();

        //reset these after contract
        public static TeamDefinition HostileMercLanceTeamDefinition = new TeamDefinition(GlobalVars.HostileMercLanceTeamDefinitionGUID, "HostileMercenaryTeam");
        public static TeamDefinition HostileAltLanceTeamDefinition = new TeamDefinition(GlobalVars.HostileAltLanceTeamDefinitionGUID, "HostileAltFactionTeam");
        public static TeamDefinition FriendlyMercLanceTeamDefinition = new TeamDefinition(GlobalVars.FriendlyMercLanceTeamDefinitionGUID, "FriendlyMercenaryTeam");
        public static TeamDefinition HostileToAllLanceTeamDefinition = new TeamDefinition(GlobalVars.HostileToAllLanceTeamDefinitionGUID, "HostileToAllPlanetTeam");
        public static TeamOverride MercFactionTeamOverride = null;
        public static TeamOverride AltFactionFactionTeamOverride = null;
        public static TeamOverride PlanetAltFactionTeamOverride = null;
        public static TeamOverride OriginalTargetFactionTeamOverride = null;
        //public static bool ActiveContractShouldReplaceLanceWithMercs = false;
        public static TeamOverride HostileMercLanceTeamOverride = null;
        public static TeamOverride HostileAltLanceTeamOverride = null;
        public static TeamOverride HostileToAllLanceTeamOverride = null;
        public static bool ActiveContractShouldSpawnAlliedMercs = false;
        public static Classes.MercDialogueBucket ChosenDialogue = new Classes.MercDialogueBucket();

        public static int RoundsInCombat = 0;
        public static bool HasBribeBeenAttempted = false;

        public static int BribeSuccess = 0;
        //public static bool QueueBribePopup = false;

       public static void InitializeMercFactionList(SimGameState sim)
       {
            if (simMercFactions.Count != 0) return;
            ModState.simDisplayedFactions = new List<string>(sim.displayedFactions);
            foreach (var faction in FactionEnumeration.FactionList)
            {
                if (!faction.IsRealFaction ||
                    (!faction.IsMercenary)) continue; //used to require does gain reputation, but maybe shouldnt
                if (ModInit.modSettings.MercFactionConfigs.ContainsKey(faction.Name))
                {
                    ModState.simMercFactions.Add(faction.Name);
                }
            }
       }

       public static void InitializeDialogueStrings()
       {
           using (StreamReader reader = new StreamReader($"{ModInit.modDir}/MercDialogue.json"))
           {
               string jdata = reader.ReadToEnd(); //dictionary key is "personality attribute" associated with Dialogue.
               ModState.MercDialogueStrings = JsonConvert.DeserializeObject<ConcurrentDictionary<string, List<Classes.MercDialogueBucket>>>(jdata);
               ModInit.modLog?.Trace?.Write($"[InitializeDialogueStrings] Initializing Merc Dialogue");
           }
           foreach (var stringconfig in ModState.MercDialogueStrings)
           {
               ModInit.modLog?.Trace?.Write($"[InitializeDialogueStrings] Initialized Merc Dialogue for attribute {stringconfig.Key}");
               foreach (var config in stringconfig.Value)
               {
                   ModInit.modLog?.Trace?.Write($"[InitializeDialogueStrings] Initialized Merc Dialogue {string.Join("; ", config.Dialogue)}");
                }
           }

           using (StreamReader reader = new StreamReader($"{ModInit.modDir}/GenericDialogue.json"))
           {
               string jdata = reader.ReadToEnd(); //dictionary key is alternate faction (match key from AlternateFactionConfigs keys) associated with Dialogue.
                ModState.GenericDialogueStrings = JsonConvert.DeserializeObject<ConcurrentDictionary<string, List<string>>>(jdata);
               ModInit.modLog?.Trace?.Write($"[InitializeDialogueStrings] Initializing other faction Dialogue");
           }
           foreach (var stringconfig in ModState.GenericDialogueStrings)
           {
               ModInit.modLog?.Trace?.Write($"[InitializeDialogueStrings] Initialized other Dialogue for attribute {stringconfig.Key}");
               foreach (var config in stringconfig.Value)
               {
                   ModInit.modLog?.Trace?.Write($"[InitializeDialogueStrings] Initialized other Dialogue {string.Join("; ", config)}");
               }
           }
       }

       public static void ResetStateAfterContract()
       { 
           HostileMercLanceTeamDefinition = new TeamDefinition(GlobalVars.HostileMercLanceTeamDefinitionGUID, "HostileMercenaryTeam"); 
           FriendlyMercLanceTeamDefinition = new TeamDefinition(GlobalVars.FriendlyMercLanceTeamDefinitionGUID, "FriendlyMercenaryTeam"); 
           HostileToAllLanceTeamDefinition = new TeamDefinition(GlobalVars.HostileToAllLanceTeamDefinitionGUID, "HostilePlanetTeam");
            MercFactionTeamOverride = null;
           AltFactionFactionTeamOverride = null;
           PlanetAltFactionTeamOverride = null;
           OriginalTargetFactionTeamOverride = null;
           //ActiveContractShouldReplaceLanceWithMercs = false;
           HostileMercLanceTeamOverride = null;
           HostileToAllLanceTeamOverride = null;
           HostileAltLanceTeamOverride = null;
           ActiveContractShouldSpawnAlliedMercs = false;
           ChosenDialogue = new Classes.MercDialogueBucket();
           RoundsInCombat = 0;
           HasBribeBeenAttempted = false;
           BribeSuccess = 0;
           //QueueBribePopup = false;
       }
    }
}
