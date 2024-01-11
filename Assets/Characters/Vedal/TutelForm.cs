using System.Collections;
using SchizoQuest.Characters.Movement;
using SchizoQuest.Game;
using UnityEngine;

namespace SchizoQuest.Characters.Vedal
{
    public sealed class TutelForm : CharacterAltForm
    {
        public MovementStats humanStats;
        public MovementStats tutelStats;

        public CapsuleCollider2D swapCollider;

        public Transform itemSlot;

        public float vedalItemYPos = 2;
        public float tutelItemYPos = -0.1f;

        public PlayerController2 controller;
        public GroundTracker groundTracker;
        public Rigidbody2D rb;

        public bool IsDashing { get; private set; }

        private bool _hintHidden = false;

        private ContactFilter2D _filter;
        private RaycastHit2D[] _raycasts;
        private void Awake()
        {
            int collMask = Physics2D.GetLayerCollisionMask(gameObject.layer) & ~(1 << gameObject.layer);
            _filter = new ContactFilter2D()
            {
                layerMask = collMask,
                useLayerMask = true,
                useTriggers = false,
            };
            _raycasts = new RaycastHit2D[1];
            GetComponent<Respawnable>().OnReset += (r) => {
                // respawn as vedal
                if (isAlt) SwapImmediate();
            };
        }

        protected override bool CanSwap(bool toAlt)
        {
            if (player.dying) return false;

            // swap to tutel - only on the ground
            if (toAlt) return groundTracker.isOnGround;
            // swap to human - only when there's space above

            if (IsDashing) return false; // cannot swap while dashing

            int rays = swapCollider.Raycast(Vector2.up, _filter, _raycasts, 0.05f);
            Debug.Log($"Swap human raycast {rays} {_raycasts[0].collider}");
            return rays == 0;
        }
        protected override void OnSwap()
        {
            if (!_hintHidden)
            {
                _hintHidden = true;
                controller.SendMessage("HideHint", HintType.F, SendMessageOptions.DontRequireReceiver);
            }

            if (isAlt)
            {
                IsDashing = Mathf.Abs(rb.velocity.x) / controller.stats.maxHorizontalSpeed >= 0.8f;

                controller.stats = tutelStats;
                itemSlot.localPosition = new Vector3(0, tutelItemYPos, 0);

                if (IsDashing) StartCoroutine(CoDash()); // TODO: prevent swap to neuro
            }
            else
            {
                controller.stats = humanStats;
                itemSlot.localPosition = new Vector3(0, vedalItemYPos, 0);
            }
        }

        private IEnumerator CoDash()
        {
            controller.canMove = controller.canJump = false;

            yield return new WaitUntil(() => Mathf.Abs(rb.velocity.x) < 0.1f);

            controller.canMove = controller.canJump = true;
            IsDashing = false;
        }

        private void Update()
        {
            if (isAlt) return;

            int rays = swapCollider.Raycast(Vector2.up, _filter, _raycasts, 0.05f);
            if (rays > 0)
            {
                Debug.Log($"Autoswap tutel raycast {_raycasts[0].collider}");
                StartCoroutine(PerformSwap());
            }
        }
    }
}
