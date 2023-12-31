using SchizoQuest.Game.Mechanisms;

namespace SchizoQuest.Characters
{
    public sealed class ControlCharacterSwitchingArea : Trigger<Player>
    {
        public CharacterSwitcher switcher;
        public bool disablesSwitching;
        protected override void OnEnter(Player target)
        {
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