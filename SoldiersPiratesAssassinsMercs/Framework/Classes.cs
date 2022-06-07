using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleTech;

namespace SoldiersPiratesAssassinsMercs.Framework
{
    public class Classes
    {
        public static TeamDefinition HostileMercTeamDefinition = new TeamDefinition("ddfd570d-f9e4-42f8-b2e8-671eb1e8f43a", "HostileMercenaryTeam");
        public static TeamDefinition FriendlyMercTeamDefinition = new TeamDefinition("be68d8cb-6e32-401e-889e-c37cf10c0528", "FriendlyMercenaryTeam");
        public class ConfigOptions
        {
            public class OpforReplacementConfig
            {
                public float BaseReplaceChance = 0f;
                public Dictionary<string, float> FactionsReplaceOverrides = new Dictionary<string, float>(); //faction-specific values completely override base chance
                public List<string> BlacklistContractTypes = new List<string>();
                public List<string> BlacklistContractIDs = new List<string>();
            }

            public class MercLanceAdditionConfig // will take place of "additional lance" or MC support lances?
            {
                public float BaseReplaceChance = 0f;
                public Dictionary<string, float> FactionsReplaceOverrides = new Dictionary<string, float>(); //faction-specific values completely override base chance
                public List<string> BlacklistContractTypes = new List<string>();
                public List<string> BlacklistContractIDs = new List<string>();
            }

        }
    }
}
