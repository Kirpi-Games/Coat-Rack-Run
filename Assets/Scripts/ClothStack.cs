using System.Collections.Generic;
using System.Linq;
using Akali.Common;
using Akali.Scripts.Core;
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

    private void Update()
    {
        LerpStack(SwerveController.Instance.transform.position.x);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.IsCloth()) AddStack(other.GetCloth());
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

        SetEndOfStack(cloth);
        cloth.SetLayer();
        stack.Add(cloth);
    }

    public void RemoveStack()
    {
    }

    private void SetEndOfStack(Clothes cloth)
    {
        cloth.transform.parent = transform;
        var last = stack[stack.Count - 1].transform;
        var position = last.position;
        position.z += cloth.col.GetLength();
        cloth.transform.position = position;
    }

    private void LerpStack(float x)
    {
        if (stack.Count < 1) return;

        var first = stack.First();
        var localPos = first.transform.localPosition;
        localPos.x = x;
        first.transform.localPosition = localPos;

        if (stack.Count < 2) return;
        for (var index = 1; index < stack.Count; index++)
        {
            var localPosition = stack[index].transform.localPosition;
            localPosition.x = Mathf.Lerp(localPosition.x, stack[index - 1].transform.localPosition.x, 0.375f);
            stack[index].transform.localPosition = localPosition;
        }
    }
}