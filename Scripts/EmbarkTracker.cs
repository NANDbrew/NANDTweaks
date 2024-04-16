using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Scripts
{
    internal class EmbarkTracker : MonoBehaviour
    {
        public List<Collider>embarkColliders;

        private void Awake()
        {
            embarkColliders = new List<Collider>();
        }
    }
}
