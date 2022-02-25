using Akali.Scripts.Utilities;
using UnityEngine;

public class BoneActivator : MonoBehaviour
{
    private DynamicBone bone;
    
    private void Awake()
    {
        bone = gameObject.GetComponent<DynamicBone>();
        bone.enabled = false;
        ClothStack.Instance.LayerChanged += ActivateDynamicBone;
    }

    private void ActivateDynamicBone(int layer)
    {
        if (layer == Constants.LayerCloth)
        {
            bone.enabled = false;
            return;
        }

        bone.enabled = true;
    }
}
