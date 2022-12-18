using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using BattleTech.Designed;
using Harmony;
using MissionControl.Logic;
using SoldiersPiratesAssassinsMercs.Framework;

namespace SoldiersPiratesAssassinsMercs.Patches
{
    public class CombatGamePatches
    {
        [HarmonyPatch(typeof(CombatGameState), "OnCombatGameDestroyed")]
        public static class CombatGameState_OnCombatGameDestroyed
        {
            public static void Postfix(CombatGameState __instance)
            {
                ModState.ResetStateAfterContract();
            }
        }

        [HarmonyPatch(typeof(DestroyLanceObjective), "UpdateCounts")]
        public static class DestroyLanceObjective_UpdateCounts
        {
            static bool Prepare() => false;
            public static void Postfix(DestroyLanceObjective __instance)
            {
                if (ModState.HostileMercLanceTeamOverride != null)
                {
                    var targetUnits = __instance.GetTargetUnits();
                    var despawnedActors= targetUnits.FindAll(x => x is Mech mech && mech.WasDespawned && x.EncounterTags.Contains(Tags.ADDITIONAL_LANCE));
                    if (despawnedActors.Count > 0)
                    {
                        __instance.lanceActorsDead += despawnedActors.Count;
                    }
                }
            }
        }
    }
}
