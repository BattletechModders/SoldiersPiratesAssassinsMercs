using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using BattleTech.Framework;
using Harmony;
using MissionControl.Data;
using MissionControl.Logic;
using MissionControl.Rules;
using MissionControl.Trigger;
using SoldiersPiratesAssassinsMercs.Framework;

namespace SoldiersPiratesAssassinsMercs.Patches
{
    public class MissionControlPatches
    {
        [HarmonyPatch(typeof(AddTargetLanceWithDestroyObjectiveBatch), MethodType.Constructor, new Type[] { typeof(EncounterRules), typeof(string), typeof(SceneManipulationLogic.LookDirection), typeof(float), typeof(float), typeof(string), typeof(int), typeof(bool), typeof(bool), typeof(bool), typeof(bool), typeof(List<string>), typeof(MLanceOverride) })]
        public static class AddTargetLanceWithDestroyObjectiveBatch_Constructor
        {
            public static bool Prefix(AddTargetLanceWithDestroyObjectiveBatch __instance, EncounterRules encounterRules, string orientationTargetKey, SceneManipulationLogic.LookDirection lookDirection, float mustBeBeyondDistance, float mustBeWithinDistance, string objectiveName, int priority, bool isPrimaryObjective, bool displayToUser, bool showObjectiveOnLanceDetected, bool excludeFromAutocomplete, List<string> lanceTags, MLanceOverride manuallySpecifiedLance = null)
            {
                if (!ModState.ActiveContractShouldReplaceLanceWithMercs) return true;
                ModInit.modLog?.Debug?.Write($"[AddTargetLanceWithDestroyObjectiveBatch_Constructor] Running SPAM for AddLanceToMercTeam. Contract should be spawning support lances?");
                int numberOfUnitsInLance = 4;
                string lanceGuid = Guid.NewGuid().ToString();
                //string contractObjectiveGuid = Guid.NewGuid().ToString(); //unused
                string objectiveGuid = Guid.NewGuid().ToString();
                List<string> unitGuids = encounterRules.GenerateGuids(numberOfUnitsInLance);
                string targetTeamGuid = ModState.HostileMercTeamDefinition.TeamDefinitionGuid;
                string spawnerName = $"Lance_Enemy_OpposingForce_{lanceGuid}";

                encounterRules.EncounterLogic.Add(new Classes.AddLanceToMercTeam(lanceGuid, unitGuids, manuallySpecifiedLance));
                encounterRules.EncounterLogic.Add(new AddDestroyWholeUnitChunk(encounterRules, targetTeamGuid, lanceGuid, unitGuids,
                    spawnerName, objectiveGuid, objectiveName, priority, isPrimaryObjective, displayToUser, lanceTags));
                if (!excludeFromAutocomplete) encounterRules.EncounterLogic.Add(new AddObjectiveToAutocompleteTrigger(objectiveGuid));
                encounterRules.EncounterLogic.Add(new SpawnLanceMembersAroundTarget(encounterRules, spawnerName, orientationTargetKey,
                    SpawnLogic.LookDirection.AWAY_FROM_TARGET, mustBeBeyondDistance, mustBeWithinDistance));

                if (showObjectiveOnLanceDetected)
                {
                    encounterRules.EncounterLogic.Add(new ShowObjectiveTrigger(MessageCenterMessageType.OnLanceDetected, lanceGuid, objectiveGuid, false));
                }
                encounterRules.ObjectReferenceQueue.Add(spawnerName);
                return false;
            }
        }

        [HarmonyPatch(typeof(ContractOverrideExtensions), "GetTeamOverrideLanceBelongsTo", new Type[]{typeof(ContractOverride), typeof(string)})]
        public static class ContractOverrideExtensions_GetTeamOverrideLanceBelongsTo
        {
            public static void Postfix(ContractOverride contractOverride, string lanceGuid, ref TeamOverride __result)
            {
                if (__result == null)
                {
                    if (ModState.HostileMercLanceTeamOverride != null)
                    {
                        var mercTeamOverride = ModState.HostileMercLanceTeamOverride;
                        if (mercTeamOverride.IsLanceInTeam(lanceGuid))
                        {
                            __result = mercTeamOverride;
                        }
                    }
                }
            }
        }
    }
}
