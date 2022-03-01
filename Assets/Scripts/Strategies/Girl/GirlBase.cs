using DG.Tweening;
using UnityEngine;

namespace Strategies.Girl
{
    public enum GirlTypes : byte
    {
        None,
        Naked,
        Dressed,
    }

    public class GirlBase : MonoBehaviour
    {
        [HideInInspector] public GirlTypes type;
        [HideInInspector] public HairId hairId;
        [HideInInspector] public Animator animator;
        
        private static readonly int Cry = Animator.StringToHash("Cry");
        private static readonly int SadWalk = Animator.StringToHash("SadWalk");
        private static readonly int Dance = Animator.StringToHash("Dance");
        private static readonly int HappyIdle = Animator.StringToHash("HappyIdle");
        private static readonly int HappyWalk = Animator.StringToHash("HappyWalk");

        public void Move(Transform t)
        {
            const float degree = 180f;
            var value = t.position.x;

            t.DOMoveX(-value, 2f).OnComplete(() =>
            {
                t.DORotateQuaternion(Quaternion.Euler(0, t.rotation.y + degree, 0), 0.4f)
                    .OnComplete(() => t.DOMoveX(t.position.x, 2f));
            }).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        #region Girl Animations

        public void GirlCryAnim()
        {
            animator.SetTrigger(Cry);
        }

        public void GirlSadWalkAnim()
        {
            animator.SetTrigger(SadWalk);
        }

        public void GirlDanceAnim()
        {
            animator.SetTrigger(Dance);
        }

        public void GirlHappyIdleAnim()
        {
            animator.SetTrigger(HappyIdle);
        }

        public void GirlHappyWalkAnim()
        {
            animator.SetTrigger(HappyWalk);
        }

        #endregion
    }
}