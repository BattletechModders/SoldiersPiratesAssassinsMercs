using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleTech;

namespace SoldiersPiratesAssassinsMercs.Framework
{
    public class ModState
    {
        public static List<string> simDisplayedFactions = new List<string>();
        public static List<string> simMercFactions = new List<string>();
        public static string MercFactionForReplacement = "";

        public static List<string> GetSimDisplayedFactions()
        {
            return simDisplayedFactions;
        }
        public static List<string> GetSimMercFactions()
        {
            return simMercFactions;
        }
        public static void InitializeMercFactionList(SimGameState sim)
        {
            if (simMercFactions.Count != 0) return;
            ModState.simDisplayedFactions = new List<string>(sim.displayedFactions);
            foreach (var faction in FactionEnumeration.FactionList)
            {
                if (!faction.IsRealFaction || !faction.DoesGainReputation ||
                    (!faction.IsMercenary && !faction.IsPirate)) continue;
                if (!ModInit.modSettings.FactionBlacklist.Contains(faction.Name))
                {
                    ModState.simMercFactions.Add(faction.Name);
                }
            }
        }

        public static void ResetStateAfterContract()
        {
            MercFactionForReplacement = "";
        }
    }
}
