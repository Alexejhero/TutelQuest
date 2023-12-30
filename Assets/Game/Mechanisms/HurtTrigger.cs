using SchizoQuest.Game.Players;

namespace SchizoQuest.Game.Mechanisms
{
    public class HurtTrigger : Trigger<Living>
    {
        public float damage;

        protected override void OnEnter(Living target)
        {
            target.TakeDamage(this, damage);
        }

        protected override void OnExit(Living target)
        {
            
        }
    }
}
