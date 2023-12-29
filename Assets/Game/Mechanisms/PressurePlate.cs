using SchizoQuest.Game.Players;
using UnityEngine;
using UnityEngine.Events;

namespace SchizoQuest.Game.Mechanisms
{
    public class PressurePlate : MonoBehaviour
    {
        public bool flipped = false;
        [HideInInspector] public bool isOn = false;
        public UnityEvent<bool> onSwitch;

        public GameObject on;
        public GameObject off;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Player>())
            {
                isOn = !flipped;

                on.SetActive(isOn);
                off.SetActive(!isOn);

                onSwitch?.Invoke(isOn);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<Player>())
            {
                isOn = flipped;

                on.SetActive(isOn);
                off.SetActive(!isOn);

                onSwitch?.Invoke(isOn);
            }
        }
    }
}
