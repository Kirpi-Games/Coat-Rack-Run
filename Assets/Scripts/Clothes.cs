using Akali.Scripts.Utilities;
using UnityEngine;

public enum ClothesTypes: byte
{
    Casual,
    Dress,
    Suit,
}

public class Clothes : MonoBehaviour
{
    public ClothesTypes type;
    [HideInInspector] public Collider col;

    private void Awake()
    {
        gameObject.layer = Constants.LayerCloth;
        col = gameObject.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (col.IsCloth() && other.IsClothStack()) ClothStack.Instance.AddStack(this);
    }

    public void SetLayer()
    {
        if (col.IsCloth())
        {
            gameObject.layer = Constants.LayerClothStack;
            return;
        }

        gameObject.layer = Constants.LayerCloth;
    }
}
