using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(RecoveryPort), "Start")]
    internal class RecoveryMooringsColorPatch
    {
        static Color color = new Color(0.687f, 0.243f, 0.291f, 1f);
        public static void Postfix(RecoveryPort __instance)
        {
            if (!Plugin.mooringColor.Value) return;

            var back = __instance.mooringBack.GetComponent<MeshRenderer>();
            if (back.material.color == Color.white) back.material.color = Color.red;
            else back.material.color = color;

            var front = __instance.mooringFront.GetComponent<MeshRenderer>();
            if (front.material.color == Color.white) front.material.color = Color.red;
            else front.material.color = color;
        }
    }
}
