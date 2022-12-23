using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using BattleTech.Data;
using BattleTech.Framework;
using BattleTech.StringInterpolation;
using BattleTech.UI;
using Harmony;
using HBS.Collections;
using HBS.Math;
using IRBTModUtils;
using Localize;
using MissionControl.Data;
using MissionControl.Logic;
using MissionControl.Rules;
using MissionControl.Trigger;
using SoldiersPiratesAssassinsMercs.Framework;
using UIWidgets;
using UnityEngine;
using static BattleTech.ModSupport.Utils.AdvancedJSONMerge;
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
                if (ModState.PlanetAltFactionTeamOverride != null)
                {
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.PlanetAltFactionTeamOverride);
                }
                else if (ModState.AltFactionFactionTeamOverride != null)
                {
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.AltFactionFactionTeamOverride);
                }
                else if (ModState.MercFactionTeamOverride != null)
                {
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.MercFactionTeamOverride);
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
                if (ModState.PlanetAltFactionTeamOverride != null)
                {
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.PlanetAltFactionTeamOverride);
                }
                else if (ModState.AltFactionFactionTeamOverride != null)
                {
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.AltFactionFactionTeamOverride);
                }
                else if (ModState.MercFactionTeamOverride != null)
                {
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.MercFactionTeamOverride);
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
                    var planetFaction = Utils.GetAlternateFactionPoolFromWeight(sim, configPlanet);
                    ModState.OriginalTargetFactionTeamOverride = __instance.Override.targetTeam.Copy();

                    var contractFactionIDs = __instance.teamFactionIDs;
                    if (contractFactionIDs.ContainsKey(__instance.Override.targetTeam.teamGuid))
                    {
                        contractFactionIDs[__instance.Override.targetTeam.teamGuid] = planetFaction;
                        //contractFactionIDsT.SetValue(contractFactionIDs);
                    }

                    __instance.Override.targetTeam.ReAssignHostileToAllFactionToTeam(contractFactionIDs, sim.DataManager);
                    ModState.PlanetAltFactionTeamOverride = __instance.Override.targetTeam;

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
                        $"[Contract_BeginRequestResources] Set ModState.AltPlanetFactionTeamOverride to {ModState.PlanetAltFactionTeamOverride?.faction}, original was {ModState.OriginalTargetFactionTeamOverride?.faction}. TargetTeam is now {__instance.Override.targetTeam.FactionValue.Name}. Reinitializing MissionControl");
                    global::MissionControl.MissionControl.Instance.SetContract(__instance);
                    return;
                }
                else if (Utils.ShouldReplaceOpforWithFactionAlternate(__instance.Override, out var configAlt))
                {
                    var altFaction = Utils.GetAlternateFactionPoolFromWeight(sim, configAlt);
                    ModState.OriginalTargetFactionTeamOverride = __instance.Override.targetTeam.Copy();


                    var contractFactionIDs = __instance.teamFactionIDs;
                    if (contractFactionIDs.ContainsKey(__instance.Override.targetTeam.teamGuid))
                    {
                        contractFactionIDs[__instance.Override.targetTeam.teamGuid] = altFaction;
                        //contractFactionIDsT.SetValue(contractFactionIDs);
                    }

                    __instance.Override.targetTeam.ReAssignHostileAltFactionToTeam(contractFactionIDs, sim.DataManager);
                    ModState.AltFactionFactionTeamOverride = __instance.Override.targetTeam;

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
                        $"[Contract_BeginRequestResources] Set ModState.AltFactionFactionTeamOverride to {ModState.AltFactionFactionTeamOverride?.faction}, original was {ModState.OriginalTargetFactionTeamOverride?.faction}. TargetTeam is now {__instance.Override.targetTeam.FactionValue.Name}. Reinitializing MissionControl");
                    global::MissionControl.MissionControl.Instance.SetContract(__instance);
                    return;
                }
                else if (Utils.ShouldReplaceOpforWithMercs(__instance.Override))
                {
                    var mercFaction = Utils.GetMercFactionPoolFromWeight(sim, __instance.Override.targetTeam.faction);
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
                    ModState.MercFactionTeamOverride = __instance.Override.targetTeam;

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
                        $"[Contract_BeginRequestResources] Set ModState.MercFactionForReplacement to {ModState.MercFactionTeamOverride?.faction}, original was {ModState.OriginalTargetFactionTeamOverride?.faction}. TargetTeam is now {__instance.Override.targetTeam.FactionValue.Name}. Reinitializing MissionControl");
                    global::MissionControl.MissionControl.Instance.SetContract(__instance);
                    return;
                }


                //handle lance replacements
                else if (Utils.ShouldAddPlanetLance(__instance.Override, out var configPlanetLance))

                {
                    var hostileToAllFaction = Utils.GetAlternateFactionPoolFromWeight(sim, configPlanetLance);
                    if (hostileToAllFaction == -1)
                    {
                        ModInit.modLog?.Error?.Write($"[Contract_BeginRequestResources] Selected faction [-1], aborting. No valid planet factions for employer, probably.");
                        return;
                    }
                    ModState.HostileToAllLanceTeamOverride = new TeamOverride(GlobalVars.HostileToAllLanceTeamDefinitionGUID,
                        "HostilePlanetTeam");
                    ModState.HostileToAllLanceTeamOverride.AssignHostileToAllFactionToTeamState(hostileToAllFaction, sim.DataManager);

                    //do lance prep stuff? -> probably need to call generate units, etc etc etc
                    ModInit.modLog?.Info?.Write(
                        $"[Contract_BeginRequestResources] Set hostile lance override to {ModState.HostileToAllLanceTeamOverride?.faction}; {ModState.HostileMercLanceTeamOverride?.FactionValue?.Name}; {ModState.HostileMercLanceTeamOverride?.FactionDef?.Name}, will be used if contract has AdditionalLances. Reinitializing MissionControl");
                    ModState.HostileToAllLanceTeamOverride.ProcessHeraldryLoadRequest(sim);
                    global::MissionControl.MissionControl.Instance.SetContract(__instance);
                    //                    ModState.HostileMercLanceTeamOverride.RunMadLibs(__instance, sim.DataManager);
                    //                    ModState.HostileMercLanceTeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, __instance.Override.finalDifficulty, sim.CurrentDate, sim.CompanyTags);
                    return;
                }
                else if (Utils.ShouldAddAltFactionLance(__instance.Override, out var configAltLance))

                {
                    var hostileAltLanceFaction = Utils.GetAlternateFactionPoolFromWeight(sim, configAltLance);
                    if (hostileAltLanceFaction == -1)
                    {
                        ModInit.modLog?.Error?.Write($"[Contract_BeginRequestResources] Selected  [-1], aborting. No valid factions for employer, probably.");
                        return;
                    }

                    ModState.HostileAltLanceTeamOverride = new TeamOverride(
                        GlobalVars.HostileMercLanceTeamDefinitionGUID,
                        "HostileAltFactionTeam");
                    ModState.HostileAltLanceTeamOverride.AssignHostileAltFactionToTeamState(hostileAltLanceFaction, sim.DataManager);

                    //do lance prep stuff? -> probably need to call generate units, etc etc etc
                    ModInit.modLog?.Info?.Write(
                        $"[Contract_BeginRequestResources] Set hostile merc lance override to {ModState.HostileMercLanceTeamOverride?.faction}; {ModState.HostileMercLanceTeamOverride?.FactionValue?.Name}; {ModState.HostileMercLanceTeamOverride?.FactionDef?.Name}, will be used if contract has AdditionalLances. Reinitializing MissionControl");
                    ModState.HostileAltLanceTeamOverride.ProcessHeraldryLoadRequest(sim);
                    global::MissionControl.MissionControl.Instance.SetContract(__instance);
//                    ModState.HostileMercLanceTeamOverride.RunMadLibs(__instance, sim.DataManager);
//                    ModState.HostileMercLanceTeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, __instance.Override.finalDifficulty, sim.CurrentDate, sim.CompanyTags);
                    return;
                }

                else if (Utils.ShouldAddMercLance(__instance.Override))

                {
                    var hostileMercLanceFaction = Utils.GetMercFactionPoolFromWeight(sim, __instance.Override.targetTeam.faction);
                    if (hostileMercLanceFaction == -1)
                    {
                        ModInit.modLog?.Error?.Write($"[Contract_BeginRequestResources] Selected MercFaction [-1], aborting. No valid merc factions for employer, probably.");
                        return;
                    }
                    ModState.HostileMercLanceTeamOverride = new TeamOverride(GlobalVars.HostileMercLanceTeamDefinitionGUID,
                        "HostileMercenaryTeam");
                    ModState.HostileMercLanceTeamOverride.AssignHostileMercFactionToTeamState(hostileMercLanceFaction, sim.DataManager);

                    //do lance prep stuff? -> probably need to call generate units, etc etc etc
                    ModInit.modLog?.Info?.Write(
                        $"[Contract_BeginRequestResources] Set hostile merc lance override to {ModState.HostileMercLanceTeamOverride?.faction}; {ModState.HostileMercLanceTeamOverride?.FactionValue?.Name}; {ModState.HostileMercLanceTeamOverride?.FactionDef?.Name}, will be used if contract has AdditionalLances. Reinitializing MissionControl");
                    ModState.HostileMercLanceTeamOverride.ProcessHeraldryLoadRequest(sim);
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
                if (ModState.HostileAltLanceTeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write(
                        $"[ContractOverride_GenerateUnits - HostileAltLanceTeamOverride] running madlips and generating team");
                    var sim = UnityGameInstance.BattleTechGame.Simulation;
                    ModState.HostileAltLanceTeamOverride.RunMadLibs(__instance.contract, sim.DataManager);
                    ModState.HostileAltLanceTeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, __instance.finalDifficulty, sim.CurrentDate, sim.CompanyTags);
                }
                else if (ModState.HostileToAllLanceTeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write(
                        $"[ContractOverride_GenerateUnits - HostileToAllLanceOverride] running madlips and generating team");
                    var sim = UnityGameInstance.BattleTechGame.Simulation;
                    ModState.HostileToAllLanceTeamOverride.RunMadLibs(__instance.contract, sim.DataManager);
                    ModState.HostileToAllLanceTeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, __instance.finalDifficulty, sim.CurrentDate, sim.CompanyTags);
                }

                else if (ModState.HostileMercLanceTeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write(
                        $"[ContractOverride_GenerateUnits - HostileMercLanceTeamOverride] running madlips and generating team");
                    var sim = UnityGameInstance.BattleTechGame.Simulation;
                    ModState.HostileMercLanceTeamOverride.RunMadLibs(__instance.contract, sim.DataManager);
                    ModState.HostileMercLanceTeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, __instance.finalDifficulty, sim.CurrentDate, sim.CompanyTags);
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
                if (ModState.HostileAltLanceTeamOverride != null)
                {
                    var dictionary = new Dictionary<string, EncounterObjectGameLogic>();
                    __instance.BuildEncounterObjectDictionary(dictionary);
                    __instance.ApplyTeamOverride(ModState.HostileAltLanceTeamOverride, dictionary, fromEditorApplyButton);
                }
                else if (ModState.HostileToAllLanceTeamOverride != null)
                {
                    var dictionary = new Dictionary<string, EncounterObjectGameLogic>();
                    __instance.BuildEncounterObjectDictionary(dictionary);
                    __instance.ApplyTeamOverride(ModState.HostileToAllLanceTeamOverride, dictionary, fromEditorApplyButton);
                }
                else if (ModState.HostileMercLanceTeamOverride != null)
                {
                    var dictionary = new Dictionary<string, EncounterObjectGameLogic>();
                    __instance.BuildEncounterObjectDictionary(dictionary);
                    __instance.ApplyTeamOverride(ModState.HostileMercLanceTeamOverride, dictionary, fromEditorApplyButton);
                }
            }
        }

        [HarmonyPatch(typeof(EncounterLayerData), "NewTeamDefinitionList", MethodType.Getter)]
        public static class TeamDefinition_CreateNewTeamDefinitionList
        {
            public static void Postfix(EncounterLayerData __instance, ref List<TeamDefinition> __result)
            {

                if (ModState.HostileAltLanceTeamOverride != null && __result.All(x => x.GUID != ModState.HostileAltLanceTeamDefinition.GUID))
                {
                    __result.Add(ModState.HostileAltLanceTeamDefinition);
                    ModInit.modLog?.Trace?.Write($"[TeamDefinition_CreateNewTeamDefinitionList] Added HostileAltLanceTeamDefinition with GUID {ModState.HostileAltLanceTeamDefinition.GUID}.");
                }

                else if (ModState.HostileToAllLanceTeamOverride != null && __result.All(x => x.GUID != ModState.HostileToAllLanceTeamDefinition.GUID))
                {
                    __result.Add(ModState.HostileToAllLanceTeamDefinition);
                    ModInit.modLog?.Trace?.Write($"[TeamDefinition_CreateNewTeamDefinitionList] Added HostileToAllLanceTeamDefinition with GUID {ModState.HostileToAllLanceTeamDefinition.GUID}.");
                }

                else if (ModState.HostileMercLanceTeamOverride != null && __result.All(x => x.GUID != ModState.HostileMercLanceTeamDefinition.GUID))
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
                var tags = requiredTags.ToArray();
                ModInit.modLog?.Trace?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] TAGSPAM {string.Join(", ",tags)}");
                if (ModState.PlanetAltFactionTeamOverride != null)
                {
                    var altFactionLowerCased = ModState.PlanetAltFactionTeamOverride.FactionValue.Name.ToLower();
                    if (requiredTags.Contains(altFactionLowerCased))
                    {
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Found alt planet faction tag: {altFactionLowerCased} in requiredTags, should be using alt planet units");
                        var resultDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                            currentDate, companyTags);
                        if (resultDefs.Count == 0)
                        {
                            requiredTags.Remove(altFactionLowerCased);
                            requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag);
                            ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Original result failed, removing alt faction tag: {altFactionLowerCased} and replacing with fallback tag: {ModInit.modSettings.FallbackUnitFactionTag}");
                            return;
                        }
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Proceeding using original requiredTags.");
                    }
                }
                else if (ModState.AltFactionFactionTeamOverride != null)
                {
                    var altFactionLowerCased = ModState.AltFactionFactionTeamOverride.FactionValue.Name.ToLower();
                    if (requiredTags.Contains(altFactionLowerCased))
                    {
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Found alt faction tag: {altFactionLowerCased} in requiredTags, should be using alt units");
                        var resultDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                            currentDate, companyTags);
                        if (resultDefs.Count == 0)
                        {
                            requiredTags.Remove(altFactionLowerCased);
                            requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag);
                            ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Original result failed, removing alt faction tag: {altFactionLowerCased} and replacing with fallback tag: {ModInit.modSettings.FallbackUnitFactionTag}");
                            return;
                        }
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Proceeding using original requiredTags.");
                    }
                }
                else if (ModState.MercFactionTeamOverride != null)
                {
                    var mercFactionLowerCased = ModState.MercFactionTeamOverride.FactionValue.Name.ToLower();
                    if (requiredTags.Contains(mercFactionLowerCased))
                    {
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Found merc faction tag: {mercFactionLowerCased} in requiredTags, should be using merc units");
                        var resultDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                            currentDate, companyTags);
                        if (resultDefs.Count == 0)
                        {
                            requiredTags.Remove(mercFactionLowerCased);
                            requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag);
                            ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Original result failed, removing merc faction tag: {mercFactionLowerCased} and replacing with fallback tag: {ModInit.modSettings.FallbackUnitFactionTag}");
                            return;
                        }
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [TargetTeam Override] Proceeding using original requiredTags.");
                    }
                }





                else if (ModState.HostileAltLanceTeamOverride != null)
                {
                    var altFactionLowerCased = ModState.HostileAltLanceTeamOverride.FactionValue.Name.ToLower();
                    if (requiredTags.Contains(altFactionLowerCased))
                    {
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Found alt faction tag: {altFactionLowerCased} in requiredTags, should be using alt units");
                        var resultDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                            currentDate, companyTags);
                        if (resultDefs.Count == 0)
                        {
                            requiredTags.Remove(altFactionLowerCased);
                            requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag);
                            ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Original result failed, removing alt faction tag: {altFactionLowerCased} and replacing with fallback tag: {ModInit.modSettings.FallbackUnitFactionTag}");
                            return;
                        }
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Proceeding using original requiredTags.");
                    }
                }
                else if (ModState.HostileToAllLanceTeamOverride != null)
                {
                    var hostileAllFactionLowerCased = ModState.HostileToAllLanceTeamOverride.FactionValue.Name.ToLower();
                    if (requiredTags.Contains(hostileAllFactionLowerCased))
                    {
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Found hostile all faction tag: {hostileAllFactionLowerCased} in requiredTags, should be using hostile all units");
                        var resultDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                            currentDate, companyTags);
                        if (resultDefs.Count == 0)
                        {
                            requiredTags.Remove(hostileAllFactionLowerCased);
                            requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag);
                            ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Original result failed, removing hostile all faction tag: {hostileAllFactionLowerCased} and replacing with fallback tag: {ModInit.modSettings.FallbackUnitFactionTag}");
                            return;
                        }
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Proceeding using original requiredTags.");
                    }
                }
                else if (ModState.HostileMercLanceTeamOverride != null)
                {
                    var mercFactionLowerCased = ModState.HostileMercLanceTeamOverride.FactionValue.Name.ToLower();
                    if (requiredTags.Contains(mercFactionLowerCased))
                    {
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Found merc faction tag: {mercFactionLowerCased} in requiredTags, should be using merc units");
                        var resultDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                            currentDate, companyTags);
                        if (resultDefs.Count == 0)
                        {
                            requiredTags.Remove(mercFactionLowerCased);
                            requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag);
                            ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] [AdditionalLance Override] Original result failed, removing merc faction tag: {mercFactionLowerCased} and replacing with fallback tag: {ModInit.modSettings.FallbackUnitFactionTag}");
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
                if (ModState.PlanetAltFactionTeamOverride != null)
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
                else if (ModState.AltFactionFactionTeamOverride != null)
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
                else if (ModState.MercFactionTeamOverride != null)
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


                else if (ModState.HostileToAllLanceTeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write($"[AAR_FactionReputationResultWidget_InitializeData_Patch] Processing reputation change for hostile all faction due to support lance deployment.");
                    var faction = UnityGameInstance.BattleTechGame.DataManager.Factions
                        .FirstOrDefault(x =>
                            x.Value.FactionValue.Name == ModState.HostileToAllLanceTeamOverride.FactionValue.Name)
                        .Value;
                    ModState.HostileToAllLanceTeamOverride.UpdateMercFactionStats(theSimState);
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
                else if (ModState.HostileAltLanceTeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write($"[AAR_FactionReputationResultWidget_InitializeData_Patch] Processing reputation change for alt faction due to support lance deployment.");
                    var faction = UnityGameInstance.BattleTechGame.DataManager.Factions
                        .FirstOrDefault(x =>
                            x.Value.FactionValue.Name == ModState.HostileToAllLanceTeamOverride.FactionValue.Name)
                        .Value;
                    ModState.HostileToAllLanceTeamOverride.UpdateMercFactionStats(theSimState);
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
                
                else if (ModState.HostileMercLanceTeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write($"[AAR_FactionReputationResultWidget_InitializeData_Patch] Processing reputation change for merc faction due to support lance deployment.");
                    var faction = UnityGameInstance.BattleTechGame.DataManager.Factions
                        .FirstOrDefault(x =>
                            x.Value.FactionValue.Name == ModState.HostileMercLanceTeamOverride.FactionValue.Name)
                        .Value;
                    ModState.HostileMercLanceTeamOverride.UpdateMercFactionStats(theSimState);
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
