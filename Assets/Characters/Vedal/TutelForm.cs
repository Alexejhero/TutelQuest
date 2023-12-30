using TarodevController;
using UnityEngine;

namespace SchizoQuest.Characters.Vedal
{
    public sealed class TutelForm : CharacterAltForm
    {
        public CapsuleCollider2D tutelCollider;
        private CapsuleCollider2D _humanCollider;
        public ScriptableStats tutelStats;
        private ScriptableStats _humanStats;

        public Transform itemSlot;

        public float vedalItemYPos = 2;
        public float tutelItemYPos = -0.1f;

        private bool _grounded;
        private void Awake()
        {
            _humanCollider = player.controller.collider_;
            _humanStats = player.controller.stats;
            player.controller.GroundedChanged += (isGrounded, _)
                => _grounded = isGrounded;
        }
        protected override bool CanSwap(bool toAlt)
        {
            // swap to tutel - only on the ground
            if (toAlt) return _grounded;
            // swap to human - only when there's space above
            RaycastHit2D rc = Physics2D.CapsuleCast(_humanCollider.bounds.center, _humanCollider.size, _humanCollider.direction, 0, Vector2.up, 0.05f, ~LayerMask.GetMask("Player"));
            Debug.Log($"Swap human raycast {rc.collider}");
            return !rc.collider;
        }
        protected override void OnSwap(bool isAlt)
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
    }
}
