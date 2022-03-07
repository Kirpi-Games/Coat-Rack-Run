using Akali.Scripts.Utilities;
using Akali.Ui_Materials.Scripts.Components;
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

        private void Start()
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
            if (other.IsClothStack() && other.GetCloth() != null)
            {
                if (!other.GetCloth().IsLast)
                {
                    Taptic.Heavy();
                    ClothStack.Instance.CutStack(other.GetCloth().id);
                    return;
                }

                col.enabled = false;
                Taptic.Medium();
                SetActiveDressed(other.GetCloth().activeCloth.type);
                ClothStack.Instance.RemoveEndOfStack(other.GetCloth());
                MoneyText.Instance.IncreaseMoney(GetAmountForGirlType(other.GetCloth().activeCloth.type));
            }
        }

        public static int GetAmountForGirlType(ClothTypes type)
        {
            return type switch
            {
                ClothTypes.Pyjama => 50,
                ClothTypes.Casual => 75,
                ClothTypes.Business => 100,
                ClothTypes.Prom => 150,
                _ => 50,
            };
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