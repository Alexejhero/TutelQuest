using SchizoQuest.Characters;
using UnityEngine;
using UnityEngine.Events;

namespace SchizoQuest.Game.Mechanisms
{
    public class PressurePlate : Trigger<Player>
    {
        public bool flipped = false;
        [HideInInspector] public bool isOn = false;
        public UnityEvent<bool> onSwitch;

        public GameObject on;
        public GameObject off;

        protected override void OnEnter(Player target)
        {
            isOn = !flipped;
            OnFlip();
        }

        protected override void OnExit(Player target)
        {
            isOn = flipped;
            OnFlip();
        }

        private void OnFlip()
        {
            on.SetActive(isOn);
            off.SetActive(!isOn);

            onSwitch?.Invoke(isOn);
        }
    }
}
