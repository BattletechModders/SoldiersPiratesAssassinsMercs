using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                public List<string> BlacklistContractTypes = new List<string>();
                public List<string> BlacklistContractIDs = new List<string>();
            }
        }
    }
}
