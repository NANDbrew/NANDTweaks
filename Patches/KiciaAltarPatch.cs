using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Patches
{
    internal class KiciaAltarPatch
    {
        [HarmonyPatch(typeof(KiciaAltar))]
        private static class AltarPatch
        {
            [HarmonyPatch("Update")]
            [HarmonyPostfix]
            public static void UpdatePatch(ParticleSystem ___sacParticles)
            {
                ___sacParticles.GetComponent<AudioSource>().enabled = false;
            }
        }
    }
}
