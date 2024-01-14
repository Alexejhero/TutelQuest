using SchizoQuest.Characters;

namespace SchizoQuest.Interaction
{
    public interface IInteractable
    {
        bool CanInteract(Player player);
        void Interact(Player player);
        void DiscardAfterUse(Player player) { }
    }
}
