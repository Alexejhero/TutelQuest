using System.Collections;
using SchizoQuest.Game;
using SchizoQuest.Game.Items;
using TarodevController;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public sealed class Player : MonoBehaviour
    {
        public static Player ActivePlayer;

        public PlayerType playerType;
        public Respawnable respawn;
        public PlayerController controller;
        public Inventory inventory;
        public ParticleSystem characterSwitchParticleEffect;
        public Rigidbody2D rb;
        public bool dying;

        private SpriteRenderer[] _renderers;
        private void Awake()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
            respawn.OnReset += (r) =>
            {
                if (this == ActivePlayer)
                {
                    rb.simulated = false;
                    controller.enabled = false;
                    dying = true;
                    EffectsManager.Instance.PlayEffect(EffectsManager.Effects.death, 1f);
                }

                if (this == ActivePlayer) StartCoroutine(Coroutine());

                if (!inventory.item) return;
                Item item = inventory.item;
                inventory.DetachItem();
                Respawnable itemRespawn = item.GetComponent<Respawnable>();
                if (itemRespawn) itemRespawn.Respawn();

                return;

                IEnumerator Coroutine()
                {
                    yield return new WaitForSeconds(1);
                    rb.simulated = true;
                    controller.enabled = true;
                    dying = false;
                }
            };
        }

        public void OnEnable()
        {
            Camera.main!.GetComponent<CameraController>().target = transform;
            ActivePlayer = this;
            controller.movementActive = false;
            StartCoroutine(WaitUntilCameraIsClose());
            SetSortOrder(1);
        }

        public IEnumerator WaitUntilCameraIsClose()
        {
            yield return new WaitUntil(() => CameraController.DistanceToActivePlayer < 50f);
            yield return new WaitUntil(() => CameraController.currVelocity.magnitude < 5f);
            if (ActivePlayer != this) yield break;

            controller.movementActive = true;
            characterSwitchParticleEffect.Play();
        }

        public void OnDisable()
        {
            characterSwitchParticleEffect.Stop();
            characterSwitchParticleEffect.Clear();
            controller.movementActive = false;
            SetSortOrder(-1);
        }

        private void SetSortOrder(int order)
        {
            foreach (var spriteRenderer in _renderers)
                spriteRenderer.sortingOrder = order;
        }
    }
}
