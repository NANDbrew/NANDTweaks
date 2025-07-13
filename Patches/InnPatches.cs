using HarmonyLib;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(Tavern))]
    internal static class InnPatches
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void Postfix(Tavern __instance) 
        {
            var trigger = __instance.gameObject.AddComponent<InteriorEffectsTrigger>();
            trigger.doors = new GPButtonTrapdoor[0];
            trigger.semiIndoor = true;

        }
    }
}
