using SchizoQuest.Characters;
using SchizoQuest.Game.Mechanisms;
using UnityEngine;

namespace SchizoQuest.Game
{
    public class HintHider : Trigger<Player>
    {
        public HintType hintType;

        protected override void OnEnter(Player target)
        {
            target.SendMessage("HideHint", hintType, SendMessageOptions.DontRequireReceiver);
        }

        protected override void OnExit(Player target)
        {
        }
    }
}
