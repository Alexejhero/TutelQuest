using SchizoQuest.Characters;
using SchizoQuest.Game.Mechanisms;

namespace SchizoQuest.Game.Interaction
{
    public sealed class DoorInteractionTrigger : Trigger<Player>
    {
        public ItemDoor interactWith;
        protected override void OnEnter(Player target)
        {
            if (target.inventory.item && interactWith.CanCompoundInteract(target, target.inventory.item))
                interactWith.CompoundInteract(target, target.inventory.item);
        }
    }
}
