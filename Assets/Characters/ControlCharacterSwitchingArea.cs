using SchizoQuest.Game.Mechanisms;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public sealed class ControlCharacterSwitchingArea : Trigger<Player>
    {
        private enum Type {
            Disable,
            Enable
        }

        [SerializeField]
        private Type type;
        protected override void OnEnter(Player target)
        {
            CharacterSwitcher switcher = CharacterSwitcher.Instance;
            switcher.enableSwitching = type == Type.Enable;
            if (type == Type.Enable)
            {
                foreach (ControlCharacterSwitchingArea trigger in FindObjectsOfType<ControlCharacterSwitchingArea>())
                    Destroy(trigger);
            }
        }

        protected override void OnExit(Player target)
        {
        }
    }
}