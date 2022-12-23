using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using BattleTech.Data;
using BattleTech.Framework;
using HBS;
using MissionControl.Data;
using MissionControl.Logic;
using UnityEngine;

namespace SoldiersPiratesAssassinsMercs.Framework
{
    public class Classes
    {
        public class ConfigOptions
        {
            public class OpforReplacementConfig
            {
                public float BaseReplaceChance = 0f;
                public Dictionary<string, float> FactionsReplaceOverrides = new Dictionary<string, float>(); //faction-specific values completely override base chance
                //public List<string> BlacklistContractTypes = new List<string>();
                //public List<string> BlacklistContractIDs = new List<string>();
                //public float MercFactionReputationFactor = 0f; // merc faction will lose rep as function of target team rep
            }
            public class MercLanceAdditionConfig // will take place of "additional lance" or MC support lances
            {
                public float BaseReplaceChance = 0f;
                public Dictionary<string, float> FactionsReplaceOverrides = new Dictionary<string, float>(); //faction-specific values completely override base chance
                //public List<string> BlacklistContractTypes = new List<string>();
                //public List<string> BlacklistContractIDs = new List<string>();
                public float MercFactionReputationFactor = 0f;
            }
            public class MercFactionConfig
            {
                public string MercFactionName = ""; //e,g, KellHounds or RazorbackMercs
                public int AppearanceWeight = 0; //base "weight" for selection
                //public float AppearanceWeightRepFactor = 0f; //additional "weight" as factor of times previously faced (internal counter)
                public List<string> EmployerBlacklist = new List<string>();
                public float UnitRating = 1; //higher rating means less likely to take bribe to disengage or switch sides
                public List<string> PersonalityAttributes = new List<string>();
            }

            public class AlternateOpforConfig // these are alternate factions for specific factions which are NOT mercenaries.
                                              // part of setting, where dictionary key = faction name being replaced
            {
                public float FactionReplaceChance = 0f;
                public float FactionMCAdditionalLanceReplaceChance = 0f;
                public Dictionary<string, int> AlternateOpforWeights = new Dictionary<string, int>();
            }
        }
        public class MercDialogueBucket //is value for dictionary where key = PersonalityAttributes
        {
            public List<string> Dialogue = new List<string>();
            //public List<string> BribeCriticalSuccessDialogue = new List<string>();
            public List<string> BribeSuccessDialogue = new List<string>();
            public List<string> BribeFailureDialogue = new List<string>();
            public int MinTimesEncountered = 0;
            public int MaxTimesEncountered = 0;
            public float BribeAcceptanceMultiplier = 1f;
        }
    }
}
