using SchizoQuest.Game.Players;

namespace SchizoQuest.Game.Mechanisms
{
    public class Pit : Trigger<Player>
    {
        protected override void OnEnter(Player target)
        {
            target.Reset();
        }

        protected override void OnExit(Player target)
        {
            
        }
    }
}
