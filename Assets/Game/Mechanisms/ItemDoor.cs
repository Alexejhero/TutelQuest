using SchizoQuest.Characters;
using SchizoQuest.Game.Items;
using SchizoQuest.Interaction;
using UnityEngine;

namespace SchizoQuest.Game.Mechanisms
{
    public class ItemDoor : MonoBehaviour, ICompoundInteractable<Item>
    {
        public bool CanCompoundInteract(Player player, Item other)
        {
            return player.inventory.item == other && other.itemType == ItemType.Key;
        }

        public void CompoundInteract(Player player, Item other)
        {
            gameObject.SetActive(false);
            Destroy(player.inventory.item.gameObject);
        }
    }
}
