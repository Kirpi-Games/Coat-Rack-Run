using UnityEngine;

public enum ClothTypes : byte
{
    Casual,
    Dress,
    Suit,
}

public class ClothScript : MonoBehaviour
{
    public ClothTypes type;
}
