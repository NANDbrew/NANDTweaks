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
        public const string PLUGIN_VERSION = "1.3.1";

        public enum DecalType
        {
            None,
            CompanyLogo,
            Origin
        }

        //--settings--
        internal static ConfigEntry<bool> storage;
        internal static ConfigEntry<bool> drunkenSleep;
        internal static ConfigEntry<bool> elixirText;
        internal static ConfigEntry<bool> compatMode;
        internal static ConfigEntry<bool> saveLoadThumbs;
        internal static ConfigEntry<DecalType> cargoDecal;
        internal static ConfigEntry<Color> decalColor;
        internal static ConfigEntry<bool> wideShipyardUI;
        internal static ConfigEntry<bool> boxLabels;
        internal static ConfigEntry<bool> shipyardInfo;
        internal static ConfigEntry<int> milesPerDegree;
        internal static ConfigEntry<bool> wheelCenter;
        internal static ConfigEntry<bool> noOutlines;
        internal static ConfigEntry<bool> ladderPatch;
        internal static ConfigEntry<float> embarkDist;
        internal static ConfigEntry<bool> skipDisclaimer;

        internal static ManualLogSource logSource;
        internal static Plugin instance;

        private void Awake()
        {
            logSource = Logger;
            instance = this;

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_ID);

            storage = Config.Bind("Storage", "Storage", true, new ConfigDescription("Put items back in crates (primary interact button)"));
            drunkenSleep = Config.Bind("Sleep", "Drunken Sleep", true, new ConfigDescription("Alcohol affects you while sleeping. (Taken from Raha's QOL mod)"));
            compatMode = Config.Bind("Save Thumbnails", "Thumbnail Compatibility mode", false, new ConfigDescription("Enable if save slot thumbnails don't save properly"));
            saveLoadThumbs = Config.Bind("Save Thumbnails", "Save and load thumbnails", true, new ConfigDescription("Enable/disable save slot thumbnails entirely (requires a restart to take effect)"));
            cargoDecal = Config.Bind("CargoDecal", "Mission goods decal", DecalType.CompanyLogo, new ConfigDescription("Add a decal to mission goods to make them easier to identify"));
            decalColor = Config.Bind("CargoDecal", "Decal color", Color.black);
            elixirText = Config.Bind("Info", "Elixir Text", true, new ConfigDescription("Show text labels on Energy Elixir and Snake Oil"));
            boxLabels = Config.Bind("Info", "Box labels", true, new ConfigDescription("Add pictograms to tobacco and candle boxes"));
            milesPerDegree = Config.Bind("Info", "Miles per degree", 90, new ConfigDescription("Changes the chiplog's new alternate mode (and depending on setting, the mission ui)\n60 matches real-world nautical miles, 90 matches normal mission miles, 140 matches normal chip log knots", new AcceptableValueList<int>(new int[3] { 60, 90, 140 })));

            wheelCenter = Config.Bind("Miscelaneous", "Wheel centering", false, new ConfigDescription("Press 'Q' (or whatever you have that control bound to) while using the helm to center it"));
            noOutlines = Config.Bind("Miscelaneous", "No outlines", false, new ConfigDescription("Removes outlines on all interactable stuff, except for new mission goods"));
            skipDisclaimer = Config.Bind("Miscelaneous", "Skip disclaimer", true, new ConfigDescription("Skip the Early Access disclaimer"));

            wideShipyardUI = Config.Bind("Shipyard", "Wide UI", true, new ConfigDescription("Adjust shipyard UI to better fit 16:9 screens"));
            shipyardInfo = Config.Bind("Shipyard", "Shipyard Info", true, new ConfigDescription("Show sail and part weight in shipyard ui"));

            ladderPatch = Config.Bind("Embark", "Ladder improvements", true, new ConfigDescription("Use ladders from nearby boats (also move non-instantly)"));
            embarkDist = Config.Bind("Embark", "Embark distance", 0.1f, new ConfigDescription("How far into boat after ladder (only applies if \"Ladder improvements\" is enabled", new AcceptableValueRange<float>(0.1f, 1f)));


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
