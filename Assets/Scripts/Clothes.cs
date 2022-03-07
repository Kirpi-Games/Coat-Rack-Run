using System.Collections.Generic;
using Akali.Scripts.Utilities;
using UnityEngine;

public class Clothes : MonoBehaviour
{
    public ParticleSystem starExplosion;
    [HideInInspector] public Collider col;
    [HideInInspector] public ClothScript activeCloth;
    [HideInInspector] public List<ClothScript> subClothes = new();
    [HideInInspector] public int id = -1;
    [HideInInspector] public Vector3 startScale;

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
        if (col.IsCloth() && other.IsClothStack())
        {
            Taptic.Medium();
            ClothStack.Instance.AddCloth(this);
        }
    }

    public void UpgradeCloth()
    {
        if (activeCloth == null)
        {
            activeCloth = subClothes[0];
            return;
        }

        if (activeCloth.type == ClothTypes.Prom) return;
        var nextType = GetNextCloth(activeCloth.type);
        var nextCloth = subClothes.Find(cloth => cloth.type == nextType);
        nextCloth.gameObject.SetActive(true);
        activeCloth.gameObject.SetActive(false);
        activeCloth = nextCloth;
        activeCloth.boneActivator.SetDynamicBone(true);
    }

    public ClothTypes GetNextCloth(ClothTypes type)
    {
        return type switch
        {
            ClothTypes.Pyjama => ClothTypes.Casual,
            ClothTypes.Casual => ClothTypes.Business,
            ClothTypes.Business => ClothTypes.Prom,
            _ => ClothTypes.Prom,
        };
    }

    public void SetLayer()
    {
        if (col.IsCloth())
        {
            gameObject.layer = Constants.LayerClothStack;
            activeCloth.boneActivator.SetDynamicBone(true);
            return;
        }

        gameObject.layer = Constants.LayerCloth;
        activeCloth.boneActivator.SetDynamicBone(false);
    }
}