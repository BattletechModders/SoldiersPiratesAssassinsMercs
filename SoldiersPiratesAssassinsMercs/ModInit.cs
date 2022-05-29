using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using Harmony;
using IRBTModUtils;
using IRBTModUtils.Logging;
using Microsoft.Win32;
using Newtonsoft.Json;
using SoldiersPiratesAssassinsMercs.Framework;

namespace SoldiersPiratesAssassinsMercs
{
    public static class ModInit
    {
        internal static DeferringLogger modLog;
        private static string modDir;
        public static readonly Random Random = new Random(456);

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
            var harmony = HarmonyInstance.Create(HarmonyPackage);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            //dump settings
            ModInit.modLog?.Info?.Write($"Settings dump: {settings}");
        }
    }
    public class Settings
    {
        public bool enableDebug = false;
        public bool enableTrace = false;
        public List<string> FactionBlacklist = new List<string>();
        public Dictionary<SimGameReputation, int> FactionAppearanceWeightsByReputation =
            new Dictionary<SimGameReputation, int>();

        public Classes.ConfigOptions.OpforReplacementConfig OpforReplacementConfig =
            new Classes.ConfigOptions.OpforReplacementConfig();

        public string FallbackUnitFactionTag = ""; //probably be just "Mercenaries" for BTA
    }
}
