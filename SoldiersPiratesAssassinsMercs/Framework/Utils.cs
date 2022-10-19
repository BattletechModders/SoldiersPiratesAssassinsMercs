using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BattleTech;
using BattleTech.Data;
using BattleTech.Framework;
using BattleTech.StringInterpolation;
using HBS.Collections;
using UIWidgets;
using UnityEngine;
using Random = System.Random;

namespace SoldiersPiratesAssassinsMercs.Framework
{
    public static class Utils
    {
        public static List<UnitDef_MDD> GetMatchingUnitDefsOriginal(MetadataDatabase mdd, TagSet requiredTags, TagSet excludedTags, bool checkOwnership, DateTime? currentDate, TagSet companyTags)
        {
            List<UnitDef_MDD> matchingDataByTagSet = mdd.GetMatchingDataByTagSet<UnitDef_MDD>(TagSetType.UnitDef, requiredTags, excludedTags, "UnitDef", "", checkOwnership, "UnitDefID");
            matchingDataByTagSet.RemoveAll((UnitDef_MDD unitDef) => !mdd.CanRandomlySelectUnitDef(unitDef, currentDate, companyTags));
            return matchingDataByTagSet;
        }

        public static void ReAssignFactionToTeam(this TeamOverride team, Dictionary<string, int> teamFactions)
        {
            if (teamFactions.ContainsKey(team.teamGuid))
            {
                FactionValue factionByID = FactionEnumeration.GetFactionByID(teamFactions[team.teamGuid]);
                team.faction = factionByID.Name;
                ModState.HostileMercTeamDefinition.FactionValue = factionByID;
            }
        }

        public static void ProcessHeraldryLoadRequest(this TeamOverride teamOverride, SimGameState sim)
        {
            var factionDef = teamOverride.FactionDef;
            LoadRequest loadRequest = sim.DataManager.CreateLoadRequest(delegate(LoadRequest request)
            {
                var heraldryDef = HeraldryDef.GetHeraldryDefForFactionDef(sim.DataManager, factionDef);
                ProcessHeraldryResources(sim, heraldryDef, null);
            }, false);
            loadRequest.ProcessRequests(10U);
        }

        public static void ProcessHeraldryResources(SimGameState sim, HeraldryDef heraldryDef, Action loadCompleteCallback = null)
        {
            LoadRequest loadRequest = sim.DataManager.CreateLoadRequest(delegate (LoadRequest request)
            {
                loadCompleteCallback?.Invoke();
            }, false);
            loadRequest.AddBlindLoadRequest(BattleTechResourceType.Texture2D, heraldryDef.textureLogoID, new bool?(false));
            loadRequest.AddBlindLoadRequest(BattleTechResourceType.Sprite, heraldryDef.textureLogoID, new bool?(false));
            loadRequest.AddBlindLoadRequest(BattleTechResourceType.ColorSwatch, heraldryDef.primaryMechColorID, new bool?(false));
            loadRequest.AddBlindLoadRequest(BattleTechResourceType.ColorSwatch, heraldryDef.secondaryMechColorID, new bool?(false));
            loadRequest.AddBlindLoadRequest(BattleTechResourceType.ColorSwatch, heraldryDef.tertiaryMechColorID, new bool?(false));
            loadRequest.ProcessRequests(10U);
        }

        public static void AssignMercFactionToTeamState(this TeamOverride team, int mercFactionKey)
        {
            FactionValue factionByID = FactionEnumeration.GetFactionByID(mercFactionKey);
            team.faction = factionByID.Name;
            ModState.HostileMercTeamDefinition.FactionValue = factionByID;
        }

        public static FactionValue GetFactionValueFromString(string factionID)
        {
            FactionValue result = FactionEnumeration.GetInvalidUnsetFactionValue();
            if (!string.IsNullOrEmpty(factionID))
            {
                result = FactionEnumeration.GetFactionByName(factionID);
            }
            return result;
        }
        
        public static int GetMercFactionPoolFromWeight(SimGameState sim, string targetTeam)
        {
            var factionValueInt = -1;
            var factionPool = new List<int>();
            foreach (var mercFaction in ModInit.modSettings.MercFactionConfigs)
            {
                if (mercFaction.Value.EmployerBlacklist.Contains(targetTeam)) continue;
                ModInit.modLog?.Trace?.Write(
                    $"[GetMercFactionPoolFromWeight] Processing weight for Merc group: {mercFaction.Value.MercFactionName}");
                var factionValue = GetFactionValueFromString(mercFaction.Value.MercFactionName);
                //var rep = sim.GetReputation(factionValue);
                var weight = mercFaction.Value.AppearanceWeight;
                for (int i = 0; i < weight; i++)
                {
                    factionPool.Add(factionValue.ID);
                    ModInit.modLog?.Trace?.Write(
                        $"[GetMercFactionPoolFromWeight] Added {factionValue.Name} to factionPool with ID {factionValue.ID}");
                }
            }
            if (factionPool.Count > 0)
            {
                factionValueInt = factionPool.GetRandomElement();
            }
            ModInit.modLog?.Info?.Write($"[GetMercFactionPoolFromWeight] Selected ID {factionValueInt}");
            return factionValueInt;
        }

        public static bool ShouldReplaceOpforWithMercs(ContractOverride contractOverride)
        {
            var chance = ModInit.modSettings.OpforReplacementConfig.BaseReplaceChance;
            if (ModInit.modSettings.OpforReplacementConfig.BlacklistContractIDs.Contains(contractOverride.ID))
            {
                ModInit.modLog?.Trace?.Write($"[ShouldReplaceOpforWithMercs] {contractOverride.ID} is blacklisted, not replacing");
                return false;
            }

            if (ModInit.modSettings.OpforReplacementConfig.BlacklistContractTypes.Contains(contractOverride
                    .ContractTypeValue.Name))
            {
                ModInit.modLog?.Trace?.Write($"[ShouldReplaceOpforWithMercs] {contractOverride.ContractTypeValue.Name} is blacklisted, not replacing");
                return false;
            }
            if (ModInit.modSettings.OpforReplacementConfig.FactionsReplaceOverrides.ContainsKey(contractOverride
                    .targetTeam.faction))
            {
                chance = ModInit.modSettings.OpforReplacementConfig.FactionsReplaceOverrides[contractOverride.targetTeam.faction];
            }
            var roll = ModInit.Random.NextDouble();
            ModInit.modLog?.Info?.Write($"[ShouldReplaceOpforWithMercs] Roll {roll} < chance {chance}? {roll < chance}");
            if (roll < chance) return true;
            return false;
        }
        public static bool ShouldAddMercLance(ContractOverride contractOverride)
        {
            var chance = ModInit.modSettings.MercLanceAdditionConfig.BaseReplaceChance;
            if (ModInit.modSettings.MercLanceAdditionConfig.BlacklistContractIDs.Contains(contractOverride.ID))
            {
                ModInit.modLog?.Trace?.Write($"[ShouldAddMercLance] {contractOverride.ID} is blacklisted, not replacing");
                return false;
            }

            if (ModInit.modSettings.MercLanceAdditionConfig.BlacklistContractTypes.Contains(contractOverride
                    .ContractTypeValue.Name))
            {
                ModInit.modLog?.Trace?.Write($"[ShouldAddMercLance] {contractOverride.ContractTypeValue.Name} is blacklisted, not replacing");
                return false;
            }
            if (ModInit.modSettings.MercLanceAdditionConfig.FactionsReplaceOverrides.ContainsKey(contractOverride
                    .targetTeam.faction))
            {
                chance = ModInit.modSettings.MercLanceAdditionConfig.FactionsReplaceOverrides[contractOverride.targetTeam.faction];
            }
            var roll = ModInit.Random.NextDouble();
            ModInit.modLog?.Info?.Write($"[ShouldAddMercLance] Roll {roll} < chance {chance}? {roll < chance}");
            if (roll < chance)
            {
                ModState.ActiveContractShouldReplaceLanceWithMercs = true;
                return true;
            }
            return false;

        }

        public static void UpdateMercFactionStats(this TeamOverride teamOverride, SimGameState sim)
        {
            var statNameF = $"{teamOverride.FactionValue.Name}_OpHistory_Count";
            var statNameSys = $"{teamOverride.FactionValue.Name}_OpHistory_Name";
            if (!sim.CompanyStats.ContainsStatistic(statNameF))
            {
                sim.CompanyStats.AddStatistic<int>(statNameF, 1);

            }
            else
            {
                sim.CompanyStats.ModifyStat("ContractResolution", -1, statNameF,
                    StatCollection.StatOperation.Int_Add, 1);
            }
            if (!sim.CompanyStats.ContainsStatistic(statNameSys))
            {
                sim.CompanyStats.AddStatistic<string>(statNameSys, sim.CurSystem.Name);

            }
            else
            {
                sim.CompanyStats.ModifyStat("ContractResolution", -1, statNameF,
                    StatCollection.StatOperation.Set, sim.CurSystem.Name);
            }
        }

        public static int GetMercFactionStat(this TeamOverride teamOverride, SimGameState sim)
        {
            var statName = $"{teamOverride.FactionValue.Name}_OpHistory_Count";
            if (!sim.CompanyStats.ContainsStatistic(statName))
            {
                return 0;
            }
            else
            {
                return sim.CompanyStats.GetValue<int>(statName);
            }
        }
        public static string GetMercFactionLastSystem(this TeamOverride teamOverride, SimGameState sim)
        {
            var statNameSys = $"{teamOverride.FactionValue.Name}_OpHistory_Name";
            if (!sim.CompanyStats.ContainsStatistic(statNameSys))
            {
                return "";
            }
            else
            {
                return sim.CompanyStats.GetValue<string>(statNameSys);
            }
        }

        public static DialogueContent[] FetchMercDialogue(this TeamOverride mercTeam, int timesFaced, SimGameState sim)
        {
            var config = ModInit.modSettings.MercFactionConfigs[mercTeam.FactionValue.Name];
            var DialogueBucket = new List<Classes.MercDialogue>();
            foreach (var attribute in config.PersonalityAttributes)
            {
                if (ModState.DialogueStrings.ContainsKey(attribute))
                {
                    foreach (var DialogueHolder in ModState.DialogueStrings[attribute])
                    {
                        if (timesFaced >= DialogueHolder.MinTimesEncountered && timesFaced <= DialogueHolder.MaxTimesEncountered)
                        {
                            DialogueBucket.Add(DialogueHolder);
                        }
                    }
                }
            }
            var chosenDialogueHolder = DialogueBucket.GetRandomElement();
            ModState.ChosenDialogueBribeMulti = chosenDialogueHolder.BribeAcceptanceMultiplier;
            var chosenDialogue = chosenDialogueHolder.Dialogue.GetRandomElement();
            chosenDialogue.InterpolateSPAMDialogue(mercTeam, sim);
            var interpolated = Interpolator.Interpolate(chosenDialogue, sim.Context, true);
            var contents = new DialogueContent(interpolated, Color.white, mercTeam.teamLeaderCastDefId, null, null,
                DialogCameraDistance.Medium, DialogCameraHeight.Default, -1f);

            DialogueContent[] content = Array.Empty<DialogueContent>();
            content[0] = contents;
            return content;
        }

        public static string InterpolateSPAMDialogue(this string dialogue, TeamOverride teamOverride, SimGameState sim)
        {
            if (string.IsNullOrEmpty(dialogue))
            {
                return string.Empty;
            }
            var interp = dialogue.Replace("{RCNT_SYSTEM}", teamOverride.GetMercFactionLastSystem(sim));
            //add other interps here
            return interp;
        }
    }
}




// assign new AI team via TeamDefinition ExtraTeamDefinitionGuidNames so extra merc team will be added by BuildItemRegistry at contract start // will need to make a GUID and keep it consistent

// define new LanceLogic that is AddLanceToTargetTeam, but instead adds to magic merc team

//MC -> may need to patch AddTargetLanceWithDestroyObjectiveBatch to inject new team

//will need to generate new TeamOverride -> ModStateHostileMercLanceTeamOverride
