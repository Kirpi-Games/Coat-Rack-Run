using System;
using System.Collections.Generic;
using Akali.Common;
using Akali.Scripts.Managers;
using Strategies.Girl;
using UnityEngine;

public class EndgameController : Singleton<EndgameController>
{
    public event Action Sell;

    public bool endgameStart;
    public int confettiCounter;
    public List<ParticleSystem> confettiSystems;

    private void Awake()
    {
        ClothStack.Instance.ConsumeStack += Complete;
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
            

        }
        if (other.IsClothStack())
        {
            ClothStack.Instance.RemoveEndOfStack(other.GetCloth());
            if (confettiCounter <= 15)
            {
                confettiSystems[confettiCounter].Play();
                confettiCounter++;
                return;
            }

            OnSell();
        }
    }

    public void Complete(int stackCount)
    {
        if (stackCount == 0) AkaliLevelManager.Instance.LevelIsCompleted();
    }

    public void OnSell()
    {
        Sell?.Invoke();
    }
}