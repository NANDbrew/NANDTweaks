using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using NANDTweaks.Patches;
using NANDTweaks.Scripts;
using SailwindModdingHelper;
using System;
using System.Reflection;
using UnityEngine;

namespace NANDTweaks
{
    [BepInPlugin(PLUGIN_ID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("com.app24.sailwindmoddinghelper", "2.0.3")]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_ID = "com.nandbrew.nandtweaks";
        public const string PLUGIN_NAME = "NAND Tweaks";
        public const string PLUGIN_VERSION = "1.3.0";

        //--settings--
        internal static ConfigEntry<bool> storage;
        internal static ConfigEntry<bool> drunkenSleep;
        internal static ConfigEntry<bool> elixirText;
        internal static ConfigEntry<bool> compatMode;
        internal static ConfigEntry<bool> saveLoadThumbs;
        internal static ConfigEntry<int> cargoDecal;
        internal static ConfigEntry<Color> decalColor;
        internal static ConfigEntry<bool> wideShipyardUI;
        internal static ConfigEntry<bool> boxLabels;
        internal static ConfigEntry<bool> shipyardInfo;
        internal static ConfigEntry<int> milesPerDegree;

        internal static ManualLogSource logSource;
        internal static Plugin instance;

        private void Awake()
        {
            logSource = Logger;
            instance = this;

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_ID);

            storage = Config.Bind("Storage", "Storage", true, new ConfigDescription("Put items back in crates (primary interact button)"));
            elixirText = Config.Bind("Info", "Elixir Text", true, new ConfigDescription("Show text labels on Energy Elixir and Snake Oil"));
            drunkenSleep = Config.Bind("Sleep", "Drunken Sleep", true, new ConfigDescription("Alcohol affects you while sleeping. (Taken from Raha's QOL mod)"));
            compatMode = Config.Bind("Save Thumbnails", "Thumbnail Compatibility mode", false, new ConfigDescription("Enable if save slot thumbnails don't save properly"));
            saveLoadThumbs = Config.Bind("Save Thumbnails", "Save and load thumbnails", true, new ConfigDescription("Enable/disable save slot thumbnails entirely (requires a restart to take effect)"));
            cargoDecal = Config.Bind("CargoDecal", "Mission goods decal", 1, new ConfigDescription("Add a decal to mission goods to make them easier to identify", new AcceptableValueRange<int>(0, 2)));
            decalColor = Config.Bind("CargoDecal", "Decal color", Color.black);
            boxLabels = Config.Bind("Info", "Box labels", true, new ConfigDescription("Add pictograms to tobacco and candle boxes"));
            milesPerDegree = Config.Bind("Info", "Miles per degree", 60, new ConfigDescription("Changes the chiplog's new alternate mode (and depending on setting, the mission ui)\n60 matches real-world nautical miles, 90 matches normal mission miles, 140 matches normal chip log knots", new AcceptableValueList<int>(new int[3] { 60, 90, 140 })));


            wideShipyardUI = Config.Bind("Shipyard", "Wide UI", true, new ConfigDescription("Adjust shipyard UI to better fit 16:9 screens"));
            shipyardInfo = Config.Bind("Shipyard", "Shipyard Info", true, new ConfigDescription("Show sail and part weight in shipyard ui"));


            decalColor.SettingChanged += (sender, args) => MatLoader.UpdateColor();
            wideShipyardUI.SettingChanged += (sender, args) => ShipyardUITweaks.UpdatePositions();

            GameEvents.OnGameStart += (_, __) =>
            {
                if (saveLoadThumbs.Value)
                {
                    MenuModder.Setup();
                }
                MatLoader.Start();
            };
        }
    }
}
