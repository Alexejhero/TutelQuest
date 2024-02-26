using SchizoQuest.Characters;
using SchizoQuest.Game.Mechanisms;
using UnityEngine;

namespace SchizoQuest.Interaction
{
    public class InteractionTrigger : Trigger<Player>
    {
        public MonoBehaviour interactWith;
        private IInteractable InteractWith => interactWith as IInteractable;
        protected override void OnEnter(Player target)
        {
            switch (InteractWith)
            {
                case ICompoundInteractable compound:
                    if (target.inventory.item && compound.CanCompoundInteract(target, target.inventory.item))
                        compound.CompoundInteract(target, target.inventory.item);
                    break;
                case IInteractable interact:
                    if (interact.CanInteract(target))
                        interact.Interact(target);
                    break;
                default:
                    Debug.LogWarning($"non-interactable target {interactWith} assigned to interaction trigger");
                    enabled = false;
                    break;
            }
        }
    }
}