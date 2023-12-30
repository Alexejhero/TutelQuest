using SchizoQuest.Characters;
using SchizoQuest.Game.Items;
using SchizoQuest.Interaction;
using UnityEngine;

namespace SchizoQuest.Game.Mechanisms
{
    public class ItemDoor : MonoBehaviour, ICompoundInteractable<Carryable>
    {
        public bool CanCompoundInteract(Player player, Carryable other)
        {
            return player.inventory.item == other && other.itemType == ItemType.Key;
        }

        public void CompoundInteract(Player player, Carryable other)
        {
            gameObject.SetActive(false);
            Destroy(player.inventory.item.gameObject);
        }
    }
}
