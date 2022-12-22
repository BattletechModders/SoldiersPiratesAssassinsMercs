using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using BattleTech.Data;
using BattleTech.Framework;
using HBS;
using MissionControl.Data;
using MissionControl.Logic;
using UnityEngine;
using DataManager = MissionControl.DataManager;

namespace SoldiersPiratesAssassinsMercs.Framework
{
    public class Classes
    {
        public class ConfigOptions
        {
            public class OpforReplacementConfig
            {
                public float BaseReplaceChance = 0f;
                public Dictionary<string, float> FactionsReplaceOverrides = new Dictionary<string, float>(); //faction-specific values completely override base chance
                public List<string> BlacklistContractTypes = new List<string>();
                public List<string> BlacklistContractIDs = new List<string>();
                //public float MercFactionReputationFactor = 0f; // merc faction will lose rep as function of target team rep
            }
            public class MercLanceAdditionConfig // will take place of "additional lance" or MC support lances
            {
                public float BaseReplaceChance = 0f;
                public Dictionary<string, float> FactionsReplaceOverrides = new Dictionary<string, float>(); //faction-specific values completely override base chance
                public List<string> BlacklistContractTypes = new List<string>();
                public List<string> BlacklistContractIDs = new List<string>();
                public float MercFactionReputationFactor = 0f;
            }
            public class MercFactionConfig
            {
                public string MercFactionName = ""; //e,g, KellHounds or RazorbackMercs
                public int AppearanceWeight = 0; //base "weight" for selection
                //public float AppearanceWeightRepFactor = 0f; //additional "weight" as factor of times previously faced (internal counter)
                public List<string> EmployerBlacklist = new List<string>();
                public float UnitRating = 1; //higher rating means less likely to take bribe to disengage or switch sides
                public List<string> PersonalityAttributes = new List<string>();
            }

            public class AlternateOpforConfig // these are alternate factions for specific factions which are NOT mercenaries.
                                              // part of setting, where dictionary key = faction name being replaced
            {
                public float FactionReplaceChance = 0f;
                public Dictionary<string, int> AlternateOpforWeights = new Dictionary<string, int>();
            }
        }
        public class MercDialogueBucket //is value for dictionary where key = PersonalityAttributes
        {
            public List<string> Dialogue = new List<string>();
            //public List<string> BribeCriticalSuccessDialogue = new List<string>();
            public List<string> BribeSuccessDialogue = new List<string>();
            public List<string> BribeFailureDialogue = new List<string>();
            public int MinTimesEncountered = 0;
            public int MaxTimesEncountered = 0;
            public float BribeAcceptanceMultiplier = 1f;
        }
        public class AddLanceToMercTeam : LanceLogic
        {
            public string lanceGuid;
            public List<string> unitGuids;
            public MLanceOverride manuallySpecifiedLance;
            public AddLanceToMercTeam(string lanceGuid, List<string> unitGuids, MLanceOverride manuallySpecifiedLance = null)
            {
                this.lanceGuid = lanceGuid;
                this.unitGuids = unitGuids;
                this.manuallySpecifiedLance = manuallySpecifiedLance;
            }
            public override void Run(RunPayload payload)
            {
                ModInit.modLog?.Trace?.Write($"[AddLanceToMercTeam - MC OVERRIDE] Adding lance to merc team {ModState.HostileMercLanceTeamOverride.FactionValue.Name}");
                MissionControl.Main.Logger.Log("[SPAM - AddLanceToTargetTeam - SPAM] Adding lance to merc team");
                ContractOverride contractOverride = ((ContractOverridePayload)payload).ContractOverride;
                TeamOverride targetTeam = ModState.HostileMercLanceTeamOverride;
                LanceOverride lanceOverride = (this.manuallySpecifiedLance == null) ? SelectMercLanceOverride("enemy").Copy() : this.manuallySpecifiedLance.Copy();
                lanceOverride.name = $"Lance_Enemy_OpposingForce_{this.lanceGuid}";
                if (this.unitGuids.Count > 4)
                {
                    for (int i = 4; i < this.unitGuids.Count; i++)
                    {
                        UnitSpawnPointOverride item = lanceOverride.unitSpawnPointOverrideList[0].Copy();
                        lanceOverride.unitSpawnPointOverrideList.Add(item);
                        ModInit.modLog?.Trace?.Write($"[AddLanceToMercTeam - MC OVERRIDE] Added {item.unitDefId} to spawn point override list");
                    }
                }
                for (int j = 0; j < lanceOverride.unitSpawnPointOverrideList.Count; j++)
                {
                    string encounterObjectGuid = this.unitGuids[j];
                    UnitSpawnPointRef unitSpawnPointRef = new UnitSpawnPointRef();
                    unitSpawnPointRef.EncounterObjectGuid = encounterObjectGuid;
                    lanceOverride.unitSpawnPointOverrideList[j].unitSpawnPoint = unitSpawnPointRef;
                }
                lanceOverride.lanceSpawner = new LanceSpawnerRef
                {
                    EncounterObjectGuid = this.lanceGuid
                };
                lanceOverride.RunMadLibs(contractOverride.contract, targetTeam);
                targetTeam.lanceOverrideList.Add(lanceOverride);
                ModInit.modLog?.Trace?.Write($"[AddLanceToMercTeam - MC OVERRIDE] Added {lanceOverride.lanceDefId} to lanceOverrideList override list");
                var sim = UnityGameInstance.BattleTechGame.Simulation;
                ModState.HostileMercLanceTeamOverride.RunMadLibs(contractOverride.contract, sim.DataManager);
                ModState.HostileMercLanceTeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, contractOverride.finalDifficulty, sim.CurrentDate, sim.CompanyTags);

            }

            public MLanceOverride SelectMercLanceOverride(string teamType)
            {
                string biome = Enum.GetName(typeof(Biome.BIOMESKIN), MissionControl.MissionControl.Instance.CurrentContract.ContractBiome);
                biome = biome.Capitalise();
                string contractType = MissionControl.MissionControl.Instance.CurrentContractType;
                FactionDef faction = FactionDef.GetFactionDefByEnum(UnityGameInstance.BattleTechGame.DataManager, ModState.HostileMercLanceTeamOverride.FactionValue.Name);
                string factionName = (faction == null) ? "UNKNOWN" : faction.Name;
                int factionRep = (MissionControl.MissionControl.Instance.IsSkirmish()) ? 0 : UnityGameInstance.Instance.Game.Simulation.GetRawReputation(faction?.FactionValue);
                int mrbRating = (MissionControl.MissionControl.Instance.IsSkirmish()) ? 0 : UnityGameInstance.Instance.Game.Simulation.GetRawReputation(FactionEnumeration.GetMercenaryReviewBoardFactionValue());
                int mrbLevel = (MissionControl.MissionControl.Instance.IsSkirmish()) ? 0 : UnityGameInstance.Instance.Game.Simulation.GetCurrentMRBLevel();
                bool useElites = MissionControl.MissionControl.Instance.ShouldUseElites(faction, teamType);
                MissionControl.Config.Lance activeAdditionalLance = MissionControl.Main.Settings.ActiveAdditionalLances.GetActiveAdditionalLanceByTeamType(teamType);
                List<string> lancePoolKeys = MissionControl.Main.Settings.ActiveAdditionalLances.GetLancePoolKeys(teamType, biome, contractType, factionName, factionRep, mrbLevel, mrbRating);

                int index = UnityEngine.Random.Range(0, lancePoolKeys.Count);
                string selectedLanceKey = lancePoolKeys[index];
                if (useElites) selectedLanceKey = $"{selectedLanceKey}{activeAdditionalLance.EliteLances.Suffix}";

                MissionControl.Main.LogDebug($"[SPAM - SelectAppropriateLanceOverride - SPAM] Lance pool keys valid for '{teamType.Capitalise()}', '{biome}', '{contractType}', '{faction}' are '{string.Join(", ", lancePoolKeys.ToArray())}'");

                if (DataManager.Instance.DoesLanceOverrideExist(selectedLanceKey))
                {
                    MissionControl.Main.Logger.Log($"[SPAM - SelectAppropriateLanceOverride - SPAM] Selected lance key '{selectedLanceKey}'");
                    return DataManager.Instance.GetLanceOverride(selectedLanceKey);
                }
                else
                {
                    if (useElites)
                    {
                        selectedLanceKey = selectedLanceKey.Replace(activeAdditionalLance.EliteLances.Suffix, "");
                        if (DataManager.Instance.DoesLanceOverrideExist(selectedLanceKey))
                        {
                            MissionControl.Main.Logger.LogError($"[SPAM - SelectAppropriateLanceOverride - SPAM] Cannot find 'ELITE' variant of '{selectedLanceKey}' so using original version with a +4 difficulty adjustment.");
                            MLanceOverride lanceOverride = DataManager.Instance.GetLanceOverride(selectedLanceKey);
                            lanceOverride.lanceDifficultyAdjustment = Mathf.Clamp(lanceOverride.lanceDifficultyAdjustment + 4, 1, 10);
                            return lanceOverride;
                        }
                    }

                    MissionControl.Main.Logger.LogError($"[SPAM - SelectAppropriateLanceOverride - SPAM] MLanceOverride of '{selectedLanceKey}' not found. Defaulting to 'Generic_Light_Battle_Lance'");
                    return DataManager.Instance.GetLanceOverride("Generic_Light_Battle_Lance");
                }
            }

        }
    }
}
