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

        public void Move(Transform t)
        {
            var value = t.position.x;
            t.DOMoveX(-value, 1f).OnComplete(() => t.DOMoveX(t.position.x, 1f)).SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}