using UnityEngine;

public enum ClothStates : byte
{
    Level0,
    Level1,
    Level2,
}

public class ClothScript : MonoBehaviour
{
    public ClothStates state;
}
