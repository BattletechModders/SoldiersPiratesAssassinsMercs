using System;
using System.Linq;
using BattleTech;
using BattleTech.Designed;
using BattleTech.Framework;
using BattleTech.UI;
using HBS;
using SoldiersPiratesAssassinsMercs.Framework;
using UnityEngine;
using ModState = SoldiersPiratesAssassinsMercs.Framework.ModState;

namespace SoldiersPiratesAssassinsMercs.Patches
{
    public class Dialogue
    {
        [HarmonyPatch(typeof(TurnDirector), "QueuePilotChatter")]
        public static class TurnDirector_QueuePilotChatter
        {
            public static void Prefix(TurnDirector __instance)
            {
                var sim = UnityGameInstance.BattleTechGame.Simulation;
                if (sim == null) return;
                var numFaced = 0;
                TeamOverride chattyFaction = null;
                Team dialogueTeam = null;
                DialogueContent[] dialogue = Array.Empty<DialogueContent>();
                if (ModState.PlanetAltFactionTeamOverride.TeamOverride != null)
                {
                    chattyFaction = ModState.PlanetAltFactionTeamOverride.TeamOverride;
                    dialogueTeam = __instance.Combat.Teams.First(x => x.GUID == "be77cadd-e245-4240-a93e-b99cc98902a5"); // targetteam GUID, const
                    chattyFaction.TryFetchGenericDialogue(dialogueTeam, numFaced, sim, out dialogue);
                }
                else if (ModState.AltFactionFactionTeamOverride.TeamOverride != null)
                {
                    chattyFaction = ModState.AltFactionFactionTeamOverride.TeamOverride;
                    dialogueTeam = __instance.Combat.Teams.First(x => x.GUID == "be77cadd-e245-4240-a93e-b99cc98902a5"); // targetteam GUID, const
                    chattyFaction.TryFetchGenericDialogue(dialogueTeam, numFaced, sim, out dialogue);
                }

                //merc dialogue only here
                else if (ModState.MercFactionTeamOverride.TeamOverride != null)
                {
                    numFaced = ModState.MercFactionTeamOverride.TeamOverride.GetMercFactionStat(sim);
                    chattyFaction = ModState.MercFactionTeamOverride.TeamOverride;
                    dialogueTeam = __instance.Combat.Teams.First(x => x.GUID == "be77cadd-e245-4240-a93e-b99cc98902a5"); // targetteam GUID, const
                    chattyFaction.TryFetchMercDialogue(dialogueTeam, numFaced, sim, out dialogue);
                }
                else if (MissionControl.MissionControl.Instance.Metrics.NumberOfTargetAdditionalLances > 0)
                {
                    if (ModState.HostileMercLanceTeamOverride.TeamOverride != null)
                    {
                        numFaced = ModState.HostileMercLanceTeamOverride.TeamOverride.GetMercFactionStat(sim);
                        chattyFaction = ModState.HostileMercLanceTeamOverride.TeamOverride;
                        dialogueTeam = __instance.Combat.Teams.First(x => x.GUID == GlobalVars.HostileMercLanceTeamDefinitionGUID);
                        chattyFaction.TryFetchMercDialogue(dialogueTeam, numFaced, sim, out dialogue);
                    }

                    else if (ModState.HostileToAllLanceTeamOverride.TeamOverride != null)
                    {
                        numFaced = ModState.HostileToAllLanceTeamOverride.TeamOverride.GetMercFactionStat(sim);
                        chattyFaction = ModState.HostileToAllLanceTeamOverride.TeamOverride;
                        dialogueTeam = __instance.Combat.Teams.First(x => x.GUID == GlobalVars.HostileToAllLanceTeamDefinitionGUID);
                        chattyFaction.TryFetchGenericDialogue(dialogueTeam, numFaced, sim, out dialogue);
                    }

                    else if (ModState.HostileAltLanceTeamOverride.TeamOverride != null)
                    {
                        numFaced = ModState.HostileAltLanceTeamOverride.TeamOverride.GetMercFactionStat(sim);
                        chattyFaction = ModState.HostileAltLanceTeamOverride.TeamOverride;
                        dialogueTeam = __instance.Combat.Teams.First(x => x.GUID == GlobalVars.HostileAltLanceTeamDefinitionGUID);
                        chattyFaction.TryFetchGenericDialogue(dialogueTeam, numFaced, sim, out dialogue);
                    }
                }

                //if (!ModInit.modSettings.MercFactionConfigs.ContainsKey(chattyFaction.FactionValue.Name)) return;

                if (dialogue == Array.Empty<DialogueContent>()) return;
                var DialogueID = $"Dialogue_SPAM_{chattyFaction.FactionValue.Name}";
                //just copypaste from BlueWinds' EDM because she da bomb
                

                ModInit.modLog.Info?.Write($"[TurnDirector_QueuePilotChatter] Displaying {DialogueID}");

                GameObject encounterLayerGameObject = __instance.Combat.EncounterLayerData.gameObject;
                GameObject dialogChunk = new GameObject($"Chunk_{DialogueID}");
                dialogChunk.transform.parent = encounterLayerGameObject.transform;
                dialogChunk.transform.localPosition = Vector3.zero;

                DialogueChunkGameLogic dialogueChunkGameLogic = dialogChunk.AddComponent<DialogueChunkGameLogic>();
                dialogueChunkGameLogic.encounterObjectName = $"Chunk_{DialogueID}";
                dialogueChunkGameLogic.encounterObjectGuid = System.Guid.NewGuid().ToString();

                GameObject go = new GameObject(DialogueID);
                go.transform.parent = dialogChunk.transform;
                go.transform.localPosition = Vector3.zero;

                DialogueGameLogic dgl = go.AddComponent<DialogueGameLogic>();
                dgl.conversationContent = new ConversationContent(DialogueID, dialogue);
                dgl.conversationContent.ContractInitialize(__instance.Combat);
                dgl.SetCombat(__instance.Combat);
                dgl.GenerateNewGuid();
                __instance.Combat.ItemRegistry.AddItem(dgl);

                dgl.TriggerDialogue(true);
                ////
            }
        }
    }

    [HarmonyPatch(typeof(Team), "OnRoundEnd")]
    public static class TurnActor_OnRoundEnd
    {
        public static void Postfix(Team __instance)
        {
            if (MissionControl.MissionControl.Instance.Metrics.NumberOfTargetAdditionalLances < 1) return; 
            if( ModState.HostileMercLanceTeamOverride.TeamOverride != null)
            {
                if (ModState.RoundsInCombat <= 1 && __instance.IsLocalPlayer && __instance.Combat.TurnDirector.IsInterleaved)
                {
                    ModState.RoundsInCombat++;
                    ModInit.modLog.Info?.Write($"[TurnActor_OnRoundEnd] Incremented rounds in combat. Now {ModState.RoundsInCombat}");
                }
            }
        }
    }

    [HarmonyPatch(typeof(TurnDirector), "IncrementActiveTurnActor")]
    public static class TurnDirector_IncrementActiveTurnActor
    {
        public static void Postfix(TurnDirector __instance)
        {
            var sim = UnityGameInstance.BattleTechGame.Simulation;
            if (sim == null) return;
            if (MissionControl.MissionControl.Instance.Metrics.NumberOfTargetAdditionalLances < 1) return;
            if (__instance.ActiveTurnActor is Team team && team.IsLocalPlayer && ModState.RoundsInCombat > 1 && !ModState.HasBribeBeenAttempted)
            {
                Tuple<float, int>[] results;
                Team mercTeam;
                var description = Utils.CalculateBribeCostAndSuccess(sim, __instance.Combat, out results, out mercTeam);
                //do bribey popup thinger here
                ModState.HasBribeBeenAttempted = true;
                var companyFunds = sim.Funds;
                var popup = GenericPopupBuilder
                    .Create("Attempt to bribe hostile mercs?", description)
                    .AddFader(new UIColorRef?(LazySingletonBehavior<UIManager>.Instance.UILookAndColorConstants
                        .PopupBackfill));
                popup.AlwaysOnTop = true;
                popup.AddButton("0%", (Action)(() => { }));

                if (results[2].Item2 < companyFunds)
                {
                    popup.AddButton("50%.", (Action)(() =>
                    {
                        var roll2 = Utils.ProcessBribeRoll(results[2], mercTeam);

                    }));
                }

                if (results[1].Item2 < companyFunds)
                {
                    popup.AddButton("25%.", (Action)(() =>
                    {
                        var roll = Utils.ProcessBribeRoll(results[1], mercTeam);

                    }));
                }

                if (results[3].Item2 < companyFunds)
                {
                    popup.AddButton("75%.", (Action)(() =>
                    {
                        var roll3 = Utils.ProcessBribeRoll(results[3], mercTeam);

                    }));
                }

                if (results[4].Item2 < companyFunds)
                {
                    popup.AddButton("100%.", (Action)(() =>
                    {
                        var roll4 = Utils.ProcessBribeRoll(results[4], mercTeam);

                    }));
                }
                popup.CancelOnEscape();
                popup.Render();
            }
        }
    }


    //add bribe ability. disabled
    [HarmonyPatch(typeof(Team), "AddUnit", new Type[] {typeof(AbstractActor)})]
    public static class Team_AddUnit_Patch
    {
        static bool Prepare() => false; // disable, not doing ability?
        public static void Postfix(Team __instance, AbstractActor unit)
        {
            if (MissionControl.MissionControl.Instance.Metrics.NumberOfTargetAdditionalLances < 1) return;
            if (ModState.HostileMercLanceTeamOverride.TeamOverride != null)
            {
                if (__instance.IsLocalPlayer && unit.GetPilot().IsPlayerCharacter)
                {
                    if (!string.IsNullOrEmpty(ModInit.modSettings.BribeAbility))
                    {
                        if (unit.GetPilot().Abilities
                                .All(x => x.Def.Id != ModInit.modSettings.BribeAbility) &&
                            unit.ComponentAbilities.All(y =>
                                y.Def.Id != ModInit.modSettings.BribeAbility))
                        {
                            unit.Combat.DataManager.AbilityDefs.TryGet(ModInit.modSettings.BribeAbility,
                                out var def);
                            var ability = new Ability(def);
                            ModInit.modLog?.Trace?.Write(
                                $"[Team.AddUnit] Adding {ability.Def?.Description?.Id} to {unit.Description?.Name}.");
                            ability.Init(unit.Combat);
                            unit.GetPilot().Abilities.Add(ability);
                            unit.GetPilot().ActiveAbilities.Add(ability);
                        }
                    }
                }
            }
        }
    }

    //maybe need to patch attackmodeselector for description?

    [HarmonyPatch(typeof(Ability), "Activate",
        new Type[] { typeof(AbstractActor), typeof(ICombatant) })]
    public static class Ability_Activate_ICombatant
    {
        static bool Prepare() => false; // disable, not doing ability?
        public static void Postfix(Ability __instance, AbstractActor creator, ICombatant target)
        {
            if (creator == null) return;
            if (UnityGameInstance.BattleTechGame.Combat.ActiveContract.ContractTypeValue.IsSkirmish) return;
            if (MissionControl.MissionControl.Instance.Metrics.NumberOfTargetAdditionalLances < 1) return;
            if (__instance.IsAvailable)
            {
                if (target is AbstractActor targetActor)
                {
                   //do roll, spend money
                }
            }
        }
    }
}