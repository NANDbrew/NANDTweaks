using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using NANDTweaks.Patches;
using System;
using System.Reflection;

namespace NANDTweaks
{
    [BepInPlugin(PLUGIN_ID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("com.app24.sailwindmoddinghelper", "2.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_ID = "com.nandbrew.nandtweaks";
        public const string PLUGIN_NAME = "NAND Tweaks";
        public const string PLUGIN_VERSION = "1.0.2";

        //--settings--
        internal static ConfigEntry<bool> storage;
        internal static ConfigEntry<float> cheatSpeed;
        internal static ConfigEntry<bool> cheats;
        internal static ConfigEntry<bool> drunkenSleep;


        internal static ManualLogSource logSource;


        private void Awake()
        {
            logSource = Logger;

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_ID);

            storage = Config.Bind("Settings", "Storage", true);
            drunkenSleep = Config.Bind("Settings", "Drunken Sleep", true, new ConfigDescription("Alcohol affects you while sleeping. (Taken from Raha's QOL mod)"));

            cheatSpeed = Config.Bind("Settings", "Cheat Speed", 10f, new ConfigDescription("Hold forward while using steering wheel to propel boat. Hold shift for triple speed", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            cheats = Config.Bind("Settings", "Cheats", false, new ConfigDescription("Don't do this unless you're a dirty cheater", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
        }
    }
}
