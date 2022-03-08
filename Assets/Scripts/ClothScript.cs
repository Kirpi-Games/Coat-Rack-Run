using UnityEngine;

public enum ClothTypes : byte
{
    Pyjama,
    Casual,
    Business,
    Prom,
}

public enum ClothLevel : byte
{
    Level0,
    Level1,
    Level2,
}

public class ClothScript : MonoBehaviour
{
    public ClothTypes type;
    public ClothLevel level;
    public BoneActivator boneActivator;
    public ParticleSystem dressParticle;
}
