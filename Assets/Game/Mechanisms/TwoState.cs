using UnityEngine;

namespace SchizoQuest.Game.Mechanisms
{
    public class TwoState : Toggleable
    {
        public GameObject on;
        public GameObject off;

        public override void Toggle()
        {
            base.Toggle();
            on.SetActive(isOn);
            off.SetActive(!isOn);
        }

    }
}