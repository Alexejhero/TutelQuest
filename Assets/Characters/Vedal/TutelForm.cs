using System;
using System.Collections;
using SchizoQuest.Characters.Movement;
using SchizoQuest.Game;
using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Characters.Vedal
{
    public sealed class TutelForm : CharacterAltForm
    {
        public Action<bool, bool> OnChange;

        public MovementStats humanStats;
        public MovementStats tutelStats;

        public CapsuleCollider2D swapCollider;

        public PlayerController controller;
        public GroundTracker groundTracker;
        public Rigidbody2D rb;

        private bool _isAlt;
        public override bool IsAlt
        {
            get => _isAlt;
            protected set
            {
                _isAlt = value;
                OnChange?.Invoke(IsAlt, IsDashing);
            }
        }

        private bool _isDashing;
        public bool IsDashing
        {
            get => _isDashing;
            private set
            {
                _isDashing = value;
                OnChange?.Invoke(IsAlt, IsDashing);
            }
        }

        private bool _hintHidden = false;

        private ContactFilter2D _filter;
        private RaycastHit2D[] _raycasts;
        private int _playerLayer;
        private void Awake()
        {
            _playerLayer = gameObject.layer;
            _filter = new ContactFilter2D()
            {
                layerMask = GetCollMask(),
                useLayerMask = true,
                useTriggers = false,
            };
            _raycasts = new RaycastHit2D[1];
            GetComponent<Respawnable>().OnResetFinish += _ =>
            {
                // respawn as vedal
                if (IsAlt) SwapImmediate();
            };
        }

        private int GetCollMask() => Physics2D.GetLayerCollisionMask(_playerLayer) & ~(1 << _playerLayer);

        protected override bool CanSwap(bool toAlt)
        {
            if (player.dying) return false;

            // swap to tutel - only on the ground
            if (toAlt) return groundTracker.IsRecentlyGrounded;
            // swap to human - only when there's space above

            if (IsDashing) return false; // cannot swap while dashing

            // todo remove this garbage and use collision messages instead
            // (make the human collider a trigger while in tutel form)
            int rays = swapCollider.Raycast(Vector2.up, _filter, _raycasts, 0.05f);
            if (rays == 0) return true;

            RaycastHit2D rc = _raycasts[0];
            bool passThrough = Physics2DHelpers.WillPassThroughPlatform(rc.collider.GetComponent<PlatformEffector2D>(), rc.normal, rb);
            if (!passThrough)
                Debug.Log($"Swap to human blocked by {rc.collider}");
            return passThrough;
        }
        protected override void OnSwap()
        {
            if (!_hintHidden)
            {
                _hintHidden = true;
                controller.SendMessage("HideHint", HintType.VedalF, SendMessageOptions.DontRequireReceiver);
            }

            if (IsAlt)
            {
                IsDashing = Mathf.Abs(rb.velocity.x) / controller.stats.maxHorizontalSpeed >= 0.8f;

                controller.stats = tutelStats;
                // neuter the "swap->jump before swap finishes->fly up" issue
                // it's now a super advanced speedrun tech that's totally 100% intended
                if (rb.velocity.y > 0) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.2f);

                if (IsDashing) StartCoroutine(CoDash()); // TODO: prevent swap to neuro
            }
            else
            {
                controller.stats = humanStats;
            }
        }

        private IEnumerator CoDash()
        {
            // "temporary" hack to match previous controller behaviour (no extra gravity while disabled)
            controller.enabled = false;
            rb.gravityScale = 1;

            yield return new WaitUntil(() => Mathf.Abs(rb.velocity.x) < 0.1f);

            controller.enabled = true;
            IsDashing = false;
        }

        private void FixedUpdate()
        {
            // todo do this through an event on evil/neuro swap
            _filter.layerMask = GetCollMask();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CheckAutoswap(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            CheckAutoswap(collision);
        }

        private void CheckAutoswap(Collision2D collision)
        {
            if (IsAlt || IsSwapping) return;
            if (!groundTracker.IsGrounded || controller.IsJumping) return;

            foreach (ContactPoint2D contact in collision.GetContacts())
            {
                if (!contact.enabled) continue;

                Vector2 normal = contact.normal;
                float angle = Vector2.SignedAngle(normal, -transform.up);

                if (Math.Abs(angle) <= 5f)
                {
                    Debug.Log($"Autoswap tutel {collision.collider} contact point {contact.point}");
                    _contactPoint = contact.point;
                    StartCoroutine(PerformSwap());
                    break;
                }
            }
            return; // all contacts disabled or not colliding with a ceiling
        }
        private Vector3 _contactPoint;
#if DEBUG
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_contactPoint, 0.3f);
        }
#endif
    }
}
