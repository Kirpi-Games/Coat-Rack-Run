using System.Collections.Generic;
using Akali.Common;
using UnityEngine;

public class ClothStack : Singleton<ClothStack>
{
    public List<Clothes> stack;
    private Collider col;

    private void SetColliderEnabled() => col.enabled = !col.enabled;

    private void Awake()
    {
        stack = new List<Clothes>();
        col = gameObject.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.IsCloth()) AddStack(other.gameObject.GetComponent<Clothes>());
    }

    public void AddStack(Clothes cloth)
    {
        if (stack.Count < 1)
        {
            cloth.transform.parent = transform;
            cloth.transform.position = transform.position;
            cloth.SetLayer();
            SetColliderEnabled();
            stack.Add(cloth);
            return;
        }
        
        SetLinear(cloth);
        cloth.SetLayer();
        stack.Add(cloth);
    }

    public void RemoveStack()
    {
        
    }

    private void SetLinear(Clothes cloth)
    {
        cloth.transform.parent = transform;
        var last = stack[stack.Count - 1].transform;
        var position = last.position;
        position.z += cloth.col.GetLength();
        cloth.transform.position = position;
    }
}