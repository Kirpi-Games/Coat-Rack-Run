using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Akali.Common;
using Akali.Scripts.Core;
using DG.Tweening;
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

    #region List Operations

    public void AddStack(Clothes cloth)
    {
        if (stack.Count < 1)
        {
            cloth.transform.parent = transform;
            cloth.transform.position = transform.position;
            cloth.SetLayer();
            SetColliderEnabled();
            stack.Add(cloth);
            ScaleStack();
            return;
        }

        SetEndOfStack(cloth);
        cloth.SetLayer();
        stack.Add(cloth);
        ScaleStack();
    }

    public void RemoveStack(Clothes clothes)
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

    #endregion

    #region Scale Operations

    private void ScaleStack()
    {
        for (var i = stack.Count - 1; i >= 0 ; i--) 
            StartCoroutine(CorScaleStack(stack[i], i));
    }

    private IEnumerator CorScaleStack(Clothes cloth, int i)
    {
        if (stack.Count < 1) yield break;
        yield return new WaitForSeconds(0.02f * (stack.Count - i));
        cloth.transform.DOScale(cloth.startScale * 1.5f, 0.05f).SetEase(Ease.OutQuad)
            .OnComplete(() => cloth.transform.DOScale(cloth.startScale, 0.05f)).SetEase(Ease.OutQuad);
    }

    #endregion
}