using System.Collections;
using FMODUnity;
using SchizoQuest.Characters.Movement;
using SchizoQuest.Characters.Vedal;
using SchizoQuest.Game;
using SchizoQuest.Game.Items;
using SchizoQuest.VFX.Transition;
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
        [HideInInspector]
        public bool dying;
        public StudioEventEmitter deathSound;

        // Scuffed but who cares, this is basically only used to communicate with CharacterAnimator
        public bool Winning { get; set; }
        public bool Locked => GetComponent<TutelForm>()?.IsDashing ?? false;

        public bool IsGrounded => controller.groundTracker.IsGrounded;
        public bool IsRecentlyGrounded => controller.groundTracker.IsRecentlyGrounded;

        private SpriteRenderer[] _renderers;
        private void Awake()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
            respawn.OnResetBegin += (r) =>
            {
                if (this != ActivePlayer) return;

                PauseMenuBehaviour.Instance.Close();
                PauseMenuBehaviour.Instance.canToggle = false;
                rb.simulated = false;
                controller.enabled = false;
                dying = true;
                if (deathSound) deathSound.Play();
                EffectsManager.Instance.PlayEffect(EffectsManager.Effects.death, 1f);
            };
            respawn.OnResetFinish += (r) =>
            {
                if (inventory.item)
                {
                    Item item = inventory.item;
                    inventory.DetachItem();
                    Respawnable itemRespawn = item.GetComponent<Respawnable>();
                    if (itemRespawn) itemRespawn.Respawn();
                }

                if (this != ActivePlayer) return;

                PauseMenuBehaviour.Instance.canToggle = true;
                rb.simulated = true;
                controller.enabled = true;
                dying = false;
            };
        }

        public void OnEnable()
        {
            Camera.main!.GetComponent<CameraController>().target = transform;
            ActivePlayer = this;
            ToggleMovement(false);
            StartCoroutine(WaitUntilCameraIsClose());
            SetSortOrder(1);
        }

        public IEnumerator WaitUntilCameraIsClose()
        {
            yield return new WaitUntil(() => CameraController.DistanceToActivePlayer < 50f);
            yield return new WaitUntil(() => CameraController.currVelocity.magnitude < 25f);
            if (ActivePlayer != this) yield break;

            ToggleMovement(true);
            characterSwitchParticleEffect.Play();
        }

        public void OnDisable()
        {
            characterSwitchParticleEffect.Stop();
            characterSwitchParticleEffect.Clear();
            ToggleMovement(false);
            SetSortOrder(-1);
        }

        private void SetSortOrder(int order)
        {
            foreach (SpriteRenderer spriteRenderer in _renderers)
                spriteRenderer.sortingOrder = order;
        }

        public void ToggleMovement(bool active)
        {
            controller.canMove = active;
            controller.canJump = active;
        }
    }
}
