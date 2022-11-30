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
using ModState = SoldiersPiratesAssassinsMercs.Framework.ModState;

namespace SoldiersPiratesAssassinsMercs.Patches
{
    public class ContractPatches
    {
        [HarmonyPatch(typeof(HostilityMatrix), MethodType.Constructor, new Type[]{typeof(CombatGameState), typeof(EncounterPlayStyle)})]
        public static class HostilityMatrix_guidToIndexDictionary
        {
            public static void Postfix(CombatGameState combatGameState, EncounterPlayStyle encounterPlayStyle, ref Dictionary<string, int> ___guidToIndexDictionary, ref Hostility[,] ____matrix)
            {
                ___guidToIndexDictionary.Add("ddfd570d-f9e4-42f8-b2e8-671eb1e8f43a", 11);
                if (encounterPlayStyle == EncounterPlayStyle.SinglePlayer)
                {
                    ____matrix = new Hostility[,]
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
                        Hostility.FRIENDLY
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
                        Hostility.FRIENDLY
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
                if (ModState.OriginalTargetFactionTeamOverride != null)
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.MercFactionTeamOverride);
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
                if (ModState.OriginalTargetFactionTeamOverride != null)
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.MercFactionTeamOverride);
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
                if (Utils.ShouldReplaceOpforWithMercs(__instance.Override))
                {
                    var mercFaction = Utils.GetMercFactionPoolFromWeight(sim, __instance.Override.targetTeam.faction);
                    if (mercFaction == -1)
                    {
                        ModInit.modLog?.Error?.Write($"[Contract_BeginRequestResources] Selected MercFaction [-1], aborting. This is an error in merc faction config.");
                        return;
                    }
                    ModState.OriginalTargetFactionTeamOverride = __instance.Override.targetTeam.Copy();
                    var contractFactionIDsT = Traverse.Create(__instance).Field("teamFactionIDs");
                    var contractFactionIDs = new Dictionary<string, int>(contractFactionIDsT.GetValue<Dictionary<string, int>>());
                    if (contractFactionIDs.ContainsKey(__instance.Override.targetTeam.teamGuid))
                    {
                        contractFactionIDs[__instance.Override.targetTeam.teamGuid] = mercFaction;
                        contractFactionIDsT.SetValue(contractFactionIDs);
                    }

                    __instance.Override.targetTeam.ReAssignFactionToTeam(contractFactionIDs);
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
                }
                else if (Utils.ShouldAddMercLance(__instance.Override))

                {
                    var hostileMercLanceFaction = Utils.GetMercFactionPoolFromWeight(sim, __instance.Override.targetTeam.faction);
                    if (hostileMercLanceFaction == -1)
                    {
                        ModInit.modLog?.Error?.Write($"[Contract_BeginRequestResources] Selected MercFaction [-1], aborting. This is an error in merc faction config.");
                        return;
                    }
                    ModState.HostileMercLanceTeamOverride = new TeamOverride("ddfd570d-f9e4-42f8-b2e8-671eb1e8f43a",
                        "HostileMercenaryTeam");
                    ModState.HostileMercLanceTeamOverride.AssignMercFactionToTeamState(hostileMercLanceFaction);

                    //do lance prep stuff? -> probably need to call generate units, etc etc etc
                    ModInit.modLog?.Info?.Write(
                        $"[Contract_BeginRequestResources] Set hostile merc lance override to {ModState.HostileMercLanceTeamOverride?.faction}; {ModState.HostileMercLanceTeamOverride?.FactionValue?.Name}; {ModState.HostileMercLanceTeamOverride?.FactionDef?.Name}, will be used if contract has AdditionalLances. Reinitializing MissionControl");
                    ModState.HostileMercLanceTeamOverride.ProcessHeraldryLoadRequest(sim);
                    global::MissionControl.MissionControl.Instance.SetContract(__instance);
//                    ModState.HostileMercLanceTeamOverride.RunMadLibs(__instance, sim.DataManager);
//                    ModState.HostileMercLanceTeamOverride.GenerateTeam(MetadataDatabase.Instance, sim.DataManager, __instance.Override.finalDifficulty, sim.CurrentDate, sim.CompanyTags);
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
                if (ModState.HostileMercLanceTeamOverride != null)
                {
                    ModInit.modLog?.Info?.Write(
                        $"[ContractOverride_GenerateUnits] running madlips and generating team");
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
                if (ModState.HostileMercLanceTeamOverride != null)
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
                if (ModState.ActiveContractShouldReplaceLanceWithMercs && __result.All(x => x.GUID != ModState.HostileMercTeamDefinition.GUID))
                {
                    __result.Add(ModState.HostileMercTeamDefinition);
                    ModInit.modLog?.Trace?.Write($"[TeamDefinition_CreateNewTeamDefinitionList] Added HostileMercTeamDefinition with GUID {ModState.HostileMercTeamDefinition.GUID}.");
                }

                if (ModState.ActiveContractShouldSpawnAlliedMercs && __result.All(x => x.GUID != ModState.FriendlyMercTeamDefinition.GUID))
                {
                    __result.Add(ModState.FriendlyMercTeamDefinition);
                    ModInit.modLog?.Trace?.Write($"[TeamDefinition_CreateNewTeamDefinitionList] Added FriendlyMercTeamDefinition with GUID {ModState.FriendlyMercTeamDefinition.GUID}.");
                }
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
                if (ModState.MercFactionTeamOverride != null)
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
                Contract theContract,
                List<SGReputationWidget_Simple> ___FactionWidgets, RectTransform ___WidgetListAnchor)
            {
                if (ModState.OriginalTargetFactionTeamOverride != null)
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
                            component.transform.SetParent(___WidgetListAnchor, false);

                            ___FactionWidgets.Add(component);

                            var repChange = 0;

                            if ((theContract.TargetReputationResults >= -1 &&
                                 theContract.Override.targetTeam.FactionValue.DoesGainReputation) ||
                                !theContract.Override.targetTeam.FactionValue.DoesGainReputation)
                            {
                                repChange = -theContract.Difficulty + 2;
                            }
                            else
                            {
                                repChange = theContract.TargetReputationResults;
                            }

                            var idx = ___FactionWidgets.Count - 1;
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
                            component.transform.SetParent(___WidgetListAnchor, false);

                            ___FactionWidgets.Add(component);

                            var repChange = 0;

                            if ((theContract.TargetReputationResults >= -1 &&
                                 theContract.Override.targetTeam.FactionValue.DoesGainReputation) ||
                                !theContract.Override.targetTeam.FactionValue.DoesGainReputation)
                            {
                                repChange = -theContract.Difficulty + 2;
                            }
                            else
                            {
                                repChange = theContract.TargetReputationResults;
                            }

                            var factoredRepChange = repChange * ModInit.modSettings.MercLanceAdditionConfig.MercFactionReputationFactor;
                            var finalRepChange = Mathf.RoundToInt(factoredRepChange);

                            var idx = ___FactionWidgets.Count - 1;
                            __instance.SetWidgetData(idx, faction.FactionValue, finalRepChange, true);
                            theSimState.SetReputation(faction.FactionValue, finalRepChange);
                        }
                    }
                }
            }
        }
    }
}
