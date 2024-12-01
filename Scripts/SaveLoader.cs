//using SailwindModdingHelper;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Scripts
{
    public static class SaveLoader
    {
        public static Dictionary<int, Vector3> velocities = new Dictionary<int, Vector3>();
        const string modString = "NANDTweaks.shipState.";

        public static IEnumerator LoadAfterDelay()
        {
            yield return new WaitUntil(() => GameState.playing && !GameState.justStarted);// !FloatingOriginManager.instance.GetPrivateField<bool>("instantShifting"));// && !GameState.justStarted);

            foreach (BoatRefs gameObject in GameObject.FindObjectsOfType<BoatRefs>())
            {
                if (!gameObject.GetComponent<PurchasableBoat>().isPurchased()) continue;
                SaveLoader.LoadSailConfig(gameObject);

                if (SaveLoader.velocities.TryGetValue(gameObject.GetComponent<SaveableObject>().sceneIndex, out Vector3 vel))
                {
                    gameObject.GetComponent<Rigidbody>().velocity = vel;
                    Debug.Log("set velocity for " + gameObject.name + " to " + vel);
                }
            }
        }
        public static void LoadSailConfig(BoatRefs refs)
        {
            string boat = modString + refs.GetComponent<SaveableObject>().sceneIndex;
            //Debug.Log("attempting to load data");
            if (!GameState.modData.ContainsKey(boat))
            {
                Debug.Log("modData does not contain config");
                return;
            }
            if (!refs.gameObject.GetComponent<BoatPerformanceSwitcher>().performanceModeIsOn())
            {
                string[] slug = GameState.modData[boat].Split('[');
                //Debug.Log("loading data: " + slug);
                string[] masts = slug[0].Split(new char[] { ')' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string mast in masts)
                {
                    //Debug.Log($"{mast}");
                    string[] foo = mast.Split('(');
                    int mastIndex = Convert.ToInt32(foo[0]);
                    string[] sails = foo[1].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < refs.masts[mastIndex].sails.Count; i++)
                    {
                        GameObject installedSail = refs.masts[mastIndex].sails[i];
                        string[] sailInfo = sails[i].Split(',');
                        if (installedSail.GetComponent<Sail>().prefabIndex == Convert.ToInt32(sailInfo[0], CultureInfo.InvariantCulture))
                        {
                            //installedSail.GetComponent<Sail>().currentUnroll = Convert.ToSingle(sailInfo[1], CultureInfo.InvariantCulture);
                            SailConnections component3 = installedSail.GetComponent<SailConnections>();

                            component3.reefController.currentLength = Convert.ToSingle(sailInfo[1], CultureInfo.InvariantCulture);
                            //Debug.Log(component3 + " : " + component3.reefController.currentLength);
                            if (component3.angleControllerMid != null && sailInfo.Length == 3)
                            {
                                component3.angleControllerMid.currentLength = Convert.ToSingle(sailInfo[2], CultureInfo.InvariantCulture);
                                Traverse.Create(component3.angleControllerMid).Field("changed").SetValue(true);
                                //Debug.Log("mid angle controller length = " + component3.angleControllerMid.currentLength);
                            }
                            else if (component3.angleControllerLeft != null && component3.angleControllerRight != null && sailInfo.Length == 4)
                            {
                                component3.angleControllerLeft.currentLength = Convert.ToSingle(sailInfo[2], CultureInfo.InvariantCulture);
                                component3.angleControllerRight.currentLength = Convert.ToSingle(sailInfo[3], CultureInfo.InvariantCulture);
                                Traverse.Create(component3.angleControllerLeft).Field("changed").SetValue(true);
                                Traverse.Create(component3.angleControllerRight).Field("changed").SetValue(true);
                                //Debug.Log("left & right angle controllers exist");
                            }
                        }
                    }
                }
                string[] extraData = slug[1].Split('(');
                string[] velData = extraData[1].Split(',');
                Vector3 velocity = new Vector3(Convert.ToSingle(velData[0], CultureInfo.InvariantCulture), Convert.ToSingle(velData[1], CultureInfo.InvariantCulture), Convert.ToSingle(velData[2], CultureInfo.InvariantCulture));
                velocities[refs.GetComponent<SaveableObject>().sceneIndex] = Vector3.ClampMagnitude(velocity, 10);

                string[] steerState = extraData[0].Split(',');
                foreach (GPButtonSteeringWheel wheel in refs.GetComponentsInChildren<GPButtonSteeringWheel>())
                {
                    if (wheel.gameObject.activeInHierarchy)
                    {
                        wheel.currentInput = Convert.ToSingle(steerState[0], CultureInfo.InvariantCulture);
                        //wheel.SetPrivateField("locked", bool.Parse(steerState[1]));
                        Traverse.Create(wheel).Field("locked").SetValue(bool.Parse(steerState[1]));
                        break;
                    }

                }
            }
            GameState.modData.Remove(boat);
        }

        public static void SaveSailConfig(BoatRefs refs)
        {
            //Debug.Log("attempting to save data");
            string boat = modString + refs.GetComponent<SaveableObject>().sceneIndex.ToString(CultureInfo.InvariantCulture);
            if (refs.GetComponent<BoatPerformanceSwitcher>().performanceModeIsOn())
            {
                GameState.modData.Remove(boat);
                return;
            }
            string text = "";
            Mast[] masts = refs.masts;
            foreach (Mast mast in masts)
            {
                if (!mast || !mast.gameObject.activeInHierarchy || mast.sails.Count < 1)
                {
                    continue;
                }
                text += mast.orderIndex.ToString(CultureInfo.InvariantCulture) + "(";
                foreach (GameObject sail in mast.sails)
                {
                    Sail component2 = sail.GetComponent<Sail>();
                    SailConnections component3 = sail.GetComponent<SailConnections>();
                    text += component2.prefabIndex.ToString(CultureInfo.InvariantCulture) + ",";
                    //text += mast.orderIndex.ToString(CultureInfo.InvariantCulture) + ",";
                    //text += component2.currentUnroll.ToString(CultureInfo.InvariantCulture) + ",";
                    text += component3.reefController.currentLength.ToString(CultureInfo.InvariantCulture);
                    if (component3.angleControllerMid != null)
                    {
                        text += "," + component3.angleControllerMid.currentLength.ToString(CultureInfo.InvariantCulture);
                    }
                    else if (component3.angleControllerLeft != null && component3.angleControllerRight != null)
                    {
                        text += "," + component3.angleControllerLeft.currentLength.ToString(CultureInfo.InvariantCulture);
                        text += "," + component3.angleControllerRight.currentLength.ToString(CultureInfo.InvariantCulture);
                    }

                    text += "|";
                }
                text += ")";
            }
            text += "[";
            foreach (GPButtonSteeringWheel wheel in refs.GetComponentsInChildren<GPButtonSteeringWheel>())
            {
                if (wheel.gameObject.activeInHierarchy)
                {
                    text += wheel.currentInput.ToString(CultureInfo.InvariantCulture) + "," + Traverse.Create(wheel).Field("locked").GetValue();
                    break;
                }
            }
            var sw = refs.GetComponent<BoatPhysicsSwitcher>();
            Vector3 vec = sw.paused ? (Vector3)Traverse.Create(sw).Field("velocity").GetValue() : refs.GetComponent<Rigidbody>().velocity;
            //Vector3 vec = refs.GetComponent<Rigidbody>().velocity;
            text += "(" + vec.x.ToString(CultureInfo.InvariantCulture) + ",";
            text += vec.y.ToString(CultureInfo.InvariantCulture) + ",";
            text += vec.z.ToString(CultureInfo.InvariantCulture);

            if (GameState.modData.ContainsKey(boat))
            {
                GameState.modData[boat] = text;
            }
            else
            {
                GameState.modData.Add(boat, text);
            }
            //Debug.Log("saving data: " + text);

        }
    }
}
