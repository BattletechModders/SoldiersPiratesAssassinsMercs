using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using BattleTech.Data;
using BattleTech.Framework;
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
        [HarmonyPatch(typeof(SGRoomController_CptQuarters), "CharacterClickedOn", new Type[]{typeof(SimGameState.SimGameCharacterType)})]
        public static class SGRoomController_CptQuarters_CharacterClickedOn
        {
            public static void Prefix(SGRoomController_CptQuarters __instance, SimGameState.SimGameCharacterType characterClicked, ref bool __state)
            {
                ModState.InitializeMercFactionList(__instance.simState);
                if (characterClicked != SimGameState.SimGameCharacterType.COMMANDER) return;
                var hk = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift);
                if (!hk)
                {
                    __instance.simState.displayedFactions = ModState.simDisplayedFactions;
                    ModInit.modLog?.Info?.Write($"[SGRoomController_CptQuarters_CharacterClickedOn] Setting displayed factions to default ModState.simDisplayedFactions: {string.Join(", ", ModState.simDisplayedFactions)}");
                    return;
                }

                __state = true;
                __instance.simState.displayedFactions = ModState.simMercFactions;
                ModInit.modLog?.Info?.Write($"[SGRoomController_CptQuarters_CharacterClickedOn] Setting displayed factions to ModState.simMercFactions: {string.Join(", ", ModState.simMercFactions)}");
            }

            public static void Postfix(SGRoomController_CptQuarters __instance, SimGameState.SimGameCharacterType characterClicked, ref bool __state, SGCaptainsQuartersReputationScreen ___reputationScreen)
            {
                if (__state)
                {
                    var auriganWidget = Traverse.Create(___reputationScreen).Field("AuriganPanelWidget")
                        .GetValue<SGFactionReputationWidget>();
                    auriganWidget.gameObject.SetActive(false);
                }
            }
        }

        [HarmonyPatch(typeof(ContractOverride), "GenerateUnits",
            new Type[] {typeof(DataManager), typeof(DateTime?), typeof(TagSet)})] // probably need to give missionControl optionallydependson so this fires first?

        // if i need to use MissionControl dependency, then this should all shift to Contract.BeginRequestResources maybe
        public static class ContractOverride_GenerateUnits
        {
            public static void Prefix(ContractOverride __instance, DataManager dataManager, DateTime? currentDate, TagSet companyTags)
            {
                var sim = UnityGameInstance.BattleTechGame.Simulation;
                if (sim == null) return;
                if (Utils.ShouldReplaceOpforWithMercs(__instance))
                {
                    var mercFaction = Utils.GetMercFactionPoolFromWeight(sim);
                    var contractFactionIDs = Traverse.Create(__instance.contract).Field("teamFactionIDs")
                        .GetValue<Dictionary<string, int>>();
                    if (contractFactionIDs.ContainsKey(__instance.targetTeam.teamGuid))
                    {
                        contractFactionIDs[__instance.targetTeam.teamGuid] = mercFaction;
                    }
                    __instance.targetTeam.AssignFactionToTeamPublic(contractFactionIDs);
                    ModState.MercFactionForReplacement = FactionEnumeration.GetFactionByID(mercFaction).Name;
                    ModInit.modLog?.Info?.Write(
                        $"[ContractOverride_GenerateUnits] Set ModState.MercFactionForReplacement to {ModState.MercFactionForReplacement}");
                }
            }
        }

        [HarmonyPatch(typeof(TagSetQueryExtensions), "GetMatchingUnitDefs",
            new Type[]
            {
                typeof(MetadataDatabase), typeof(TagSet), typeof(TagSet), typeof(bool), typeof(DateTime?), typeof(TagSet)
            })] // probably need to give RTS optionallydependson so this fires first? no, be
        public static class TagSetQueryExtensions_GetMatchingUnitDefs
        {
            public static void Prefix(MetadataDatabase mdd, ref TagSet requiredTags, TagSet excludedTags, bool checkOwnership, DateTime? currentDate, TagSet companyTags)
            {
                if (requiredTags.Contains(ModState.MercFactionForReplacement))
                {
                    ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] Found merc faction tag: {ModState.MercFactionForReplacement} in requiredTags, should be using merc units");
                    var resultDefs = Utils.GetMatchingUnitDefsOriginal(mdd, requiredTags, excludedTags, checkOwnership,
                        currentDate, companyTags);
                    if (resultDefs.Count == 0)
                    {
                        requiredTags.Remove(ModState.MercFactionForReplacement);
                        requiredTags.Add(ModInit.modSettings.FallbackUnitFactionTag);
                        ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] Original result failed, removing merc faction tag: {ModState.MercFactionForReplacement} and replacing with fallback tag: {ModInit.modSettings.FallbackUnitFactionTag}");
                        return;
                    }
                    ModInit.modLog?.Info?.Write($"[TagSetQueryExtensions_GetMatchingUnitDefs] Proceeding using original requiredTags.");
                }
            }
        }
    }
}
