using SchizoQuest.Game.Mechanisms;
using SchizoQuest.Helpers;

namespace SchizoQuest.Characters
{
    public sealed class ControlCharacterSwitchingArea : Trigger<Player>
    {
        public bool disablesSwitching;
        protected override void OnEnter(Player target)
        {
            CharacterSwitcher switcher = MonoSingleton<CharacterSwitcher>.Instance;
            switcher.enableSwitching = !disablesSwitching;
            if (!disablesSwitching)
            {
                foreach (var trigger in FindObjectsOfType<ControlCharacterSwitchingArea>())
                    Destroy(trigger);
            }
        }

        protected override void OnExit(Player target)
        {
        }
    }
}