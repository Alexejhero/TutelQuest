namespace SchizoQuest.Game.Mechanisms
{
    public class Switch : Toggleable
    {
        public Toggleable[] activates;

        public override void Toggle()
        {
            base.Toggle();
            foreach (Toggleable toggleable in activates)
            {
                toggleable.Toggle();
            }
        }
    }
}