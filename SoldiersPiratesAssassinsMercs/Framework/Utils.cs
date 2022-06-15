using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BattleTech;
using BattleTech.Data;
using BattleTech.Framework;
using HBS.Collections;
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
        public static int GetReputationMercFactionWeight(SimGameReputation repValue)
        {
            if (ModInit.modSettings.FactionAppearanceWeightsByReputation.ContainsKey(repValue.ToString()))
            {
                return ModInit.modSettings.FactionAppearanceWeightsByReputation[repValue.ToString()];
            }
            return 0;
        }
        public static int GetMercFactionPoolFromWeight(SimGameState sim)
        {
            var factionValueInt = -1;
            var factionPool = new List<int>();
            foreach (var faction in ModState.simMercFactions)
            {
                ModInit.modLog?.Trace?.Write(
                    $"[GetMercFactionPoolFromWeight] Processing reputation for Merc group: {faction}");
                var factionValue = GetFactionValueFromString(faction);
                var rep = sim.GetReputation(factionValue);
                var weight = GetReputationMercFactionWeight(rep);
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
    }
}




// assign new AI team via TeamDefinition ExtraTeamDefinitionGuidNames so extra merc team will be added by BuildItemRegistry at contract start // will need to make a GUID and keep it consistent

// define new LanceLogic that is AddLanceToTargetTeam, but instead adds to magic merc team

//MC -> may need to patch AddTargetLanceWithDestroyObjectiveBatch to inject new team

//will need to generate new TeamOverride -> ModStateHostileMercLanceTeamOverride
