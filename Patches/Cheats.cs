using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine;


namespace NANDTweaks.Patches
{
    internal class Cheats
    {
        [HarmonyPatch(typeof(GPButtonSteeringWheel), "Update")]
        private static class CheatySpeed
        {
            private static void Postfix(GPButtonSteeringWheel __instance, GoPointer ___stickyClickedBy)
            {
                //if (!Main.cheats.Value) return;
                if (___stickyClickedBy)
                {
                    if (GameInput.GetKey(InputName.MoveUp))
                    {
                        if (GameInput.GetKey(InputName.Run))
                        {
                            GameState.currentBoat.GetComponentInParent<Rigidbody>().AddRelativeForce(Vector3.forward * Plugin.cheatSpeed.Value * 3, ForceMode.Acceleration);
                        }
                        else
                        {
                            GameState.currentBoat.GetComponentInParent<Rigidbody>().AddRelativeForce(Vector3.forward * Plugin.cheatSpeed.Value, ForceMode.Acceleration);
                        }

                        //ModLogger.Log(Main.mod, "recieved input");
                    }
                    else if (GameInput.GetKey(InputName.MoveDown))
                    {
                        if (GameInput.GetKey(InputName.Run))
                        {
                            GameState.currentBoat.GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * Plugin.cheatSpeed.Value * 1.5f, ForceMode.Acceleration);
                        }
                        else
                        {
                            GameState.currentBoat.GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * Plugin.cheatSpeed.Value * 0.5f, ForceMode.Acceleration);
                        }
                    }
                }
            }
        }
    }
}
