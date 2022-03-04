using Akali.Scripts.Utilities;
using UnityEngine;

namespace Strategies.Girl
{
    public enum NakedState : byte
    {
        Fixed,
        Moved,
    }

    public class GirlController : MonoBehaviour
    {
        public ParticleSystem starProof;
        public HairId hairId;
        private NakedState state;
        private Collider col;
        private GirlNaked naked;
        private GirlDressed dressed;

        private void Awake()
        {
            col = gameObject.GetComponent<Collider>();
            naked = gameObject.GetComponentInChildren<GirlNaked>();
            dressed = gameObject.GetComponentInChildren<GirlDressed>();
            naked.hairId = hairId;
            dressed.hairId = hairId;
            SetActiveNaked();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.IsClothStack())
            {
                col.enabled = false;
                SetActiveDressed(other.GetCloth().activeCloth.type);

                if (other.GetCloth().IsLast)
                {
                    Taptic.Light();
                    ClothStack.Instance.RemoveEndOfStack(other.GetCloth());
                    return;
                }

                Taptic.Heavy();
                ClothStack.Instance.CutStack(other.GetCloth().id);
            }
        }

        private void SetActiveNaked()
        {
            dressed.gameObject.SetActive(false);
            naked.SetHair();
            if (gameObject.layer == Constants.LayerMoved)
                state = NakedState.Moved;

            if (state == NakedState.Moved)
            {
                naked.Move(transform);
                naked.GirlSadWalkAnim();
                return;
            }

            SetGirlAnim(true);
        }

        public void SetActiveDressed(ClothTypes type)
        {
            naked.gameObject.SetActive(false);
            dressed.gameObject.SetActive(true);
            dressed.SetCloth(type);
            dressed.SetHair();
            starProof.transform.SetParent(MovementZ.Instance.transform);
            starProof.Play();
            SetGirlAnim();
        }

        public void SetGirlAnim(bool start = false)
        {
            if (start)
            {
                var rand = Random.Range(0, 2);
                if (rand == 0) naked.GirlCryAnim();
                return;
            }

            if (state == NakedState.Fixed)
            {
                var rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    dressed.GirlDanceAnim();
                    return;
                }

                dressed.GirlHappyIdleAnim();
                return;
            }

            dressed.GirlHappyWalkAnim();
        }
    }
}