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
        private void Awake()
        {
            _humanCollider = player.controller.collider_;
            _humanStats = player.controller.stats;
        }
        protected override void OnSwap(bool isAlt)
        {
            player.controller.collider_ = isAlt
                ? tutelCollider
                : _humanCollider;
            
            player.controller.stats = isAlt
                ? tutelStats
                : _humanStats;
        }
    }
}