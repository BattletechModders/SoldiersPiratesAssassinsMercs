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
                if (ModState.MercFactionTeamOverride != null)
                {
                    numFaced = ModState.MercFactionTeamOverride.GetMercFactionStat(sim);
                    mercFaction = ModState.MercFactionTeamOverride;
                }
                else if (ModState.HostileMercLanceTeamOverride != null)
                {
                    numFaced = ModState.HostileMercLanceTeamOverride.GetMercFactionStat(sim);
                    mercFaction = ModState.HostileMercLanceTeamOverride;
                }
                if (mercFaction == null) return;
                if (!ModInit.modSettings.MercFactionConfigs.ContainsKey(mercFaction.FactionValue.Name)) return;
                var DialogueID = $"Dialogue_SPAM_{mercFaction.FactionValue.Name}";
                //just copypaste from BlueWinds' EDM because she da bomb
                var dialogue = mercFaction.FetchMercDialogue(numFaced, sim);

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
}