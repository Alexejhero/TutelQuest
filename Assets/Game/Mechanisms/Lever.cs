using SchizoQuest.Characters;
using SchizoQuest.Interaction;
using UnityEngine;

namespace SchizoQuest.Game.Mechanisms
{
    public class Lever : Switch, IInteractable
    {
        public bool CanInteract(Player player)
        {
            Debug.Log(player.inventory);
            Debug.Log(player.inventory.item);
            return !player.inventory || !player.inventory.item;
        }

        public void Interact(Player player)
        {
            Toggle();
        }
    }
}
