using TarodevController;
using UnityEngine;

namespace SchizoQuest.Game.Players
{
    public sealed class TutelForm : CharacterAltForm
    {
        public CapsuleCollider2D tutelCollider;
        private CapsuleCollider2D _humanCollider;
        private void Awake()
        {
            _humanCollider = GetComponent<PlayerController>().collider_;
        }
        protected override void OnSwap(bool isAlt)
        {
            player.controller.collider_ = isAlt
                ? tutelCollider
                : _humanCollider;
            // bruh moment in the controller
            if (isAlt)
            {
                //player.controller
            }
        }
    }
}