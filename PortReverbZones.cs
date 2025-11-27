using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks
{
    public struct ReverbZoneData
    {
        public Vector3 localPosition;
        public float minDistance;
        public float maxDistance;
        public AudioReverbPreset reverbPreset;
    }
    internal static class PortReverbZones
    {
        public static Dictionary<int, ReverbZoneData[]> reverbZones = new Dictionary<int, ReverbZoneData[]>
        {

            { 18, new ReverbZoneData[] {// siren song
                new ReverbZoneData { //outdoor
                    localPosition = new Vector3(-51, 0, -63), minDistance = 120, maxDistance = 180, reverbPreset = AudioReverbPreset.Mountains },
                new ReverbZoneData {// port office
                    localPosition = new Vector3(7.2f, 0, 3f), minDistance = 4, maxDistance = 5, reverbPreset = AudioReverbPreset.Livingroom },
                }
            },
            { 28, new ReverbZoneData[] {// firefly grotto
                new ReverbZoneData {//main
                    localPosition = new Vector3(48, -24, 43), minDistance = 170, maxDistance = 245, reverbPreset = AudioReverbPreset.Quarry },
                new ReverbZoneData {//north entrance
                    localPosition = new Vector3(-241, -50, -48), minDistance = 160, maxDistance = 230, reverbPreset = AudioReverbPreset.Cave },
                new ReverbZoneData {//port office
                    localPosition = new Vector3(-8.47f, 1, 3.3f), minDistance = 4, maxDistance = 5, reverbPreset = AudioReverbPreset.Livingroom },
                }
            },
        };
    }
}
