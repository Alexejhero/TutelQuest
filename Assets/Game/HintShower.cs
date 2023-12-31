using SchizoQuest.Characters;
using SchizoQuest.Game.Mechanisms;
using UnityEngine;

namespace SchizoQuest.Game
{
    public enum HintType
    {
        WASD,
        Space,
        F,
        E,
        C
    }

    public class HintShower : Trigger<Player>
    {
        public HintType hintType;

        protected override void OnEnter(Player target)
        {
            target.SendMessage("ShowHint", hintType, SendMessageOptions.DontRequireReceiver);
        }

        protected override void OnExit(Player target)
        {
        }
    }
}
