using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(GPButtonKeybinding), "SetNewKey")]
    public static class KeyBindPatch
    {
        private static void Postfix(KeyCode newKey, InputName ___inputNameEnum, bool ___input2, bool ___controllerButton)
        {
            if (newKey == KeyCode.Backspace)
            {
                GameInput.SetKeyMap(___inputNameEnum, KeyCode.None, ___input2, ___controllerButton);

            }

        }
    }
}
