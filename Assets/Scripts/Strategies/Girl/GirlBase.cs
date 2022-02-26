using UnityEngine;

namespace Strategies.Girl
{
    public enum GirlTypes : byte
    {
        None,
        Naked,
        Dressed,
    }
    
    public class GirlBase : MonoBehaviour
    {
        [HideInInspector] public GirlTypes type;
        [HideInInspector] public HairId hairId;
    }
}
