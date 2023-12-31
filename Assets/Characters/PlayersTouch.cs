using SchizoQuest.Game.Mechanisms;

namespace SchizoQuest.Characters
{
    public class PlayersTouch : Trigger<Player>
    {
        protected override void OnEnter(Player target)
        {
            EffectsManager.Instance.PlayEffect(EffectsManager.Effects.gameFinish, 2f);
        }

        protected override void OnExit(Player target)
        {
        }
    }
}
