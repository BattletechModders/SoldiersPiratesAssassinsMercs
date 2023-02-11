using BattleTech.Data;
using BattleTech.Framework;
using BattleTech;
using MissionControl.Data;
using MissionControl.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;
using DataManager = MissionControl.DataManager;

namespace SoldiersPiratesAssassinsMercs.Framework
{
    internal class MissionControlClasses
    {
        public class AddLanceToAltTeam : LanceLogic //override objective text somewhere?  AddTargetLanceWithDestroyObjectiveBatch maybe?
        {
            public string lanceGuid;
            public List<string> unitGuids;
            public MLanceOverride manuallySpecifiedLance;
            public AddLanceToAltTeam(string lanceGuid, List<string> unitGuids, MLanceOverride manuallySpecifiedLance = null)
            {
                this.lanceGuid = lanceGuid;
                this.unitGuids = unitGuids;
                this.manuallySpecifiedLance = manuallySpecifiedLance;
            }
            public override void Run(RunPayload payload)
            {
                ModInit.modLog?.Trace?.Write($"[AddLanceToAltTeam - MC OVERRIDE] Adding lance to alt team {ModState.HostileAltLanceTeamOverride.TeamOverride.FactionValue.Name}");
                MissionControl.Main.Logger.Log("[SPAM - AddLanceToAltTeam - SPAM] Adding lance to alt team");
                ContractOverride contractOverride = ((ContractOverridePayload)payload).ContractOverride;
                TeamOverride targetTeam = ModState.HostileAltLanceTeamOverride.TeamOverride;
                LanceOverride lanceOverride = (this.manuallySpecifiedLance == null) ? SelectAltLanceOverride("enemy").Copy() : this.manuallySpecifiedLance.Copy();
                lanceOverride.name = $"Lance_Enemy_OpposingForce_{this.lanceGuid}";
                if (this.unitGuids.Count > 4)
                {
                    for (int i = 4; i < this.unitGuids.Count; i++)
                    {
                        UnitSpawnPointOverride item = lanceOverride.unitSpawnPointOverrideList[0].Copy();
                        lanceOverride.unitSpawnPointOverrideList.Add(item);
                        ModInit.modLog?.Trace?.Write($"[AddLanceToAltTeam - MC OVERRIDE] Added {item.unitDefId} to spawn point override list");
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
                ModInit.modLog?.Trace?.Write($"[AddLanceToAltTeam - MC OVERRIDE] Added {lanceOverride.lanceDefId} to lanceOverrideList override list");
                var sim = UnityGameInstance.BattleTechGame.Simulation;
                ModState.HostileAltLanceTeamOverride.TeamOverride.RunMadLibs(contractOverride.contract, sim.DataManager);
                ModState.HostileAltLanceTeamOverride.TeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, contractOverride.finalDifficulty, sim.CurrentDate, sim.CompanyTags);

            }

            public MLanceOverride SelectAltLanceOverride(string teamType)
            {
                string biome = Enum.GetName(typeof(Biome.BIOMESKIN), MissionControl.MissionControl.Instance.CurrentContract.ContractBiome);
                biome = biome.Capitalise();
                string contractType = MissionControl.MissionControl.Instance.CurrentContractType;
                FactionDef faction = FactionDef.GetFactionDefByEnum(UnityGameInstance.BattleTechGame.DataManager, ModState.HostileAltLanceTeamOverride.TeamOverride.FactionValue.Name);
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

                MissionControl.Main.LogDebug($"[SPAM - SelectAltLanceOverride - SPAM] Lance pool keys valid for '{teamType.Capitalise()}', '{biome}', '{contractType}', '{faction}' are '{string.Join(", ", lancePoolKeys.ToArray())}'");

                if (DataManager.Instance.DoesLanceOverrideExist(selectedLanceKey))
                {
                    MissionControl.Main.Logger.Log($"[SPAM - SelectAltLanceOverride - SPAM] Selected lance key '{selectedLanceKey}'");
                    return DataManager.Instance.GetLanceOverride(selectedLanceKey);
                }
                else
                {
                    if (useElites)
                    {
                        selectedLanceKey = selectedLanceKey.Replace(activeAdditionalLance.EliteLances.Suffix, "");
                        if (DataManager.Instance.DoesLanceOverrideExist(selectedLanceKey))
                        {
                            MissionControl.Main.Logger.LogError($"[SPAM - SelectAltLanceOverride - SPAM] Cannot find 'ELITE' variant of '{selectedLanceKey}' so using original version with a +4 difficulty adjustment.");
                            MLanceOverride lanceOverride = DataManager.Instance.GetLanceOverride(selectedLanceKey);
                            lanceOverride.lanceDifficultyAdjustment = Mathf.Clamp(lanceOverride.lanceDifficultyAdjustment + 4, 1, 10);
                            return lanceOverride;
                        }
                    }

                    MissionControl.Main.Logger.LogError($"[SPAM - SelectAltLanceOverride - SPAM] MLanceOverride of '{selectedLanceKey}' not found. Defaulting to 'Generic_Light_Battle_Lance'");
                    return DataManager.Instance.GetLanceOverride("Generic_Light_Battle_Lance");
                }
            }

        }
        public class AddLanceToHostileToAllTeam : LanceLogic
        {
            public string lanceGuid;
            public List<string> unitGuids;
            public MLanceOverride manuallySpecifiedLance;
            public AddLanceToHostileToAllTeam(string lanceGuid, List<string> unitGuids, MLanceOverride manuallySpecifiedLance = null)
            {
                this.lanceGuid = lanceGuid;
                this.unitGuids = unitGuids;
                this.manuallySpecifiedLance = manuallySpecifiedLance;
            }
            public override void Run(RunPayload payload)
            {
                ModInit.modLog?.Trace?.Write($"[AddLanceToAltTeam - MC OVERRIDE] Adding lance to hostile to all team {ModState.HostileToAllLanceTeamOverride.TeamOverride.FactionValue.Name}");
                MissionControl.Main.Logger.Log("[SPAM - AddLanceToAltTeam - SPAM] Adding lance to hostile to all team");
                ContractOverride contractOverride = ((ContractOverridePayload)payload).ContractOverride;
                TeamOverride targetTeam = ModState.HostileToAllLanceTeamOverride.TeamOverride;
                LanceOverride lanceOverride = (this.manuallySpecifiedLance == null) ? SelectHostileToAllLanceOverride("enemy").Copy() : this.manuallySpecifiedLance.Copy();
                lanceOverride.name = $"Lance_Enemy_OpposingForce_{this.lanceGuid}";
                if (this.unitGuids.Count > 4)
                {
                    for (int i = 4; i < this.unitGuids.Count; i++)
                    {
                        UnitSpawnPointOverride item = lanceOverride.unitSpawnPointOverrideList[0].Copy();
                        lanceOverride.unitSpawnPointOverrideList.Add(item);
                        ModInit.modLog?.Trace?.Write($"[AddLanceToAltTeam - MC OVERRIDE] Added {item.unitDefId} to spawn point override list");
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
                ModInit.modLog?.Trace?.Write($"[AddLanceToAltTeam - MC OVERRIDE] Added {lanceOverride.lanceDefId} to lanceOverrideList override list");
                var sim = UnityGameInstance.BattleTechGame.Simulation;
                ModState.HostileToAllLanceTeamOverride.TeamOverride.RunMadLibs(contractOverride.contract, sim.DataManager);
                ModState.HostileToAllLanceTeamOverride.TeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, contractOverride.finalDifficulty, sim.CurrentDate, sim.CompanyTags);
            }

            public MLanceOverride SelectHostileToAllLanceOverride(string teamType)
            {
                string biome = Enum.GetName(typeof(Biome.BIOMESKIN), MissionControl.MissionControl.Instance.CurrentContract.ContractBiome);
                biome = biome.Capitalise();
                string contractType = MissionControl.MissionControl.Instance.CurrentContractType;
                FactionDef faction = FactionDef.GetFactionDefByEnum(UnityGameInstance.BattleTechGame.DataManager, ModState.HostileToAllLanceTeamOverride.TeamOverride.FactionValue.Name);
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

                MissionControl.Main.LogDebug($"[SPAM - SelectAltLanceOverride - SPAM] Lance pool keys valid for '{teamType.Capitalise()}', '{biome}', '{contractType}', '{faction}' are '{string.Join(", ", lancePoolKeys.ToArray())}'");

                if (DataManager.Instance.DoesLanceOverrideExist(selectedLanceKey))
                {
                    MissionControl.Main.Logger.Log($"[SPAM - SelectAltLanceOverride - SPAM] Selected lance key '{selectedLanceKey}'");
                    return DataManager.Instance.GetLanceOverride(selectedLanceKey);
                }
                else
                {
                    if (useElites)
                    {
                        selectedLanceKey = selectedLanceKey.Replace(activeAdditionalLance.EliteLances.Suffix, "");
                        if (DataManager.Instance.DoesLanceOverrideExist(selectedLanceKey))
                        {
                            MissionControl.Main.Logger.LogError($"[SPAM - SelectAltLanceOverride - SPAM] Cannot find 'ELITE' variant of '{selectedLanceKey}' so using original version with a +4 difficulty adjustment.");
                            MLanceOverride lanceOverride = DataManager.Instance.GetLanceOverride(selectedLanceKey);
                            lanceOverride.lanceDifficultyAdjustment = Mathf.Clamp(lanceOverride.lanceDifficultyAdjustment + 4, 1, 10);
                            return lanceOverride;
                        }
                    }

                    MissionControl.Main.Logger.LogError($"[SPAM - SelectAltLanceOverride - SPAM] MLanceOverride of '{selectedLanceKey}' not found. Defaulting to 'Generic_Light_Battle_Lance'");
                    return DataManager.Instance.GetLanceOverride("Generic_Light_Battle_Lance");
                }
            }

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
                ModInit.modLog?.Trace?.Write($"[AddLanceToMercTeam - MC OVERRIDE] Adding lance to merc team {ModState.HostileMercLanceTeamOverride.TeamOverride.FactionValue.Name}");
                MissionControl.Main.Logger.Log("[SPAM - AddLanceToTargetTeam - SPAM] Adding lance to merc team");
                ContractOverride contractOverride = ((ContractOverridePayload)payload).ContractOverride;
                TeamOverride targetTeam = ModState.HostileMercLanceTeamOverride.TeamOverride;
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
                ModState.HostileMercLanceTeamOverride.TeamOverride.RunMadLibs(contractOverride.contract, sim.DataManager);
                ModState.HostileMercLanceTeamOverride.TeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, contractOverride.finalDifficulty, sim.CurrentDate, sim.CompanyTags);

            }

            public MLanceOverride SelectMercLanceOverride(string teamType)
            {
                string biome = Enum.GetName(typeof(Biome.BIOMESKIN), MissionControl.MissionControl.Instance.CurrentContract.ContractBiome);
                biome = biome.Capitalise();
                string contractType = MissionControl.MissionControl.Instance.CurrentContractType;
                FactionDef faction = FactionDef.GetFactionDefByEnum(UnityGameInstance.BattleTechGame.DataManager, ModState.HostileMercLanceTeamOverride.TeamOverride.FactionValue.Name);
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
