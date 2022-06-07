using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using Harmony;
using SoldiersPiratesAssassinsMercs.Framework;

namespace SoldiersPiratesAssassinsMercs.Patches
{
    internal class Combatgame
    {
        [HarmonyPatch(typeof(CombatGameState), "OnCombatGameDestroyed")]
        public static class CombatGameState_OnCombatGameDestroyed
        {
            private static void Postfix(CombatGameState __instance)
            {
                ModState.ResetStateAfterContract();
            }
        }
    }
}
