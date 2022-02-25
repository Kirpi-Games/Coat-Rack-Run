using UnityEngine;

namespace Strategies.Girl
{
    public class GirlDressed : GirlBase
    {
        private void Awake()
        {
            type = GirlTypes.Dressed;
        }
    }
}
