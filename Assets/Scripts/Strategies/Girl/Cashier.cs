using System;
using Akali.Common;
using UnityEngine;

namespace Strategies.Girl
{
    public class Cashier : Singleton<Cashier>
    {
        private Animator animator;
        private static readonly int Clap1 = Animator.StringToHash("Clap1");
        private static readonly int Clap2 = Animator.StringToHash("Clap2");
        private static readonly int Fail = Animator.StringToHash("Fail");

        private void Awake()
        {
            animator = gameObject.GetComponent<Animator>();
        }

        public void CashierClap1Anim()
        {
            animator.SetTrigger(Clap1);
        }

        public void CashierClap2Anim()
        {
            animator.SetTrigger(Clap2);   
        }

        public void CashierFail()
        {
            animator.SetTrigger(Fail);
        }
    }
}
