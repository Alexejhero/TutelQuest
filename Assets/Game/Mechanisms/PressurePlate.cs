using SchizoQuest.Characters;

namespace SchizoQuest.Game.Mechanisms
{
    public class PressurePlate : Trigger<Player>
    {
        public Switch activates;
        public TwoState plateObjects;

        protected override void OnEnter(Player target)
        {
            activates.Toggle();
        }

        protected override void OnExit(Player target)
        {
            activates.Toggle();
        }
    }
}
