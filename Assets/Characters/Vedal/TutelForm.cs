using TarodevController;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

namespace SchizoQuest.Characters.Vedal
{
    public sealed class TutelForm : CharacterAltForm
    {
        public CapsuleCollider2D tutelCollider;
        public CapsuleCollider2D swapCollider;
        private CapsuleCollider2D _humanCollider;
        public ScriptableStats tutelStats;
        private ScriptableStats _humanStats;

        public Transform itemSlot;

        public float vedalItemYPos = 2;
        public float tutelItemYPos = -0.1f;

        private bool _grounded;
        private int _collMask;
        private ContactFilter2D _filter;
        private RaycastHit2D[] _raycasts;
        private void Awake()
        {
            _humanCollider = player.controller.collider_;
            _humanStats = player.controller.stats;
            player.controller.GroundedChanged += (isGrounded, _)
                => _grounded = isGrounded;
            _collMask = Physics2D.GetLayerCollisionMask(gameObject.layer) & ~(1 << gameObject.layer);
            _filter = new ContactFilter2D()
            {
                layerMask = _collMask,
                useLayerMask = true,
                useTriggers = false,
            };
            _raycasts = new RaycastHit2D[1];
        }

        protected override bool CanSwap(bool toAlt)
        {
            // swap to tutel - only on the ground
            if (toAlt) return _grounded;
            // swap to human - only when there's space above
            
            int rays = swapCollider.Raycast(Vector2.up, _filter, _raycasts, 0.05f);
            Debug.Log($"Swap human raycast {rays} {_raycasts[0].collider}");
            return rays == 0;
        }
        protected override void OnSwap()
        {
            if (isAlt)
            {
                player.controller.collider_ = tutelCollider;
                player.controller.stats = tutelStats;
                itemSlot.localPosition = new Vector3(0, tutelItemYPos, 0);
            }
            else
            {
                player.controller.collider_ = _humanCollider;
                player.controller.stats = _humanStats;
                itemSlot.localPosition = new Vector3(0, vedalItemYPos, 0);
            }
        }

        public void Update()
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
