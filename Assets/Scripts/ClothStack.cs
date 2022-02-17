using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Akali.Common;
using Akali.Scripts.Core;
using DG.Tweening;
using UnityEngine;

public class ClothStack : Singleton<ClothStack>
{
    public List<Clothes> stack = new();
    private List<Vector2> points = new();
    private Collider col;
    [SerializeField] private Collider cutCollider;


    private void SetColliderEnabled() => col.enabled = !col.enabled;

    private void Awake()
    {
        col = gameObject.GetComponent<Collider>();
    }

    private void Update()
    {
        LerpStack(SwerveController.Instance.transform.position.x);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.IsCloth()) AddCloth(other.GetCloth());
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
            SetColliderEnabled();
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
        stack.Remove(cloth);
        Destroy(cloth.gameObject);
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

            //var target = GetRandomPointInBounds(cutCollider.bounds);
            
        }
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
        yield return new WaitForSeconds(0.02f * (stack.Count - i));
        cloth.transform.DOScale(cloth.startScale * 1.5f, 0.05f).SetEase(Ease.OutQuad)
            .OnComplete(() => cloth.transform.DOScale(cloth.startScale, 0.05f)).SetEase(Ease.OutQuad);
    }

    #endregion
}