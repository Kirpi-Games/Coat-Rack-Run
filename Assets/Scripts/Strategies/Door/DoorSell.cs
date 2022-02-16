using System;
using UnityEngine;

namespace Strategies.Door
{
    public class DoorSell : DoorBase
    {
        private void Start()
        {
            type = DoorTypes.Sell;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.IsClothStack()) Sell(other);
        }

        private void Sell(Collider other)
        {
            ClothStack.Instance.RemoveStack(other.GetCloth());
        }
    }
}
