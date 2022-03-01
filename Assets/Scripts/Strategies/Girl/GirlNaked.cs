using System.Collections.Generic;
using UnityEngine;

namespace Strategies.Girl
{
    public class GirlNaked : GirlBase
    {
        [HideInInspector] public List<Hairs> hairs = new();
        
        private void Awake()
        {
            type = GirlTypes.Naked;
            animator = gameObject.GetComponent<Animator>();
            gameObject.GetComponentsInChildren(hairs);
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
