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

        private SpriteRenderer[] _renderers;
        private void Awake()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
            respawn.OnReset += (r) =>
            {
                if (this == ActivePlayer)
                    EffectsManager.Instance.PlayEffect(EffectsManager.Effects.death, 0.3f);
                if (!inventory.item) return;
                Item item = inventory.item;
                inventory.DetachItem();
                Respawnable itemRespawn = item.GetComponent<Respawnable>();
                if (itemRespawn) itemRespawn.Respawn();
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
            yield return new WaitUntil(() => CameraController.currVelocity.magnitude < 1f);
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
