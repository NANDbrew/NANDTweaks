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
        public const string PLUGIN_VERSION = "1.2.0";

        //--settings--
        internal static ConfigEntry<bool> storage;
        internal static ConfigEntry<float> cheatSpeed;
        internal static ConfigEntry<bool> cheats;
        internal static ConfigEntry<bool> drunkenSleep;
        internal static ConfigEntry<bool> elixirText;
        internal static ConfigEntry<bool> compatMode;
        internal static ConfigEntry<bool> saveLoadThumbs;
        internal static ConfigEntry<bool> cargoDecal;
        internal static ConfigEntry<Color> decalColor;
        internal static ConfigEntry<bool> wideShipyardUI;

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
            cargoDecal = Config.Bind("CargoDecal", "Mission goods decal", true, new ConfigDescription("Add a decal to mission goods to make them easier to identify"));
            decalColor = Config.Bind("CargoDecal", "Decal color", Color.black);
            wideShipyardUI = Config.Bind("Shipyard", "Wide UI", true, new ConfigDescription("Adjust shipyard UI to better fit 16:9 screens"));


            cheatSpeed = Config.Bind("Cheats", "Cheat Speed", 10f, new ConfigDescription("Hold forward while using steering wheel to propel boat. Hold shift for triple speed", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            cheats = Config.Bind("Cheats", "Cheaty Movement", false, new ConfigDescription("Don't do this unless you're a dirty cheater", null, new ConfigurationManagerAttributes { IsAdvanced = true }));

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
