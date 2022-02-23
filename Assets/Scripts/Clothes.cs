using System.Collections.Generic;
using Akali.Scripts.Utilities;
using UnityEngine;

public class Clothes : MonoBehaviour
{
    [HideInInspector] public Collider col;
    [HideInInspector] public ClothScript activeCloth;

    [HideInInspector] public int id = -1;
    [HideInInspector] public Vector3 startScale;
    [HideInInspector] public List<ClothScript> subClothes = new();
    
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
        
        if (activeCloth.state == ClothStates.Level2) return;
        var nextType = GetNextCloth(activeCloth.state);
        var nextCloth = subClothes.Find(cloth => cloth.state == nextType);
        nextCloth.gameObject.SetActive(true);
        activeCloth.gameObject.SetActive(false);
        activeCloth = nextCloth;
    }

    public ClothStates GetNextCloth(ClothStates state)
    {
        return state switch
        {
            ClothStates.Level0 => ClothStates.Level1,
            ClothStates.Level1 => ClothStates.Level2,
            _ => ClothStates.Level2,
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