using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
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
        public const string PLUGIN_VERSION = "0.1.0";

        //--settings--
        internal static ConfigEntry<bool> storage;
        internal static ManualLogSource logSource;


        private void Awake()
        {
            logSource = Logger;

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_ID);

            storage = Config.Bind("Settings", "Storage", true);
        }
    }
}
