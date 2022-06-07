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
            }
        }

        public static void AssignMercFactionToTeamState(this TeamOverride team, int mercFactionKey)
        {
            FactionValue factionByID = FactionEnumeration.GetFactionByID(mercFactionKey);
            team.faction = factionByID.Name;
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


//Contract.AddTeamFaction() and dictionary Contract.teamFactionIDs has faction info; dict is teamGUID [target team, etc] key, factionID (as key int) value from faction.json; bleh

//prep contract is where teamFactionIDs get factions added to it (team guid, faction int)

//assignFactionsToTeams is hwere targetTeam gets faction assigned to it

//patch GenerateUnits and override targetTeam?


//MC -> may need to patch AddTargetLanceWithDestroyObjectiveBatch to inject new team (possibly use target ally? and just disallow in 3 way?)

// assign new AI team via TeamDefinition ExtraTeamDefinitionGuidNames so extra merc team will be added by BuildItemRegistry at contract start // will need to make a GUID and keep it consistent

//will need to generate new TeamOverride and do...something with it. FACK
