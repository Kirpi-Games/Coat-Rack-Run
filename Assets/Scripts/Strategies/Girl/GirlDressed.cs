using System.Collections.Generic;
using UnityEngine;

namespace Strategies.Girl
{
    public class GirlDressed : GirlBase
    {
        [HideInInspector] public List<GirlCloth> girlCloths = new();
        [HideInInspector] public List<Hairs> hairs = new();

        private void Awake()
        {
            type = GirlTypes.Dressed;
            gameObject.GetComponentsInChildren(girlCloths);
            gameObject.GetComponentsInChildren(hairs);
        }

        public void SetCloth(ClothTypes clothType)
        {
            for (var i = 0; i < girlCloths.Count; i++)
            {
                if (girlCloths[i].type == clothType) return;
                girlCloths[i].gameObject.SetActive(false);
            }
        }

        public void SetHair()
        {
            for (var i = 0; i < hairs.Count; i++)
            {
                if (hairs[i].id == hairId) return;
                hairs[i].gameObject.SetActive(false);
            }
        }
    }
}
