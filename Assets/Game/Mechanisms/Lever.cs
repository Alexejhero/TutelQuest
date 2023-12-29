using SchizoQuest.Game.Players;
using SchizoQuest.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace SchizoQuest.Game.Mechanisms
{
    public class Lever : MonoBehaviour, IInteractable
    {
        public bool isOn = false;
        public UnityEvent<bool> onSwitch;

        public GameObject on;
        public GameObject off;

        public bool CanInteract(Player player)
        {
            Debug.Log(player.inventory);
            Debug.Log(player.inventory.item);
            return !player.inventory || !player.inventory.item;
        }

        public void Interact(Player player)
        {
            isOn = !isOn;
            on.SetActive(isOn);
            off.SetActive(!isOn);

            onSwitch?.Invoke(isOn);
        }
    }
}
