using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using NANDTweaks.Patches;
using NANDTweaks.Scripts;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace NANDTweaks
{
    [BepInPlugin(PLUGIN_ID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_ID = "com.nandbrew.nandtweaks";
        public const string PLUGIN_NAME = "NANDTweaks";
        public const string PLUGIN_VERSION = "1.4.2";

        public enum DecalType
        {
            None,
            CompanyLogo,
            Origin
        }
        internal static string dataPath;

        //- settings -
        //internal static ConfigEntry<bool> storage;
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
        internal static ConfigEntry<float> embarkHeight;
        internal static ConfigEntry<bool> skipDisclaimer;
        internal static ConfigEntry<bool> saveLoadState;
        internal static ConfigEntry<bool> hideLoading;

        internal static ManualLogSource logSource;
        internal static Plugin instance;

        private void Awake()
        {
            logSource = Logger;
            instance = this;

            dataPath = Directory.GetParent(Plugin.instance.Info.Location).FullName;
            if (Directory.Exists(Path.Combine(dataPath, PLUGIN_NAME)))
            {
                dataPath = Path.Combine(dataPath, PLUGIN_NAME);
            }

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_ID);

            //storage = Config.Bind("Storage", "Storage", false, new ConfigDescription("Put items back in crates (primary interact button)"));
            drunkenSleep = Config.Bind("--------- Sleep ---------", "Drunken Sleep", true, new ConfigDescription("Alcohol affects you while sleeping. (Taken from Raha's QOL mod)"));
            compatMode = Config.Bind("---- Save Thumbnails ----", "Thumbnail Compatibility mode", false, new ConfigDescription("Enable if save slot thumbnails don't save properly", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            saveLoadThumbs = Config.Bind("---- Save Thumbnails ----", "Save and load thumbnails", true, new ConfigDescription("Enable/disable save slot thumbnails entirely (requires a restart to take effect)"));
            cargoDecal = Config.Bind("------ Cargo Decal ------", "Mission goods decal", DecalType.CompanyLogo, new ConfigDescription("Add a decal to mission goods to make them easier to identify"));
            decalColor = Config.Bind("------ Cargo Decal ------", "Decal color", new Color(0.12f, 0.10f, 0.10f));
            elixirText = Config.Bind("--------- Info ----------", "Elixir Text", true, new ConfigDescription("Show text labels on Energy Elixir and Snake Oil"));
            boxLabels = Config.Bind("--------- Info ----------", "Box labels", true, new ConfigDescription("Add pictograms to tobacco and candle boxes"));
            milesPerDegree = Config.Bind("--------- Info ----------", "Miles per degree", 90, new ConfigDescription("Changes the chiplog's new alternate mode (and depending on setting, the mission ui)\n60 matches real world nautical miles, 90 matches normal mission miles, 140 matches normal chip log knots", new AcceptableValueList<int>(new int[3] { 60, 90, 140 })));
            wheelCenter = Config.Bind("----- Miscellaneous -----", "Wheel centering", false, new ConfigDescription("Press 'Q' (or whatever you have that control bound to) while using the helm to center it"));
            noOutlines = Config.Bind("----- Miscellaneous -----", "No outlines", false, new ConfigDescription("Removes outlines on all interactable stuff, except for new mission goods"));
            skipDisclaimer = Config.Bind("----- Miscellaneous -----", "Skip disclaimer", true, new ConfigDescription("Skip the Early Access disclaimer"));
            saveLoadState = Config.Bind("----- Miscellaneous -----", "Save and load ship state", true, new ConfigDescription("Saves the ship's speed, which sails are furled, and whether the steering is locked"));

            wideShipyardUI = Config.Bind("------- Shipyard --------", "Wide UI", true, new ConfigDescription("Adjust shipyard UI to better fit 16:9 screens"));
            shipyardInfo = Config.Bind("------- Shipyard --------", "Shipyard Info", true, new ConfigDescription("Show sail and part weight in shipyard ui"));

            ladderPatch = Config.Bind("-------- Embark ---------", "Ladder improvements", true, new ConfigDescription("Use ladders from nearby boats (also move non instantly)"));
            embarkDist = Config.Bind("-------- Embark ---------", "Embark distance", 0.1f, new ConfigDescription("How far into boat after ladder (only applies if \"Ladder improvements\" is enabled", new AcceptableValueRange<float>(0.1f, 2f), new ConfigurationManagerAttributes { IsAdvanced = true }));
            embarkHeight = Config.Bind("-------- Embark ---------", "Embark height", 1.25f, new ConfigDescription("How far up after ladder (only applies if \"Ladder improvements\" is enabled", new AcceptableValueRange<float>(0.25f, 2f), new ConfigurationManagerAttributes { IsAdvanced = true }));
            hideLoading = Config.Bind("----- Miscellaneous -----", "Hide loading", false, new ConfigDescription("Keep the screen black until the physics engine has settled", null, new ConfigurationManagerAttributes { IsAdvanced = true }));

            decalColor.SettingChanged += (sender, args) => MatLoader.UpdateColor();
            wideShipyardUI.SettingChanged += (sender, args) => ShipyardUITweaks.UpdatePositions();

        }

        [HarmonyPatch(typeof(StartMenu), "Start")]
        private static class GameStartPatch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (saveLoadThumbs.Value)
                {
                    MenuModder.Setup();
                }
                MatLoader.Start();
            }
        }

    }
}
