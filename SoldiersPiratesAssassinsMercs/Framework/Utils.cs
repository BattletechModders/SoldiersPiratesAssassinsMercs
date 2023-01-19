using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BattleTech;
using BattleTech.Data;
using BattleTech.Designed;
using BattleTech.Framework;
using BattleTech.StringInterpolation;
using FogOfWar;
using Harmony;
using HBS;
using HBS.Collections;
using MissionControl.Logic;
using UnityEngine;
using us.frostraptor.modUtils.CustomDialog;

namespace SoldiersPiratesAssassinsMercs.Framework
{
    public static class Utils
    {
        public static MethodInfo _despawnActorMethod = AccessTools.Method(typeof(AbstractActor), "DespawnActor");

        public static List<UnitDef_MDD> GetMatchingUnitDefsOriginal(MetadataDatabase mdd, TagSet requiredTags, TagSet excludedTags, bool checkOwnership, DateTime? currentDate, TagSet companyTags)
        {
            List<UnitDef_MDD> matchingDataByTagSet = mdd.GetMatchingDataByTagSet<UnitDef_MDD>(TagSetType.UnitDef, requiredTags, excludedTags, "UnitDef", "", checkOwnership, "UnitDefID");
            matchingDataByTagSet.RemoveAll((UnitDef_MDD unitDef) => !mdd.CanRandomlySelectUnitDef(unitDef, currentDate, companyTags));
            return matchingDataByTagSet;
        }

        public static void ReAssignHostileMercFactionToTeam(this TeamOverride team, Dictionary<string, int> teamFactions, DataManager dm)
        {
            if (teamFactions.ContainsKey(team.teamGuid))
            {
                FactionValue factionByID = FactionEnumeration.GetFactionByID(teamFactions[team.teamGuid]);
                team.faction = factionByID.Name;
                team.dataManager = dm;
                ModState.HostileMercLanceTeamDefinition.FactionValue = factionByID;
            }
        }
        public static void ReAssignHostileAltFactionToTeam(this TeamOverride team, Dictionary<string, int> teamFactions, DataManager dm)
        {
            if (teamFactions.ContainsKey(team.teamGuid))
            {
                FactionValue factionByID = FactionEnumeration.GetFactionByID(teamFactions[team.teamGuid]);
                team.faction = factionByID.Name;
                team.dataManager = dm;
                ModState.HostileAltLanceTeamDefinition.FactionValue = factionByID;
            }
        }

        public static void ReAssignHostileToAllFactionToTeam(this TeamOverride team, Dictionary<string, int> teamFactions, DataManager dm)
        {
            if (teamFactions.ContainsKey(team.teamGuid))
            {
                FactionValue factionByID = FactionEnumeration.GetFactionByID(teamFactions[team.teamGuid]);
                team.faction = factionByID.Name;
                team.dataManager = dm;
                ModState.HostileToAllLanceTeamDefinition.FactionValue = factionByID;
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

        public static void AssignHostileMercFactionToTeamState(this TeamOverride team, int mercFactionKey, DataManager dm)
        {
            FactionValue factionByID = FactionEnumeration.GetFactionByID(mercFactionKey);
            team.faction = factionByID.Name;
            team.dataManager = dm;
            ModState.HostileMercLanceTeamDefinition.FactionValue = factionByID;
        }

        public static void AssignHostileAltFactionToTeamState(this TeamOverride team, int mercFactionKey, DataManager dm)
        {
            FactionValue factionByID = FactionEnumeration.GetFactionByID(mercFactionKey);
            team.faction = factionByID.Name;
            team.dataManager = dm;
            ModState.HostileAltLanceTeamDefinition.FactionValue = factionByID;
        }

        public static void AssignHostileToAllFactionToTeamState(this TeamOverride team, int hostileFactionKey, DataManager dm)
        {
            FactionValue factionByID = FactionEnumeration.GetFactionByID(hostileFactionKey);
            team.faction = factionByID.Name;
            team.dataManager = dm;
            ModState.HostileToAllLanceTeamDefinition.FactionValue = factionByID;
        }

        public static FactionValue GetFactionValueFromString(string factionName)
        {
            FactionValue result = FactionEnumeration.GetInvalidUnsetFactionValue();
            if (!string.IsNullOrEmpty(factionName))
            {
                result = FactionEnumeration.GetFactionByName(factionName);
            }
            return result;
        }
        
        public static int GetMercFactionPoolFromWeight(SimGameState sim, string targetTeam, out string mercFactionFallbackTag)
        {
            var factionValueInt = -1;
            var factionPool = new List<int>();
            foreach (var mercFaction in ModInit.modSettings.MercFactionConfigs)
            {
                if (mercFaction.Value.EmployerBlacklist.Contains(targetTeam)) continue;
                ModInit.modLog?.Trace?.Write(
                    $"[GetMercFactionPoolFromWeight] Processing weight for Merc group: {mercFaction.Key}");
                var factionValue = GetFactionValueFromString(mercFaction.Key);
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
            if (factionValueInt > 0)
            {
                var mercFactionValue = FactionEnumeration.GetFactionByID(factionValueInt);
                if (ModInit.modSettings.MercFactionConfigs.ContainsKey(mercFactionValue.Name))
                {
                    mercFactionFallbackTag = ModInit.modSettings.MercFactionConfigs[mercFactionValue.Name]
                        .MercFactionFallbackTag;
                    return factionValueInt;
                }
            }
            mercFactionFallbackTag = "";
            ModInit.modLog?.Error?.Write($"[GetMercFactionPoolFromWeight] ERROR Getting mercFactionValue from int. Invalid config, probably key in MercFactionConfigs is not faction name");
            return factionValueInt;
        }
        public static bool ShouldReplaceOpforWithPlanetAlternate(ContractOverride contractOverride, out Classes.ConfigOptions.AlternateOpforConfig config)
        {
            config = null;
            var sim = UnityGameInstance.BattleTechGame.Simulation;
            if (sim == null) return false;
            if (ModInit.modSettings.PlanetFactionConfigs.ContainsKey(sim.CurSystem.Def.CoreSystemID))// eg. starsystemdef_Place
            {
                config = ModInit.modSettings.PlanetFactionConfigs[sim.CurSystem.Def.CoreSystemID];
                var roll = ModInit.Random.NextDouble();
                ModInit.modLog?.Info?.Write($"[ShouldReplaceOpforWithPlanetAlternate] Roll {roll} < chance {config.FactionReplaceChance}? {roll < config.FactionReplaceChance}");
                if (roll < config.FactionReplaceChance) return true;
            }
            return false;
        }
        public static bool ShouldReplaceOpforWithFactionAlternate(ContractOverride contractOverride, out Classes.ConfigOptions.AlternateOpforConfig config)
        {
            config = null;
            if (ModInit.modSettings.AlternateFactionConfigs.ContainsKey(contractOverride.targetTeam.faction))
            {
                config = ModInit.modSettings.AlternateFactionConfigs[contractOverride.targetTeam.faction];
                var roll = ModInit.Random.NextDouble();
                ModInit.modLog?.Info?.Write($"[ShouldReplaceOpforWithFactionAlternate] Roll {roll} < chance {config.FactionReplaceChance}? {roll < config.FactionReplaceChance}");
                if (roll < config.FactionReplaceChance) return true;
            }
            return false;
        }

        public static int GetAlternateFactionPoolFromWeight(SimGameState sim, Classes.ConfigOptions.AlternateOpforConfig config, out string fallback)
        {
            var factionValueInt = -1;
            var factionPoolFlattened = new List<Classes.FactionWeightAndFallback>();
            fallback = "";
            foreach (var altFaction in config.AlternateOpforWeights)
            {
                ModInit.modLog?.Trace?.Write(
                    $"[GetAlternateFactionPoolFromWeight] Processing weight for alt faction group: {altFaction.FactionName}");
                //var rep = sim.GetReputation(factionValue);
                var weight = altFaction.FactionWeight;
                for (int i = 0; i < weight; i++)
                {
                    factionPoolFlattened.Add(altFaction);
                    ModInit.modLog?.Trace?.Write(
                        $"[GetAlternateFactionPoolFromWeight] Added {altFaction.FactionName} to factionPool");
                }
            }
            if (factionPoolFlattened.Count > 0)
            {
                var selectedFactionConfig = factionPoolFlattened.GetRandomElement();
                fallback = selectedFactionConfig.FactionFallback;
                factionValueInt = GetFactionValueFromString(selectedFactionConfig.FactionName).ID;
                ModInit.modLog?.Info?.Write($"[GetAlternateFactionPoolFromWeight] Selected faction {selectedFactionConfig.FactionName} with ID {factionValueInt} and fallback {selectedFactionConfig.FactionFallback}");
                return factionValueInt;
            }
            ModInit.modLog?.Error?.Write($"[GetAlternateFactionPoolFromWeight] Selected faction ID {factionValueInt}. Probably not good.");
            return factionValueInt;
        }

        public static bool ShouldReplaceOpforWithMercs(ContractOverride contractOverride)
        {
            var chance = ModInit.modSettings.OpforReplacementConfig.BaseReplaceChance;
            
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

        public static bool ShouldAddAltFactionLance(ContractOverride contractOverride, out Classes.ConfigOptions.AlternateOpforConfig config)
        {
            config = null;
            if (ModInit.modSettings.AlternateFactionConfigs.ContainsKey(contractOverride.targetTeam.faction))
            {
                config = ModInit.modSettings.AlternateFactionConfigs[contractOverride.targetTeam.faction];
                var roll = ModInit.Random.NextDouble();
                ModInit.modLog?.Info?.Write($"[ShouldAddAltFactionLance] Roll {roll} < chance {config.FactionMCAdditionalLanceReplaceChance}? {roll < config.FactionReplaceChance}");
                if (roll < config.FactionMCAdditionalLanceReplaceChance) return true;
            }
            return false;
        }
        public static bool ShouldAddPlanetLance(ContractOverride contractOverride, out Classes.ConfigOptions.AlternateOpforConfig config)
        {
            config = null;
            var sim = UnityGameInstance.BattleTechGame.Simulation;
            if (sim == null) return false;
            if (ModInit.modSettings.PlanetFactionConfigs.ContainsKey(sim.CurSystem.Def.CoreSystemID))
            {
                config = ModInit.modSettings.PlanetFactionConfigs[sim.CurSystem.Def.CoreSystemID];
                var roll = ModInit.Random.NextDouble();
                ModInit.modLog?.Info?.Write($"[ShouldAddPlanetLance] Roll {roll} < chance {config.FactionMCAdditionalLanceReplaceChance}? {roll < config.FactionReplaceChance}");
                if (roll < config.FactionMCAdditionalLanceReplaceChance) return true;
            }
            return false;
        }

        public static bool ShouldAddMercLance(ContractOverride contractOverride)
        {
            var chance = ModInit.modSettings.MercLanceAdditionConfig.BaseReplaceChance;
           
            if (ModInit.modSettings.MercLanceAdditionConfig.FactionsReplaceOverrides.ContainsKey(contractOverride
                    .targetTeam.faction))
            {
                chance = ModInit.modSettings.MercLanceAdditionConfig.FactionsReplaceOverrides[contractOverride.targetTeam.faction];
            }
            var roll = ModInit.Random.NextDouble();
            ModInit.modLog?.Info?.Write($"[ShouldAddMercLance] Roll {roll} < chance {chance}? {roll < chance}");
            if (roll < chance)
            {
                //ModState.ActiveContractShouldReplaceLanceWithMercs = true;
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
                sim.CompanyStats.ModifyStat("ContractResolution", -1, statNameSys,
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
        public static bool TryFetchGenericDialogue(this TeamOverride dialogueTeamOverride, Team dialogueTeam, int timesFaced, SimGameState sim, out DialogueContent[] dialogueContent)
        {
            dialogueContent = Array.Empty<DialogueContent>();
            var combat = UnityGameInstance.BattleTechGame.Combat;
            if (!ModState.GenericDialogueStrings.ContainsKey(dialogueTeamOverride.FactionValue.Name))
                return false;
            var dialogueStrings = ModState.GenericDialogueStrings[dialogueTeamOverride.FactionValue.Name];
            var chosenDialogueRaw = dialogueStrings.GetRandomElement();
            var customInterpedDialogue = InterpolateSPAMDialogue(chosenDialogueRaw, dialogueTeamOverride, sim);
            var interpolated = Interpolator.Interpolate(customInterpedDialogue, sim.Context, true);
            var quips = new List<string> { interpolated };

            CastDef castDef = Coordinator.CreateCast(combat, Guid.NewGuid().ToString(), dialogueTeam);
            DialogueContent content = BuildContent(dialogueTeamOverride, castDef, quips);
            //var contents = new DialogueContent(interpolated, Color.white, mercTeamOverride.teamLeaderCastDefId, null, null,
            //    DialogCameraDistance.Medium, DialogCameraHeight.Default, -1f);
            //Traverse.Create(contents).Field("castDef").SetValue(castDef);
            DialogueContent[] contents = { content };
            //content[0] = contents;
            dialogueContent = contents;
            return true;
        }
        public static bool TryFetchMercDialogue(this TeamOverride mercTeamOverride, Team mercTeam, int timesFaced, SimGameState sim, out DialogueContent[] dialogueContent)
        {
            dialogueContent = Array.Empty<DialogueContent>();
            var combat = UnityGameInstance.BattleTechGame.Combat;
            if (!ModInit.modSettings.MercFactionConfigs.ContainsKey(mercTeamOverride.FactionValue.Name))
                return false;
            var config = ModInit.modSettings.MercFactionConfigs[mercTeamOverride.FactionValue.Name];
            var dialogueBucketBin = new List<Classes.MercDialogueBucket>();
            foreach (var attribute in config.PersonalityAttributes)
            {
                if (ModState.MercDialogueStrings.ContainsKey(attribute))
                {
                    foreach (var dialogueHolder in ModState.MercDialogueStrings[attribute])
                    {
                        if ((dialogueHolder.MinTimesEncountered == -1 || timesFaced >= dialogueHolder.MinTimesEncountered) && (dialogueHolder.MaxTimesEncountered == -1 || timesFaced <= dialogueHolder.MaxTimesEncountered))
                        {
                            dialogueBucketBin.Add(dialogueHolder);
                        }
                    }
                }
            }
            var chosenDialogueBucket = dialogueBucketBin.GetRandomElement();
            ModState.ChosenDialogue = chosenDialogueBucket;
            var chosenDialogue = chosenDialogueBucket.Dialogue.GetRandomElement();
            var customInterpedDialogue = InterpolateSPAMDialogue(chosenDialogue, mercTeamOverride, sim);
            var interpolated = Interpolator.Interpolate(customInterpedDialogue, sim.Context, true);
            var quips = new List<string> {interpolated};
            
            CastDef castDef = Coordinator.CreateCast(combat, Guid.NewGuid().ToString(), mercTeam);
            DialogueContent content = BuildContent(mercTeamOverride, castDef, quips);
            //var contents = new DialogueContent(interpolated, Color.white, mercTeamOverride.teamLeaderCastDefId, null, null,
            //    DialogCameraDistance.Medium, DialogCameraHeight.Default, -1f);
            //Traverse.Create(contents).Field("castDef").SetValue(castDef);
            DialogueContent[] contents = {content};
            //content[0] = contents;
            dialogueContent = contents;
            return true;
        }

        public static string InterpolateSPAMDialogue(string dialogue, TeamOverride teamOverride, SimGameState sim)
        {
            if (string.IsNullOrEmpty(dialogue))
            {
                return string.Empty;
            }
            var interp = dialogue.Replace("{RCNT_SYSTEM}", teamOverride.GetMercFactionLastSystem(sim));
            //add other interps here
            return interp;
        }

        public static string CalculateBribeCostAndSuccess(SimGameState sim, CombatGameState combat, out Tuple<float, int>[] results, out Team mercTeam)
        {
            results = new Tuple<float, int>[5];
            mercTeam = new Team();
            //if (ModState.MercFactionTeamOverride != null) mercTeam = combat.Teams.First(x => x.GUID == "be77cadd-e245-4240-a93e-b99cc98902a5");
            //else 
            if (ModState.HostileMercLanceTeamOverride.TeamOverride != null) mercTeam = combat.Teams.First(x => x.GUID == GlobalVars.HostileMercLanceTeamDefinitionGUID);
            if (mercTeam != null)
            {
                var totalValue = 0;
                foreach (var unit in mercTeam.units)
                {
                    totalValue += unit.PilotableActorDef.BattleValue;
                }

                var baselineCost = totalValue * ModInit.modSettings.BribeCostBaselineMulti;
                var baselineAcceptance = 1f * ModState.ChosenDialogue.BribeAcceptanceMultiplier; // may need to normalize based on unitrating values or some shit
                var playerRep = FetchNormalizedPlayerMRBRep();
                if (ModInit.modSettings.MercFactionConfigs.ContainsKey(mercTeam.FactionValue.Name))
                {
                    var mercRep = ModInit.modSettings.MercFactionConfigs[mercTeam.FactionValue.Name].UnitRating;
                    if (mercRep == 0) mercRep = 1;
                    if (playerRep == 0) playerRep = 1;
                    baselineAcceptance *= (playerRep / mercRep);
                    
                    ModInit.modLog?.Trace?.Write($"[CalculateBribeCostAndSuccess] Player MRB rep {playerRep} vs mercRep {mercRep}, baseline acceptance calcd as {baselineAcceptance}.");
                }
                ModInit.modLog?.Trace?.Write($"[CalculateBribeCostAndSuccess] Total Lance Cost: {totalValue}, bribe calculated  as {baselineCost} from multiplier {ModInit.modSettings.BribeCostBaselineMulti}.");

                var moneyResults1 = Mathf.RoundToInt(baselineCost * .25f);
                var moneyResults2 = Mathf.RoundToInt(baselineCost * .5f);
                var moneyResults3 = Mathf.RoundToInt(baselineCost * .75f);
                var moneyResults4 = Mathf.RoundToInt(baselineCost);


                results[0] = new Tuple<float, int>(0, 0);
                results[1] = new Tuple<float, int>(baselineAcceptance * .25f, moneyResults1);
                results[2] = new Tuple<float, int>(baselineAcceptance * .5f, moneyResults2);
                results[3] = new Tuple<float, int>(baselineAcceptance * .75f, moneyResults3);
                results[4] = new Tuple<float, int>(baselineAcceptance, moneyResults4);

                var resultsPerCent1 = Mathf.RoundToInt(results[1].Item1 * 100f);
                var resultsPerCent2 = Mathf.RoundToInt(results[2].Item1 * 100f);
                var resultsPerCent3 = Mathf.RoundToInt(results[3].Item1 * 100f);
                var resultsPerCent4 = Mathf.RoundToInt(results[4].Item1 * 100f);

                var bribeDescription =
                    $"The {mercTeam.FactionValue?.FactionDef?.Name} have a unit rating of {ModInit.modSettings.MercFactionConfigs[mercTeam.FactionValue.Name].UnitRating}. Spreadsheet warrior that he is, Darius has calculated that they have the following chances of accepting a bribe. Attempting to bribe will send half the money upfront, with the other half wired on acceptance. You currently have {sim.Funds} c-bills." +
                    $"\n\n0% funding - 0¢ (0% chance of success)" +
                    $"\n\n25% funding - {moneyResults1}¢ Total. ({resultsPerCent1}% chance of success)" +
                    $"\n\n50% funding - {moneyResults2}¢ Total. ({resultsPerCent2}% chance of success)" +
                    $"\n\n75% funding - {moneyResults3}¢ Total. ({resultsPerCent3}% chance of success)" +
                    $"\n\n100% funding - {moneyResults4}¢ Total. ({resultsPerCent4}% chance of success)";
                return bribeDescription;
            }
            
            return "SHITS BROKE";
        }

        public static int FetchNormalizedPlayerMRBRep()
        {
            var sim = UnityGameInstance.BattleTechGame.Simulation;
            if (sim == null) return 1;
            var rawRating = sim.GetCareerMRBRating();
            var rawProportion = rawRating / sim.Constants.Story.MRBRepMaxCap;
            return Mathf.RoundToInt(rawProportion * 13f);
        }

        public static int ProcessBribeRoll(Tuple<float, int> mercBribe, Team mercTeam)
        {
            var teamOverride = ModState.HostileMercLanceTeamOverride;
            if (teamOverride == null) return 0;
            var roll = ModInit.Random.NextDouble();
            var sim = UnityGameInstance.BattleTechGame.Simulation;
            var combat = UnityGameInstance.BattleTechGame.Combat;
            ModInit.modLog?.Trace?.Write($"[ProcessBribeRoll] Rolled {roll} need to be < {mercBribe.Item1}.");
            if (roll < mercBribe.Item1)
            {
                var objectives = combat.ItemRegistry.GetObjectsOfType(TaggedObjectType.Objective).ConvertAll<ObjectiveGameLogic>((ITaggedItem x) => x as ObjectiveGameLogic);
                foreach (var objective in objectives)
                {
                    if (objective is DestroyLanceObjective destroyLanceObjective)
                    {
                        if (destroyLanceObjective.EncounterTags.Contains(Tags.ADDITIONAL_LANCE) && destroyLanceObjective
                                .GetTargetUnits().Any(x => mercTeam.units.Contains(x)))
                        {
                            destroyLanceObjective.IgnoreObjective();
                        }
                    }
                }

                if (false) //disabled fuck it
                {
                    //switch sides?
                    ModInit.modLog?.Trace?.Write($"[ProcessBribeRoll] Bribe critical success! Switching merc team to friendly support.");

                    var playerSupportTeam = combat.LocalPlayerTeam.SupportTeam;
                    for (var index = mercTeam.units.Count - 1; index >= 0; index--)
                    {
                        var unit = mercTeam.units[index];
                        unit.RemoveFromLance();
                        unit.RemoveFromTeam();
                        mercTeam.RemoveUnit(unit);

                        playerSupportTeam.AddUnit(unit);
                        unit.AddToTeam(playerSupportTeam);
                        if (playerSupportTeam.lances.Count == 0)
                        {
                            Lance lance = new Lance(playerSupportTeam, Array.Empty<LanceSpawnerRef>());
                            var lanceGuid =
                                $"{LanceSpawnerGameLogic.GetLanceGuid(Guid.NewGuid().ToString())}_{playerSupportTeam.GUID}_SPAM";
                            lance.lanceGuid = lanceGuid;
                            combat.ItemRegistry.AddItem(lance);
                            playerSupportTeam.lances.Add(lance);
                            ModInit.modLog?.Info?.Write(
                                $"Created lance {lance.DisplayName} for Team {playerSupportTeam.DisplayName}.");
                        }//unit.team.GUID == "9ed02e70-beff-4131-952e-49d366e2f7cc" debugline
                        unit.AddToLance(playerSupportTeam.lances.First());
                        playerSupportTeam.lances.First()?.AddUnitGUID(unit.GUID);
                        unit.BehaviorTree = BehaviorTreeFactory.MakeBehaviorTree(combat.BattleTechGame, unit, BehaviorTreeIDEnum.CoreAITree);
                        unit.ResetPathing();
                        unit.ResetBehaviorVariables();
                        CombatantSwitchedTeams combatantSwitchedTeams = new CombatantSwitchedTeams(unit.GUID, playerSupportTeam.GUID);
                        combat.MessageCenter.PublishMessage(combatantSwitchedTeams);
                        //unit.AddToTeam(playerSupportTeam);
                    }
                    LazySingletonBehavior<FogOfWarView>.Instance.FowSystem.Rebuild();
                    combat.RebuildAllLists();
                    playerSupportTeam.RebuildVisibilityCacheAllUnits(combat.GetAllLivingCombatants());
                    PlayBribeResult(mercTeam.Combat, Guid.NewGuid().ToString(), teamOverride.TeamOverride, mercTeam, 2);
                    sim.AddFunds(-mercBribe.Item2, null, false);
                    return 2;
                }
                else
                {
                    ModInit.modLog?.Trace?.Write($"[ProcessBribeRoll] Bribe success! Despawning merc team!");
                    foreach (var unit in mercTeam.units)
                    {
                        var msg = new DespawnActorMessage(EncounterLayerData.MapLogicGuid, unit.GUID, (DeathMethod)DespawnFloatieMessage.Escaped);
                        Utils._despawnActorMethod.Invoke(unit, new object[] { msg });
                    }
                    PlayBribeResult(mercTeam.Combat, Guid.NewGuid().ToString(), teamOverride.TeamOverride, mercTeam, 1);
                    sim.AddFunds(-mercBribe.Item2, null, false);
                    return 1;
                }
            }
            ModInit.modLog?.Trace?.Write($"[ProcessBribeRoll] Bribe failure! Doing nothing.");
            PlayBribeResult(mercTeam.Combat, Guid.NewGuid().ToString(), teamOverride.TeamOverride, mercTeam, 0);
            sim.AddFunds(-Mathf.RoundToInt(mercBribe.Item2 * .5f), null, false);
            return 0;
        }

        public static void PlayBribeResult(CombatGameState combat, string sourceGUID, TeamOverride teamOverride, Team team, int success, float showDuration = 3)
        {
            CastDef castDef = Coordinator.CreateCast(combat, sourceGUID, team);
            List<string> quips;
            switch (success)
            {
                //case 2:
                    //quips = ModState.ChosenDialogue.BribeCriticalSuccessDialogue;
                   // break;
                case 1:
                    quips = ModState.ChosenDialogue.BribeSuccessDialogue;
                    break;
                default:
                    quips = ModState.ChosenDialogue.BribeFailureDialogue;
                    break;
            }
            DialogueContent content = BuildContent(teamOverride, castDef, quips);
            combat.MessageCenter.PublishMessage(new CustomDialogMessage(sourceGUID, content, showDuration));
        }

        private static DialogueContent BuildContent(TeamOverride teamOverride, CastDef castDef, List<string> quips)
        {
            var sim = UnityGameInstance.BattleTechGame.Simulation;
            string quip = quips.GetRandomElement();
            string quipInterpolated = InterpolateSPAMDialogue(quip, teamOverride, sim);
            string localizedQuip = new Localize.Text(quipInterpolated).ToString();
            return Coordinator.BuildDialogueContent(castDef, localizedQuip, Color.white);
        }
    }
}
