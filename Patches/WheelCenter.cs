using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(GPButtonSteeringWheel), "ExtraFixedUpdate")]
    internal static class WheelCenter
    {
        private static void Postfix(GoPointer ___stickyClickedBy, ref float ___currentInput)
        {
            if (!Plugin.wheelCenter.Value) return;
            if (___stickyClickedBy)
            {
                if (GameInput.GetKey(InputName.RotateH))
                {
                    ___currentInput = 0;
                }

            }
        }
    }
}
