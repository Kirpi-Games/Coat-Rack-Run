using System.Collections.Generic;
using Akali.Scripts.Utilities;
using UnityEngine;
using PlayerPrefs = Akali.Scripts.Utilities.PlayerPrefs;

public class Clothes : MonoBehaviour
{
    public ParticleSystem starExplosion;
    [HideInInspector] public Collider col;
    [HideInInspector] public ClothScript activeCloth;
    [HideInInspector] public List<ClothScript> subClothes = new List<ClothScript>();
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
        var nextLevel = GetNextLevel();
        var nextCloth = subClothes.Find(cloth => cloth.type == nextType && cloth.level == nextLevel);
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

    public ClothLevel GetNextLevel()
    {
        var currentLevel = PlayerPrefs.GetLevelText();
        return (currentLevel >= 0) switch
        {
            true when currentLevel < 6 => ClothLevel.Level0,
            _ => (currentLevel >= 6) switch
            {
                true when currentLevel < 11 => ClothLevel.Level1,
                _ => (currentLevel >= 11) switch
                {
                    true when currentLevel < 21 => ClothLevel.Level2,
                    _ => (currentLevel >= 21) switch
                    {
                        true when currentLevel < 31 => ClothLevel.Level0,
                        _ => (currentLevel >= 31) switch
                        {
                            true when currentLevel < 41 => ClothLevel.Level1,
                            _ => (currentLevel >= 41) switch
                            {
                                true when currentLevel < 51 => ClothLevel.Level2,
                                _ => (currentLevel >= 51) switch
                                {
                                    true when currentLevel < 61 => ClothLevel.Level0,
                                    _ => (currentLevel >= 61) switch
                                    {
                                        true when currentLevel < 71 => ClothLevel.Level1,
                                        _ => (currentLevel >= 71) switch
                                        {
                                            true when currentLevel < 81 => ClothLevel.Level2,
                                            _ => (currentLevel >= 81) switch
                                            {
                                                true when currentLevel < 91 => ClothLevel.Level0,
                                                _ => (currentLevel >= 91) switch
                                                {
                                                    true when currentLevel < 101 => ClothLevel.Level1,
                                                    _ => (currentLevel >= 101) switch
                                                    {
                                                        true when currentLevel < 111 => ClothLevel.Level2,
                                                        _ => (currentLevel >= 111) switch
                                                        {
                                                            true => ClothLevel.Level0,
                                                            _ => ClothLevel.Level0,
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
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