using Akali.Scripts.Utilities;
using UnityEngine;

public static class Extensions
{
    private const float Threshold = .1f;
    
    public static float GetLength(this Collider collider)
    {
        return collider.bounds.size.z + Threshold;
    }

    public static Clothes GetCloth(this Collider other)
    {
        return other.gameObject.GetComponent<Clothes>();
    }

    public static bool IsCloth(this Collider other)
    {
        return other.gameObject.layer.Equals(Constants.LayerCloth);
    }

    public static bool IsClothStack(this Collider other)
    {
        return other.gameObject.layer.Equals(Constants.LayerClothStack);
    }
}
