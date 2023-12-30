using SchizoQuest.Game.Items;
using TarodevController;
using UnityEngine;

namespace SchizoQuest.Game.Players
{
    public sealed class Player : MonoBehaviour
    {
        public static Player ActivePlayer;

        public PlayerType playerType;
        public Living living;
        public PlayerController controller;
        public Inventory inventory;
        public ParticleSystem characterSwitchParticleEffect;
        public Vector3 checkpoint;

        private SpriteRenderer[] _renderers;
        private void Awake()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
            checkpoint = transform.position;
            living.OnDeath += OnDeath;
        }

        public void OnEnable()
        {
            Camera.main!.GetComponent<FollowTransform>().target = transform;
            controller.movementActive = true;
            ActivePlayer = this;
            characterSwitchParticleEffect.Play();
            SetSortOrder(1);
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

        private void OnDeath()
        {
            Reset();
        }

        public void Reset()
        {
            transform.position = checkpoint;
            living.ResetHealth();
        }
    }
}
