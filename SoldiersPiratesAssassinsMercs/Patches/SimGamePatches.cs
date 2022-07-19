using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using BattleTech.UI;
using Harmony;
using SoldiersPiratesAssassinsMercs.Framework;
using UnityEngine;

namespace SoldiersPiratesAssassinsMercs.Patches
{
    public class SimGamePatches
    {
        [HarmonyPatch(typeof(SimGameState), "AddCachedFactionsToDisplayList",
            new Type[] { })]
        public static class SimGameState_AddCachedFactionsToDisplayList
        {
            public static void Postfix(SimGameState __instance)
            {
                ModState.InitializeMercFactionList(__instance);
            }
        }

        [HarmonyPatch(typeof(SGRoomController_CptQuarters), "CharacterClickedOn", new Type[] { typeof(SimGameState.SimGameCharacterType) })]
        public static class SGRoomController_CptQuarters_CharacterClickedOn
        {
            static bool Prepare() => false; // disabled, unneeded
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
    }
}
