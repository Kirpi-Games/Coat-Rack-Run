using System.Collections.Generic;
using Akali.Scripts.Utilities;
using UnityEngine;

public class Clothes : MonoBehaviour
{
    [HideInInspector] public Collider col;
    [HideInInspector] public ClothScript activeCloth;

    public int id = -1;
    public Vector3 startScale;
    public List<ClothScript> subClothes = new();
    
    public bool IsLast => this == ClothStack.Instance.stack[ClothStack.Instance.stack.Count - 1];

    private void Awake()
    {
        gameObject.layer = Constants.LayerCloth;
        gameObject.GetComponentsInChildren(subClothes);
        for (var i = 0; i < subClothes.Count; i++)
        {
            if (i != 0) subClothes[i].gameObject.SetActive(false);
        }

        UpgradeCloth();
        col = gameObject.GetComponent<Collider>();
        startScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (col.IsCloth() && other.IsClothStack()) ClothStack.Instance.AddCloth(this);
    }

    public void UpgradeCloth()
    {
        if (activeCloth == null)
        {
            activeCloth = subClothes[0];
            return;
        }
        
        if (activeCloth.type == ClothTypes.Suit) return;
        var nextType = GetNextCloth(activeCloth.type);
        var nextCloth = subClothes.Find(cloth => cloth.type == nextType);
        nextCloth.gameObject.SetActive(true);
        activeCloth.gameObject.SetActive(false);
        activeCloth = nextCloth;
    }

    public ClothTypes GetNextCloth(ClothTypes type)
    {
        return type switch
        {
            ClothTypes.Casual => ClothTypes.Dress,
            ClothTypes.Dress => ClothTypes.Suit,
            _ => ClothTypes.Suit,
        };
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