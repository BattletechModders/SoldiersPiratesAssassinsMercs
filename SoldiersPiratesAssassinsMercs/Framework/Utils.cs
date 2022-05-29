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

        public static void AssignFactionToTeamPublic(this TeamOverride team, Dictionary<string, int> teamFactions)
        {
            if (teamFactions.ContainsKey(team.teamGuid))
            {
                FactionValue factionByID = FactionEnumeration.GetFactionByID(teamFactions[team.teamGuid]);
                if (team.FactionValue.IsInvalidUnset)
                {
                    team.faction = factionByID.Name;
                }
            }
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
            switch (repValue)
            {
                case SimGameReputation.LOATHED:
                    return ModInit.modSettings.FactionAppearanceWeightsByReputation[SimGameReputation.LOATHED];
                case SimGameReputation.HATED:
                    return ModInit.modSettings.FactionAppearanceWeightsByReputation[SimGameReputation.HATED];
                case SimGameReputation.DISLIKED:
                    return ModInit.modSettings.FactionAppearanceWeightsByReputation[SimGameReputation.DISLIKED];
                case SimGameReputation.INDIFFERENT:
                    return ModInit.modSettings.FactionAppearanceWeightsByReputation[SimGameReputation.INDIFFERENT];
                case SimGameReputation.LIKED:
                    return ModInit.modSettings.FactionAppearanceWeightsByReputation[SimGameReputation.LIKED];
                case SimGameReputation.FRIENDLY:
                    return ModInit.modSettings.FactionAppearanceWeightsByReputation[SimGameReputation.FRIENDLY];
                case SimGameReputation.HONORED:
                    return ModInit.modSettings.FactionAppearanceWeightsByReputation[SimGameReputation.HONORED];
                default:
                    return ModInit.modSettings.FactionAppearanceWeightsByReputation[SimGameReputation.HONORED];
            }
        }
        public static int GetMercFactionPoolFromWeight(SimGameState sim)
        {
            var factionValueInt = -1;
            var factionPool = new List<int>();
            foreach (var faction in ModState.simMercFactions)
            {
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
    }
}
//Contract.AddTeamFaction() and dictionary Contract.teamFactionIDs has faction info; dict is teamGUID [target team, etc] key, factionID (as key int) value from faction.json; bleh


//prep contract is where teamFactionIDs get factions added to it (team guid, faction int)

//assignFactionsToTeams is hwere targetTeam gets faction assigned to it

//patch GenerateUnits and override targetTeam?