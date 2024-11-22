using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(GPButtonSteeringWheel), "ExtraFixedUpdate")]
    internal static class WheelCenter
    {
        private static void Postfix(GoPointer ___stickyClickedBy, ref float ___currentInput)
        {
            if (!Plugin.wheelCenter.Value) return;
            if ((bool)___stickyClickedBy && GameInput.GetKey(InputName.RotateH))
            {
                if (___currentInput > 0) ___currentInput = Mathf.Max(___currentInput - 3, 0);
                else if (___currentInput < 0) ___currentInput = Mathf.Min(___currentInput + 3, 0);

            }
        }
    }
}
