using SchizoQuest.Game.Players;

namespace SchizoQuest.Interaction
{
    public interface IInteractable
    {
        bool CanInteract(Player player);
        void Interact(Player player);

        bool CanCompoundInteract(Player player, IInteractable interactable);
        void CompoundInteract(Player player, IInteractable interactable);
    }
}
