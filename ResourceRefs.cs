using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks
{
    internal class ResourceRefs
    {

        public static readonly Vector3[] triggerLocs =
        {            // LOCAL COORDINATES
            new Vector3(-0.3f, 1.2f, 1.2f), // gold rock city
            new Vector3(-0.8f, 1.2f, 0.9f), // al'nilem
            new Vector3(0.75f, 1.1f, 0.5f), // neverdin
            new Vector3(1.42f, 1.2f, -0.01f), // albacore town
            new Vector3(1.1f, 1.2f, 1.1f), // alchemist's island
            new Vector3(0.0f, 0.0f, 0.00f), // academy (skip this one, it's outside)
            new Vector3(-1.1f, 1.2f, 1.1f), // oasis
            new Vector3(0f, 0f, 0f), // 
            new Vector3(0f, 0f, 0f), // 
            new Vector3(0.1f, 1.2f, 2.82f), // dragon cliffs
            new Vector3(0.0f, 1.2f, 1.05f), // sanctuary
            new Vector3(1.3f, 1.3f, 2.1f), // crab beach 
            new Vector3(0.1f, 1.2f, 0.55f), // new port
            new Vector3(1.1f, 1.2f, 2.45f), // sage hills
            new Vector3(0.0f, 1.2f, 0.95f), // serpent isle
            new Vector3(1.85f, 1.7f, 1.35f), // fort aestrin
            new Vector3(-2.86f, 1.7f, 0.48f), // sunspire
            new Vector3(-2.86f, 1.7f, 0.48f), // mount malefic
            new Vector3(-1.1f, 1.7f, -0.1f), // siren song
            new Vector3(-2.6f, 1.7f, 0.4f), // eastwind
            new Vector3(1.3122f, 1.7f, 0.1959f), // happy bay
            new Vector3(-2.2f, 1.7f, -0.5f), // chronos
            new Vector3(0.65f, 1.2f, 1.25f), // kicia bay
            new Vector3(0.1f, 1.2f, 3.1f), // fire fish town
            new Vector3(0f, 0f, 0f), // on'na (skip this one, it's outside)
            new Vector3(2.1f, 1.2f, 0.5f), // sen'na
            new Vector3(2.1f, 1.2f, 0.5f), // spacer for future islands
            new Vector3(2.1f, 1.2f, 0.5f), // spacer for future islands
            new Vector3(2.1f, 1.2f, 0.5f), // spacer for future islands
            new Vector3(2.1f, 1.2f, 0.5f), // spacer for future islands
            new Vector3(-42.1f, 2.5f, -35.8f), // gold rock city art shop
            new Vector3(-120.4f, 0.0f, -88.3f), // fort aestrin art shop
            new Vector3(-133.8f, 8f, 6.4f), // chronos art shop
            new Vector3(6.8f, 1.2f, 11.9f), // sen'na dock shop
            new Vector3(14.5f, 9.0f, 0.1f), // sage hills food shop
        };

        public static readonly Quaternion[] triggerRotations =
        {   // world, not local
            new Quaternion(0.0f, 0.4653f, 0.0f, 0.8852f), // gold rock city
            new Quaternion(0.0f, 0.2079f, 0.0f, 0.9781f), // al'nilem
            new Quaternion(0.0f, 0.5188f, 0.0f, 0.8549f), // neverdin
            new Quaternion(0.0f, -0.6307f, 0.0f, 0.7760f), // albacore town
            new Quaternion(0.0f, 0.2588f, 0.0f, 0.9659f), // alchemist's island
            new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), // academy (skip this one, it's outside)
            new Quaternion(0.0f, 0.2924f, 0.0f, 0.9563f), // oasis
            new Quaternion(0f, 0f, 0f, 0f), // 
            new Quaternion(0f, 0f, 0f, 0f), // 
            new Quaternion(0.0f, 0.2797f, 0.0f, 0.9601f), // dragon cliffs
            new Quaternion(0.0f, 0.5345f, 0.0f, 0.8452f), // sanctuary
            new Quaternion(0.0f, -0.9302f, 0.0f,-0.3671f), // crab beach
            new Quaternion(0.0f, -0.7076f, 0.0f, 0.7066f), // new port
            new Quaternion(0.0f, 0.1564f, 0.0f, 0.9877f), // sage hills
            new Quaternion(0.0f, 0.3983f, 0.0f, 0.9173f), // serpent isle
            new Quaternion(0.0f, -0.2164f, 0.0f, -0.9763f), // fort aestrin
            new Quaternion(0.0f, -0.6009f, 0.0f, 0.7993f), // sunspire
            new Quaternion(0.0f, 0.6293f, 0.0f, 0.7771f), // mount malefic
            new Quaternion(0.0f, -0.9899f, 0.0f, -0.1415f), // siren song
            new Quaternion(0.0f, 0.0436f, 0.0f, -0.9990f), // eastwind
            new Quaternion(0.0f, 0.3338f, 0.0f, 0.9426f), // happy bay
            new Quaternion(0.0f, 0.7133f, 0.0f, 0.7009f), // chronos
            new Quaternion(0.0f, 0.8192f, 0.0f, 0.5736f), // kicia bay
            new Quaternion(0.0f, 0.5299f, 0.0f, 0.8480f), // fire fish town
            new Quaternion(0f, 0f, 0f, 0f), // on'na (skip this one, it's outside)
            new Quaternion(0.0f, 0.8870f, 0.0f, 0.4617f), // sen'na
            new Quaternion(0.0f, 0.8870f, 0.0f, 0.4617f), // spacer for future islands
            new Quaternion(0.0f, 0.8870f, 0.0f, 0.4617f), // spacer for future islands
            new Quaternion(0.0f, 0.8870f, 0.0f, 0.4617f), // spacer for future islands
            new Quaternion(0.0f, 0.8870f, 0.0f, 0.4617f), // spacer for future islands
            new Quaternion(0.0f, 0.9459f, 0.0f, 0.3244f), // gold rock city art shop
            new Quaternion(0.0f, 0.0122f, 0.0f, -0.9999f), // fort aestrin art shop
            new Quaternion(0.0f, 0.3600f, 0.0f, 0.9330f), // chronos art shop
            new Quaternion(0.0f, 0.8870f, 0.0f, 0.4617f), // sen'na dock shop
            new Quaternion(0.0f, -0.3792f, 0.0f, -0.9253f), // sage hills food shop
        };
        public static readonly Vector3[] colSizes =
        {
            new Vector3(5.8f, 3.0f, 5.5f), // gold rock city
            new Vector3(5.8f, 3.0f, 4.8f), // al'nilem
            new Vector3(5.8f, 3.0f, 4.8f), // neverdin
            new Vector3(5.8f, 3.0f, 5.0f), // albacore town
            new Vector3(6.3f, 3.5f, 5.5f), // alchemist's island
            new Vector3(6.0f, 3.0f, 6.0f), // academy (skip this one, it's outside)
            new Vector3(6.0f, 3.0f, 6.0f), // oasis
            new Vector3(0f, 0f, 0f), // 
            new Vector3(0f, 0f, 0f), // 
            new Vector3(5.1f, 2.7f, 8.0f), // dragon cliffs
            new Vector3(5.1f, 2.7f, 8.0f), // sanctuary
            new Vector3(5.1f, 2.7f, 8.0f), // crab beach
            new Vector3(5.1f, 2.7f, 8.0f), // new port
            new Vector3(5.1f, 2.7f, 8.0f), // sage hills
            new Vector3(5.1f, 2.7f, 8.0f), // serpent isle
            new Vector3(7.9f, 4.0f, 7.9f), // fort aestrin
            new Vector3(7.9f, 4.0f, 7.9f), // sunspire
            new Vector3(7.9f, 4.0f, 7.9f), // mount malefic
            new Vector3(7.9f, 4.0f, 7.9f), // siren song
            new Vector3(7.9f, 4.0f, 7.9f), // eastwind
            new Vector3(7.9f, 4.0f, 7.9f), // happy bay
            new Vector3(7.2f, 4.0f, 7.2f), // chronos
            new Vector3(5.1f, 2.7f, 8.0f), // kicia bay
            new Vector3(5.1f, 2.7f, 8.0f), // fire fish town
            new Vector3(0.0f, 0.0f, 0.0f), // on'na (skip this one, it's outside)
            new Vector3(5.1f, 2.7f, 8.0f), // sen'na
            new Vector3(5.1f, 2.7f, 8.0f), // spacer for future islands
            new Vector3(5.1f, 2.7f, 8.0f), // spacer for future islands
            new Vector3(5.1f, 2.7f, 8.0f), // spacer for future islands
            new Vector3(5.1f, 2.7f, 8.0f), // spacer for future islands
            new Vector3(5.8f, 3.5f, 5.5f), // gold rock city art shop
            new Vector3(8.3f, 4.0f, 8.3f), // fort aestrin art shop
            new Vector3(7.0f, 4.0f, 7.0f), // chronos art shop
            new Vector3(5.1f, 2.7f, 8.0f), // sen'na dock shop
            new Vector3(5.0f, 2.7f, 8.0f), // sage hills food shop
        };
    }
}
