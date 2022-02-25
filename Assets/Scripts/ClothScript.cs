using UnityEngine;

public enum ClothTypes : byte
{
    Pyjama,
    Casual,
    Business,
    Prom,
}

public class ClothScript : MonoBehaviour
{
    public ClothTypes type;
}
