using System;
using System.Collections.Generic;
using System.Reflection;
using IRBTModUtils.Logging;
using Newtonsoft.Json;
using SoldiersPiratesAssassinsMercs.Framework;
using ModState = SoldiersPiratesAssassinsMercs.Framework.ModState;

namespace SoldiersPiratesAssassinsMercs
{
    public static class ModInit
    {
        internal static DeferringLogger modLog;
        internal static string modDir;
        public static readonly Random Random = new Random();

        public static Settings modSettings;
        public const string HarmonyPackage = "us.tbone.SoldiersPiratesAssassinsMercs";

        public static void Init(string directory, string settings)
        {
            modDir = directory;
            Exception settingsException = null;
            try
            {
                modSettings = JsonConvert.DeserializeObject<Settings>(settings);
            }
            catch (Exception ex)
            {
                settingsException = ex;
                modSettings = new Settings();
            }

            //HarmonyInstance.DEBUG = true;
            modLog = new DeferringLogger(modDir, "logSPAM", modSettings.enableDebug, modSettings.enableTrace);
            if (settingsException != null)
            {
                ModInit.modLog?.Error?.Write($"EXCEPTION while reading settings file! Error was: {settingsException}");
            }

            ModInit.modLog?.Info?.Write($"Initializing SPAM - Version {typeof(Settings).Assembly.GetName().Version}");
            //var harmony = HarmonyInstance.Create(HarmonyPackage);
            //harmony.PatchAll(Assembly.GetExecutingAssembly());
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), HarmonyPackage);
            //dump settings
            ModInit.modLog?.Info?.Write($"Settings dump: {settings}");
            ModState.InitializeDialogueStrings();
            if (modSettings.dumpSubFactions) ModState.GenerateFactionMap();
            ModState.BuildFallbackMap();
        }
    }
    public class Settings
    {
        public bool dumpSubFactions = false;
        public bool enableDebug = false;
        public bool enableTrace = false;
        public List<string> BlacklistedContractTypesAndIDs = new List<string>();
        public Classes.ConfigOptions.OpforReplacementConfig OpforReplacementConfig =
            new Classes.ConfigOptions.OpforReplacementConfig();
        public Classes.ConfigOptions.MercLanceAdditionConfig MercLanceAdditionConfig =
            new Classes.ConfigOptions.MercLanceAdditionConfig();

        public Dictionary<string, Classes.ConfigOptions.AlternateOpforConfig> AlternateFactionConfigs =
            new Dictionary<string, Classes.ConfigOptions.AlternateOpforConfig>();

        public Dictionary<string, Classes.ConfigOptions.AlternateOpforConfig> PlanetFactionConfigs =
            new Dictionary<string, Classes.ConfigOptions.AlternateOpforConfig>(); // eg. starsystemdef_Place for keys


        public Dictionary<string, Classes.ConfigOptions.MercFactionConfig> MercFactionConfigs = new Dictionary<string, Classes.ConfigOptions.MercFactionConfig>(); //merc factions need to be configd here to be used. key is Merc Name

        public string FallbackUnitFactionTag = ""; //probably be just "mercenaries" for BTA// faction tags are lowercase

        public float BribeCostBaselineMulti = 0.01f; //default to 1% of total opfor lance value (uses "BV" calculation)
        // deprecated, but left in for disabled code
        public string BribeAbility = "AbilityDefAttemptBribe";

        public List<string> BattleRoyaleContracts = new List<string>();
    }
}