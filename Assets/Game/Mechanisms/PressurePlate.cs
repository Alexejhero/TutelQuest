using SchizoQuest.Characters;
using UnityEngine;

namespace SchizoQuest.Game.Mechanisms
{
    public class PressurePlate : Trigger<Player>
    {
        public Switch activates;
        [SerializeField]
        private int playersOnPlate;

        protected override void OnEnter(Player target)
        {
            playersOnPlate++;
            if (playersOnPlate == 1)
                activates.Toggle();
        }

        protected override void OnExit(Player target)
        {
            playersOnPlate--;
            if (playersOnPlate == 0)
                activates.Toggle();
        }
    }
}
