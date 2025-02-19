using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(SaveableBoatCustomization), "Awake")]
    internal static class BoatListCreator
    {
        internal static List<BoatRefs> boatList = new List<BoatRefs>();
        public static void Postfix(BoatRefs ___refs)
        {
            boatList.Add(___refs);
        }
    }
}
