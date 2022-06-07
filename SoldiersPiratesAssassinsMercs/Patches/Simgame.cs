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
using SoldiersPiratesAssassinsMercs.Framework;
using UIWidgets;
using UnityEngine;
using ModState = SoldiersPiratesAssassinsMercs.Framework.ModState;

namespace SoldiersPiratesAssassinsMercs.Patches
{
    public class Simgame
    {
        [HarmonyPatch(typeof(SimGameState), "AddCachedFactionsToDisplayList",
            new Type[] {})]
        public static class SimGameState_AddCachedFactionsToDisplayList
        {
            public static void Postfix(SimGameState __instance)
            {
                ModState.InitializeMercFactionList(__instance);
            }
        }

        [HarmonyPatch(typeof(SGRoomController_CptQuarters), "CharacterClickedOn", new Type[]{typeof(SimGameState.SimGameCharacterType)})]
        public static class SGRoomController_CptQuarters_CharacterClickedOn
        {
            public static void Prefix(SGRoomController_CptQuarters __instance, SimGameState.SimGameCharacterType characterClicked, ref bool __state)
            {
                //ModState.InitializeMercFactionList(__instance.simState);
                if (characterClicked != SimGameState.SimGameCharacterType.COMMANDER) return;
                var hk = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift);
                if (!hk)
                {
                    __instance.simState.displayedFactions = ModState.simDisplayedFactions;
                    ModInit.modLog?.Info?.Write($"[SGRoomController_CptQuarters_CharacterClickedOn] Setting displayed factions to default ModState.simDisplayedFactions: {string.Join(", ", ModState.simDisplayedFactions)}");
                    return;
                }

                //__state = true;
                __instance.simState.displayedFactions = ModState.simMercFactions;
                ModInit.modLog?.Info?.Write($"[SGRoomController_CptQuarters_CharacterClickedOn] Setting displayed factions to ModState.simMercFactions: {string.Join(", ", ModState.simMercFactions)}");
            }

            // not using this; stupid fuckin AuriganPanelWidget is always on for some reason and i dont care
            //            public static void Postfix(SGRoomController_CptQuarters __instance, SimGameState.SimGameCharacterType characterClicked, ref bool __state, SGCaptainsQuartersReputationScreen ___reputationScreen)
            //            {
            //                if (__state)
            //                {
            //                    var auriganWidget = Traverse.Create(___reputationScreen).Field("AuriganPanelWidget")
            //                        .GetValue<SGFactionReputationWidget>();
            //                    auriganWidget.enabled = false;
            //                    auriganWidget.gameObject.SetActive(false);
            //                }
            //            }
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
                    ModState.OriginalTargetFactionOverride = __instance.Override.targetTeam.Copy();
     //               ModState.OriginalGameContext = __instance.GameContext.CopyOriginalGameContext();
                    //ModState.OriginalGameContext.MemberwiseCopyFrom(__instance.GameContext);

     //               if (ModState.OriginalGameContext.GetObject(GameContextObjectTagEnum
     //                       .TeamTarget) is TeamOverride team)
     //               {
     //                   ModInit.modLog?.Debug?.Write($"[Contract_BeginRequestResources] Check original context target team in state: {team.faction}");
     //               }

                    var mercFaction = Utils.GetMercFactionPoolFromWeight(sim);
                    var contractFactionIDsT = Traverse.Create(__instance).Field("teamFactionIDs");
                    var contractFactionIDs = new Dictionary<string, int>(contractFactionIDsT.GetValue<Dictionary<string, int>>());
                    if (contractFactionIDs.ContainsKey(__instance.Override.targetTeam.teamGuid))
                    {
                        contractFactionIDs[__instance.Override.targetTeam.teamGuid] = mercFaction;
                        contractFactionIDsT.SetValue(contractFactionIDs);
                    }

                    __instance.Override.targetTeam.ReAssignFactionToTeam(contractFactionIDs);
                    ModState.MercFactionOverride = __instance.Override.targetTeam;

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
                        $"[Contract_BeginRequestResources] Set ModState.MercFactionForReplacement to {ModState.MercFactionOverride.faction}, original was {ModState.OriginalTargetFactionOverride?.faction}. TargetTeam is now {__instance.Override.targetTeam.FactionValue.Name}. Reinitializing MissionControl");
                    MissionControl.MissionControl.Instance.SetContract(__instance);
                }
                else if (Utils.ShouldAddMercLance(__instance.Override))

                {
                    var hostileMercLanceFaction = Utils.GetMercFactionPoolFromWeight(sim);
                    ModState.HostileMercLanceTeamOverride.AssignMercFactionToTeamState(hostileMercLanceFaction);

                    //do lance prep stuff? -> probably need to call generate units, etc etc etc
                }
                //maybwe inject friendly mercs here. one thing at a time though.
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
                ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] TAGSPAM {string.Join(", ",tags)}");
                var mercFactionLowerCased = ModState.MercFactionOverride.FactionValue.Name.ToLower();
                if (requiredTags.Contains(mercFactionLowerCased))
                {
                    ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] Found merc faction tag: {mercFactionLowerCased} in requiredTags, should be using merc units");
                    var resultDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                        currentDate, companyTags);
                    if (resultDefs.Count == 0)
                    {
                        requiredTags.Remove(mercFactionLowerCased);
                        requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag);
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] Original result failed, removing merc faction tag: {mercFactionLowerCased} and replacing with fallback tag: {ModInit.modSettings.FallbackUnitFactionTag}");
                        return;
                    }
                    ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] Proceeding using original requiredTags.");
                }
            }
        }

        [HarmonyPatch(typeof(Contract), "ShortDescription", MethodType.Getter)]
        public static class Contract_ShortDescription_Getter
        {
            public static void Prefix(Contract __instance)
            {
                if (ModState.OriginalTargetFactionOverride != null)
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.OriginalTargetFactionOverride);
            }

            public static void Postfix(Contract __instance)
            {
                if (ModState.OriginalTargetFactionOverride != null)
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.MercFactionOverride);
            }
        }

        [HarmonyPatch(typeof(Contract), "LongDescription", MethodType.Getter)]
        public static class Contract_LongDescription_Getter
        {
            public static void Prefix(Contract __instance)
            {
                if (ModState.OriginalTargetFactionOverride != null)
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.OriginalTargetFactionOverride);
            }

            public static void Postfix(Contract __instance)
            {
                if (ModState.OriginalTargetFactionOverride != null)
                    __instance.GameContext.SetObject(GameContextObjectTagEnum.TeamTarget, ModState.MercFactionOverride);
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
                if (ModState.OriginalTargetFactionOverride == null) return;

                //var faction = theSimState.GetFactionDef(ModState.OriginalTargetFactionOverride.faction);

                var faction = UnityGameInstance.BattleTechGame.DataManager.Factions
                    .FirstOrDefault(x => x.Value.FactionValue.Name == ModState.OriginalTargetFactionOverride.FactionValue.Name).Value;

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

                        var idx = ___FactionWidgets.Count-1;
                        __instance.SetWidgetData(idx, faction.FactionValue, repChange, true);
                        theSimState.SetReputation(faction.FactionValue, repChange);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(TeamDefinition), "CreateNewTeamDefinitionList",
            new Type[] {})]
        public static class TeamDefinition_CreateNewTeamDefinitionList
        {
            public static void Postfix(TeamDefinition __instance, ref List<TeamDefinition> __result)
            {
                if (ModState.ActiveContractShouldReplaceLanceWithMercs)
                {
                    __result.Add(Classes.HostileMercTeamDefinition);
                    ModInit.modLog?.Debug?.Write($"[TeamDefinition_CreateNewTeamDefinitionList] Added HostileMercTeamDefinition with GUID {Classes.HostileMercTeamDefinition.GUID}.");
                }

                if (ModState.ActiveContractShouldSpawnAlliedMercs)
                {
                    __result.Add(Classes.FriendlyMercTeamDefinition);
                    ModInit.modLog?.Debug?.Write($"[TeamDefinition_CreateNewTeamDefinitionList] Added FriendlyMercTeamDefinition with GUID {Classes.FriendlyMercTeamDefinition.GUID}.");
                }
            }
        }
    }
}
