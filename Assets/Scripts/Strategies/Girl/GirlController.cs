using System;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.Girl
{
    public class GirlController : MonoBehaviour
    {
        public HairId hairId;

        private GirlNaked naked;
        private GirlDressed dressed;

        private void Awake()
        {
            naked = gameObject.GetComponentInChildren<GirlNaked>();
            dressed = gameObject.GetComponentInChildren<GirlDressed>();
            naked.hairId = hairId;
            dressed.hairId = hairId;
            SetActiveNaked();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.IsClothStack()) SetActiveDressed(other.GetCloth().activeCloth.type);
        }

        private void SetActiveNaked()
        {
            dressed.gameObject.SetActive(false);
            naked.SetHair();
        }

        public void SetActiveDressed(ClothTypes type)
        {
            naked.gameObject.SetActive(false);
            dressed.gameObject.SetActive(true);
            dressed.SetCloth(type);
            dressed.SetHair();
        }
    }
}
