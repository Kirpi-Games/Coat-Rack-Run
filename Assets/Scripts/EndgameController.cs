using System.Collections.Generic;
using Akali.Common;
using Akali.Scripts.Managers;
using DG.Tweening;
using Strategies.Girl;
using UnityEngine;
using Random = UnityEngine.Random;

public class EndgameController : Singleton<EndgameController>
{
    public bool endgameStart;
    public int confettiCounter;
    public List<ParticleSystem> confettiSystems;
    public List<GameObject> clothes;

    private void Awake()
    {
        ClothStack.Instance.ConsumeStack += Complete;
        for (var i = 0; i < clothes.Count; i++) clothes[i].SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!endgameStart)
        {
            endgameStart = true;
            if (ClothStack.Instance.stack.Count == 0)
            {
                Cashier.Instance.CashierFail();
                AkaliLevelManager.Instance.LevelIsFail();
                return;
            }

            var rand = Random.Range(0, 2);
            if (rand == 0) Cashier.Instance.CashierClap1Anim();
            else Cashier.Instance.CashierClap2Anim();
        }

        if (other.IsClothStack() && other.GetCloth() != null)
        {
            ClothStack.Instance.RemoveEndOfStack(other.GetCloth());
            if (confettiCounter <= 15)
            {
                clothes[confettiCounter].SetActive(true);
                if (Camera.main != null)
                {
                    var t = Camera.main.transform;
                    t.DOKill();
                    t.DOMoveY(t.position.y + 1f, 0.2f).SetEase(Ease.Linear);
                }

                var fold = clothes[confettiCounter].GetComponentsInChildren<ClothFold>();
                for (var i = 0; i < fold.Length; i++)
                {
                    if (fold[i].type != other.GetCloth().activeCloth.type)
                    {
                        fold[i].gameObject.SetActive(false);
                    }
                }

                confettiSystems[confettiCounter].Play();
                confettiCounter++;
            }
            else AkaliLevelManager.Instance.LevelIsCompleted();
        }
    }

    public void Complete(int stackCount)
    {
        if (stackCount == 0) AkaliLevelManager.Instance.LevelIsCompleted();
    }
}