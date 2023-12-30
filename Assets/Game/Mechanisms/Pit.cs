using SchizoQuest.Characters;

namespace SchizoQuest.Game.Mechanisms
{
    public class Pit : Trigger<Player>
    {
        protected override void OnEnter(Player target)
        {
            target.respawn.Respawn();
        }

        protected override void OnExit(Player target)
        {
            
        }
    }
}
