using System.Collections.Generic;
using UnityEngine;

namespace NANDTweaks
{
    internal readonly struct InnTriggers
    {
        public static readonly Dictionary<int, Vector3> triggerLocs = new Dictionary<int, Vector3>
        {            // LOCAL COORDINATES
            { 13, new Vector3(-0.148f, -0.17f, -0.31f) }, // island ?? sage hills
            { 38, new Vector3(0.0f, 0.0f, -0.17f) }, // island ?? turtle island
            { 39, new Vector3(-0.05f, -0.04f, 0.1f) }, // island 39 onsen
            //{ 38, new Vector3(1.42f, 1.2f, -0.01f) }, // 

        };

        public static readonly Dictionary<int, Quaternion> triggerRotations = new Dictionary<int, Quaternion>
        {   // local rotations
            { 13, Quaternion.identity }, // island ?? sage hills
            { 38, Quaternion.identity }, // island ?? turtle island
            { 39, Quaternion.identity }, // island 39 onsen
            //new Quaternion(0.0f, -0.6307f, 0.0f, 0.7760f), // 
        };
        public static readonly Dictionary<int, Vector3> colSizes = new Dictionary<int, Vector3>
        {
            { 13, new Vector3(0.8f, 0.4f, 0.38f) }, // island ?? sage hills
            { 38, new Vector3(0.9f, 0.6f, 0.5f) }, // island ?? turtle island
            { 39, new Vector3(0.55f, 0.5f, 0.7f) }, // island 39 onsen
            //new Vector3(5.8f, 3.0f, 5.0f), // 
        };
    }
}
