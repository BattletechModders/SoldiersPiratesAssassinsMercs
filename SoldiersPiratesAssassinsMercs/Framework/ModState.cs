using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BattleTech;
using BattleTech.Data;
using BattleTech.Framework;
using Newtonsoft.Json;
using UnityEngine;
using static SoldiersPiratesAssassinsMercs.Framework.Classes;

namespace SoldiersPiratesAssassinsMercs.Framework
{ 
    public class ModState
    {
        public static HashSet<string> BattleRoyaleEmblems = new HashSet<string>();
        //these stay through play session
        public static Dictionary<string, List<string>> UniversalFactionFallbackMap = new Dictionary<string, List<string>>();
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
        public static TeamOverride OriginalTargetFactionTeamOverride = null;
        public static SPAMTeamOverride MercFactionTeamOverride = new SPAMTeamOverride();
        public static SPAMTeamOverride AltFactionFactionTeamOverride = new SPAMTeamOverride();
        public static SPAMTeamOverride PlanetAltFactionTeamOverride = new SPAMTeamOverride();
        //public static bool ActiveContractShouldReplaceLanceWithMercs = false;
        public static SPAMTeamOverride HostileMercLanceTeamOverride = new SPAMTeamOverride();
        public static SPAMTeamOverride HostileAltLanceTeamOverride = new SPAMTeamOverride();
        public static SPAMTeamOverride HostileToAllLanceTeamOverride = new SPAMTeamOverride();
        public static bool ActiveContractShouldSpawnAlliedMercs = false;
        public static Classes.MercDialogueBucket ChosenDialogue = new Classes.MercDialogueBucket();

        public static int RoundsInCombat = 0;
        public static bool HasBribeBeenAttempted = false;

        public static int BribeSuccess = 0;
        //public static bool QueueBribePopup = false;
        public static List<Vector3> UnitSpawnPointLocs = new List<Vector3>();

        public static void BuildEmblems(DataManager dm)
        {
            var manifestList = new List<VersionManifestEntry>();
            manifestList.AddRange(dm.Unlocks.GetHeraldryEntries());
            VersionManifestAddendum addendumByName = dm.ResourceLocator.GetAddendumByName("PlayerEmblems");
            manifestList.AddRange(dm.ResourceLocator.AllEntriesOfResourceFromAddendum(BattleTechResourceType.Sprite, addendumByName, false));
            foreach (var item in manifestList)
            {
                ModState.BattleRoyaleEmblems.Add(item.Id);
            }
            foreach (var heraldry in dm.Heraldries)
            {
                ModState.BattleRoyaleEmblems.Add(heraldry.Value.textureLogoID);
            } 
        }

        public static void BuildFallbackMap()
        {
            var outputDictionary = new Dictionary<string, List<string>>();
            foreach (var altConfig in ModInit.modSettings.AlternateFactionConfigs)
            {
                foreach (var altFactions in altConfig.Value.AlternateOpforWeights)
                {
                    if (!outputDictionary.ContainsKey(altFactions.FactionName.ToLower()))
                    {
                        outputDictionary.Add(altFactions.FactionName.ToLower(), new List<string>{altFactions.FactionFallback.ToLower() });
                    }
                    else if (!outputDictionary[altFactions.FactionName.ToLower()].Contains(altFactions.FactionFallback.ToLower()))
                    {
                        outputDictionary[altFactions.FactionName.ToLower()].Add(altFactions.FactionFallback.ToLower());
                    }
                }
            }
            foreach (var mercConfig in ModInit.modSettings.MercFactionConfigs)
            {
                if (!outputDictionary.ContainsKey(mercConfig.Key.ToLower()))
                {
                    outputDictionary.Add(mercConfig.Key.ToLower(), new List<string>{ mercConfig.Value.MercFactionFallbackTag.ToLower() });
                }
                else if (!outputDictionary[mercConfig.Key.ToLower()].Contains(mercConfig.Value.MercFactionFallbackTag.ToLower()))
                {
                    outputDictionary[mercConfig.Key.ToLower()].Add(mercConfig.Value.MercFactionFallbackTag.ToLower());
                }
            }

            foreach (var planetConfig in ModInit.modSettings.PlanetFactionConfigs)
            {
                foreach (var planetFactions in planetConfig.Value.AlternateOpforWeights)
                {
                    if (!outputDictionary.ContainsKey(planetFactions.FactionName.ToLower()))
                    {
                        outputDictionary.Add(planetFactions.FactionName.ToLower(), new List<string>{ planetFactions.FactionFallback.ToLower() });
                    }
                    else if (!outputDictionary[planetFactions.FactionName.ToLower()].Contains(planetFactions.FactionFallback.ToLower()))
                    {
                        outputDictionary[planetFactions.FactionName.ToLower()].Add(planetFactions.FactionFallback.ToLower());
                    }
                }
            }
            ModState.UniversalFactionFallbackMap = outputDictionary;
            ModInit.modLog?.Info?.Write($"Built Universal Fallback Map: {JsonConvert.SerializeObject(outputDictionary, Formatting.Indented)}");
        }
        public static void GenerateFactionMap()
        {
            var outputDictionary = new Dictionary<string, List<string>>();
            foreach (var altConfig in ModInit.modSettings.AlternateFactionConfigs)
            {
                if (!outputDictionary.ContainsKey(altConfig.Key))
                {
                    outputDictionary.Add(altConfig.Key, new List<string>());
                }
                foreach (var altFaction in altConfig.Value.AlternateOpforWeights)
                {
                    if (!outputDictionary[altConfig.Key].Contains(altFaction.FactionName))
                    {
                        outputDictionary[altConfig.Key].Add(altFaction.FactionName);
                    }
                }
            }

            foreach (var mercConfig in ModInit.modSettings.MercFactionConfigs)
            {
                if (!outputDictionary.ContainsKey(mercConfig.Value.MercFactionFallbackTag))
                {
                    outputDictionary.Add(mercConfig.Value.MercFactionFallbackTag, new List<string>());
                }

                if (!outputDictionary[mercConfig.Value.MercFactionFallbackTag].Contains(mercConfig.Key))
                {
                    outputDictionary[mercConfig.Value.MercFactionFallbackTag].Add(mercConfig.Key);
                }
            }

            foreach (var planetConfig in ModInit.modSettings.PlanetFactionConfigs)
            {
                foreach (var factionConfig in planetConfig.Value.AlternateOpforWeights)
                {
                    if (!outputDictionary.ContainsKey(factionConfig.FactionFallback))
                    {
                        outputDictionary.Add(factionConfig.FactionFallback, new List<string>());
                    }

                    if (!outputDictionary[factionConfig.FactionFallback].Contains(factionConfig.FactionName))
                    {
                        outputDictionary[factionConfig.FactionFallback].Add(factionConfig.FactionName);
                    }
                }
            }

            string path = Path.Combine(ModInit.modDir, "subFaction.json");

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(JsonConvert.SerializeObject(outputDictionary, Formatting.Indented));
                writer.Flush();
            }
        }

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
            MercFactionTeamOverride = new SPAMTeamOverride();
            AltFactionFactionTeamOverride = new SPAMTeamOverride();
            PlanetAltFactionTeamOverride = new SPAMTeamOverride();
            OriginalTargetFactionTeamOverride = null;
            //ActiveContractShouldReplaceLanceWithMercs = false;
            HostileMercLanceTeamOverride = new SPAMTeamOverride();
            HostileToAllLanceTeamOverride = new SPAMTeamOverride();
            HostileAltLanceTeamOverride = new SPAMTeamOverride();
            ActiveContractShouldSpawnAlliedMercs = false;
            ChosenDialogue = new Classes.MercDialogueBucket();
            RoundsInCombat = 0;
            HasBribeBeenAttempted = false;
            BribeSuccess = 0;
            UnitSpawnPointLocs = new List<Vector3>();
            //QueueBribePopup = false;
        }
    }
}
