using SchizoQuest.Characters;

namespace SchizoQuest.Game.Interaction
{
    public interface IInteractable
    {
        bool CanInteract(Player player);
        void Interact(Player player);
        void DiscardAfterUse(Player player) { }
    }
}
