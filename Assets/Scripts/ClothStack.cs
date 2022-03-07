using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Akali.Common;
using Akali.Scripts.Core;
using Akali.Scripts.Utilities;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClothStack : Singleton<ClothStack>
{
    public event Action<int> ConsumeStack;
    
    public List<Clothes> stack = new();
    private readonly List<Vector2> points = new();
    private Collider col;
    [SerializeField] private Collider cutCollider;

    private void InvokeSetColliderEnabled() => Invoke(nameof(SetColliderEnabled), 0.2f);
    
    private void SetColliderEnabled() => col.enabled = stack.Count < 1;

    private void Awake()
    {
        gameObject.layer = Constants.LayerClothStack;
        col = gameObject.GetComponent<Collider>();
    }

    private void Update()
    {
        LerpStack(SwerveController.Instance.transform.position.x);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.IsCloth())
        {
            Taptic.Medium();
            AddCloth(other.GetCloth());
        }
    }

    private void LerpStack(float x)
    {
        if (stack.Count < 1)
        {
            var localPos0 = transform.localPosition;
            localPos0.x = x;
            transform.localPosition = localPos0;
            return;
        }

        transform.localPosition = new Vector3(0,0,2);
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

    private void SetClothId()
    {
        for (var i = 0; i < stack.Count; i++) stack[i].id = i;
    }

    #region List Operations

    public void AddCloth(Clothes cloth)
    {
        if (stack.Count < 1)
        {
            cloth.transform.parent = transform;
            cloth.transform.position = transform.position;
            cloth.SetLayer();
            InvokeSetColliderEnabled();
            stack.Add(cloth);
            SetClothId();
            ScaleStack();
            return;
        }

        SetEndOfStack(cloth);
        cloth.SetLayer();
        stack.Add(cloth);
        SetClothId();
        ScaleStack();
    }

    private void SetEndOfStack(Clothes cloth)
    {
        cloth.transform.parent = transform;
        var last = stack[stack.Count - 1].transform;
        var position = last.position;
        position.z += cloth.col.GetLength();
        cloth.transform.position = position;
    }

    public void RemoveEndOfStack(Clothes cloth)
    {
        if (cloth != null)
        {
            stack.Remove(cloth);
            Destroy(cloth.gameObject);
            InvokeSetColliderEnabled();
        }

        if (EndgameController.Instance.endgameStart) OnConsumeStack(stack.Count);
    }

    public void CutStack(int id)
    {
        if (id == -1) return;
        var deleted = stack.Where(t => t.id >= id).ToList();

        SetVirtualPoints(deleted.Count);
        for (var i = 0; i < deleted.Count; i++)
        {
            var cloth = deleted[i];
            stack.Remove(cloth);
            cloth.id = -1;
            cloth.SetLayer();

            const float totalTime = 0.5f;
            var target = GetRandomPointInBounds(cutCollider.bounds);
            var distance = target - cloth.transform.position;
            var targetX = target.x;
            var targetZ = target.z;
            var firstY = distance.magnitude * 0.35f;
            var secondY = 0.5f;

            var seq = DOTween.Sequence();
            seq.Join(cloth.transform.DOMoveX(targetX, totalTime).SetEase(Ease.InOutSine));
            seq.Join(cloth.transform.DOMoveZ(targetZ, totalTime).SetEase(Ease.InOutSine));
            seq.Join(cloth.transform.DOMoveY(firstY, totalTime * 0.5f).SetEase(Ease.InOutSine));
            seq.Join(cloth.transform.DOMoveY(secondY, totalTime * 0.5f).SetEase(Ease.InOutSine)
                .SetDelay(totalTime * 0.5f));
            seq.OnComplete((() =>
            {
                cloth.transform.position = target;
                cloth.transform.parent = MovementZ.Instance.transform;
            }));
        }

        InvokeSetColliderEnabled();
    }

    private void SetVirtualPoints(int count)
    {
        points.Clear();
        var sqrt = Mathf.Sqrt(count);
        var minX = (int) cutCollider.bounds.min.x;
        var maxX = (int) cutCollider.bounds.max.x;
        var minZ = (int) cutCollider.bounds.min.z;
        var maxZ = (int) cutCollider.bounds.max.z;
        var areaX = (cutCollider.bounds.size.x) / sqrt;
        var areaZ = (cutCollider.bounds.size.z) / sqrt;
        for (float i = minX; i < maxX; i += areaX)
        {
            for (float j = minZ; j < maxZ; j += areaZ)
            {
                var randX = Random.Range(0, areaX / 4);
                var randZ = Random.Range(0, areaZ / 4);
                points.Add(new Vector2(i + randX, j + randZ));
            }
        }
    }

    private Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        var range = Random.Range(0, points.Count);
        var point = points[range];
        points.RemoveAt(range);
        return new Vector3(point.x,
            0.5f,
            point.y
        );
    }

    #endregion

    #region Scale Operations

    private void ScaleStack()
    {
        for (var i = stack.Count - 1; i >= 0; i--)
            StartCoroutine(CorScaleStack(stack[i], i));
    }

    private IEnumerator CorScaleStack(Clothes cloth, int i)
    {
        if (stack.Count < 1) yield break;
        yield return new WaitForSeconds(0.04f * (stack.Count - i));

        cloth.transform.DOScale(cloth.startScale * 1.2f, 0.04f).SetEase(Ease.OutQuad)
            .OnComplete(() => cloth.transform.DOScale(cloth.startScale, 0.04f)).SetEase(Ease.OutQuad);
    }

    #endregion

    public void OnConsumeStack(int stackCount)
    {
        for (var i = 0; i < stack.Count; i++) 
            stack[i].transform.localScale = stack[i].startScale;
        StopAllCoroutines();
        ConsumeStack?.Invoke(stackCount);
    }
}