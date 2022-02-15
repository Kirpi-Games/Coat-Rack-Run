using System;
using UnityEngine;

namespace Strategies.Door
{
    public class DoorUpgrade : DoorBase
    {
        private void Start()
        {
            type = DoorTypes.Upgrade;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.IsClothStack()) other.GetCloth().UpgradeCloth();
        }
    }
}