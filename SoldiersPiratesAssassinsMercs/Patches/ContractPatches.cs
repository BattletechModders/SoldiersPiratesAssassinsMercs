using System;
using System.Collections.Generic;
using System.Linq;
using BattleTech;
using BattleTech.Data;
using BattleTech.Framework;
using BattleTech.UI;
using Harmony;
using HBS.Collections;
using SoldiersPiratesAssassinsMercs.Framework;
using UnityEngine;
using Hostility = BattleTech.Hostility;
using ModState = SoldiersPiratesAssassinsMercs.Framework.ModState;

namespace SoldiersPiratesAssassinsMercs.Patches
{
    public class ContractPatches
    {
        [HarmonyPatch(typeof(HostilityMatrix), MethodType.Constructor, new Type[]{typeof(CombatGameState), typeof(EncounterPlayStyle)})]
        public static class HostilityMatrix_guidToIndexDictionary
        {
            public static void Postfix(HostilityMatrix __instance, CombatGameState combatGameState, EncounterPlayStyle encounterPlayStyle)
            {
                __instance.guidToIndexDictionary.Add(GlobalVars.HostileMercLanceTeamDefinitionGUID, 11); //hostile mercs1
                __instance.guidToIndexDictionary.Add(GlobalVars.HostileAltLanceTeamDefinitionGUID, 12); //hostile alt
                __instance.guidToIndexDictionary.Add(GlobalVars.HostileToAllLanceTeamDefinitionGUID, 13); //hostile to all
                if (encounterPlayStyle == EncounterPlayStyle.SinglePlayer)
                {
                    __instance._matrix = new Hostility[,]
                {
                    {
                        Hostility.FRIENDLY,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.FRIENDLY,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL
                    },
                    {
                        Hostility.NEUTRAL,
                        Hostility.FRIENDLY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY
                    },
                    {
                        Hostility.NEUTRAL,
                        Hostility.FRIENDLY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY
                    },
                    {
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY
                    },
                    {
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY
                    },
                    {
                        Hostility.NEUTRAL,
                        Hostility.FRIENDLY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY
                    },
                    {
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.FRIENDLY,
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY
                    },
                    {
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.FRIENDLY,
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY
                    },
                    {
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL,
                        Hostility.NEUTRAL
                    },
                    {
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY
                    },
                    {
                        Hostility.NEUTRAL,
                        Hostility.FRIENDLY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY
                    },
                    {
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.FRIENDLY,
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY
                    },
                    {
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.FRIENDLY,
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY
                    },
                    {
                        Hostility.NEUTRAL,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.ENEMY,
                        Hostility.FRIENDLY
                    }
                };
                }
            }
        }

        [HarmonyPatch(typeof(Contract), "ShortDescription", MethodType.Getter)]
        public static class Contract_ShortDescription_Getter
        {
            public static void Prefix(Contract __instance)
            {
                if (ModState.OriginalTargetFactionTeamOverride != null)
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.OriginalTargetFactionTeamOverride);
            }

            public static void Postfix(Contract __instance)
            {
                if (ModState.PlanetAltFactionTeamOverride.TeamOverride != null)
                {
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.PlanetAltFactionTeamOverride.TeamOverride);
                }
                else if (ModState.AltFactionFactionTeamOverride.TeamOverride != null)
                {
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.AltFactionFactionTeamOverride.TeamOverride);
                }
                else if (ModState.MercFactionTeamOverride.TeamOverride != null)
                {
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.MercFactionTeamOverride.TeamOverride);
                }
            }
        }

        [HarmonyPatch(typeof(Contract), "LongDescription", MethodType.Getter)]
        public static class Contract_LongDescription_Getter
        {
            public static void Prefix(Contract __instance)
            {
                if (ModState.OriginalTargetFactionTeamOverride != null)
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.OriginalTargetFactionTeamOverride);
            }

            public static void Postfix(Contract __instance)
            {
                if (ModState.PlanetAltFactionTeamOverride.TeamOverride != null)
                {
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.PlanetAltFactionTeamOverride.TeamOverride);
                }
                else if (ModState.AltFactionFactionTeamOverride.TeamOverride != null)
                {
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.AltFactionFactionTeamOverride.TeamOverride);
                }
                else if (ModState.MercFactionTeamOverride.TeamOverride != null)
                {
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.MercFactionTeamOverride.TeamOverride);
                }
            }
        }

        [HarmonyPatch(typeof(Contract), "BeginRequestResources",
            new Type[] {typeof(bool)})]
        public static class Contract_BeginRequestResources
        {
            public static void Prefix(Contract __instance, bool generateUnits)
            {
                var sim = UnityGameInstance.BattleTechGame.Simulation;
                if (sim == null) return;
                if (!__instance.Accepted) return;

                if (ModInit.modSettings.BlacklistedContractTypesAndIDs.Contains(__instance.Override
                        .ContractTypeValue.Name) || ModInit.modSettings.BlacklistedContractTypesAndIDs.Contains(__instance.Override.ID))
                {
                    ModInit.modLog?.Trace?.Write($"[ShouldAddMercLance] {__instance.Override.ID} is blacklisted, either by type or ID, not replacing");
                    return;
                }

                if (Utils.ShouldReplaceOpforWithPlanetAlternate(__instance.Override, out var configPlanet))
                {
                    var planetFaction = Utils.GetAlternateFactionPoolFromWeight(sim, configPlanet, out var fallback);
                    ModState.OriginalTargetFactionTeamOverride = __instance.Override.targetTeam.Copy();

                    var contractFactionIDs = __instance.teamFactionIDs;
                    if (contractFactionIDs.ContainsKey(__instance.Override.targetTeam.teamGuid))
                    {
                        contractFactionIDs[__instance.Override.targetTeam.teamGuid] = planetFaction;
                        //contractFactionIDsT.SetValue(contractFactionIDs);
                    }

                    __instance.Override.targetTeam.ReAssignHostileToAllFactionToTeam(contractFactionIDs, sim.DataManager);
                    //ModState.PlanetAltFactionTeamOverride = new Tuple<TeamOverride, string>(__instance.Override.targetTeam, configPlanet.AlternateFactionFallbackTag);
                    ModState.PlanetAltFactionTeamOverride.TeamOverride = __instance.Override.targetTeam;
                    ModState.PlanetAltFactionTeamOverride.TeamOverrideFallback = fallback;
                    if (ModInit.modSettings.enableTrace)
                    {
                        var debugIDS = "";
                        foreach (var kvp in __instance.TeamFactionIDs)
                        {
                            debugIDS += $"{kvp.Key}: {FactionEnumeration.GetFactionByID(kvp.Value).Name}\n";
                        }
                        ModInit.modLog?.Trace?.Write($"[Contract_BeginRequestResources] contractFactionIDs are now {debugIDS}\n");
                    }

                    ModInit.modLog?.Info?.Write(
                        $"[Contract_BeginRequestResources] Set ModState.AltPlanetFactionTeamOverride to {ModState.PlanetAltFactionTeamOverride?.TeamOverride.faction}, original was {ModState.OriginalTargetFactionTeamOverride?.faction}. TargetTeam is now {__instance.Override.targetTeam.FactionValue.Name}. Reinitializing MissionControl");
                    __instance.Override.RunMadLibs(sim.DataManager);
                    global::MissionControl.MissionControl.Instance.SetContract(__instance);
                    return;
                }
                else if (Utils.ShouldReplaceOpforWithFactionAlternate(__instance.Override, out var configAlt))
                {
                    var altFaction = Utils.GetAlternateFactionPoolFromWeight(sim, configAlt, out var fallback);
                    ModState.OriginalTargetFactionTeamOverride = __instance.Override.targetTeam.Copy();


                    var contractFactionIDs = __instance.teamFactionIDs;
                    if (contractFactionIDs.ContainsKey(__instance.Override.targetTeam.teamGuid))
                    {
                        contractFactionIDs[__instance.Override.targetTeam.teamGuid] = altFaction;
                        //contractFactionIDsT.SetValue(contractFactionIDs);
                    }

                    __instance.Override.targetTeam.ReAssignHostileAltFactionToTeam(contractFactionIDs, sim.DataManager);
                    //ModState.AltFactionFactionTeamOverride = new Tuple<TeamOverride, string>(__instance.Override.targetTeam, configAlt.AlternateFactionFallbackTag);
                    ModState.AltFactionFactionTeamOverride.TeamOverride = __instance.Override.targetTeam;
                    ModState.AltFactionFactionTeamOverride.TeamOverrideFallback = fallback;
                    if (ModInit.modSettings.enableTrace)
                    {
                        var debugIDS = "";
                        foreach (var kvp in __instance.TeamFactionIDs)
                        {
                            debugIDS += $"{kvp.Key}: {FactionEnumeration.GetFactionByID(kvp.Value).Name}\n";
                        }
                        ModInit.modLog?.Trace?.Write($"[Contract_BeginRequestResources] contractFactionIDs are now {debugIDS}\n");
                    }

                    ModInit.modLog?.Info?.Write(
                        $"[Contract_BeginRequestResources] Set ModState.AltFactionFactionTeamOverride to {ModState.AltFactionFactionTeamOverride?.TeamOverride.faction}, original was {ModState.OriginalTargetFactionTeamOverride?.faction}. TargetTeam is now {__instance.Override.targetTeam.FactionValue.Name}. Reinitializing MissionControl");
                    __instance.Override.RunMadLibs(sim.DataManager);
                    global::MissionControl.MissionControl.Instance.SetContract(__instance);
                    return;
                }
                else if (Utils.ShouldReplaceOpforWithMercs(__instance.Override))
                {
                    var mercFaction = Utils.GetMercFactionPoolFromWeight(sim, __instance.Override.targetTeam.faction, out var fallbackTag);
                    if (mercFaction == -1)
                    {
                        ModInit.modLog?.Error?.Write($"[Contract_BeginRequestResources] Selected MercFaction [-1], aborting. No valid merc factions for employer, probably.");
                        return;
                    }
                    ModState.OriginalTargetFactionTeamOverride = __instance.Override.targetTeam.Copy();
                    //var contractFactionIDsT = Traverse.Create(__instance).Field("teamFactionIDs");
                    //var contractFactionIDs = new Dictionary<string, int>(contractFactionIDsT.GetValue<Dictionary<string, int>>());
                    var contractFactionIDs = __instance.teamFactionIDs;
                    if (contractFactionIDs.ContainsKey(__instance.Override.targetTeam.teamGuid))
                    {
                        contractFactionIDs[__instance.Override.targetTeam.teamGuid] = mercFaction;
                        //contractFactionIDsT.SetValue(contractFactionIDs);
                    }

                    __instance.Override.targetTeam.ReAssignHostileMercFactionToTeam(contractFactionIDs, sim.DataManager);
                    //ModState.MercFactionTeamOverride = new Tuple<TeamOverride, string>(__instance.Override.targetTeam, fallbackTag);
                    ModState.MercFactionTeamOverride.TeamOverride = __instance.Override.targetTeam;
                    ModState.MercFactionTeamOverride.TeamOverrideFallback = fallbackTag;
                    if (ModInit.modSettings.enableTrace)
                    {
                        var debugIDS = "";
                        foreach (var kvp in __instance.TeamFactionIDs)
                        {
                            debugIDS += $"{kvp.Key}: {FactionEnumeration.GetFactionByID(kvp.Value).Name}\n";
                        }
                        ModInit.modLog?.Trace?.Write($"[Contract_BeginRequestResources] contractFactionIDs are now {debugIDS}\n");
                    }

                    ModInit.modLog?.Info?.Write(
                        $"[Contract_BeginRequestResources] Set ModState.MercFactionForReplacement to {ModState.MercFactionTeamOverride?.TeamOverride.faction}, original was {ModState.OriginalTargetFactionTeamOverride?.faction}. TargetTeam is now {__instance.Override.targetTeam.FactionValue.Name}. Reinitializing MissionControl");
                    __instance.Override.RunMadLibs(sim.DataManager);
                    global::MissionControl.MissionControl.Instance.SetContract(__instance);
                    return;
                }


                //handle lance replacements
                else if (Utils.ShouldAddPlanetLance(__instance.Override, out var configPlanetLance))

                {
                    var hostileToAllFaction = Utils.GetAlternateFactionPoolFromWeight(sim, configPlanetLance, out var fallback);
                    if (hostileToAllFaction == -1)
                    {
                        ModInit.modLog?.Error?.Write($"[Contract_BeginRequestResources] Selected faction [-1], aborting. No valid planet factions for employer, probably.");
                        return;
                    }
                    //ModState.HostileToAllLanceTeamOverride = new Tuple<TeamOverride, string>(new TeamOverride(GlobalVars.HostileToAllLanceTeamDefinitionGUID,
                    //    "HostilePlanetTeam"), configPlanetLance.AlternateFactionFallbackTag);
                    ModState.HostileToAllLanceTeamOverride.TeamOverride = new TeamOverride(GlobalVars.HostileToAllLanceTeamDefinitionGUID, "HostilePlanetTeam");
                    ModState.HostileToAllLanceTeamOverride.TeamOverrideFallback = fallback;
                    ModState.HostileToAllLanceTeamOverride.TeamOverride.AssignHostileToAllFactionToTeamState(hostileToAllFaction, sim.DataManager);

                    //do lance prep stuff? -> probably need to call generate units, etc etc etc
                    ModInit.modLog?.Info?.Write(
                        $"[Contract_BeginRequestResources] Set hostile planet lance override to {ModState.HostileToAllLanceTeamOverride?.TeamOverride.faction}; {ModState.HostileToAllLanceTeamOverride?.TeamOverride.FactionValue?.Name}; {ModState.HostileToAllLanceTeamOverride?.TeamOverride.FactionDef?.Name}, will be used if contract has AdditionalLances. Reinitializing MissionControl");
                    ModState.HostileToAllLanceTeamOverride?.TeamOverride.ProcessHeraldryLoadRequest(sim);
                    global::MissionControl.MissionControl.Instance.SetContract(__instance);
                    //                    ModState.HostileMercLanceTeamOverride.RunMadLibs(__instance, sim.DataManager);
                    //                    ModState.HostileMercLanceTeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, __instance.Override.finalDifficulty, sim.CurrentDate, sim.CompanyTags);
                    return;
                }
                else if (Utils.ShouldAddAltFactionLance(__instance.Override, out var configAltLance))

                {
                    var hostileAltLanceFaction = Utils.GetAlternateFactionPoolFromWeight(sim, configAltLance, out var fallback);
                    if (hostileAltLanceFaction == -1)
                    {
                        ModInit.modLog?.Error?.Write($"[Contract_BeginRequestResources] Selected  [-1], aborting. No valid factions for employer, probably.");
                        return;
                    }

                    //ModState.HostileAltLanceTeamOverride = new Tuple<TeamOverride, string>(new TeamOverride(
                    //    GlobalVars.HostileMercLanceTeamDefinitionGUID, "HostileAltFactionTeam"),configAltLance.AlternateFactionFallbackTag);
                    ModState.HostileAltLanceTeamOverride.TeamOverride = new TeamOverride(GlobalVars.HostileMercLanceTeamDefinitionGUID, "HostileAltFactionTeam");
                    ModState.HostileAltLanceTeamOverride.TeamOverrideFallback = fallback;
                    ModState.HostileAltLanceTeamOverride.TeamOverride.AssignHostileAltFactionToTeamState(hostileAltLanceFaction, sim.DataManager);

                    //do lance prep stuff? -> probably need to call generate units, etc etc etc
                    ModInit.modLog?.Info?.Write(
                        $"[Contract_BeginRequestResources] Set hostile alt lance override to {ModState.HostileAltLanceTeamOverride?.TeamOverride.faction}; {ModState.HostileAltLanceTeamOverride?.TeamOverride.FactionValue?.Name}; {ModState.HostileAltLanceTeamOverride?.TeamOverride.FactionDef?.Name}, will be used if contract has AdditionalLances. Reinitializing MissionControl");
                    ModState.HostileAltLanceTeamOverride?.TeamOverride.ProcessHeraldryLoadRequest(sim);
                    global::MissionControl.MissionControl.Instance.SetContract(__instance);
//                    ModState.HostileMercLanceTeamOverride.RunMadLibs(__instance, sim.DataManager);
//                    ModState.HostileMercLanceTeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, __instance.Override.finalDifficulty, sim.CurrentDate, sim.CompanyTags);
                    return;
                }

                else if (Utils.ShouldAddMercLance(__instance.Override))

                {
                    var hostileMercLanceFaction = Utils.GetMercFactionPoolFromWeight(sim, __instance.Override.targetTeam.faction, out var fallbackTag);
                    if (hostileMercLanceFaction == -1)
                    {
                        ModInit.modLog?.Error?.Write($"[Contract_BeginRequestResources] Selected MercFaction [-1], aborting. No valid merc factions for employer, probably.");
                        return;
                    }
                    //ModState.HostileMercLanceTeamOverride = new Tuple<TeamOverride, string>(new TeamOverride(GlobalVars.HostileMercLanceTeamDefinitionGUID,
                    //    "HostileMercenaryTeam"), fallbackTag);
                    ModState.HostileMercLanceTeamOverride.TeamOverride = new TeamOverride(
                        GlobalVars.HostileMercLanceTeamDefinitionGUID,
                        "HostileMercenaryTeam");
                    ModState.HostileMercLanceTeamOverride.TeamOverrideFallback = fallbackTag;
                    ModState.HostileMercLanceTeamOverride.TeamOverride.AssignHostileMercFactionToTeamState(hostileMercLanceFaction, sim.DataManager);

                    //do lance prep stuff? -> probably need to call generate units, etc etc etc
                    ModInit.modLog?.Info?.Write(
                        $"[Contract_BeginRequestResources] Set hostile merc lance override to {ModState.HostileMercLanceTeamOverride?.TeamOverride.faction}; {ModState.HostileMercLanceTeamOverride?.TeamOverride.FactionValue?.Name}; {ModState.HostileMercLanceTeamOverride?.TeamOverride.FactionDef?.Name}, will be used if contract has AdditionalLances. Reinitializing MissionControl");
                    ModState.HostileMercLanceTeamOverride?.TeamOverride.ProcessHeraldryLoadRequest(sim);
                    global::MissionControl.MissionControl.Instance.SetContract(__instance);
                    //                    ModState.HostileMercLanceTeamOverride.RunMadLibs(__instance, sim.DataManager);
                    //                    ModState.HostileMercLanceTeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, __instance.Override.finalDifficulty, sim.CurrentDate, sim.CompanyTags);
                    return;
                }
                //maybwe inject friendly mercs here. one thing at a time though.
            }
        }

        [HarmonyPatch(typeof(ContractOverride), "GenerateUnits",
            new Type[] {typeof(DataManager), typeof(DateTime?), typeof(TagSet)})]
        public static class ContractOverride_GenerateUnits
        {
            static bool Prepare() => false; // disabled, unneeded
            public static void Postfix(ContractOverride __instance, DataManager dataManager, DateTime? currentDate, TagSet companyTags)
            {
                if (ModState.HostileAltLanceTeamOverride.TeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write(
                        $"[ContractOverride_GenerateUnits - HostileAltLanceTeamOverride] running madlips and generating team");
                    var sim = UnityGameInstance.BattleTechGame.Simulation;
                    ModState.HostileAltLanceTeamOverride.TeamOverride.RunMadLibs(__instance.contract, sim.DataManager);
                    ModState.HostileAltLanceTeamOverride.TeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, __instance.finalDifficulty, sim.CurrentDate, sim.CompanyTags);
                }
                else if (ModState.HostileToAllLanceTeamOverride.TeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write(
                        $"[ContractOverride_GenerateUnits - HostileToAllLanceOverride] running madlips and generating team");
                    var sim = UnityGameInstance.BattleTechGame.Simulation;
                    ModState.HostileToAllLanceTeamOverride.TeamOverride.RunMadLibs(__instance.contract, sim.DataManager);
                    ModState.HostileToAllLanceTeamOverride.TeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, __instance.finalDifficulty, sim.CurrentDate, sim.CompanyTags);
                }

                else if (ModState.HostileMercLanceTeamOverride.TeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write(
                        $"[ContractOverride_GenerateUnits - HostileMercLanceTeamOverride] running madlips and generating team");
                    var sim = UnityGameInstance.BattleTechGame.Simulation;
                    ModState.HostileMercLanceTeamOverride.TeamOverride.RunMadLibs(__instance.contract, sim.DataManager);
                    ModState.HostileMercLanceTeamOverride.TeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, __instance.finalDifficulty, sim.CurrentDate, sim.CompanyTags);
                }
            }
        }

        [HarmonyPatch(typeof(EncounterLayerData), "ApplyContractOverride",
            new Type[] {typeof(ContractOverride), typeof(bool)})]
        public static class EncounterLayerData_ApplyContractOverride
        {
            static bool Prepare() => true;

            public static void Postfix(EncounterLayerData __instance, ContractOverride contractOverride, bool fromEditorApplyButton)
            {
                if (ModState.HostileAltLanceTeamOverride.TeamOverride != null)
                {
                    var dictionary = new Dictionary<string, EncounterObjectGameLogic>();
                    __instance.BuildEncounterObjectDictionary(dictionary);
                    __instance.ApplyTeamOverride(ModState.HostileAltLanceTeamOverride.TeamOverride, dictionary, fromEditorApplyButton);
                }
                else if (ModState.HostileToAllLanceTeamOverride.TeamOverride != null)
                {
                    var dictionary = new Dictionary<string, EncounterObjectGameLogic>();
                    __instance.BuildEncounterObjectDictionary(dictionary);
                    __instance.ApplyTeamOverride(ModState.HostileToAllLanceTeamOverride.TeamOverride, dictionary, fromEditorApplyButton);
                }
                else if (ModState.HostileMercLanceTeamOverride.TeamOverride != null)
                {
                    var dictionary = new Dictionary<string, EncounterObjectGameLogic>();
                    __instance.BuildEncounterObjectDictionary(dictionary);
                    __instance.ApplyTeamOverride(ModState.HostileMercLanceTeamOverride.TeamOverride, dictionary, fromEditorApplyButton);
                }
            }
        }

        [HarmonyPatch(typeof(EncounterLayerData), "NewTeamDefinitionList", MethodType.Getter)]
        public static class TeamDefinition_CreateNewTeamDefinitionList
        {
            public static void Postfix(EncounterLayerData __instance, ref List<TeamDefinition> __result)
            {

                if (ModState.HostileAltLanceTeamOverride.TeamOverride != null && __result.All(x => x.GUID != ModState.HostileAltLanceTeamDefinition.GUID))
                {
                    __result.Add(ModState.HostileAltLanceTeamDefinition);
                    ModInit.modLog?.Trace?.Write($"[TeamDefinition_CreateNewTeamDefinitionList] Added HostileAltLanceTeamDefinition with GUID {ModState.HostileAltLanceTeamDefinition.GUID}.");
                }

                else if (ModState.HostileToAllLanceTeamOverride.TeamOverride != null && __result.All(x => x.GUID != ModState.HostileToAllLanceTeamDefinition.GUID))
                {
                    __result.Add(ModState.HostileToAllLanceTeamDefinition);
                    ModInit.modLog?.Trace?.Write($"[TeamDefinition_CreateNewTeamDefinitionList] Added HostileToAllLanceTeamDefinition with GUID {ModState.HostileToAllLanceTeamDefinition.GUID}.");
                }

                else if (ModState.HostileMercLanceTeamOverride.TeamOverride != null && __result.All(x => x.GUID != ModState.HostileMercLanceTeamDefinition.GUID))
                {
                    __result.Add(ModState.HostileMercLanceTeamDefinition);
                    ModInit.modLog?.Trace?.Write($"[TeamDefinition_CreateNewTeamDefinitionList] Added HostileMercLanceTeamDefinition with GUID {ModState.HostileMercLanceTeamDefinition.GUID}.");
                }

//                if (ModState.ActiveContractShouldSpawnAlliedMercs && __result.All(x => x.GUID != ModState.FriendlyMercTeamDefinition.GUID))
//                {
//                    __result.Add(ModState.FriendlyMercTeamDefinition);
//                    ModInit.modLog?.Trace?.Write($"[TeamDefinition_CreateNewTeamDefinitionList] Added FriendlyMercTeamDefinition with GUID {ModState.FriendlyMercTeamDefinition.GUID}.");
//                }
            }
        }

       [HarmonyPatch(typeof(TagSetQueryExtensions), "GetMatchingUnitDefs",
            new Type[]
            {
                typeof(MetadataDatabase), typeof(TagSet), typeof(TagSet), typeof(bool), typeof(DateTime?), typeof(TagSet)
            })]
        public static class TagSetQueryExtensions_GetMatchingUnitDefs
        {
            public static void Prefix(MetadataDatabase mdd, ref TagSet requiredTags, TagSet excludedTags, bool checkOwnership, DateTime? currentDate, TagSet companyTags)
            {
                //check if merc lance needs to use fallback tag
                ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] Running GetMatchingUnitDefs");
                ModInit.modLog?.Trace?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] TAGSPAM {string.Join(", ",requiredTags.items)}");
                if (ModState.PlanetAltFactionTeamOverride.TeamOverride != null)
                {
                    var altPlanetFactionLowerCased = ModState.PlanetAltFactionTeamOverride.TeamOverride.FactionValue.Name.ToLower();
                    if (requiredTags.Contains(altPlanetFactionLowerCased))
                    {
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Found alt planet faction tag: {altPlanetFactionLowerCased} in requiredTags, should be using alt planet units");
                        var resultDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                            currentDate, companyTags);
                        if (resultDefs.Count == 0)
                        {
                            if (!string.IsNullOrEmpty(ModState.PlanetAltFactionTeamOverride.TeamOverrideFallback))
                            {
                                requiredTags.Remove(altPlanetFactionLowerCased);
                                requiredTags.Add(ModState.PlanetAltFactionTeamOverride.TeamOverrideFallback.ToLower());
                                var fallBackDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                                    currentDate, companyTags);
                                ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Original result failed, removing alt planet faction tag: {altPlanetFactionLowerCased}" +
                                                            $"and replacing with faction fallback tag: {ModState.PlanetAltFactionTeamOverride.TeamOverrideFallback.ToLower()}");
                                if (fallBackDefs.Count == 0)
                                {
                                    ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Faction result failed, removing alt planet faction tag: {ModState.PlanetAltFactionTeamOverride.TeamOverrideFallback.ToLower()} " +
                                                                $"and replacing with generic fallback tag: {ModInit.modSettings.FallbackUnitFactionTag.ToLower()}");

                                    requiredTags.Remove(ModState.PlanetAltFactionTeamOverride.TeamOverrideFallback.ToLower());
                                    requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag.ToLower());
                                    return;
                                }
                                return;
                            }
                            requiredTags.Remove(altPlanetFactionLowerCased);
                            requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag.ToLower());
                            ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Original result failed, removing alt planet faction tag: {altPlanetFactionLowerCased} and replacing with fallback tag: {ModInit.modSettings.FallbackUnitFactionTag.ToLower()}");
                            return;
                        }
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Proceeding using original requiredTags.");
                    }
                }
                else if (ModState.AltFactionFactionTeamOverride.TeamOverride != null)
                {
                    var altFactionLowerCased = ModState.AltFactionFactionTeamOverride.TeamOverride.FactionValue.Name.ToLower();
                    if (requiredTags.Contains(altFactionLowerCased))
                    {
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Found alt faction tag: {altFactionLowerCased} in requiredTags, should be using alt units");
                        var resultDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                            currentDate, companyTags);
                        if (resultDefs.Count == 0)
                        {
                            if (!string.IsNullOrEmpty(ModState.AltFactionFactionTeamOverride.TeamOverrideFallback))
                            {
                                requiredTags.Remove(altFactionLowerCased);
                                requiredTags.Add(ModState.AltFactionFactionTeamOverride.TeamOverrideFallback.ToLower());
                                var fallBackDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                                    currentDate, companyTags);
                                ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Original result failed, removing alt faction tag: {altFactionLowerCased}" +
                                                            $"and replacing with faction fallback tag: {ModState.AltFactionFactionTeamOverride.TeamOverrideFallback.ToLower()}");
                                if (fallBackDefs.Count == 0)
                                {
                                    ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Faction result failed, removing alt faction tag: {ModState.AltFactionFactionTeamOverride.TeamOverrideFallback.ToLower()} " +
                                                                $"and replacing with generic fallback tag: {ModInit.modSettings.FallbackUnitFactionTag.ToLower()}");

                                    requiredTags.Remove(ModState.AltFactionFactionTeamOverride.TeamOverrideFallback.ToLower());
                                    requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag.ToLower());
                                    return;
                                }
                                return;
                            }
                            requiredTags.Remove(altFactionLowerCased);
                            requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag.ToLower());
                            ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Original result failed, removing alt faction tag: {altFactionLowerCased} and replacing with fallback tag: {ModInit.modSettings.FallbackUnitFactionTag.ToLower()}");
                            return;
                        }
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Proceeding using original requiredTags.");
                    }
                }
                else if (ModState.MercFactionTeamOverride.TeamOverride != null)
                {
                    var mercFactionLowerCased = ModState.MercFactionTeamOverride.TeamOverride.FactionValue.Name.ToLower();
                    if (requiredTags.Contains(mercFactionLowerCased))
                    {
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Found merc faction tag: {mercFactionLowerCased} in requiredTags, should be using merc units");
                        var resultDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                            currentDate, companyTags);
                        if (resultDefs.Count == 0)
                        {
                            if (!string.IsNullOrEmpty(ModState.MercFactionTeamOverride.TeamOverrideFallback))
                            {
                                requiredTags.Remove(mercFactionLowerCased);
                                requiredTags.Add(ModState.MercFactionTeamOverride.TeamOverrideFallback.ToLower());
                                var fallBackDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                                    currentDate, companyTags);
                                ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Original result failed, removing alt faction tag: {mercFactionLowerCased}" +
                                                            $"and replacing with faction fallback tag: {ModState.MercFactionTeamOverride.TeamOverrideFallback.ToLower()}");
                                if (fallBackDefs.Count == 0)
                                {
                                    ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Faction result failed, removing alt faction tag: {ModState.MercFactionTeamOverride.TeamOverrideFallback.ToLower()} " +
                                                                $"and replacing with generic fallback tag: {ModInit.modSettings.FallbackUnitFactionTag.ToLower()}");

                                    requiredTags.Remove(ModState.MercFactionTeamOverride.TeamOverrideFallback.ToLower());
                                    requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag.ToLower());
                                    return;
                                }
                                return;
                            }
                            requiredTags.Remove(mercFactionLowerCased);
                            requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag.ToLower());
                            ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Original result failed, removing merc faction tag: {mercFactionLowerCased} and replacing with fallback tag: {ModInit.modSettings.FallbackUnitFactionTag.ToLower()}");
                            return;
                        }
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Proceeding using original requiredTags.");
                    }
                }

                else if (ModState.HostileAltLanceTeamOverride.TeamOverride != null)
                {
                    var altLanceFactionLowerCased = ModState.HostileAltLanceTeamOverride.TeamOverride.FactionValue.Name.ToLower();
                    if (requiredTags.Contains(altLanceFactionLowerCased))
                    {
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Found alt faction tag: {altLanceFactionLowerCased} in requiredTags, should be using alt units");
                        var resultDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                            currentDate, companyTags);
                        if (resultDefs.Count == 0)
                        {
                            if (!string.IsNullOrEmpty(ModState.HostileAltLanceTeamOverride.TeamOverrideFallback))
                            {
                                requiredTags.Remove(altLanceFactionLowerCased);
                                requiredTags.Add(ModState.HostileAltLanceTeamOverride.TeamOverrideFallback.ToLower());
                                var fallBackDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                                    currentDate, companyTags);
                                ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Original result failed, removing alt faction tag: {altLanceFactionLowerCased}" +
                                                            $"and replacing with faction fallback tag: {ModState.HostileAltLanceTeamOverride.TeamOverrideFallback.ToLower()}");
                                if (fallBackDefs.Count == 0)
                                {
                                    ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Faction result failed, removing alt faction tag: {ModState.HostileAltLanceTeamOverride.TeamOverrideFallback.ToLower()} " +
                                                                $"and replacing with generic fallback tag: {ModInit.modSettings.FallbackUnitFactionTag.ToLower()}");

                                    requiredTags.Remove(ModState.HostileAltLanceTeamOverride.TeamOverrideFallback.ToLower());
                                    requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag.ToLower());
                                    return;
                                }
                                return;
                            }
                            requiredTags.Remove(altLanceFactionLowerCased);
                            requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag.ToLower());
                            ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Original result failed, removing alt faction tag: {altLanceFactionLowerCased} and replacing with fallback tag: {ModInit.modSettings.FallbackUnitFactionTag.ToLower()}");
                            return;
                        }
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Proceeding using original requiredTags.");
                    }
                }
                else if (ModState.HostileToAllLanceTeamOverride.TeamOverride != null)
                {
                    var hostileAllFactionLowerCased = ModState.HostileToAllLanceTeamOverride.TeamOverride.FactionValue.Name.ToLower();
                    if (requiredTags.Contains(hostileAllFactionLowerCased))
                    {
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Found hostile all faction tag: {hostileAllFactionLowerCased} in requiredTags, should be using hostile all units");
                        var resultDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                            currentDate, companyTags);
                        if (resultDefs.Count == 0)
                        {
                            if (!string.IsNullOrEmpty(ModState.HostileToAllLanceTeamOverride.TeamOverrideFallback))
                            {
                                requiredTags.Remove(hostileAllFactionLowerCased);
                                requiredTags.Add(ModState.HostileToAllLanceTeamOverride.TeamOverrideFallback.ToLower());
                                var fallBackDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                                    currentDate, companyTags);
                                ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Original result failed, removing alt faction tag: {hostileAllFactionLowerCased}" +
                                                            $"and replacing with faction fallback tag: {ModState.HostileToAllLanceTeamOverride.TeamOverrideFallback.ToLower()}");
                                if (fallBackDefs.Count == 0)
                                {
                                    ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Faction result failed, removing alt faction tag: {ModState.HostileToAllLanceTeamOverride.TeamOverrideFallback.ToLower()} " +
                                                                $"and replacing with generic fallback tag: {ModInit.modSettings.FallbackUnitFactionTag.ToLower()}");

                                    requiredTags.Remove(ModState.HostileToAllLanceTeamOverride.TeamOverrideFallback.ToLower());
                                    requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag.ToLower());
                                    return;
                                }
                                return;
                            }
                            requiredTags.Remove(hostileAllFactionLowerCased);
                            requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag.ToLower());
                            ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Original result failed, removing hostile all faction tag: {hostileAllFactionLowerCased} and replacing with fallback tag: {ModInit.modSettings.FallbackUnitFactionTag.ToLower()}");
                            return;
                        }
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Proceeding using original requiredTags.");
                    }
                }
                else if (ModState.HostileMercLanceTeamOverride.TeamOverride != null)
                {
                    var mercLanceFactionLowerCased = ModState.HostileMercLanceTeamOverride.TeamOverride.FactionValue.Name.ToLower();
                    if (requiredTags.Contains(mercLanceFactionLowerCased))
                    {
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Found merc faction tag: {mercLanceFactionLowerCased} in requiredTags, should be using merc units");
                        var resultDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                            currentDate, companyTags);
                        if (resultDefs.Count == 0)
                        {
                            if (!string.IsNullOrEmpty(ModState.HostileMercLanceTeamOverride.TeamOverrideFallback))
                            {
                                requiredTags.Remove(mercLanceFactionLowerCased);
                                requiredTags.Add(ModState.HostileMercLanceTeamOverride.TeamOverrideFallback.ToLower());
                                var fallBackDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                                    currentDate, companyTags);
                                ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Original result failed, removing alt faction tag: {mercLanceFactionLowerCased}" +
                                                            $"and replacing with faction fallback tag: {ModState.HostileMercLanceTeamOverride.TeamOverrideFallback.ToLower()}");
                                if (fallBackDefs.Count == 0)
                                {
                                    ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Faction result failed, removing alt faction tag: {ModState.HostileMercLanceTeamOverride.TeamOverrideFallback.ToLower()}" +
                                                                $"and replacing with generic fallback tag: {ModInit.modSettings.FallbackUnitFactionTag.ToLower()}");

                                    requiredTags.Remove(ModState.HostileMercLanceTeamOverride.TeamOverrideFallback.ToLower());
                                    requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag.ToLower());
                                    return;
                                }
                                return;
                            }
                            requiredTags.Remove(mercLanceFactionLowerCased);
                            requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag.ToLower());
                            ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Original result failed, removing merc faction tag: {mercLanceFactionLowerCased} and replacing with fallback tag: {ModInit.modSettings.FallbackUnitFactionTag.ToLower()}");
                            return;
                        }
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Proceeding using original requiredTags.");
                    }
                }
            }
        }

        [HarmonyPatch(typeof(AAR_FactionReputationResultWidget), "InitializeData",
            new Type[] {typeof(SimGameState), typeof(Contract)})]
        public static class AAR_FactionReputationResultWidget_InitializeData_Patch
        {
            public static void Postfix(AAR_FactionReputationResultWidget __instance, SimGameState theSimState,
                Contract theContract)
            {
                if (ModState.PlanetAltFactionTeamOverride.TeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write($"[AAR_FactionReputationResultWidget_InitializeData_Patch] Processing reputation change for original target faction due to replacement with planet alt faction.");
                    var faction = UnityGameInstance.BattleTechGame.DataManager.Factions
                        .FirstOrDefault(x =>
                            x.Value.FactionValue.Name == ModState.OriginalTargetFactionTeamOverride.FactionValue.Name)
                        .Value;

                    if (faction != null)
                    {
                        if (faction.FactionValue.DoesGainReputation) //TargetVsEmployer
                        {
                            var component = theSimState.DataManager
                                .PooledInstantiate("uixPrfWidget_AAR_FactionRepBarAndIcon",
                                    BattleTechResourceType.UIModulePrefabs)
                                .GetComponent<SGReputationWidget_Simple>();
                            component.transform.SetParent(__instance.WidgetListAnchor, false);

                            __instance.FactionWidgets.Add(component);

                            var repChange = 0;

                            if ((theContract.TargetReputationResults >= -1 &&
                                 theContract.Override.targetTeam.FactionValue.DoesGainReputation) ||
                                !theContract.Override.targetTeam.FactionValue.DoesGainReputation)
                            {
                                if (theContract.EmployerReputationResults > 0)
                                {
                                    repChange = -theContract.EmployerReputationResults;
                                }
                                else repChange = -(theContract.Difficulty + 2);
                            }
                            else
                            {
                                repChange = theContract.TargetReputationResults;
                            }

                            var idx = __instance.FactionWidgets.Count - 1;
                            __instance.SetWidgetData(idx, faction.FactionValue, repChange, true);
                            theSimState.SetReputation(faction.FactionValue, repChange);
                        }
                    }
                }
                else if (ModState.AltFactionFactionTeamOverride.TeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write($"[AAR_FactionReputationResultWidget_InitializeData_Patch] Processing reputation change for original target faction due to replacement with alt faction.");
                    var faction = UnityGameInstance.BattleTechGame.DataManager.Factions
                        .FirstOrDefault(x =>
                            x.Value.FactionValue.Name == ModState.OriginalTargetFactionTeamOverride.FactionValue.Name)
                        .Value;

                   if (faction != null) 
                   {
                        if (faction.FactionValue.DoesGainReputation) //TargetVsEmployer
                        {
                            var component = theSimState.DataManager
                                .PooledInstantiate("uixPrfWidget_AAR_FactionRepBarAndIcon",
                                    BattleTechResourceType.UIModulePrefabs)
                                .GetComponent<SGReputationWidget_Simple>();
                            component.transform.SetParent(__instance.WidgetListAnchor, false);

                            __instance.FactionWidgets.Add(component);

                            var repChange = 0;

                            if ((theContract.TargetReputationResults >= -1 &&
                                 theContract.Override.targetTeam.FactionValue.DoesGainReputation) ||
                                !theContract.Override.targetTeam.FactionValue.DoesGainReputation)
                            {
                                if (theContract.EmployerReputationResults > 0)
                                {
                                    repChange = -theContract.EmployerReputationResults;
                                }
                                else repChange = -(theContract.Difficulty + 2);
                            }
                            else
                            {
                                repChange = theContract.TargetReputationResults;
                            }

                            var idx = __instance.FactionWidgets.Count - 1;
                            __instance.SetWidgetData(idx, faction.FactionValue, repChange, true);
                            theSimState.SetReputation(faction.FactionValue, repChange);
                        }
                    }
                }
                else if (ModState.MercFactionTeamOverride.TeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write($"[AAR_FactionReputationResultWidget_InitializeData_Patch] Processing reputation change for original target faction due to replacement with mercs.");
                    var faction = UnityGameInstance.BattleTechGame.DataManager.Factions
                        .FirstOrDefault(x =>
                            x.Value.FactionValue.Name == ModState.OriginalTargetFactionTeamOverride.FactionValue.Name)
                        .Value;

                    theContract.Override.targetTeam.UpdateMercFactionStats(theSimState);

                    if (faction != null)
                    {
                        if (faction.FactionValue.DoesGainReputation) //TargetVsEmployer
                        {
                            var component = theSimState.DataManager
                                .PooledInstantiate("uixPrfWidget_AAR_FactionRepBarAndIcon",
                                    BattleTechResourceType.UIModulePrefabs)
                                .GetComponent<SGReputationWidget_Simple>();
                            component.transform.SetParent(__instance.WidgetListAnchor, false);

                            __instance.FactionWidgets.Add(component);

                            var repChange = 0;

                            if ((theContract.TargetReputationResults >= -1 &&
                                 theContract.Override.targetTeam.FactionValue.DoesGainReputation) ||
                                !theContract.Override.targetTeam.FactionValue.DoesGainReputation)
                            {
                                if (theContract.EmployerReputationResults > 0)
                                {
                                    repChange = -theContract.EmployerReputationResults;
                                }
                                else repChange = -(theContract.Difficulty + 2);
                            }
                            else
                            {
                                repChange = theContract.TargetReputationResults;
                            }

                            var idx = __instance.FactionWidgets.Count - 1;
                            __instance.SetWidgetData(idx, faction.FactionValue, repChange, true);
                            theSimState.SetReputation(faction.FactionValue, repChange);
                        }
                    }
                }


                else if (ModState.HostileToAllLanceTeamOverride.TeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write($"[AAR_FactionReputationResultWidget_InitializeData_Patch] Processing reputation change for hostile all faction due to support lance deployment.");
                    var faction = UnityGameInstance.BattleTechGame.DataManager.Factions
                        .FirstOrDefault(x =>
                            x.Value.FactionValue.Name == ModState.HostileToAllLanceTeamOverride.TeamOverride.FactionValue.Name)
                        .Value;
                    ModState.HostileToAllLanceTeamOverride.TeamOverride.UpdateMercFactionStats(theSimState);
                    if (faction != null)
                    {
                        if (faction.FactionValue.DoesGainReputation) //TargetVsEmployer
                        {
                            var component = theSimState.DataManager
                                .PooledInstantiate("uixPrfWidget_AAR_FactionRepBarAndIcon",
                                    BattleTechResourceType.UIModulePrefabs)
                                .GetComponent<SGReputationWidget_Simple>();
                            component.transform.SetParent(__instance.WidgetListAnchor, false);

                            __instance.FactionWidgets.Add(component);

                            var repChange = 0;

                            if ((theContract.TargetReputationResults >= -1 &&
                                 theContract.Override.targetTeam.FactionValue.DoesGainReputation) ||
                                !theContract.Override.targetTeam.FactionValue.DoesGainReputation)
                            {
                                if (theContract.EmployerReputationResults > 0)
                                {
                                    repChange = -theContract.EmployerReputationResults;
                                }
                                else repChange = -(theContract.Difficulty + 2);
                            }
                            else
                            {
                                repChange = theContract.TargetReputationResults;
                            }

                            //var factoredRepChange = repChange * ModInit.modSettings.MercLanceAdditionConfig.MercFactionReputationFactor;
                            //var finalRepChange = Mathf.RoundToInt(factoredRepChange);

                            var idx = __instance.FactionWidgets.Count - 1;
                            __instance.SetWidgetData(idx, faction.FactionValue, repChange, true);
                            theSimState.SetReputation(faction.FactionValue, repChange);
                        }
                    }
                }
                else if (ModState.HostileAltLanceTeamOverride.TeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write($"[AAR_FactionReputationResultWidget_InitializeData_Patch] Processing reputation change for alt faction due to support lance deployment.");
                    var faction = UnityGameInstance.BattleTechGame.DataManager.Factions
                        .FirstOrDefault(x =>
                            x.Value.FactionValue.Name == ModState.HostileAltLanceTeamOverride?.TeamOverride.FactionValue.Name)
                        .Value;
                    ModState.HostileAltLanceTeamOverride?.TeamOverride.UpdateMercFactionStats(theSimState);
                    if (faction != null)
                    {
                        if (faction.FactionValue.DoesGainReputation) //TargetVsEmployer
                        {
                            var component = theSimState.DataManager
                                .PooledInstantiate("uixPrfWidget_AAR_FactionRepBarAndIcon",
                                    BattleTechResourceType.UIModulePrefabs)
                                .GetComponent<SGReputationWidget_Simple>();
                            component.transform.SetParent(__instance.WidgetListAnchor, false);

                            __instance.FactionWidgets.Add(component);

                            var repChange = 0;

                            if ((theContract.TargetReputationResults >= -1 &&
                                 theContract.Override.targetTeam.FactionValue.DoesGainReputation) ||
                                !theContract.Override.targetTeam.FactionValue.DoesGainReputation)
                            {
                                if (theContract.EmployerReputationResults > 0)
                                {
                                    repChange = -theContract.EmployerReputationResults;
                                }
                                else repChange = -(theContract.Difficulty + 2);
                            }
                            else
                            {
                                repChange = theContract.TargetReputationResults;
                            }

                            //var factoredRepChange = repChange * ModInit.modSettings.MercLanceAdditionConfig.MercFactionReputationFactor;
                            //var finalRepChange = Mathf.RoundToInt(factoredRepChange);

                            var idx = __instance.FactionWidgets.Count - 1;
                            __instance.SetWidgetData(idx, faction.FactionValue, repChange, true);
                            theSimState.SetReputation(faction.FactionValue, repChange);
                        }
                    }
                }
                
                else if (ModState.HostileMercLanceTeamOverride.TeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write($"[AAR_FactionReputationResultWidget_InitializeData_Patch] Processing reputation change for merc faction due to support lance deployment.");
                    var faction = UnityGameInstance.BattleTechGame.DataManager.Factions
                        .FirstOrDefault(x =>
                            x.Value.FactionValue.Name == ModState.HostileMercLanceTeamOverride.TeamOverride.FactionValue.Name)
                        .Value;
                    ModState.HostileMercLanceTeamOverride.TeamOverride.UpdateMercFactionStats(theSimState);
                    if (faction != null)
                    {
                        if (faction.FactionValue.DoesGainReputation) //TargetVsEmployer
                        {
                            var component = theSimState.DataManager
                                .PooledInstantiate("uixPrfWidget_AAR_FactionRepBarAndIcon",
                                    BattleTechResourceType.UIModulePrefabs)
                                .GetComponent<SGReputationWidget_Simple>();
                            component.transform.SetParent(__instance.WidgetListAnchor, false);

                            __instance.FactionWidgets.Add(component);

                            var repChange = 0;

                            if ((theContract.TargetReputationResults >= -1 &&
                                 theContract.Override.targetTeam.FactionValue.DoesGainReputation) ||
                                !theContract.Override.targetTeam.FactionValue.DoesGainReputation)
                            {
                                if (theContract.EmployerReputationResults > 0)
                                {
                                    repChange = -theContract.EmployerReputationResults;
                                }
                                else repChange = -(theContract.Difficulty + 2);
                            }
                            else
                            {
                                repChange = theContract.TargetReputationResults;
                            }

                            var factoredRepChange = repChange * ModInit.modSettings.MercLanceAdditionConfig.MercFactionReputationFactor;
                            var finalRepChange = Mathf.RoundToInt(factoredRepChange);

                            var idx = __instance.FactionWidgets.Count - 1;
                            __instance.SetWidgetData(idx, faction.FactionValue, finalRepChange, true);
                            theSimState.SetReputation(faction.FactionValue, finalRepChange);
                        }
                    }
                }
            }
        }
    }
}
