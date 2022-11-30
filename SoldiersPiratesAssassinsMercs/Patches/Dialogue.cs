using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using BattleTech.Data;
using BattleTech.Designed;
using BattleTech.Framework;
using BattleTech.StringInterpolation;
using BattleTech.UI;
using Harmony;
using HBS;
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
using static DamageAssetGroup;
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
                TeamOverride mercFaction = null;
                Team mercTeam = null;
                if (ModState.MercFactionTeamOverride != null)
                {
                    numFaced = ModState.MercFactionTeamOverride.GetMercFactionStat(sim);
                    mercFaction = ModState.MercFactionTeamOverride;
                    mercTeam = __instance.Combat.Teams.First(x => x.GUID == "be77cadd-e245-4240-a93e-b99cc98902a5");
                }
                else if (ModState.HostileMercLanceTeamOverride != null)
                {
                    numFaced = ModState.HostileMercLanceTeamOverride.GetMercFactionStat(sim);
                    mercFaction = ModState.HostileMercLanceTeamOverride;
                    mercTeam = __instance.Combat.Teams.First(x => x.GUID == "ddfd570d-f9e4-42f8-b2e8-671eb1e8f43a");
                }

                if (mercFaction == null) return;
                if (!ModInit.modSettings.MercFactionConfigs.ContainsKey(mercFaction.FactionValue.Name)) return;
                var DialogueID = $"Dialogue_SPAM_{mercFaction.FactionValue.Name}";
                //just copypaste from BlueWinds' EDM because she da bomb
                var dialogue = mercFaction.FetchMercDialogue(mercTeam, numFaced, sim);

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
            if (ModState.HostileMercLanceTeamOverride != null)
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
            if (__instance.ActiveTurnActor is Team team && team.IsLocalPlayer && ModState.RoundsInCombat > 1 && !ModState.HasBribeBeenAttempted)
            {
                Tuple<float, int>[] results;
                Team mercTeam;
                var description = Utils.CalculateBribeCostAndSuccess(__instance.Combat, out results, out mercTeam);
                //do bribey popup thinger here
                ModState.HasBribeBeenAttempted = true;
                var companyFunds = sim.Funds;
                var popup = GenericPopupBuilder
                    .Create("Attempt to bribe hostile mercs?", description)
                    .AddFader(new UIColorRef?(LazySingletonBehavior<UIManager>.Instance.UILookAndColorConstants
                        .PopupBackfill));
                popup.AlwaysOnTop = true;
                popup.AddButton("0%", (Action)(() => { }));
                if (results[1].Item2 < companyFunds)
                {
                    popup.AddButton("25%.", (Action)(() =>
                    {
                        var roll = Utils.ProcessBribeRoll(results[1], mercTeam);

                    }));
                }

                if (results[2].Item2 < companyFunds)
                {
                    popup.AddButton("50%.", (Action)(() =>
                    {
                        var roll2 = Utils.ProcessBribeRoll(results[2], mercTeam);

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
            if (ModState.MercFactionTeamOverride != null || ModState.HostileMercLanceTeamOverride != null)
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