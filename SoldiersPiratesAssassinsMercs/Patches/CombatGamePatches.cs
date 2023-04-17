using BattleTech;
using BattleTech.Designed;
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
            static bool Prepare() => false; // disabled
            public static void Postfix(DestroyLanceObjective __instance)
            {
                if (ModState.HostileMercLanceTeamOverride.TeamOverride != null)
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
