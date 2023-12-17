using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Scripts
{
    internal class ShipItemMoveOnAltActivate : MonoBehaviour
    {
        public bool moving;

        public ShipItem shipItem;

        public float targetDistance = 1.15f;

        public float defaultDistance;

        public float targetHeight;

        public float defaultHeight;

        private void Awake()
        {
            shipItem = GetComponent<ShipItem>();
            defaultDistance = shipItem.holdDistance;
            defaultHeight = shipItem.holdHeight;
        }
    }
}
