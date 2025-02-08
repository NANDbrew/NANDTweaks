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
        private const char bigSep = '[';
        private const char smallSep = ',';
        private const char opener = '(';
        private const char closer = ')';
        private const char pipe = '|';

        public static IEnumerator LoadAfterDelay()
        {
            yield return new WaitUntil(() => GameState.playing && !GameState.justStarted);// !FloatingOriginManager.instance.GetPrivateField<bool>("instantShifting"));// && !GameState.justStarted);

            foreach (BoatRefs gameObject in GameObject.FindObjectsOfType<BoatRefs>())
            {
                if (!gameObject.GetComponent<PurchasableBoat>().isPurchased()) continue;
                SaveLoader.LoadSailConfig(gameObject);

                if (gameObject.GetComponent<BoatPerformanceSwitcher>().performanceModeIsOn())
                {
                    Debug.Log("skipping velocity for " + gameObject.name + " due to performance mode");
                }
                else if (velocities.TryGetValue(gameObject.GetComponent<SaveableObject>().sceneIndex, out Vector3 vel))
                {
                    gameObject.GetComponent<Rigidbody>().velocity = vel;
                    Debug.Log("set velocity for " + gameObject.name + " to " + vel);
                }
                else Debug.Log("skipping velocity for " + gameObject.name + "; no saved velocity");
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

            char[] bigSep1 = new char[1] { bigSep };
            char[] smallSep1 = new char[1] { smallSep };
            char[] opener1 = new char[1] { opener };
            char[] closer1 = new char[1] { closer };


            string[] slug = GameState.modData[boat].Split(bigSep1/*, StringSplitOptions.RemoveEmptyEntries*/);
            //Debug.Log("loading data: " + slug);
            if (slug[0].Length > 0)
            {
                string[] masts = slug[0].Split(closer1, StringSplitOptions.RemoveEmptyEntries);
                foreach (string mast in masts)
                {
                    string[] foo = mast.Split(opener1, StringSplitOptions.RemoveEmptyEntries);
                    int mastIndex = Convert.ToInt32(foo[0]);
                    //Debug.Log("nandTweaks: loading sails for mast " + refs.masts[mastIndex]);
                    string[] sails = foo[1].Split(new char[] { pipe }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < sails.Length; i++)
                    {
                        if (i >= refs.masts[mastIndex].sails.Count) break;
                        GameObject installedSail = refs.masts[mastIndex].sails[i];
                        string[] sailInfo = sails[i].Split(smallSep1, StringSplitOptions.RemoveEmptyEntries);
                        if (installedSail.GetComponent<Sail>().prefabIndex == Convert.ToInt32(sailInfo[0], CultureInfo.InvariantCulture))
                        {
                            //installedSail.GetComponent<Sail>().currentUnroll = Convert.ToSingle(sailInfo[1], CultureInfo.InvariantCulture);
                            SailConnections component3 = installedSail.GetComponent<SailConnections>();

                            component3.reefController.currentLength = Convert.ToSingle(sailInfo[1], CultureInfo.InvariantCulture);
                            Debug.Log(component3 + " : " + component3.reefController.currentLength);
                            if (component3.angleControllerMid != null && sailInfo.Length == 3)
                            {
                                component3.angleControllerMid.currentLength = Convert.ToSingle(sailInfo[2], CultureInfo.InvariantCulture);
                                Traverse.Create(component3.angleControllerMid).Field("changed").SetValue(true);
                                Debug.Log("mid angle controller length = " + component3.angleControllerMid.currentLength);
                            }
                            else if (component3.angleControllerLeft != null && component3.angleControllerRight != null && sailInfo.Length == 4)
                            {
                                component3.angleControllerLeft.currentLength = Convert.ToSingle(sailInfo[2], CultureInfo.InvariantCulture);
                                component3.angleControllerRight.currentLength = Convert.ToSingle(sailInfo[3], CultureInfo.InvariantCulture);
                                Traverse.Create(component3.angleControllerLeft).Field("changed").SetValue(true);
                                Traverse.Create(component3.angleControllerRight).Field("changed").SetValue(true);
                                Debug.Log("left & right angle controllers exist");
                            }
                        }
                    }
                }
            }
            string[] extraData = slug[1].Split(opener1, StringSplitOptions.RemoveEmptyEntries);

            //Debug.Log("starting wheel stuff");
            string[] steerState = extraData[0].Split(smallSep1, StringSplitOptions.RemoveEmptyEntries);
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

            if (extraData.Length >= 2)
            {
                string[] velData = extraData[1].Split(smallSep1, StringSplitOptions.RemoveEmptyEntries);
                Vector3 velocity = new Vector3(Convert.ToSingle(velData[0], CultureInfo.InvariantCulture), Convert.ToSingle(velData[1], CultureInfo.InvariantCulture), Convert.ToSingle(velData[2], CultureInfo.InvariantCulture));

                velocities[refs.GetComponent<SaveableObject>().sceneIndex] = Vector3.ClampMagnitude(velocity, 15);
            }

            //Debug.Log("stuff???");
            if (slug.Length >= 3 && Plugin.toggleDoors.Value)
            {
                //Debug.Log("heyyyy");
                string[] doorData = slug[2].Split(smallSep1, StringSplitOptions.RemoveEmptyEntries);
                GPButtonTrapdoor[] doors = refs.GetComponentsInChildren<GPButtonTrapdoor>();
                int j = 0;
                for (int i = 0; i < doors.Length; i++)
                {
                    if (j >= doorData.Length) break;
                    GPButtonTrapdoor door = doors[i];
                    if (door.gameObject.activeInHierarchy)
                    {
                        if (doorData[j] == "1")
                        {
                            door.OnActivate();
                            //Debug.Log("NT: toggled door #" + i + ": " + door.name);
                            //Debug.Log("NT: door data # " + j);
                        }
                        j++;
                    }
                }
            }
            
            GameState.modData.Remove(boat);
        }

        public static void SaveSailConfig(BoatRefs refs)
        {
            //Debug.Log("attempting to save data");
            string boat = modString + refs.GetComponent<SaveableObject>().sceneIndex.ToString(CultureInfo.InvariantCulture);
/*            if (refs.GetComponent<BoatPerformanceSwitcher>().performanceModeIsOn())
            {
                GameState.modData.Remove(boat);
                return;
            }*/
            string text = "";
            Mast[] masts = refs.masts;
            foreach (Mast mast in masts)
            {
                if (!mast || !mast.gameObject.activeInHierarchy || mast.sails.Count < 1)
                {
                    continue;
                }
                text += mast.orderIndex.ToString(CultureInfo.InvariantCulture) + opener;
                foreach (GameObject sail in mast.sails)
                {
                    Sail component2 = sail.GetComponent<Sail>();
                    SailConnections component3 = sail.GetComponent<SailConnections>();
                    text += component2.prefabIndex.ToString(CultureInfo.InvariantCulture) + smallSep;
                    //text += mast.orderIndex.ToString(CultureInfo.InvariantCulture) + smallSep;
                    //text += component2.currentUnroll.ToString(CultureInfo.InvariantCulture) + smallSep;
                    text += component3.reefController.currentLength.ToString(CultureInfo.InvariantCulture);
                    if (component3.angleControllerMid != null)
                    {
                        text += smallSep + component3.angleControllerMid.currentLength.ToString(CultureInfo.InvariantCulture);
                    }
                    else if (component3.angleControllerLeft != null && component3.angleControllerRight != null)
                    {
                        text += smallSep + component3.angleControllerLeft.currentLength.ToString(CultureInfo.InvariantCulture);
                        text += smallSep + component3.angleControllerRight.currentLength.ToString(CultureInfo.InvariantCulture);
                    }

                    text += pipe;
                }
                text += closer;
            }
            text += bigSep;
            foreach (GPButtonSteeringWheel wheel in refs.GetComponentsInChildren<GPButtonSteeringWheel>())
            {
                if (wheel.gameObject.activeInHierarchy)
                {
                    text += wheel.currentInput.ToString(CultureInfo.InvariantCulture) + smallSep + Traverse.Create(wheel).Field("locked").GetValue();
                    break;
                }
            }

            var sw = refs.GetComponent<BoatPhysicsSwitcher>();
            Vector3 vec = sw.paused ? (Vector3)Traverse.Create(sw).Field("velocity").GetValue() : refs.GetComponent<Rigidbody>().velocity;

            text += opener;
            text += vec.x.ToString(CultureInfo.InvariantCulture) + smallSep;
            text += vec.y.ToString(CultureInfo.InvariantCulture) + smallSep;
            text += vec.z.ToString(CultureInfo.InvariantCulture);

            text += bigSep;
            foreach (GPButtonTrapdoor door in refs.GetComponentsInChildren<GPButtonTrapdoor>())
            {
                if (door.gameObject.activeInHierarchy)
                {
                    text += door.IsOpen() ? "1" : "0";
                    text += smallSep;
                }
            }
            if (GameState.modData.ContainsKey(boat))
            {
                GameState.modData[boat] = text;
            }
            else
            {
                GameState.modData.Add(boat, text);
            }
#if DEBUG
            Debug.Log("saving data: " + text);
#endif
        }
    }
}
