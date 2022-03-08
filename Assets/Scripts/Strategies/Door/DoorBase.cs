using UnityEngine;

namespace Strategies.Door
{
    public enum DoorTypes : byte
    {
        None,
        Upgrade,
        Sell,
    }
    
    public class DoorBase : MonoBehaviour
    {
        [SerializeField] protected DoorTypes type;
    }
}
